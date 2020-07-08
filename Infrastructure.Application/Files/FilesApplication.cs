using Infrastructure.Domain.Files;
using Utils.IO.Files;
using SeedWork;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Infrastructure.Domain;
using System.Reflection;

namespace Infrastructure.Application.Files
{

    /// <summary>
    /// Clase aplicacion para el manejo de archivos. Contiene la logica necesaria para la administracion de los archivos
    /// de las entidades registradas. Usa diferentes clases para la administracion de ellos: un FileManager y un repositorio de archivos.
    /// </summary>
    public class FilesApplication : IFilesApplication
    {

        /// <summary>
        /// Administrador de archivos
        /// </summary>
        private IFilesManager filesManager;



        private Dictionary<string, string> EntitiesUrlPaths;


        /// <summary>
        /// Repositorio para los archivos
        /// </summary>
        private SeedWork.IRepository<Infrastructure.Domain.Files.File> filesRepository;


        /// <summary>
        /// Repositorio para las entidades de configuracion
        /// </summary>
        private SeedWork.IRepository<Infrastructure.Domain.Files.EntityConfiguration> entityConfigurationRepository;


        /// <summary>
        /// Unidad de trabajo de la aplicacion de archivos
        /// </summary>
        private IInfrastructureUnitOfWork unitOfWork;


        /// <summary>
        /// 
        /// </summary>
        private bool isReady;


        /// <summary>
        /// 
        /// </summary>
        public bool IsReady { get { return isReady; } }


        private string protocol;

        private string domain;

        private int port;

        private string urlPath;

        /// <summary>
        /// Diccionario de archivos que son guardados temporalmente en memoria para que puedan ser descargados. Una vez descargados
        /// se eliminan del diccionario
        /// </summary>
        private static IDictionary<string, Stream> inMemoryStreams = new Dictionary<string, Stream>();


        /// <summary>
        /// 
        /// </summary>
        private static IDictionary<string, FileDto> inMemoryFiles = new Dictionary<string, FileDto>();
        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filesManager"></param>
        public FilesApplication(IFilesManager filesManager, IInfrastructureUnitOfWork unitOfWork)
        {

            this.filesManager = filesManager;

            this.unitOfWork = unitOfWork;

            this.filesRepository = unitOfWork.FilesRepository;

            this.entityConfigurationRepository = unitOfWork.EntitiesConfigurationsRepository;

            isReady = false;

        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityCodeName"></param>
        /// <returns></returns>
        private async Task<Domain.Files.EntityConfiguration> GetEntityConfiguration(string entityCodeName)
        {

            //Se obtiene la configuracion desde el manejador de archivos
            Utils.IO.Files.EntityConfiguration entityConfigurationFromFileManager = filesManager.GetEntityConfiguration(entityCodeName);


            //Se obtiene la configuracion desde la base de datos
            EntityCodeNameSpecification entityCodeNameSpecification = new EntityCodeNameSpecification(entityCodeName);
            Domain.Files.EntityConfiguration entityConfigurationFromDatabase = (await entityConfigurationRepository.GetBySpecAsync(entityCodeNameSpecification)).FirstOrDefault();

            if (entityConfigurationFromFileManager != null && entityConfigurationFromDatabase != null)
            {
                throw new Exception("FILES_APPLICATION_BAD_CONFIGURATION");
            }
            else if (entityConfigurationFromFileManager != null && entityConfigurationFromDatabase == null)
            {
                return new Domain.Files.EntityConfiguration() {Server = entityConfigurationFromFileManager.Server, Path = entityConfigurationFromFileManager.Path, FolderCreationExpression = entityConfigurationFromFileManager.FolderExpressionExpression };
            }
            else if (entityConfigurationFromFileManager == null && entityConfigurationFromDatabase != null)
            {
                return entityConfigurationFromDatabase;
            }
            else
            {
                return null;
            }

        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityCodeName"></param>
        /// <param name="streams"></param>
        public async Task<List<FileDto>> SaveFiles(string entityCodeName, List<FileCreation> filesCreations)
        {
            try
            {
                if (IsReady)
                {
                    string path = null;
                    string pathWithNameAndExtension = null;
                    int? entityConfigurationId = null;
                    string folderPath = null;

                    //Si el nombre de la entidad es diferente de nulo entonces se busca la configuracion de la entidad
                    //esta incluye el nombre del servidor
                    //ruta completa
                    //expresion para crear folder
                    if (entityCodeName != null)
                    {
                        Domain.Files.EntityConfiguration entityConfiguration = await GetEntityConfiguration(entityCodeName);
                        if (entityConfiguration != null)
                        {
                            
                            folderPath = filesManager.CalculateFolderName(entityConfiguration.FolderCreationExpression);
                            string[] paths = null;
                            if (entityConfiguration.IsRelativePath)
                            {
                                string currentDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                                paths = new string[] { currentDirectory, entityConfiguration.Path, folderPath };
                            }
                            else
                            {
                                paths = new string[] { entityConfiguration.Server, entityConfiguration.Path, folderPath };
                            }

                            path = Path.Combine(paths);
                            entityConfigurationId = entityConfiguration.Id;
                        }
                        else //No se ha configurado en donde se van a guardar los archivos para este tipo de entidad
                        {
                            throw new Exception("ENTITY_NOT_CONFIGURED");
                        }
                    }



                    List<FileDto> filesDtos = new List<FileDto>();
                    foreach (FileCreation fileCreation in filesCreations)
                    {

                        //Se guarda la informacion del archivo en la base de datos
                        string fileExtension = Path.GetExtension(fileCreation.NameWithExtension);
                        string name = Path.GetFileNameWithoutExtension(fileCreation.NameWithExtension);
                        string currentDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                        string[] paths = new string[] { currentDirectory, "genericFiles" };
                        path = Path.Combine(paths);
                        Domain.Files.File file = new Domain.Files.File() { EntityConfigurationId = entityConfigurationId, Extension = fileExtension, IsVirtual = false, Name = name, CreationDate = DateTime.Now, Path = path };

                        this.filesRepository.Add(file);

                        //Unidad de trabajo completada, se inicia el proceso de guardado en la base de datos
                        //Se guarda por cada interacion en los archivos para obtener el uid de cada informacion de archivo guardada, esto facilita asociar rapidamente los objetos  //fileEntityDescriptor(FileEntityDescriptor) y file(Domain.Files.File)
                        int result = await unitOfWork.CompleteAsync();


                        if (file.Id != null)
                        {
                            
                            //Si el directorio no existe entonces se crea
                            if (!Directory.Exists(path))
                            {
                                Directory.CreateDirectory(path);
                            }

                            //Se obtiene la ruta completa con nombre de archivo y extension
                            //Como nombre del archivo se toma el UID guardado en la base de datos
                            pathWithNameAndExtension = Path.Combine(path, string.Concat(file.Id.ToString(), fileExtension));

                            //Se guarda el archivo en el sistema de archivos
                            FileEntityDescriptor fileEntityDescriptor = await filesManager.SaveFile(pathWithNameAndExtension, folderPath, fileCreation.Stream, true);


                            filesDtos.Add(new FileDto(GetFileUrl(file.Id.ToString()), fileEntityDescriptor.NameWithExtension) { EntityCodeName = entityCodeName });
                        }
                        else
                        {
                            //Log Error al guarda la informacion el archivo 
                        }

                    }


                    return filesDtos;
                }

                throw new Exception("FILES_APPLICATION_NOT_CONFIGURED");
            }
            catch (DirectoryNotFoundException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        private FileDto FindFileInMemory(Guid guid)
        {
            Stream stream;
            inMemoryStreams.TryGetValue(guid.ToString(), out stream);
            FileDto fileDto;
            inMemoryFiles.TryGetValue(guid.ToString(), out fileDto);
            if (stream != null && fileDto != null)
            {
                fileDto.Stream = stream;
                return fileDto;
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="entityCodeName"></param>
        /// <returns></returns>
        public async Task<FileDto> GetFileAsync(string id)
        {
            try
            {

                Guid guid;

                if (Guid.TryParse(id, out guid))
                {
                    string key = inMemoryFiles.Keys.FirstOrDefault(searchKey => searchKey == id);
                    if (key != null)
                    {
                        return FindFileInMemory(guid);
                    }
                    else
                    {
                        //Se obtiene los metados del archivo de acuerdo a su id
                        List<Domain.Files.File> files = (await filesRepository.GetBySpecAsync(new FindByUidSpecification(guid))).ToList();
                        if (files != null && files.Count > 0)
                        {
                            //Se obtiene el archivo
                            Domain.Files.File file = files[0];

                            string fullPathWithNameAndExtension = Path.Combine(file.Path, string.Concat(file.Id.ToString(), file.Extension));

                            FileEntityDescriptor fileEntityDescriptor = filesManager.GetFile(fullPathWithNameAndExtension);

                            if (fileEntityDescriptor != null)
                            {
                                string urlPath = GetFileUrl(file.Id.ToString());
                                return new FileDto(urlPath, file.NameWithExtension) { Stream = fileEntityDescriptor.Stream };
                            }
                        }
                        else
                        {
                            return null;
                        }
                    }
                   
                    return null;
                }
                
                return null;
            }
            catch(Exception ex)
            {
                throw ex;
            }

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public string GetFileUrl(string uid)
        {

            string urlPathLocal = urlPath.EndsWith("/") ? urlPath : string.Concat(urlPath, "/");
            return string.Concat(protocol, "://", domain, ":", port.ToString(), "/", urlPathLocal, uid);
            
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="domain"></param>
        /// <param name="port"></param>
        /// <param name="urlPath"></param>
        public void Config(string protocol, string domain, int port, string urlPath)
        {
            this.protocol   = protocol;
            this.domain     = domain;
            this.port       = port;
            this.urlPath    = urlPath;
            isReady = true;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="filesCreations"></param>
        /// <returns></returns>
        public Task<List<FileDto>> SaveFilesInMemory(List<FileCreation> filesCreations)
        {
            List<FileDto> filesDtos = new List<FileDto>();
            foreach (FileCreation fileCreation in filesCreations)
            {
                string[] fileNameAndExtension = fileCreation.NameWithExtension.Split(new char[]{'.'});
                Guid id = Guid.NewGuid();

                Domain.Files.File file = new Domain.Files.File()
                {
                    CreationDate = DateTime.Now,
                    EntityConfiguration = null,
                    EntityConfigurationId = null,
                    Extension = fileNameAndExtension[1],
                    Id = id,
                    IsVirtual = false,
                    Name = fileNameAndExtension[0],
                    ParentId = null,
                    Path = null
                };

                //Se guarda el stream en memoria
                inMemoryStreams.Add(id.ToString(), fileCreation.Stream);
                FileDto fileDto = new FileDto(GetFileUrl(id.ToString()), fileCreation.NameWithExtension, fileCreation.ContentType);
                inMemoryFiles.Add(id.ToString(), fileDto);
                
                filesDtos.Add(fileDto);
            }

            return Task.FromResult(filesDtos);
        }
    }



    public class FileCreation
    {

        public Stream Stream { get; set; }


        /// <summary>
        /// 
        /// </summary>
        private string nameWithExtension;


        /// <summary>
        /// 
        /// </summary>
        public string NameWithExtension
        {
            get { return nameWithExtension; }

            set
            {
                if (Validate(value))
                {
                    nameWithExtension = value;
                }
               
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private bool Validate(string name)
        {
            if (!name.Contains(".exe") && !name.Contains(".dll") && !name.Contains(".cmd")) return true;

            return false;
        }

        
        /// <summary>
        /// Tipo de contenido. Generalmente se usa cuando se quiere especificar que tipo de archivo para cuando se requiere descargarlo
        /// </summary>
        public string ContentType { get; set; }
    }



    /// <summary>
    /// Esta clase es un objeto de transferencia de datos que se usa para:
    /// Especificar la creacion de un archivo que sera administrador por la api.
    /// Especificar como se debe de crear el objeto FileStreamResult cuando se requiera descargar el archivo creado
    /// </summary>
    public class FileDto
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fullName"></param>
        /// <param name="urlPath"></param>
        public FileDto(string url, string fullName)
        {
            FullName        = fullName;
            Url = url;
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fullName"></param>
        /// <param name="urlPath"></param>
        public FileDto(string url, string fullName, string contentType)
        {
            FullName = fullName;
            Url = url;
            ContentType = contentType;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="fullName"></param>
        /// <param name="urlPath"></param>
        public FileDto(string url, string fullName, Stream stream, string contentType)
        {
            FullName = fullName;
            Url = url;
            ContentType = contentType;
            Stream = stream;
        }

        /// <summary>
        /// Nombre completo del archivo
        /// </summary>
        public string FullName { get; private set; }


        /// <summary>
        /// En caso de que el archivo pertenezca a una entidad de negocio, (solicitud, ticket, incidente) aqui se
        /// especifica el nombre de la entidad, pues en caso de que la api las administre sepa como administrarlas
        /// </summary>
        public string EntityCodeName { get; set; }

        /// <summary>
        /// En caso de que el archivo sea administrado por la api, aqui se guarda su direccion url unica
        /// </summary>
        public string Url { get; set; }

        
        /// <summary>
        /// Stream (flujo) del contenido del archivo
        /// </summary>
        public Stream Stream { get; set; }


        /// <summary>
        /// Tipo de contenido del archivo
        /// </summary>
        public string ContentType { get; set; }


        /// <summary>
        /// Indica en donde se guardo el documento, si el 
        /// </summary>
        public StorageLocations StorageLocation { get; set; }

    }



    /// <summary>
    /// 
    /// </summary>
    public enum StorageLocations
    {
        Disk,
        Memory
    }
}

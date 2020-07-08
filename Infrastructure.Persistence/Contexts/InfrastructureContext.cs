using Infrastructure.Domain.Files;
//using Infrastructure.Persistence.EntitiesConfiguration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Reflection;
using MongoDB.Driver;
using Utils.DB;

namespace Infrastructure.Persistence.Contexts
{
    public interface IInfrastructureContext {

        IMongoCollection<T> Set<T>();

    }

    public class InfrastructureContext: IInfrastructureContext
    {

        /// <summary>
        /// Archivos / Files
        /// </summary>
        public IMongoCollection<File> Files { get; set;}


        /// <summary>
        /// Conguracion de las entidades relacionadas con los archivos / Entities configurations
        /// related to the files
        /// </summary>
        public IMongoCollection<EntityConfiguration> EntitiesConfigurations { get; set; }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        public InfrastructureContext(IInfrastructureConnectionString settings)
        {
              MongoClient client = new MongoClient(settings.GetMongoConnectionString());
              IMongoDatabase database = client.GetDatabase(settings.Database);

              Files = database.GetCollection<File>("Files");

        }

       public IMongoCollection<T> Set<T>()
        {
            //string modelName = typeof(T).Name;
            PropertyInfo[] propertyInfos = this.GetType().GetProperties();
            foreach(PropertyInfo propertyInfo in propertyInfos)
            {
                Type t1 = propertyInfo.PropertyType;
                Type t2 = typeof(IMongoCollection<T>);
                if(t1 == t2)
                {
                    return propertyInfo.GetValue(this) as IMongoCollection<T>;
                }
            }

            return null;
        }

    }

}

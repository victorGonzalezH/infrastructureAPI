using Infrastructure.Domain;
using Infrastructure.Domain.Files;
using Infrastructure.Persistence.Contexts;
using SeedWork;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence
{
   
    public class UnitOfWork : IInfrastructureUnitOfWork
    {

        /// <summary>
        /// 
        /// </summary>
        private readonly IInfrastructureContext context;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public UnitOfWork(IInfrastructureContext context)
        {
            this.context = context;

            FilesRepository = new Repositories.Repository<File>(context);

            EntitiesConfigurationsRepository = new Repositories.Repository<EntityConfiguration>(context);

        }


        /// <summary>
        /// 
        /// </summary>
        public IRepository<EntityConfiguration> EntitiesConfigurationsRepository { get; private set; }


        /// <summary>
        /// 
        /// </summary>
        public IRepository<File> FilesRepository { get; private set; }



        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int Complete()
        {
            //return context.SaveChanges();
            return 0;
        }



        /// <summary>
        /// Completa mediante el guardado todas las modificaciones hechas en el contexto
        /// </summary>
        /// <returns></returns>
        public async Task<int> CompleteAsync()
        {
            //return await context.SaveChangesAsync();
            return 0;
        }



        /// <summary>
        /// 
        /// </summary>
        // public void Dispose()
        // {
        //     context.Dispose();
        // }

    }
}

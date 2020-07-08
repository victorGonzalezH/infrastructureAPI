using Infrastructure.Persistence.Contexts;
using SeedWork;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Infrastructure.Persistence.Repositories
{
    public class Repository<T>: IRepository<T> where T : class
    {

        /// <summary>
        /// 
        /// </summary>
        protected IInfrastructureContext context;



        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public Repository(IInfrastructureContext context)
        {
            this.context = context;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<T> GetByIdAsync(string id)
        {
            var filter = Builders<T>.Filter.Eq("Id", id);
            return await context.Set<T>().Find(filter) .FirstOrDefaultAsync();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T GetById(string id)
        {
            var filter = Builders<T>.Filter.Eq("Id", id);
            return context.Set<T>().Find(filter).FirstOrDefault();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public T Add(T item)
        {
            context.Set<T>().InsertOne(item);
            return item;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task<T> AddAsync(T item)
        {
            await context.Set<T>().InsertOneAsync(item);
            return item;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="items">Entidades que seran guardadas en la base de datos</param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> items)
        {
            await context.Set<T>().InsertManyAsync(items);
            return items;
        }




        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<T> GetAll()
        {
            return context.Set<T>().Find(doc => true).ToList();
        }



        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var items = await context.Set<T>().FindSync(doc => true).ToListAsync();
            return items.AsEnumerable();
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="spec"></param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> GetBySpecAsync(ISpecification<T> spec)
        {
            //Primer query que incluye los includes como expresiones
            // IQueryable<T> queryableResultWithIncludes = spec.Includes.Aggregate(context.Set<T>().AsQueryable(), (current, include) => current.AsQueryable().ToLookup(include));


            // IQueryable<T> secondaryResult = spec.IncludeStrings.Aggregate(queryableResultWithIncludes, (current, include) => current.Include(include));

            return await context.Set<T>().Find(spec.Criteria).ToListAsync();
            // return await secondaryResult.Where(spec.Criteria).ToListAsync();
            
        }
        
    }

}


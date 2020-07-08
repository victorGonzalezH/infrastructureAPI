using SeedWork;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Infrastructure.Domain.Files
{

    public class EntityCodeNameSpecification : SpecificationBase<EntityConfiguration>
    {


        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityCodeName"></param>
        public EntityCodeNameSpecification(string entityCodeName) : base(ec => ec.EntityCodeName == entityCodeName)
        {
            
        }
    }
}

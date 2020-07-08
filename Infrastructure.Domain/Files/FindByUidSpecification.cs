using SeedWork;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Infrastructure.Domain.Files
{
    public class FindByUidSpecification : SpecificationBase<File>
    {

        public FindByUidSpecification(Guid guid) : base(file => file.Id.CompareTo(guid) == 0)
        {

        }
    }
}

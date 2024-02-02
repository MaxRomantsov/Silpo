using Ardalis.Specification;
using Silpo.Core.Entities.Site;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Silpo.Core.Entities.Specifications
{
    public static class Products
    {
        public class All : Specification<Product>
        {
            public All()
            {
                Query.Include(x => x.Category).OrderByDescending(x => x.Id);
            }
        }

        public class ById : Specification<Product>
        {
            public ById(int id)
            {
                Query.Where(p => p.Id == id).Include(x => x.Category);
            }
        }

        public class ByCategory : Specification<Product>
        {
            public ByCategory(int categoryId)
            {
                Query
                  .Include(x => x.Category)
                  .Where(c => c.CategoryId == categoryId).OrderByDescending(x => x.Id); ;
            }
        }
        public class Search : Specification<Product>
        {
            public Search(string searchString)
            {
                Query
                    .Include(p => p.Category)
                    .Where(p => p.Title.Contains(searchString)).OrderByDescending(x => x.Id);
            }
        }
    }
}

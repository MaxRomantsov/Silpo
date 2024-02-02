using Microsoft.EntityFrameworkCore;
using Silpo.Core.Entities.Site;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silpo.Infrastructure.Initializers
{
    internal static class CategoryAndGoodsInitializer
    {
        public static void SeedCategories(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(new Category[]
            {
                new Category() { Id = 1, Name = "Hygiene"},
                new Category() { Id = 2, Name = "Baking"},
                new Category() { Id = 3, Name = "For Home"},
                new Category() { Id = 4, Name = "Sweet"},
            });
        }
    }
}

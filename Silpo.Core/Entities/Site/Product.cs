using Ardalis.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Silpo.Core.Interfaces;

namespace Silpo.Core.Entities.Site
{
    public class Product : IEntity
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Price { get; set; } = string.Empty;
        public string? ImagePath { get; set; } = "Default.png";
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}

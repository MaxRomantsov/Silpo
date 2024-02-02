using AutoMapper;
using Silpo.Core.DTO_s.Product;
using Silpo.Core.Entities.Site;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silpo.Core.AutoMapper.Products
{
    public class AutoMapperProductsProfile : Profile
    {
        public AutoMapperProductsProfile()
        {
            CreateMap<ProductDto, Product>().ReverseMap();
        }
    }
}

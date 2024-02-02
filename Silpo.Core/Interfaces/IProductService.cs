using Silpo.Core.DTO_s.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silpo.Core.Interfaces
{
    public interface IProductService
    {
        Task<List<ProductDto>> GetAll();
        Task<ProductDto?> Get(int id);
        Task<List<ProductDto>> GetByCategory(int id);
        Task Create(ProductDto model);
        Task Update(ProductDto model);
        Task Delete(int id);
    }
}

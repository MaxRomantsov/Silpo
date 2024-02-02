using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Silpo.Core.DTO_s.Product;
using Silpo.Core.Entities.Site;
using Silpo.Core.Entities.Specifications;
using Silpo.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silpo.Core.Services
{
    public class ProductService : IProductService
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMapper _mapper;
        private readonly IRepository<Product> _productRepo;

        public ProductService(IConfiguration configuration, IRepository<Product> productRepo, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _mapper = mapper;
            _productRepo = productRepo;
            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;
        }
        public async Task Create(ProductDto model)
        {
            if (model.File.Count > 0)
            {
                string webRootPath = _webHostEnvironment.WebRootPath;
                string upload = webRootPath + _configuration.GetValue<string>("ImageSettings:ImagePath");
                var files = model.File;
                string fileName = Guid.NewGuid().ToString();
                string extensions = Path.GetExtension(files[0].FileName);
                using (var fileStream = new FileStream(Path.Combine(upload, fileName + extensions), FileMode.Create))
                {
                    files[0].CopyTo(fileStream);
                }
                model.ImagePath = fileName + extensions;
            }
            else
            {
                model.ImagePath = "Default.png";
            }

            await _productRepo.Insert(_mapper.Map<Product>(model));
            await _productRepo.Save();
        }
        public async Task<ProductDto?> Get(int id)
        {
            if (id < 0) return null;

            var post = await _productRepo.GetByID(id);

            if (post == null) return null;

            return _mapper.Map<ProductDto>(post);
        }

        public async Task<List<ProductDto>> GetByCategory(int id)
        {
            var result = await _productRepo.GetListBySpec(new Products.ByCategory(id));
            return _mapper.Map<List<ProductDto>>(result);
        }
        public async Task<List<ProductDto>> GetAll()
        {
            var result = await _productRepo.GetListBySpec(new Products.All());
            return _mapper.Map<List<ProductDto>>(result);
        }
        public async Task Update(ProductDto model)
        {
            var currentPost = await _productRepo.GetByID(model.Id);
            if (model.File.Count > 0)
            {
                string webPathRoot = _webHostEnvironment.WebRootPath;
                string upload = webPathRoot + _configuration.GetValue<string>("ImageSettings:ImagePath");

                string existingFilePath = Path.Combine(upload, currentPost.ImagePath);

                if (File.Exists(existingFilePath) && model.ImagePath != "Default.png")
                {
                    File.Delete(existingFilePath);
                }

                var files = model.File;

                string fileName = Guid.NewGuid().ToString();
                string extension = Path.GetExtension(files[0].FileName);
                using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                {
                    files[0].CopyTo(fileStream);
                }
                model.ImagePath = fileName + extension;

            }
            else
            {
                model.ImagePath = currentPost.ImagePath;
            }
            await _productRepo.Update(_mapper.Map<Product>(model));
            await _productRepo.Save();
        }
        public async Task Delete(int id)
        {
            var currentPost = await Get(id);

            if (currentPost == null) return;

            string webPathRoot = _webHostEnvironment.WebRootPath;
            string upload = webPathRoot + _configuration.GetValue<string>("ImageSettings:ImagePath");

            string existingFilePath = Path.Combine(upload, currentPost.ImagePath);

            if (File.Exists(existingFilePath) && currentPost.ImagePath != "Default.png")
            {
                File.Delete(existingFilePath);
            }

            await _productRepo.Delete(id);
            await _productRepo.Save();
        }
    }
}

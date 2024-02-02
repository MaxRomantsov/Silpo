using FluentValidation;
using Silpo.Core.DTO_s.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silpo.Core.Validation.Product
{
    public class CreateValidation : AbstractValidator<ProductDto>
    {
        public CreateValidation()
        {
            RuleFor(r => r.Title).NotEmpty();
            RuleFor(r => r.Price).NotEmpty();
            RuleFor(r => r.CategoryId).NotEmpty();
        }
    }
}

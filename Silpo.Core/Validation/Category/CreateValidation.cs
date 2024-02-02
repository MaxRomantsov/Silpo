using FluentValidation;
using Silpo.Core.DTO_s.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silpo.Core.Validation.Category
{
    public class CreateValidation : AbstractValidator<CategoryDto>
    {
        public CreateValidation()
        {
            RuleFor(r => r.Name).NotEmpty();
        }
    }
}

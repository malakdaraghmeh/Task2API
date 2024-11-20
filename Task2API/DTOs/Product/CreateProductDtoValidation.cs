using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Task2API.Data;

namespace Task2API.DTOs.Product
{
    public class CreateProductDtoValidation:AbstractValidator<CreateProductDtos>
    {
        private readonly ApplicationDbContext context;

        public CreateProductDtoValidation(ApplicationDbContext context)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Product name is required.")
        .MinimumLength(5).WithMessage("Product name must be at least 5 characters long.")
        .MaximumLength(30).WithMessage("Product name must not exceed 30 characters.")
        .MustAsync(BeUnique).WithMessage("Product name must be unique.");


            RuleFor(x => x.Description).NotEmpty().WithMessage("Product description is required.")
            .MinimumLength(10).WithMessage("Product description must be at least 10 characters long.");

            RuleFor(x => x.price).NotEmpty().WithMessage("Product price is required.")
                .InclusiveBetween(20, 3000).WithMessage("Product price must be between 20 and 3000.");
            this.context = context;
        }

        private async Task<bool> BeUnique(string name, CancellationToken cancellationToken)
        {
            return !await context.Products.AnyAsync(p => p.Name == name, cancellationToken);
        }
    }
}

using FluentValidation;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Task2API.Data;
using Task2API.DTOs.Product;
using Task2API.Models;

namespace Task2API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public ProductsController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet("GetAll")]
        public async Task <IActionResult> GetAllAsync()
        {
            var product = await context.Products.ToListAsync();
            var response = product.Adapt<IEnumerable<GetProcutDtos>>();
            return Ok(response);
        }

        [HttpGet("Details")]
        public async Task<IActionResult> GetByIdAsync(int Id)
        {
            var product = await context.Products.FindAsync(Id);
            if (product is null)
            {
                return NotFound("employee not found");
            }
            var response = product.Adapt<DetailsProductDtos>();

            return Ok(response);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateAsync(CreateProductDtos productDto,
            [FromServices]IValidator<CreateProductDtos> validator)
        {
            var validationResult= await validator.ValidateAsync(productDto);
            if (!validationResult.IsValid)
            {
                var modelState = new ModelStateDictionary();
                validationResult.Errors.ForEach(error =>
                {
                    modelState.AddModelError(error.PropertyName, error.ErrorMessage);
                });
                return ValidationProblem(modelState);
            }
            var product = productDto.Adapt<Product>();
            await context.Products.AddAsync(product);
            await context.SaveChangesAsync();
            return Ok("success");

        }

        [HttpPut("Update")]
        public async Task<IActionResult> UpdateAsync(int Id, UpdateProductDtos productDto)
        {
            var current = await context.Products.FindAsync(Id);
            if (current is null)
            {
                return NotFound("employee not found");
            }
            productDto.Adapt(current);
            await context.SaveChangesAsync();
            var product = current.Adapt<UpdateProductDtos>();
            return Ok(product);
        }
        [HttpDelete("Remove")]
        public async Task<IActionResult> RemoveAsync(int Id)
        {
            var product = await context.Products.FindAsync(Id);
            if (product is null)
            {
                return NotFound("employee not found");
            }
            context.Products.Remove(product);
            var pro = product.Adapt<RemoveProductDtos>();
           await context.SaveChangesAsync();
            return Ok(pro);
        }
    }
}

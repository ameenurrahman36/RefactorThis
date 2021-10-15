using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RefactorThis.Models;
using RefactorThis.Providers;
using RefactorThis.Repository;

namespace RefactorThis.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsRepository<Product> _productsRepository;
        private readonly IProductOptionsRepository<ProductOption> _productOptionsRepository;
        private readonly ILoggerManager _logger;

        public ProductsController(IProductsRepository<Product> productsRepository,
            IProductOptionsRepository<ProductOption> productOptionsRepository,
            ILoggerManager logger)
        {
            _productsRepository = productsRepository;
            _productOptionsRepository = productOptionsRepository;
            _logger = logger;
        }

        // GET /products` - gets all products.
        // GET /products?name={name}` - finds all products matching the specified name.
        [HttpGet]
        public async Task<ActionResult> GetProducts([FromQuery] string name)
        {
            _logger.LogInformation($"GET /products?name={name}");

            IEnumerable<Product> products = null;

            if (!string.IsNullOrEmpty(name))
            {
                products = await _productsRepository.GetAll(name);
            }
            else
            {
                products = await _productsRepository.GetAll();
            }

            if (products == null)
            {
                return NotFound();
            }

            return Ok(products);
        }

        // GET /products/{id}` - gets the project that matches the specified ID - ID is a GUID.
        [HttpGet("{id:Guid}")]
        public async Task<ActionResult> GetProduct(Guid id)
        {
            _logger.LogInformation($"GET /products/{id}");

            var product = await _productsRepository.Get(id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        // PUT /products/{id}` - updates a product.
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(Guid id, Product product)
        {
            _logger.LogInformation($"PUT /products/{id}");

            if (id != product.Id)
            {
                return BadRequest();
            }

            if (!await _productsRepository.Exists(id))
            {
                return NotFound();
            }

            await _productsRepository.Update(product);

            return NoContent();
        }

        // POST /products` - creates a new product.
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult> PostProduct(Product product)
        {
            _logger.LogInformation($"POST /products");

            await _productsRepository.Add(product);

            return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        }

        // DELETE /products/{id}` - deletes a product and its options.
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            _logger.LogInformation($"DELETE /products/{id}");

            var product = await _productsRepository.Get(id);
            if (product == null)
            {
                return NotFound();
            }

            await _productsRepository.Delete(product);

            return NoContent();
        }

        // GET /products/{id}/options` - finds all options for a specified product.
        [HttpGet("{id:Guid}/options")]
        public async Task<ActionResult> GetOptionsByProduct(Guid id)
        {
            _logger.LogInformation($"GET /products/{id}/options");

            var productOptions = await _productOptionsRepository.GetAll(id);

            if (productOptions == null)
            {
                return NotFound();
            }

            return Ok(productOptions);
        }

        // GET /products/{id}/options/{optionId}` - finds the specified product option for the specified product.
        [HttpGet("{id:Guid}/options/{optionId:Guid}")]
        public async Task<ActionResult> GetOption(Guid id, Guid optionId)
        {
            _logger.LogInformation($"GET /products/{id}/options/{optionId}");

            var productOption = await _productOptionsRepository.Get(id, optionId);

            if (productOption == null)
            {
                return NotFound();
            }

            return Ok(productOption);
        }

        // POST /products/{id}/options` - adds a new product option to the specified product.
        [HttpPost("{id:Guid}/options")]
        public async Task<ActionResult> PostProduct(Guid id, ProductOption productOption)
        {
            _logger.LogInformation($"POST /products/{id}/options");

            productOption.ProductId = id;

            await _productOptionsRepository.Add(productOption);

            return CreatedAtAction("GetOption", new { id = productOption.ProductId, optionId = productOption.Id }, productOption);
        }

        // PUT /products/{id}/options/{optionId}` - updates the specified product option.
        [HttpPut("{id:Guid}/options/{optionId:Guid}")]
        public async Task<IActionResult> PutProductOption(Guid id, Guid optionId, ProductOption productOption)
        {
            _logger.LogInformation($"PUT /products/{id}/options/{optionId}");

            if (id != productOption.ProductId || optionId != productOption.Id)
            {
                return BadRequest();
            }

            if (!await _productOptionsRepository.Exists(optionId))
            {
                return NotFound();
            }

            await _productOptionsRepository.Update(productOption);

            return NoContent();
        }

        // DELETE /products/{id}/options/{optionId}` - deletes the specified product option.
        [HttpDelete("{id:Guid}/options/{optionId:Guid}")]
        public async Task<IActionResult> DeleteProductOption(Guid id, Guid optionId)
        {
            _logger.LogInformation($"DELETE /products/{id}/options/{optionId}");

            var productOption = await _productOptionsRepository.Get(id, optionId);

            if (productOption == null)
            {
                return NotFound();
            }

            await _productOptionsRepository.Delete(productOption);

            return NoContent();
        }
    }
}

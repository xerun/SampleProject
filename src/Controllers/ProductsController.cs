using Microsoft.AspNetCore.Mvc;
using MyApi.Services;
using MyApi.Models;
using System.Collections.Generic;

namespace MyApi.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        // GET: api/products
        [HttpGet]
        public ActionResult<IEnumerable<ProductModel>> GetProducts()
        {
            return Ok(_productService.GetProducts());
        }

        // GET: api/products/{id}
        [HttpGet("{id}")]
        public ActionResult<ProductModel> GetProductById(int id)
        {
            var product = _productService.GetProductById(id);
            if (product == null)
            {
                return NotFound(new { message = "Product not found" });
            }
            return Ok(product);
        }
    }
}

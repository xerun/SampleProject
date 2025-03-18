using System.Collections.Generic;
using System.Linq;
using MyApi.Models;
using MyApi.Services;
using Xunit;

namespace MyApi.Tests.Services
{
    public class ProductServiceTests
    {
        private readonly ProductService _productService;

        public ProductServiceTests()
        {
            // Arrange
            _productService = new ProductService();
        }

        [Fact]
        public void GetProducts_WhenCalled_ReturnsAllProducts()
        {
            // Act
            var products = _productService.GetProducts();

            // Assert
            Assert.NotNull(products);
            Assert.Equal(2, products.Count());
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void GetProductById_WhenValidId_ReturnsCorrectProduct(int id)
        {
            // Act
            var product = _productService.GetProductById(id);

            // Assert
            Assert.NotNull(product);
            Assert.Equal(id, product.Id);
        }

        [Fact]
        public void GetProductById_WhenInvalidId_ReturnsNull()
        {
            // Arrange
            int id = 3; // Assuming there is no product with ID 3

            // Act
            var product = _productService.GetProductById(id);

            // Assert
            Assert.Null(product);
        }

        [Fact]
        public void ProductService_Constructor_InitializesWithTwoProducts()
        {
            // Arrange & Act (already done in the constructor call)
            
            // Assert
            var products = _productService.GetProducts();
            Assert.NotNull(products);
            Assert.Equal(2, products.Count());
        }
    }
}
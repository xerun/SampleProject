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
            _productService = new ProductService();
        }

        [Fact]
        public void GetProducts_WhenCalled_ReturnsAllProducts()
        {
            // Act
            var result = _productService.GetProducts();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Contains(new ProductModel { Id = 1, Name = "Laptop", Price = 1200.99M }, result);
            Assert.Contains(new ProductModel { Id = 2, Name = "Smartphone", Price = 799.50M }, result);
        }

        [Fact]
        public void GetProductById_WhenIdExists_ReturnsCorrectProduct()
        {
            // Act
            var product = _productService.GetProductById(1);

            // Assert
            Assert.NotNull(product);
            Assert.Equal("Laptop", product.Name);
            Assert.Equal(1200.99M, product.Price);
        }

        [Fact]
        public void GetProductById_WhenIdDoesNotExist_ReturnsNull()
        {
            // Act
            var product = _productService.GetProductById(3);

            // Assert
            Assert.Null(product);
        }

        [Fact]
        public void Constructor_WhenCalled_InitializesProductList()
        {
            // Act
            var result = _productService.GetProducts();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Contains(new ProductModel { Id = 1, Name = "Laptop", Price = 1200.99M }, result);
            Assert.Contains(new ProductModel { Id = 2, Name = "Smartphone", Price = 799.50M }, result);
        }
    }
}
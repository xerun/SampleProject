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
        public void GetProductByName_WhenNameExists_ReturnsCorrectProduct()
        {
            // Act
            var product = _productService.GetProductByName("Laptop");

            // Assert
            Assert.NotNull(product);
            Assert.Equal(1, product.Id);
            Assert.Equal(1200.99M, product.Price);
        }

        [Fact]
        public void GetProductByName_WhenNameDoesNotExist_ReturnsNull()
        {
            // Act
            var product = _productService.GetProductByName("Tablet");

            // Assert
            Assert.Null(product);
        }

        [Fact]
        public void GetProductByName_WhenNameCaseInsensitive_ReturnsCorrectProduct()
        {
            // Act
            var productLowercase = _productService.GetProductByName("laptop");
            var productUppercase = _productService.GetProductByName("LAPTOP");

            // Assert
            Assert.NotNull(productLowercase);
            Assert.NotNull(productUppercase);
            Assert.Equal(1, productLowercase.Id);
            Assert.Equal(1200.99M, productLowercase.Price);
            Assert.Equal(1, productUppercase.Id);
            Assert.Equal(1200.99M, productUppercase.Price);
        }
    }
}
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
        public void GetProductByName_WhenNameExistsButCaseDifferent_ReturnsCorrectProduct()
        {
            // Act
            var product = _productService.GetProductByName("lAPTOP");

            // Assert
            Assert.NotNull(product);
            Assert.Equal(1, product.Id);
            Assert.Equal(1200.99M, product.Price);
        }

        [Fact]
        public void AddProduct_WhenProductIsNull_DoesNotAddProduct()
        {
            // Arrange
            var initialCount = _productService.GetProducts().Count();

            // Act
            _productService.AddProduct(null);

            // Assert
            var finalCount = _productService.GetProducts().Count();
            Assert.Equal(initialCount, finalCount);
        }

        [Fact]
        public void AddProduct_WhenProductIdAlreadyExists_DoesNotAddProduct()
        {
            // Arrange
            var initialCount = _productService.GetProducts().Count();
            var existingProduct = new ProductModel { Id = 1, Name = "Existing Laptop", Price = 999.99M };

            // Act
            _productService.AddProduct(existingProduct);

            // Assert
            var finalCount = _productService.GetProducts().Count();
            Assert.Equal(initialCount, finalCount);
        }

        [Fact]
        public void AddProduct_WhenProductIdDoesNotExist_AddsProduct()
        {
            // Arrange
            var initialCount = _productService.GetProducts().Count();
            var newProduct = new ProductModel { Id = 3, Name = "Tablet", Price = 299.99M };

            // Act
            _productService.AddProduct(newProduct);

            // Assert
            var finalCount = _productService.GetProducts().Count();
            Assert.Equal(initialCount + 1, finalCount);
            var addedProduct = _productService.GetProductById(3);
            Assert.NotNull(addedProduct);
            Assert.Equal("Tablet", addedProduct.Name);
            Assert.Equal(299.99M, addedProduct.Price);
        }
    }
}
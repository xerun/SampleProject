using Xunit;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using MyApi.Models;
using MyApi.Services;

namespace MyApi.Tests.Services
{
    public class ProductServiceTests
    {
        [Fact]
        public void GetProducts_WhenNoSort_ReturnsAllProducts()
        {
            // Arrange
            var productService = new ProductService();

            // Act
            var result = productService.GetProducts().ToList();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, p => p.Id == 1 && p.Name == "Laptop");
            Assert.Contains(result, p => p.Id == 2 && p.Name == "Smartphone");
        }

        [Fact]
        public void GetProducts_WhenSortByNameAscending_ReturnsSortedByName()
        {
            // Arrange
            var productService = new ProductService();

            // Act
            var result = productService.GetProducts("name", true).ToList();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal("Laptop", result[0].Name);
            Assert.Equal("Smartphone", result[1].Name);
        }

        [Fact]
        public void GetProducts_WhenSortByNameDescending_ReturnsSortedByName()
        {
            // Arrange
            var productService = new ProductService();

            // Act
            var result = productService.GetProducts("name", false).ToList();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal("Smartphone", result[0].Name);
            Assert.Equal("Laptop", result[1].Name);
        }

        [Fact]
        public void GetProducts_WhenSortByPriceAscending_ReturnsSortedByPrice()
        {
            // Arrange
            var productService = new ProductService();

            // Act
            var result = productService.GetProducts("price", true).ToList();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal(799.50M, result[0].Price);
            Assert.Equal(1200.99M, result[1].Price);
        }

        [Fact]
        public void GetProducts_WhenSortByPriceDescending_ReturnsSortedByPrice()
        {
            // Arrange
            var productService = new ProductService();

            // Act
            var result = productService.GetProducts("price", false).ToList();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal(1200.99M, result[0].Price);
            Assert.Equal(799.50M, result[1].Price);
        }

        [Fact]
        public void GetProductById_WhenValidId_ReturnsProduct()
        {
            // Arrange
            var productService = new ProductService();

            // Act
            var result = productService.GetProductById(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Laptop", result.Name);
        }

        [Fact]
        public void GetProductById_WhenInvalidId_ReturnsNull()
        {
            // Arrange
            var productService = new ProductService();

            // Act
            var result = productService.GetProductById(3);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetProductByName_WhenValidName_ReturnsProduct()
        {
            // Arrange
            var productService = new ProductService();

            // Act
            var result = productService.GetProductByName("Laptop");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
        }

        [Fact]
        public void GetProductByName_WhenInvalidName_ReturnsNull()
        {
            // Arrange
            var productService = new ProductService();

            // Act
            var result = productService.GetProductByName("Tablet");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void SearchProducts_WhenKeywordMatchesName_ReturnsMatchingProducts()
        {
            // Arrange
            var productService = new ProductService();

            // Act
            var result = productService.SearchProducts("Laptop").ToList();

            // Assert
            Assert.Single(result);
            Assert.Equal("Laptop", result[0].Name);
        }

        [Fact]
        public void SearchProducts_WhenKeywordMatchesCategory_ReturnsMatchingProducts()
        {
            // Arrange
            var productService = new ProductService();

            // Act
            var result = productService.SearchProducts("Electronics").ToList();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, p => p.Name == "Laptop");
            Assert.Contains(result, p => p.Name == "Smartphone");
        }

        [Fact]
        public void SearchProducts_WhenNoMatches_ReturnsEmptyList()
        {
            // Arrange
            var productService = new ProductService();

            // Act
            var result = productService.SearchProducts("Tablet").ToList();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void FilterByCategory_WhenValidCategory_ReturnsMatchingProducts()
        {
            // Arrange
            var productService = new ProductService();

            // Act
            var result = productService.FilterByCategory("Electronics").ToList();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, p => p.Name == "Laptop");
            Assert.Contains(result, p => p.Name == "Smartphone");
        }

        [Fact]
        public void FilterByCategory_WhenInvalidCategory_ReturnsEmptyList()
        {
            // Arrange
            var productService = new ProductService();

            // Act
            var result = productService.FilterByCategory("Clothing").ToList();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void CreateProduct_WhenValidProduct_ReturnsCreatedProduct()
        {
            // Arrange
            var productService = new ProductService();
            var product = new ProductModel { Name = "Tablet", Price = 399.99M, Category = "Electronics", InStock = true };

            // Act
            var result = productService.CreateProduct(product);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Id);
            Assert.Equal("Tablet", result.Name);
        }

        [Fact]
        public void UpdateProduct_WhenValidId_ReturnsTrueAndUpdatesProduct()
        {
            // Arrange
            var productService = new ProductService();
            var updatedProduct = new ProductModel { Name = "Updated Laptop", Price = 1300.99M, Category = "Electronics", InStock = false };

            // Act
            var result = productService.UpdateProduct(1, updatedProduct);

            // Assert
            Assert.True(result);
            var product = productService.GetProductById(1);
            Assert.NotNull(product);
            Assert.Equal("Updated Laptop", product.Name);
        }

        [Fact]
        public void UpdateProduct_WhenInvalidId_ReturnsFalse()
        {
            // Arrange
            var productService = new ProductService();
            var updatedProduct = new ProductModel { Name = "Updated Laptop", Price = 1300.99M, Category = "Electronics", InStock = false };

            // Act
            var result = productService.UpdateProduct(3, updatedProduct);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void DeleteProduct_WhenValidId_ReturnsTrueAndDeletesProduct()
        {
            // Arrange
            var productService = new ProductService();

            // Act
            var result = productService.DeleteProduct(1);

            // Assert
            Assert.True(result);
            var product = productService.GetProductById(1);
            Assert.Null(product);
        }

        [Fact]
        public void DeleteProduct_WhenInvalidId_ReturnsFalse()
        {
            // Arrange
            var productService = new ProductService();

            // Act
            var result = productService.DeleteProduct(3);

            // Assert
            Assert.False(result);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using MyApi.Models;
using MyApi.Services;

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
        public void GetProducts_WhenDefault_ReturnsAllProductsUnsorted()
        {
            // Act
            var result = _productService.GetProducts().ToList();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, p => p.Id == 1);
            Assert.Contains(result, p => p.Id == 2);
        }

        [Fact]
        public void GetProducts_WhenSortByNameAscending_ReturnsProductsSortedByName()
        {
            // Act
            var result = _productService.GetProducts("name", true).ToList();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal("Laptop", result[0].Name);
            Assert.Equal("Smartphone", result[1].Name);
        }

        [Fact]
        public void GetProducts_WhenSortByNameDescending_ReturnsProductsSortedByNameDesc()
        {
            // Act
            var result = _productService.GetProducts("name", false).ToList();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal("Smartphone", result[0].Name);
            Assert.Equal("Laptop", result[1].Name);
        }

        [Fact]
        public void GetProducts_WhenSortByPriceAscending_ReturnsProductsSortedByPrice()
        {
            // Act
            var result = _productService.GetProducts("price", true).ToList();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal("Smartphone", result[0].Name);
            Assert.Equal("Laptop", result[1].Name);
        }

        [Fact]
        public void GetProducts_WhenSortByPriceDescending_ReturnsProductsSortedByPriceDesc()
        {
            // Act
            var result = _productService.GetProducts("price", false).ToList();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal("Laptop", result[0].Name);
            Assert.Equal("Smartphone", result[1].Name);
        }

        [Fact]
        public void GetProductById_WhenIdExists_ReturnsProduct()
        {
            // Act
            var product = _productService.GetProductById(1);

            // Assert
            Assert.NotNull(product);
            Assert.Equal(1, product.Id);
            Assert.Equal("Laptop", product.Name);
        }

        [Fact]
        public void GetProductById_WhenIdDoesNotExist_ReturnsNull()
        {
            // Act
            var product = _productService.GetProductById(99);

            // Assert
            Assert.Null(product);
        }

        [Fact]
        public void GetProductByName_WhenNameExists_ReturnsProduct()
        {
            // Act
            var product = _productService.GetProductByName("Smartphone");

            // Assert
            Assert.NotNull(product);
            Assert.Equal(2, product.Id);
            Assert.Equal("Smartphone", product.Name);
        }

        [Fact]
        public void GetProductByName_WhenNameDoesNotExist_ReturnsNull()
        {
            // Act
            var product = _productService.GetProductByName("Nonexistent");

            // Assert
            Assert.Null(product);
        }

        [Fact]
        public void SearchProducts_WhenKeywordMatchesProductName_ReturnsMatchingProducts()
        {
            // Act
            var result = _productService.SearchProducts("laptop").ToList();

            // Assert
            Assert.Single(result);
            Assert.Equal(1, result[0].Id);
            Assert.Equal("Laptop", result[0].Name);
        }

        [Fact]
        public void SearchProducts_WhenKeywordMatchesCategory_ReturnsMatchingProducts()
        {
            // Act
            var result = _productService.SearchProducts("electronics").ToList();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, p => p.Id == 1);
            Assert.Contains(result, p => p.Id == 2);
        }

        [Fact]
        public void SearchProducts_WhenKeywordDoesNotMatch_ReturnsNoProduct()
        {
            // Act
            var result = _productService.SearchProducts("nonexistent").ToList();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void FilterByCategory_WhenCategoryExists_ReturnsMatchingProducts()
        {
            // Act
            var result = _productService.FilterByCategory("electronics").ToList();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, p => p.Id == 1);
            Assert.Contains(result, p => p.Id == 2);
        }

        [Fact]
        public void FilterByCategory_WhenCategoryDoesNotExist_ReturnsNoProduct()
        {
            // Act
            var result = _productService.FilterByCategory("nonexistent").ToList();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void CreateProduct_WhenNewProductAdded_ReturnsCreatedProductWithId()
        {
            // Arrange
            var newProduct = new ProductModel { Name = "Tablet", Price = 399.00M, Category = "Electronics", InStock = true };

            // Act
            var result = _productService.CreateProduct(newProduct);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Id); // Assuming no other products were added
        }

        [Fact]
        public void UpdateProduct_WhenIdExists_ReturnsTrueAndUpdateProduct()
        {
            // Arrange
            var updatedProduct = new ProductModel { Name = "Updated Laptop", Price = 1099.99M, Category = "Electronics", InStock = true };

            // Act
            var updateResult = _productService.UpdateProduct(1, updatedProduct);
            var resultProduct = _productService.GetProductById(1);

            // Assert
            Assert.True(updateResult);
            Assert.NotNull(resultProduct);
            Assert.Equal("Updated Laptop", resultProduct.Name);
        }

        [Fact]
        public void UpdateProduct_WhenIdDoesNotExist_ReturnsFalse()
        {
            // Arrange
            var updatedProduct = new ProductModel { Name = "Nonexistent Product", Price = 10.99M, Category = "Electronics", InStock = true };

            // Act
            var updateResult = _productService.UpdateProduct(99, updatedProduct);

            // Assert
            Assert.False(updateResult);
        }

        [Fact]
        public void DeleteProduct_WhenIdExists_ReturnsTrueAndRemovesProduct()
        {
            // Arrange
            var productToDelete = new ProductModel { Name = "To Be Deleted", Price = 10.99M, Category = "Electronics", InStock = true };
            _productService.CreateProduct(productToDelete);

            // Act
            var deleteResult = _productService.DeleteProduct(3);
            var resultProduct = _productService.GetProductById(3);

            // Assert
            Assert.True(deleteResult);
            Assert.Null(resultProduct);
        }

        [Fact]
        public void DeleteProduct_WhenIdDoesNotExist_ReturnsFalse()
        {
            // Act
            var deleteResult = _productService.DeleteProduct(99);

            // Assert
            Assert.False(deleteResult);
        }
    }
}
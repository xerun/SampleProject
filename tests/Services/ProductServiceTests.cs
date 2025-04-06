using System;
using System.Collections.Generic;
using System.Linq;
using MyApi.Models;
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
        public void GetProducts_WhenNoSort_ReturnsUnsortedList()
        {
            var result = _productService.GetProducts().ToList();

            Assert.Equal(2, result.Count);
            Assert.Equal("Laptop", result[0].Name);
            Assert.Equal("Smartphone", result[1].Name);
        }

        [Fact]
        public void GetProducts_WhenSortByNameAscending_ReturnsSortedByName()
        {
            var result = _productService.GetProducts(sortBy: "name").ToList();

            Assert.Equal(2, result.Count);
            Assert.Equal("Laptop", result[0].Name);
            Assert.Equal("Smartphone", result[1].Name);
        }

        [Fact]
        public void GetProducts_WhenSortByNameDescending_ReturnsSortedByName()
        {
            var result = _productService.GetProducts(sortBy: "name", ascending: false).ToList();

            Assert.Equal(2, result.Count);
            Assert.Equal("Smartphone", result[0].Name);
            Assert.Equal("Laptop", result[1].Name);
        }

        [Fact]
        public void GetProducts_WhenSortByPriceAscending_ReturnsSortedByPrice()
        {
            var result = _productService.GetProducts(sortBy: "price").ToList();

            Assert.Equal(2, result.Count);
            Assert.Equal("Smartphone", result[0].Name);
            Assert.Equal("Laptop", result[1].Name);
        }

        [Fact]
        public void GetProducts_WhenSortByPriceDescending_ReturnsSortedByPrice()
        {
            var result = _productService.GetProducts(sortBy: "price", ascending: false).ToList();

            Assert.Equal(2, result.Count);
            Assert.Equal("Laptop", result[0].Name);
            Assert.Equal("Smartphone", result[1].Name);
        }

        [Fact]
        public void GetProductById_WhenIdExists_ReturnsProduct()
        {
            var result = _productService.GetProductById(1);

            Assert.NotNull(result);
            Assert.Equal("Laptop", result.Name);
        }

        [Fact]
        public void GetProductById_WhenIdDoesNotExist_ReturnsNull()
        {
            var result = _productService.GetProductById(3);

            Assert.Null(result);
        }

        [Fact]
        public void GetProductByName_WhenNameExists_ReturnsProduct()
        {
            var result = _productService.GetProductByName("Laptop");

            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
        }

        [Fact]
        public void GetProductByName_WhenNameDoesNotExist_ReturnsNull()
        {
            var result = _productService.GetProductByName("Tablet");

            Assert.Null(result);
        }

        [Fact]
        public void SearchProducts_WhenKeywordMatches_ReturnsMatchingProducts()
        {
            var result = _productService.SearchProducts("smart").ToList();

            Assert.Single(result);
            Assert.Equal(2, result[0].Id);
        }

        [Fact]
        public void SearchProducts_WhenKeywordDoesNotMatch_ReturnsEmptyList()
        {
            var result = _productService.SearchProducts("tablet");

            Assert.Empty(result);
        }

        [Fact]
        public void FilterByCategory_WhenCategoryMatches_ReturnsMatchingProducts()
        {
            var result = _productService.FilterByCategory("electronics").ToList();

            Assert.Equal(2, result.Count);
            Assert.Equal("Laptop", result[0].Name);
            Assert.Equal("Smartphone", result[1].Name);
        }

        [Fact]
        public void FilterByCategory_WhenCategoryDoesNotMatch_ReturnsEmptyList()
        {
            var result = _productService.FilterByCategory("furniture");

            Assert.Empty(result);
        }

        [Fact]
        public void CreateProduct_WhenNewProductAdded_ReturnsCreatedProductWithId()
        {
            var product = new ProductModel { Name = "Tablet", Price = 300.75M, Category = "Electronics", InStock = true };

            var result = _productService.CreateProduct(product);

            Assert.NotNull(result);
            Assert.Equal(3, result.Id);
        }

        [Fact]
        public void UpdateProduct_WhenIdExists_ReturnsTrue()
        {
            var updatedProduct = new ProductModel { Name = "Updated Laptop", Price = 1500.99M, Category = "Electronics", InStock = true };

            var result = _productService.UpdateProduct(1, updatedProduct);

            Assert.True(result);
            var product = _productService.GetProductById(1);
            Assert.NotNull(product);
            Assert.Equal("Updated Laptop", product.Name);
        }

        [Fact]
        public void UpdateProduct_WhenIdDoesNotExist_ReturnsFalse()
        {
            var updatedProduct = new ProductModel { Name = "Nonexistent Laptop", Price = 2000.99M, Category = "Electronics", InStock = true };

            var result = _productService.UpdateProduct(3, updatedProduct);

            Assert.False(result);
            var product = _productService.GetProductById(3);
            Assert.Null(product);
        }
    }
}
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
        public void GetProducts_WhenNoSorting_ReturnsDefaultOrder()
        {
            var result = _productService.GetProducts().ToList();

            Assert.Equal(2, result.Count);
            Assert.Equal("Laptop", result[0].Name);
            Assert.Equal("Smartphone", result[1].Name);
        }

        [Fact]
        public void GetProducts_WhenSortingByNameAscending_ReturnsSortedByName()
        {
            var result = _productService.GetProducts(sortBy: "name").ToList();

            Assert.Equal(2, result.Count);
            Assert.Equal("Laptop", result[0].Name);
            Assert.Equal("Smartphone", result[1].Name);
        }

        [Fact]
        public void GetProducts_WhenSortingByNameDescending_ReturnsSortedByName()
        {
            var result = _productService.GetProducts(sortBy: "name", ascending: false).ToList();

            Assert.Equal(2, result.Count);
            Assert.Equal("Smartphone", result[0].Name);
            Assert.Equal("Laptop", result[1].Name);
        }

        [Fact]
        public void GetProducts_WhenSortingByPriceAscending_ReturnsSortedByPrice()
        {
            var result = _productService.GetProducts(sortBy: "price").ToList();

            Assert.Equal(2, result.Count);
            Assert.Equal("Smartphone", result[0].Name);
            Assert.Equal("Laptop", result[1].Name);
        }

        [Fact]
        public void GetProducts_WhenSortingByPriceDescending_ReturnsSortedByPrice()
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
            var result = _productService.GetProductById(999);

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
            var result = _productService.GetProductByName("Unknown Product");

            Assert.Null(result);
        }

        [Fact]
        public void SearchProducts_WhenKeywordMatchesName_ReturnsMatchingProducts()
        {
            var result = _productService.SearchProducts("smart").ToList();

            Assert.Single(result);
            Assert.Equal("Smartphone", result[0].Name);
        }

        [Fact]
        public void SearchProducts_WhenKeywordMatchesCategory_ReturnsMatchingProducts()
        {
            var result = _productService.SearchProducts("electronics").ToList();

            Assert.Equal(2, result.Count);
            Assert.Contains(result, p => p.Name == "Laptop");
            Assert.Contains(result, p => p.Name == "Smartphone");
        }

        [Fact]
        public void SearchProducts_WhenKeywordDoesNotMatch_ReturnsEmpty()
        {
            var result = _productService.SearchProducts("unknown").ToList();

            Assert.Empty(result);
        }

        [Fact]
        public void FilterByCategory_WhenCategoryMatches_ReturnsMatchingProducts()
        {
            var result = _productService.FilterByCategory("electronics").ToList();

            Assert.Equal(2, result.Count);
            Assert.Contains(result, p => p.Name == "Laptop");
            Assert.Contains(result, p => p.Name == "Smartphone");
        }

        [Fact]
        public void FilterByCategory_WhenCategoryDoesNotMatch_ReturnsEmpty()
        {
            var result = _productService.FilterByCategory("unknown").ToList();

            Assert.Empty(result);
        }

        [Fact]
        public void CreateProduct_WhenValidProduct_ReturnsCreatedProductWithNewId()
        {
            var newProduct = new ProductModel { Name = "Tablet", Price = 349.99M, Category = "Electronics", InStock = true };
            
            var result = _productService.CreateProduct(newProduct);

            Assert.NotNull(result);
            Assert.Equal(3, result.Id);
        }

        [Fact]
        public void UpdateProduct_WhenProductExists_ReturnsTrueAndUpdateProduct()
        {
            var updatedProduct = new ProductModel { Name = "Updated Laptop", Price = 1500.99M, Category = "Electronics", InStock = true };
            
            bool success = _productService.UpdateProduct(1, updatedProduct);

            Assert.True(success);
            var product = _productService.GetProductById(1);
            Assert.Equal("Updated Laptop", product.Name);
        }

        [Fact]
        public void UpdateProduct_WhenProductIdDoesNotExist_ReturnsFalse()
        {
            var updatedProduct = new ProductModel { Name = "Updated Laptop", Price = 1500.99M, Category = "Electronics", InStock = true };
            
            bool success = _productService.UpdateProduct(999, updatedProduct);

            Assert.False(success);
        }
    }
}
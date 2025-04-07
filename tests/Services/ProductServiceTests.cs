using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using MyApi.Models;
using Moq;

namespace MyApi.Tests.Services
{
    public class ProductServiceTests
    {
        private readonly ProductService _productService;
        private readonly List<ProductModel> _testProducts;

        public ProductServiceTests()
        {
            _testProducts = new List<ProductModel>
            {
                new ProductModel { Id = 1, Name = "Laptop", Price = 1200.99M, Category = "Electronics", InStock = true },
                new ProductModel { Id = 2, Name = "Smartphone", Price = 799.50M, Category = "Electronics", InStock = true }
            };

            _productService = new ProductService();
        }

        [Fact]
        public void GetProducts_WhenNoSort_ReturnsUnsortedList()
        {
            var result = _productService.GetProducts();

            Assert.Equal(_testProducts.OrderBy(p => p.Id), result);
        }

        [Fact]
        public void GetProducts_WhenNameAscending_ReturnsSortedByNameAscending()
        {
            var expected = new List<ProductModel> { _testProducts[1], _testProducts[0] };

            var result = _productService.GetProducts("name", true);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void GetProducts_WhenNameDescending_ReturnsSortedByNameDescending()
        {
            var expected = new List<ProductModel> { _testProducts[0], _testProducts[1] };

            var result = _productService.GetProducts("name", false);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void GetProducts_WhenPriceAscending_ReturnsSortedByPriceAscending()
        {
            var expected = new List<ProductModel> { _testProducts[1], _testProducts[0] };

            var result = _productService.GetProducts("price", true);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void GetProducts_WhenPriceDescending_ReturnsSortedByPriceDescending()
        {
            var expected = new List<ProductModel> { _testProducts[0], _testProducts[1] };

            var result = _productService.GetProducts("price", false);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void GetProductById_WhenIdExists_ReturnsCorrectProduct()
        {
            var expected = _testProducts[0];

            var result = _productService.GetProductById(1);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void GetProductById_WhenIdDoesNotExist_ReturnsNull()
        {
            var result = _productService.GetProductById(999);

            Assert.Null(result);
        }

        [Fact]
        public void GetProductByName_WhenNameExists_ReturnsCorrectProduct()
        {
            var expected = _testProducts[0];

            var result = _productService.GetProductByName("Laptop");

            Assert.Equal(expected, result);
        }

        [Fact]
        public void GetProductByName_WhenNameDoesNotExist_ReturnsNull()
        {
            var result = _productService.GetProductByName("NonExistentProduct");

            Assert.Null(result);
        }

        [Fact]
        public void SearchProducts_WhenKeywordMatches_ReturnsMatchingProducts()
        {
            var keyword = "elect";
            var expected = new List<ProductModel> { _testProducts[0], _testProducts[1] };

            var result = _productService.SearchProducts(keyword);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void SearchProducts_WhenKeywordDoesNotMatch_ReturnsEmptyList()
        {
            var keyword = "NonExistent";

            var result = _productService.SearchProducts(keyword);

            Assert.Empty(result);
        }

        [Fact]
        public void FilterByCategory_WhenCategoryMatches_ReturnsMatchingProducts()
        {
            var category = "Electronics";
            var expected = new List<ProductModel> { _testProducts[0], _testProducts[1] };

            var result = _productService.FilterByCategory(category);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void FilterByCategory_WhenCategoryDoesNotMatch_ReturnsEmptyList()
        {
            var category = "NonExistent";

            var result = _productService.FilterByCategory(category);

            Assert.Empty(result);
        }

        [Fact]
        public void CreateProduct_WhenNewProductIsAdded_ReturnsCreatedProductWithNewId()
        {
            var newProduct = new ProductModel { Name = "Tablet", Price = 499.99M, Category = "Electronics", InStock = true };

            var result = _productService.CreateProduct(newProduct);

            Assert.Equal(3, result.Id);
            Assert.Contains(result, _productService.GetProducts());
        }

        [Fact]
        public void UpdateProduct_WhenIdExists_ReturnsTrueAndUpdateProduct()
        {
            var updatedProduct = new ProductModel { Name = "Updated Laptop", Price = 1100.99M, Category = "Electronics", InStock = true };

            var result = _productService.UpdateProduct(1, updatedProduct);

            Assert.True(result);
            Assert.Equal(updatedProduct.Name, _productService.GetProductById(1)?.Name);
        }

        [Fact]
        public void UpdateProduct_WhenIdDoesNotExist_ReturnsFalse()
        {
            var updatedProduct = new ProductModel { Name = "NonExistent", Price = 99.99M };

            var result = _productService.UpdateProduct(999, updatedProduct);

            Assert.False(result);
        }

        [Fact]
        public void DeleteProduct_WhenIdExists_ReturnsTrueAndDeletesProduct()
        {
            var result = _productService.DeleteProduct(1);

            Assert.True(result);
            Assert.Null(_productService.GetProductById(1));
        }

        [Fact]
        public void DeleteProduct_WhenIdDoesNotExist_ReturnsFalse()
        {
            var result = _productService.DeleteProduct(999);

            Assert.False(result);
        }
    }
}
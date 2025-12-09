using System;
using System.Collections.Generic;
using System.Linq;
using MyApi.Models;
using MyApi.Services;
using Xunit;

namespace MyApi.Tests
{
    public class ProductServiceTests
    {
        private readonly ProductService _productService;

        public ProductServiceTests()
        {
            // Initialize the ProductService instance with default products
            _productService = new ProductService();
        }

        [Fact]
        public void Constructor_WhenCalled_SetsDefaultProducts()
        {
            var expectedCount = 2;
            Assert.Equal(expectedCount, _productService.GetProducts().Count());
        }

        [Theory]
        [InlineData(null, true)]
        [InlineData("name", true)]
        [InlineData("price", false)]
        public void GetProducts_WhenSortByIsProvided_ReturnsSortedProducts(string sortBy, bool ascending)
        {
            var products = _productService.GetProducts(sortBy, ascending).ToList();

            if (sortBy == "name")
            {
                Assert.Equal(ascending ? "Laptop" : "Smartphone", products.First().Name);
            }
            else if (sortBy == "price")
            {
                Assert.Equal(ascending ? 799.50M : 1200.99M, products.First().Price);
            }
        }

        [Fact]
        public void GetProducts_WhenNoSortByIsProvided_ReturnsUnsortedProducts()
        {
            var expectedIds = new[] { 1, 2 };
            var actualIds = _productService.GetProducts().Select(p => p.Id).ToArray();

            Assert.Equal(expectedIds, actualIds);
        }

        [Fact]
        public void GetProductById_WhenIdExists_ReturnsProduct()
        {
            var productId = 1;
            var product = _productService.GetProductById(productId);

            Assert.NotNull(product);
            Assert.Equal(productId, product.Id);
        }

        [Fact]
        public void GetProductById_WhenIdDoesNotExist_ReturnsNull()
        {
            var nonExistingProductId = 999;
            var product = _productService.GetProductById(nonExistingProductId);

            Assert.Null(product);
        }

        [Fact]
        public void GetProductByName_WhenNameExists_ReturnsProduct()
        {
            var productName = "Laptop";
            var product = _productService.GetProductByName(productName);

            Assert.NotNull(product);
            Assert.Equal(productName, product.Name);
        }

        [Fact]
        public void GetProductByName_WhenNameDoesNotExist_ReturnsNull()
        {
            var nonExistingProductName = "Unknown Product";
            var product = _productService.GetProductByName(nonExistingProductName);

            Assert.Null(product);
        }

        [Theory]
        [InlineData("Smartphone")]
        [InlineData("electronics")]
        public void SearchProducts_WhenKeywordMatches_ReturnsMatchingProducts(string keyword)
        {
            var products = _productService.SearchProducts(keyword).ToList();

            Assert.NotEmpty(products);
            Assert.All(products, p => p.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase) || p.Category?.Contains(keyword, StringComparison.OrdinalIgnoreCase) == true);
        }

        [Fact]
        public void SearchProducts_WhenKeywordDoesNotMatch_ReturnsNoProducts()
        {
            var nonMatchingKeyword = "NonExisting";
            var products = _productService.SearchProducts(nonMatchingKeyword).ToList();

            Assert.Empty(products);
        }

        [Theory]
        [InlineData("Electronics")]
        [InlineData("electronics")]
        public void FilterByCategory_WhenCategoryMatches_ReturnsFilteredProducts(string category)
        {
            var products = _productService.FilterByCategory(category).ToList();

            Assert.NotEmpty(products);
            Assert.All(products, p => p.Category?.Equals(category, StringComparison.OrdinalIgnoreCase) == true);
        }

        [Fact]
        public void FilterByCategory_WhenCategoryDoesNotMatch_ReturnsNoProducts()
        {
            var nonMatchingCategory = "NonExistingCategory";
            var products = _productService.FilterByCategory(nonMatchingCategory).ToList();

            Assert.Empty(products);
        }

        [Fact]
        public void CreateProduct_WhenValidProductProvided_AddsProductAndReturnsIt()
        {
            var newProduct = new ProductModel { Name = "New Laptop", Price = 1500.00M, Category = "Electronics", InStock = true };
            var createdProduct = _productService.CreateProduct(newProduct);

            Assert.NotNull(createdProduct);
            Assert.NotEqual(0, createdProduct.Id); // ID should be assigned
            Assert.Equal("New Laptop", createdProduct.Name);
            Assert.Equal(1500.00M, createdProduct.Price);
        }

        [Fact]
        public void UpdateProduct_WhenIdExists_ReturnsTrueAndUpdateProduct()
        {
            var existingProductId = 1;
            var updatedProduct = new ProductModel { Name = "Updated Laptop", Price = 1300.99M, Category = "Electronics", InStock = true };
            var updateResult = _productService.UpdateProduct(existingProductId, updatedProduct);

            Assert.True(updateResult);
            var product = _productService.GetProductById(existingProductId);
            Assert.NotNull(product);
            Assert.Equal("Updated Laptop", product.Name);
            Assert.Equal(1300.99M, product.Price);
        }

        [Fact]
        public void UpdateProduct_WhenIdDoesNotExist_ReturnsFalse()
        {
            var nonExistingProductId = 999;
            var updatedProduct = new ProductModel { Name = "Updated Laptop", Price = 1300.99M, Category = "Electronics", InStock = true };
            var updateResult = _productService.UpdateProduct(nonExistingProductId, updatedProduct);

            Assert.False(updateResult);
        }
    }
}
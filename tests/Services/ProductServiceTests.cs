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
        [InlineData("name", true)]
        [InlineData("price", false)]
        public void GetProducts_WhenSortByAndAscending_ReturnsSortedProducts(string sortBy, bool ascending)
        {
            // Act
            var products = _productService.GetProducts(sortBy, ascending);

            // Assert
            Assert.NotNull(products);
            if (sortBy == "name")
            {
                var orderedNames = ascending 
                    ? _productService._products.OrderBy(p => p.Name).Select(p => p.Name) 
                    : _productService._products.OrderByDescending(p => p.Name).Select(p => p.Name);
                Assert.Equal(orderedNames, products.Select(p => p.Name));
            }
            else if (sortBy == "price")
            {
                var orderedPrices = ascending
                    ? _productService._products.OrderBy(p => p.Price).Select(p => p.Price)
                    : _productService._products.OrderByDescending(p => p.Price).Select(p => p.Price);
                Assert.Equal(orderedPrices, products.Select(p => p.Price));
            }
        }

        [Fact]
        public void GetProducts_WhenInvalidSortBy_ReturnsUnsortedProducts()
        {
            // Act
            var products = _productService.GetProducts("invalid");

            // Assert
            Assert.NotNull(products);
            Assert.Equal(_productService._products.Select(p => p.Name), products.Select(p => p.Name));
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
        public void GetProductByName_WhenValidName_ReturnsCorrectProduct()
        {
            // Arrange
            string name = "Laptop";

            // Act
            var product = _productService.GetProductByName(name);

            // Assert
            Assert.NotNull(product);
            Assert.Equal(name, product.Name);
        }

        [Fact]
        public void GetProductByName_WhenInvalidName_ReturnsNull()
        {
            // Arrange
            string name = "NonExistentProduct";

            // Act
            var product = _productService.GetProductByName(name);

            // Assert
            Assert.Null(product);
        }

        [Fact]
        public void SearchProducts_WhenValidKeyword_ReturnsMatchingProducts()
        {
            // Arrange
            string keyword = "smart";

            // Act
            var products = _productService.SearchProducts(keyword).ToList();

            // Assert
            Assert.NotNull(products);
            Assert.Equal(1, products.Count);
            Assert.Equal("Smartphone", products[0].Name);
        }

        [Fact]
        public void SearchProducts_WhenInvalidKeyword_ReturnsEmptyList()
        {
            // Arrange
            string keyword = "NonExistent";

            // Act
            var products = _productService.SearchProducts(keyword).ToList();

            // Assert
            Assert.NotNull(products);
            Assert.Empty(products);
        }

        [Fact]
        public void FilterByCategory_WhenValidCategory_ReturnsMatchingProducts()
        {
            // Arrange
            string category = "Electronics";

            // Act
            var products = _productService.FilterByCategory(category).ToList();

            // Assert
            Assert.NotNull(products);
            Assert.Equal(2, products.Count);
            Assert.All(products, p => Assert.Equal(category, p.Category));
        }

        [Fact]
        public void FilterByCategory_WhenInvalidCategory_ReturnsEmptyList()
        {
            // Arrange
            string category = "NonExistent";

            // Act
            var products = _productService.FilterByCategory(category).ToList();

            // Assert
            Assert.NotNull(products);
            Assert.Empty(products);
        }

        [Fact]
        public void CreateProduct_WhenCalled_AddsProductWithNewId()
        {
            // Arrange
            var newProduct = new ProductModel { Name = "Tablet", Price = 349.99M, Category = "Electronics", InStock = true };

            // Act
            var createdProduct = _productService.CreateProduct(newProduct);

            // Assert
            Assert.NotNull(createdProduct);
            Assert.Equal(3, createdProduct.Id); // Assuming the next ID is 3 after initial 2 products
        }

        [Fact]
        public void UpdateProduct_WhenValidId_ReturnsUpdatedProduct()
        {
            // Arrange
            var updatedProduct = new ProductModel { Id = 1, Name = "Updated Laptop", Price = 999.99M, Category = "Electronics", InStock = true };

            // Act
            var result = _productService.UpdateProduct(updatedProduct);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Updated Laptop", result.Name);
        }

        [Fact]
        public void UpdateProduct_WhenInvalidId_ReturnsNull()
        {
            // Arrange
            var updatedProduct = new ProductModel { Id = 99, Name = "NonExistent Product", Price = 1.0M, Category = "Unknown", InStock = false };

            // Act
            var result = _productService.UpdateProduct(updatedProduct);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void DeleteProduct_WhenValidId_DeletesProduct()
        {
            // Arrange
            int idToDelete = 1;

            // Act
            bool isDeleted = _productService.DeleteProduct(idToDelete);

            // Assert
            Assert.True(isDeleted);
            var deletedProduct = _productService.GetProductById(idToDelete);
            Assert.Null(deletedProduct);
        }

        [Fact]
        public void DeleteProduct_WhenInvalidId_ReturnsFalse()
        {
            // Arrange
            int idToDelete = 99;

            // Act
            bool isDeleted = _productService.DeleteProduct(idToDelete);

            // Assert
            Assert.False(isDeleted);
        }
    }
}
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
            var products = _productService.GetProducts(sortBy, ascending).ToList();

            // Assert
            Assert.NotNull(products);
            if (sortBy == "name")
            {
                for (int i = 0; i < products.Count - 1; i++)
                {
                    if (ascending)
                        Assert.True(string.Compare(products[i].Name, products[i + 1].Name) <= 0);
                    else
                        Assert.True(string.Compare(products[i].Name, products[i + 1].Name) >= 0);
                }
            }

            if (sortBy == "price")
            {
                for (int i = 0; i < products.Count - 1; i++)
                {
                    if (ascending)
                        Assert.True(products[i].Price <= products[i + 1].Price);
                    else
                        Assert.True(products[i].Price >= products[i + 1].Price);
                }
            }
        }

        [Fact]
        public void GetProducts_WhenInvalidSortBy_ReturnsUnsortedProducts()
        {
            // Act
            var products = _productService.GetProducts("invalid").ToList();

            // Assert
            Assert.NotNull(products);
            Assert.Equal(2, products.Count());
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

        [Theory]
        [InlineData("Laptop")]
        [InlineData("Smartphone")]
        public void GetProductByName_WhenValidName_ReturnsCorrectProduct(string name)
        {
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
            string name = "NonExistent"; 

            // Act
            var product = _productService.GetProductByName(name);

            // Assert
            Assert.Null(product);
        }

        [Theory]
        [InlineData("lap")]
        [InlineData("smart")]
        public void SearchProducts_WhenValidKeyword_ReturnsMatchingProducts(string keyword)
        {
            // Act
            var products = _productService.SearchProducts(keyword).ToList();

            // Assert
            Assert.NotNull(products);
            Assert.True(products.All(p => p.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase) || p.Category?.Contains(keyword, StringComparison.OrdinalIgnoreCase) == true));
        }

        [Fact]
        public void SearchProducts_WhenNoMatchingKeyword_ReturnsEmptyList()
        {
            // Arrange
            string keyword = "NonExistent";

            // Act
            var products = _productService.SearchProducts(keyword).ToList();

            // Assert
            Assert.NotNull(products);
            Assert.Empty(products);
        }

        [Theory]
        [InlineData("Electronics")]
        public void FilterByCategory_WhenValidCategory_ReturnsMatchingProducts(string category)
        {
            // Act
            var products = _productService.FilterByCategory(category).ToList();

            // Assert
            Assert.NotNull(products);
            Assert.True(products.All(p => p.Category?.Equals(category, StringComparison.OrdinalIgnoreCase) == true));
        }

        [Fact]
        public void FilterByCategory_WhenNoMatchingCategory_ReturnsEmptyList()
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
        public void CreateProduct_WhenCalled_AddsNewProduct()
        {
            // Arrange
            var newProduct = new ProductModel { Name = "Tablet", Price = 299.99M, Category = "Electronics", InStock = true };

            // Act
            var createdProduct = _productService.CreateProduct(newProduct);

            // Assert
            Assert.NotNull(createdProduct);
            Assert.Equal(3, createdProduct.Id); // Assuming the last product id was 2
        }

        [Fact]
        public void UpdateProduct_WhenCalled_UpdatesExistingProduct()
        {
            // Arrange
            var updatedProduct = new ProductModel { Id = 1, Name = "Updated Laptop", Price = 999.99M, Category = "Electronics", InStock = true };

            // Act
            var result = _productService.UpdateProduct(updatedProduct);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void UpdateProduct_WhenInvalidId_ReturnsFalse()
        {
            // Arrange
            var updatedProduct = new ProductModel { Id = 99, Name = "NonExistent", Price = 0M, Category = "", InStock = false };

            // Act
            var result = _productService.UpdateProduct(updatedProduct);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void DeleteProduct_WhenValidId_ReturnsTrue()
        {
            // Arrange
            int idToDelete = 1;

            // Act
            var result = _productService.DeleteProduct(idToDelete);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void DeleteProduct_WhenInvalidId_ReturnsFalse()
        {
            // Arrange
            int invalidId = 99;

            // Act
            var result = _productService.DeleteProduct(invalidId);

            // Assert
            Assert.False(result);
        }
    }
}
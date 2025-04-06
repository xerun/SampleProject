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
        public void GetProducts_WhenSortBy_ReturnsSortedProducts(string sortBy, bool ascending)
        {
            // Act
            var products = _productService.GetProducts(sortBy, ascending);

            // Assert
            Assert.NotNull(products);
            switch (sortBy.ToLower())
            {
                case "name":
                    if (ascending)
                        Assert.True(products.SequenceEqual(products.OrderBy(p => p.Name)));
                    else
                        Assert.True(products.SequenceEqual(products.OrderByDescending(p => p.Name)));
                    break;
                case "price":
                    if (ascending)
                        Assert.True(products.SequenceEqual(products.OrderBy(p => p.Price)));
                    else
                        Assert.True(products.SequenceEqual(products.OrderByDescending(p => p.Price)));
                    break;
            }
        }

        [Fact]
        public void GetProductById_WhenValidId_ReturnsCorrectProduct()
        {
            // Act
            var product = _productService.GetProductById(1);

            // Assert
            Assert.NotNull(product);
            Assert.Equal(1, product.Id);
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
            // Act
            var product = _productService.GetProductByName("Laptop");

            // Assert
            Assert.NotNull(product);
            Assert.Equal("Laptop", product.Name);
        }

        [Fact]
        public void GetProductByName_WhenInvalidName_ReturnsNull()
        {
            // Arrange
            string name = "Tablet"; // Assuming there is no product with Name Tablet

            // Act
            var product = _productService.GetProductByName(name);

            // Assert
            Assert.Null(product);
        }

        [Fact]
        public void SearchProducts_WhenKeywordMatchesName_ReturnsMatchingProducts()
        {
            // Arrange
            string keyword = "Laptop";

            // Act
            var products = _productService.SearchProducts(keyword);

            // Assert
            Assert.NotNull(products);
            Assert.Equal(1, products.Count());
            Assert.Single(products.Where(p => p.Name.Equals("Laptop", StringComparison.OrdinalIgnoreCase)));
        }

        [Fact]
        public void SearchProducts_WhenKeywordMatchesCategory_ReturnsMatchingProducts()
        {
            // Arrange
            string keyword = "Electronics";

            // Act
            var products = _productService.SearchProducts(keyword);

            // Assert
            Assert.NotNull(products);
            Assert.Equal(2, products.Count());
            Assert.All(products, p => p.Category?.Equals("Electronics", StringComparison.OrdinalIgnoreCase) == true);
        }

        [Fact]
        public void SearchProducts_WhenKeywordDoesNotMatch_ReturnsNoProducts()
        {
            // Arrange
            string keyword = "Tablet"; // Assuming there is no product with name or category Tablet

            // Act
            var products = _productService.SearchProducts(keyword);

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
            var products = _productService.FilterByCategory(category);

            // Assert
            Assert.NotNull(products);
            Assert.Equal(2, products.Count());
            Assert.All(products, p => p.Category?.Equals("Electronics", StringComparison.OrdinalIgnoreCase) == true);
        }

        [Fact]
        public void FilterByCategory_WhenInvalidCategory_ReturnsNoProducts()
        {
            // Arrange
            string category = "HomeAppliances"; // Assuming there is no product with this category

            // Act
            var products = _productService.FilterByCategory(category);

            // Assert
            Assert.NotNull(products);
            Assert.Empty(products);
        }

        [Fact]
        public void CreateProduct_WhenCalled_AddsNewProduct()
        {
            // Arrange
            var newProduct = new ProductModel { Name = "Tablet", Price = 399.99M, Category = "Electronics", InStock = true };

            // Act
            var createdProduct = _productService.CreateProduct(newProduct);

            // Assert
            Assert.NotNull(createdProduct);
            Assert.Equal(3, createdProduct.Id); // Assuming previous products have IDs 1 and 2
            Assert.Contains(createdProduct, _productService.GetProducts());
        }

        [Fact]
        public void ProductService_WhenInitialized_HasTwoInitialProducts()
        {
            // Act
            var products = _productService.GetProducts();

            // Assert
            Assert.NotNull(products);
            Assert.Equal(2, products.Count());
        }
    }
using System;
using System.Collections.Generic;
using System.Linq;
using MyApi.Models;

namespace MyApi.Services
{
    public class ProductService : IProductService
    {
        private readonly List<ProductModel> _products = new()
        {
            new ProductModel { Id = 1, Name = "Laptop", Price = 1200.99M, Category = "Electronics", InStock = true },
            new ProductModel { Id = 2, Name = "Smartphone", Price = 799.50M, Category = "Electronics", InStock = true }
        };

        private int _nextId => _products.Any() ? _products.Max(p => p.Id) + 1 : 1;

        public ProductModel? GetProductById(int id)
        {
            return _products.FirstOrDefault(p => p.Id == id);
        }

        public ProductModel? GetProductByName(string name)
        {
            return _products.FirstOrDefault(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public IEnumerable<ProductModel> SearchProducts(string keyword)
        {
            return _products.Where(p =>
                p.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                p.Category?.Contains(keyword, StringComparison.OrdinalIgnoreCase) == true
            );
        }

        public IEnumerable<ProductModel> FilterByCategory(string category)
        {
            return _products.Where(p => p.Category?.Equals(category, StringComparison.OrdinalIgnoreCase) == true);
        }

        public ProductModel CreateProduct(ProductModel product)
        {
            product.Id = _nextId;
            _products.Add(product);
            return product;
        }

        public bool UpdateProduct(int id, ProductModel updatedProduct)
        {
            var index = _products.FindIndex(p => p.Id == id);
            if (index == -1) return false;

            updatedProduct.Id = id;
            _products[index] = updatedProduct;
            return true;
        }
    }
}

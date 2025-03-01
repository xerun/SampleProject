using System.Collections.Generic;
using System.Linq;
using MyApi.Models;

namespace MyApi.Services
{
    public class ProductService : IProductService
    {
        private readonly List<ProductModel> _products = new()
        {
            new ProductModel { Id = 1, Name = "Laptop", Price = 1200.99M },
            new ProductModel { Id = 2, Name = "Smartphone", Price = 799.50M }
        };

        public IEnumerable<ProductModel> GetProducts()
        {
            return _products;
        }

        public ProductModel GetProductById(int id)
        {
            return _products.FirstOrDefault(p => p.Id == id);
        }
    }
}

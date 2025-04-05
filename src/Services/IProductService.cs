using System.Collections.Generic;
using MyApi.Models;

namespace MyApi.Services
{
    public interface IProductService
    {
        IEnumerable<ProductModel> GetProducts(string? sortBy = null, bool ascending = true);
        ProductModel? GetProductById(int id);
        ProductModel? GetProductByName(string name);
        IEnumerable<ProductModel> SearchProducts(string keyword);
        IEnumerable<ProductModel> FilterByCategory(string category);
        ProductModel CreateProduct(ProductModel product);
        bool UpdateProduct(int id, ProductModel updatedProduct);
        bool DeleteProduct(int id);
    }
}

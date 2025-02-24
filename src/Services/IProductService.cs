using System.Collections.Generic;
using MyApi.Models;

namespace MyApi.Services
{
    public interface IProductService
    {
        IEnumerable<ProductModel> GetProducts();
        ProductModel GetProductById(int id);
    }
}

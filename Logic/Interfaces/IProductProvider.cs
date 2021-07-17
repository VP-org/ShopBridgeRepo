using Logic.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Interfaces
{
    public interface IProductProvider
    {
        Task<List<ProductModel>> GetProducts();
        Task<ProductModel> LoadProduct(int productId);
        Task SaveProduct(ProductModel model);
        Task DeleteProduct(int productId);
    }
}

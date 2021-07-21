using DataService;
using Logic.Exceptions;
using Logic.Interfaces;
using Logic.Models;
using Logic.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Providers
{
    public class ProductProvider:BaseProvider,IProductProvider
    {
        public ProductProvider(Lazy<Shop_Bridge_dbContext> db) : base(db)
        {

        }
        public async Task SaveProduct(ProductModel model)
        {
            var flag = await _db.Products.AnyAsync(a => a.Id != model.Id && a.Name == model.Name);
            if (flag)
            {
                throw new ProductAlreadyExistsException(model.Name);
            }

            if (model.Id != 0)
            {
                var product = _db.Products.Where(a => a.Id == model.Id).FirstOrDefault();
                if (product == null)
                {
                    throw new ProductNotFoundException(model.Id);
                }
                product.Name = model.Name;
                product.Description = model.Description;
                product.Price = model.Price;
            }
            else 
            {
                var product = new Product
                {
                    Id = model.Id,
                    Name = model.Name,
                    Description = model.Description,
                    Price = model.Price,
                    CreatedBy = model.CreatedBy,
                    CreatedDate = DateTime.Now
                };
                _db.Products.Add(product);
            }
            
            
            await _db.SaveChangesAsync();
        }

        public async Task<List<ProductModel>> GetProducts(int pageNumber, int pageSize)
        {
            return await _db.Products
                .Skip((pageNumber-1)*pageSize)
                .Take(pageSize)
                .Project().ToListAsync();
        }

        public async Task<ProductModel> LoadProduct(int productId)
        {
            var model= await _db.Products.Where(a => a.Id == productId).Project().FirstOrDefaultAsync();
            if (model == null)
                throw new ProductNotFoundException(productId);

            return model;
        }

        public async Task DeleteProduct(int productId)
        {
            var model=await _db.Products.Where(a => a.Id == productId).FirstOrDefaultAsync();
            if (model == null)
            {
                throw new ProductNotFoundException(productId);
            }
            else
            {
                _db.Products.Remove(model);
                await _db.SaveChangesAsync();
            }
        }

    }
}

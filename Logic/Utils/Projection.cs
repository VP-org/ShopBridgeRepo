using DataService;
using Logic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic.Utils
{
    public static class Projection
    {
        public static IQueryable<ProductModel> Project(this IQueryable<Product> query)
        {
            return query.Select(a => new ProductModel
            {
                Id = a.Id,
                Name = a.Name,
                Description = a.Description,
                Price = a.Price,
                CreatedBy=a.CreatedBy,
                CreatedDate=a.CreatedDate

            }).AsQueryable();
        }

    }
}

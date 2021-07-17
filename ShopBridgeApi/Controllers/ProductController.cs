using Logic.Exceptions;
using Logic.Interfaces;
using Logic.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopBridgeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : BaseController
    {
        public ProductController(Lazy<IProductProvider> productProvider)
        {
            _lazyProductProvider = productProvider;
        }

        [HttpGet("[action]")]
        public async Task<GetProductsResponse> GetProducts()
        {
            var response = new GetProductsResponse();
            response.Message = "";

            try
            {
                var result = await _productProvider.GetProducts();

                response.productModels = result;
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "GetProducts failed due to error " + ex.Message;
            }

            return response;
        }

        [HttpGet("[action]/{id}")]
        public async Task<LoadProductResponse> LoadProduct([FromRoute] int id)
        {
            var response = new LoadProductResponse();
            response.Message = "";

            try
            {
                var result = await _productProvider.LoadProduct(id);

                response.model = result;
                response.Success = true;
            }
            catch (ProductNotFoundException ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Fetching Product failed due to error " + ex.Message;
            }

            return response;
        }

        [HttpDelete("[action]/{id}")]
        public async Task<BaseResponse> DeleteProduct([FromRoute] int id)
        {
            var response = new BaseResponse();
            response.Message = "";

            try
            {
                await _productProvider.DeleteProduct(id);

                response.Message = "Successfully deleted product";
                response.Success = true;
            }
            catch (ProductNotFoundException ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Product delete failed due to error " + ex.Message;
            }

            return response;
        }

        [HttpPost("[action]")]
        public async Task<BaseResponse> SaveProduct([FromBody] SaveProductRequest request)
        {
            var response = new BaseResponse();
            response.Message = "";

            try
            {
                if (request==null || request.model == null || string.IsNullOrEmpty(request.model.Name))
                {
                    response.Success = false;
                    response.Message = "Invalid data. Please try again";
                    return response;
                }

                await _productProvider.SaveProduct(request.model);

                response.Message = "Successfully saved product";
                response.Success = true;
            }
            catch (ProductAlreadyExistsException ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            catch (ProductNotFoundException ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Product save failed due to error "+ex.Message;
            }

            return response;
        }

    }

    public class BaseResponse
    {
        public string Message { get; set; }
        public bool Success { get; set; }
    }
    public class GetProductsResponse : BaseResponse
    {
        public List<ProductModel> productModels { get; set; }
    }

    public class LoadProductResponse : BaseResponse
    {
        public ProductModel model { get; set; }
    }

    public class SaveProductRequest
    {
        public ProductModel model { get; set; }
    }
}

using Logic.Exceptions;
using Logic.Interfaces;
using Logic.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using ShopBridgeApi.Controllers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApiTest.Controllers
{
    public class ProductControllerTest
    {
        private List<ProductModel> _products = null;
        private ProductController _controller;
        private ServiceProvider _serviceProvider;

        [SetUp]
        public void Setup()
        {
            var services = new ServiceCollection();
            _serviceProvider = services.BuildServiceProvider();
            _products = new List<ProductModel> {
                     new ProductModel{ Id=1,Name="Shirt",Description="Polo Shirt",Price=900 },
                     new ProductModel{ Id=2,Name="T-Shirt",Description="Rugger T-Shirt",Price=600 },
                     new ProductModel{ Id=3,Name="Jean",Description="Levis Jean",Price=1200 },
                     new ProductModel{ Id=4,Name="Cap",Description="Sports Cap",Price=300 },
           };

            Mock<IProductProvider> provider = new Mock<IProductProvider>();
            provider.Setup(k => k.GetProducts(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(_products);
            provider.Setup(k => k.LoadProduct(It.IsAny<int>())).ReturnsAsync(_products[0]);
            provider.Setup(k => k.SaveProduct(It.IsAny<ProductModel>())).Returns(Task.FromResult(true));
            provider.Setup(k => k.DeleteProduct(It.IsAny<int>())).Returns(Task.FromResult(true));
            _controller = new ProductController(new Lazy<IProductProvider>(() => provider.Object));
            SetUpController(_controller);
        }

        private void SetUpController(ProductController controller)
        {
            controller.ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext() { HttpContext = new DefaultHttpContext() };
            controller.ControllerContext.HttpContext.RequestServices = _serviceProvider;
        }

        [Test]
        public async Task GetProductsValid()
        {
            GetProductsRequest data = new GetProductsRequest() { pageNumber = 2, pageSize = 2 };
            GetProductsResponse response = await _controller.GetProducts(data);
            Assert.IsTrue(response.Success);
            Assert.AreEqual(_products.Count, response.productModels.Count);
        }

        [Test]
        public async Task LoadProductValid()
        {

            LoadProductResponse response = await _controller.LoadProduct(1);
            Assert.IsTrue(response.Success);
            Assert.AreEqual(_products[0].Name, response.model.Name);
        }

        [Test]
        public async Task LoadProductNotFound()
        {
            Mock<IProductProvider> provider = new Mock<IProductProvider>();
            provider.Setup(k => k.LoadProduct(It.IsAny<int>())).Throws<ProductNotFoundException>();
            var controller= new ProductController(new Lazy<IProductProvider>(() => provider.Object));
            SetUpController(controller);


            LoadProductResponse response = await controller.LoadProduct(9);
            Assert.IsFalse(response.Success);
            Assert.AreEqual("Product not found", response.Message);
        }

        [Test]
        public async Task SaveProductValid()
        {
            SaveProductRequest request = new SaveProductRequest();
            request.model = new ProductModel { Id = 7, Name = "Shoe", Description = "Reebok Shoe", Price = 1500, CreatedBy = "Vish"};
            var response = await _controller.SaveProduct(request);
            Assert.IsTrue(response.Success);
        }

        [Test]
        public async Task SaveProductDuplicate()
        {
            Mock<IProductProvider> provider = new Mock<IProductProvider>();
            provider.Setup(k => k.SaveProduct(It.IsAny<ProductModel>())).Throws<ProductAlreadyExistsException>();
            var controller = new ProductController(new Lazy<IProductProvider>(() => provider.Object));
            SetUpController(controller);

            SaveProductRequest request = new SaveProductRequest();
            request.model = new ProductModel { Id = 7, Name = "Shirt", Description = "Shirt", Price = 1500, CreatedBy = "Vish" };
            var response = await controller.SaveProduct(request);
            Assert.IsFalse(response.Success);
            Assert.AreEqual("Product already exists", response.Message);
        }

        [Test]
        public async Task SaveProductInvalid()
        {
            SaveProductRequest request = new SaveProductRequest();
            request.model = new ProductModel { Id = 7, Name = "", Description = "Shirt", Price = 1500, CreatedBy = "Vish" };
            var response = await _controller.SaveProduct(request);
            Assert.IsFalse(response.Success);
            Assert.AreEqual("Invalid data. Please try again", response.Message);
        }

        [Test]
        public async Task SaveProductNotFound()
        {
            Mock<IProductProvider> provider = new Mock<IProductProvider>();
            provider.Setup(k => k.SaveProduct(It.IsAny<ProductModel>())).Throws<ProductNotFoundException>();
            var controller = new ProductController(new Lazy<IProductProvider>(() => provider.Object));
            SetUpController(controller);

            SaveProductRequest request = new SaveProductRequest();
            request.model = new ProductModel { Id = 9, Name = "Jacket", Description = "Shirt", Price = 1500, CreatedBy = "Vish" };
            var response = await controller.SaveProduct(request);
            Assert.IsFalse(response.Success);
            Assert.AreEqual("Product not found", response.Message);
        }

        [Test]
        public async Task DeleteProductValid()
        {
            var response = await _controller.DeleteProduct(1);
            Assert.IsTrue(response.Success);
        }

        [Test]
        public async Task DeleteProductNotFound()
        {
            Mock<IProductProvider> provider = new Mock<IProductProvider>();
            provider.Setup(k => k.DeleteProduct(It.IsAny<int>())).Throws<ProductNotFoundException>();
            var controller = new ProductController(new Lazy<IProductProvider>(() => provider.Object));
            SetUpController(controller);

            var response = await controller.DeleteProduct(9);
            Assert.IsFalse(response.Success);
            Assert.AreEqual("Product not found", response.Message);
        }
    }
}

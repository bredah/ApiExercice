using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Net;
using WebApi.Controllers;
using WebApi.Data;
using WebApi.Models;
using WebApi.Services;
using Xunit;

namespace WebApi.Tests.Controlers
{
    public class ProductsV1ControllerTest
    {
        private readonly ProductsV1Controller _controller;
        private List<Product> _products;

        public ProductsV1ControllerTest()
        {

            _products = new List<Product>()
            {
                new Product(){ Id = 1, ProductName = "Mouse",Price = 10.00M },
                new Product(){ Id = 2, ProductName = "Keyboard", Price = 20.00M },
                new Product(){ Id = 3, ProductName = "Notebook", Price = 200.00M }
            };

            //var options = new DbContextOptionsBuilder<ProductsDbContext>()
            //.UseInMemoryDatabase(databaseName: "bookstore")
            //.Options;

            //var context = new ProductsDbContext(options);

            //var repository = new ProductRepository(context);

            //_controller = new ProductsV1Controller(repository);

            //Seed(context);
        }

        [Fact]
        public void Get_All()
        {
            // Prepare the mock
            var products = new List<Product>()
            {
                new Product(){ Id = 1, ProductName = "Mouse",Price = 10.00M },
                new Product(){ Id = 2, ProductName = "Keyboard", Price = 20.00M },
                new Product(){ Id = 3, ProductName = "Notebook", Price = 200.00M }
            };

            var mockRepository = new Mock<IProduct>();
            mockRepository
                .Setup(x => x.GetProducts(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(products);
            var controller = new ProductsV1Controller(mockRepository.Object);

            // Make the request
            var response = controller.Get() as ObjectResult;

            // Validate the response
            Assert.Equal((int)HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(response);
            Assert.NotNull(response.Value);
            Assert.IsType<Product>(response.Value);
        }

        [Fact]
        public void Get_ById()
        {
            // Prepare the mock
            var mockRepository = new Mock<IProduct>();
            mockRepository.Setup(x => x.GetProduct(_products[0].Id))
                 .Returns(_products[0]);
            var controller = new ProductsV1Controller(mockRepository.Object);

            // Make the request
            var response = controller.Get(1) as OkObjectResult;

            // Validate the response
            Assert.Equal((int)HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(response);
            Assert.NotNull(response.Value);
            Assert.IsType<Product>(response.Value);
        }

        [Fact]
        public void Get_ById_NotFound()
        {
            // Prepare the mock
            var mockRepository = new Mock<IProduct>();
            var controller = new ProductsV1Controller(mockRepository.Object);

            // Make the request
            var response = controller.Get(1) as ObjectResult;

            // Validate the response
            Assert.Equal((int)HttpStatusCode.NotFound, response.StatusCode);
            Assert.NotNull(response);
            Assert.NotNull(response.Value);
            Assert.Equal("Product not found", response.Value);
        }


        [Fact]
        public void Post()
        {
            var product = new Product()
            {
                ProductName = "Product",
                Price = 600.00M
            };

            // Prepare the mock
            var mockRepository = new Mock<IProduct>();
            mockRepository.Setup(x => x.AddProduct(product))
                 .Returns(1);
            mockRepository.Setup(x => x.GetProduct(product.Id))
                 .Returns(product);

            var controller = new ProductsV1Controller(mockRepository.Object);

            // Make the request
            var response = controller.Post(product) as OkObjectResult;

            // Validate the response
            Assert.Equal((int)HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(response);
            Assert.NotNull(response.Value);
            Assert.IsType<Product>(response.Value);
            Assert.Equal(product, response.Value);
        }


        [Fact]
        public void Put()
        {
            //var product = new Product()
            //{
            //    Id = 3,
            //    ProductName = $"Product Modified",
            //    Price = 1000.00M
            //};
            //// Post the request and capture the return
            //var response = await Client.PutAsync($"{basePath}/{product.Id}",
            //    new StringContent(JsonConvert.SerializeObject(product),
            //        encoding: Encoding.UTF8,
            //        mediaType: "application/json")
            //);
            //// Check the response status code
            //Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);
        }




        [Fact]
        public void Delete()
        {
            // Prepare the mock
            var mockRepository = new Mock<IProduct>();
            mockRepository.Setup(x => x.DeleteProduct(1))
                 .Returns(1);
            var controller = new ProductsV1Controller(mockRepository.Object);

            // Make the request
            var response = controller.Delete(1) as OkObjectResult;

            // Validate the response
            Assert.Equal((int)HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(response);
            Assert.NotNull(response.Value);
            Assert.Equal("Product is removed", response.Value);
        }

        [Fact]
        public void Delete_NotFound()
        {
            // Prepare the mock
            var mockRepository = new Mock<IProduct>();
            var controller = new ProductsV1Controller(mockRepository.Object);

            // Make the request
            var response = controller.Delete(1) as ObjectResult;

            // Validate the response 
            Assert.Equal((int)HttpStatusCode.NotFound, response.StatusCode);
            Assert.NotNull(response);
            Assert.NotNull(response.Value);
            Assert.Equal("No record found against with id", response.Value);
        }




        private void Seed(ProductsDbContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            _products = new List<Product>()
            {
                new Product(){ Id = 1, ProductName = "Mouse",Price = 10.00M },
                new Product(){ Id = 2, ProductName = "Keyboard", Price = 20.00M },
                new Product(){ Id = 3, ProductName = "Notebook", Price = 200.00M }
            };
            context.Products.AddRange(_products);
            context.SaveChanges(true);
        }

    }
}

using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApi.Controllers.v1;
using WebApi.Models;
using WebApi.Services;
using Xunit;

namespace WebApi.Tests.Controllers.v1 {
    public class ProductsControllerTest {
        private readonly List<Product> _products;

        public ProductsControllerTest () {

            _products = new List<Product> () {
                new Product () { Id = 1, ProductName = "Mouse", Price = 10.00M },
                new Product () { Id = 2, ProductName = "Keyboard", Price = 20.00M },
                new Product () { Id = 3, ProductName = "Notebook", Price = 200.00M }
            };
        }

        [Fact]
        public void Get_All () {
            // Prepare the mock
            var mockRepository = new Mock<IProduct> ();
            mockRepository
                .Setup (x => x.GetProducts (It.IsAny<string> (), It.IsAny<string> (), It.IsAny<int> (), It.IsAny<int> ()))
                .Returns (_products);
            var controller = new ProductsController (mockRepository.Object);

            // Make the request
            var response = controller.Get () as OkObjectResult;

            // Validate the response
            Assert.Equal ((int) HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull (response);
            Assert.NotNull (response.Value);
            var items = Assert.IsType<List<Product>> (response.Value);
            // Check if the content has the same size
            Assert.Equal (_products.Count, items.Count);
            // Check if the content has the same values
            Assert.Equal (_products, items);
        }

        [Fact]
        public void Get_ById () {
            // Prepare the mock
            var mockRepository = new Mock<IProduct> ();
            mockRepository.Setup (x => x.GetProduct (_products[0].Id))
                .Returns (_products[0]);
            var controller = new ProductsController (mockRepository.Object);

            // Make the request
            var response = controller.Get (1) as OkObjectResult;

            // Validate the response
            Assert.Equal ((int) HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull (response);
            Assert.NotNull (response.Value);
            Assert.IsType<Product> (response.Value);
        }

        [Fact]
        public void Get_ById_NotFound () {
            // Prepare the mock
            var mockRepository = new Mock<IProduct> ();
            var controller = new ProductsController (mockRepository.Object);

            // Make the request
            var response = controller.Get (1) as ObjectResult;

            // Validate the response
            Assert.Equal ((int) HttpStatusCode.NotFound, response.StatusCode);
            Assert.NotNull (response);
            Assert.NotNull (response.Value);
            Assert.Equal ("Product not found", response.Value);
        }

        [Fact]
        public void Post () {
            var product = new Product () {
                ProductName = "Product",
                Price = 600.00M
            };
            // Prepare the mock
            var mockRepository = new Mock<IProduct> ();
            mockRepository.Setup (x => x.AddProduct (product))
                .Returns (1);
            mockRepository.Setup (x => x.GetProduct (product.Id))
                .Returns (product);

            var controller = new ProductsController (mockRepository.Object);
            // Make the request
            var response = controller.Post (product) as CreatedAtActionResult;
            // Validate the response
            Assert.Equal ((int) HttpStatusCode.Created, response.StatusCode);
            Assert.NotNull (response);
            Assert.NotNull (response.Value);
            Assert.IsType<Product> (response.Value);
            Assert.Equal (product, response.Value);
        }

        [Theory, MemberData (nameof (PostBadRequest))]
        public void Post_BadRequest (Product product, string field, string messageError) {
            // Prepare the mock
            var mockRepository = new Mock<IProduct> ();
            var controller = new ProductsController (mockRepository.Object);
            controller.ModelState.AddModelError (field, messageError);
            // Make the request
            var response = controller.Post (product) as ObjectResult;
            // Validate the response
            Assert.Equal ((int) HttpStatusCode.BadRequest, response.StatusCode);
            Assert.IsType<SerializableError> (response.Value);
        }

        [Fact]
        public void Put () {
            var product = new Product () {
                Id = 3,
                ProductName = $"Product Modified",
                Price = 1000.00M
            };
            // Prepare the mock
            var mockRepository = new Mock<IProduct> ();
            mockRepository.Setup (x => x.UpdateProduct (product))
                .Returns (1);
            var controller = new ProductsController (mockRepository.Object);

            // Make the request
            var response = controller.Put (product.Id, product) as AcceptedAtActionResult;

            // Validate the response
            Assert.Equal ((int) HttpStatusCode.Accepted, response.StatusCode);
            Assert.NotNull (response);
            Assert.NotNull (response.Value);
            Assert.IsType<Product> (response.Value);
            Assert.Equal (product, response.Value);
        }

        [Theory, MemberData (nameof (PutModelStateBadRequest))]
        public void Put_BadRequest_ModelState (int id, Product product, string field, string messageError) {
            // Prepare the mock
            var mockRepository = new Mock<IProduct> ();
            var controller = new ProductsController (mockRepository.Object);
            controller.ModelState.AddModelError (field, messageError);
            // Make the request
            var response = controller.Put (id, product) as ObjectResult;
            // Validate the response
            Assert.Equal ((int) HttpStatusCode.BadRequest, response.StatusCode);
            Assert.IsType<SerializableError> (response.Value);
        }

        [Fact]
        public void Put_BadRequest_InvalidId () {
            var product = new Product () {
                Id = 3,
                ProductName = $"Product Modified",
                Price = 1000.00M
            };
            // Prepare the mock
            var mockRepository = new Mock<IProduct> ();
            var controller = new ProductsController (mockRepository.Object);
            // Make the request
            var response = controller.Put (product.Id + 1, product) as ObjectResult;
            // Validate the response
            Assert.Equal ((int) HttpStatusCode.BadRequest, response.StatusCode);
            Assert.NotNull (response);
            Assert.NotNull (response.Value);
        }

        [Fact]
        public void Put_NotFound () {
            var product = new Product () {
                Id = 3,
                ProductName = $"Product Modified",
                Price = 1000.00M
            };
            // Prepare the mock
            var mockRepository = new Mock<IProduct> ();
            mockRepository.Setup (x => x.UpdateProduct (product))
                .Throws (new System.Exception ());
            var controller = new ProductsController (mockRepository.Object);
            // Make the request
            var response = controller.Put (product.Id, product) as ObjectResult;
            // Validate the response
            Assert.Equal ((int) HttpStatusCode.NotFound, response.StatusCode);
            Assert.NotNull (response);
            Assert.NotNull (response.Value);
        }

        [Fact]
        public void Delete () {
            // Prepare the mock
            var mockRepository = new Mock<IProduct> ();
            mockRepository.Setup (x => x.DeleteProduct (1))
                .Returns (1);
            var controller = new ProductsController (mockRepository.Object);

            // Make the request
            var response = controller.Delete (1) as OkObjectResult;

            // Validate the response
            Assert.Equal ((int) HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull (response);
            Assert.NotNull (response.Value);
            Assert.Equal ("Product is removed", response.Value);
        }

        [Fact]
        public void Delete_NotFound () {
            // Prepare the mock
            var mockRepository = new Mock<IProduct> ();
            var controller = new ProductsController (mockRepository.Object);

            // Make the request
            var response = controller.Delete (1) as ObjectResult;

            // Validate the response 
            Assert.Equal ((int) HttpStatusCode.NotFound, response.StatusCode);
            Assert.NotNull (response);
            Assert.NotNull (response.Value);
            Assert.Equal ("No record found against with id", response.Value);
        }

        [Fact]
        public void Delete_BadRequest () {
            // Prepare the mock
            var mockRepository = new Mock<IProduct> ();
            mockRepository.Setup (x => x.DeleteProduct (1))
                .Throws (new System.Exception ());
            var controller = new ProductsController (mockRepository.Object);

            // Make the request
            var response = controller.Delete (1) as ObjectResult;

            // Validate the response 
            Assert.Equal ((int) HttpStatusCode.BadRequest, response.StatusCode);
            Assert.NotNull (response);
            Assert.NotNull (response.Value);
        }

        public static IEnumerable<object[]> PostBadRequest =>
            new List<object[]> {
                new object[] { new Product (), "ProductName", "ProductName is required" },
                new object[] { new Product () { Id = 5, ProductName = $"", Price = 500.00M }, "ProductName", "ProductName is required" },
                new object[] { new Product () { Id = 5, ProductName = $"New Product" }, "Price", "Price is required" }
            };

        public static IEnumerable<object[]> PutModelStateBadRequest =>
            new List<object[]> {
                new object[] { 5, new Product (), "ProductName", "ProductName is required" },
                new object[] { 5, new Product () { Id = 5, ProductName = $"", Price = 500.00M }, "ProductName", "ProductName is required" },
            };
    }
}
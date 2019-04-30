using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebApi.Models;
using Xunit;

namespace WebApi.Integration.Tests.Controller
{
    public class ProductsV1ControllerTest : IClassFixture<WebApplicationFactory<Startup>>,
        IClassFixture<ProductsControllerFixture>, IDisposable
    {
        private HttpClient Client { get; }

        private const string apiVersion = "1";
        private readonly string basePath;

        public ProductsV1ControllerTest(WebApplicationFactory<Startup> factory)
        {
            basePath = $"api/v{apiVersion}/products";
            // Create a client using the main server app
            Client = factory.CreateClient();
        }

        public void Dispose()
        {
            Client?.Dispose();
            GC.SuppressFinalize(this);
        }

        [Fact]
        public async Task Get_All()
        {
            var response = await Client.GetAsync($"{basePath}");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            // Validate response content
            var json = await response.Content.ReadAsStringAsync();
            Assert.True(JArray.Parse(json).Count > 0, "DB has no record");
        }

        [Fact]
        public async Task Get_All_InvalidUrl()
        {
            var response = await Client.GetAsync($"api/product");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Get_ById()
        {
            var response = await Client.GetAsync($"{basePath}/1");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var json = await response.Content.ReadAsStringAsync();
            Assert.True(JObject.Parse(json).Count > 0, "DB has no record");
        }

        [Fact]
        public async Task Get_ById_NotFound()
        {
            var response = await Client.GetAsync($"{basePath}/99");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Post()
        {
            var product = new Product()
            {
                ProductName = $"New Product",
                Price = "500.00"
            };
            // Post the request and capture the return
            var response = await Client.PostAsync($"{basePath}",
                new StringContent(JsonConvert.SerializeObject(product),
                    encoding: Encoding.UTF8,
                    mediaType: "application/json")
            );
            // Check the response status code
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Theory, MemberData(nameof(PostBadRequest))]
        public async Task Post_BadRequest(Product product, string expectedMessage)
        {
            // Post the request and capture the return
            var response = await Client.PostAsync($"{basePath}",
                new StringContent(JsonConvert.SerializeObject(product),
                    encoding: Encoding.UTF8,
                    mediaType: "application/json")
            );
            // Check the response status code
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            // Check the response content
            var responseContent = await response.Content.ReadAsStringAsync();
            Assert.True(responseContent.Contains(expectedMessage),
                $"Received: {responseContent} - Expected: {expectedMessage}");
        }

        [Fact]
        public async Task Put()
        {
            var product = new Product()
            {
                Id = 3,
                ProductName = $"Product Modified",
                Price = "1000.00"
            };
            // Post the request and capture the return
            var response = await Client.PutAsync($"{basePath}/{product.Id}",
                new StringContent(JsonConvert.SerializeObject(product),
                    encoding: Encoding.UTF8,
                    mediaType: "application/json")
            );
            // Check the response status code
            Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);
        }

        [Theory, MemberData(nameof(PutBadRequest))]
        public async Task Put_BadRequest(int productId, Product product, string expectedMessage)
        {
            // Post the request and capture the return
            var response = await Client.PutAsync($"{basePath}/{productId}",
                new StringContent(JsonConvert.SerializeObject(product),
                    encoding: Encoding.UTF8,
                    mediaType: "application/json")
            );
            // Check the response status code
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            // Check the response content
            var responseContent = await response.Content.ReadAsStringAsync();
            Assert.True(responseContent.Contains(expectedMessage),
                $"Received: {responseContent} - Expected: {expectedMessage}");
        }

        [Fact]
        public async Task Put_NotFound()
        {
            var product = new Product()
            {
                Id = 1001,
                ProductName = "Product Modified",
                Price = "1000.00"
            };
            // Post the request and capture the return
            var response = await Client.PutAsync($"{basePath}/{product.Id}",
                new StringContent(JsonConvert.SerializeObject(product),
                    encoding: Encoding.UTF8,
                    mediaType: "application/json")
            );
            // Check the response status code
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Delete()
        {
            // Post the request and capture the return
            var response = await Client.DeleteAsync($"{basePath}/2");
            // Check the response status code
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Delete_NotFound()
        {
            // Post the request and capture the return
            var response = await Client.DeleteAsync($"{basePath}/50");
            // Check the response status code
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        public static IEnumerable<object[]> PostBadRequest =>
            new List<object[]>
            {
                new object[]
                    {new Product(), "ProductName is required"},
                new object[]
                    {new Product() {Id = 5, ProductName = $"", Price = "500.00"}, "ProductName is required"},
                new object[]
                    {new Product() {Id = 5, ProductName = $"New Product", Price = ""}, "Price is required"},
                new object[]
                    {new Product() {Id = 5, ProductName = $"New Product", Price = "500"}, "Invalid price value"},
            };

        public static IEnumerable<object[]> PutBadRequest =>
            new List<object[]>
            {
                new object[]
                    {5, new Product(), "ProductName is required"},
                new object[]
                    {5, new Product() {Id = 5, ProductName = $"", Price = "500,00"}, "ProductName is required"},
                new object[]
                    {5, new Product() {Id = 5, ProductName = $"New Product", Price = ""}, "Price is required"},
                new object[]
                    {5, new Product() {Id = 5, ProductName = $"New Product", Price = "500"}, "Invalid price value"},
                new object[]
                    {6, new Product() {Id = 5, ProductName = $"New Product", Price = "500.00"}, "Check the product id"},
            };
    }
}
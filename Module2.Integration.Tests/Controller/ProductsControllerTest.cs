using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Module2.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Module2.Integration.Tests.Controller
{
    public class ProductsControllerTest : IClassFixture<WebApplicationFactory<Startup>>,
        IClassFixture<ProductsControllerFixture>, IDisposable
    {
        private HttpClient Client { get; }


        public ProductsControllerTest(WebApplicationFactory<Startup> factory)
        {
            // Create a client using the main server app
            Client = factory.CreateClient();
        }

        public void Dispose()
        {
            Client?.Dispose();
        }

        [Fact]
        public async Task Get_All()
        {
            var response = await Client.GetAsync($"api/products");
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
            var response = await Client.GetAsync($"api/products/1");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var json = await response.Content.ReadAsStringAsync();
            Assert.True(JObject.Parse(json).Count > 0, "DB has no record");
        }

        [Fact]
        public async Task Get_ById_NotFound()
        {
            var response = await Client.GetAsync($"api/products/6");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Post_AddProduct()
        {
            var product = new Product()
            {
                Id = 4,
                ProductName = $"New Product",
                Price = "500,00"
            };
            // Post the request and capture the return
            var response = await Client.PostAsync($"api/products",
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
            var response = await Client.PostAsync($"api/products",
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
        public async Task Put_UpdateProduct()
        {
            var product = new Product()
            {
                Id = 3,
                ProductName = $"Product Modified",
                Price = "1000,00"
            };
            // Post the request and capture the return
            var response = await Client.PutAsync($"api/products/{product.Id}",
                new StringContent(JsonConvert.SerializeObject(product),
                    encoding: Encoding.UTF8,
                    mediaType: "application/json")
            );
            // Check the response status code
            Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);
        }

        [Fact]
        public async Task Delete_RemoveAProduct()
        {
            // Post the request and capture the return
            var response = await Client.DeleteAsync($"api/products/2");
            // Check the response status code
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        
        public static IEnumerable<object[]> PostBadRequest =>
            new List<object[]>
            {
                new object[]
                    {new Product() {Id = 5, ProductName = $"", Price = "500,00"}, "ProductName is required"},
                new object[]
                    {new Product() {Id = 5, ProductName = $"New Product", Price = ""}, "Price is required"},
                new object[]
                    {new Product() {Id = 5, ProductName = $"New Product", Price = "500"}, "Invalid price value"},
            };
    }
}
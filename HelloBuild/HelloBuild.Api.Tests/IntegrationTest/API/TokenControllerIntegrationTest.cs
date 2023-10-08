using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text.Json;
using Xunit;

namespace HelloBuild.Api.Tests.IntegrationTest.API
{
    public class TokenControllerIntegrationTest : IntegrationTestBuilder
    {
        [Fact]
        public void GetTokenSuccess()
        {
            var requestUser = new
            {
                Name = "Andres",
                Email = "AndresPaniagua@andres.es",
                Password = "123456"
            };
            var requestToken = new
            {
                Email = "AndresPaniagua@andres.es",
                Password = "123456"
            };

            // We load the data to the db and consult the token
            HttpResponseMessage load = TestClient.PostAsync("/api/User/SaveUser", requestUser, new JsonMediaTypeFormatter()).Result;
            _ = load.EnsureSuccessStatusCode();
            _ = load.Content.ReadAsStringAsync().Result;

            HttpResponseMessage responseLoad = TestClient.PostAsync("/api/Token/Authentication", requestToken, new JsonMediaTypeFormatter()).Result;
            _ = responseLoad.EnsureSuccessStatusCode();
            string response = responseLoad.Content.ReadAsStringAsync().Result;
            Dictionary<string, string> responseQuery = JsonSerializer.Deserialize<Dictionary<string, string>>(response);

            responseQuery["token"].Should().NotBeEmpty();
        }

        [Fact]
        public void GetTokenError()
        {
            var requestToken = new
            {
                Email = "Andres@andres.es",
                Password = "654321"
            };
            HttpResponseMessage load = null;
            try
            {
                load = TestClient.PostAsync("/api/Token/Authentication", requestToken, new JsonMediaTypeFormatter()).Result;
                _ = load.EnsureSuccessStatusCode();

                Assert.True(false, "Must fail");
            }
            catch (Exception)
            {
                load.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            }
        }
    }
}

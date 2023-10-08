using FluentAssertions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using Xunit;

namespace HelloBuild.Api.Tests.IntegrationTest.API
{
    public class UserControllerIntegrationTest : IntegrationTestBuilder
    {
        [Fact]
        public void SaveUserSuccess()
        {
            // Arrange
            var requestUser = new
            {
                Name = "Andres",
                Email = "AndresPaniagua@andres.es",
                Password = "123456"
            };

            // Act
            HttpResponseMessage load = TestClient.PostAsync("/api/User/SaveUser", requestUser, new JsonMediaTypeFormatter()).Result;
            _ = load.EnsureSuccessStatusCode();
            Dictionary<string, object> response = JsonConvert.DeserializeObject<Dictionary<string, object>>(load.Content.ReadAsStringAsync().Result);

            // Assert
            _ = load.StatusCode.Should().Be(HttpStatusCode.OK);
            _ = ((bool)response["isSave"]).Should().BeTrue();
        }

        [Fact]
        public void SaveUserError()
        {
            // Arrange
            var requestUser = new
            {
                Email = "AndresPaniagua@andres.es",
                Password = "123456"
            };
            HttpResponseMessage load = null;
            try
            {
                // Act
                load = TestClient.PostAsync("/api/User/SaveUser", requestUser, new JsonMediaTypeFormatter()).Result;
                _ = load.EnsureSuccessStatusCode();

                // Assert
                Assert.True(false, "Must fail");
            }
            catch (Exception)
            {
                // Assert
                _ = load.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            }
        }

        [Fact]
        public void GetUserSuccess()
        {
            // Arrange
            var saveUser = new
            {
                Name = "Andres",
                Email = "AndresPaniagua@andres.es",
                Password = "123456"
            };
            var requestUser = new
            {
                Email = "AndresPaniagua@andres.es",
                Password = "123456"
            };

            // We load the data to the db and consult the token
            HttpResponseMessage load = TestClient.PostAsync("/api/User/SaveUser", saveUser, new JsonMediaTypeFormatter()).Result;
            _ = load.EnsureSuccessStatusCode();
            _ = load.Content.ReadAsStringAsync().Result;

            // Act
            HttpResponseMessage responseLoad = TestClient.PostAsync("/api/User/UserRegistered", requestUser, new JsonMediaTypeFormatter()).Result;
            _ = responseLoad.EnsureSuccessStatusCode();
            Dictionary<string, object> response = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseLoad.Content.ReadAsStringAsync().Result);

            // Assert
            _ = responseLoad.StatusCode.Should().Be(HttpStatusCode.OK);
            _ = ((bool)response["exists"]).Should().BeTrue();
        }

        [Fact]
        public void GetUserError()
        {
            // Arrange
            var requestUser = new
            {
                Email = "Andres@andres.es",
                Password = "654321"
            };
            HttpResponseMessage load = null;
            try
            {
                load = TestClient.PostAsync("/api/User/UserRegistered", requestUser, new JsonMediaTypeFormatter()).Result;
                _ = load.EnsureSuccessStatusCode();

                // Assert
                Assert.True(false, "Must fail");
            }
            catch (Exception)
            {
                // Assert
                _ = load.StatusCode.Should().Be(HttpStatusCode.NotFound);
            }
        }

        [Fact]
        public void GetUserInfoSuccess()
        {
            // Arrange
            var saveUser = new
            {
                Name = "Andres",
                Email = "AndresPaniagua@andres.es",
                Password = "123456"
            };

            var requestToken = new
            {
                saveUser.Email,
                saveUser.Password
            };

            var requestUserInfo = new
            {
                saveUser.Email
            };

            // We load the data to the db
            HttpResponseMessage load = TestClient.PostAsync("/api/User/SaveUser", saveUser, new JsonMediaTypeFormatter()).Result;
            _ = load.EnsureSuccessStatusCode();
            _ = load.Content.ReadAsStringAsync().Result;

            // Consult the token
            HttpResponseMessage responseToken = TestClient.PostAsync("/api/Token/Authentication", requestToken, new JsonMediaTypeFormatter()).Result;
            _ = responseToken.EnsureSuccessStatusCode();
            string responseT = responseToken.Content.ReadAsStringAsync().Result;
            Dictionary<string, string> responseQuery = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseT);

            string token = responseQuery["token"];

            // Add token in header
            TestClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act
            HttpResponseMessage responseLoad = TestClient.PostAsync("/api/User/GetUserInfo", requestUserInfo, new JsonMediaTypeFormatter()).Result;
            _ = responseLoad.EnsureSuccessStatusCode();
            Dictionary<string, string> response = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseLoad.Content.ReadAsStringAsync().Result);

            // Assert
            _ = responseLoad.StatusCode.Should().Be(HttpStatusCode.OK);
            _ = response["name"].Should().Equals(saveUser.Name);
        }

        [Fact]
        public void GetUserInfoError()
        {
            // Arrange
            var requestUser = new
            {
                Email = "Andres@andres.es"
            };
            HttpResponseMessage load = null;

            try
            {
                // Act
                load = TestClient.PostAsync("/api/User/GetUserInfo", requestUser, new JsonMediaTypeFormatter()).Result;
                _ = load.EnsureSuccessStatusCode();

                // Assert
                Assert.True(false, "Must fail");
            }
            catch (Exception)
            {
                // Assert
                _ = load.StatusCode.Should().Be(401);
            }
        }

    }
}

using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using Refit;
using SleekFlow.Web.Todos;
using SleekFlow.Web.WebAPI.DTOs.Login;
using SleekFlow.Web.WebAPI.Test.SleekFlowWebApiClient;
using System.IdentityModel.Tokens.Jwt;
using System.Net;

namespace SleekFlow.Web.WebAPI.Test
{
    public class WebAuthTest
    {
        private string? token;
        private ISleekFlowWebApiClient apiClient;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            apiClient = RestService.For<ISleekFlowWebApiClient>(Common.Configuration["ApiBase"], new RefitSettings()
            {
                AuthorizationHeaderValueGetter = () => Task.FromResult(token)
            });

            Login("admin").Wait();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
        }

        private async Task Login(string username)
        {
            token = await Common.GetToken(username);
        }

        [Test, Order(1)]
        public async Task TestLogin()
        {
            try { new JwtSecurityToken(token); }
            catch { Assert.Fail("login failed"); }

            var validationOutput = await apiClient.ValidateLogin();

            Assert.That(validationOutput, Is.True, "validate failed");
        }

        [Test, Order(2)]
        public async Task TestAuthorization()
        {
            var unauthenticatedResponse = await RestService.For<ISleekFlowWebApiClient>(Common.Configuration["ApiBase"]).RawTestAuthorization();
            Assert.That(unauthenticatedResponse.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized), "authentication failed");

            var unauthorizedResponse = await RestService.For<ISleekFlowWebApiClient>(Common.Configuration["ApiBase"], new RefitSettings()
            {
                AuthorizationHeaderValueGetter = async () => await Common.GetToken("user")
            }).RawTestAuthorization();
            Assert.That(unauthorizedResponse.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden), "authorization failed");

            var authorizedResponse = await RestService.For<ISleekFlowWebApiClient>(Common.Configuration["ApiBase"], new RefitSettings()
            {
                AuthorizationHeaderValueGetter = async () => await Common.GetToken("admin")
            }).RawTestAuthorization();
            Assert.That(authorizedResponse.IsSuccessStatusCode, Is.True, "authorization failed");
        }
    }
}
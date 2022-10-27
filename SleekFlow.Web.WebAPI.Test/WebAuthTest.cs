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
    internal class WebAuthTest : WebApiTestBase
    {

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            base.OneTimeSetUp();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            base.OneTimeTearDown();
        }

        [Test, Order(1)]
        public async Task TestLogin()
        {
            await Login("admin");

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
                AuthorizationHeaderValueGetter = async () => await GetToken("user")
            }).RawTestAuthorization();
            Assert.That(unauthorizedResponse.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden), "authorization failed");

            var authorizedResponse = await RestService.For<ISleekFlowWebApiClient>(Common.Configuration["ApiBase"], new RefitSettings()
            {
                AuthorizationHeaderValueGetter = async () => await GetToken("admin")
            }).RawTestAuthorization();
            Assert.That(authorizedResponse.IsSuccessStatusCode, Is.True, "authorization failed");
        }
    }
}
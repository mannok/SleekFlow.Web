using NUnit.Framework;
using Refit;
using SleekFlow.Web.WebAPI.DTOs.Login;
using SleekFlow.Web.WebAPI.Test.SleekFlowWebApiClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SleekFlow.Web.WebAPI.Test
{
    internal class WebApiTestBase
    {
        protected string? token;
        protected ISleekFlowWebApiClient apiClient;

        [OneTimeSetUp]
        public virtual void OneTimeSetUp()
        {
            apiClient = RestService.For<ISleekFlowWebApiClient>(Common.Configuration["ApiBase"], new RefitSettings()
            {
                AuthorizationHeaderValueGetter = () => Task.FromResult(token)
            });
        }

        [OneTimeTearDown]
        public virtual void OneTimeTearDown()
        {
        }

        [SetUp]
        public virtual void Setup()
        {

        }

        [TearDown]
        public virtual void TearDown()
        {
            Logout();
        }

        protected async Task<string> GetToken(string username) => await RestService.For<ISleekFlowWebApiClient>(Common.Configuration["ApiBase"]).Login(new LoginDto
        {
            Username = Common.Configuration[$"Users:{username}:Username"],
            Password = Common.Configuration[$"Users:{username}:Password"]
        });

        protected async Task<string> Login(string username) => token = await GetToken(username);

        protected void Logout() { token = null; }
    }
}

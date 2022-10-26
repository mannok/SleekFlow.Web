using Refit;
using SleekFlow.Web.WebAPI.DTOs.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SleekFlow.Web.WebAPI.Test.SleekFlowWebApiClient
{
    partial interface ISleekFlowWebApiClient
    {
        [Post("/WebAuth/Login")]
        Task<string> Login(LoginDto login);

        [Get("/WebAuth/Status")]
        [Headers("Authorization: Bearer")]
        Task<bool> ValidateLogin();

        [Get("/WebAuth/TestAuthorization")]
        [Headers("Authorization: Bearer")]
        Task<HttpResponseMessage> RawTestAuthorization();
    }
}

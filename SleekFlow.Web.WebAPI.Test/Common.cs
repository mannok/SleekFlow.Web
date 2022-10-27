using Microsoft.Extensions.Configuration;
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
    internal static class Common
    {
        public static IConfiguration Configuration { get; }

        static Common()
        {
            Configuration = new ConfigurationBuilder().AddJsonFile($"appsettings.json", true, true).Build();
        }
    }
}

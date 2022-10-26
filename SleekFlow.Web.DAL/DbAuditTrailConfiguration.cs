using Audit.EntityFramework.Providers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using SleekFlow.Web.DAL.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SleekFlow.Web.DAL
{
    public static class DbAuditTrailConfiguration
    {
        public static void UseDbAuditTrail(this IApplicationBuilder applicationBuilder)
        {
            Audit.Core.Configuration.Setup().UseFactory(() => new EntityFrameworkDataProvider()
            {
                DbContextBuilder = ev => applicationBuilder.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<SleekFlowWebDbContext>(),
                ExplicitMapper = ee => typeof(AuditEntry),
                AuditEntityAction = (evt, entry, auditEntity) =>
                {
                    var a = (AuditEntry)auditEntity;
                    a.Id = default;
                    a.AuditDate = DateTime.UtcNow;
                    a.UserName = applicationBuilder.ApplicationServices.GetRequiredService<IHttpContextAccessor>().HttpContext.User.Claims.FirstOrDefault(x => x.Type == System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? evt.Environment.UserName;
                    a.EntityType = entry.Table;
                    a.AuditAction = entry.Action; // Insert, Update, Delete
                    switch (entry.Action)
                    {
                        case "Insert":
                            a.Mutations = entry.ColumnValues;
                            break;
                        case "Update":
                            a.Mutations = entry.Changes;
                            break;
                        case "Delete":
                            a.Mutations = entry.PrimaryKey;
                            break;
                    }

                    return Task.FromResult(true); // return false to ignore the audit
                }
            });
        }
    }
}

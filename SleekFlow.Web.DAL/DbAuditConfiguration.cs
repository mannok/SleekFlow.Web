using Audit.EntityFramework.Providers;
using SleekFlow.Web.DAL.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SleekFlow.Web.DAL
{
    public class DbAuditConfiguration
    {
        [ModuleInitializer]
        public static void Configure()
        {
            Audit.Core.Configuration.DataProvider = new EntityFrameworkDataProvider()
            {
                DbContextBuilder = ev => new SleekFlowWebDbContext(),
                ExplicitMapper = ee => typeof(AuditEntry),
                AuditEntityAction = (evt, entry, auditEntity) =>
                {
                    var a = (AuditEntry)auditEntity;
                    a.Id = 0;
                    a.AuditDate = DateTime.UtcNow;
                    a.UserName = evt.Environment.UserName;
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
            };
        }
    }
}

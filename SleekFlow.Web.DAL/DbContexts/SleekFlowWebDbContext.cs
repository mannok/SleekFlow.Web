using System;
using System.Collections.Generic;
using Audit.EntityFramework;
using Audit.EntityFramework.Providers;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SleekFlow.Web.DAL;

namespace SleekFlow.Web.DAL.DbContexts
{
    public partial class SleekFlowWebDbContext : AuditDbContext
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IConfiguration configuration;

        public DbSet<Todos.Todo> Todos { get; set; }
        public DbSet<AuditEntry> AuditEntries { get; set; }

        public SleekFlowWebDbContext(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public SleekFlowWebDbContext(DbContextOptions<SleekFlowWebDbContext> options, IConfiguration configuration)
            : base(options)
        {
            this.configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(configuration.GetConnectionString("SleekFlowWebDb"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            OnModelCreatingPartial(modelBuilder);

            modelBuilder.Entity<AuditEntry>().Property(e => e.Mutations).HasConversion(
                v => JsonConvert.SerializeObject(v, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }),
                v => JsonConvert.DeserializeAnonymousType(v, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore })
            );
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

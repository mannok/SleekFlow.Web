using System;
using System.Collections.Generic;
using Audit.EntityFramework;
using Audit.EntityFramework.Providers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Newtonsoft.Json;
using SleekFlow.Web.DAL;

namespace SleekFlow.Web.DAL.DbContexts
{
    public partial class SleekFlowWebDbContext : AuditDbContext
    {
        public DbSet<Todos.Todo> Todos { get; set; }
        public DbSet<AuditEntry> AuditEntries { get; set; }

        public SleekFlowWebDbContext()
        {
        }

        public SleekFlowWebDbContext(DbContextOptions<SleekFlowWebDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=SleekFlow.Web;Trusted_Connection=True;");
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

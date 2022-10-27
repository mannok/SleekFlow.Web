using System;
using System.Collections.Generic;
using Audit.EntityFramework;
using Audit.EntityFramework.Providers;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SleekFlow.Web.DAL;
using SleekFlow.Web.Domain;

namespace SleekFlow.Web.DAL.DbContexts
{
    public partial class SleekFlowWebDbContext : AuditDbContext
    {
        private readonly IConfiguration configuration;
        private readonly IHttpContextAccessor httpContextAccessor;

        public DbSet<Todos.Todo> Todos { get; set; }
        public DbSet<AuditEntry> AuditEntries { get; set; }

        public SleekFlowWebDbContext(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            this.configuration = configuration;
            this.httpContextAccessor = httpContextAccessor;
        }

        public SleekFlowWebDbContext(DbContextOptions<SleekFlowWebDbContext> options, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
            : base(options)
        {
            this.configuration = configuration;
            this.httpContextAccessor = httpContextAccessor;
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

        private string GetCurrentUsername() => httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? Environment.UserName;

        private void FillBasicAuditFields()
        {
            ChangeTracker.DetectChanges();

            IEnumerable<EntityEntry> entities = ChangeTracker.Entries().Where(t => t.Entity is AuditEntityBase && (
                t.State == EntityState.Added
                || t.State == EntityState.Modified
            ));

            DateTimeOffset timestamp = DateTimeOffset.UtcNow;

            foreach (var entry in entities)
            {
                var entity = (AuditEntityBase)entry.Entity;

                switch (entry.State)
                {
                    case EntityState.Added:
                        entity.CreatedOn = timestamp;
                        entity.CreatedBy = GetCurrentUsername();
                        entity.UpdatedOn = timestamp;
                        entity.UpdatedBy = GetCurrentUsername();
                        break;
                    case EntityState.Modified:
                        entity.UpdatedOn = timestamp;
                        entity.UpdatedBy = GetCurrentUsername();
                        break;
                }
            }
        }

        public override int SaveChanges()
        {
            FillBasicAuditFields();
            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            FillBasicAuditFields();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            FillBasicAuditFields();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            FillBasicAuditFields();
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}

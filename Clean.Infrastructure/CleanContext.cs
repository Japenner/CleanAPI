namespace Clean.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Clean.Core.Common;
    using Clean.Core.Entities;
    using Clean.Core.Interfaces;
    using Clean.Infrastructure.ModelBuilders;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;

    /// <summary>
    /// Database context that represents the entire Clean database.
    /// </summary>
    public class CleanContext : DbContext, IUnitOfWork
    {
        private readonly List<IEntityConfiguration> typeConfigurations;
        private readonly string currentUser = "system";

        /// <summary>
        /// Initializes a new instance of the <see cref="CleanContext"/> class.
        /// </summary>
        /// <param name="options">The options to configure the context.</param>
        /// <param name="typeConfigurations">The DI registered type configurations.</param>
        public CleanContext(DbContextOptions<CleanContext> options, IEntityConfiguration[] typeConfigurations)
            : base(options)
        {
            this.typeConfigurations = typeConfigurations.ToList();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CleanContext"/> class.
        /// </summary>
        public CleanContext()
        {
        }

        /// <summary>
        /// Gets or sets the collection of versions.
        /// </summary>
        public DbSet<EntityVersion> Versions { get; set; }

        /// <summary>
        /// Gets or sets the collection of to do items.
        /// </summary>
        public DbSet<ToDoItem> ToDoItems { get; set; }

        /// <summary>
        /// Override SaveChanges to get changes.
        /// </summary>
        /// <param name="acceptAllChangesOnSuccess">some parameter</param>
        /// <returns>Number of records affected.</returns>
        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            UpdateShadowFields();
            AuditChanges();

            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        /// <summary>
        /// Override SaveChangesAsync to get changes.
        /// </summary>
        /// <param name="acceptAllChangesOnSuccess">some parameter</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Number of records affected.</returns>
        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            UpdateShadowFields();
            AuditChanges();

            return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        /// <summary>
        /// Override the model creating process.
        /// </summary>
        /// <param name="modelBuilder">The model builder to use.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.AddConfiguration(new EntityVersionConfiguration());
            modelBuilder.AddConfiguration(new ToDoItemConfiguration());

            typeConfigurations.ForEach(c => c.Map(modelBuilder));

            foreach (var entityType in modelBuilder.Model.GetEntityTypes()
                .Where(e => typeof(IAuditable).IsAssignableFrom(e.ClrType)))
            {
                modelBuilder.Entity(entityType.ClrType)
                    .Property<DateTimeOffset>("CreatedAt");

                modelBuilder.Entity(entityType.ClrType)
                    .Property<DateTimeOffset>("ModifiedAt");

                modelBuilder.Entity(entityType.ClrType)
                    .Property<string>("CreatedBy").HasMaxLength(250).IsRequired();

                modelBuilder.Entity(entityType.ClrType)
                    .Property<string>("ModifiedBy").HasMaxLength(250).IsRequired();
            }

            base.OnModelCreating(modelBuilder);
        }

        private void UpdateShadowFields()
        {
            var entitiesToUpdate = ChangeTracker.Entries<Entity>().Where(e => (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entity in entitiesToUpdate)
            {
                entity.Property("ModifiedAt").CurrentValue = DateTimeOffset.UtcNow;
                entity.Property("ModifiedBy").CurrentValue = currentUser;

                if (entity.State == EntityState.Added)
                {
                    entity.Property("CreatedAt").CurrentValue = DateTimeOffset.UtcNow;
                    entity.Property("CreatedBy").CurrentValue = currentUser;
                }
            }
        }

        private void AuditChanges()
        {
            var entitiesToAudit = ChangeTracker.Entries<Entity>().Where(e => (e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted));
            var changes = new Dictionary<string, object>();
            var entityVersions = new List<EntityVersion>();

            foreach (var entity in entitiesToAudit)
            {
                changes.Clear();
                if (entity.State == EntityState.Added)
                {
                    foreach (var property in entity.Properties)
                    {
                        changes.Add(property.Metadata.Name, new { Original = (object)null, New = property.CurrentValue });
                    }
                }
                else if (entity.State == EntityState.Modified)
                {
                    foreach (var property in entity.Properties.Where(p => p.IsModified))
                    {
                        changes.Add(property.Metadata.Name, new { Original = property.OriginalValue, New = property.CurrentValue });
                    }
                }
                else if (entity.State == EntityState.Deleted)
                {
                    foreach (var property in entity.Properties)
                    {
                        changes.Add(property.Metadata.Name, new { Original = property.OriginalValue, New = (object)null });
                    }
                }

                var entityType = Model.FindEntityType(entity.Entity.GetType())?.FindAnnotation("Relational:TableName")?.Value ?? Model.FindEntityType(entity.Entity.GetType())?.BaseType.FindAnnotation("Relational:TableName")?.Value;

                var version = new EntityVersion()
                {
                    ChangedBy = currentUser,
                    ChangeType = entity.State.ToString(),
                    Timestamp = DateTimeOffset.UtcNow,
                    EntityType = entityType as string,
                    EntityId = entity.State == EntityState.Deleted ? entity.Property<Guid>("Id").OriginalValue : entity.Property<Guid>("Id").CurrentValue,
                    Changes = JsonConvert.SerializeObject(changes)
                };

                entityVersions.Add(version);
            }

            Versions.AddRange(entityVersions);
        }
    }
}

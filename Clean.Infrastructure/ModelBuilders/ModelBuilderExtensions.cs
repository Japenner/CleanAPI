namespace Clean.Infrastructure.ModelBuilders
{
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Extension methods for <see cref="ModelBuilder"/>.
    /// </summary>
    public static class ModelBuilderExtensions
    {
        /// <summary>
        /// Add configuration to the model builder.
        /// </summary>
        /// <typeparam name="TEntity">Type of entity to configure.</typeparam>
        /// <param name="modelBuilder">Model builder.</param>
        /// <param name="configuration">Configuration for the entity.</param>
        public static void AddConfiguration<TEntity>(this ModelBuilder modelBuilder, EntityTypeConfiguration<TEntity> configuration)
            where TEntity : class
        {
            configuration.Map(modelBuilder.Entity<TEntity>());
        }
    }
}
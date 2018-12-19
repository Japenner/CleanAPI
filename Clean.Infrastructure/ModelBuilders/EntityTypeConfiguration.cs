namespace Clean.Infrastructure.ModelBuilders
{
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>
    /// Abstract class for configuring entity types.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity to configure.</typeparam>
    public abstract class EntityTypeConfiguration<TEntity>
        where TEntity : class
    {
        /// <summary>
        /// Maps the configuration into the builder.
        /// </summary>
        /// <param name="builder">The builder applied.</param>
        public abstract void Map(EntityTypeBuilder<TEntity> builder);
    }
}
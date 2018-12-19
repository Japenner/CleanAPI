namespace Clean.Infrastructure.ModelBuilders
{
    using Clean.Core.Entities;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>
    /// Configures the version entity for EntityFramework
    /// </summary>
    public class EntityVersionConfiguration : EntityTypeConfiguration<EntityVersion>
    {
        /// <summary>
        /// Maps the configuration into the builder.
        /// </summary>
        /// <param name="builder">The builder to use.</param>
        public override void Map(EntityTypeBuilder<EntityVersion> builder)
        {
            builder.HasKey(v => new { v.EntityId, v.EntityType, v.Timestamp });

            builder.Property(v => v.ChangedBy)
                .IsRequired()
                .HasMaxLength(250);

            builder.Property(v => v.ChangeType)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(v => v.Changes)
                .IsRequired();
        }
    }
}

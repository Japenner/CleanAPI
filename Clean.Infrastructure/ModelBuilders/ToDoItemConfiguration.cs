namespace Clean.Infrastructure.ModelBuilders
{
    using Clean.Core.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>
    /// Configures the ToDoItems for EntityFramework
    /// </summary>
    public class ToDoItemConfiguration : EntityTypeConfiguration<ToDoItem>
    {
        /// <summary>
        /// Maps the configuration into the builder.
        /// </summary>
        /// <param name="builder">The builder to use</param>
        public override void Map(EntityTypeBuilder<ToDoItem> builder)
        {
            builder.HasKey(tdi => tdi.Id);

            builder.Property(tdi => tdi.Id)
                .IsRequired();

            builder.Property(tdi => tdi.Title)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(tdi => tdi.Description)
                .IsRequired()
                .HasMaxLength(250);

            builder.Property(tdi => tdi.IsDone)
                .IsRequired()
                .HasDefaultValue(false);
        }
    }
}

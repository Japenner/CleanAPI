namespace Clean.Infrastructure.ModelBuilders
{
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Interface for configuring entities.
    /// </summary>
    /// <remarks>
    /// By implementing this interface, the configuration will automatically be picked up by Autofac and injected into <see cref="CleanContext"/>.
    /// The configuration does not have to be explicitly added.
    /// </remarks>
    public interface IEntityConfiguration
    {
        /// <summary>
        /// Maps the entity into the data model.
        /// </summary>
        /// <param name="modelBuilder">The builder for the model.</param>
        void Map(ModelBuilder modelBuilder);
    }
}

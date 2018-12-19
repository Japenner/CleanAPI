namespace Clean.Core.Common
{
    using System;
    using Clean.Core.Interfaces;

    /// <summary>
    /// Base entity class to capture common fields for all entities.
    /// </summary>
    public abstract class Entity : IAuditable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Entity"/> class.
        /// </summary>
        /// <param name="id">The id of the entity - DO NOT USE A STANDARD GUID!</param>
        public Entity(Guid id)
        {
            Id = id;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Entity"/> class.
        /// </summary>
        protected Entity()
        {
        }

        /// <summary>
        /// Gets the ID of the entity.
        /// </summary>
        public Guid Id { get; private set; }
    }
}

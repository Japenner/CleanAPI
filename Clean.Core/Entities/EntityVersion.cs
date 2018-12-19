namespace Clean.Core.Entities
{
    using System;

    /// <summary>
    /// Tracks changes to entities.
    /// </summary>
    public class EntityVersion
    {
        /// <summary>
        /// Gets or sets the type of entity the version is for.
        /// </summary>
        public string EntityType { get; set; }

        /// <summary>
        /// Gets or sets the id of the entity the version is for.
        /// </summary>
        public Guid EntityId { get; set; }

        /// <summary>
        /// Gets or sets the timestamp of when the entity was changed.
        /// </summary>
        public DateTimeOffset Timestamp { get; set; }

        /// <summary>
        /// Gets or sets the username of who made the change.
        /// </summary>
        public string ChangedBy { get; set; }

        /// <summary>
        /// Gets or sets the type of change made.
        /// </summary>
        public string ChangeType { get; set; }

        /// <summary>
        /// Gets or sets the changes as a JSON encoded string.
        /// </summary>
        public string Changes { get; set; }
    }
}

namespace Clean.Core.Entities
{
    using Clean.Core.Common;
    using System;

    /// <summary>
    /// The base class for Family and Individual directory entries
    /// </summary>
    public abstract class DirectoryEntry : Entity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DirectoryEntry"/> class.
        /// </summary>
        public DirectoryEntry()
             : base(RT.Comb.Provider.Sql.Create())
        {
        }

        /// <summary>
        /// Gets or sets the value for created at
        /// </summary>
        public DateTimeOffset CreatedAt { get; protected set; }

        /// <summary>
        /// Gets or sets the value for created by
        /// </summary>
        public string CreatedBy { get; protected set; }

        /// <summary>
        /// Gets or sets the value for modified at
        /// </summary>
        public DateTimeOffset ModifiedAt { get; protected set; }

        /// <summary>
        /// Gets or sets the value for modified by
        /// </summary>
        public string ModifiedBy { get; protected set; }

        /// <summary>
        /// Gets the value for entry type
        /// </summary>
        public string EntryType { get; private set; }
    }
}

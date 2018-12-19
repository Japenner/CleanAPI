namespace Clean.Core.Interfaces
{
    /// <summary>
    /// Interface that represents a base repository to ensure the unit of work is available.
    /// </summary>
    /// <typeparam name="T">The aggregate root type.</typeparam>
    public interface IRepository<T>
        where T : IAggregateRoot
    {
        /// <summary>
        /// Gets the unit of work.
        /// </summary>
        IUnitOfWork UnitOfWork { get; }
    }

    /// <summary>
    /// Interface that represents a base repository to ensure the unit of work is available.
    /// </summary>
    public interface IRepository
    {
        /// <summary>
        /// Gets the unit of work.
        /// </summary>
        IUnitOfWork UnitOfWork { get; }
    }
}

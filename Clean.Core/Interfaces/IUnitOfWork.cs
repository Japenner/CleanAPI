namespace Clean.Core.Interfaces
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Interface to implement the unit of work pattern.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Saves all changes asynchronously.
        /// </summary>
        /// <param name="cancellationToken">Token to allow for cancellation of asynchronous call.</param>
        /// <returns>Returns a count of the number of entities updated.</returns>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}
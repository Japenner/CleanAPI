namespace Clean.Core.Interfaces
{
    using Clean.Core.Entities;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Repository interface for forms.
    /// </summary>
    public interface IToDoItemsRepository : IRepository<ToDoItem>
    {
        /// <summary>
        /// Gets a list of all to do items.
        /// </summary>
        /// <returns>Returns a list of to do items</returns>
        Task<IList<ToDoItem>> GetToDoItemsAsync();

        /// <summary>
        /// Gets a single to do item
        /// </summary>
        /// <param name="toDoItemId">The ID of the to do item to be retrieved</param>
        /// <returns>Returns a single to do item</returns>
        Task<ToDoItem> GetToDoItemAsync(Guid toDoItemId);

        /// <summary>
        /// Deletes the specified to do item
        /// </summary>
        /// <param name="toDoItem">The to do item to be deleted</param>
        /// <returns>Nothing</returns>
        Task DeleteToDoItemAsync(ToDoItem toDoItem);
    }
}

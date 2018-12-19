namespace Clean.Infrastructure.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Clean.Core.Entities;
    using Clean.Core.Interfaces;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Implementation of <see cref="IToDoItemsRepository"/>.
    /// </summary>
    public class ToDoItemsRepository : IToDoItemsRepository
    {
        private readonly CleanContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="ToDoItemsRepository"/> class.
        /// </summary>
        /// <param name="context">Database context</param>
        public ToDoItemsRepository(CleanContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Gets the unit of work for this repository.
        /// </summary>
        public IUnitOfWork UnitOfWork => context;

        /// <summary>
        /// Gets a list of all to do items.
        /// </summary>
        /// <returns>Returns a list of to do items.</returns>
        public async Task<IList<ToDoItem>> GetToDoItemsAsync()
        {
            return await context.ToDoItems.ToListAsync();
        }

        /// <summary>
        /// Gets a single to do item
        /// </summary>
        /// <param name="toDoItemId">The ID of the to do item to be retrieved</param>
        /// <returns>Returns a single to do item</returns>
        public async Task<ToDoItem> GetToDoItemAsync(Guid toDoItemId)
        {
            var toDoItem = await context.ToDoItems
                .Where(tdi => tdi.Id == toDoItemId)
                .FirstOrDefaultAsync();

            if (toDoItem == null)
            {
                throw new KeyNotFoundException("To do item does not exist.");
            }

            return toDoItem;
        }

        /// <summary>
        /// Deletes the specified to do item
        /// </summary>
        /// <param name="toDoItem">The to do item to be deleted</param>
        /// <returns>Nothing</returns>
        public async Task DeleteToDoItemAsync(ToDoItem toDoItem)
        {
            context.Remove(toDoItem);
            await context.SaveChangesAsync();
        }
    }
}
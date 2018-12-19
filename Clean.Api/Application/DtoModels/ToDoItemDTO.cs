namespace Clean.Web.ApiModels
{
    using System;
    using Clean.Core.Entities;

    /// <summary>
    /// The dto for a ToDoItem related to the organization
    /// </summary>
    public class ToDoItemDTO
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ToDoItemDTO"/> class.
        /// </summary>
        public ToDoItemDTO()
        {
        }

        /// <summary>
        /// Gets the to do item ID
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// Gets or sets the to do item title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the to do item description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets a value indicating whether or not the to do item is marked as done
        /// </summary>
        public bool IsDone { get; private set; }

        /// <summary>
        /// Creates an <see cref="ToDoItemDTO"/> from an existing <see cref="ToDoItem"/>.
        /// </summary>
        /// <param name="toDoItem">The to do item to convert.</param>
        /// <returns>Returns a to do item DTO.</returns>
        public static ToDoItemDTO CreateFrom(ToDoItem toDoItem)
        {
            return new ToDoItemDTO()
            {
                Id = toDoItem.Id,
                Title = toDoItem.Title,
                Description = toDoItem.Description,
                IsDone = toDoItem.IsDone
            };
        }
    }
}
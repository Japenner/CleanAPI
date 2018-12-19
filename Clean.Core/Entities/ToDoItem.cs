namespace Clean.Core.Entities
{
    using Clean.Core.Common;
    using Clean.Core.Interfaces;

    /// <summary>
    /// Base entity that represents a to do item in the system.
    /// </summary>
    public class ToDoItem : Entity, IAggregateRoot
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ToDoItem"/> class.
        /// </summary>
        /// <param name="title">The title of the to do item</param>
        /// <param name="description">The description of the to do item</param>
        public ToDoItem(string title, string description)
            : base(RT.Comb.Provider.Sql.Create())
        {
            Title = title;
            Description = description;
            IsDone = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ToDoItem"/> class.
        /// <para>This is the copy constructor</para>
        /// </summary>
        /// <param name="toDoItem">The to do item to copy</param>
        public ToDoItem(ToDoItem toDoItem)
            : base(RT.Comb.Provider.Sql.Create())
        {
            Title = toDoItem.Title;
            Description = toDoItem.Description;
            IsDone = toDoItem.IsDone;
        }

        private ToDoItem()
        {
        }

        /// <summary>
        /// Gets the title of the to do item
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// Gets the description of the to do item
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the to do item has been completed
        /// </summary>
        public bool IsDone { get; private set; }

        /// <summary>
        /// Sets the to do item title
        /// </summary>
        /// <param name="title">The title to be set</param>
        public void SetTitle(string title)
        {
            Title = title;
        }

        /// <summary>
        /// Sets the to do item title
        /// </summary>
        /// <param name="description">The title to be set</param>
        public void SetDescription(string description)
        {
            Description = description;
        }

        /// <summary>
        /// Sets the to do item title
        /// </summary>
        /// <param name="isDone">A value indicating whether or not the to do item is completed</param>
        public void SetIsDone(bool isDone)
        {
            IsDone = isDone;
        }

        /// <summary>
        /// Marks the to do item as completed
        /// </summary>
            public void MarkComplete()
        {
            IsDone = true;
        }
    }
}

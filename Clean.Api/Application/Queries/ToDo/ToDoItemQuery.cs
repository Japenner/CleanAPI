namespace CleanArchictecture.Web.Application.Queries.ToDo
{
    using Clean.Web.ApiModels;
    using MediatR;
    using Newtonsoft.Json;
    using System;

    /// <summary>
    /// Represents a query to get a to do item.
    /// </summary>
    public class ToDoItemQuery : IRequest<ToDoItemDTO>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ToDoItemQuery"/> class.
        /// </summary>
        public ToDoItemQuery()
            : base()
        {
        }

        /// <summary>
        /// Gets or sets the to do item id.
        /// </summary>
        [JsonIgnore]
        public Guid ToDoItemId { get; set; }
    }
}

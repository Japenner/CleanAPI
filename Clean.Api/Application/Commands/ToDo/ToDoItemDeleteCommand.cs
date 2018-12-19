namespace Clean.Api.Application.Commands.ToDo
{
    using MediatR;
    using System;

    /// <summary>
    /// The command used to delete a to do item
    /// </summary>
    public class ToDoItemDeleteCommand : IRequest
    {
        /// <summary>
        /// Gets or sets the to do item id
        /// </summary>
        public Guid ToDoItemId { get; set; }
    }
}

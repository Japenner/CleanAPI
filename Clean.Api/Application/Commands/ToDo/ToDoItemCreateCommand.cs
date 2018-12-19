namespace Clean.Api.Application.Commands.ToDo
{
    using MediatR;
    using Newtonsoft.Json;
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// The command used to create to do items
    /// </summary>
    public class ToDoItemCreateCommand : IRequest<Guid>
    {
        /// <summary>
        /// Gets or sets the title of the to do item
        /// </summary>
        [Required]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the description of the to do item
        /// </summary>
        [Required]
        public string Description { get; set; }
    }
}

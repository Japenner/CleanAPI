namespace Clean.Api.Application.Commands.ToDo
{
    using MediatR;
    using Newtonsoft.Json;
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// The command used to update a to do item
    /// </summary>
    public class ToDoItemUpdateCommand : IRequest
    {
        /// <summary>
        /// Gets or sets the to do item id
        /// </summary>
        [JsonIgnore]
        public Guid ToDoItemId { get; set; }

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

        /// <summary>
        /// Gets or sets a value indicating whether or not the to do item is completed
        /// </summary>
        [Required]
        public bool IsDone { get; set; }
    }
}
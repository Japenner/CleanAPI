namespace Mosaic.Api.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Clean.Api.Application.Commands.ToDo;
    using Clean.Web.ApiModels;
    using CleanArchictecture.Web.Application.DtoModels;
    using CleanArchictecture.Web.Application.Queries.ToDo;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Controller for public data
    /// </summary>
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/toDoItems")]
    public class ToDoItemsController : Controller
    {
        private readonly IMediator mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="ToDoItemsController"/> class.
        /// </summary>
        /// <param name="mediator">The mediator service.</param>
        public ToDoItemsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Creates a to do item
        /// </summary>
        /// <param name="command">The command data to create a to do item</param>
        /// <returns>The Guid for the new to do item</returns>
        /// <response code="200">Success</response>
        /// <response code="400">Parameter is missing</response>
        [HttpPost]
        [Route("")]
        [ProducesResponseType(typeof(Guid), 200)]
        public async Task<IActionResult> CreateToDoItem([FromBody] ToDoItemCreateCommand command)
        {
            var result = await mediator.Send(command);

            return Ok(result);
        }

        /// <summary>
        /// Gets all to do items
        /// </summary>
        /// <returns>All to do items</returns>
        /// <response code="200">Success</response>
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(CollectionResult<ToDoItemDTO>), 200)]
        public async Task<IActionResult> GetToDoItemListing()
        {
            var result = await mediator.Send(new ToDoItemListingQuery());
            return Ok(result);
        }

        /// <summary>
        /// Gets a to do item
        /// </summary>
        /// <param name="toDoItemId">The id of the to do item to retrieve</param>
        /// <returns>A to do item</returns>
        /// <response code="200">Success</response>
        /// <response code="404">Missing or invalid <paramref name="toDoItemId"/> is not a valid <see cref="Guid"/></response>
        [HttpGet]
        [Route("{toDoItemId:guid}")]
        [ProducesResponseType(typeof(ToDoItemDTO), 200)]
        public async Task<IActionResult> GetToDoItem(Guid toDoItemId)
        {
            var query = new ToDoItemQuery
            {
                ToDoItemId = toDoItemId
            };

            var result = await mediator.Send(query);

            return Ok(result);
        }

        /// <summary>
        /// Updates the entire to do item
        /// </summary>
        /// <param name="toDoItemId">The to do item id</param>
        /// <param name="command">The to do item update object populated from request body</param>
        /// <returns>a response</returns>
        /// <response code="204">Success</response>
        /// <response code="404">Passed in to do item is not found</response>
        [HttpPut]
        [Route("{toDoItemId:guid}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> UpdateToDoItem(Guid toDoItemId, [FromBody] ToDoItemUpdateCommand command)
        {
            command.ToDoItemId = toDoItemId;

            await mediator.Send(command);
            return NoContent();
        }

        /// <summary>
        /// Deletes a to do item
        /// </summary>
        /// <param name="toDoItemId">The id of the to do item to delete</param>
        /// <returns>No content</returns>
        /// <response code="204">Success</response>
        /// <response code="404">Missing or invalid <paramref name="toDoItemId"/> is not a valid <see cref="Guid"/></response>
        [HttpDelete]
        [Route("{toDoItemId:guid}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> DeleteToDoItem(Guid toDoItemId)
        {
            var command = new ToDoItemDeleteCommand
            {
                ToDoItemId = toDoItemId
            };

            await mediator.Send(command);
            return NoContent();
        }
    }
}

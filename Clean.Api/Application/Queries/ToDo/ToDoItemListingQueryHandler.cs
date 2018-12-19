namespace Clean.Web.Application.Queries.ToDo
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Clean.Core.Interfaces;
    using Clean.Web.ApiModels;
    using CleanArchictecture.Web.Application.DtoModels;
    using CleanArchictecture.Web.Application.Queries.ToDo;
    using MediatR;

    /// <summary>
    /// Handles the ToDoItemListingQuery.
    /// </summary>
    public class ToDoItemListingQueryHandler : IRequestHandler<ToDoItemListingQuery, CollectionResult<ToDoItemDTO>>
    {
        private IToDoItemsRepository repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ToDoItemListingQueryHandler"/> class.
        /// </summary>
        /// <param name="repository">The repository to use.</param>
        public ToDoItemListingQueryHandler(IToDoItemsRepository repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Handles the query.
        /// </summary>
        /// <param name="query">The query to process.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task<CollectionResult<ToDoItemDTO>> Handle(ToDoItemListingQuery query, CancellationToken token)
        {
            var toDoItems = await repository.GetToDoItemsAsync();

            List<ToDoItemDTO> dtoListing = new List<ToDoItemDTO>(toDoItems.Select(tdi => ToDoItemDTO.CreateFrom(tdi)));

            return new CollectionResult<ToDoItemDTO>(dtoListing.ToArray());
        }
    }
}

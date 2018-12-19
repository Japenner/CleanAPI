namespace Clean.Api.Application.Queries.ToDoItem
{
    using System.Threading;
    using System.Threading.Tasks;
    using Clean.Core.Interfaces;
    using Clean.Web.ApiModels;
    using CleanArchictecture.Web.Application.Queries.ToDo;
    using MediatR;

    /// <summary>
    /// Handles the ToDoItemQuery
    /// </summary>
    public class ToDoItemQueryHandler : IRequestHandler<ToDoItemQuery, ToDoItemDTO>
    {
        private IToDoItemsRepository repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ToDoItemQueryHandler"/> class.
        /// </summary>
        /// <param name="repository">The repository to use</param>
        public ToDoItemQueryHandler(IToDoItemsRepository repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Handles the query
        /// </summary>
        /// <param name="query">The query to process</param>
        /// <param name="token">The cancellation token</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation</returns>
        public async Task<ToDoItemDTO> Handle(ToDoItemQuery query, CancellationToken token)
        {
            var toDoItem = await repository.GetToDoItemAsync(query.ToDoItemId);

            return ToDoItemDTO.CreateFrom(toDoItem);
        }
    }
}

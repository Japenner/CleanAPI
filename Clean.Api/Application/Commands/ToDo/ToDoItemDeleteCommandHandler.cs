namespace Clean.Api.Application.Commands.ToDoItem
{
    using System.Threading;
    using System.Threading.Tasks;
    using Clean.Api.Application.Commands.ToDo;
    using Clean.Core.Interfaces;
    using MediatR;

    /// <summary>
    /// Handles the <see cref="ToDoItemDeleteCommandHandler"/>.
    /// </summary>
    public class ToDoItemDeleteCommandHandler : IRequestHandler<ToDoItemDeleteCommand, Unit>
    {
        private IToDoItemsRepository toDoItemsRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ToDoItemDeleteCommandHandler"/> class.
        /// </summary>
        /// <param name="toDoItemsRepository">The repository to use</param>
        public ToDoItemDeleteCommandHandler(IToDoItemsRepository toDoItemsRepository)
        {
            this.toDoItemsRepository = toDoItemsRepository;
        }

        /// <summary>
        /// Process the <see cref="ToDoItemDeleteCommand"/>.
        /// </summary>
        /// <param name="command">The command to process</param>
        /// <param name="token">The cancellation token</param>
        /// <returns>Nothing</returns>
        public async Task<Unit> Handle(ToDoItemDeleteCommand command, CancellationToken token)
        {
            var toDoItem = await toDoItemsRepository.GetToDoItemAsync(command.ToDoItemId);

            await toDoItemsRepository.DeleteToDoItemAsync(toDoItem);

            return Unit.Value;
        }
    }
}

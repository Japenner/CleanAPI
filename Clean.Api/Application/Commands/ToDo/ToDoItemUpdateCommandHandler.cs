namespace Clean.Api.Application.Commands.ToDo
{
    using System.Threading;
    using System.Threading.Tasks;
    using Clean.Core.Interfaces;
    using Clean.Infrastructure;
    using MediatR;

    /// <summary>
    /// Handler for creating to do items
    /// </summary>
    public class ToDoItemUpdateCommandHandler : IRequestHandler<ToDoItemUpdateCommand>
    {
        private IToDoItemsRepository toDoItemsRepository;
        private CleanContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="ToDoItemUpdateCommandHandler"/> class.
        /// </summary>
        /// <param name="toDoItemsRepository">The to do items repository to use</param>
        /// <param name="context">The context to use</param>
        public ToDoItemUpdateCommandHandler(IToDoItemsRepository toDoItemsRepository, CleanContext context)
        {
            this.toDoItemsRepository = toDoItemsRepository;
            this.context = context;
        }

        /// <summary>
        /// Handles the command
        /// </summary>
        /// <param name="command">The request</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The guid of the to do item created</returns>
        public async Task<Unit> Handle(ToDoItemUpdateCommand command, CancellationToken token)
        {
            var toDoItem = await toDoItemsRepository.GetToDoItemAsync(command.ToDoItemId);

            toDoItem.SetTitle(command.Title);
            toDoItem.SetDescription(command.Description);
            toDoItem.SetIsDone(command.IsDone);

            await toDoItemsRepository.UnitOfWork.SaveChangesAsync();

            return Unit.Value;
        }
    }
}

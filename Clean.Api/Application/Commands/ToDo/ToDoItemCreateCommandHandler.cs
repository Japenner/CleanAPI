namespace Clean.Api.Application.Commands.ToDo
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Clean.Core.Entities;
    using Clean.Core.Interfaces;
    using Clean.Infrastructure;
    using MediatR;

    /// <summary>
    /// Handler for creating to do items
    /// </summary>
    public class ToDoItemCreateCommandHandler : IRequestHandler<ToDoItemCreateCommand, Guid>
    {
        private IToDoItemsRepository toDoItemsRepository;
        private CleanContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="ToDoItemCreateCommandHandler"/> class.
        /// </summary>
        /// <param name="toDoItemsRepository">The to do items repository to use</param>
        /// <param name="context">The context to use</param>
        public ToDoItemCreateCommandHandler(IToDoItemsRepository toDoItemsRepository, CleanContext context)
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
        public async Task<Guid> Handle(ToDoItemCreateCommand command, CancellationToken token)
        {
            var toDoItem = new ToDoItem(command.Title, command.Description);

            context.ToDoItems.Add(toDoItem);

            var result = await toDoItemsRepository.UnitOfWork.SaveChangesAsync();

            return toDoItem.Id;
        }
    }
}

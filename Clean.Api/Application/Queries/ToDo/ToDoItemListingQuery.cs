namespace CleanArchictecture.Web.Application.Queries.ToDo
{
    using Clean.Web.ApiModels;
    using CleanArchictecture.Web.Application.DtoModels;
    using MediatR;

    /// <summary>
    /// Represents a query to get the list of to do items.
    /// </summary>
    public class ToDoItemListingQuery : IRequest<CollectionResult<ToDoItemDTO>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ToDoItemListingQuery"/> class.
        /// </summary>
        public ToDoItemListingQuery()
            : base()
        {
        }
    }
}

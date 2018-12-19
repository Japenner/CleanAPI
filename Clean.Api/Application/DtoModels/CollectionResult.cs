namespace CleanArchictecture.Web.Application.DtoModels
{
    /// <summary>
    /// A generic result that contains a collection.
    /// </summary>
    /// <typeparam name="T">The type of the returned data.</typeparam>
    public class CollectionResult<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CollectionResult{T}"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        public CollectionResult(T[] data)
        {
            Data = data;
        }

        /// <summary>
        /// Gets the count of elements in <see cref="Data"/>.
        /// </summary>
        public int Count
        {
            get
            {
                return Data?.Length ?? 0;
            }
        }

        /// <summary>
        /// Gets the collection.
        /// </summary>
        public T[] Data { get; private set; }
    }
}

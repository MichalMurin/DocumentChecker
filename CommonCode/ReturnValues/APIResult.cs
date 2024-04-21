
namespace CommonCode.ReturnValues
{
    /// <summary>
    /// Represents the result of an API operation.
    /// </summary>
    /// <typeparam name="T">The type of the result.</typeparam>
    public class APIResult<T>
    {
        /// <summary>
        /// Gets or sets the result of the API operation.
        /// </summary>
        public T? Result { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the API operation was successful.
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// Gets or sets the error message if the API operation failed.
        /// </summary>
        public string? ErrorMessage { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="APIResult{T}"/> class.
        /// </summary>
        /// <param name="result">The result of the API operation.</param>
        /// <param name="isSuccess">A value indicating whether the API operation was successful.</param>
        /// <param name="errorMessage">The error message if the API operation failed.</param>
        public APIResult(T? result, bool isSuccess, string? errorMessage)
        {
            Result = result;
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
        }
    }
}

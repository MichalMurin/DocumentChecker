
namespace CommonCode.ReturnValues
{
    /// <summary>
    /// Represents the result of an API operation.
    /// </summary>
    /// <typeparam name="T">The type of the result.</typeparam>
    /// <remarks>
    /// Initializes a new instance of the <see cref="APIResult{T}"/> class.
    /// </remarks>
    /// <param name="result">The result of the API operation.</param>
    /// <param name="isSuccess">A value indicating whether the API operation was successful.</param>
    /// <param name="errorMessage">The error message if the API operation failed.</param>
    public class APIResult<T>(T? result, bool isSuccess, string? errorMessage)
    {
        /// <summary>
        /// Gets or sets the result of the API operation.
        /// </summary>
        public T? Result { get; set; } = result;

        /// <summary>
        /// Gets or sets a value indicating whether the API operation was successful.
        /// </summary>
        public bool IsSuccess { get; set; } = isSuccess;

        /// <summary>
        /// Gets or sets the error message if the API operation failed.
        /// </summary>
        public string? ErrorMessage { get; set; } = errorMessage;
    }
}

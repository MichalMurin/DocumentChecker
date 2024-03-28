using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonCode.ReturnValues
{
    public class APIResult<T>
    {
        public T? Result { get; set; }
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }

        public APIResult(T? result, bool isSuccess, string? errorMessage)
        {
            Result = result;
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
        }
    }
}

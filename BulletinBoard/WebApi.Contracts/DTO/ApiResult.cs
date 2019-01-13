using System;
using System.Collections.Generic;
using System.Text;

namespace WebApi.Contracts.DTO
{
    public class ApiResult<T>
    {
        public T Result { get; }
        public bool IsSuccess { get; }
        public string[] Errors { get; set; }

        public ApiResult(T result, bool isSuccess, string[] errors)
        {
            Result = result;
            IsSuccess = isSuccess;
            Errors = errors;
        }
    }
}

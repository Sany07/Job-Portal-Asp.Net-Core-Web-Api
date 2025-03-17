using System.Collections.Generic;

namespace JobPortal.CQRS.Common.Models
{
    public class Result
    {
        public bool IsSuccess { get; }
        public string Error { get; }
        public IEnumerable<string> Errors { get; }
        
        protected Result(bool isSuccess, string error)
        {
            IsSuccess = isSuccess;
            Error = error;
            Errors = error != null ? new[] { error } : new List<string>();
        }

        protected Result(bool isSuccess, IEnumerable<string> errors)
        {
            IsSuccess = isSuccess;
            Errors = errors ?? new List<string>();
            Error = string.Join(", ", Errors);
        }
        
        public static Result Success() => new Result(true, (string)null);
        
        public static Result Failure(string error) => new Result(false, error);
        
        public static Result Failure(IEnumerable<string> errors) => new Result(false, errors);
    }
    
    public class Result<T> : Result
    {
        public T Value { get; }
        
        protected Result(T value, bool isSuccess, string error) 
            : base(isSuccess, error)
        {
            Value = value;
        }
        
        protected Result(T value, bool isSuccess, IEnumerable<string> errors)
            : base(isSuccess, errors)
        {
            Value = value;
        }
        
        public static Result<T> Success(T value) => new Result<T>(value, true, (string)null);
        
        public static new Result<T> Failure(string error) => new Result<T>(default, false, error);
        
        public static new Result<T> Failure(IEnumerable<string> errors) => new Result<T>(default, false, errors);
    }
} 
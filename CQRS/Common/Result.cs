using System;
using System.Collections.Generic;

namespace JobPortal.CQRS.Common
{
    public class Result
    {
        public bool IsSuccess { get; }
        public string Error { get; }
        public bool IsFailure => !IsSuccess;

        protected Result(bool isSuccess, string error)
        {
            IsSuccess = isSuccess;
            Error = error;
        }

        public static Result Success() => new Result(true, string.Empty);
        public static Result Failure(string error) => new Result(false, error);
        
        public static Result<T> Success<T>(T value) => Result<T>.Success(value);
        public static Result<T> Failure<T>(string error) => Result<T>.Failure(error);
    }

    public class Result<T> : Result
    {
        private readonly T _value;
        public T Value => IsSuccess 
            ? _value 
            : throw new InvalidOperationException("Cannot access Value of a failed result");

        protected internal Result(bool isSuccess, T value, string error) 
            : base(isSuccess, error)
        {
            _value = value;
        }

        public static Result<T> Success(T value) => new Result<T>(true, value, string.Empty);
        public static new Result<T> Failure(string error) => new Result<T>(false, default, error);
    }

    public class PagedResult<T> : Result<IReadOnlyList<T>>
    {
        public int TotalCount { get; }
        public int PageNumber { get; }
        public int PageSize { get; }
        public int TotalPages { get; }
        public bool HasPrevious => PageNumber > 1;
        public bool HasNext => PageNumber < TotalPages;

        private PagedResult(bool isSuccess, IReadOnlyList<T> value, string error, int totalCount, int pageNumber, int pageSize)
            : base(isSuccess, value, error)
        {
            TotalCount = totalCount;
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        }

        public static PagedResult<T> Success(IReadOnlyList<T> value, int totalCount, int pageNumber, int pageSize)
            => new PagedResult<T>(true, value, string.Empty, totalCount, pageNumber, pageSize);

        public static PagedResult<T> Failure(string error)
            => new PagedResult<T>(false, new List<T>().AsReadOnly(), error, 0, 0, 0);
    }
} 
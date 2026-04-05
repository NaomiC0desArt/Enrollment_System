using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversitySystem.Application.Auxiliary
{
    public class Result
    {
        public bool IsSuccess { get; }
        public string Error { get; }

        protected Result(bool success, string error)
        {
            IsSuccess = success;
            Error = error;
        }

        public static Result Success() => new Result(true, null);
        public static Result Failure(string error) => new Result(false, error);
    }

    public class Result<T> : Result
    {
        public T Value { get; }

        private Result(T value, bool success, string error) : base(success, error)
        {
            Value = value;
        }

        public static Result<T> Success(T value) => new Result<T>(value, true, null);
        public static new Result<T> Failure(string error) => new Result<T>(default, false, error);
    }
}

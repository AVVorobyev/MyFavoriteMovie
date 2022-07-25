using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class DomainResult
    {
        public string? Message { get; }
        public bool Succes { get; }


        public DomainResult()
        {
            Succes = true;
        }

        public DomainResult(string message)
        {
            Message = message;
        }


        public static DomainResult Succeeded() => new();
        public static DomainResult Failed(string message) => new(message);
    }

    public sealed class DomainResult<T> : DomainResult
    {
        public T? Result { get; }

        public DomainResult(T result) => Result = result;
        private DomainResult(string message) : base(message) { }

        public static DomainResult<T> Succeeded(T result) => new(result);
        public static new DomainResult<T> Failed(string message) => new(message);
    }
}

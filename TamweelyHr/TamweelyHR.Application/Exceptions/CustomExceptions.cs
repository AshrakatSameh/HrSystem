using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TamweelyHR.Application.Exceptions
{
 
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message) { }
    }
    public class DuplicateException : Exception
    {
        public DuplicateException(string message) : base(message) { }
    }
    public class AppValidationException : Exception
    {
        public Dictionary<string, string[]> Errors { get; }

        public AppValidationException(Dictionary<string, string[]> errors)
            : base("One or more validation errors occurred.")
        {
            Errors = errors;
        }
    }


}

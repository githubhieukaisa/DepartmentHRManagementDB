using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace assignment.utility
{
    public class ValidationRule<T>
    {
        public Func<T, bool> validationFunc { get; set; }
        public string errorMessage { get; set; }
        public ValidationRule(Func<T, bool> validationFunc, string errorMessage)
        {
            this.validationFunc = validationFunc;
            this.errorMessage = errorMessage;
        }

    }
}

using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace assignment.utility
{
    public static class Validation
    {
        public static bool Validate<T>(string value, Func<string, T> parser, List<ValidationRule<T>> rules, out string errorMessage, out T result, string label)
        {
            errorMessage = string.Empty;
            if (value.IsNullOrEmpty())
            {
                errorMessage = label + " cannot be empty";
                result = default!;
                return false;
            }
            try
            {
                result = parser(value);
            }
            catch
            {
                errorMessage = label + " is Invalid input";
                result = default!;
                return false;
            }
            foreach (var rule in rules)
            {
                if (!rule.validationFunc(result))
                {
                    errorMessage = rule.errorMessage;
                    return false;
                }
            }
            return true;
        }

        public static string CapitalizeEachWord(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return input;

            var words = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < words.Length; i++)
            {
                var word = words[i];
                if (word.Length > 0)
                {
                    words[i] = char.ToUpper(word[0]) + word.Substring(1).ToLower();
                }
            }
            return string.Join(' ', words);
        }

    }
}

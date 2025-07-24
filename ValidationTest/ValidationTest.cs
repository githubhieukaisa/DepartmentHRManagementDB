using assignment.utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static assignment.utility.Validation;

namespace ValidationTest
{
    public class ValidationTest
    {
        [Fact]
        public void Validate_ShouldReturnFalse_WhenValueIsEmpty()
        {
            // Arrange
            var rules = new List<ValidationRule<int>>();
            string errorMessage;
            int result;

            // Act
            var isValid = Validation.Validate("", int.Parse, rules, out errorMessage, out result, "Number");

            // Assert
            Assert.False(isValid);
            Assert.Equal("Number cannot be empty", errorMessage);
            Assert.Equal(default(int), result);
        }

        [Fact]
        public void Validate_ShouldReturnFalse_WhenParserThrowsException()
        {
            // Arrange
            var rules = new List<ValidationRule<int>>();
            string errorMessage;
            int result;

            // Act
            var isValid = Validation.Validate("invalid", int.Parse, rules, out errorMessage, out result, "Number");

            // Assert
            Assert.False(isValid);
            Assert.Equal("Number is Invalid input", errorMessage);
            Assert.Equal(default(int), result);
        }

        [Fact]
        public void Validate_ShouldReturnTrue_WhenValueIsValidAndRulesPass()
        {
            // Arrange
            var rules = new List<ValidationRule<int>>
            {
                new ValidationRule<int> (x => x > 0,"Number must be positive" )
            };
            string errorMessage;
            int result;

            // Act
            var isValid = Validation.Validate("42", int.Parse, rules, out errorMessage, out result, "Number");

            // Assert
            Assert.True(isValid);
            Assert.Empty(errorMessage);
            Assert.Equal(42, result);
        }

        [Fact]
        public void Validate_ShouldReturnFalse_WhenRuleFails()
        {
            // Arrange
            var rules = new List<ValidationRule<int>>
            {
                new ValidationRule<int> (x => x > 0, "Number must be positive")
            };
            string errorMessage;
            int result;

            // Act
            var isValid = Validate("-5", int.Parse, rules, out errorMessage, out result, "Number");

            // Assert
            Assert.False(isValid);
            Assert.Equal("Number must be positive", errorMessage);
            Assert.Equal(-5, result);
        }

        [Fact]
        public void Validate_ShouldHandleMultipleRulesCorrectly()
        {
            // Arrange
            var rules = new List<ValidationRule<int>>
            {
                new ValidationRule<int> (x => x > 0, "Number must be positive"),
                new ValidationRule<int> (x => x % 2 == 0, "Number must be even" )
            };
            string errorMessage;
            int result;

            // Act
            var isValid = Validation.Validate("3", int.Parse, rules, out errorMessage, out result, "Number");

            // Assert
            Assert.False(isValid);
            Assert.Equal("Number must be even", errorMessage);
            Assert.Equal(3, result);
        }
        [Theory]
        [InlineData(null, "Full Name cannot be empty", null)]
        [InlineData("", "Full Name cannot be empty", null)]
        [InlineData("nguyen a", "Full Name không được ít hơn 10 ký tự", "Nguyen A")]
        [InlineData("nguyen van a very long name that exceeds fifty characters limit", "Full Name không được quá 50 ký tự", "Nguyen Van A Very Long Name That Exceeds Fifty Characters Limit")]
        [InlineData("nguyen van a123", "Full Name chỉ được chứa chữ cái hoặc space", "Nguyen Van A123")]
        [InlineData("nguyen van a", "", "Nguyen Van A")]
        [InlineData("nguyen van test", "", "Nguyen Van Test")]
        public void Validate_TestFullname_ShouldHandleVariousCases(string input, string expectedErrorMessage, string expectedResult)
        {
            // Arrange
            var rules = new List<ValidationRule<string>>
            {
                new ValidationRule<string>(s => s.Length >= 10, "Full Name không được ít hơn 10 ký tự"),
                new ValidationRule<string>(s => s.Length <= 50, "Full Name không được quá 50 ký tự"),
                new ValidationRule<string>(s => s.All(c => char.IsLetter(c) || c == ' '), "Full Name chỉ được chứa chữ cái hoặc space")
            };
            string errorMessage;
            string result;
            // Act
            var isValid = Validate(input, CapitalizeEachWord, rules, out errorMessage, out result, "Full Name");
            // Assert
            Assert.Equal(string.IsNullOrEmpty(expectedErrorMessage), isValid);
            Assert.Equal(expectedErrorMessage, errorMessage);
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(null, "Email cannot be empty", null)]
        [InlineData("", "Email cannot be empty", null)]
        [InlineData("short", "Email không được ít hơn 10 ký tự", "short")]
        [InlineData("verylongemailaddressverylongemailaddressverylongemailaddress@example.com", "Email không được quá 50 ký tự", "verylongemailaddressverylongemailaddressverylongemailaddress@example.com")]
        [InlineData("noatsign.com", "Email phải chứa '@' và '.'", "noatsign.com")]
        [InlineData("valid@example.com", "", "valid@example.com")]
        [InlineData("user123@example.com", "", "user123@example.com")]
        [InlineData("user123@ex.com", "", "user123@ex.com")] // Biên dưới: 10 ký tự
        [InlineData("a1234567890123456789012345678901234567@example.com", "", "a1234567890123456789012345678901234567@example.com")] // Biên trên: 50 ký tự
        public void Validate_TestEmail_ShouldHandleVariousCases(string input, string expectedErrorMessage, string expectedResult)
        {
            var rules = new List<ValidationRule<string>>
            {
                new ValidationRule<string>(s => s.Length >= 10,"Email không được ít hơn 10 ký tự"),
                new ValidationRule<string>(s => s.Length <= 50,"Email không được quá 50 ký tự"),
                new ValidationRule<string>(s => s.Contains("@") && s.Contains("."), "Email phải chứa '@' và '.'")
            };
            string errorMessage;
            string result;
            
            var isValid = Validate(input, s => s, rules, out errorMessage, out result, "Email");

            Assert.Equal(string.IsNullOrEmpty(expectedErrorMessage), isValid);
            Assert.Equal(expectedErrorMessage, errorMessage);
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(null, "Start Date cannot be empty", null)]
        [InlineData("", "Start Date cannot be empty", null)]
        [InlineData("invalid_date", "Start Date is Invalid input", null)]
        [InlineData("2025-07-23", "Start date phải trước ngày hiện tại.", "2025-07-23")]
        [InlineData("2025-07-24", "", "2025-07-24")] // Biên: Ngày hiện tại
        [InlineData("2025-07-25", "", "2025-07-25")] // Ngày tương lai
        public void Validate_TestStartDate_ShouldHandleVariousCases(string input, string expectedErrorMessage, string expectedResult)
        {
            // Arrange
            var rules = new List<ValidationRule<DateTime>>
            {
                new ValidationRule<DateTime>(d => d.Date >= DateTime.Today, "Start date phải trước ngày hiện tại.")
            };
            string errorMessage;
            DateTime result;

            // Act
            var isValid = Validation.Validate(input, s => DateTime.Parse(s), rules, out errorMessage, out result, "Start Date");

            // Assert
            Assert.Equal(string.IsNullOrEmpty(expectedErrorMessage), isValid);
            Assert.Equal(expectedErrorMessage, errorMessage);
            if (expectedResult == null)
            {
                Assert.Equal(default(DateTime), result);
            }
            else
            {
                Assert.Equal(DateTime.Parse(expectedResult), result);
            }
        }
    }
}

using assignment.Models;
using assignment.Service;
using FluentAssertions;
using MockQueryable.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LoginTest
{
    public class LoginServiceTests
    {
        private readonly Mock<IProjectManagementDbContext> _mockContext;
        private readonly LoginService _loginService;

        public LoginServiceTests()
        {
            _mockContext = new Mock<IProjectManagementDbContext>();
            _loginService = new LoginService(_mockContext.Object);
        }

        private void SetupMockEmployees()
        {
            var employees = new List<Employee>
            {
                new Employee { Email = "admin@example.com", PasswordHash = "123", Status = "Active", RoleId = 1 },
                new Employee { Email = "qa1@example.com", PasswordHash = "hashed_123456", Status = "Active", RoleId = 2 },
                new Employee { Email = "qa2@example.com", PasswordHash = "hashed_123456", Status = "Inactive", RoleId = 2 },
                new Employee { Email = "vuahoarong@gmail.com", PasswordHash = "123456", Status = "Active", RoleId = 2 }
            }.AsQueryable().BuildMockDbSet();

            _mockContext.Setup(c => c.Employees).Returns(employees.Object);
        }

        [Fact]
        public void Login_WithValidQAEmployee_ReturnsEmployee()
        {
            // Arrange
            SetupMockEmployees();

            // Act
            var result = _loginService.Login("qa1@example.com", "hashed_123456");

            // Assert
            result.Should().NotBeNull();
            result.Email.Should().Be("qa1@example.com");
        }

        [Fact]
        public void Login_WithInactiveAccount_ThrowsUnauthorizedAccess()
        {
            // Arrange
            SetupMockEmployees();

            // Act & Assert
            var act = () => _loginService.Login("qa2@example.com", "hashed_123456");
            act.Should().Throw<UnauthorizedAccessException>();
        }

        [Fact]
        public void Login_WithWrongPassword_ThrowsUnauthorizedAccess()
        {
            // Arrange
            SetupMockEmployees();

            // Act & Assert
            var act = () => _loginService.Login("admin@example.com", "wrong");
            act.Should().Throw<UnauthorizedAccessException>();
        }

        [Fact]
        public void Login_WithNonExistentEmail_ThrowsUnauthorizedAccess()
        {
            // Arrange
            SetupMockEmployees();

            // Act & Assert
            var act = () => _loginService.Login("ghost@example.com", "any");
            act.Should().Throw<UnauthorizedAccessException>();
        }

        [Theory]
        [InlineData(null, "123456")]
        [InlineData("vuahoarong@gmail.com", null)]
        [InlineData("", "123456")]
        [InlineData("vuahoarong@gmail.com", "")]
        public void Login_WithNullOrEmptyInputs_ThrowsArgumentException(string email, string password)
        {
            // Arrange
            SetupMockEmployees();

            // Act
            var act = () => _loginService.Login(email, password);

            // Assert
            act.Should().Throw<ArgumentException>();
        }
    }
}
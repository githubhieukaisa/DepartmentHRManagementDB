using assignment.Models;
using assignment.Service;
using FluentAssertions;
using MockQueryable.Moq;
using Moq;
using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagementTest
{
    public class EmployeeManagementTest
    {
        private readonly Mock<IProjectManagementDbContext> _mockContext;
        private readonly EmployeeManagementService _employeeManagementService;
        public EmployeeManagementTest()
        {
            _mockContext = new Mock<IProjectManagementDbContext>();
            _employeeManagementService = new EmployeeManagementService(_mockContext.Object);
        }
        [Fact]
        public void SaveEmployee_ShouldAddNewEmployee_WithValidData()
        {
            // Arrange
            var mockDbSet = new List<Employee>().AsQueryable().BuildMockDbSet();
            _mockContext.Setup(c => c.Employees).Returns(mockDbSet.Object);

            var fullname = "Nguyen Van A";
            var email = "vana@example.com";
            var password = "password123";
            var roleId = 1;
            var departmentId = 2;
            var status = "Active";

            // Act
            _employeeManagementService.SaveEmployee(null, "Add", fullname, email, password, roleId, departmentId, status);

            // Assert
            _mockContext.Verify(c => c.Employees.Add(It.Is<Employee>(e =>
                e.FullName == fullname &&
                e.Email == email &&
                e.PasswordHash == password &&
                e.RoleId == roleId &&
                e.DepartmentId == departmentId &&
                e.Status == status
            )), Times.Once);

            _mockContext.Verify(c => c.SaveChanges(), Times.Once);
        }

        [Fact]
        public void SaveEmployee_ShouldUpdateExistingEmployee_WithValidData()
        {
            // Arrange
            var existingEmp = new Employee
            {
                EmployeeId = 1,
                FullName = "Old Name",
                Email = "old@example.com",
                PasswordHash = "oldpassword",
                RoleId = 2,
                DepartmentId = 3,
                Status = "Inactive"
            };

            var mockDbSet = new List<Employee> { existingEmp }.AsQueryable().BuildMockDbSet();
            _mockContext.Setup(c => c.Employees).Returns(mockDbSet.Object);

            var newFullname = "Nguyen Van B";
            var newEmail = "vanb@example.com";
            var newPassword = "newpassword123";
            var newRoleId = 3;
            var newDepartmentId = 4;
            var newStatus = "Active";

            // Act
            _employeeManagementService.SaveEmployee(existingEmp, "Update", newFullname, newEmail, newPassword, newRoleId, newDepartmentId, newStatus);

            // Assert
            Assert.Equal(newFullname, existingEmp.FullName);
            Assert.Equal(newEmail, existingEmp.Email);
            Assert.Equal(newPassword, existingEmp.PasswordHash);
            Assert.Equal(newRoleId, existingEmp.RoleId);
            Assert.Equal(newDepartmentId, existingEmp.DepartmentId);
            Assert.Equal(newStatus, existingEmp.Status);

            _mockContext.Verify(c => c.Employees.Update(existingEmp), Times.Once);
            _mockContext.Verify(c => c.SaveChanges(), Times.Once);
        }

        [Fact]
        public void SaveEmployee_ShouldAddNewEmployee_WithEmptyFields()
        {
            // Arrange
            var mockDbSet = new List<Employee>().AsQueryable().BuildMockDbSet();
            _mockContext.Setup(c => c.Employees).Returns(mockDbSet.Object);

            var fullname = "";
            var email = "";
            var password = "";
            var roleId = 0;
            var departmentId = 0;
            var status = "";

            // Act
            _employeeManagementService.SaveEmployee(null, "Add", fullname, email, password, roleId, departmentId, status);

            // Assert
            _mockContext.Verify(c => c.Employees.Add(It.Is<Employee>(e =>
                e.FullName == fullname &&
                e.Email == email &&
                e.PasswordHash == password &&
                e.RoleId == roleId &&
                e.DepartmentId == departmentId &&
                e.Status == status
            )), Times.Once);

            _mockContext.Verify(c => c.SaveChanges(), Times.Once);
        }

        [Fact]
        public void SaveEmployee_ShouldUpdateExistingEmployee_WithEmptyFields()
        {
            // Arrange
            var existingEmp = new Employee
            {
                EmployeeId = 1,
                FullName = "Old Name",
                Email = "old@example.com",
                PasswordHash = "oldpassword",
                RoleId = 2,
                DepartmentId = 3,
                Status = "Inactive"
            };

            var mockDbSet = new List<Employee> { existingEmp }.AsQueryable().BuildMockDbSet();
            _mockContext.Setup(c => c.Employees).Returns(mockDbSet.Object);

            var newFullname = "";
            var newEmail = "";
            var newPassword = "";
            var newRoleId = 0;
            var newDepartmentId = 0;
            var newStatus = "";

            // Act
            _employeeManagementService.SaveEmployee(existingEmp, "Update", newFullname, newEmail, newPassword, newRoleId, newDepartmentId, newStatus);

            // Assert
            Assert.Equal(newFullname, existingEmp.FullName);
            Assert.Equal(newEmail, existingEmp.Email);
            Assert.Equal(newPassword, existingEmp.PasswordHash);
            Assert.Equal(newRoleId, existingEmp.RoleId);
            Assert.Equal(newDepartmentId, existingEmp.DepartmentId);
            Assert.Equal(newStatus, existingEmp.Status);

            _mockContext.Verify(c => c.Employees.Update(existingEmp), Times.Once);
            _mockContext.Verify(c => c.SaveChanges(), Times.Once);
        }

        [Fact]
        public void SaveEmployee_ShouldAddNewEmployee_WithInactiveStatus()
        {
            // Arrange
            var mockDbSet = new List<Employee>().AsQueryable().BuildMockDbSet();
            _mockContext.Setup(c => c.Employees).Returns(mockDbSet.Object);

            var fullname = "Nguyen Van C";
            var email = "vanc@example.com";
            var password = "password123";
            var roleId = 1;
            var departmentId = 2;
            var status = "Inactive";

            // Act
            _employeeManagementService.SaveEmployee(null, "Add", fullname, email, password, roleId, departmentId, status);

            // Assert
            _mockContext.Verify(c => c.Employees.Add(It.Is<Employee>(e =>
                e.FullName == fullname &&
                e.Email == email &&
                e.PasswordHash == password &&
                e.RoleId == roleId &&
                e.DepartmentId == departmentId &&
                e.Status == status
            )), Times.Once);

            _mockContext.Verify(c => c.SaveChanges(), Times.Once);
        }
    }
}

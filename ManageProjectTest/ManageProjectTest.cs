using assignment.Models;
using assignment.Service;
using MockQueryable.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ManageProjectTest
{
    public class ManageProjectTest
    {
        private readonly Mock<IProjectManagementDbContext> _mockContext;
        private readonly ProjectService _service;

        public ManageProjectTest()
        {
            _mockContext = new Mock<IProjectManagementDbContext>();
            _service = new ProjectService(_mockContext.Object);
        }

        [Fact]
        public void SaveProject_ShouldAddNewProject_WhenSelectedProjectIsNull()
        {
            // Arrange
            var mockDbSet = new List<Project>().AsQueryable().BuildMockDbSet();
            _mockContext.Setup(c => c.Projects).Returns(mockDbSet.Object);

            var name = "Dự án kiểm thử unit test số 01";
            var desc = "Mô tả vượt quá 30 ký tự để hợp lệ.";
            var start = DateTime.Today;
            var end = DateTime.Today.AddDays(7);

            // Act
            _service.SaveProject(null, name, desc, start, end);

            // Assert
            _mockContext.Verify(c => c.Projects.Add(It.Is<Project>(p =>
                p.ProjectName == name &&
                p.Description == desc &&
                p.StartDate == DateOnly.FromDateTime(start) &&
                p.EndDate == DateOnly.FromDateTime(end)
            )), Times.Once);

            _mockContext.Verify(c => c.SaveChanges(), Times.Once);
        }

        [Fact]
        public void SaveProject_ShouldUpdateProject_WhenSelectedProjectIsNotNull()
        {
            // Arrange
            var existingProject = new Project
            {
                ProjectId = 1,
                ProjectName = "Old Name",
                Description = "Old Desc",
                StartDate = DateOnly.FromDateTime(DateTime.Today),
                EndDate = DateOnly.FromDateTime(DateTime.Today.AddDays(5))
            };

            // Setup DbSet<Project> để tránh null
            var mockDbSet = new List<Project> { existingProject }.AsQueryable().BuildMockDbSet();
            _mockContext.Setup(c => c.Projects).Returns(mockDbSet.Object);

            var name = "Tên mới cập nhật";
            var desc = "Mô tả mới cập nhật dài hơn 30 ký tự.";
            var start = DateTime.Today;
            var end = DateTime.Today.AddDays(10);

            // Act
            _service.SaveProject(existingProject, name, desc, start, end);

            // Assert
            Assert.Equal(name, existingProject.ProjectName);
            Assert.Equal(desc, existingProject.Description);
            Assert.Equal(DateOnly.FromDateTime(start), existingProject.StartDate);
            Assert.Equal(DateOnly.FromDateTime(end), existingProject.EndDate);

            _mockContext.Verify(c => c.Projects.Update(existingProject), Times.Once);
            _mockContext.Verify(c => c.SaveChanges(), Times.Once);
        }

        [Fact]
        public void SaveProject_ShouldNotThrow_WhenDatesAreValid()
        {
            // Arrange
            var mockDbSet = new List<Project>().AsQueryable().BuildMockDbSet();
            _mockContext.Setup(c => c.Projects).Returns(mockDbSet.Object);

            var name = "Dự án hợp lệ";
            var desc = "Mô tả hợp lệ dài hơn 30 ký tự.";
            var start = DateTime.Today;
            var end = DateTime.Today.AddDays(5);

            // Act + Assert
            var exception = Record.Exception(() => _service.SaveProject(null, name, desc, start, end));
            Assert.Null(exception);
        }

        [Fact]
        public void SaveProject_ShouldCallAddOnly_WhenCreatingNewProject()
        {
            // Arrange
            var mockDbSet = new List<Project>().AsQueryable().BuildMockDbSet();
            _mockContext.Setup(c => c.Projects).Returns(mockDbSet.Object);

            // Act
            _service.SaveProject(null, "Project A", "Mô tả đủ dài để test thêm", DateTime.Today, DateTime.Today.AddDays(1));

            // Assert
            _mockContext.Verify(c => c.Projects.Add(It.IsAny<Project>()), Times.Once);
            _mockContext.Verify(c => c.Projects.Update(It.IsAny<Project>()), Times.Never);
        }

        [Fact]
        public void SaveProject_ShouldCallUpdateOnly_WhenUpdatingExistingProject()
        {
            // Arrange
            var existingProject = new Project
            {
                ProjectId = 2,
                ProjectName = "Project B",
                Description = "Cũ",
                StartDate = DateOnly.FromDateTime(DateTime.Today),
                EndDate = DateOnly.FromDateTime(DateTime.Today.AddDays(2))
            };

            // Act
            _service.SaveProject(existingProject, "Project C", "Mới hơn nhiều mô tả", DateTime.Today, DateTime.Today.AddDays(5));

            // Assert
            _mockContext.Verify(c => c.Projects.Update(It.IsAny<Project>()), Times.Once);
            _mockContext.Verify(c => c.Projects.Add(It.IsAny<Project>()), Times.Never);
        }
    }
}

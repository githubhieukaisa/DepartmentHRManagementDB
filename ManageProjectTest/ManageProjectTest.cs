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
            var mockDbSet = new List<Project>().AsQueryable().BuildMockDbSet();
            _mockContext.Setup(c => c.Projects).Returns(mockDbSet.Object);

            var name = "Dự án kiểm thử unit test số 01";
            var desc = "Mô tả vượt quá 30 ký tự để hợp lệ.";
            var start = DateTime.Today;
            var end = DateTime.Today.AddDays(7);

            _service.SaveProject(null, name, desc, start, end);

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
            var existingProject = new Project
            {
                ProjectId = 1,
                ProjectName = "Old Name",
                Description = "Old Desc",
                StartDate = DateOnly.FromDateTime(DateTime.Today),
                EndDate = DateOnly.FromDateTime(DateTime.Today.AddDays(5))
            };

            var mockDbSet = new List<Project> { existingProject }.AsQueryable().BuildMockDbSet();
            _mockContext.Setup(c => c.Projects).Returns(mockDbSet.Object);

            var name = "Tên mới cập nhật";
            var desc = "Mô tả mới cập nhật dài hơn 30 ký tự.";
            var start = DateTime.Today;
            var end = DateTime.Today.AddDays(10);

            _service.SaveProject(existingProject, name, desc, start, end);

            Assert.Equal(name, existingProject.ProjectName);
            Assert.Equal(desc, existingProject.Description);
            Assert.Equal(DateOnly.FromDateTime(start), existingProject.StartDate);
            Assert.Equal(DateOnly.FromDateTime(end), existingProject.EndDate);

            _mockContext.Verify(c => c.Projects.Update(existingProject), Times.Once);
            _mockContext.Verify(c => c.SaveChanges(), Times.Once);
        }

        [Fact]
        public void SaveProject_ShouldCallAddOnly_WhenCreatingNewProject()
        {
            var mockDbSet = new List<Project>().AsQueryable().BuildMockDbSet();
            _mockContext.Setup(c => c.Projects).Returns(mockDbSet.Object);

            _service.SaveProject(null, "Project A", "Mô tả đủ dài để test thêm", DateTime.Today, DateTime.Today.AddDays(1));

            _mockContext.Verify(c => c.Projects.Add(It.IsAny<Project>()), Times.Once);
            _mockContext.Verify(c => c.Projects.Update(It.IsAny<Project>()), Times.Never);
        }

        [Fact]
        public void SaveProject_ShouldAddNewProject_WhenProjectNameIsEmpty()
        {
            var mockDbSet = new List<Project>().AsQueryable().BuildMockDbSet();
            _mockContext.Setup(c => c.Projects).Returns(mockDbSet.Object);

            var name = "";
            var desc = "Mô tả đủ dài để kiểm tra tính hợp lệ của dự án.";
            var start = DateTime.Today;
            var end = DateTime.Today.AddDays(7);

            _service.SaveProject(null, name, desc, start, end);

            _mockContext.Verify(c => c.Projects.Add(It.Is<Project>(p =>
                p.ProjectName == name &&
                p.Description == desc &&
                p.StartDate == DateOnly.FromDateTime(start) &&
                p.EndDate == DateOnly.FromDateTime(end)
            )), Times.Once);

            _mockContext.Verify(c => c.SaveChanges(), Times.Once);
        }

        [Fact]
        public void SaveProject_ShouldUpdateProject_WhenStartAndEndDateAreSame()
        {
            var existingProject = new Project
            {
                ProjectId = 1,
                ProjectName = "Original Name",
                Description = "Original Description",
                StartDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-1)),
                EndDate = DateOnly.FromDateTime(DateTime.Today.AddDays(5))
            };

            var mockDbSet = new List<Project> { existingProject }.AsQueryable().BuildMockDbSet();
            _mockContext.Setup(c => c.Projects).Returns(mockDbSet.Object);

            var name = "Updated Project";
            var desc = "Mô tả đủ dài để kiểm tra tính hợp lệ của dự án.";
            var start = DateTime.Today;
            var end = DateTime.Today;

            _service.SaveProject(existingProject, name, desc, start, end);

            Assert.Equal(name, existingProject.ProjectName);
            Assert.Equal(desc, existingProject.Description);
            Assert.Equal(DateOnly.FromDateTime(start), existingProject.StartDate);
            Assert.Equal(DateOnly.FromDateTime(end), existingProject.EndDate);

            _mockContext.Verify(c => c.Projects.Update(existingProject), Times.Once);
            _mockContext.Verify(c => c.SaveChanges(), Times.Once);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using assignment.Models;
using assignment.Service;
using FluentAssertions;
using MockQueryable.Moq;
using Moq;
using Xunit;

namespace SaveTeamTest
{
    public class SaveTeamTest
    {
        public readonly Mock<IProjectManagementDbContext> _mockContext;
        private readonly SaveTeamService _service;
        public SaveTeamTest()
        {
            _mockContext = new Mock<IProjectManagementDbContext>();
            _service = new SaveTeamService(_mockContext.Object);
        }
        [Fact]
        public void SaveTeam_ShouldAddNewTeam_WhenExistingTeamIsNull()
        {
            // Arrange
            var mockDbSet = new List<Team>().AsQueryable().BuildMockDbSet();
            _mockContext.Setup(c => c.Teams).Returns(mockDbSet.Object);

            var name = "New Team";
            var desc = "This is a new team";

            // Act
            _service.SaveTeam(null, name, desc);

            // Assert
            _mockContext.Verify(c => c.Teams.Add(It.Is<Team>(t =>
                t.TeamName == name &&
                t.Description == desc &&
                t.CreatedAt != default
            )), Times.Once);

            _mockContext.Verify(c => c.SaveChanges(), Times.Once);
        }

        [Fact]
        public void SaveTeam_ShouldUpdateExistingTeam_WhenExistingTeamIsNotNull()
        {
            // Arrange
            var existingTeam = new Team
            {
                TeamId = 1,
                TeamName = "Old Name",
                Description = "Old Description",
                CreatedAt = DateTime.Now.AddDays(-1)
            };

            var mockDbSet = new List<Team> { existingTeam }.AsQueryable().BuildMockDbSet();
            _mockContext.Setup(c => c.Teams).Returns(mockDbSet.Object);

            var newName = "Updated Team";
            var newDesc = "Updated description";

            // Act
            _service.SaveTeam(existingTeam, newName, newDesc);

            // Assert
            existingTeam.TeamName.Should().Be(newName);
            existingTeam.Description.Should().Be(newDesc);
            existingTeam.CreatedAt.Should().BeCloseTo(DateTime.Now.AddDays(-1), TimeSpan.FromSeconds(1));

            _mockContext.Verify(c => c.Teams.Update(existingTeam), Times.Once);
            _mockContext.Verify(c => c.SaveChanges(), Times.Once);
        }

        [Fact]
        public void SaveTeam_ShouldAddNewTeam_WithEmptyName()
        {
            // Arrange
            var mockDbSet = new List<Team>().AsQueryable().BuildMockDbSet();
            _mockContext.Setup(c => c.Teams).Returns(mockDbSet.Object);

            var name = "";
            var desc = "Team with empty name";

            // Act
            _service.SaveTeam(null, name, desc);

            // Assert
            _mockContext.Verify(c => c.Teams.Add(It.Is<Team>(t =>
                t.TeamName == name &&
                t.Description == desc &&
                t.CreatedAt != default
            )), Times.Once);

            _mockContext.Verify(c => c.SaveChanges(), Times.Once);
        }

        [Fact]
        public void SaveTeam_ShouldUpdateExistingTeam_WithEmptyDescription()
        {
            // Arrange
            var existingTeam = new Team
            {
                TeamId = 1,
                TeamName = "Team Name",
                Description = "Old Description",
                CreatedAt = DateTime.Now.AddDays(-1)
            };

            var mockDbSet = new List<Team> { existingTeam }.AsQueryable().BuildMockDbSet();
            _mockContext.Setup(c => c.Teams).Returns(mockDbSet.Object);

            var newName = "Updated Team";
            var newDesc = "";

            // Act
            _service.SaveTeam(existingTeam, newName, newDesc);

            // Assert
            existingTeam.TeamName.Should().Be(newName);
            existingTeam.Description.Should().Be(newDesc);

            _mockContext.Verify(c => c.Teams.Update(existingTeam), Times.Once);
            _mockContext.Verify(c => c.SaveChanges(), Times.Once);
        }

        [Fact]
        public void SaveTeam_ShouldSetCreatedAt_WhenAddingNewTeam()
        {
            // Arrange
            var mockDbSet = new List<Team>().AsQueryable().BuildMockDbSet();
            _mockContext.Setup(c => c.Teams).Returns(mockDbSet.Object);

            var name = "New Team";
            var desc = "Description";
            var beforeCall = DateTime.Now;

            // Act
            _service.SaveTeam(null, name, desc);
            var afterCall = DateTime.Now;

            // Assert
            _mockContext.Verify(c => c.Teams.Add(It.Is<Team>(t =>
                t.CreatedAt >= beforeCall &&
                t.CreatedAt <= afterCall
            )), Times.Once);

            _mockContext.Verify(c => c.SaveChanges(), Times.Once);
        }
    }
}

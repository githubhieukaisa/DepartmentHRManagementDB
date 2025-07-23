using assignment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace assignment.Service
{
    public class ProjectService
    {
        private readonly IProjectManagementDbContext _context;
        public ProjectService(IProjectManagementDbContext context)
        {
            _context = context;
        }

        public void SaveProject(Project? existingProject, string name, string desc, DateTime start, DateTime end)
        {
            if (existingProject == null)
            {
                var project = new Project
                {
                    ProjectName = name,
                    Description = desc,
                    StartDate = DateOnly.FromDateTime(start),
                    EndDate = DateOnly.FromDateTime(end),
                };
                _context.Projects.Add(project);
            }
            else
            {
                existingProject.ProjectName = name;
                existingProject.Description = desc;
                existingProject.StartDate = DateOnly.FromDateTime(start);
                existingProject.EndDate = DateOnly.FromDateTime(end);
                _context.Projects.Update(existingProject);
            }

            _context.SaveChanges();
        }
    }
}

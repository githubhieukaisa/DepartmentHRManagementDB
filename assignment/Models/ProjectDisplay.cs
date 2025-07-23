using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace assignment.Models
{
    public class ProjectDisplay
    {
        public Project Project { get; set; } = null!;
        public string ProjectName => Project.ProjectName;
        public string Description => Project.Description;
        public DateOnly? StartDate => Project.StartDate;
        public DateOnly? EndDate => Project.EndDate;
        public Team Team => Project.Team!;
        public bool ShowMarkAsDone { get; set; }
    }
}

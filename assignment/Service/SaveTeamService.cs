using assignment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace assignment.Service
{
    public class SaveTeamService
    {
        private readonly IProjectManagementDbContext _context;
        public SaveTeamService(IProjectManagementDbContext context)
        {
            _context = context;
        }
        public void SaveTeam(Team? existingTeam, string name, string desc)
        {
            if (existingTeam == null)
            {
                var team = new Team
                {
                    TeamName = name,
                    Description = desc,
                    CreatedAt = DateTime.Now
                };
                _context.Teams.Add(team);
            }
            else
            {
                existingTeam.TeamName = name;
                existingTeam.Description = desc;
                _context.Teams.Update(existingTeam);
            }
            _context.SaveChanges();
        }
    }
}

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace assignment.Models
{
    public interface IProjectManagementDbContext
    {
        DbSet<Employee> Employees { get; set; }
        int SaveChanges();
        DbSet<Project> Projects { get; set; }
    }
}

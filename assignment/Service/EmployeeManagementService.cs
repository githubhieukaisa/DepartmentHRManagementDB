using assignment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace assignment.Service
{
    public class EmployeeManagementService
    {
        private readonly IProjectManagementDbContext _context;
        public EmployeeManagementService(IProjectManagementDbContext context)
        {
            _context = context;
        }

        public void SaveEmployee(Employee? existingEmp, string action, string fullname, string email, string password, int roleid, int departmentId, string status)
        {
            if(action=="Add")
            {
                var emp = new Employee
                {
                    FullName = fullname,
                    Email = email,
                    PasswordHash = password,
                    RoleId = roleid,
                    DepartmentId = departmentId,
                    Status = status
                };
                _context.Employees.Add(emp);
            }
            else
            {
                existingEmp.FullName = fullname;
                existingEmp.Email = email;
                existingEmp.PasswordHash = password;
                existingEmp.RoleId = roleid;
                existingEmp.DepartmentId = departmentId;
                existingEmp.Status = status;
                _context.Employees.Update(existingEmp);
            }

            _context.SaveChanges();
        }
    }
}

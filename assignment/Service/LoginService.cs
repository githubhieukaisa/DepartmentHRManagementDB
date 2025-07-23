using assignment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace assignment.Service
{
    public class LoginService
    {
        private readonly IProjectManagementDbContext _context;
        public LoginService(IProjectManagementDbContext context)
        {
            _context = context;
        }
        public Employee Login(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                throw new ArgumentException("Email and password are required.");

            var user = _context.Employees
                .FirstOrDefault(emp => emp.Email == email && emp.PasswordHash == password && emp.Status == "Active");

            if (user == null)
                throw new UnauthorizedAccessException("Invalid credentials.");

            return user;
        }
    }
}

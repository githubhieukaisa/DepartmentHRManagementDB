using assignment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace assignment.utility
{
    public static class ConstantClass
    {
        public readonly static ProjectManagementDbContext context = new();
        public readonly static int ADMIN_ROLE_ID = context.Roles
            .Where(r => r.RoleName == "System Admin")
            .Select(r => r.RoleId)
            .FirstOrDefault();
        public readonly static int EMPLOYEE_ROLE_ID = context.Roles
            .Where(r => r.RoleName == "Employee")
            .Select(r => r.RoleId)
            .FirstOrDefault();
        public readonly static int[] mainRoleId= new int[]
        {
            ADMIN_ROLE_ID,
            EMPLOYEE_ROLE_ID
        };
        public readonly static int secondaryRoleId = context.Roles
            .Where(r => !mainRoleId.Contains(r.RoleId))
            .Select(r => r.RoleId)
            .FirstOrDefault();
    }
}

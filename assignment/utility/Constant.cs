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
        public readonly static Role[] secondaryRoleId = context.Roles
            .Where(r => !mainRoleId.Contains(r.RoleId)).ToArray();
        public readonly static List<Models.TaskStatus> defaultStatusOfDev = context.TaskStatuses
            .Where(s => s.StatusName == "To Do" || s.StatusName == "Rejected" || s.StatusName == "In Progress" || s.StatusName == "Done")
            .ToList();
        public readonly static List<Models.TaskStatus> statusOfDev = context.TaskStatuses
            .Where(s => s.StatusName == "In Progress" || s.StatusName == "Done")
            .ToList();
        public readonly static List<Models.TaskStatus> defaultStatusOfTester = context.TaskStatuses
            .Where(s => s.StatusName == "Done" || s.StatusName == "Ready for Testing" || s.StatusName == "Verified" || s.StatusName == "Rejected")
            .ToList();
        public readonly static List<Models.TaskStatus> statusOfTester = context.TaskStatuses
            .Where(s => s.StatusName == "Ready for Testing" || s.StatusName == "Verified" || s.StatusName == "Rejected").ToList();
        public readonly static List<Models.TaskStatus> statusOfQA = context.TaskStatuses.Where(s=> s.StatusName=="To Do").ToList();

        public readonly static Dictionary<string, List<Models.TaskStatus>> roleStatusMap = new()
        {
            { "Developer", statusOfDev },
            { "Tester", statusOfTester },
            { "QA", statusOfQA }
        };
        public readonly static Dictionary<string, List<Models.TaskStatus>> roleDefaultStatusMap = new()
        {
            { "Developer", defaultStatusOfDev },
            { "Tester", defaultStatusOfTester }
        };
    }
}

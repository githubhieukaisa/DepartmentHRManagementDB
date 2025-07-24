using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace assignment.Service.ExcelService
{
    public class ExcelExporterFactory
    {
        public static IExcelExporter CreateExporter(string type)
        {
            if (type.Equals("employee", StringComparison.OrdinalIgnoreCase))
            {
                return new EmployeeExcelExporter();
            } else if(type.Equals("project", StringComparison.OrdinalIgnoreCase))
            {
                return new ProjectExcelExporter();
            }
            else
            {
                throw new ArgumentException("Invalid exporter type");
            }
        }
    }
}

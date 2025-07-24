using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml;
using System.IO;

namespace assignment.Service.ExcelService
{
    public interface IExcelExporter
    {
        void ExportToExcel(Dictionary<string,object> data, string filePath);
    }
}

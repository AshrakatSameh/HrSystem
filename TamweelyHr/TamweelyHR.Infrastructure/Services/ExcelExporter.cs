using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TamweelyHR.Application.DTOs.Employees;

namespace TamweelyHR.Infrastructure.Services
{
    public class ExcelExporter
    {
        public static byte[] ExportEmployees(List<EmployeeDto> employees)
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Employees");

            // Headers
            worksheet.Cell(1, 1).Value = "ID";
            worksheet.Cell(1, 2).Value = "First Name";
            worksheet.Cell(1, 3).Value = "Last Name";
            worksheet.Cell(1, 4).Value = "Email";
            worksheet.Cell(1, 5).Value = "Mobile";
            worksheet.Cell(1, 6).Value = "Date of Birth";
            worksheet.Cell(1, 7).Value = "Department";
            worksheet.Cell(1, 8).Value = "Job Title";
            worksheet.Cell(1, 9).Value = "Date of hire";

            // Style headers
            var headerRange = worksheet.Range(1, 1, 1, 8);
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;

            // Data
            for (int i = 0; i < employees.Count; i++)
            {
                var emp = employees[i];
                var row = i + 2;

                worksheet.Cell(row, 1).Value = emp.Id;
                worksheet.Cell(row, 2).Value = emp.FirstName;
                worksheet.Cell(row, 3).Value = emp.LastName;
                worksheet.Cell(row, 4).Value = emp.Email;
                worksheet.Cell(row, 5).Value = emp.PhoneNumber;
                worksheet.Cell(row, 6).Value = emp.DateOfBirth.ToString("yyyy-MM-dd");
                worksheet.Cell(row, 7).Value = emp.DepartmentName;
                worksheet.Cell(row, 8).Value = emp.JobName;
                worksheet.Cell(row, 9).Value = emp.HireDate.ToString("yyyy-MM-dd");
            }

            worksheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }
    }
}

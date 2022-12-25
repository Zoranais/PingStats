using OfficeOpenXml;
using PingStats.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PingStats
{
    public class ExcelOutput
    {
        private FileInfo _file;
        public ExcelOutput(FileInfo file)
        {
            _file = file;
        }
        public async void SaveExcelFileAsync(List<PingModel> ping)
        {
            DeleteIfExists(_file);
            using var package = new ExcelPackage(_file);
            var ws = package.Workbook.Worksheets.Add("Output");
            ws.Cells["A1"].LoadFromCollection(ping);
            await package.SaveAsync();
        }
        void DeleteIfExists(FileInfo file)
        {
            if (file.Exists)
            {
                file.Delete();
            }
        }
    }
}

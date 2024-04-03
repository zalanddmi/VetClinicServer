using Aspose.Cells;

namespace VetClinicServer.Utils
{
    public class ExcelConverter
    {
        public byte[] ExportToExcel(string name, IEnumerable<IEnumerable<object>> data, IEnumerable<string> columns)
        {
            if (data == null || !data.Any())
                throw new ArgumentException("Данные отсутствуют");
            if (columns == null || !columns.Any())
                throw new ArgumentException("Наименования столбцов отсутствуют");

            Workbook workbook = new();
            Worksheet worksheet = workbook.Worksheets[0];

            worksheet.Name = name;

            worksheet.Cells[0, 0].PutValue(name);

            int columnIndex = 0;
            foreach (string column in columns)
            {
                worksheet.Cells[1, columnIndex].PutValue(column);
                columnIndex++;
            }
            int rowIndex = 2;

            foreach (IEnumerable<object> rowData in data)
            {
                columnIndex = 0;
                foreach (object cellData in rowData)
                {
                    worksheet.Cells[rowIndex, columnIndex].PutValue(cellData);
                    columnIndex++;
                }
                rowIndex++;
            }
            worksheet.AutoFitColumns();
            MemoryStream stream = new();
            workbook.Save(stream, SaveFormat.Xlsx);
            stream.Position = 0;

            return stream.ToArray();
        }
    }
}

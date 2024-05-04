using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Hydra.Common.Repository.IService;
using Hydra.DatbaseLayer.IRepository;

namespace Hydra.Common.Repository.Service
{
    public class ReportService(IUnitOfWork unitOfWork) : IReportService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public string DownloadExcelFromJson(List<Dictionary<string, string>> jsonData, string reportName, string reportDescription, string fromDate, string toDate)
        {
            MemoryStream mem = new MemoryStream();

            using (SpreadsheetDocument document = SpreadsheetDocument.Create(mem, SpreadsheetDocumentType.Workbook))
            {
                WorkbookPart workbookPart = document.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();

                WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                worksheetPart.Worksheet = new Worksheet(new SheetData());

                Sheets sheets = document.WorkbookPart.Workbook.AppendChild(new Sheets());
                Sheet sheet = new Sheet { Id = document.WorkbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "Sheet1" };
                sheets.Append(sheet);
                Worksheet_Style(document);

                SheetData sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();

                int currentRowIndex = 1;

                int lastHeaderColumnIndex = jsonData.FirstOrDefault().Keys.Count() - 1;

                if (!string.IsNullOrEmpty(reportName))
                {
                    AppendToSheet(sheetData, reportName, currentRowIndex++, lastHeaderColumnIndex);
                }

                if (!string.IsNullOrEmpty(reportDescription))
                {
                    AppendToSheet(sheetData, reportDescription, currentRowIndex++, lastHeaderColumnIndex);
                }

                sheetData.AppendChild(new Row());
                currentRowIndex++;

                var headerRow = new Row();
                foreach (var columnName in jsonData.FirstOrDefault().Keys)
                {
                    var cell = new Cell() { DataType = CellValues.String, CellValue = new CellValue(columnName) };
                    cell.StyleIndex = 1;
                    headerRow.AppendChild(cell);
                }
                sheetData.AppendChild(headerRow);
                currentRowIndex++;


                foreach (var row in jsonData)
                {
                    var dataRow = new Row();
                    foreach (var column in row.Values)
                    {
                        var cell = new Cell() { DataType = CellValues.String, CellValue = new CellValue(column) };
                        cell.StyleIndex = 2;
                        dataRow.AppendChild(cell);
                    }
                    sheetData.AppendChild(dataRow);
                    currentRowIndex++;
                }
                worksheetPart.Worksheet.Save();
                workbookPart.Workbook.Save();
            }

            byte[] byteArray = mem.ToArray();
            string temp_inBase64 = Convert.ToBase64String(byteArray);
            return temp_inBase64;
        }

        public void AppendToSheet(SheetData sheetData, string value, int rowIndex, int mergeCount)
        {
            var titleRow = new Row();
            var titleCell = new Cell() { DataType = CellValues.String, CellValue = new CellValue(value) };

            string mergeReference = $"A{rowIndex}:{GetCellRef(mergeCount)}{rowIndex}";

            MergeCells mergeCells = sheetData.Descendants<MergeCells>().FirstOrDefault();
            if (mergeCells == null)
            {
                mergeCells = new MergeCells();
                sheetData.InsertAfter(mergeCells, sheetData.Descendants<SheetFormatProperties>().FirstOrDefault());
            }

            mergeCells.AppendChild(new MergeCell() { Reference = new StringValue(mergeReference) });
            titleCell.StyleIndex = 3;
            titleRow.AppendChild(titleCell);
            sheetData.AppendChild(titleRow);
        }

        public string GetCellRef(int count)
        {
            if (count < 1)
            {
                throw new ArgumentOutOfRangeException("count", "Count must be positive and non-zero.");
            }

            string cellRef = string.Empty;
            while (count > 0)
            {
                count--; // Decrementing count to make it 0-based
                cellRef = (char)('A' + (count % 26)) + cellRef;
                count /= 26;
            }
            return cellRef;
        }

        private static WorkbookStylesPart Worksheet_Style(SpreadsheetDocument document)
        {
            WorkbookStylesPart create_style = document.WorkbookPart.AddNewPart<WorkbookStylesPart>();
            Fonts fonts = new Fonts(
            new Font(),
            new Font(new Bold()),
            new Font()
            );

            Fills fills = new Fills(new Fill());
            Borders borders = new Borders(
                new Border(),
                new Border()
                {
                    LeftBorder = new LeftBorder()
                    {
                        Style = BorderStyleValues.Thin,
                        Color = new Color() { Indexed = (UInt32Value)64U },

                    },
                    RightBorder = new RightBorder()
                    {
                        Style = BorderStyleValues.Thin,
                        Color = new Color() { Indexed = (UInt32Value)64U },

                    },
                    BottomBorder = new BottomBorder()
                    {
                        Style = BorderStyleValues.Thin,
                        Color = new Color() { Indexed = (UInt32Value)64U },

                    },
                    TopBorder = new TopBorder()
                    {
                        Style = BorderStyleValues.Thin,
                        Color = new Color() { Indexed = (UInt32Value)64U },

                    }
                });

            CellFormats cellFormats = new CellFormats(
                new CellFormat(),
                new CellFormat { FontId = 1, FillId = 0, BorderId = 1, Alignment = new Alignment { Horizontal = HorizontalAlignmentValues.Center, Vertical = VerticalAlignmentValues.Center } },
                new CellFormat { FontId = 2, FillId = 0, BorderId = 1, Alignment = new Alignment { Horizontal = HorizontalAlignmentValues.Center, Vertical = VerticalAlignmentValues.Center } },
                new CellFormat { FontId = 1, FillId = 0, BorderId = 0, Alignment = new Alignment { Horizontal = HorizontalAlignmentValues.Center, Vertical = VerticalAlignmentValues.Center } }
            );
            create_style.Stylesheet = new Stylesheet(fonts, fills, borders, cellFormats);
            create_style.Stylesheet.Save();

            return create_style;
        }
    }
}

using DocumentFormat.OpenXml.Spreadsheet;

namespace Hydra.Common.Repository.IService
{
    public interface IReportService
    {
        string DownloadExcelFromJson(List<Dictionary<string, string>> jsonData, string reportName = null, string reportDescription = null, string fromDate = null, string toDate = null);

        void AppendToSheet(SheetData sheetData, string value, int rowIndex, int mergeCount);

        string GetCellRef(int count);
    }
}

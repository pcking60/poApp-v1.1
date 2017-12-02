using OfficeOpenXml;
using OfficeOpenXml.Table;
using PostOffice.Common.ViewModels;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;

namespace PostOffice.Common
{
    public static class ReportHelper
    {
        public static Task GenerateXls<T>(List<T> datasource, string filePath)
        {
            return Task.Run(() =>
            {
                using (ExcelPackage pck = new ExcelPackage(new FileInfo(filePath)))
                {
                    //Create the worksheet
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add(nameof(T));
                    ws.Cells["A1:H1"].Merge = true;
                    ws.Cells["A2:H2"].Merge = true;
                    ws.Cells["A1"].Value = "TỔNG CÔNG TY BƯU ĐIỆN VIỆT NAM";
                    ws.Cells["A2"].Value = "BƯU ĐIỆN TỈNH SÓC TRĂNG";
                    ws.Cells["A4"].LoadFromCollection<T>(datasource, true, TableStyles.Light1);
                    ws.Column(8).Style.Numberformat.Format = "dd/MM/yyyy";
                    ws.Cells.AutoFitColumns();
                    pck.Save();
                }
            });
        }

        public static Task StatisticXls<T>(List<T> datasource, string filePath, statisticReportViewModel vm)
        {
            return Task.Run(() =>
            {
                using (ExcelPackage pck = new ExcelPackage(new FileInfo(filePath)))
                {
                    //Create the worksheet
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add(nameof(T));

                    #region templateInfo

                    ws.Cells["A1:E1"].Merge = true;
                    ws.Cells["A1:E1"].Value = "TỔNG CÔNG TY BƯU ĐIỆN VIỆT NAM \n BƯU ĐIỆN TỈNH SÓC TRĂNG";
                    ws.Cells["A1:E1"].Style.WrapText = true;
                    ws.Cells["A3:E3"].Merge = true;
                    ws.Cells["A3:E3"].Value = "BÁO CÁO TỔNG HỢP";
                    ws.Row(1).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    ws.Row(1).Height = 35;
                    ws.Row(1).Style.Font.Bold = true;
                    ws.Row(3).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    ws.Row(3).Style.Font.Size = 13;
                    ws.Row(3).Style.Font.Bold = true;

                    // Custom fill
                    ws.Cells["C4:E4"].Merge = true;
                    ws.Cells["C4:E4"].Style.Font.Bold = true;
                    ws.Cells["C4:E4"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    ws.Cells["C4:E4"].Style.Indent = 2;
                    ws.Cells["C4:E4"].Value = vm.PoName;

                    ws.Cells["C5:E5"].Merge = true;
                    ws.Cells["C5:E5"].Style.Font.Bold = true;
                    ws.Cells["C5:E5"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    ws.Cells["C5:E5"].Style.Indent = 2;
                    ws.Cells["C5:E5"].Value = "Từ " + vm.fromDate + " đến " + vm.toDate;

                    ws.Cells["C6:E6"].Merge = true;
                    ws.Cells["C6:E6"].Style.Font.Bold = true;
                    ws.Cells["C6:E6"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    ws.Cells["C6:E6"].Style.Indent = 2;
                    ws.Cells["C6:E6"].Value = vm.ServiceName;

                    #endregion templateInfo

                    int noRow = datasource.Count;

                    ws.Cells["A8"].LoadFromCollection<T>(datasource, true, TableStyles.Light1);
                    ws.Column(8).Style.Numberformat.Format = "dd/MM/yyyy";
                    ws.Cells.AutoFitColumns();

                    ws.Cells[noRow + 10, 3, noRow + 10, 5].Merge = true;
                    ws.Cells[noRow + 10, 3, noRow + 10, 5].Value = "Người lập báo cáo";
                    ws.Cells[noRow + 10, 3, noRow + 10, 5].Style.Font.Bold = true;
                    ws.Cells[noRow + 10, 3, noRow + 10, 5].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                    ws.Cells[noRow + 14, 3, noRow + 14, 5].Merge = true;
                    ws.Cells[noRow + 14, 3, noRow + 14, 5].Value = vm.UserName;
                    ws.Cells[noRow + 14, 3, noRow + 14, 5].Style.Font.Bold = true;
                    ws.Cells[noRow + 14, 3, noRow + 14, 5].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                    ws.Cells[noRow + 16, 3, noRow + 16, 5].Merge = true;
                    ws.Cells[noRow + 16, 3, noRow + 16, 5].Value = DateTime.Now;
                    ws.Cells[noRow + 16, 3, noRow + 16, 5].Style.Numberformat.Format = "dd/MM/yyyy HH:mm:ss";
                    ws.Cells[noRow + 16, 3, noRow + 16, 5].Style.Font.Italic = true;
                    ws.Cells[noRow + 16, 3, noRow + 16, 5].Style.Font.Size = 10;
                    pck.Save();
                }
            });
        }

        /*
            code: RP1
            name: Bảng kê tổng hợp thu tiền tại bưu cục
        */

        public static Task RP1<T>(List<T> datasource, string filePath, ReportTemplate vm, IEnumerable<RP1Advance> rp1)
        {
            return Task.Run(() =>
            {
                using (ExcelPackage pck = new ExcelPackage(new FileInfo(filePath)))
                {
                    //Create the worksheet
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add(nameof(T));

                    #region templateInfo

                    // all
                    ws.Cells["A1:Z1000"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                    //header
                    ws.Cells["A1:E1"].Merge = true;
                    ws.Cells["A1:E1"].Value = "TỔNG CÔNG TY BƯU ĐIỆN VIỆT NAM \n BƯU ĐIỆN TỈNH SÓC TRĂNG";
                    ws.Row(1).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    ws.Row(1).Height = 45;
                    ws.Row(1).Style.Font.Bold = true;
                    ws.Row(1).Style.Font.Size = 15;

                    //functionName
                    ws.Cells["A1:E1"].Style.WrapText = true;
                    ws.Cells["A3:E3"].Merge = true;
                    ws.Cells["A3:E3"].Formula = "upper(\"" + vm.FunctionName.ToString() + "\")";

                    ws.Row(3).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    ws.Row(3).Style.Font.Size = 13;
                    ws.Row(3).Style.Font.Bold = true;

                    // Custom fill
                    //district
                    ws.Cells["C4:E4"].Merge = true;
                    ws.Cells["C4:E4"].Style.Font.Bold = true;
                    ws.Cells["C4:E4"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    ws.Cells["C4:E4"].Style.Indent = 2;
                    if (vm.District == null)
                    {
                        vm.District = "Tất cả";
                    }
                    ws.Cells["C4:E4"].Value = vm.District;

                    //unit
                    ws.Cells["C5:E5"].Merge = true;
                    ws.Cells["C5:E5"].Style.Font.Bold = true;
                    ws.Cells["C5:E5"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    ws.Cells["C5:E5"].Style.Indent = 2;
                    if (vm.Unit == null)
                    {
                        vm.Unit = "Tất cả";
                    }
                    ws.Cells["C5:e5"].Value = vm.Unit;

                    //time
                    ws.Cells["C6:E6"].Merge = true;
                    ws.Cells["C6:E6"].Style.Font.Bold = true;
                    ws.Cells["C6:E6"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    ws.Cells["C6:E6"].Style.Indent = 2;
                    ws.Cells["C6:E6"].Value = "Từ " + vm.FromDate.ToString("dd/MM/yyyy") + " đến " + vm.ToDate.ToString("dd/MM/yyyy"); ;

                    #endregion templateInfo

                    int noRow = datasource.Count;

                    // load data
                    ws.Cells["A8"].LoadFromCollection<T>(datasource, true, TableStyles.Light1);

                    //header
                    ws.Cells["A8"].Value = "STT";
                    ws.Cells["B8"].Value = "Nhóm dịch vụ";
                    ws.Cells["C8"].Value = "Doanh thu";
                    ws.Cells["D8"].Value = "Thuế";
                    ws.Cells["E8"].Value = "Tổng cộng";
                    ws.Cells["A8:E8"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    ws.Cells["A8:E8"].Style.Font.Bold = true;
                    ws.Cells[8, 1, 8, 5].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    ws.Cells[8, 1, 8, 5].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(236, 143, 50));

                    ws.Column(8).Style.Numberformat.Format = "dd/MM/yyyy";
                    ws.Cells.AutoFitColumns();

                    //format col 1
                    ws.Column(1).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                    //format col 3,4,5
                    //ws.Cells[9, 3, noRow + 10, 8].Style.Numberformat.Format = "#,##0.00";

                    //sum part 1
                    ws.Cells[noRow + 9, 2].Value = "Tổng cộng doanh thu";
                    ws.Cells[noRow + 9, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    ws.Row(noRow + 9).Style.Font.Bold = true;
                    ws.Cells[noRow + 9, 3].Formula = "sum(c9:c" + (noRow + 8) + ")";
                    ws.Cells[noRow + 9, 4].Formula = "sum(d9:d" + (noRow + 8) + ")";
                    ws.Cells[noRow + 9, 5].Formula = "sum(e9:e" + (noRow + 8) + ")";

                    //part 2
                    ws.Cells[noRow + 11, 2].Value = "Tiền giữ hộ";
                    ws.Cells[noRow + 11, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    ws.Cells[noRow + 11, 2].Style.Font.Bold = true;
                    ws.Cells[noRow + 12, 1].Value = "1";
                    ws.Cells[noRow + 13, 1].Value = "2";
                    ws.Cells[noRow + 12, 2].Value = "Phụ thu nước ngoài";
                    ws.Cells[noRow + 12, 1, noRow + 12, 5].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    ws.Cells[noRow + 12, 1, noRow + 12, 5].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(191, 191, 191));
                    ws.Cells[noRow + 13, 2].Value = "EMS Ngoại giao công vụ";

                    //fill data
                    int i = 12;
                    int j = 3;
                    foreach (var item in rp1)
                    {
                        ws.Cells[noRow + i, j].Value = item.Revenue;
                        ws.Cells[noRow + i, j + 1].Value = item.Tax;
                        ws.Cells[noRow + i, j + 2].Value = item.TotalMoney;
                        i++;
                    }
                    //format col 3,4,5
                    ws.Column(3).Style.Numberformat.Format = "#,##0.00";
                    ws.Column(4).Style.Numberformat.Format = "#,##0.00";
                    ws.Column(5).Style.Numberformat.Format = "#,##0.00";

                    //sum part 2
                    ws.Cells[noRow + 14, 2].Value = "Tổng tiền thu hộ";
                    ws.Cells[noRow + 14, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    ws.Row(noRow + 14).Style.Font.Bold = true;
                    ws.Cells[noRow + 14, 3].Formula = "sum(c" + (noRow + 12) + ":c" + (noRow + 13) + ")";
                    ws.Cells[noRow + 14, 4].Formula = "sum(d" + (noRow + 12) + ":d" + (noRow + 13) + ")";
                    ws.Cells[noRow + 14, 5].Formula = "sum(e" + (noRow + 12) + ":e" + (noRow + 13) + ")";

                    // ------Tổng thu---------
                    ws.Cells[noRow + 15, 2].Value = "TỔNG THU";
                    ws.Cells[noRow + 15, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    ws.Row(noRow + 15).Style.Font.Bold = true;
                    ws.Cells[noRow + 15, 3].Formula = "C" + (noRow + 9) + "+" + "C" + (noRow + 14);
                    ws.Cells[noRow + 15, 4].Formula = "D" + (noRow + 9) + "+" + "D" + (noRow + 14);
                    ws.Cells[noRow + 15, 5].Formula = "E" + (noRow + 9) + "+" + "E" + (noRow + 14);

                    #region template 2

                    //info
                    ws.Cells["A4:B4"].Merge = true;
                    ws.Cells["A4:B4"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                    ws.Row(4).Style.Font.Bold = true;
                    ws.Cells["A4:B4"].Value = "Huyện: ";
                    ws.Cells["A4:B4"].Style.Indent = 1;

                    ws.Cells["A5:B5"].Merge = true;
                    ws.Cells["A5:B5"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                    ws.Row(5).Style.Font.Bold = true;
                    ws.Cells["A5:B5"].Value = "Bưu cục: ";
                    ws.Cells["A5:B5"].Style.Indent = 1;

                    ws.Cells["A6:B6"].Merge = true;
                    ws.Cells["A6:B6"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                    ws.Row(6).Style.Font.Bold = true;
                    ws.Cells["A6:B6"].Value = "Thời gian:";
                    ws.Cells["A6:B6"].Style.Indent = 1;

                    //fix width
                    ws.Column(1).Width = 6;
                    ws.Column(2).Width = 40;
                    ws.Column(3).Width = 18;
                    ws.Column(4).Width = 14;
                    ws.Column(5).Width = 20;

                    //border table
                    ws.Cells[8, 1, noRow + 15, 5].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    ws.Cells[8, 1, noRow + 15, 5].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    ws.Cells[8, 1, noRow + 15, 5].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    ws.Cells[8, 1, noRow + 15, 5].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                    //signal
                    ws.Cells[noRow + 20, 1, noRow + 20, 2].Merge = true;
                    ws.Cells[noRow + 20, 1, noRow + 20, 2].Value = "Người lập bảng";
                    ws.Cells[noRow + 20, 1, noRow + 20, 2].Style.Font.Bold = true;
                    ws.Cells[noRow + 20, 1, noRow + 20, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                    ws.Cells[noRow + 21, 1, noRow + 21, 2].Merge = true;
                    ws.Cells[noRow + 21, 1, noRow + 21, 2].Value = vm.CreatedBy;
                    ws.Cells[noRow + 21, 1, noRow + 21, 2].Style.Font.Bold = true;
                    ws.Cells[noRow + 21, 1, noRow + 21, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                    ws.Cells[noRow + 20, 3, noRow + 20, 5].Merge = true;
                    ws.Cells[noRow + 20, 3, noRow + 20, 5].Value = "Người phê duyệt";
                    ws.Cells[noRow + 20, 3, noRow + 20, 5].Style.Font.Bold = true;
                    ws.Cells[noRow + 20, 3, noRow + 20, 5].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                    ws.Cells[noRow + 22, 3, noRow + 22, 5].Merge = true;
                    ws.Cells[noRow + 22, 3, noRow + 22, 5].Value = DateTime.Now;
                    ws.Cells[noRow + 22, 3, noRow + 22, 5].Style.Numberformat.Format = "dd/MM/yyyy HH:mm:ss";
                    ws.Cells[noRow + 22, 3, noRow + 22, 5].Style.Font.Italic = true;
                    ws.Cells[noRow + 22, 3, noRow + 22, 5].Style.Font.Size = 10;

                    #endregion template 2

                    pck.Save();
                }
            });
        }

        /*
            code:
            name: Export Bảng kê thu tiền tại bưu cục - tổng hợp
        */

        public static Task Export_By_Service_Group_And_Time<T1, T2, T3>(List<T1> bccpDataSource, List<T2> ppttDataSource, List<T3> tcbcDataSource, string filePath, ReportTemplate vm)
        {
            return Task.Run(() =>
            {
                using (ExcelPackage pck = new ExcelPackage(new FileInfo(filePath)))
                {
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Thống kê tổng hợp");

                    #region count data

                    int noRowBCCP = bccpDataSource.Count; //count number rows BCCP
                    int noRowPPTT = ppttDataSource.Count; // count row of PPTT
                    int noRowTCBC = tcbcDataSource.Count;  // count row of TCBC
                    //int noRowOther = otherDataSource.Count; // count row of OTHER

                    #endregion count data

                    #region BCCP

                    if (noRowBCCP > 0)
                    {
                        //load data source 1 BCCP start A9
                        ws.Cells["A10"].LoadFromCollection<T1>(bccpDataSource, true, TableStyles.Light1);
                        //fill STT
                        for (int i = 1; i <= noRowBCCP; i++)
                        {
                            ws.Cells["A" + (i + 10)].Value = i;
                        }

                        //format col 1
                        ws.Column(1).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        //Header Label BCCP
                        ws.Cells["A9:H9"].Merge = true;
                        ws.Cells["A9:H9"].Value = "I. Nhóm Bưu Chính Chuyển Phát";
                        ws.Cells["A9:H9"].Style.Font.Bold = true;
                        ws.Row(9).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;

                        //header
                        ws.Row(10).Height = 30;
                        ws.Cells["A10"].Value = "STT";
                        ws.Cells["B10"].Value = "Dịch vụ";
                        ws.Cells["C10"].Value = "Số \nlượng";
                        ws.Cells["D10"].Value = "Tiền mặt";
                        ws.Cells["E10"].Value = "Tiền nợ";
                        ws.Cells["F10"].Value = "Tổng \ndoanh thu \n trước thuế";
                        ws.Cells["G10"].Value = "Thuế VAT";
                        ws.Cells["H10"].Value = "Doanh thu \ntính lương";

                        ws.Cells["A10:H10"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        ws.Cells["A10:H10"].Style.Font.Bold = true;
                        ws.Cells[10, 1, 10, 8].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        ws.Cells[10, 1, 10, 8].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(236, 143, 50));

                        ws.Cells.AutoFitColumns();
                        ws.Row(10).Style.WrapText = true;
                        ws.Row(10).Height = 42.23;

                        ws.Cells["d11:H" + (noRowBCCP + 11)].Style.Numberformat.Format = "#,##0.00";

                        //sum group 1
                        ws.Cells[noRowBCCP + 11, 2].Value = "Tổng cộng";
                        ws.Cells[noRowBCCP + 11, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        ws.Row(noRowBCCP + 11).Style.Font.Bold = true;
                        ws.Cells[noRowBCCP + 11, 3].Formula = "sum(c11:c" + (noRowBCCP + 10) + ")";
                        ws.Cells[noRowBCCP + 11, 4].Formula = "sum(D11:D" + (noRowBCCP + 10) + ")";
                        ws.Cells[noRowBCCP + 11, 5].Formula = "sum(E11:E" + (noRowBCCP + 10) + ")";
                        ws.Cells[noRowBCCP + 11, 6].Formula = "sum(F11:F" + (noRowBCCP + 10) + ")";
                        ws.Cells[noRowBCCP + 11, 7].Formula = "sum(G11:G" + (noRowBCCP + 10) + ")";
                        ws.Cells[noRowBCCP + 11, 8].Formula = "sum(H11:H" + (noRowBCCP + 10) + ")";
                    }

                    #endregion BCCP

                    #region TCBC

                    if (noRowTCBC > 0)
                    {
                        if (noRowBCCP == 0) // BCCP khong co du lieu
                        {
                            ws.Cells["A9"].LoadFromCollection<T3>(tcbcDataSource, true, TableStyles.Light1);
                            ws.Cells["A9:H9"].Merge = true;
                            ws.Cells["A9"].Value = "II. Nhóm Tài Chính Bưu Chính";
                            ws.Cells["A9:H9"].Style.Font.Bold = true;
                            ws.Row(9).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                            //header
                            ws.Row(noRowBCCP + 14).Height = 50;
                            ws.Cells["A10"].Value = "STT";
                            ws.Cells["B10"].Value = "Dịch vụ";
                            ws.Cells["C10"].Value = "Số \nlượng";
                            ws.Cells["D10"].Value = "Số tiền \nthu hộ";
                            ws.Cells["E10"].Value = "Số tiền \nchi hộ";
                            ws.Cells["F10"].Value = "Số tiền \n cước";
                            ws.Cells["G10"].Value = "Doanh thu \n trước thuế";
                            ws.Cells["H10"].Value = "Thuế VAT";
                            ws.Cells["I10"].Value = "Doanh thu \ntính lương";
                            ws.Cells["A10" + ":I10"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            ws.Cells["A10" + ":I10"].Style.Font.Bold = true;
                            ws.Cells[14, 1, 14, 10].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            ws.Cells[14, 1, 14, 10].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(236, 143, 50));
                            ws.Row(14).Style.WrapText = true;
                            // fill STT
                            for (int i = 1; i <= noRowTCBC; i++)
                            {
                                ws.Cells["A" + (i + 10)].Value = i;
                            }
                            // sum source 2
                            ws.Cells[noRowTCBC + 11, 2].Value = "Tổng cộng";
                            ws.Cells[0 + noRowTCBC + 11, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            ws.Row(0 + noRowTCBC + 11).Style.Font.Bold = true;
                            ws.Cells[0 + noRowTCBC + 11, 3].Formula = "sum(c" + (11 + 0) + ":c" + (0 + noRowTCBC + 14) + ")";
                            ws.Cells[0 + noRowTCBC + 11, 4].Formula = "sum(D" + (11 + 0) + ":D" + (0 + noRowTCBC + 14) + ")";
                            ws.Cells[0 + noRowTCBC + 11, 5].Formula = "sum(E" + (11 + 0) + ":E" + (0 + noRowTCBC + 14) + ")";
                            ws.Cells[0 + noRowTCBC + 11, 6].Formula = "sum(F" + (11 + 0) + ":F" + (0 + noRowTCBC + 14) + ")";
                            ws.Cells[0 + noRowTCBC + 11, 7].Formula = "sum(G" + (11 + 0) + ":G" + (0 + noRowTCBC + 14) + ")";
                            ws.Cells[0 + noRowTCBC + 11, 8].Formula = "sum(H" + (11 + 0) + ":H" + (0 + noRowTCBC + 14) + ")";
                            ws.Cells[0 + noRowTCBC + 11, 9].Formula = "sum(I" + (11 + 0) + ":I" + (0 + noRowTCBC + 14) + ")";
                            ws.Cells[0 + 11, 3, 0 + noRowTCBC + 11, 9].Style.Numberformat.Format = "#,##0.00";
                        }
                        else
                        {
                            // load data source 2
                            ws.Cells["A" + (noRowBCCP + 14)].LoadFromCollection<T3>(tcbcDataSource, true, TableStyles.Light1);
                            ws.Cells["A" + (noRowBCCP + 13) + ":H" + (noRowBCCP + 13)].Merge = true;
                            ws.Cells["A" + (noRowBCCP + 13)].Value = "II. Nhóm Tài Chính Bưu Chính";
                            ws.Cells["A" + (noRowBCCP + 13) + ":H" + (noRowBCCP + 13)].Style.Font.Bold = true;
                            ws.Row(noRowBCCP + 13).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;

                            //header
                            ws.Row(noRowBCCP + 14).Height = 50;
                            ws.Cells["A" + (noRowBCCP + 14)].Value = "STT";
                            ws.Cells["B" + (noRowBCCP + 14)].Value = "Dịch vụ";
                            ws.Cells["C" + (noRowBCCP + 14)].Value = "Số \nlượng";
                            ws.Cells["D" + (noRowBCCP + 14)].Value = "Số tiền \nthu hộ";
                            ws.Cells["E" + (noRowBCCP + 14)].Value = "Số tiền \nchi hộ";
                            ws.Cells["F" + (noRowBCCP + 14)].Value = "Số tiền cước";
                            ws.Cells["G" + (noRowBCCP + 14)].Value = "Doanh thu \n trước thuế";
                            ws.Cells["H" + (noRowBCCP + 14)].Value = "Thuế vAT";
                            ws.Cells["I" + (noRowBCCP + 14)].Value = "Doanh thu \ntính lương";
                            ws.Cells["A" + (noRowBCCP + 14) + ":I" + (noRowBCCP + 14)].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            ws.Cells["A" + (noRowBCCP + 14) + ":I" + (noRowBCCP + 14)].Style.Font.Bold = true;
                            ws.Cells[(noRowBCCP + 14), 1, (noRowBCCP + 14), 9].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            ws.Cells[(noRowBCCP + 14), 1, (noRowBCCP + 14), 9].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(236, 143, 50));
                            ws.Row(noRowBCCP + 14).Style.WrapText = true;

                            // fill STT
                            for (int i = 1; i <= noRowTCBC; i++)
                            {
                                ws.Cells["A" + (i + noRowBCCP + 14)].Value = i;
                            }

                            // sum source 2
                            ws.Cells[noRowBCCP + noRowTCBC + 15, 2].Value = "Tổng cộng";
                            ws.Cells[noRowBCCP + noRowTCBC + 15, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            ws.Row(noRowBCCP + noRowTCBC + 15).Style.Font.Bold = true;
                            ws.Cells[noRowBCCP + noRowTCBC + 15, 3].Formula = "sum(c" + (15 + noRowBCCP) + ":c" + (noRowBCCP + noRowTCBC + 14) + ")";
                            ws.Cells[noRowBCCP + noRowTCBC + 15, 4].Formula = "sum(d" + (15 + noRowBCCP) + ":d" + (noRowBCCP + noRowTCBC + 14) + ")";
                            ws.Cells[noRowBCCP + noRowTCBC + 15, 5].Formula = "sum(e" + (15 + noRowBCCP) + ":e" + (noRowBCCP + noRowTCBC + 14) + ")";
                            ws.Cells[noRowBCCP + noRowTCBC + 15, 6].Formula = "sum(f" + (15 + noRowBCCP) + ":f" + (noRowBCCP + noRowTCBC + 14) + ")";
                            ws.Cells[noRowBCCP + noRowTCBC + 15, 7].Formula = "sum(g" + (15 + noRowBCCP) + ":g" + (noRowBCCP + noRowTCBC + 14) + ")";
                            ws.Cells[noRowBCCP + noRowTCBC + 15, 8].Formula = "sum(h" + (15 + noRowBCCP) + ":h" + (noRowBCCP + noRowTCBC + 14) + ")";
                            ws.Cells[noRowBCCP + noRowTCBC + 15, 9].Formula = "sum(i" + (15 + noRowBCCP) + ":i" + (noRowBCCP + noRowTCBC + 14) + ")";
                            ws.Cells[noRowBCCP + 15, 4, noRowBCCP + noRowTCBC + 15, 9].Style.Numberformat.Format = "#,##0.00";
                        }
                    }

                    #endregion TCBC

                    #region PPTT

                    if (noRowPPTT > 0)
                    {
                        //load data source 1 PPTT start from noRowBCCP + noRowBCCP+ 18
                        ws.Cells["A" + (noRowBCCP + noRowTCBC + 18)].LoadFromCollection<T2>(ppttDataSource, true, TableStyles.Light1);
                        //fill STT
                        for (int i = 1; i <= noRowPPTT; i++)
                        {
                            ws.Cells["A" + (i + noRowBCCP + noRowTCBC + 18)].Value = i;
                        }

                        //Header Label PPTT
                        ws.Cells["A" + (noRowBCCP + noRowTCBC + 17) + ":H" + (noRowBCCP + noRowTCBC + 17)].Merge = true;
                        ws.Cells["A" + (noRowBCCP + noRowTCBC + 17)].Value = "III. Phân phối truyền thông";
                        ws.Cells["A" + (noRowBCCP + noRowTCBC + 17) + ":H" + (noRowBCCP + noRowTCBC + 17)].Style.Font.Bold = true;
                        ws.Row(noRowBCCP + noRowTCBC + 17).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;

                        //header
                        ws.Row(noRowBCCP + noRowTCBC + 18).Height = 30;
                        ws.Cells["A" + (noRowBCCP + noRowTCBC + 18)].Value = "STT";
                        ws.Cells["B" + (noRowBCCP + noRowTCBC + 18)].Value = "Dịch vụ";
                        ws.Cells["C" + (noRowBCCP + noRowTCBC + 18)].Value = "Số \nlượng";
                        ws.Cells["D" + (noRowBCCP + noRowTCBC + 18)].Value = "Tiền mặt";
                        ws.Cells["E" + (noRowBCCP + noRowTCBC + 18)].Value = "Tiền nợ";
                        ws.Cells["F" + (noRowBCCP + noRowTCBC + 18)].Value = "Tổng \ndoanh thu \n trước thuế";
                        ws.Cells["G" + (noRowBCCP + noRowTCBC + 18)].Value = "Thuế VAT";
                        ws.Cells["H" + (noRowBCCP + noRowTCBC + 18)].Value = "Doanh thu \ntính lương";

                        ws.Cells["A" + (noRowBCCP + noRowTCBC + 18) + ":H" + (noRowBCCP + noRowTCBC + 18)].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        ws.Cells["A" + (noRowBCCP + noRowTCBC + 18) + ":H" + (noRowBCCP + noRowTCBC + 18)].Style.Font.Bold = true;
                        ws.Cells[noRowBCCP + noRowTCBC + 18, 1, noRowBCCP + noRowTCBC + 18, 8].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        ws.Cells[noRowBCCP + noRowTCBC + 18, 1, noRowBCCP + noRowTCBC + 18, 8].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(236, 143, 50));

                        ws.Cells.AutoFitColumns();
                        ws.Row(noRowBCCP + noRowTCBC + 18).Style.WrapText = true;

                        ws.Cells["C" + (noRowBCCP + noRowTCBC + 19) + ":H" + (noRowBCCP + noRowTCBC + 19 + noRowPPTT)].Style.Numberformat.Format = "#,##0.00";

                        //sum group 1
                        ws.Cells[noRowBCCP + noRowTCBC + 19 + noRowPPTT, 2].Value = "Tổng cộng";
                        ws.Cells[noRowBCCP + noRowTCBC + 19 + noRowPPTT, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        ws.Cells[noRowBCCP + noRowTCBC + 19 + noRowPPTT, 3].Formula = "sum(c" + (noRowBCCP + noRowTCBC + 19) + ":c" + (noRowBCCP + noRowTCBC + 18 + noRowPPTT) + ")";
                        ws.Cells[noRowBCCP + noRowTCBC + 19 + noRowPPTT, 4].Formula = "sum(d" + (noRowBCCP + noRowTCBC + 19) + ":d" + (noRowBCCP + noRowTCBC + 18 + noRowPPTT) + ")";
                        ws.Cells[noRowBCCP + noRowTCBC + 19 + noRowPPTT, 5].Formula = "sum(e" + (noRowBCCP + noRowTCBC + 19) + ":e" + (noRowBCCP + noRowTCBC + 18 + noRowPPTT) + ")";
                        ws.Cells[noRowBCCP + noRowTCBC + 19 + noRowPPTT, 6].Formula = "sum(f" + (noRowBCCP + noRowTCBC + 19) + ":f" + (noRowBCCP + noRowTCBC + 18 + noRowPPTT) + ")";
                        ws.Cells[noRowBCCP + noRowTCBC + 19 + noRowPPTT, 7].Formula = "sum(g" + (noRowBCCP + noRowTCBC + 19) + ":g" + (noRowBCCP + noRowTCBC + 18 + noRowPPTT) + ")";
                        ws.Cells[noRowBCCP + noRowTCBC + 19 + noRowPPTT, 8].Formula = "sum(h" + (noRowBCCP + noRowTCBC + 19) + ":h" + (noRowBCCP + noRowTCBC + 18 + noRowPPTT) + ")";
                        ws.Row(noRowBCCP + noRowTCBC + 19 + noRowPPTT).Style.Font.Bold = true;
                    }

                    #endregion PPTT

                    #region templateInfo

                    // all
                    ws.PrinterSettings.TopMargin = 2 / 2.54M;
                    ws.PrinterSettings.BottomMargin = 1 / 2.54M;
                    ws.PrinterSettings.LeftMargin = (decimal)0.8 / 2.54M;
                    ws.PrinterSettings.RightMargin = (decimal)0.8 / 2.54M;
                    ws.PrinterSettings.Orientation = eOrientation.Landscape;
                    ws.Cells["A1:Z1000"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    ws.Cells["A1:Z1000"].Style.Font.SetFromFont(new Font("Segoe UI", 9));
                    ws.Cells["A1:Z1000"].AutoFitColumns();
                    //header
                    ws.Cells["A1:C1"].Merge = true;
                    ws.Cells["A1:C1"].Value = "TỔNG CÔNG TY BƯU ĐIỆN VIỆT NAM \n BƯU ĐIỆN TỈNH SÓC TRĂNG";
                    ws.Row(1).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    ws.Row(1).Height = 29.35;
                    ws.Row(1).Style.Font.Bold = true;
                    //functionName
                    ws.Cells["A1:H1"].Style.WrapText = true;
                    ws.Cells["A3:H3"].Merge = true;
                    ws.Cells["A3:H3"].Formula = "upper(\"" + vm.FunctionName.ToString() + "\")";
                    ws.Row(3).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    ws.Row(3).Style.Font.Bold = true;

                    // fill district
                    ws.Cells["C4:H4"].Merge = true;
                    ws.Cells["C4:H4"].Style.Font.Bold = true;
                    ws.Cells["C4:H4"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    ws.Cells["C4:H4"].Style.Indent = 2;
                    if (vm.District == null)
                    {
                        vm.District = "Tất cả";
                    }
                    ws.Cells["C4:H4"].Value = vm.District;

                    // fill unit
                    ws.Cells["C5:H5"].Merge = true;
                    ws.Cells["C5:H5"].Style.Font.Bold = true;
                    ws.Cells["C5:H5"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    ws.Cells["C5:H5"].Style.Indent = 2;
                    if (vm.Unit == null)
                    {
                        vm.Unit = "Tất cả";
                    }
                    ws.Cells["C5:H5"].Value = vm.Unit;

                    // fill time
                    ws.Cells["C6:H6"].Merge = true;
                    ws.Cells["C6:H6"].Style.Font.Bold = true;
                    ws.Cells["C6:H6"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    ws.Cells["C6:H6"].Style.Indent = 2;
                    ws.Cells["C6:H6"].Value = "Từ " + vm.FromDate.ToString("dd/MM/yyyy") + " đến " + vm.ToDate.ToString("dd/MM/yyyy");

                    //info
                    ws.Cells["A4:B4"].Merge = true;
                    ws.Cells["A4:B4"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                    ws.Row(4).Style.Font.Bold = true;
                    ws.Cells["A4:B4"].Value = "TP/ Huyện: ";
                    ws.Cells["A4:B4"].Style.Indent = 1;

                    ws.Cells["A5:B5"].Merge = true;
                    ws.Cells["A5:B5"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                    ws.Row(5).Style.Font.Bold = true;
                    ws.Cells["A5:B5"].Value = "Bưu cục/ VHX: ";
                    ws.Cells["A5:B5"].Style.Indent = 1;

                    ws.Cells["A6:B6"].Merge = true;
                    ws.Cells["A6:B6"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                    ws.Row(6).Style.Font.Bold = true;
                    ws.Cells["A6:B6"].Value = "Thời gian:";
                    ws.Cells["A6:B6"].Style.Indent = 1;

                    #endregion templateInfo

                    if (noRowBCCP == 0 && noRowTCBC == 0 && noRowPPTT == 00)
                    {
                        ws.Cells["A10"].Value = "Không có dữ liệu";
                    }
                    ws.Column(1).Width = 4;
                    ws.Column(2).Style.WrapText = true;
                    ws.Column(3).Width = 6.5;
                    ws.Column(4).Width = 14.86;
                    ws.Column(5).Width = 13.5;
                    ws.Column(6).Width = 14.86;
                    ws.Column(7).Width = 12.23;
                    ws.Column(8).Width = 14.86;
                    ws.Column(9).Width = 14.86;
                    ws.Column(8).Width = 14.86;
                    pck.Save();
                }
            });
        }

        /*
            code: RP2_1
            name: Bảng kê thu tiền tại bưu cục - chi tiết
        */

        public static Task RP2_1<T1, T2, T3>(List<T1> datasource1, List<T2> datasource2, List<T3> datasource3, string filePath, ReportTemplate vm)
        {
            return Task.Run(() =>
            {
                using (ExcelPackage pck = new ExcelPackage(new FileInfo(filePath)))
                {
                    //Create the worksheet
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add(nameof(T1));

                    #region templateInfo

                    // all
                    ws.PrinterSettings.TopMargin = 2 / 2.54M;
                    ws.PrinterSettings.BottomMargin = 1 / 2.54M;
                    ws.PrinterSettings.LeftMargin = (decimal)0.8 / 2.54M;
                    ws.PrinterSettings.RightMargin = (decimal)0.8 / 2.54M;
                    ws.PrinterSettings.Orientation = eOrientation.Landscape;
                    ws.Cells["A1:Z1000"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    ws.Cells["A1:Z1000"].Style.Font.SetFromFont(new Font("Segoe UI", 9));
                    ws.Cells["A1:Z1000"].AutoFitColumns();
                    //header
                    ws.Cells["A1:B1"].Merge = true;
                    ws.Cells["A1:B1"].Value = "TỔNG CÔNG TY BƯU ĐIỆN VIỆT NAM \n BƯU ĐIỆN TỈNH SÓC TRĂNG";
                    ws.Row(1).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    ws.Row(1).Height = 29.35;
                    ws.Row(1).Style.Font.Bold = true;
                    //functionName
                    ws.Cells["A1:H1"].Style.WrapText = true;
                    ws.Cells["A3:H3"].Merge = true;
                    ws.Cells["A3:H3"].Formula = "upper(\"" + vm.FunctionName.ToString() + "\")";
                    ws.Row(3).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    ws.Row(3).Style.Font.Bold = true;

                    // fill district
                    ws.Cells["C4:H4"].Merge = true;
                    ws.Cells["C4:H4"].Style.Font.Bold = true;
                    ws.Cells["C4:H4"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    ws.Cells["C4:H4"].Style.Indent = 2;
                    if (vm.District == null)
                    {
                        vm.District = "Tất cả";
                    }
                    ws.Cells["C4:H4"].Value = vm.District;

                    // fill unit
                    ws.Cells["C5:H5"].Merge = true;
                    ws.Cells["C5:H5"].Style.Font.Bold = true;
                    ws.Cells["C5:H5"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    ws.Cells["C5:H5"].Style.Indent = 2;
                    if (vm.Unit == null)
                    {
                        vm.Unit = "Tất cả";
                    }
                    ws.Cells["C5:H5"].Value = vm.Unit;

                    // fill time
                    ws.Cells["C6:H6"].Merge = true;
                    ws.Cells["C6:H6"].Style.Font.Bold = true;
                    ws.Cells["C6:H6"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    ws.Cells["C6:H6"].Style.Indent = 2;
                    ws.Cells["C6:H6"].Value = "Từ " + vm.FromDate.ToString("dd/MM/yyyy") + " đến " + vm.ToDate.ToString("dd/MM/yyyy");

                    //info
                    ws.Cells["A4:B4"].Merge = true;
                    ws.Cells["A4:B4"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                    ws.Row(4).Style.Font.Bold = true;
                    ws.Cells["A4:B4"].Value = "Huyện: ";
                    ws.Cells["A4:B4"].Style.Indent = 1;

                    ws.Cells["A5:B5"].Merge = true;
                    ws.Cells["A5:B5"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                    ws.Row(5).Style.Font.Bold = true;
                    ws.Cells["A5:B5"].Value = "Bưu cục: ";
                    ws.Cells["A5:B5"].Style.Indent = 1;

                    ws.Cells["A6:B6"].Merge = true;
                    ws.Cells["A6:B6"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                    ws.Row(6).Style.Font.Bold = true;
                    ws.Cells["A6:B6"].Value = "Thời gian:";
                    ws.Cells["A6:B6"].Style.Indent = 1;

                    #endregion templateInfo

                    #region count data

                    //count number rows BCCP
                    int noRow = datasource1.Count;
                    // count row of TCBC
                    int noRow2 = datasource3.Count;
                    // count row of PPTT
                    int noRow3 = datasource2.Count;

                    #endregion count data

                    #region BCCP

                    //format number
                    if (noRow > 0)
                    {
                        //load data source 1
                        ws.Cells["A9"].LoadFromCollection<T1>(datasource1, true, TableStyles.Light1);
                        //fill STT
                        for (int i = 1; i <= noRow; i++)
                        {
                            ws.Cells["A" + (i + 9)].Value = i;
                        }

                        //format col 1
                        ws.Column(1).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        //Header BCCP
                        ws.Cells["A8:h8"].Merge = true;
                        ws.Cells["A8:h8"].Value = "I. Nhóm Bưu Chính Chuyển Phát";
                        ws.Cells["A8:h8"].Style.Font.Bold = true;
                        ws.Row(8).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;

                        //header
                        ws.Row(9).Height = 35;
                        ws.Cells["A9"].Value = "STT";
                        ws.Cells["B9"].Value = "Dịch vụ";
                        ws.Cells["C9"].Value = "Số \nlượng";
                        ws.Cells["D9"].Value = "Tiền mặt";
                        ws.Cells["E9"].Value = "Tiền nợ";
                        ws.Cells["F9"].Value = "Tổng \ndoanh thu \n trước thuế";
                        ws.Cells["G9"].Value = "Thuế VAT";
                        ws.Cells["H9"].Value = "DTTL";

                        ws.Cells["A9:H9"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        ws.Cells["A9:H9"].Style.Font.Bold = true;
                        ws.Cells[9, 1, 9, 8].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        ws.Cells[9, 1, 9, 8].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(236, 143, 50));

                        ws.Cells.AutoFitColumns();
                        ws.Row(9).Style.WrapText = true;

                        ws.Cells["d10:h" + (noRow + 10)].Style.Numberformat.Format = "#,##0.00";

                        //sum group 1
                        ws.Cells[noRow + 10, 2].Value = "Tổng cộng";
                        ws.Cells[noRow + 10, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        ws.Row(noRow + 10).Style.Font.Bold = true;
                        ws.Cells[noRow + 10, 3].Formula = "sum(c10:c" + (noRow + 9) + ")";
                        ws.Cells[noRow + 10, 4].Formula = "sum(d10:d" + (noRow + 9) + ")";
                        ws.Cells[noRow + 10, 5].Formula = "sum(e10:e" + (noRow + 9) + ")";
                        ws.Cells[noRow + 10, 6].Formula = "sum(f10:f" + (noRow + 9) + ")";
                        ws.Cells[noRow + 10, 7].Formula = "sum(g10:g" + (noRow + 9) + ")";
                        ws.Cells[noRow + 10, 8].Formula = "sum(h10:h" + (noRow + 9) + ")";
                    }

                    #endregion BCCP

                    #region TCBC

                    if (noRow2 > 0)
                    {
                        // load data source 2
                        ws.Cells["A" + (noRow + 13)].LoadFromCollection<T3>(datasource3, true, TableStyles.Light1);
                        ws.Cells["A" + (noRow + 12) + ":I" + (noRow + 12)].Merge = true;
                        ws.Cells["A" + (noRow + 12) + ":I" + (noRow + 12)].Value = "II. Nhóm Tài Chính Bưu Chính";
                        ws.Cells["A" + (noRow + 12) + ":I" + (noRow + 12)].Style.Font.Bold = true;
                        ws.Row(noRow + 12).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;

                        //header
                        ws.Row(noRow + 13).Height = 30;
                        ws.Cells["A" + (noRow + 13)].Value = "STT";
                        ws.Cells["B" + (noRow + 13)].Value = "Dịch vụ";
                        ws.Cells["C" + (noRow + 13)].Value = "Số \nlượng";
                        ws.Cells["D" + (noRow + 13)].Value = "Số tiền \nthu hộ";
                        ws.Cells["E" + (noRow + 13)].Value = "Số tiền \nchi hộ";
                        ws.Cells["F" + (noRow + 13)].Value = "Số tiền \ncước";
                        ws.Cells["G" + (noRow + 13)].Value = "Doanh thu \ntrước thuế";
                        ws.Cells["H" + (noRow + 13)].Value = "Thuế VAT";
                        ws.Cells["I" + (noRow + 13)].Value = "Doanh thu \ntính lương";
                        ws.Cells["A" + (noRow + 13) + ":I" + (noRow + 13)].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        ws.Cells["A" + (noRow + 13) + ":I" + (noRow + 13)].Style.Font.Bold = true;
                        ws.Cells[(noRow + 13), 1, (noRow + 13), 9].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        ws.Cells[(noRow + 13), 1, (noRow + 13), 9].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(236, 143, 50));
                        ws.Row(noRow + 13).Style.WrapText = true;

                        // fill STT
                        for (int i = 1; i <= noRow2; i++)
                        {
                            ws.Cells["A" + (i + noRow + 13)].Value = i;
                        }

                        // sum source 2
                        ws.Cells[noRow + noRow2 + 14, 2].Value = "Tổng cộng";
                        ws.Cells[noRow + noRow2 + 14, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        ws.Row(noRow + noRow2 + 14).Style.Font.Bold = true;
                        ws.Cells[noRow + noRow2 + 14, 3].Formula = "sum(c" + (14 + noRow) + ":c" + (noRow + noRow2 + 13) + ")";
                        ws.Cells[noRow + noRow2 + 14, 4].Formula = "sum(d" + (14 + noRow) + ":d" + (noRow + noRow2 + 13) + ")";
                        ws.Cells[noRow + noRow2 + 14, 5].Formula = "sum(e" + (14 + noRow) + ":e" + (noRow + noRow2 + 13) + ")";
                        ws.Cells[noRow + noRow2 + 14, 6].Formula = "sum(f" + (14 + noRow) + ":f" + (noRow + noRow2 + 13) + ")";
                        ws.Cells[noRow + noRow2 + 14, 7].Formula = "sum(g" + (14 + noRow) + ":g" + (noRow + noRow2 + 13) + ")";
                        ws.Cells[noRow + noRow2 + 14, 8].Formula = "sum(h" + (14 + noRow) + ":h" + (noRow + noRow2 + 13) + ")";
                        ws.Cells[noRow + noRow2 + 14, 9].Formula = "sum(i" + (14 + noRow) + ":i" + (noRow + noRow2 + 13) + ")";
                        ws.Cells[noRow + 14, 4, noRow + noRow2 + 14, 9].Style.Numberformat.Format = "#,##0.00";
                    }

                    #endregion TCBC

                    #region PPTT

                    if (noRow3 > 0)
                    {
                        // load data source 2
                        ws.Cells["A" + (noRow + noRow2 + 17)].LoadFromCollection<T2>(datasource2, true, TableStyles.Light1);
                        ws.Cells["A" + (noRow + noRow2 + 16) + ":I" + (noRow + noRow2 + 16)].Merge = true;
                        ws.Cells["A" + (noRow + noRow2 + 16) + ":I" + (noRow + noRow2 + 16)].Value = "III. Nhóm Phân Phối Truyền Thông";
                        ws.Cells["A" + (noRow + noRow2 + 16) + ":I" + (noRow + noRow2 + 16)].Style.Font.Bold = true;
                        ws.Row(noRow + noRow2 + 16).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;

                        //header
                        ws.Row(noRow + noRow2 + 17).Height = 30;
                        ws.Cells["A" + (noRow + noRow2 + 17)].Value = "STT";
                        ws.Cells["B" + (noRow + noRow2 + 17)].Value = "Dịch vụ";
                        ws.Cells["C" + (noRow + noRow2 + 17)].Value = "Số \nlượng";
                        ws.Cells["D" + (noRow + noRow2 + 17)].Value = "Tiền mặt";
                        ws.Cells["E" + (noRow + noRow2 + 17)].Value = "Tiền nợ";
                        ws.Cells["F" + (noRow + noRow2 + 17)].Value = "Doanh thu\n trước thuế";
                        ws.Cells["G" + (noRow + noRow2 + 17)].Value = "Thuế VAT";
                        ws.Cells["H" + (noRow + noRow2 + 17)].Value = "Doanh thu \ntính lương";
                        ws.Cells["A" + (noRow + noRow2 + 17) + ":H" + (noRow + noRow2 + 17)].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        ws.Cells["A" + (noRow + noRow2 + 17) + ":H" + (noRow + noRow2 + 17)].Style.Font.Bold = true;
                        ws.Cells[(noRow + noRow2 + 17), 1, (noRow + noRow2 + 17), 8].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        ws.Cells[(noRow + noRow2 + 17), 1, (noRow + noRow2 + 17), 8].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(236, 143, 50));
                        ws.Row(noRow + noRow2 + 17).Style.WrapText = true;

                        // fill STT
                        for (int i = 1; i <= noRow3; i++)
                        {
                            ws.Cells["A" + (i + noRow + noRow2 + 17)].Value = i;
                        }

                        // sum source 2
                        ws.Cells[noRow + noRow2 + noRow3 + 18, 2].Value = "Tổng cộng";
                        ws.Cells[noRow + noRow2 + noRow3 + 18, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        ws.Row(noRow + noRow2 + noRow3 + 18).Style.Font.Bold = true;
                        ws.Cells[noRow + noRow2 + noRow3 + 18, 3].Formula = "sum(c" + (18 + noRow + noRow2) + ":c" + (noRow + noRow2 + noRow3 + 17) + ")";
                        ws.Cells[noRow + noRow2 + noRow3 + 18, 4].Formula = "sum(d" + (18 + noRow + noRow2) + ":d" + (noRow + noRow2 + noRow3 + 17) + ")";
                        ws.Cells[noRow + noRow2 + noRow3 + 18, 5].Formula = "sum(e" + (18 + noRow + noRow2) + ":e" + (noRow + noRow2 + noRow3 + 17) + ")";
                        ws.Cells[noRow + noRow2 + noRow3 + 18, 6].Formula = "sum(f" + (18 + noRow + noRow2) + ":f" + (noRow + noRow2 + noRow3 + 17) + ")";
                        ws.Cells[noRow + noRow2 + noRow3 + 18, 7].Formula = "sum(g" + (18 + noRow + noRow2) + ":g" + (noRow + noRow2 + noRow3 + 17) + ")";
                        ws.Cells[noRow + noRow2 + noRow3 + 18, 8].Formula = "sum(h" + (18 + noRow + noRow2) + ":h" + (noRow + noRow2 + noRow3 + 17) + ")";
                        ws.Cells[noRow + noRow2 + 18, 3, noRow + noRow2 + noRow3 + 18, 9].Style.Numberformat.Format = "#,##0.00";
                    }

                    #endregion PPTT

                    #region fix width

                    //fix width
                    ws.Column(1).Width = 5;
                    ws.Column(2).Style.WrapText = true;
                    ws.Column(3).Width = 7.5;
                    ws.Column(4).Width = 15.86;
                    ws.Column(5).Width = 14.5;
                    ws.Column(6).Width = 15.86;
                    ws.Column(7).Width = 13.23;
                    ws.Column(8).Width = 15.86;
                    ws.Column(9).Width = 15.86;
                    //ws.Column(4).Style.WrapText = true;
                    //ws.Column(5).Style.WrapText = true;
                    //ws.Column(6).Style.WrapText = true;
                    //ws.Column(7).Style.WrapText = true;
                    //ws.Column(8).Style.WrapText = true;
                    //ws.Column(9).Style.WrapText = true;

                    #endregion fix width

                    //border table
                    //ws.Cells[8, 1, noRow + 15, 9].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    //ws.Cells[8, 1, noRow + 15, 9].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    //ws.Cells[8, 1, noRow + 15, 9].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    //ws.Cells[8, 1, noRow + 15, 9].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                    #region Signal

                    //signal
                    ws.Cells[noRow + noRow2 + noRow3 + 23, 1, noRow + noRow2 + noRow3 + 23, 2].Merge = true;
                    ws.Cells[noRow + noRow2 + noRow3 + 23, 1, noRow + noRow2 + noRow3 + 23, 2].Value = "Người lập bảng";
                    ws.Cells[noRow + noRow2 + noRow3 + 23, 1, noRow + noRow2 + noRow3 + 23, 2].Style.Font.Bold = true;
                    ws.Cells[noRow + noRow2 + noRow3 + 23, 1, noRow + noRow2 + noRow3 + 23, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                    ws.Cells[noRow + noRow2 + noRow3 + 26, 1, noRow + noRow2 + noRow3 + 26, 2].Merge = true;
                    ws.Cells[noRow + noRow2 + noRow3 + 26, 1, noRow + noRow2 + noRow3 + 26, 2].Value = vm.CreatedBy;
                    ws.Cells[noRow + noRow2 + noRow3 + 26, 1, noRow + noRow2 + noRow3 + 26, 2].Style.Font.Bold = true;
                    ws.Cells[noRow + noRow2 + noRow3 + 26, 1, noRow + noRow2 + noRow3 + 26, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                    ws.Cells[noRow + noRow2 + noRow3 + 23, 7, noRow + noRow2 + noRow3 + 23, 9].Merge = true;
                    ws.Cells[noRow + noRow2 + noRow3 + 23, 7, noRow + noRow2 + noRow3 + 23, 9].Value = "Người phê duyệt";
                    ws.Cells[noRow + noRow2 + noRow3 + 23, 7, noRow + noRow2 + noRow3 + 23, 9].Style.Font.Bold = true;
                    ws.Cells[noRow + noRow2 + noRow3 + 23, 7, noRow + noRow2 + noRow3 + 23, 9].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                    ws.Cells[noRow + noRow2 + noRow3 + 27, 3, noRow + noRow2 + noRow3 + 27, 9].Merge = true;
                    ws.Cells[noRow + noRow2 + noRow3 + 27, 3, noRow + noRow2 + noRow3 + 27, 9].Value = DateTime.Now;
                    ws.Cells[noRow + noRow2 + noRow3 + 27, 3, noRow + noRow2 + noRow3 + 27, 9].Style.Numberformat.Format = "dd/MM/yyyy HH:mm:ss";
                    ws.Cells[noRow + noRow2 + noRow3 + 27, 3, noRow + noRow2 + noRow3 + 27, 9].Style.Font.Italic = true;
                    ws.Cells[noRow + noRow2 + noRow3 + 27, 3, noRow + noRow2 + noRow3 + 27, 9].Style.Font.Size = 10;

                    #endregion Signal

                    pck.Save();
                }
            });
        }

        /*
            code:
            name: Export Bảng kê TKBD - tổng hợp
        */

        public static Task TKBD_Export_General<T1>(List<T1> dataSource, string filePath, Export_Info_Template vm)
        {
            return Task.Run(() =>
            {
                using (ExcelPackage pck = new ExcelPackage(new FileInfo(filePath)))
                {
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Thống kê tổng hợp");

                    #region count data

                    int noRowTKBD = dataSource.Count; //count number rows TKBD

                    #endregion count data

                    #region TKBD General

                    if (noRowTKBD > 0)
                    {
                        //load data source 1 TKBD start A9
                        ws.Cells["A10"].LoadFromCollection<T1>(dataSource, true, TableStyles.Light1);
                        //fill STT
                        for (int i = 1; i <= noRowTKBD; i++)
                        {
                            ws.Cells["A" + (i + 10)].Value = i;
                        }

                        //format col 1
                        ws.Column(1).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                        //header
                        ws.Row(10).Height = 30;
                        ws.Cells["A10"].Value = "STT";
                        ws.Cells["B10"].Value = "Tháng";
                        ws.Cells["C10"].Value = "Số \nlượng";
                        ws.Cells["D10"].Value = "Số dư cuối kỳ";
                        ws.Cells["E10"].Value = "Doanh thu \n tính lương";
                        ws.Cells["F10"].Value = "Mã \n nhân viên";
                        ws.Cells["G10"].Value = "Tên \n nhân viên";

                        ws.Cells["A10:G10"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        ws.Cells["A10:G10"].Style.Font.Bold = true;
                        ws.Cells[10, 1, 10, 7].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        ws.Cells[10, 1, 10, 7].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(236, 143, 50));

                        ws.Cells.AutoFitColumns();
                        ws.Row(10).Style.WrapText = true;

                        ws.Cells["D11:G" + (noRowTKBD + 11)].Style.Numberformat.Format = "#,##0.00";

                        //sum group 1
                        if (noRowTKBD > 0)
                        {
                            ws.Cells["A" + (noRowTKBD + 11) + ":B" + (noRowTKBD + 11)].Merge = true;
                            ws.Cells[noRowTKBD + 11, 1].Value = "Tổng cộng:   ";
                            ws.Cells[noRowTKBD + 11, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            ws.Row(noRowTKBD + 11).Style.Font.Bold = true;
                            ws.Cells[noRowTKBD + 11, 3].Formula = "sum(c11:c" + (noRowTKBD + 10) + ")";
                            ws.Cells[noRowTKBD + 11, 4].Formula = "sum(D11:D" + (noRowTKBD + 10) + ")";
                            ws.Cells[noRowTKBD + 11, 5].Formula = "sum(E11:E" + (noRowTKBD + 10) + ")";
                        }
                    }

                    #endregion TKBD General

                    #region templateInfo

                    // all
                    ws.Cells["A1:Z1000"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    //header
                    ws.Cells["A1:I1"].Merge = true;
                    ws.Cells["A1:I1"].Value = "TỔNG CÔNG TY BƯU ĐIỆN VIỆT NAM \n BƯU ĐIỆN TỈNH SÓC TRĂNG";
                    ws.Row(1).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    ws.Row(1).Height = 45;
                    ws.Row(1).Style.Font.Bold = true;
                    ws.Row(1).Style.Font.Size = 15;
                    //functionName
                    ws.Cells["A1:I1"].Style.WrapText = true;
                    ws.Cells["A3:I3"].Merge = true;
                    ws.Cells["A3:I3"].Formula = "upper(\"" + vm.FunctionName.ToString() + "\")";
                    ws.Row(3).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    ws.Row(3).Style.Font.Size = 13;
                    ws.Row(3).Style.Font.Bold = true;

                    // fill district
                    ws.Cells["C4:I4"].Merge = true;
                    ws.Cells["C4:I4"].Style.Font.Bold = true;
                    ws.Cells["C4:I4"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    ws.Cells["C4:I4"].Style.Indent = 2;
                    if (vm.District == null)
                    {
                        vm.District = "Tất cả";
                    }
                    ws.Cells["C4:I4"].Value = vm.District;

                    // fill unit
                    ws.Cells["C5:I5"].Merge = true;
                    ws.Cells["C5:I5"].Style.Font.Bold = true;
                    ws.Cells["C5:I5"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    ws.Cells["C5:I5"].Style.Indent = 2;
                    if (vm.Unit == null)
                    {
                        vm.Unit = "Tất cả";
                    }
                    ws.Cells["C5:I5"].Value = vm.Unit;

                    // fill user
                    ws.Cells["C6:I6"].Merge = true;
                    ws.Cells["C6:I6"].Style.Font.Bold = true;
                    ws.Cells["C6:I6"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    ws.Cells["C6:I6"].Style.Indent = 2;
                    if (vm.user == null)
                    {
                        vm.user = "Tất cả";
                    }
                    ws.Cells["C6:I6"].Value = vm.user;

                    // fill time
                    ws.Cells["C7:I7"].Merge = true;
                    ws.Cells["C7:I7"].Style.Font.Bold = true;
                    ws.Cells["C7:I7"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    ws.Cells["C7:I7"].Style.Indent = 2;
                    ws.Cells["C7:I7"].Value = "Tháng  " + vm.Month + "/ " + vm.Year;

                    // service
                    ws.Cells["C8:I8"].Merge = true;
                    ws.Cells["C8:I8"].Style.Font.Bold = true;
                    ws.Cells["C8:I8"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    ws.Cells["C8:I8"].Style.Indent = 2;
                    ws.Cells["C8:I8"].Value = "Tiết kiệm bưu điện";

                    //info
                    ws.Cells["A4:B4"].Merge = true;
                    ws.Cells["A4:B4"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                    ws.Row(4).Style.Font.Bold = true;
                    ws.Cells["A4:B4"].Value = "Huyện: ";
                    ws.Cells["A4:B4"].Style.Indent = 1;

                    ws.Cells["A5:B5"].Merge = true;
                    ws.Cells["A5:B5"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                    ws.Row(5).Style.Font.Bold = true;
                    ws.Cells["A5:B5"].Value = "Bưu cục: ";
                    ws.Cells["A5:B5"].Style.Indent = 1;

                    ws.Cells["A6:B6"].Merge = true;
                    ws.Cells["A6:B6"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                    ws.Row(6).Style.Font.Bold = true;
                    ws.Cells["A6:B6"].Value = "Nhân viên:";
                    ws.Cells["A6:B6"].Style.Indent = 1;

                    ws.Cells["A7:B7"].Merge = true;
                    ws.Cells["A7:B7"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                    ws.Row(7).Style.Font.Bold = true;
                    ws.Cells["A7:B7"].Value = "Thời gian:";
                    ws.Cells["A7:B7"].Style.Indent = 1;

                    ws.Cells["A8:B8"].Merge = true;
                    ws.Cells["A8:B8"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                    ws.Row(8).Style.Font.Bold = true;
                    ws.Cells["A8:B8"].Value = "Dịch vụ:";
                    ws.Cells["A8:B8"].Style.Indent = 1;

                    #endregion templateInfo

                    if (noRowTKBD == 0)
                    {
                        ws.Cells["A10"].Value = "Không có dữ liệu";
                    }

                    pck.Save();
                }
            });
        }

        /*
           code:
           name: Export Bảng kê TKBD - Chi tiết
       */

        public static Task TKBD_Export_Detail<T1>(List<T1> dataSource, string filePath, Export_Info_Template vm)
        {
            return Task.Run(() =>
            {
                using (ExcelPackage pck = new ExcelPackage(new FileInfo(filePath)))
                {
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Thống kê chi tiết");

                    #region count data

                    int noRowTKBD = dataSource.Count; //count number rows TKBD

                    #endregion count data

                    #region TKBD General

                    if (noRowTKBD > 0)
                    {
                        //load data source 1 TKBD start A9
                        ws.Cells["A10"].LoadFromCollection<T1>(dataSource, true, TableStyles.Light1);
                        //fill STT
                        for (int i = 1; i <= noRowTKBD; i++)
                        {
                            ws.Cells["A" + (i + 10)].Value = i;
                        }

                        //format col 1
                        ws.Column(1).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                        //header
                        ws.Row(10).Height = 30;
                        ws.Cells["A10"].Value = "STT";
                        ws.Cells["B10"].Value = "Tháng";
                        ws.Cells["C10"].Value = "Tài khoản";
                        ws.Cells["D10"].Value = "Số dư cuối kỳ";
                        ws.Cells["E10"].Value = "Doanh thu \n tính lương";
                        ws.Cells["F10"].Value = "Mã \n nhân viên";
                        ws.Cells["G10"].Value = "Tên \n nhân viên";

                        ws.Cells["A10:G10"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        ws.Cells["A10:G10"].Style.Font.Bold = true;
                        ws.Cells[10, 1, 10, 7].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        ws.Cells[10, 1, 10, 7].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(236, 143, 50));

                        ws.Cells.AutoFitColumns();
                        ws.Row(10).Style.WrapText = true;

                        ws.Cells["D11:F" + (noRowTKBD + 11)].Style.Numberformat.Format = "#,##0.00";

                        //sum group 1
                        ws.Cells[noRowTKBD + 11, 2].Value = "Tổng cộng";
                        ws.Cells[noRowTKBD + 11, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        ws.Row(noRowTKBD + 11).Style.Font.Bold = true;
                        ws.Cells[noRowTKBD + 11, 4].Formula = "sum(D11:D" + (noRowTKBD + 10) + ")";
                        ws.Cells[noRowTKBD + 11, 5].Formula = "sum(E11:E" + (noRowTKBD + 10) + ")";
                    }

                    #endregion TKBD General

                    #region templateInfo

                    // all
                    ws.Cells["A1:Z1000"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    //header
                    ws.Cells["A1:I1"].Merge = true;
                    ws.Cells["A1:I1"].Value = "TỔNG CÔNG TY BƯU ĐIỆN VIỆT NAM \n BƯU ĐIỆN TỈNH SÓC TRĂNG";
                    ws.Row(1).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    ws.Row(1).Height = 45;
                    ws.Row(1).Style.Font.Bold = true;
                    ws.Row(1).Style.Font.Size = 15;
                    //functionName
                    ws.Cells["A1:I1"].Style.WrapText = true;
                    ws.Cells["A3:I3"].Merge = true;
                    ws.Cells["A3:I3"].Formula = "upper(\"" + vm.FunctionName.ToString() + "\")";
                    ws.Row(3).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    ws.Row(3).Style.Font.Size = 13;
                    ws.Row(3).Style.Font.Bold = true;

                    // fill district
                    ws.Cells["C4:I4"].Merge = true;
                    ws.Cells["C4:I4"].Style.Font.Bold = true;
                    ws.Cells["C4:I4"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    ws.Cells["C4:I4"].Style.Indent = 2;
                    if (vm.District == null)
                    {
                        vm.District = "Tất cả";
                    }
                    ws.Cells["C4:I4"].Value = vm.District;

                    // fill unit
                    ws.Cells["C5:I5"].Merge = true;
                    ws.Cells["C5:I5"].Style.Font.Bold = true;
                    ws.Cells["C5:I5"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    ws.Cells["C5:I5"].Style.Indent = 2;
                    if (vm.Unit == null)
                    {
                        vm.Unit = "Tất cả";
                    }
                    ws.Cells["C5:I5"].Value = vm.Unit;

                    // fill user
                    ws.Cells["C6:I6"].Merge = true;
                    ws.Cells["C6:I6"].Style.Font.Bold = true;
                    ws.Cells["C6:I6"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    ws.Cells["C6:I6"].Style.Indent = 2;
                    if (vm.user == null)
                    {
                        vm.user = "Tất cả";
                    }
                    ws.Cells["C6:I6"].Value = vm.user;

                    // fill time
                    ws.Cells["C7:I7"].Merge = true;
                    ws.Cells["C7:I7"].Style.Font.Bold = true;
                    ws.Cells["C7:I7"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    ws.Cells["C7:I7"].Style.Indent = 2;
                    ws.Cells["C7:I7"].Value = "Tháng " + vm.Month + " / " + vm.Year;

                    // service
                    ws.Cells["C8:I8"].Merge = true;
                    ws.Cells["C8:I8"].Style.Font.Bold = true;
                    ws.Cells["C8:I8"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    ws.Cells["C8:I8"].Style.Indent = 2;
                    ws.Cells["C8:I8"].Value = "Tiết kiệm bưu điện";

                    //info
                    ws.Cells["A4:B4"].Merge = true;
                    ws.Cells["A4:B4"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                    ws.Row(4).Style.Font.Bold = true;
                    ws.Cells["A4:B4"].Value = "Huyện: ";
                    ws.Cells["A4:B4"].Style.Indent = 1;

                    ws.Cells["A5:B5"].Merge = true;
                    ws.Cells["A5:B5"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                    ws.Row(5).Style.Font.Bold = true;
                    ws.Cells["A5:B5"].Value = "Bưu cục: ";
                    ws.Cells["A5:B5"].Style.Indent = 1;

                    ws.Cells["A6:B6"].Merge = true;
                    ws.Cells["A6:B6"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                    ws.Row(6).Style.Font.Bold = true;
                    ws.Cells["A6:B6"].Value = "Nhân viên:";
                    ws.Cells["A6:B6"].Style.Indent = 1;

                    ws.Cells["A7:B7"].Merge = true;
                    ws.Cells["A7:B7"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                    ws.Row(7).Style.Font.Bold = true;
                    ws.Cells["A7:B7"].Value = "Thời gian:";
                    ws.Cells["A7:B7"].Style.Indent = 1;

                    ws.Cells["A8:B8"].Merge = true;
                    ws.Cells["A8:B8"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                    ws.Row(8).Style.Font.Bold = true;
                    ws.Cells["A8:B8"].Value = "Dịch vụ:";
                    ws.Cells["A8:B8"].Style.Indent = 1;

                    #endregion templateInfo

                    if (noRowTKBD == 0)
                    {
                        ws.Cells["A10"].Value = "Không có dữ liệu";
                    }

                    pck.Save();
                }
            });
        }
    }
}
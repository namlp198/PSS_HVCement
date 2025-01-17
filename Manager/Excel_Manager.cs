using OpenXMLParser;
using PSS_HVCement.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PSS_HVCement.Manager
{
    public class Excel_Manager
    {
        private bool _disposed;

        private static Excel_Manager m_instance;
        public static Excel_Manager Instance
        {
            get
            {
                if (m_instance == null)
                    m_instance = new Excel_Manager();
                return m_instance;
            }
            private set { }
        }
        public Excel_Manager()
        {

        }
        //~Excel_Manager()
        //{
        //    //Dispose();
        //    KillAppExcel();
        //}

        private List<string> m_listColumnNameProductionData = null;
        private List<string> m_listColumnNameSystemData = null;
        private ExcelParser m_excelParser = new ExcelParser();

        private string m_strExcelFilePath = string.Empty;
        private string m_strReportFolderPath = "D:\\DuLieuSanXuat\\BaoCao";
        public string ExcelFilePath
        {
            get => m_strExcelFilePath;
            set => m_strExcelFilePath = value;
        }

        #region Func
        public void Initialize()
        {
            if (m_listColumnNameProductionData == null)
                m_listColumnNameProductionData = new List<string>() { "A", "B", "C", "D", "E", "F", "G", "H" };

            if (m_listColumnNameSystemData == null)
                m_listColumnNameSystemData = new List<string>() { "A", "B", "C", "D" };
        }
        public void StartAppExcel()
        {
            KillAppExcel();
            OpenExcelResultFile();
        }
        public void CancelExcelParser()
        {
            if(m_excelParser != null)
                m_excelParser.Close();
        }
        private void KillAppExcel()
        {
            foreach (var process in Process.GetProcessesByName("EXCEL"))
            {
                process.Kill();
            }
        }
        void OpenExcelResultFile()
        {
            try
            {
                if(m_excelParser == null)
                    m_excelParser = new ExcelParser();

                string currentDir = m_strReportFolderPath + "\\Template\\" + "Report_Template.xlsx";
                string currentDailyData = m_strReportFolderPath + "\\BaoCao_" + DateTime.Now.ToString("dd-MM-yyyy") + ".xlsx";
                if (!File.Exists(currentDailyData))
                {
                    File.Copy(@currentDir, @currentDailyData);
                }
                if (m_excelParser.OpenFile(currentDailyData, true))
                {
                    ExcelFilePath = currentDailyData;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private Task ExcelProductionDataInput(List<ExcelProductionDataModel> excelProductionDataModels, string sheetName, int cellStartWrite)
        {
            int cell = cellStartWrite;
            int index = 1;
            string addressName = string.Empty;
            string value = string.Empty;
            return Task.Run(() =>
            {
                foreach (var excelModel in excelProductionDataModels)
                {

                    if (excelModel == null) return;

                    addressName = m_listColumnNameProductionData[0] + cell;
                    value = index + "";
                    if (!m_excelParser.SetCellValue(sheetName, addressName, value))
                    {
                        m_excelParser.AddCell(sheetName, addressName, value);
                    }

                    addressName = m_listColumnNameProductionData[1] + cell;
                    value = excelModel.PDate;
                    if (!m_excelParser.SetCellValue(sheetName, addressName, value))
                    {
                        m_excelParser.AddCell(sheetName, addressName, value);
                    }

                    addressName = m_listColumnNameProductionData[2] + cell;
                    value = excelModel.PStartTime;
                    if (!m_excelParser.SetCellValue(sheetName, addressName, value))
                    {
                        m_excelParser.AddCell(sheetName, addressName, value);
                    }

                    addressName = m_listColumnNameProductionData[3] + cell;
                    value = excelModel.PEndTime;
                    if (!m_excelParser.SetCellValue(sheetName, addressName, value))
                    {
                        m_excelParser.AddCell(sheetName, addressName, value);
                    }

                    addressName = m_listColumnNameProductionData[4] + cell;
                    value = excelModel.PShift;
                    if (!m_excelParser.SetCellValue(sheetName, addressName, value))
                    {
                        m_excelParser.AddCell(sheetName, addressName, value);
                    }

                    addressName = m_listColumnNameProductionData[5] + cell;
                    value = excelModel.DeliveryCode;
                    if (!m_excelParser.SetCellValue(sheetName, addressName, value))
                    {
                        m_excelParser.AddCell(sheetName, addressName, value);
                    }

                    addressName = m_listColumnNameProductionData[6] + cell;
                    value = excelModel.PrintCode;
                    if (!m_excelParser.SetCellValue(sheetName, addressName, value))
                    {
                        m_excelParser.AddCell(sheetName, addressName, value);
                    }

                    addressName = m_listColumnNameProductionData[7] + cell;
                    value = excelModel.PrintCount + "";
                    if (!m_excelParser.SetCellValue(sheetName, addressName, value))
                    {
                        m_excelParser.AddCell(sheetName, addressName, value);
                    }

                    m_excelParser.Save();

                    cell++;
                    index++;
                }
                //m_excelParser.Close();
            });
        }
        public Task ExportProductionData(List<ExcelProductionDataModel> excelProductionDataModels, string sheetName, int cellStartWrite)
        {
            return Task.Run(async () =>
            {
                await ExcelProductionDataInput(excelProductionDataModels, sheetName, cellStartWrite);
            });
        }
        private Task ExcelSystemDataInput(List<ExcelSystemDataModel> excelSystemDataModels, string sheetName, int cellStartWrite)
        {
            int cell = cellStartWrite;
            int index = 1;
            string addressName = string.Empty;
            string value = string.Empty;
            return Task.Run(() =>
            {
                foreach (var excelModel in excelSystemDataModels)
                {

                    if (excelModel == null) return;

                    addressName = m_listColumnNameSystemData[0] + cell;
                    value = index + "";
                    if (!m_excelParser.SetCellValue(sheetName, addressName, value))
                    {
                        m_excelParser.AddCell(sheetName, addressName, value);
                    }

                    addressName = m_listColumnNameSystemData[1] + cell;
                    value = excelModel.SysDate;
                    if (!m_excelParser.SetCellValue(sheetName, addressName, value))
                    {
                        m_excelParser.AddCell(sheetName, addressName, value);
                    }

                    addressName = m_listColumnNameSystemData[2] + cell;
                    value = excelModel.SysTime;
                    if (!m_excelParser.SetCellValue(sheetName, addressName, value))
                    {
                        m_excelParser.AddCell(sheetName, addressName, value);
                    }

                    addressName = m_listColumnNameSystemData[3] + cell;
                    value = excelModel.PrintReport;
                    if (!m_excelParser.SetCellValue(sheetName, addressName, value))
                    {
                        m_excelParser.AddCell(sheetName, addressName, value);
                    }

                    m_excelParser.Save();

                    cell++;
                    index++;
                }
                //m_excelParser.Close();
            });
        }
        public Task ExportSystemData(List<ExcelSystemDataModel> excelSystemDataModels, string sheetName, int cellStartWrite)
        {
            return Task.Run(async () =>
            {
                await ExcelSystemDataInput(excelSystemDataModels, sheetName, cellStartWrite);
            });
        }
        private void OpenFolder(string folderPath)
        {
            System.Diagnostics.Process.Start("explorer.exe", @folderPath);
        }
        public void OpenReportFolder()
        {
            OpenFolder(m_strReportFolderPath);
        }

        //protected virtual void Dispose(bool disposing)
        //{
        //    if (!_disposed)
        //    {
        //        if (disposing)
        //        {
        //            //dispose managed resources
        //            CancelExcelParser();
        //            KillAppExcel();
        //        }
        //    }
        //    //dispose unmanaged resources
        //    _disposed = true;
        //}
        //public void Dispose()
        //{
        //    Dispose(true);
        //    GC.SuppressFinalize(this);
        //}
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using PSS_HVCement.Models;

namespace PSS_HVCement.Manager
{
    public class Csv_Manager
    {
        private string m_strProductionDataFolderPath = "D:\\DuLieuSanXuat";
        private string m_strDailyProductionDataFolderPath = "";

        private List<string> m_listProductionData = new List<string>();
        private List<string> m_listSysData = new List<string>();

        private static Csv_Manager m_instance;
        public static Csv_Manager Instance
        {
            get
            {
                if (m_instance == null)
                    m_instance = new Csv_Manager();
                return m_instance;
            }
            private set { }
        }

        public Csv_Manager() 
        {
           
        }
        public void Initialize(int nNumberOfPrinter)
        {
            CreateFolder(m_strProductionDataFolderPath);
            //CreateFolder(m_strReportFolderPath);

            m_strDailyProductionDataFolderPath = m_strProductionDataFolderPath + "\\DuLieu_" + DateTime.Now.ToString("dd-MM-yyyy");
            CreateFolder(m_strDailyProductionDataFolderPath);

            CreateReportFile(nNumberOfPrinter);
        }
        private void CreateFolder(string folderPath)
        {
            try
            {
                if(!Directory.Exists(folderPath))
                {
                    DirectoryInfo di = Directory.CreateDirectory(folderPath);
                }
            }
            catch(IOException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CreateReportFile(int nNumberOfPrinter)
        {
            if (nNumberOfPrinter == 0)
                return;

            if(string.IsNullOrEmpty(m_strProductionDataFolderPath))
                return;

            if (string.IsNullOrEmpty(m_strDailyProductionDataFolderPath))
                return;

            for(int i =0; i< nNumberOfPrinter; i++)
            {
                string productionData = m_strDailyProductionDataFolderPath + "\\DuLieuSanXuat_MAYIN" + (i+1) + ".csv";
                string sysData = m_strDailyProductionDataFolderPath + "\\DuLieuHeThong_MAYIN" + (i + 1) + ".csv";

                if (!File.Exists(productionData))
                {
                    using (File.Create(productionData)) 
                    {
                        m_listProductionData.Add(productionData);
                    }
                }
                else
                {
                    m_listProductionData.Add(productionData);
                }

                if (!File.Exists(sysData))
                {
                    using (File.Create(sysData)) 
                    { 
                        m_listSysData.Add(sysData);
                    }
                }
                else
                {
                    m_listSysData.Add(sysData);
                }
            }
        }

        #region Read/Write Production Data
        public void WriteNewProductionDataModelToCsv(List<ExcelProductionDataModel> excelProductionDataModels, int nPrinterOrder)
        {
            if (m_listProductionData.Count <= 0)
                return;

            if (nPrinterOrder > m_listProductionData.Count)
                return;

            int nPrinterIdx = nPrinterOrder - 1;

            try
            {
                var configExcelTempModel = new CsvConfiguration(CultureInfo.InvariantCulture);
                {
                    configExcelTempModel.HasHeaderRecord = false;
                };

                using (StreamWriter streamWriter = new StreamWriter(m_listProductionData[nPrinterIdx], true))
                using (CsvWriter csvWriter = new CsvWriter(streamWriter, configExcelTempModel))
                {
                    csvWriter.WriteRecordsAsync(excelProductionDataModels);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public List<ExcelProductionDataModel> ReadExcelProductionDataModelFromCsv(int nPrinterOrder)
        {
            if (m_listProductionData.Count <= 0)
                return null;

            if (nPrinterOrder > m_listProductionData.Count)
                return null;

            int nPrinterIdx = nPrinterOrder - 1;

            try
            {
                if (File.Exists(m_listProductionData[nPrinterIdx]))
                {
                    List<ExcelProductionDataModel> excelProductionDataModels = new List<ExcelProductionDataModel>();
                    var config = new CsvConfiguration(CultureInfo.InvariantCulture);
                    {
                        config.HasHeaderRecord = false;
                    }
                    using (StreamReader streamReader = new StreamReader(m_listProductionData[nPrinterIdx]))
                    using (CsvReader csvReader = new CsvReader(streamReader, config))
                    {
                        // Read records from CSV file
                        IEnumerable<ExcelProductionDataModel> records = csvReader.GetRecords<ExcelProductionDataModel>();
                        excelProductionDataModels = records.ToList();
                    }
                    return excelProductionDataModels;
                }
                return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }
        #endregion

        #region Read/Write Sys Data
        public void WriteNewSysDataModelToCsv(List<ExcelSystemDataModel> excelSysDataModels, int nPrinterOrder)
        {
            if (m_listSysData.Count <= 0)
                return;

            if (nPrinterOrder > m_listSysData.Count)
                return;

            int nPrinterIdx = nPrinterOrder - 1;

            try
            {
                var configExcelTempModel = new CsvConfiguration(CultureInfo.InvariantCulture);
                {
                    configExcelTempModel.HasHeaderRecord = false;
                };

                using (StreamWriter streamWriter = new StreamWriter(m_listSysData[nPrinterIdx], true))
                using (CsvWriter csvWriter = new CsvWriter(streamWriter, configExcelTempModel))
                {
                    csvWriter.WriteRecordsAsync(excelSysDataModels);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public List<ExcelSystemDataModel> ReadExcelSysDataModelFromCsv(int nPrinterOrder)
        {
            if (m_listSysData.Count <= 0)
                return null;

            if (nPrinterOrder > m_listSysData.Count)
                return null;

            int nPrinterIdx = nPrinterOrder - 1;

            try
            {
                if (File.Exists(m_listSysData[nPrinterIdx]))
                {
                    List<ExcelSystemDataModel> excelSysDataModels = new List<ExcelSystemDataModel>();
                    var config = new CsvConfiguration(CultureInfo.InvariantCulture);
                    {
                        config.HasHeaderRecord = false;
                    }
                    using (StreamReader streamReader = new StreamReader(m_listSysData[nPrinterIdx]))
                    using (CsvReader csvReader = new CsvReader(streamReader, config))
                    {
                        // Read records from CSV file
                        IEnumerable<ExcelSystemDataModel> records = csvReader.GetRecords<ExcelSystemDataModel>();
                        excelSysDataModels = records.ToList();
                    }
                    return excelSysDataModels;
                }
                return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }
        #endregion

        #region Read/Write Template
        public void WriteNewModelToCsv(List<ExcelTemplateModel> excelTemplateModels, string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    using (File.Create(filePath)) { }
                }

                var configExcelTempModel = new CsvConfiguration(CultureInfo.InvariantCulture);
                {
                    configExcelTempModel.HasHeaderRecord = false;
                };

                using (StreamWriter streamWriter = new StreamWriter(filePath, true))
                using (CsvWriter csvWriter = new CsvWriter(streamWriter, configExcelTempModel))
                {
                    csvWriter.WriteRecordsAsync(excelTemplateModels);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public List<ExcelTemplateModel> ReadExcelTemplateModelFromCsv(string filePath)
        {
            try
            {
                if(File.Exists(filePath))
                {
                    List<ExcelTemplateModel> excelTemplateModels = new List<ExcelTemplateModel>();
                    var config = new CsvConfiguration(CultureInfo.InvariantCulture);
                    {
                        config.HasHeaderRecord = false;
                    }
                    using (StreamReader streamReader = new StreamReader(filePath))
                    using (CsvReader csvReader = new CsvReader(streamReader, config))
                    {
                        // Read records from CSV file
                        IEnumerable<ExcelTemplateModel> records = csvReader.GetRecords<ExcelTemplateModel>();
                        excelTemplateModels = records.ToList();
                    }
                    return excelTemplateModels;
                }
                return null;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }
        #endregion
    }
}

﻿using DocumentFormat.OpenXml.Drawing.Charts;
using MVVMBasic;
using PSS_HVCement.Manager;
using PSS_HVCement.Models;
using PSS_HVCement.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;

namespace PSS_HVCement.ViewModels
{
    public class ReportViewModel : ViewModelBase
    {
        private readonly Dispatcher m_dispatcher;
        private ReportView m_reportView;

        private List<ExcelProductionDataModel> m_listExcelProductDataModel = new List<ExcelProductionDataModel>();
        private List<ExcelSystemDataModel> m_listExcelSystemDataModel = new List<ExcelSystemDataModel>();
        private List<string> m_listPrinter = new List<string>();

        private ExcelProductionDataModel m_excelProductionDataSelected = new ExcelProductionDataModel();
        private ExcelSystemDataModel m_excelSystemDataSelected = new ExcelSystemDataModel();
        private string m_printerSelected = string.Empty;

        public ReportViewModel(Dispatcher dispatcher, ReportView reportView)
        {
            m_dispatcher = dispatcher;
            m_reportView = reportView;

            List<string> lstPrinter = new List<string>();
            for (int i = 0; i < MainWindowViewModel.Instance.SettingsVM.NumberOfPrinter; i++)
            {
                lstPrinter.Add(string.Format("MÁY IN {0}", i + 1));
            }
            Printers = lstPrinter;
            m_reportView.cbbPrinters.SelectedIndex = 0;
            m_reportView.tabReport.SelectedIndex = 0;

            m_reportView.btnExcelExport.Click += BtnExcelExport_Click;
            m_reportView.btnExcelExportAll.Click += BtnExcelExportAll_Click;
            m_reportView.datePickerReport.SelectedDateChanged += DatePickerReport_SelectedDateChanged;
            Excel_Manager.Instance.Initialize();
        }

        private void DatePickerReport_SelectedDateChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var datePicker = sender as DatePicker;
            if (datePicker != null)
            {
                var datetime = datePicker.SelectedDate.Value;
                if (datetime != null)
                {

                }
            }
        }

        private async void BtnExcelExportAll_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Excel_Manager.Instance.StartAppExcel();

            for (int i = 0; i < Printers.Count; i++)
            {
                int nPrinterOrder = i + 1;
                List<ExcelProductionDataModel> excelProductionDataModels = Csv_Manager.Instance.ReadExcelProductionDataModelFromCsv(nPrinterOrder);
                List<ExcelSystemDataModel> excelSystemDataModels = Csv_Manager.Instance.ReadExcelSysDataModelFromCsv(nPrinterOrder);

                switch(nPrinterOrder)
                {
                    case 1:
                        await Excel_Manager.Instance.ExportProductionData(excelProductionDataModels, "BaoCaoSanXuat_MAYIN1", 3);
                        await Excel_Manager.Instance.ExportSystemData(excelSystemDataModels, "BaoCaoHeThong_MAYIN1", 3);
                        break;
                    case 2:
                        await Excel_Manager.Instance.ExportProductionData(excelProductionDataModels, "BaoCaoSanXuat_MAYIN2", 3);
                        await Excel_Manager.Instance.ExportSystemData(excelSystemDataModels, "BaoCaoHeThong_MAYIN2", 3);
                        break;
                    case 3:
                        await Excel_Manager.Instance.ExportProductionData(excelProductionDataModels, "BaoCaoSanXuat_MAYIN3", 3);
                        await Excel_Manager.Instance.ExportSystemData(excelSystemDataModels, "BaoCaoHeThong_MAYIN3", 3);
                        break;
                }
            }
            Excel_Manager.Instance.CancelExcelParser();
            Excel_Manager.Instance.OpenReportFolder();
        }

        private async void BtnExcelExport_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (m_reportView.cbbPrinters.SelectedIndex < 0)
                return;

            int nPrinterOrder = m_reportView.cbbPrinters.SelectedIndex + 1;

            Excel_Manager.Instance.StartAppExcel();

            switch(nPrinterOrder)
            {
                // MAY IN 1
                case 1:
                    await Excel_Manager.Instance.ExportProductionData(ExcelProductionDataModels, "BaoCaoSanXuat_MAYIN1", 3);
                    await Excel_Manager.Instance.ExportSystemData(ExcelSystemDataModels, "BaoCaoHeThong_MAYIN1", 3);
                    break;
                // MAY IN 2
                case 2:
                    await Excel_Manager.Instance.ExportProductionData(ExcelProductionDataModels, "BaoCaoSanXuat_MAYIN2", 3);
                    await Excel_Manager.Instance.ExportSystemData(ExcelSystemDataModels, "BaoCaoHeThong_MAYIN2", 3);
                    break;
                // MAY IN 3
                case 3:
                    await Excel_Manager.Instance.ExportProductionData(ExcelProductionDataModels, "BaoCaoSanXuat_MAYIN3", 3);
                    await Excel_Manager.Instance.ExportSystemData(ExcelSystemDataModels, "BaoCaoHeThong_MAYIN3", 3);
                    break;
            }

            Excel_Manager.Instance.CancelExcelParser();
            Excel_Manager.Instance.OpenReportFolder();
        }

        public List<ExcelProductionDataModel> ExcelProductionDataModels
        {
            get => m_listExcelProductDataModel;
            set
            {
                if (SetProperty(ref m_listExcelProductDataModel, value))
                {

                }
            }
        }

        public List<ExcelSystemDataModel> ExcelSystemDataModels
        {
            get => m_listExcelSystemDataModel;
            set
            {
                if (SetProperty(ref m_listExcelSystemDataModel, value))
                {

                }
            }
        }
        public List<string> Printers
        {
            get => m_listPrinter;
            set
            {
                if (SetProperty(ref m_listPrinter, value))
                {

                }
            }
        }
        public ExcelProductionDataModel ExcelProductDataSelected
        {
            get => m_excelProductionDataSelected;
            set
            {
                if (SetProperty(ref m_excelProductionDataSelected, value))
                {

                }
            }
        }
        public ExcelSystemDataModel ExcelSystemDataSelected
        {
            get => m_excelSystemDataSelected;
            set
            {
                if (SetProperty(ref m_excelSystemDataSelected, value))
                {

                }
            }
        }
        public string PrinterSelected
        {
            get => m_printerSelected;
            set
            {
                if (SetProperty(ref m_printerSelected, value))
                {
                    if (m_printerSelected.CompareTo("MÁY IN 1") == 0)
                    {
                        List<ExcelProductionDataModel> excelProductionDataModels = Csv_Manager.Instance.ReadExcelProductionDataModelFromCsv(1);
                        List<ExcelSystemDataModel> excelSystemDataModels = Csv_Manager.Instance.ReadExcelSysDataModelFromCsv(1);
                        ExcelProductionDataModels = excelProductionDataModels;
                        ExcelSystemDataModels = excelSystemDataModels;
                    }
                    else if (m_printerSelected.CompareTo("MÁY IN 2") == 0)
                    {
                        List<ExcelProductionDataModel> excelProductionDataModels = Csv_Manager.Instance.ReadExcelProductionDataModelFromCsv(2);
                        List<ExcelSystemDataModel> excelSystemDataModels = Csv_Manager.Instance.ReadExcelSysDataModelFromCsv(2);
                        ExcelProductionDataModels = excelProductionDataModels;
                        ExcelSystemDataModels = excelSystemDataModels;
                    }
                    else if (m_printerSelected.CompareTo("MÁY IN 3") == 0)
                    {
                        List<ExcelProductionDataModel> excelProductionDataModels = Csv_Manager.Instance.ReadExcelProductionDataModelFromCsv(3);
                        List<ExcelSystemDataModel> excelSystemDataModels = Csv_Manager.Instance.ReadExcelSysDataModelFromCsv(3);
                        ExcelProductionDataModels = excelProductionDataModels;
                        ExcelSystemDataModels = excelSystemDataModels;
                    }
                }
            }
        }
    }
}

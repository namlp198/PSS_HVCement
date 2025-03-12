using JetPrinter.ui;
using MVVMBasic;
using PSS_HVCement.Manager;
using PSS_HVCement.Models;
using PSS_HVCement.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace PSS_HVCement.ViewModels
{
    public class PrintersViewModel : ViewModelBase
    {
        private readonly Dispatcher m_dispatcher;
        private PrintersView m_printerView;
        public PrintersView PrinterView { get { return m_printerView; } set { m_printerView = value; } }

        private KGKJetPrinterView m_kgkPrinter1 = new KGKJetPrinterView();
        private KGKJetPrinterView m_kgkPrinter2 = new KGKJetPrinterView();
        private KGKJetPrinterView m_kgkPrinter3 = new KGKJetPrinterView();
        public KGKJetPrinterView KGKJetPrinter1 { get => m_kgkPrinter1; set => m_kgkPrinter1 = value; }
        public KGKJetPrinterView KGKJetPrinter2 { get => m_kgkPrinter2; set => m_kgkPrinter2 = value; }
        public KGKJetPrinterView KGKJetPrinter3 { get => m_kgkPrinter3; set => m_kgkPrinter3 = value; }

        private readonly string m_sDate = string.Empty;
        private readonly string m_sPrintCode = string.Empty;

        public PrintersViewModel(Dispatcher dispatcher, PrintersView printerView)
        {
            m_dispatcher = dispatcher;
            m_printerView = printerView;

            m_sDate = DateTime.Now.ToString("dd/MM/yyyy");
            m_sPrintCode = "NSX:" + m_sDate;
        }
        public void Initialize()
        {
            m_kgkPrinter1.PrintCompletedEvent += m_kgkPrinter1_PrintCompletedEvent;
            m_kgkPrinter2.PrintCompletedEvent += m_kgkPrinter2_PrintCompletedEvent;
            m_kgkPrinter3.PrintCompletedEvent += m_kgkPrinter3_PrintCompletedEvent;

            //m_kgkPrinter1.PrintCountIncreaseEvent += kgkPrinter1_PrintCountIncreaseEvent;
            //m_kgkPrinter2.PrintCountIncreaseEvent += kgkPrinter2_PrintCountIncreaseEvent;
            //m_kgkPrinter3.PrintCountIncreaseEvent += kgkPrinter3_PrintCountIncreaseEvent;

            m_kgkPrinter1.ReportFromPrinterEvent += M_kgkPrinter1_ReportFromPrinterEvent;
            m_kgkPrinter2.ReportFromPrinterEvent += M_kgkPrinter2_ReportFromPrinterEvent;
            m_kgkPrinter3.ReportFromPrinterEvent += M_kgkPrinter3_ReportFromPrinterEvent;

            m_kgkPrinter1.PrinterStatusChangeEvent += M_kgkPrinter1_PrinterStatusChangeEvent;
            m_kgkPrinter2.PrinterStatusChangeEvent += M_kgkPrinter2_PrinterStatusChangeEvent;
            m_kgkPrinter3.PrinterStatusChangeEvent += M_kgkPrinter3_PrinterStatusChangeEvent;

            m_kgkPrinter1.IsVisibleStackShiftProduction = true;
            m_kgkPrinter2.IsVisibleStackShiftProduction = true;
            m_kgkPrinter3.IsVisibleStackShiftProduction = true;

            m_kgkPrinter1.UseAutoMode = true;
            m_kgkPrinter2.UseAutoMode = true;
            m_kgkPrinter3.UseAutoMode = true;
        }

        private void M_kgkPrinter1_PrinterStatusChangeEvent(enPrinterStatus printerStatus)
        {
            MainWindowViewModel.Instance.MainView.Dispatcher.Invoke(new Action(() =>
            {
                MainWindowViewModel.Instance.SendPrinterStatus("01", (int)printerStatus);
            }));
        }
        private void M_kgkPrinter2_PrinterStatusChangeEvent(enPrinterStatus printerStatus)
        {
            MainWindowViewModel.Instance.MainView.Dispatcher.Invoke(new Action(() =>
            {
                MainWindowViewModel.Instance.SendPrinterStatus("02", (int)printerStatus);
            }));
        }
        private void M_kgkPrinter3_PrinterStatusChangeEvent(enPrinterStatus printerStatus)
        {
            MainWindowViewModel.Instance.MainView.Dispatcher.Invoke(new Action(() =>
            {
                MainWindowViewModel.Instance.SendPrinterStatus("03", (int)printerStatus);
            }));
        }

        private void kgkPrinter1_PrintCountIncreaseEvent(uint nPrintCount)
        {
            // record to database
            List<ExcelProductionDataModel> excelProductionDataModels = new List<ExcelProductionDataModel>();
            ExcelProductionDataModel excelModel = new ExcelProductionDataModel();
            excelModel.PDate = m_sDate;
            excelModel.PStartTime = "";
            excelModel.PEndTime = DateTime.Now.ToString("HH:mm:ss");
            excelModel.PShift = "";
            excelModel.DeliveryCode = "";
            excelModel.PrintCode = m_sPrintCode;
            excelModel.PrintCount = (int)nPrintCount;

            excelProductionDataModels.Add(excelModel);
            Csv_Manager.Instance.WriteNewProductionDataModelToCsv(excelProductionDataModels, 1);
        }
        private void kgkPrinter2_PrintCountIncreaseEvent(uint nPrintCount)
        {
            // record to database
            List<ExcelProductionDataModel> excelProductionDataModels = new List<ExcelProductionDataModel>();
            ExcelProductionDataModel excelModel = new ExcelProductionDataModel();
            excelModel.PDate = m_sDate;
            excelModel.PStartTime = "";
            excelModel.PEndTime = DateTime.Now.ToString("HH:mm:ss");
            excelModel.PShift = "";
            excelModel.DeliveryCode = "";
            excelModel.PrintCode = m_sPrintCode;
            excelModel.PrintCount = (int)nPrintCount;

            excelProductionDataModels.Add(excelModel);
            Csv_Manager.Instance.WriteNewProductionDataModelToCsv(excelProductionDataModels, 2);
        }
        private void kgkPrinter3_PrintCountIncreaseEvent(uint nPrintCount)
        {
            // record to database
            List<ExcelProductionDataModel> excelProductionDataModels = new List<ExcelProductionDataModel>();
            ExcelProductionDataModel excelModel = new ExcelProductionDataModel();
            excelModel.PDate = m_sDate;
            excelModel.PStartTime = "";
            excelModel.PEndTime = DateTime.Now.ToString("HH:mm:ss");
            excelModel.PShift = "";
            excelModel.DeliveryCode = "";
            excelModel.PrintCode = m_sPrintCode;
            excelModel.PrintCount = (int)nPrintCount;

            excelProductionDataModels.Add(excelModel);
            Csv_Manager.Instance.WriteNewProductionDataModelToCsv(excelProductionDataModels, 3);
        }

        private void M_kgkPrinter3_ReportFromPrinterEvent(string data)
        {
            // record to database
            List<ExcelSystemDataModel> excelSystemDataModels = new List<ExcelSystemDataModel>();
            ExcelSystemDataModel excelModel = new ExcelSystemDataModel();

            excelModel.SysDate = DateTime.Now.ToString("dd-MM-yyyy");
            excelModel.SysTime = DateTime.Now.ToString("HH:mm:ss");
            excelModel.PrintReport = data;

            excelSystemDataModels.Add(excelModel);
            Csv_Manager.Instance.WriteNewSysDataModelToCsv(excelSystemDataModels, 3);
        }

        private void M_kgkPrinter2_ReportFromPrinterEvent(string data)
        {
            // record to database
            List<ExcelSystemDataModel> excelSystemDataModels = new List<ExcelSystemDataModel>();
            ExcelSystemDataModel excelModel = new ExcelSystemDataModel();

            excelModel.SysDate = DateTime.Now.ToString("dd-MM-yyyy");
            excelModel.SysTime = DateTime.Now.ToString("HH:mm:ss");
            excelModel.PrintReport = data;

            excelSystemDataModels.Add(excelModel);
            Csv_Manager.Instance.WriteNewSysDataModelToCsv(excelSystemDataModels, 2);
        }

        private void M_kgkPrinter1_ReportFromPrinterEvent(string data)
        {
            // record to database
            List<ExcelSystemDataModel> excelSystemDataModels = new List<ExcelSystemDataModel>();
            ExcelSystemDataModel excelModel = new ExcelSystemDataModel();

            excelModel.SysDate = DateTime.Now.ToString("dd-MM-yyyy");
            excelModel.SysTime = DateTime.Now.ToString("HH:mm:ss");
            excelModel.PrintReport = data;

            excelSystemDataModels.Add(excelModel);
            Csv_Manager.Instance.WriteNewSysDataModelToCsv(excelSystemDataModels, 1);
        }

        private void m_kgkPrinter3_PrintCompletedEvent(List<string> data)
        {
            // record to database
            List<ExcelProductionDataModel> excelProductionDataModels = new List<ExcelProductionDataModel>();
            ExcelProductionDataModel excelModel = new ExcelProductionDataModel();
            excelModel.PDate = data[0];
            excelModel.PStartTime = data[1];
            excelModel.PEndTime = data[2];
            excelModel.PShift = data[3];
            excelModel.DeliveryCode = data[4];
            excelModel.PrintCode = data[5];
            excelModel.PrintCount = int.Parse(data[6]);

            excelProductionDataModels.Add(excelModel);
            Csv_Manager.Instance.WriteNewProductionDataModelToCsv(excelProductionDataModels, 3);
        }

        private void m_kgkPrinter2_PrintCompletedEvent(List<string> data)
        {
            // record to database
            List<ExcelProductionDataModel> excelProductionDataModels = new List<ExcelProductionDataModel>();
            ExcelProductionDataModel excelModel = new ExcelProductionDataModel();
            excelModel.PDate = data[0];
            excelModel.PStartTime = data[1];
            excelModel.PEndTime = data[2];
            excelModel.PShift = data[3];
            excelModel.DeliveryCode = data[4];
            excelModel.PrintCode = data[5];
            excelModel.PrintCount = int.Parse(data[6]);

            excelProductionDataModels.Add(excelModel);
            Csv_Manager.Instance.WriteNewProductionDataModelToCsv(excelProductionDataModels, 2);
        }

        private void m_kgkPrinter1_PrintCompletedEvent(List<string> data)
        {
            // record to database
            List<ExcelProductionDataModel> excelProductionDataModels = new List<ExcelProductionDataModel>();
            ExcelProductionDataModel excelModel = new ExcelProductionDataModel();
            excelModel.PDate = data[0];
            excelModel.PStartTime = data[1];
            excelModel.PEndTime = data[2];
            excelModel.PShift = data[3];
            excelModel.DeliveryCode = data[4];
            excelModel.PrintCode = data[5];
            excelModel.PrintCount = int.Parse(data[6]);

            excelProductionDataModels.Add(excelModel);
            Csv_Manager.Instance.WriteNewProductionDataModelToCsv(excelProductionDataModels, 1);
        }
    }
}

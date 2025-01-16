using DocumentFormat.OpenXml.Drawing.Charts;
using MVVMBasic;
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
    public class ReportViewModel : ViewModelBase
    {
        private readonly Dispatcher m_dispatcher;
        private ReportView m_reportView;

        private List<ExcelProductionDataModel> m_listExcelProductDataModel = new List<ExcelProductionDataModel>();
        private List<ExcelSystemDataModel> m_listExcelSystemDataModel = new List<ExcelSystemDataModel>();
        private List<string> m_listPrinter = new List<string>();

        public ReportViewModel(Dispatcher dispatcher, ReportView reportView)
        {
            m_dispatcher = dispatcher;
            m_reportView = reportView;

            List<string> lstPrinter = new List<string>();
            for(int i = 0; i < MainWindowViewModel.Instance.SettingsVM.NumberOfPrinter; i++)
            {
                lstPrinter.Add(string.Format("MÁY IN {0}", i + 1));
            }
            Printers = lstPrinter;
        }

        public List<ExcelProductionDataModel> ExcelProductionDataModels
        {
            get => m_listExcelProductDataModel;
            set
            {
                if(SetProperty(ref m_listExcelProductDataModel, value))
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
                if(SetProperty(ref m_listPrinter, value))
                {

                }
            }
        }
    }
}

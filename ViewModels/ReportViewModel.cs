using MVVMBasic;
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

        public ReportViewModel(Dispatcher dispatcher, ReportView reportView)
        {
            m_dispatcher = dispatcher;
            m_reportView = reportView;
        }
    }
}

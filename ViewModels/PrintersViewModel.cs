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
    public class PrintersViewModel : ViewModelBase
    {
        private readonly Dispatcher m_dispatcher;
        private PrintersView m_printerView;
        public PrintersView PrinterView { get { return m_printerView; } set { m_printerView = value; } }

        public PrintersViewModel(Dispatcher dispatcher, PrintersView printerView)
        {
            m_dispatcher = dispatcher;
            m_printerView = printerView;
        }
    }
}

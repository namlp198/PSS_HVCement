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
    public class DataCustomerViewModel : ViewModelBase
    {
        private readonly Dispatcher m_dispatcher;
        private DataCustomerView m_dataCusView;
        public DataCustomerView DataCustomerView { get => m_dataCusView; set { m_dataCusView = value; } }
        public DataCustomerViewModel(Dispatcher dispatcher, DataCustomerView dataCustomerView) 
        { 
            m_dispatcher = dispatcher;
            m_dataCusView = dataCustomerView;
        }
    }
}

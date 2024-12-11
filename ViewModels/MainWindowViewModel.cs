using MVVMBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace PSS_HVCement.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly Dispatcher m_dispatcher;
        private MainWindow m_mainView;
        public MainWindow MainView { get => m_mainView; set => m_mainView = value; }

        #region Singleton
        private static MainWindowViewModel m_instance;
        public static MainWindowViewModel Instance
        {
            get => m_instance;
            private set { }
        }
        #endregion
        public MainWindowViewModel(Dispatcher dispatcher, MainWindow mainView, PrintersViewModel printersVM,
                                   DataCustomerViewModel dataCusVM) 
        {
            if (m_instance == null) m_instance = this;
            else return;

            m_dispatcher = dispatcher;
            m_mainView = mainView;

            PrintersVM = printersVM;
            DataCustomerVM = dataCusVM;
        }

        #region ViewModels
        public PrintersViewModel PrintersVM { get; set; }
        public DataCustomerViewModel DataCustomerVM { get; set; }
        #endregion
    }
}

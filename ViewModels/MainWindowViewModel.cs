using MVVMBasic;
using PSS_12Printer.ViewModels;
using PSS_HVCement.Commands.Cmd;
using PSS_HVCement.Common;
using PSS_HVCement.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
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
        public MainWindowViewModel(Dispatcher dispatcher, MainWindow mainView, SettingsViewModel settingsVM, PrintersViewModel printersVM,
                                   DataCustomerViewModel dataCusVM) 
        {
            if (m_instance == null) m_instance = this;
            else return;

            m_dispatcher = dispatcher;
            m_mainView = mainView;

            PrintersVM = printersVM;
            DataCustomerVM = dataCusVM;
            SettingsVM = settingsVM;

            this.AboutCmd = new AboutCmd();
            this.OpenSettingViewCmd = new OpenSettingViewCmd();
            this.OpenReportViewCmd = new OpenReportViewCmd();

            // Create report
            Csv_Manager.Instance.Initialize(SettingsVM.NumberOfPrinter);
        }

        #region ViewModels
        public PrintersViewModel PrintersVM { get; set; }
        public DataCustomerViewModel DataCustomerVM { get; set; }
        public SettingsViewModel SettingsVM { get; set; }
        #endregion

        public ICommand AboutCmd { get; }
        public ICommand OpenSettingViewCmd { get; }
        public ICommand OpenReportViewCmd { get; }
    }
}

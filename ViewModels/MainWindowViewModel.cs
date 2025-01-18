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
            this.OpenLoginViewCmd = new OpenLoginViewCmd();

            // Create report
            Csv_Manager.Instance.Initialize(SettingsVM.NumberOfPrinter);

            LoginViewModel.LoginSystemSuccessEvent += LoginSystemEventHandle;
        }

        #region ViewModels
        public PrintersViewModel PrintersVM { get; set; }
        public DataCustomerViewModel DataCustomerVM { get; set; }
        public SettingsViewModel SettingsVM { get; set; }
        #endregion

        private void LoginSystemEventHandle(emLoginStatus eStatus, emRole eRole)
        {
            switch (eStatus)
            {
                case emLoginStatus.LoginStatus_Success:
                    m_role = eRole;

                    switch (eRole)
                    {
                        case emRole.Role_Operator:
                            MainView.tbLogin.Text = "LOGIN";
                            DisplayImage_LoginStatusPath = "/Resources/Images/account.png";

                            IsAllowOperation = false;
                            DOpacity = 0.3;
                            break;

                        case emRole.Role_Engineer:
                        case emRole.Role_Admin:
                        case emRole.Role_SuperAdmin:
                            MainView.tbLogin.Text = "LOGOUT";
                            DisplayImage_LoginStatusPath = "/Resources/Images/logout.png";

                            IsAllowOperation = true;
                            DOpacity = 1.0;
                            break;
                    }
                    break;
                case emLoginStatus.LoginStatus_Failed:
                    break;
            }
        }

        private string m_displayImage_LoginStatusPath = "/Resources/Images/account.png";
        public string DisplayImage_LoginStatusPath
        {
            get => m_displayImage_LoginStatusPath;
            set
            {
                if (SetProperty(ref m_displayImage_LoginStatusPath, value))
                {

                }
            }
        }

        private emRole m_role = emRole.Role_Operator;
        public emRole ROLE
        {
            get => m_role;
            set
            {
                if (SetProperty(ref m_role, value))
                {
                    if (m_role == emRole.Role_Operator)
                    {
                        LoginSystemEventHandle(emLoginStatus.LoginStatus_Success, emRole.Role_Operator);
                    }
                }
            }
        }
        private bool m_bAllowOperation = false;
        public bool IsAllowOperation
        {
            get => m_bAllowOperation;
            set
            {
                if(SetProperty(ref m_bAllowOperation, value))
                {

                }
            }
        }

        private double m_dOpacity = 0.3;
        public double DOpacity
        {
            get => m_dOpacity;
            set
            {
                if(SetProperty(ref m_dOpacity, value))
                {

                }
            }
        }

        public ICommand AboutCmd { get; }
        public ICommand OpenSettingViewCmd { get; }
        public ICommand OpenReportViewCmd { get; }
        public ICommand OpenLoginViewCmd { get; }
    }
}

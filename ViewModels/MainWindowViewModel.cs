using DocumentFormat.OpenXml.Presentation;
using MVVMBasic;
using Ndev.NNetSocket;
using PSS_12Printer.ViewModels;
using PSS_HVCement.Commands.Cmd;
using PSS_HVCement.Common;
using PSS_HVCement.Manager;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace PSS_HVCement.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly Dispatcher m_dispatcher;
        private MainWindow m_mainView;
        public MainWindow MainView { get => m_mainView; set => m_mainView = value; }

        private NClientSocket m_socket;

        private bool m_bConnectedServer = false;

        int[] arrPrintCountYes;

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
            Csv_Manager.Instance.Initialize(SettingsVM.NumberOfPrinter, true);

            LoginViewModel.LoginSystemSuccessEvent += LoginSystemEventHandle;

            m_socket = new NClientSocket(SettingsVM.SysModel.IP, SettingsVM.SysModel.Port);
            m_socket.ConnectionEventCallback += M_socket_ConnectionEventCallback;

            m_socket.ClientConnect();

            arrPrintCountYes = new int[SettingsVM.NumberOfPrinter];
            CreateDailyResult();
            LoadDailyResultYes();
        }

        private void M_socket_ConnectionEventCallback(NClientSocket.EConnectionEventClient e, object obj)
        {
            switch (e)
            {
                case NClientSocket.EConnectionEventClient.RECEIVEDATA:

                    //format: *11@!?PP|Content#
                    if (m_socket.ReceiveString == null)
                        return;

                    // show receive data
                    MainView.Dispatcher.Invoke(new Action(() =>
                    {
                        MainView.labelReceiveData.Content = m_socket.ReceiveString;
                    }));

                    if (m_socket.ReceiveString.Length < 6)
                        return;

                    int length = m_socket.ReceiveString.Length;
                    int idx_1 = m_socket.ReceiveString.IndexOf('?');
                    int idx_2 = m_socket.ReceiveString.IndexOf('|');

                    if (idx_1 < 0 || idx_2 < 0) return;

                    string printer = m_socket.ReceiveString.Substring(idx_1 + 1, 2);
                    if (printer == null) return;

                    string content = m_socket.ReceiveString.Substring(idx_2 + 1, length - 10);
                    if (content == null) return;

                    switch (printer)
                    {
                        case "01":
                            PrintersVM.KGKJetPrinter1.Dispatcher.Invoke(new Action(() =>
                            {
                                PrintersVM.KGKJetPrinter1.MessageContent = content;
                                PrintersVM.KGKJetPrinter1.PerformPushMessage();
                            }));
                            break;
                        case "02":
                            PrintersVM.KGKJetPrinter2.Dispatcher.Invoke(new Action(() =>
                            {
                                PrintersVM.KGKJetPrinter2.MessageContent = content;
                                PrintersVM.KGKJetPrinter2.PerformPushMessage();
                            }));
                            break;
                        case "03":
                            PrintersVM.KGKJetPrinter3.Dispatcher.Invoke(new Action(() =>
                            {
                                PrintersVM.KGKJetPrinter3.MessageContent = content;
                                PrintersVM.KGKJetPrinter3.PerformPushMessage();
                            }));
                            break;
                    }
                    break;
                case NClientSocket.EConnectionEventClient.CLIENTCONNECTED:
                    IsConnectedServer = true;
                    break;
                case NClientSocket.EConnectionEventClient.CLIENTDISCONNECTED:
                    IsConnectedServer = false;
                    break;
                default:
                    break;
            }
        }

        #region ViewModels
        public PrintersViewModel PrintersVM { get; set; }
        public DataCustomerViewModel DataCustomerVM { get; set; }
        public SettingsViewModel SettingsVM { get; set; }
        public bool IsConnectedServer
        {
            get => m_bConnectedServer;
            set
            {
                if (SetProperty(ref m_bConnectedServer, value))
                {
                    if (m_bConnectedServer)
                    {
                        MainView.labelStatusServer.Content = "Đã kết nối Server";
                        MainView.labelStatusServer.Foreground = Brushes.Green;
                    }
                    else
                    {
                        MainView.labelStatusServer.Content = "Chưa kết nối Server";
                        MainView.labelStatusServer.Foreground = Brushes.Red;
                    }
                }
            }
        }
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
        private void CreateDailyResult()
        {
            string date = "Result" + DateTime.Now.ToString("ddMMyy");
            string fileName = Defines.STARTUP_PROG_PATH + "\\DailyResult\\" + date + ".txt";

            if (!File.Exists(fileName))
            {
                File.Create(fileName);
            }
        }
        public void SaveDailyResult()
        {
            string date = "Result" + DateTime.Now.ToString("ddMMyy");
            string fileName = Defines.STARTUP_PROG_PATH + "\\DailyResult\\" + date + ".txt";

            string[] arr = new string[arrPrintCountYes.Length];
            for (int i = 0; i < SettingsVM.NumberOfPrinter; i++)
            {
                arr[i] = arrPrintCountYes[i].ToString();
            }

            File.WriteAllLines(fileName, arr);
        }
        private void LoadDailyResultYes()
        {
            string dateYes = "Result" + DateTime.Now.AddDays(-1).ToString("ddMMyy");
            string fileNameYes = Defines.STARTUP_PROG_PATH + "\\DailyResult\\" + dateYes + ".txt";

            if (!File.Exists(fileNameYes))
            {
                return;
            }

            using (StreamReader rd = File.OpenText(fileNameYes))
            {
                for (int i = 0; i < SettingsVM.NumberOfPrinter; i++)
                {
                    arrPrintCountYes[i] = Convert.ToInt32(rd.ReadLine());
                }
            }
        }

        public void SendPrinterStatus(string printer, int status)
        {
            if (m_socket == null)
                return;

            if (!m_socket.IsConnected)
                return;

            string cmd = "*100@!?" + printer + "|" + status + "#";

            // show send data
            MainView.Dispatcher.Invoke(new Action(() =>
            {
                MainView.labelSendData.Content = cmd;
            }));

            m_socket.SendMsg(cmd);
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
                if (SetProperty(ref m_bAllowOperation, value))
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
                if (SetProperty(ref m_dOpacity, value))
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

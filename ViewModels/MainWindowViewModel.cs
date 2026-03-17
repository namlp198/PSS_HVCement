using DocumentFormat.OpenXml.Presentation;
using MVVMBasic;
using Ndev.NNetSocket;
using PSS_HVCement.ViewModels;
using PSS_HVCement.Commands.Cmd;
using PSS_HVCement.Common;
using PSS_HVCement.Manager;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using System.Reflection;

namespace PSS_HVCement.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private static readonly log4net.ILog log =
       log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly Dispatcher m_dispatcher;
        private MainWindow m_mainView;
        public MainWindow MainView { get => m_mainView; set => m_mainView = value; }

        private NClientSocket m_socket;

        private bool m_bConnectedServer = false;
        private System.Timers.Timer m_timReconnectServer = new System.Timers.Timer();
        private System.Timers.Timer m_timCheckShiftNow = new System.Timers.Timer();
        private int m_nShiftNow = -1;

        int[] arrPrintCountYes;

        private string m_strAppVersion = string.Empty;
        private string m_strAppName = string.Empty;

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

            m_timCheckShiftNow.Interval = 10000;
            m_timCheckShiftNow.Elapsed += M_timCheckShiftNow_Elapsed;
            m_timCheckShiftNow.Enabled = false;

            m_socket = new NClientSocket(SettingsVM.SysModel.IP, SettingsVM.SysModel.Port);
            m_socket.ConnectionEventCallback += M_socket_ConnectionEventCallback;
            m_socket.ClientErrorEventCallback += M_socket_ClientErrorEventCallback;

            if (SettingsVM.SysModel.UseAutoMode)
            {
                m_socket.ClientConnect();
                MainView.btnReconnet.Visibility = Visibility.Visible;
                MainView.labelStatusServer.Visibility = Visibility.Visible;
            }

            arrPrintCountYes = new int[SettingsVM.NumberOfPrinter];
            CreateDailyResult();
            LoadDailyResultYes();

            m_timReconnectServer.Interval = 8000;
            m_timReconnectServer.Elapsed += M_timReconnectServer_Elapsed;

            ShiftNow = CheckManufactureShift();

            GetAppInfo();

            log.Info("Initialize MainViewModel completed!");
        }

        private void M_timCheckShiftNow_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (ShiftNow != CheckManufactureShift())
            {
                ShiftNow = CheckManufactureShift();
            }
        }

        private void M_timReconnectServer_Elapsed(object sender, ElapsedEventArgs e)
        {
            ReconnectServer();
        }

        private void M_socket_ClientErrorEventCallback(string errorMsg)
        {

        }

        private void M_socket_ConnectionEventCallback(NClientSocket.EConnectionEventClient e, object obj)
        {
            switch (e)
            {
                case NClientSocket.EConnectionEventClient.RECEIVEDATA:

                    //format: *11@!?PP|Content#
                    if (m_socket.ReceiveString == null)
                        return;

                    if (SettingsVM.SysModel.IsShowData)
                    {
                        // show receive data
                        MainView.Dispatcher.Invoke(new Action(() =>
                        {
                            MainView.labelReceiveData.Content = m_socket.ReceiveString;
                        }));
                    }

                    if (m_socket.ReceiveString.Length < 6)
                        return;

                    log.Info($"Data receive from server: {m_socket.ReceiveString}");

                    if (m_socket.ReceiveString.StartsWith("*11@!?"))
                    {

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
                                    PrintersVM.KGKJetPrinter1.PerformPushMessageAuto();
                                }));
                                break;
                            case "02":
                                PrintersVM.KGKJetPrinter2.Dispatcher.Invoke(new Action(() =>
                                {
                                    PrintersVM.KGKJetPrinter2.MessageContent = content;
                                    PrintersVM.KGKJetPrinter2.PerformPushMessageAuto();
                                }));
                                break;
                            case "03":
                                PrintersVM.KGKJetPrinter3.Dispatcher.Invoke(new Action(() =>
                                {
                                    PrintersVM.KGKJetPrinter3.MessageContent = content;
                                    PrintersVM.KGKJetPrinter3.PerformPushMessageAuto();
                                }));
                                break;
                            case "04":
                                PrintersVM.KGKJetPrinter4.Dispatcher.Invoke(new Action(() =>
                                {
                                    PrintersVM.KGKJetPrinter4.MessageContent = content;
                                    PrintersVM.KGKJetPrinter4.PerformPushMessageAuto();
                                }));
                                break;
                            case "05":
                                PrintersVM.KGKJetPrinter5.Dispatcher.Invoke(new Action(() =>
                                {
                                    PrintersVM.KGKJetPrinter5.MessageContent = content;
                                    PrintersVM.KGKJetPrinter5.PerformPushMessageAuto();
                                }));
                                break;
                            case "06":
                                PrintersVM.KGKJetPrinter6.Dispatcher.Invoke(new Action(() =>
                                {
                                    PrintersVM.KGKJetPrinter6.MessageContent = content;
                                    PrintersVM.KGKJetPrinter6.PerformPushMessageAuto();
                                }));
                                break;
                        }
                    }
                    break;
                case NClientSocket.EConnectionEventClient.CLIENTCONNECTED:
                    IsConnectedServer = true;

                    m_timReconnectServer.Stop();

                    log.Info("Connect to server success!");
                    //m_timCheckShiftNow.Start();
                    PrintersVM.StartTimerSendData();
                    break;
                case NClientSocket.EConnectionEventClient.CLIENTDISCONNECTED:
                    IsConnectedServer = false;

                    log.Error("Disconnect to server");
                    PrintersVM.StopTimerSendData();

                    if(SettingsVM.SysModel.UseAutoMode)
                    {
                        m_timReconnectServer.Start();
                        log.Info("Reconnect to server");
                    }    
                    //m_timCheckShiftNow.Stop();
                    break;
                default:
                    break;
            }
        }

        public void ReconnectServer()
        {
            if (m_socket == null)
                return;

            if (m_socket.ClientSocket == null)
            {
                m_socket.ClientConnect();
                return;
            }

            if (!m_socket.IsConnected)
            {
                m_socket.ClientConnect();
            }
        }
        public void DisconnectServer()
        {
            if (m_socket == null)
                return;

            if (m_socket.IsConnected)
            {
                m_socket.ClientDisconnect();
            }
        }

        #region ViewModels
        public PrintersViewModel PrintersVM { get; set; }
        public DataCustomerViewModel DataCustomerVM { get; set; }
        public SettingsViewModel SettingsVM { get; set; }
        #endregion

        #region Functions
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
        private int CheckManufactureShift()
        {
            int hourNow = DateTime.Now.Hour;
            int dayNow = DateTime.Now.Day;

            if (hourNow >= 6 && hourNow < 14)
            {
                return 0; // Shift 1
            }
            else if (hourNow >= 14 && hourNow < 22)
            {
                return 1; // shitf 2
            }
            else if ((hourNow >= 22 && hourNow < 24) || (hourNow >= 0 && hourNow < 6))
            {
                return 2; // shift 3
            }
            else
            {
                return -1;
            }
        }

        public void SendPrinterStatus(string printer, int[] status)
        {
            if (m_socket == null)
                return;

            if (!m_socket.IsConnected)
                return;

            string st = string.Empty;
            for (int i = 0; i < status.Length; i++)
            {
                st += status[i].ToString() + "|";
            }

            string s = st.Substring(0, st.Length - 1);

            string cmd = "*100@!?" + s + "#";

            if (SettingsVM.SysModel.IsShowData)
            {
                // show send data
                MainView.Dispatcher.Invoke(new Action(() =>
                {
                    MainView.labelSendData.Content = cmd;
                }));
            }

            m_socket.SendMsg(cmd);
        }
        private void GetAppInfo()
        {
            // get info Assembly
            Assembly assembly = Assembly.GetExecutingAssembly();

            // get name
            string appName = assembly.GetName().Name;

            // get version
            Version version = assembly.GetName().Version;
            string appVersion = version.ToString();
            AppVersion = "Version: " + appVersion;
            AppName = appName;
        }
        #endregion

        #region Properties
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
        public bool IsConnectedServer
        {
            get => m_bConnectedServer;
            set
            {
                if (SetProperty(ref m_bConnectedServer, value))
                {

                }
            }
        }
        public int ShiftNow
        {
            get => m_nShiftNow;
            set
            {
                if (SetProperty(ref m_nShiftNow, value))
                {
                    PrintersVM.KGKJetPrinter1.ShiftNow = m_nShiftNow;
                    PrintersVM.KGKJetPrinter2.ShiftNow = m_nShiftNow;
                    PrintersVM.KGKJetPrinter3.ShiftNow = m_nShiftNow;
                }
            }
        }
        public string AppVersion
        {
            get => m_strAppVersion;
            set { SetProperty(ref m_strAppVersion, value);}
        }
        public string AppName
        {
            get => m_strAppName;
            set { SetProperty(ref m_strAppName, value); }
        }
        #endregion

        public ICommand AboutCmd { get; }
        public ICommand OpenSettingViewCmd { get; }
        public ICommand OpenReportViewCmd { get; }
        public ICommand OpenLoginViewCmd { get; }
    }
}

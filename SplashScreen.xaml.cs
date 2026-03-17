using JetPrinter.ui;
using PSS_HVCement.ViewModels;
using PSS_HVCement.Common;
using PSS_HVCement.Properties;
using PSS_HVCement.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PSS_HVCement
{
    /// <summary>
    /// Interaction logic for SplashScreen.xaml
    /// </summary>
    public partial class SplashScreen : Window
    {
        private static readonly log4net.ILog log =
         log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private bool closeCompleted = false;
        public SplashScreen()
        {
            InitializeComponent();
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            BackgroundWorker bgWorker = new BackgroundWorker();
            bgWorker.WorkerReportsProgress = true;
            bgWorker.DoWork += BgWorker_DoWork;
            bgWorker.ProgressChanged += BgWorker_ProgressChanged;
            bgWorker.RunWorkerAsync();
        }
        private void BgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            for (int i = 0; i <= 100; i++)
            {
                (sender as BackgroundWorker).ReportProgress(i);
                Thread.Sleep(2);
            }
        }


        private async void BgWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
            if (progressBar.Value == 100)
            {
                SettingView settingView = new SettingView();
                SettingsViewModel settingsVM = new SettingsViewModel(settingView.Dispatcher, settingView);
                settingView.DataContext = settingsVM;

                PrintersView printersView = new PrintersView();
                DataCustomerView dataView = new DataCustomerView();

                log.Info("Start create UI Printer");
                if (CheckRemainingMaintenancePeriod())
                {
                    PrintersViewModel printerVM = new PrintersViewModel(printersView.Dispatcher, printersView);
                    bool mode = settingsVM.SysModel.UseAutoMode;

                    printerVM.KGKJetPrinter1 = new KGKJetPrinterView(settingsVM.PrinterModels[0].IpPrinter, settingsVM.PrinterModels[0].Id);
                    printerVM.KGKJetPrinter1.SetParamsDefault(settingsVM.PrinterModels[0].TextModule, settingsVM.PrinterModels[0].UseTimerCheckPrintState,
                                                              settingsVM.PrinterModels[0].UseTimerCheckPrintCount, settingsVM.PrinterModels[0].CheckPrintStateDelay,
                                                              settingsVM.PrinterModels[0].CheckPrintCountDelay, settingsVM.PrinterModels[0].IsResetPrintCount, mode);

                    printerVM.KGKJetPrinter2 = new KGKJetPrinterView(settingsVM.PrinterModels[1].IpPrinter, settingsVM.PrinterModels[1].Id);
                    printerVM.KGKJetPrinter2.SetParamsDefault(settingsVM.PrinterModels[1].TextModule, settingsVM.PrinterModels[1].UseTimerCheckPrintState,
                                                              settingsVM.PrinterModels[1].UseTimerCheckPrintCount, settingsVM.PrinterModels[1].CheckPrintStateDelay,
                                                              settingsVM.PrinterModels[1].CheckPrintCountDelay, settingsVM.PrinterModels[1].IsResetPrintCount, mode);

                    printerVM.KGKJetPrinter3 = new KGKJetPrinterView(settingsVM.PrinterModels[2].IpPrinter, settingsVM.PrinterModels[2].Id);
                    printerVM.KGKJetPrinter3.SetParamsDefault(settingsVM.PrinterModels[2].TextModule, settingsVM.PrinterModels[2].UseTimerCheckPrintState,
                                                              settingsVM.PrinterModels[2].UseTimerCheckPrintCount, settingsVM.PrinterModels[2].CheckPrintStateDelay,
                                                              settingsVM.PrinterModels[2].CheckPrintCountDelay, settingsVM.PrinterModels[2].IsResetPrintCount, mode);

                    printerVM.KGKJetPrinter4 = new KGKJetPrinterView(settingsVM.PrinterModels[3].IpPrinter, settingsVM.PrinterModels[3].Id);
                    printerVM.KGKJetPrinter4.SetParamsDefault(settingsVM.PrinterModels[3].TextModule, settingsVM.PrinterModels[3].UseTimerCheckPrintState,
                                                              settingsVM.PrinterModels[3].UseTimerCheckPrintCount, settingsVM.PrinterModels[3].CheckPrintStateDelay,
                                                              settingsVM.PrinterModels[3].CheckPrintCountDelay, settingsVM.PrinterModels[3].IsResetPrintCount, mode);

                    printerVM.KGKJetPrinter5 = new KGKJetPrinterView(settingsVM.PrinterModels[4].IpPrinter, settingsVM.PrinterModels[4].Id);
                    printerVM.KGKJetPrinter5.SetParamsDefault(settingsVM.PrinterModels[4].TextModule, settingsVM.PrinterModels[4].UseTimerCheckPrintState,
                                                              settingsVM.PrinterModels[4].UseTimerCheckPrintCount, settingsVM.PrinterModels[4].CheckPrintStateDelay,
                                                              settingsVM.PrinterModels[4].CheckPrintCountDelay, settingsVM.PrinterModels[4].IsResetPrintCount, mode);

                    printerVM.KGKJetPrinter6 = new KGKJetPrinterView(settingsVM.PrinterModels[5].IpPrinter, settingsVM.PrinterModels[5].Id);
                    printerVM.KGKJetPrinter6.SetParamsDefault(settingsVM.PrinterModels[5].TextModule, settingsVM.PrinterModels[5].UseTimerCheckPrintState,
                                                              settingsVM.PrinterModels[5].UseTimerCheckPrintCount, settingsVM.PrinterModels[5].CheckPrintStateDelay,
                                                              settingsVM.PrinterModels[5].CheckPrintCountDelay, settingsVM.PrinterModels[5].IsResetPrintCount, mode);

                    printersView.contentPrinter1.Content = printerVM.KGKJetPrinter1;
                    printersView.contentPrinter2.Content = printerVM.KGKJetPrinter2;
                    printersView.contentPrinter3.Content = printerVM.KGKJetPrinter3;
                    printersView.contentPrinter4.Content = printerVM.KGKJetPrinter4;
                    printersView.contentPrinter5.Content = printerVM.KGKJetPrinter5;
                    printersView.contentPrinter6.Content = printerVM.KGKJetPrinter6;

                    printerVM.Initialize();
                    printersView.DataContext = printerVM;

                    DataCustomerViewModel dataVM = new DataCustomerViewModel(dataView.Dispatcher, dataView);
                    dataView.DataContext = dataVM;

                    MainWindow mainView = new MainWindow();
                    MainWindowViewModel mainViewModel = new MainWindowViewModel(mainView.Dispatcher, mainView, settingsVM, printerVM, dataVM);

                    mainView.contentPrinters.Content = printersView;
                    mainView.contentData.Content = dataView;
                    mainView.DataContext = mainViewModel;


                    for (double x = 1; x > 0; x -= 0.01d)
                    {
                        await Task.Delay(2);

                        this.Opacity = x;
                    }
                    this.Close();

                    mainView.Show();
                }
                else
                {
                    MainWindow mainView = new MainWindow();
                    MainWindowViewModel mainViewModel = new MainWindowViewModel(mainView.Dispatcher, mainView, null, null, null);

                    mainView.contentPrinters.Content = printersView;
                    mainView.contentData.Content = dataView;
                    mainView.DataContext = mainViewModel;

                    mainView.Show();

                    MessageBox.Show("Đã hết hạn bảo trì phần mềm \r\n Vui lòng liên hệ nhà cung cấp", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                    this.Close();

                }
            }
        }

        private bool CheckRemainingMaintenancePeriod()
        {
            string key = string.Empty;
            try
            {
                string file = string.Format(@"{0}\\maintenance.lic", Environment.CurrentDirectory);
                key = File.ReadAllText(file);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            if (string.IsNullOrEmpty(key))
                return false;

            SKGL.Validate validate = new SKGL.Validate();
            validate.secretPhase = "ndev";
            validate.Key = key;

            int dayRemaining = (validate.ExpireDate - DateTime.Now.Date).Days;
            if (dayRemaining <= 0)
            {
                log.Error("License key expire date!!!");
                return false;
            }    

            if (dayRemaining <= 14)
            {
                string s = string.Format("{0}: {1} {2}\r\n{3}", "Thời gian bảo trì phần mềm chỉ còn lại ", dayRemaining, "ngày", "Hãy liên hệ nhà cung cấp");
                MessageBox.Show(s, "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            Defines.DaysRemaining = dayRemaining;

            log.Info("Check remain maintenance success!");
            return true;
        }
    }
}

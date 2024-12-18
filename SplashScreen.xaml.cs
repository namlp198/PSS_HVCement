using JetPrinter.ui;
using PSS_HVCement.Common;
using PSS_HVCement.Properties;
using PSS_HVCement.ViewModels;
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
                PrintersView printersView = new PrintersView();
                DataCustomerView dataView = new DataCustomerView();

                if (CheckRemainingMaintenancePeriod())
                {
                    PrintersViewModel printerVM = new PrintersViewModel(printersView.Dispatcher, printersView);
                    printersView.contentPrinter1.Content = new KGKJetPrinterView("192.168.1.12", 1);
                    printersView.contentPrinter2.Content = new KGKJetPrinterView("192.168.1.13", 2);
                    printersView.contentPrinter3.Content = new KGKJetPrinterView("192.168.1.14", 3);
                    printersView.DataContext = printerVM;

                    DataCustomerViewModel dataVM = new DataCustomerViewModel(dataView.Dispatcher, dataView);
                    dataView.DataContext = dataVM;

                    MainWindow mainView = new MainWindow();
                    MainWindowViewModel mainViewModel = new MainWindowViewModel(mainView.Dispatcher, mainView, printerVM, dataVM);

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
                    MainWindowViewModel mainViewModel = new MainWindowViewModel(mainView.Dispatcher, mainView, null, null);

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
                return false;

            if (dayRemaining <= 14)
            {
                string s = string.Format("{0}: {1} {2}\r\n{3}", "Thời gian bảo trì phần mềm chỉ còn lại ", dayRemaining, "ngày", "Hãy liên hệ nhà cung cấp");
                MessageBox.Show(s, "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            Defines.DaysRemaining = dayRemaining;
            return true;
        }
    }
}

using JetPrinter.ui;
using PSS_HVCement.Properties;
using PSS_HVCement.ViewModels;
using PSS_HVCement.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
                PrintersViewModel printerVM = new PrintersViewModel(printersView.Dispatcher, printersView);
                printersView.contentPrinter1.Content = new KGKJetPrinterView("192.168.1.12");
                printersView.contentPrinter2.Content = new KGKJetPrinterView("");
                printersView.contentPrinter3.Content = new KGKJetPrinterView("");
                printersView.DataContext = printerVM;

                DataCustomerView dataView = new DataCustomerView();
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
        }
    }
}

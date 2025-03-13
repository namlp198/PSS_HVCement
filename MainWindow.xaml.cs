using PSS_HVCement.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PSS_HVCement
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool m_bFlag = false;
        public MainWindow()
        {
            InitializeComponent();
        }
        protected override void OnClosed(EventArgs e)
        {
            MainWindowViewModel.Instance.SaveDailyResult();

            base.OnClosed(e);

            Application.Current.Shutdown();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!m_bFlag)
            {
                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    MainWindowViewModel.Instance.SendPrinterStatus("01", 1);
                }));
            }
            else
            {
                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    MainWindowViewModel.Instance.SendPrinterStatus("01", 2);
                }));
            }

            m_bFlag = !m_bFlag;
        }
    }
}

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
using System.Windows.Shapes;

namespace PSS_HVCement.Views
{
    /// <summary>
    /// Interaction logic for SettingView.xaml
    /// </summary>
    public partial class SettingView : Window
    {
        public SettingView()
        {
            InitializeComponent();
        }

        private void btnResetPrintCount1_Click(object sender, RoutedEventArgs e)
        {
            MainWindowViewModel.Instance.PrintersVM.KGKJetPrinter1.ResetPrintCount();
        }

        private void btnResetPrintCount2_Click(object sender, RoutedEventArgs e)
        {
            MainWindowViewModel.Instance.PrintersVM.KGKJetPrinter2.ResetPrintCount();
        }

        private void btnResetPrintCount3_Click(object sender, RoutedEventArgs e)
        {
            MainWindowViewModel.Instance.PrintersVM.KGKJetPrinter3.ResetPrintCount();
        }

        private void chkUseCheckPrintCount_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void chkUseCheckPrintCount_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void btnSaveSysSetting_Click(object sender, RoutedEventArgs e)
        {
            MainWindowViewModel.Instance.SettingsVM.SaveSysSettings();
        }
    }
}

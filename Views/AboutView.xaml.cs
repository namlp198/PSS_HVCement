using PSS_HVCement.Common;
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
    /// Interaction logic for Abouts.xaml
    /// </summary>
    public partial class AboutView : Window
    {
        public AboutView()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            tbExpirationDay.Text = Defines.DaysRemaining + "";

            if(Defines.DaysRemaining <= 14)
            {
                tbExpirationDay.Foreground = Brushes.Red;
                btnRegister.IsEnabled = true;
            }
            else
            {
                tbExpirationDay.Foreground= Brushes.Green;
                btnRegister.IsEnabled = false;
            }
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            RegisterView registerView = new RegisterView();
            registerView.ShowDialog();
        }
    }
}

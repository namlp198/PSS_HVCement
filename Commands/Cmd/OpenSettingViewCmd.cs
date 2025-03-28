using PSS_HVCement.ViewModels;
using PSS_HVCement.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PSS_HVCement.Commands.Cmd
{
    public class OpenSettingViewCmd : CommandBase
    {
        public OpenSettingViewCmd() { }
        public override void Execute(object parameter)
        {
            try
            {
                //MainWindowViewModel.Instance.SettingsVM.SettingView.ShowDialog();
                SettingView settingView = new SettingView();
                MainWindowViewModel.Instance.SettingsVM = new SettingsViewModel(settingView.Dispatcher, settingView);
                settingView.DataContext = MainWindowViewModel.Instance.SettingsVM;
                settingView.ShowDialog();
            }
            catch (Exception ex)
            { 
                MessageBox.Show(ex.Message);
            }
        }
    }
}

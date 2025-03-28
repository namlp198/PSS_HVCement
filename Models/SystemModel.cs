using DocumentFormat.OpenXml.Presentation;
using MVVMBasic;
using PSS_HVCement.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSS_HVCement.Models
{
    public class SystemModel : ModelBase
    {
        private string _ip;
        private string _port;
        private bool _useAutoMode;
        private bool _isShowData;
        public string IP {  get; set; }
        public int Port { get; set; }
        public bool UseAutoMode
        {
            get => _useAutoMode;
            set
            {
                if(SetProperty(ref _useAutoMode, value))
                {
                    if (MainWindowViewModel.Instance == null)
                        return;

                    if(_useAutoMode)
                    {
                        MainWindowViewModel.Instance.ReconnectServer();
                        MainWindowViewModel.Instance.MainView.Dispatcher.Invoke(new Action(() =>
                        {
                            MainWindowViewModel.Instance.MainView.labelStatusServer.Visibility = System.Windows.Visibility.Visible;
                            MainWindowViewModel.Instance.MainView.btnReconnet.Visibility = System.Windows.Visibility.Visible;
                        }));
                    }
                    else
                    {
                        MainWindowViewModel.Instance.DisconnectServer();
                        MainWindowViewModel.Instance.MainView.Dispatcher.Invoke(new Action(() =>
                        {
                            MainWindowViewModel.Instance.MainView.labelStatusServer.Visibility = System.Windows.Visibility.Hidden;
                            MainWindowViewModel.Instance.MainView.btnReconnet.Visibility = System.Windows.Visibility.Hidden;
                        }));
                    }
                }
            }
        }
        public bool IsShowData
        {
            get => _isShowData;
            set
            {
                if(SetProperty(ref _isShowData, value))
                {
                    if (MainWindowViewModel.Instance == null)
                        return;

                    MainWindowViewModel.Instance.MainView.Dispatcher.Invoke(new Action(() =>
                    {
                        MainWindowViewModel.Instance.MainView.labelSendData.Content = null;
                    }));
                }
            }
        }
    }
}

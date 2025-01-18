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
    public class OpenLoginViewCmd : CommandBase
    {
        public OpenLoginViewCmd() { }
        public override void Execute(object parameter)
        {
            string s = parameter as string;
            if (s != null)
            {
                if (s.CompareTo("LOGIN") == 0)
                {
                    LoginView loginView = new LoginView();
                    loginView.ShowDialog();

                }
                else if (s.CompareTo("LOGOUT") == 0)
                {
                    MainWindowViewModel.Instance.ROLE = emRole.Role_Operator;
                }
            }
        }
    }
}

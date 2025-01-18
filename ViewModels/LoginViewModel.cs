using MVVMBasic;
using PSS_HVCement.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Windows.Threading;
using PSS_HVCement.Commands.Cmd;

namespace PSS_HVCement.ViewModels
{
    public enum emRole
    {
        Role_Operator,
        Role_Engineer,
        Role_Admin,
        Role_SuperAdmin
    }
    public enum emLoginStatus
    {
        LoginStatus_Success,
        LoginStatus_Failed
    }
    public class LoginViewModel : ViewModelBase
    {
        public delegate void LoginSystemSuccess_Handler(emLoginStatus eLoginSta, emRole eRole);
        public static event LoginSystemSuccess_Handler LoginSystemSuccessEvent;

        private string m_strEngineer = "engineer";
        private string m_strEngPassword = "1";
        private string m_strAdmin = "admin";
        private string m_strAdminPassword = "1234";
        private string m_strSuperAdmin = "superadmin";
        private string m_strSuperPassword = "1234";

        private emLoginStatus m_loginStatus;
        private emRole m_role;

        private readonly Dispatcher _dispatcher;
        private LoginView m_loginView;
        public LoginView LoginView { get => m_loginView; private set { } }
        public LoginViewModel(Dispatcher dispatcher, LoginView loginView)
        {
            _dispatcher = dispatcher;
            m_loginView = loginView;

            this.LoginCmd = new LoginCmd(this);
        }
        #region Method
        public bool CheckInfoLogin()
        {
            bool ret = false;

            if (Engineer.CompareTo(LoginView.txtUser.Text.Trim()) == 0
                && EngineerPassword.CompareTo(LoginView.pwBox.Password) == 0)
            {
                ret = true;
                LoginSystemSuccessEvent?.Invoke(emLoginStatus.LoginStatus_Success, emRole.Role_Engineer);
            }
            else if (Admin.CompareTo(LoginView.txtUser.Text.Trim()) == 0
                && AdminPassword.CompareTo(LoginView.pwBox.Password) == 0)
            {
                ret = true;
                LoginSystemSuccessEvent?.Invoke(emLoginStatus.LoginStatus_Success, emRole.Role_Admin);
            }
            else if (SuperAdmin.CompareTo(LoginView.txtUser.Text.Trim()) == 0
                && SuperPassword.CompareTo(LoginView.pwBox.Password) == 0)
            {
                ret = true;
                LoginSystemSuccessEvent?.Invoke(emLoginStatus.LoginStatus_Success, emRole.Role_SuperAdmin);
            }
            else
            {
                ret = false;
                LoginSystemSuccessEvent?.Invoke(emLoginStatus.LoginStatus_Failed, emRole.Role_Operator);
                MessageBox.Show("Login Failed! Try again", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return ret;
        }
        #endregion
        #region Properties
        public emLoginStatus LoginStatus
        {
            get => m_loginStatus;
            set => m_loginStatus = value;
        }
        public emRole Role
        {
            get => m_role;
            set => m_role = value;
        }
        public string Engineer
        {
            get => m_strEngineer;
            set => m_strEngineer = value;
        }
        public string EngineerPassword
        {
            get => m_strEngPassword;
            set => m_strEngPassword = value;
        }
        public string Admin
        {
            get => m_strAdmin;
            set => m_strAdmin = value;
        }
        public string AdminPassword
        {
            get => m_strAdminPassword;
            set => m_strAdminPassword = value;
        }
        public string SuperAdmin
        {
            get => m_strSuperAdmin;
            set => m_strSuperAdmin = value;
        }
        public string SuperPassword
        {
            get => m_strSuperPassword;
            set => m_strSuperPassword = value;
        }
        #endregion
        public ICommand LoginCmd { get; }
    }
}

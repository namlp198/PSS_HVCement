using PSS_HVCement.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PSS_HVCement.Commands.Cmd
{
    public class OpenReportViewCmd : CommandBase
    {
        public OpenReportViewCmd() { }
        public override void Execute(object parameter)
        {
            ReportView reportView = new ReportView();
            reportView.ShowDialog();
        }
    }
}

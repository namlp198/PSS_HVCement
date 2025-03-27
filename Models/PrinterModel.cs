using DocumentFormat.OpenXml.Presentation;
using MVVMBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSS_HVCement.Models
{
    public class PrinterModel : ModelBase
    {
        public int Id { get; set; }
        public string IpPrinter { get; set; }
        public bool IsShowPrintCount { get; set; }
        public bool UseTimerCheckPrintState { get; set; }
        public bool UseTimerCheckPrintCount { get; set; }
        public bool IsResetPrintCount { get; set; }
        public int TextModule { get; set; }
        public int CheckPrintStateDelay { get; set; }
        public int CheckPrintCountDelay { get; set; }
    }
}

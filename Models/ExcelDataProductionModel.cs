using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSS_HVCement.Models
{
    public class ExcelDataProductionModel
    {
        public string PDate { get; set; }
        public string PStartTime { get; set; }
        public string PEndTime { get; set; }
        public string PShift { get; set; }
        public string DeliveryCode { get; set; }
        public string PrintCode { get; set; }
        public int PrintCount { get; set; }
    }
}

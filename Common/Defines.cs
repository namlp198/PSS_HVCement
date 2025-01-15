using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PSS_HVCement.Common
{
    public class Defines
    {
        public static string ProductID = string.Empty;
        public static int DaysRemaining = 0;
        public const int PRINTER_COUNT = 3;
        public static string STARTUP_PROG_PATH = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
    }
}

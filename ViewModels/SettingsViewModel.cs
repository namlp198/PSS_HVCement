using MVVMBasic;
using PSS_HVCement.Common;
using PSS_HVCement.Models;
using PSS_HVCement.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Xml;

namespace PSS_12Printer.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        private readonly Dispatcher m_dispatcher;
        private SettingView m_settingView;
        private List<PrinterModel> m_printerModels = new List<PrinterModel>();
        private XmlManagement m_xmlManagement = new XmlManagement();
        public SettingView PrinterView { get { return m_settingView; } set { m_settingView = value; } }
        public List<PrinterModel> PrinterModels { get => m_printerModels; set { m_printerModels = value; } }
        public SettingsViewModel(Dispatcher dispatcher, SettingView settingView)
        {
            m_dispatcher = dispatcher;
            m_settingView = settingView;

            LoadSettings();
        }

        private void LoadSettings()
        {
            string settingsPath = Defines.STARTUP_PROG_PATH + "\\Settings.config";

            m_xmlManagement.Load(settingsPath);

            // Printer 01
            XmlNode nodePrinter01 = m_xmlManagement.SelectSingleNode("//Configurations//Printer01");
            if(nodePrinter01 != null)
            {
                PrinterModel model1 = new PrinterModel();
                int.TryParse(m_xmlManagement.GetAttributeValueFromNode(nodePrinter01, "Id"), out int id);
                model1.Id = id;
                model1.IpPrinter = m_xmlManagement.GetAttributeValueFromNode(nodePrinter01, "IP");
                model1.IsShowPrintCount = string.Compare(m_xmlManagement.GetAttributeValueFromNode(nodePrinter01, "IsShowPrintCount"), "true") == 1 ? true : false;

                m_printerModels.Add(model1);
            }

            // Printer 02
            XmlNode nodePrinter02 = m_xmlManagement.SelectSingleNode("//Configurations//Printer02");
            if (nodePrinter02 != null)
            {
                PrinterModel model2 = new PrinterModel();
                int.TryParse(m_xmlManagement.GetAttributeValueFromNode(nodePrinter02, "Id"), out int id);
                model2.Id = id;
                model2.IpPrinter = m_xmlManagement.GetAttributeValueFromNode(nodePrinter02, "IP");
                model2.IsShowPrintCount = string.Compare(m_xmlManagement.GetAttributeValueFromNode(nodePrinter01, "IsShowPrintCount"), "true") == 1 ? true : false;

                m_printerModels.Add(model2);
            }

            // Printer 03
            XmlNode nodePrinter03 = m_xmlManagement.SelectSingleNode("//Configurations//Printer03");
            if (nodePrinter03 != null)
            {
                PrinterModel model3 = new PrinterModel();
                int.TryParse(m_xmlManagement.GetAttributeValueFromNode(nodePrinter03, "Id"), out int id);
                model3.Id = id;
                model3.IpPrinter = m_xmlManagement.GetAttributeValueFromNode(nodePrinter03, "IP");
                model3.IsShowPrintCount = string.Compare(m_xmlManagement.GetAttributeValueFromNode(nodePrinter01, "IsShowPrintCount"), "true") == 1 ? true : false;

                m_printerModels.Add(model3);
            }

            m_xmlManagement.Close();
        }
    }
}

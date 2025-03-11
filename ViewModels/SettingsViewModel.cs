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
        private SystemModel m_sysModel = new SystemModel();
        private XmlManagement m_xmlManagement = new XmlManagement();

        public SettingView PrinterView { get { return m_settingView; } set { m_settingView = value; } }
        public List<PrinterModel> PrinterModels { get => m_printerModels; set { m_printerModels = value; } }
        public SystemModel SysModel { get => m_sysModel; set {  m_sysModel = value; } }
        public SettingsViewModel(Dispatcher dispatcher, SettingView settingView)
        {
            m_dispatcher = dispatcher;
            m_settingView = settingView;

            LoadSysSettings();
            LoadSettings();
        }

        private int m_nNumberOfPrinter = 0;
        public int NumberOfPrinter
        {
            get => m_nNumberOfPrinter;
            set
            {
                if(SetProperty(ref m_nNumberOfPrinter, value))
                {

                }    
            }
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
                model1.IsShowPrintCount = string.Compare(m_xmlManagement.GetAttributeValueFromNode(nodePrinter01, "IsShowPrintCount"), "true") == 0 ? true : false;
                model1.TextModule = int.Parse(m_xmlManagement.GetAttributeValueFromNode(nodePrinter01, "TextModule"));
                model1.UseTimerCheckPrintState = string.Compare(m_xmlManagement.GetAttributeValueFromNode(nodePrinter01, "UseTimerCheckPrintState"), "true") == 0 ? true : false;
                model1.UseTimerCheckPrintCount = string.Compare(m_xmlManagement.GetAttributeValueFromNode(nodePrinter01, "UseTimerCheckPrintCount"), "true") == 0 ? true : false;
                model1.CheckPrintStateDelay = int.Parse(m_xmlManagement.GetAttributeValueFromNode(nodePrinter01, "CheckPrintStateDelay"));
                model1.CheckPrintCountDelay = int.Parse(m_xmlManagement.GetAttributeValueFromNode(nodePrinter01, "CheckPrintCountDelay"));
                model1.IsResetPrintCount = string.Compare(m_xmlManagement.GetAttributeValueFromNode(nodePrinter01, "IsResetPrintCount"), "true") == 0 ? true : false;

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
                model2.IsShowPrintCount = string.Compare(m_xmlManagement.GetAttributeValueFromNode(nodePrinter01, "IsShowPrintCount"), "true") == 0 ? true : false;
                model2.TextModule = int.Parse(m_xmlManagement.GetAttributeValueFromNode(nodePrinter02, "TextModule"));
                model2.UseTimerCheckPrintState = string.Compare(m_xmlManagement.GetAttributeValueFromNode(nodePrinter02, "UseTimerCheckPrintState"), "true") == 0 ? true : false;
                model2.UseTimerCheckPrintCount = string.Compare(m_xmlManagement.GetAttributeValueFromNode(nodePrinter02, "UseTimerCheckPrintCount"), "true") == 0 ? true : false;
                model2.CheckPrintStateDelay = int.Parse(m_xmlManagement.GetAttributeValueFromNode(nodePrinter02, "CheckPrintStateDelay"));
                model2.CheckPrintCountDelay = int.Parse(m_xmlManagement.GetAttributeValueFromNode(nodePrinter02, "CheckPrintCountDelay"));
                model2.IsResetPrintCount = string.Compare(m_xmlManagement.GetAttributeValueFromNode(nodePrinter02, "IsResetPrintCount"), "true") == 0 ? true : false;

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
                model3.IsShowPrintCount = string.Compare(m_xmlManagement.GetAttributeValueFromNode(nodePrinter01, "IsShowPrintCount"), "true") == 0 ? true : false;
                model3.TextModule = int.Parse(m_xmlManagement.GetAttributeValueFromNode(nodePrinter03, "TextModule"));
                model3.UseTimerCheckPrintState = string.Compare(m_xmlManagement.GetAttributeValueFromNode(nodePrinter03, "UseTimerCheckPrintState"), "true") == 0 ? true : false;
                model3.UseTimerCheckPrintCount = string.Compare(m_xmlManagement.GetAttributeValueFromNode(nodePrinter03, "UseTimerCheckPrintCount"), "true") == 0 ? true : false;
                model3.CheckPrintStateDelay = int.Parse(m_xmlManagement.GetAttributeValueFromNode(nodePrinter03, "CheckPrintStateDelay"));
                model3.CheckPrintCountDelay = int.Parse(m_xmlManagement.GetAttributeValueFromNode(nodePrinter03, "CheckPrintCountDelay"));
                model3.IsResetPrintCount = string.Compare(m_xmlManagement.GetAttributeValueFromNode(nodePrinter03, "IsResetPrintCount"), "true") == 0 ? true : false;

                m_printerModels.Add(model3);
            }

            NumberOfPrinter = m_printerModels.Count;

            m_xmlManagement.Close();
        }
        private void LoadSysSettings()
        {
            string settingsPath = Defines.STARTUP_PROG_PATH + "\\Settings.config";

            m_xmlManagement.Load(settingsPath);

            // System setting
            XmlNode nodeServer = m_xmlManagement.SelectSingleNode("//Configurations//Systems//Server");

            if(nodeServer != null)
            {
                SystemModel sysModel = new SystemModel();
                sysModel.IP = m_xmlManagement.GetAttributeValueFromNode(nodeServer, "IP");
                sysModel.Port = int.Parse(m_xmlManagement.GetAttributeValueFromNode(nodeServer, "Port"));

                m_sysModel = sysModel;
            }
        }
    }
}

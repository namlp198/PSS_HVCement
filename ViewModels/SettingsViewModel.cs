using MVVMBasic;
using PSS_HVCement.Common;
using PSS_HVCement.Models;
using PSS_HVCement.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using System.Xml;

namespace PSS_HVCement.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        private static readonly log4net.ILog log =
      log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly Dispatcher m_dispatcher;
        private SettingView m_settingView;
        private List<PrinterModel> m_printerModels = new List<PrinterModel>();
        private SystemModel m_sysModel = new SystemModel();
        private XmlManagement m_xmlManagement = new XmlManagement();

        public SettingView SettingView { get { return m_settingView; } set { m_settingView = value; } }
        public List<PrinterModel> PrinterModels { get => m_printerModels; set { m_printerModels = value; } }
        public SystemModel SysModel
        {
            get => m_sysModel; set
            {
                if (SetProperty(ref m_sysModel, value))
                {

                }
            }
        }
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

            try
            {
                m_xmlManagement.Load(settingsPath);

                // Printer 01
                XmlNode nodePrinter01 = m_xmlManagement.SelectSingleNode("//Configurations//Printer01");
                List<PrinterModel> printers = new List<PrinterModel>();
                if (nodePrinter01 != null)
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

                    log.Info($"Text Module 1: {model1.TextModule}");
                    printers.Add(model1);
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

                    log.Info($"Text Module 2: {model2.TextModule}");
                    printers.Add(model2);
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

                    log.Info($"Text Module 3: {model3.TextModule}");
                    printers.Add(model3);
                }

                // Printer 04
                XmlNode nodePrinter04 = m_xmlManagement.SelectSingleNode("//Configurations//Printer04");
                if (nodePrinter04 != null)
                {
                    PrinterModel model4 = new PrinterModel();
                    int.TryParse(m_xmlManagement.GetAttributeValueFromNode(nodePrinter04, "Id"), out int id);
                    model4.Id = id;
                    model4.IpPrinter = m_xmlManagement.GetAttributeValueFromNode(nodePrinter04, "IP");
                    model4.IsShowPrintCount = string.Compare(m_xmlManagement.GetAttributeValueFromNode(nodePrinter04, "IsShowPrintCount"), "true") == 0 ? true : false;
                    model4.TextModule = int.Parse(m_xmlManagement.GetAttributeValueFromNode(nodePrinter04, "TextModule"));
                    model4.UseTimerCheckPrintState = string.Compare(m_xmlManagement.GetAttributeValueFromNode(nodePrinter04, "UseTimerCheckPrintState"), "true") == 0 ? true : false;
                    model4.UseTimerCheckPrintCount = string.Compare(m_xmlManagement.GetAttributeValueFromNode(nodePrinter04, "UseTimerCheckPrintCount"), "true") == 0 ? true : false;
                    model4.CheckPrintStateDelay = int.Parse(m_xmlManagement.GetAttributeValueFromNode(nodePrinter04, "CheckPrintStateDelay"));
                    model4.CheckPrintCountDelay = int.Parse(m_xmlManagement.GetAttributeValueFromNode(nodePrinter04, "CheckPrintCountDelay"));
                    model4.IsResetPrintCount = string.Compare(m_xmlManagement.GetAttributeValueFromNode(nodePrinter04, "IsResetPrintCount"), "true") == 0 ? true : false;

                    log.Info($"Text Module 4: {model4.TextModule}");
                    printers.Add(model4);
                }

                // Printer 05
                XmlNode nodePrinter05 = m_xmlManagement.SelectSingleNode("//Configurations//Printer05");
                if (nodePrinter05 != null)
                {
                    PrinterModel model5 = new PrinterModel();
                    int.TryParse(m_xmlManagement.GetAttributeValueFromNode(nodePrinter05, "Id"), out int id);
                    model5.Id = id;
                    model5.IpPrinter = m_xmlManagement.GetAttributeValueFromNode(nodePrinter05, "IP");
                    model5.IsShowPrintCount = string.Compare(m_xmlManagement.GetAttributeValueFromNode(nodePrinter05, "IsShowPrintCount"), "true") == 0 ? true : false;
                    model5.TextModule = int.Parse(m_xmlManagement.GetAttributeValueFromNode(nodePrinter05, "TextModule"));
                    model5.UseTimerCheckPrintState = string.Compare(m_xmlManagement.GetAttributeValueFromNode(nodePrinter05, "UseTimerCheckPrintState"), "true") == 0 ? true : false;
                    model5.UseTimerCheckPrintCount = string.Compare(m_xmlManagement.GetAttributeValueFromNode(nodePrinter05, "UseTimerCheckPrintCount"), "true") == 0 ? true : false;
                    model5.CheckPrintStateDelay = int.Parse(m_xmlManagement.GetAttributeValueFromNode(nodePrinter05, "CheckPrintStateDelay"));
                    model5.CheckPrintCountDelay = int.Parse(m_xmlManagement.GetAttributeValueFromNode(nodePrinter05, "CheckPrintCountDelay"));
                    model5.IsResetPrintCount = string.Compare(m_xmlManagement.GetAttributeValueFromNode(nodePrinter05, "IsResetPrintCount"), "true") == 0 ? true : false;

                    log.Info($"Text Module 5: {model5.TextModule}");
                    printers.Add(model5);
                }

                // Printer 06
                XmlNode nodePrinter06 = m_xmlManagement.SelectSingleNode("//Configurations//Printer06");
                if (nodePrinter06 != null)
                {
                    PrinterModel model6 = new PrinterModel();
                    int.TryParse(m_xmlManagement.GetAttributeValueFromNode(nodePrinter06, "Id"), out int id);
                    model6.Id = id;
                    model6.IpPrinter = m_xmlManagement.GetAttributeValueFromNode(nodePrinter06, "IP");
                    model6.IsShowPrintCount = string.Compare(m_xmlManagement.GetAttributeValueFromNode(nodePrinter06, "IsShowPrintCount"), "true") == 0 ? true : false;
                    model6.TextModule = int.Parse(m_xmlManagement.GetAttributeValueFromNode(nodePrinter06, "TextModule"));
                    model6.UseTimerCheckPrintState = string.Compare(m_xmlManagement.GetAttributeValueFromNode(nodePrinter06, "UseTimerCheckPrintState"), "true") == 0 ? true : false;
                    model6.UseTimerCheckPrintCount = string.Compare(m_xmlManagement.GetAttributeValueFromNode(nodePrinter06, "UseTimerCheckPrintCount"), "true") == 0 ? true : false;
                    model6.CheckPrintStateDelay = int.Parse(m_xmlManagement.GetAttributeValueFromNode(nodePrinter06, "CheckPrintStateDelay"));
                    model6.CheckPrintCountDelay = int.Parse(m_xmlManagement.GetAttributeValueFromNode(nodePrinter06, "CheckPrintCountDelay"));
                    model6.IsResetPrintCount = string.Compare(m_xmlManagement.GetAttributeValueFromNode(nodePrinter06, "IsResetPrintCount"), "true") == 0 ? true : false;

                    log.Info($"Text Module 6: {model6.TextModule}");
                    printers.Add(model6);
                }

                PrinterModels = printers;
                NumberOfPrinter = m_printerModels.Count;

                m_xmlManagement.Close();
            }
            catch (Exception e)
            {
                log.Error($"Occur exception Load System Settings file [LOAD PRINTER]: {e.Message}" );
                MessageBox.Show(e.Message);
            }
        }
        private void LoadSysSettings()
        {
            string settingsPath = Defines.STARTUP_PROG_PATH + "\\Settings.config";

            try
            {
                m_xmlManagement.Load(settingsPath);

                // System setting
                XmlNode nodeServer = m_xmlManagement.SelectSingleNode("//Configurations//Systems//Server");

                if (nodeServer != null)
                {
                    SystemModel sysModel = new SystemModel();
                    sysModel.IP = m_xmlManagement.GetAttributeValueFromNode(nodeServer, "IP");
                    sysModel.Port = int.Parse(m_xmlManagement.GetAttributeValueFromNode(nodeServer, "Port"));
                    sysModel.UseAutoMode = string.Compare(m_xmlManagement.GetAttributeValueFromNode(nodeServer, "UseAutoMode"), "true") == 0 ? true : false;
                    sysModel.IsShowData = string.Compare(m_xmlManagement.GetAttributeValueFromNode(nodeServer, "IsShowData"), "true") == 0 ? true : false;

                    SysModel = sysModel;
                }
            }
            catch (Exception e)
            {
                log.Error($"Occur exception Load System Settings file [LOAD SYSTEMS CONFIG]: {e.Message}" );
                MessageBox.Show(e.Message);
            }
        }
        public void SaveSysSettings()
        {
            string settingsPath = Defines.STARTUP_PROG_PATH + "\\Settings.config";

            try
            {
                m_xmlManagement.Load(settingsPath);

                // System setting
                XmlNode nodeServer = m_xmlManagement.SelectSingleNode("//Configurations//Systems//Server");

                if (nodeServer != null)
                {
                    m_xmlManagement.SetAttributeValueFromXPath("//Configurations//Systems//Server", "UseAutoMode", SysModel.UseAutoMode.ToString().ToLower());
                    m_xmlManagement.SetAttributeValueFromXPath("//Configurations//Systems//Server", "IsShowData", SysModel.IsShowData.ToString().ToLower());
                }

                if (m_xmlManagement.Save(settingsPath))
                {
                    MessageBox.Show("Save success");
                }
            }
            catch (Exception e)
            {
                log.Error($"Save Setting Failed! {e.Message}");
                MessageBox.Show(e.Message);
            }
        }
    }
}

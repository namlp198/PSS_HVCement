﻿using JetPrinter.ui;
using MVVMBasic;
using PSS_HVCement.Models;
using PSS_HVCement.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace PSS_HVCement.ViewModels
{
    public class DataCustomerViewModel : ViewModelBase
    {
        private readonly Dispatcher m_dispatcher;
        private DataCustomerView m_dataCusView;

        private List<DataCustomerModel> m_listDataCusModel = new List<DataCustomerModel>();
        private DataCustomerModel m_dataCusModelSelected = new DataCustomerModel();

        public DataCustomerView DataCustomerView { get => m_dataCusView; set { m_dataCusView = value; } }
        public DataCustomerViewModel(Dispatcher dispatcher, DataCustomerView dataCustomerView) 
        { 
            m_dispatcher = dispatcher;
            m_dataCusView = dataCustomerView;

            List<DataCustomerModel> lst = new List<DataCustomerModel>();
            DataCustomerModel model1 = new DataCustomerModel();
            model1.Idx = 1;
            model1.Delivery_Code = "abcxyz123";
            model1.Print_Code = "xxx";
            DataCustomerModel model2 = new DataCustomerModel();
            model2.Idx = 2;
            model2.Delivery_Code = "abcxyz456";
            model2.Print_Code = "yyy";
            lst.Add(model1);
            lst.Add(model2);

            DataCustomers = lst;
        }

        public List<DataCustomerModel> DataCustomers
        {
            get => m_listDataCusModel;
            set
            {
                if(SetProperty(ref m_listDataCusModel, value))
                {
                    
                }
            }
        }
        public DataCustomerModel DataCustomerModelSelected
        {
            get => m_dataCusModelSelected;
            set
            {
                if(SetProperty(ref m_dataCusModelSelected, value))
                {
                    var KGKPrinter1 = MainWindowViewModel.Instance.PrintersVM.PrinterView.contentPrinter1.Content as KGKJetPrinterView;
                    var KGKPrinter2 = MainWindowViewModel.Instance.PrintersVM.PrinterView.contentPrinter2.Content as KGKJetPrinterView;
                    var KGKPrinter3 = MainWindowViewModel.Instance.PrintersVM.PrinterView.contentPrinter3.Content as KGKJetPrinterView;
                    if (KGKPrinter1 != null)
                    {
                        KGKPrinter1.MessageContent = m_dataCusModelSelected.Print_Code + KGKPrinter1.DateTimeSelected;
                    }
                    if (KGKPrinter2 != null)
                    {
                        KGKPrinter2.MessageContent = m_dataCusModelSelected.Print_Code + KGKPrinter2.DateTimeSelected;
                    }
                    if (KGKPrinter3 != null)
                    {
                        KGKPrinter3.MessageContent = m_dataCusModelSelected.Print_Code + KGKPrinter3.DateTimeSelected;
                    }
                }
            }
        }
    }
}

using MVVMBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSS_HVCement.Models
{
    public class DataCustomerModel : ModelBase
    {
        //private int m_nIdx;
        private string m_sDelivery_Code;
        private string m_sPrint_Code;

        //public int Idx
        //{
        //    get { return m_nIdx; }
        //    set
        //    {
        //        if (SetProperty(ref m_nIdx, value))
        //        {

        //        }
        //    }
        //}
        public string Delivery_Code
        {
            get { return m_sDelivery_Code; }
            set
            {
                if (SetProperty(ref m_sDelivery_Code, value))
                {

                }
            }
        }
        public string Print_Code
        {
            get { return m_sPrint_Code; }
            set
            {
                if (SetProperty(ref m_sPrint_Code, value))
                {

                }
            }
        }
    }
}

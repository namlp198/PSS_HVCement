using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using PSS_HVCement.ViewModels;
using PSS_HVCement.Models;
using PSS_HVCement.Utils;

namespace PSS_HVCement.Commands.Cmd
{
    public class GetDataCustomerCmd : CommandBase
    {
        public GetDataCustomerCmd() { }

        public override void Execute(object parameter)
        {
            DateTime d1 = DateTime.Now;
            d1 = d1.AddDays(-1);
            string strFormat = "yyyy-MM-dd 00:00:00";
            string val1 = d1.ToString(strFormat);
            DateTime d2 = DateTime.Now;
            d2 = d2.AddDays(+1);
            string val2 = d2.ToString(strFormat);

            string connectionString = "Data Source=192.168.170.4,1433;Initial Catalog=DongBo;Persist Security Info=True;User ID=sa; Password=Haivan@1234";
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT Delivery_Code,Print_Code FROM WSDBs.DeliveryCodeMap where NgayTao>@val1 and Status='RECEIVING' order by Delivery_Code asc", con))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@val1", val1);
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            //DataSet ds = new DataSet();
                            //sda.Fill(ds, "danhsachxe");
                            //dataGridView1.DataSource = ds;
                            //dataGridView1.DataMember = "danhsachxe";

                            DataTable dt = new DataTable();
                            sda.Fill(dt);

                            MainWindowViewModel.Instance.DataCustomerVM.DataCustomers = dt.ToList<DataCustomerModel>();
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}

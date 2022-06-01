using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CN_BaoHanh_SanPham
{
    class ConnectDatabase
    {
        static SqlConnection con;
        static SqlDataAdapter da;
        static DataSet ds;
        static SqlCommand cmd;
        static string strCon;

        public static void openConnection()
        {
            strCon = @"Data Source=VanHien;Initial Catalog=DA_QLBANTHIETBIDIENTU_CHUNANG_BAOHANH_1;Integrated Security=True;User ID=sa;Password=123";
            con = new SqlConnection(strCon);
            try
            {
                con.Open();
            }catch
            {
                MessageBox.Show("Error", "Error when connecting your database");
                return;
            }
        }
        public static void executeQuery(String query)
        {
            cmd = new SqlCommand();
            openConnection();
            try
            {
                cmd.Connection = con;
                cmd.CommandText = query;
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
                con.Close();
            }
            catch
            {
                MessageBox.Show("Error", "Error");
                return;
            }
        }
        public static DataSet getDataSet(String query)
        {
            openConnection();
            da = new SqlDataAdapter(query, con);
            ds = new DataSet();
            da.Fill(ds);
            con.Close();
            return ds;
        }
        public static DataTable getDataTable(String query)
        {
                openConnection();
                da = new SqlDataAdapter(query, con);
                ds = new DataSet();
                da.Fill(ds);
                con.Close();
                return ds.Tables[0];
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace CN_BaoHanh_SanPham
{
    public partial class DangNhap : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source = VanHien; Initial Catalog = DA_QLBANTHIETBIDIENTU_CHUNANG_BAOHANH_1; User ID = sa; Password = 123");
        SqlCommand cmd;
        public DangNhap()
        {
            InitializeComponent();
        }

        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                string query = "SELECT * FROM NHANVIEN WHERE MaNV = '" + txtUserName.Text + "'";
                cmd = new SqlCommand(query, con);
                SqlDataReader dr = cmd.ExecuteReader();
                if(dr.HasRows)
                {
                   while(dr.Read())
                    {
                        txtPassword.Text = dr["Hoten"].ToString();
                    }
                }
                else
                {
                    MessageBox.Show("KHOOGN TIM THAY", "THONGBAO");
                }
                dr.Close();
                con.Close();
            }
            catch
            {
                MessageBox.Show("LỗI KẾT NỐI", "THÔNG BÁO LỖI");
            }
        }
    }
}

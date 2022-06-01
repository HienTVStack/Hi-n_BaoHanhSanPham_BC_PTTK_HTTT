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
    public partial class Form1 : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source = VanHien; Initial Catalog = DA_QLBANTHIETBIDIENTU_CHUNANG_BAOHANH; User ID = sa; Password = 123");
        SqlCommand cmd;
        public Form1()
        {
            InitializeComponent();
        }

        private void txtIDKH_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (txtIDKH.Text == "")
            {
                MessageBox.Show("CHƯA NHẬP THÔNG TIN TÌM KIẾM", "THÔNG BÁO");
                return;
            }
            else
            {
                DataTable dt = ConnectDatabase.getDataTable("SELECT * FROM TAIKHOAN WHERE SDT = '" + txtIDKH.Text + "'");
                if (dt.Rows.Count > 0)
                {
                    //Hiển thị thông tin sản phẩm khách hàng đã mua và chưa bảo hành
                    KH_DanhSachChuaBaoHanh();
                }
                else
                {
                    MessageBox.Show("KHÔNG TÌM THẤY KHÁCH HÀNG", "THÔNG BÁO");
                }
            }
        }
        public void HienThi_DataGridView()
        {
            dataGridView1.ColumnCount = 2;
            dataGridView1.Columns[0].Name = "Tên sản phẩm";
            dataGridView1.Columns[1].Name = "ABC";
        }

        public void KH_DanhSachChuaBaoHanh()
        {
            HienThi_DataGridView();
            string query = "SELECT* FROM HOADON, CHITIETHOADON " +
                "WHERE HOADON.ID = CHITIETHOADON.ID_HD " +
                "AND HOADON.ID_KH = '0337122712' " +
                "AND CHITIETHOADON.TRANGTHAIBAOHANH = 0";
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();

            try
            {
                con.Open();
                cmd = new SqlCommand(query, con);
                SqlDataReader dr = cmd.ExecuteReader();
                string[] row = new string[] { };
                while(dr.Read())
                {
                    

                    row = new string[] { ID_TenSanPham(dr["ID_SP"].ToString()), dr["SOLUONG"].ToString() };
                    dataGridView1.Rows.Add(row);
                }
                dr.Close();
                con.Close();
            }
            catch
            {
                MessageBox.Show("CO LOI");
            }
        }
        public string ID_TenSanPham(string id)
        {
            string tenSanPham; 
            string query = "SELECT * FROM SANPHAM WHERE ID = '" + id.ToString() + "'";
            try
            {
                con.Open();
                cmd = new SqlCommand(query, con);
                SqlDataReader dr = cmd.ExecuteReader();

                tenSanPham = dr["TEN"].ToString();

                dr.Close();
                con.Close();
            }
            catch
            {
                tenSanPham = "Không xác định";
                MessageBox.Show("CO LOI ID_TenSanPham");
            }
            return tenSanPham;

        }
    }
}

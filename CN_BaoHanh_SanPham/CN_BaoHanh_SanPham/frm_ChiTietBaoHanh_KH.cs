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
using System.Net;
using System.Net.Mail;
using System.Threading;

namespace CN_BaoHanh_SanPham
{
    public partial class frm_ChiTietBaoHanh_KH : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source = VanHien; Initial Catalog = DA_QLBANTHIETBIDIENTU_CHUNANG_BAOHANH_1; User ID = sa; Password = 123");
        SqlCommand cmd;
        NetworkCredential login;
        SmtpClient client;
        MailMessage msg;
        WaitFormFunc waitForm = new WaitFormFunc();
        public frm_ChiTietBaoHanh_KH()
        {
            InitializeComponent();
            timer1.Start();
            setDatGidView();
        }
        private void frm_ChiTietBaoHanh_KH_Load(object sender, EventArgs e)
        {
            lb_dateNow.Text = getDatetimeNow();
            lb_nameEmp.Text = getNameEmp("1");


            //Enable panels
            panel3.Enabled = false;
            //dataGridView1.Enabled = false;
            //Set SDT Customer when load 
            txtPhoneNumberCus.Text = "0337122712";
            //
        }

        public string getDatetimeNow()
        {
            DateTime time = DateTime.Now;
            string date = time.Day + "/" + time.Month + "/" + time.Year;
            return date;
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            lb_timeNow.Text = DateTime.Now.ToString("T");
        }
        public string getNameEmp(string ID)
        {
            string name;
            try
            {
                string query = "SELECT * FROM NHANVIEN WHERE MANV = '" + ID + "'";
                con.Open();
                cmd = new SqlCommand(query, con);
                SqlDataReader dr = cmd.ExecuteReader();

                if(!dr.HasRows)
                {
                    name = "KHÔNG XÁC ĐỊNH";
                }
                name = dr["HOTEN"].ToString();
                dr.Close();
                con.Close();
            }
            catch
            {
                name = "KHÔNG XÁC ĐỊNH";
                con.Close();
            }
            return name;
        }

        private void btnWarrantyCus_Click(object sender, EventArgs e)
        {
            try
            {
                string query = "SELECT * FROM KHACHHANG WHERE SODIENTHOAI = '" + txtPhoneNumberCus.Text + "'";
                con.Open();
                cmd = new SqlCommand(query, con);
                SqlDataReader dr = cmd.ExecuteReader();


                if(dr.HasRows)
                {
                    // Hiển thị thông tin panels
                    panel3.Enabled = true;
                    showWarrantyCus();
                }
                else
                {
                    MessageBox.Show("Không tìm thấy khách hàng", "Chú ý", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPhoneNumberCus.Clear();
                    txtPhoneNumberCus.Focus();
                }
                dr.Close();
                con.Close();
            }
            catch
            {
                MessageBox.Show("Xảy ra lỗi trong quá trình tìm kiếm thông tin khách hàng", "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPhoneNumberCus.Clear();
                txtPhoneNumberCus.Focus();
                con.Close();
            }
        }
        public void showWarrantyCus()
        {
            string ID_KH = "1";
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
            string query = "SELECT CHITIETBAOHANH.MACHITIET, ID_SP, TENSP, GIA, ID_HD, SOLANBAOHANH FROM BAOHANH, CHITIETBAOHANH, SANPHAM" +
                " WHERE BAOHANH.MABAOHANH = CHITIETBAOHANH.MABAOHANH" +
                " AND SANPHAM.MASP = CHITIETBAOHANH.ID_SP" +
                " AND CHITIETBAOHANH.TRANGTHAI = 0" +
                " AND BAOHANH.ID_KH = 1";
            try
            {
                con.Close();
                con.Open();
                cmd = new SqlCommand(query, con);
                SqlDataReader dr = cmd.ExecuteReader();
                string[] rows = new string[] { };
                while(dr.Read())
                {
                    rows = new string[] { dr["MACHITIET"].ToString(), dr["TENSP"].ToString(), dr["ID_HD"].ToString(), dr["SOLANBAOHANH"].ToString() };
                    dataGridView1.Rows.Add(rows);
                }
                dr.Close();
                con.Close();
            }
            catch
            {
                MessageBox.Show("ERROR", "THÔNG BÁO");
            }
        }
        public void setDatGidView()
        {
            dataGridView1.ColumnCount = 4;
            dataGridView1.Columns[0].Name = "Mã chi tiết";
            dataGridView1.Columns[1].Name = "Tên sản phẩm";
            dataGridView1.Columns[2].Name = "Hóa đơn thanh toán";
            dataGridView1.Columns[3].Name = "Số lần bảo hành";

            DataGridViewButtonColumn btn = new DataGridViewButtonColumn();
            dataGridView1.Columns.Add(btn);
            btn.HeaderText = "Chức năng";
            btn.Text = "Bảo hành";
            btn.Name = "btnWannrecy";
            btn.UseColumnTextForButtonValue = true;
        }
        void btn_click(object sender, EventArgs e)
        {
            
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.CurrentRow.Selected = true;
            // Ignore clicks that are not in our 
           if(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
            {
                txtID.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                txtName.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                txtSLBH.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
                txtDateReceived.Text = getDatetimeNow();
                txtSLBH.Text = "1";
                //txtName.Text = dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();
                //txtName.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            }
        }

        private void btnWarrency_Click(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                string query = "SET DATEFORMAT DMY UPDATE CHITIETBAOHANH " +
                    "SET MOTA = N'" + txtDescriptWanrrency.Text + "', THOIGIANBATDAU = '" + txtDateReceived.Text + "', THOIGIANBAOHANHDUKIEN = '" + txtWarrencyEnd.Text + "', TRANGTHAI = '" + 1 + "' " +
                    "WHERE MACHITIET = '" + txtID.Text + "' ";
                cmd = new SqlCommand(query, con);
                cmd.ExecuteNonQuery();
                con.Close();
                waitForm.Show(this);
                Thread.Sleep(5000);
                //Form2 frm = new Form2();
                //frm.Show();
                waitForm.Close();
                try
                {
                    SmtpClient client = new SmtpClient("smtp.gmail.com", 25);   //smtp.gmail.com // For Gmail
                                                                                //smtp.live.com // Windows live / Hotmail
                                                                                //smtp.mail.yahoo.com // Yahoo
                                                                                //smtp.aim.com // AIM
                                                                                //my.inbox.com // Inbox


                    //Authentication.
                    //This is where the valid email account comes into play. You must have a valid email account(with password) to give our program a place to send the mail from.

                    NetworkCredential cred = new NetworkCredential("hientranvan27@gmail.com", "VanHien2001");

                    //To send an email we must first create a new mailMessage(an email) to send.
                    MailMessage Msg = new MailMessage();

                    // Sender e-mail address.
                    Msg.From = new MailAddress("hientranvan27@gmail.com");//Nothing But Above Credentials or your credentials (*******@gmail.com)

                    // Recipient e-mail address.

                    Msg.To.Add(txtEmail.Text);

                    // Assign the subject of our message.
                    Msg.Subject = "THÔNG BÁO BẢO HÀNH SẢN PHẨM";

                    // Create the content(body) of our message.
                    string html =
                        "<h2>CHỨNG TỪ BẢO HÀNH SẢN PHẨM</h2>" +
                        "<span>Khách hàng: Trần Văn Hiền</span>" +
                        "<table>" +
                            "<tr>" +
                                "<th>Tên sản phẩm</th>" +
                                "<th>Ngày bảo hành</th>" +
                                "<th>Thời gian bảo hành</th>" +
                                "<th>Mô tả lỗi</th>" +
                                "<th>Nhân viên tiếp nhận</th>" +
                            "</tr>" +
                            "<tr>" +
                                "<td>" + txtName.Text + "</td>" +
                                "<td>" + txtWarrencyEnd.Text + "</td>" +
                                "<td>" + txtSLBH.Text + "</td>" +
                                "<td>" + txtDescriptWanrrency.Text + "</td>" +
                                "<td>Trần Văn Hiền</td>" +
                            "</tr>" +
                        "</table>";
                        //"<h1>Thông tin liên hệ</h1>" +
                        //"<p>Trần Văn Hiền</p>" +
                        //"<p>033.712.2712</p>" +
                        //"<p>Công ty thương mai phụ kiện điện tử</p>" +
                        //"<p>140 Lê Trọng Tấn, phường Tây Thạnh, quận Tân Phú</p>";
                    Msg.Body = html;
                    Msg.BodyEncoding = System.Text.Encoding.UTF8;
                    Msg.IsBodyHtml = true;
                    // Send our account login details to the client.
                    client.Credentials = cred;

                    //Enabling SSL(Secure Sockets Layer, encyription) is reqiured by most email providers to send mail
                    client.EnableSsl = true;

                    //Confirmation After Click the Button
                    //label5.Text = "Mail Sended Succesfully";

                    // Send our email.
                    client.Send(Msg);
                    
                    MessageBox.Show("CẬP NHẬT BẢO HÀNH THÀNH CÔNG, YÊU CẦU KHÁCH HÀNG KIỂM TRA THÔNG TIN TRÊN E-MAIL", "THÔNG BÁO");
                    showWarrantyCus();
                    txtDescriptWanrrency.Clear();
                    txtID.Clear();
                    txtName.Clear();
                    txtWarrencyEnd.Clear();
                    txtDateReceived.Clear();
                    txtEmail.Clear();
                    dataGridView1.Rows[0].Selected = true;
                    
                }
                catch
                {
                    MessageBox.Show("Không thể gửi Email cho khách hàng", "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                
            }
            catch
            {
                MessageBox.Show("CÓ LỖI KHI TIẾN HÀNH BẢO HÀNH", "THÔNG BÁO LỖI", MessageBoxButtons.OK, MessageBoxIcon.Error);
                con.Close();
            }
        }
        protected void SendEmail(string _subject, MailAddress _from, MailAddress _to, List<MailAddress> _cc, List<MailAddress> _bcc = null)
        {
            string Text = "";
            SmtpClient mailClient = new SmtpClient("Mailhost");
            MailMessage msgMail;
            Text = "Stuff";
            msgMail = new MailMessage();
            msgMail.From = _from;
            msgMail.To.Add(_to);
            foreach (MailAddress addr in _cc)
            {
                msgMail.CC.Add(addr);
            }
            if (_bcc != null)
            {
                foreach (MailAddress addr in _bcc)
                {
                    msgMail.Bcc.Add(addr);
                }
            }
            msgMail.Subject = _subject;
            msgMail.Body = Text;
            msgMail.IsBodyHtml = true;
            mailClient.Send(msgMail);
            msgMail.Dispose();
        }
    } 
}

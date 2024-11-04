using QLCF.NhanVienForm;
using QLCF.NhanVienForm.user_SanPham;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace QLCF
{
    public partial class DangNhap : Form
    {
        private SqlConnection sqlConnection = null;// đối tượng kết nối
        string connectionString = LoadDatabase.connectionString;

        public DangNhap()
        {
            InitializeComponent();
        }

        private void DangNhap_Load(object sender, EventArgs e)
        {
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có muốn thoát chương trình không? ", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

            // Kiểm tra nút mà người dùng đã nhấn.
            if (result == DialogResult.OK)
            {
                Application.Exit();
            }
            else if (result == DialogResult.Cancel){}
        }

        private void btnDN_Click(object sender, EventArgs e)
        {
            
            string username = txtUser.Text.Trim();
            string password = txtPass.Text; // Không cần Trim ở đây vì mật khẩu có thể có khoảng trắng
            using (sqlConnection = new SqlConnection())
            {
                sqlConnection.ConnectionString = connectionString; // Truyền chuỗi kết nối
                sqlConnection.Open(); // Mở kết nối

                SqlCommand sqlcmd = new SqlCommand();
                sqlcmd.CommandType = CommandType.Text;
                sqlcmd.CommandText = "select * from QL_Users Where MaNhanVien='" + username + "'AND MatKhau='" + password + "'";
                sqlcmd.Connection = sqlConnection;
                SqlDataReader data = sqlcmd.ExecuteReader();
                data.Read();

                if (data.HasRows == true)
                {
                    if (data[3].ToString() == "QL")
                    {
                       
                        progressBar1.Visible = true;
                        StartProcessing(2);

                        TruyenData.Instance._LoginTK = username;
                        MainQuanLy mainQuanLy = new MainQuanLy();
                        mainQuanLy.ShowDialog();
                        this.Hide();
                    }
                    else if (data[3].ToString() == "NV")
                    {
                        progressBar1.Visible = true;
                        StartProcessing(1);

                        TruyenData.Instance._LoginTK = username;
                        FormSellNhanVien formSellNhanVien = new FormSellNhanVien();
                        formSellNhanVien.ShowDialog();
                        this.Hide();
                    }
                }
                else
                {
                    MessageBox.Show("Đăng nhập không thành công");
                    this.txtUser.Clear();
                    this.txtPass.Clear();
                    this.txtUser.Focus();
                }
                //Đóng reader
                data.Close();
            }
        }

        private void LoadDataFormQuanLy(object sender, EventArgs e)
        {
            MainQuanLy.instanceMainQuanLy.MainQuanLy_Load(sender, e);
           
            HoaDon.instanceHoaDon.HoaDon_Load(sender, e);

            SanPham.instanceTaiKhoan.SanPham_Load(sender, e);

            TaiKhoan.instanceTaiKhoan.TaiKhoan_Load(sender, e);
           
        }
        private void LoadDataFormNhanVien(object sender, EventArgs e)
        {
            FormSellNhanVien.instanceFormSellNhanVien.FormSellNhanVien_Load(sender, e);
            User_Sell.instanceUSell.User_Sell_Load(sender, e);
            User_Donban.instanceDonBan.User_Donban_Load(sender, e);
            User_DatBan.instanceDatBan.User_DatBan_Load(sender, e);
        }



        private void StartProcessing(int fromcanload)
        {
            // công việc cần thực hiện mà mất thời gian
            int totalTasks = 100; // Số lượng công việc cần thực hiện
            progressBar1.Maximum = totalTasks;

            for (int i = 0; i < totalTasks; i++)
            {
                // Thực hiện công việc tại đây
                System.Threading.Thread.Sleep(5);

                if (fromcanload == 1)
                {
                    btnDN.Click += LoadDataFormNhanVien;
                }
                else
                {
                    btnDN.Click += LoadDataFormQuanLy;
                }



                // Cập nhật giá trị của ProgressBar
                UpdateProgressBar(i+1);
            }

        }




        private void UpdateProgressBar(int value)
        {
            // Đảm bảo rằng chúng ta đang thao tác trên luồng UI chính
            if (InvokeRequired)
            {
                // Nếu không phải luồng UI chính, thực hiện một phương thức đệ quy để đảm bảo rằng nó được gọi trên luồng UI chính
                Invoke(new Action<int>(UpdateProgressBar), value);
            }
            else
            {
                // Cập nhật giá trị của ProgressBar
                progressBar1.Value = value;
            }
        }

        private void btnclose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnmin_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
    }
}

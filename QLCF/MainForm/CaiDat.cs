using QLCF.NhanVienForm.user_SanPham;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLCF
{
    public partial class CaiDat : UserControl
    {
        private SqlConnection sqlConnection = null;// đối tượng kết nối
        //string connectionString = "Data Source=DESKTOP-39BMSC8\\GIAKHANH;Initial Catalog=QL_QuanCF;Integrated Security=True";
        string connectionString = LoadDatabase.connectionString;

        public EventHandler LogoutClicked;

        public CaiDat()
        {
            InitializeComponent();
        }

        private void CaiDat_Load(object sender, EventArgs e)
        {
            txtMaQL.Text = TruyenData.Instance._LoginTK;
            LoadDataNhanvien();
        }

        //respontive form
        public void responsive_CaiDat(int newWidthForm)
        {

            if (newWidthForm == 1920)
            {
                flowLayoutPanel_containTitle.Size = new Size(1389+310, 61);
                pnlReponsiveCaiDat.Size = new Size(927 + 310, 22);
                //pnlContainTableCaiDat.Size = new Size(1389 + 300, 589 + 305);
            }
            else if (newWidthForm == 1615)
            {
                flowLayoutPanel_containTitle.Size = new Size(1389, 61);
                pnlReponsiveCaiDat.Size = new Size(927, 28);
                //pnlContainTableCaiDat.Size = new Size(1389, 632);

            }
        }

        private void btnDangXuat_Click(object sender, EventArgs e)
        {
            LogoutClicked?.Invoke(this, EventArgs.Empty);
        }

        private void btnDieuChinhThongTin_Click(object sender, EventArgs e)
        {
            txtMaQL.ReadOnly = false;
            txtMaQL.BorderStyle = BorderStyle.Fixed3D;
            txtMaQL.BackColor = Color.Bisque;
        }

        private void btnLuuThayDoi_Click(object sender, EventArgs e)
        {
            txtMaQL.ReadOnly = true;
            txtMaQL.BorderStyle = BorderStyle.None;
            txtMaQL.BackColor = Color.FromArgb(205, 245, 253); ;
        }

        private void LoadDataNhanvien()
        {
            using (sqlConnection = new SqlConnection())
            {
                sqlConnection.ConnectionString = connectionString; // Truyền chuỗi kết nối
                sqlConnection.Open(); // Mở kết nối

                // Truy vấn SQL để lấy dữ liệu từ bảng
                string query = "SELECT MaNhanVien, TenNhanVien, SoDienThoai, ChucVu, LoaiNhanVien, NgayNhanViec, CaLamViec\r\nFROM QL_NhanVien\r\nWHERE MaNhanVien = '"+ txtMaQL.Text + "';";
                using (SqlCommand command = new SqlCommand(query, sqlConnection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Gán giá trị từ cột vào TextBox tương ứng
                            txtMaQL.Text = reader["MaNhanVien"].ToString();
                            txtTenNhanvien.Text = reader["TenNhanVien"].ToString();
                            txtSDT.Text = reader["SoDienThoai"].ToString();
                            txtChucVu.Text = reader["ChucVu"].ToString();
                            txtLoatNhanvien.Text = reader["LoaiNhanVien"].ToString();
                            txtNgayNhanviec.Text = reader["NgayNhanViec"].ToString();
                            txtCalamViec.Text = reader["CaLamViec"].ToString();
                        }
                    }
                }
            }
        }
    }
}

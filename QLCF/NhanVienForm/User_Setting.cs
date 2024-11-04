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

namespace QLCF.NhanVienForm
{
    public partial class User_Setting : UserControl
    {

        private SqlConnection sqlConnection = null;// đối tượng kết nối
        //string connectionString = "Data Source=DESKTOP-39BMSC8\\GIAKHANH;Initial Catalog=QL_QuanCF;Integrated Security=True";
        string connectionString = LoadDatabase.connectionString;

        // Tạo một sự kiện để thông báo về việc đăng xuất
        public event EventHandler LogoutClicked;

        public User_Setting()
        {
            InitializeComponent();

            
            
        }

        private void User_Setting_Load(object sender, EventArgs e)
        {
            lbMaNV.Text = TruyenData.Instance._LoginTK;
            Console.WriteLine(TruyenData.Instance._LoginTK);
            LoadDataNhanvien();
        }

        public void btnDangXuat_Click(object sender, EventArgs e)
        {
            // Khi nút đăng xuất được click, kích hoạt sự kiện LogoutClicked
            LogoutClicked?.Invoke(this, EventArgs.Empty);
        }


        //respontive form
        public void responsive_Setting(int newWidthForm)
        {

            if (newWidthForm == 1920)
            {
               
            }
            else //if (newWidthForm == 1615)
            {
                

            }

        }


        private void LoadDataNhanvien()
        {
            using (sqlConnection = new SqlConnection())
            {
                sqlConnection.ConnectionString = connectionString; // Truyền chuỗi kết nối
                sqlConnection.Open(); // Mở kết nối

                // Truy vấn SQL để lấy dữ liệu từ bảng
                string query = "SELECT MaNhanVien, TenNhanVien, SoDienThoai, ChucVu, LoaiNhanVien, NgayNhanViec, CaLamViec\r\nFROM QL_NhanVien\r\nWHERE MaNhanVien = '" + lbMaNV.Text + "';";
                using (SqlCommand command = new SqlCommand(query, sqlConnection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Gán giá trị từ cột vào TextBox tương ứng
                            lbMaNV.Text = reader["MaNhanVien"].ToString();
                            TenNhanVien.Text = reader["TenNhanVien"].ToString();
                            lbSoDienThoai.Text = reader["SoDienThoai"].ToString();
                            lbchucvu.Text = reader["ChucVu"].ToString();
                            LoaiNhanVien.Text = reader["LoaiNhanVien"].ToString();
                            NgayNhanViec.Text = reader["NgayNhanViec"].ToString();
                            CaLamViec.Text = reader["CaLamViec"].ToString();
                        }
                    }
                }
            }
        }

    }
}

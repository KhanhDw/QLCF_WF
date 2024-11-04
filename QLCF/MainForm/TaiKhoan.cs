using Microsoft.SqlServer.Server;
//using QLCF.MainForm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace QLCF
{
    public partial class TaiKhoan : UserControl
    {
        // Gọi instance của Singleton từ một lớp khác
        private SqlConnection sqlConnection = null;// đối tượng kết nối
        string connectionString = LoadDatabase.connectionString;
        //string connectionString = "Data Source=DESKTOP-39BMSC8\\GIAKHANH;Initial Catalog=QL_QuanCF;Integrated Security=True";



        public static TaiKhoan instanceTaiKhoan;

        public TaiKhoan()
        {
            InitializeComponent();
            instanceTaiKhoan = this;
        }

        public void TaiKhoan_Load(object sender, EventArgs e)
        {
            // Thực hiện kết nối SQL Server khi form được mở
            LoadDataSQL();
            ThemDuLieuVaoDataGridView();
            cbbQuyenNV.SelectedIndex = 0;
        }

        private void ThemDuLieuVaoDataGridView()
        {
            dataGridView_TaiKhoan.ScrollBars = ScrollBars.Vertical; // Ẩn thanh cuộn ngan

            // Điều chỉnh độ rộng của cột tự động lấp đầy bảng
            dataGridView_TaiKhoan.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Điều chỉnh chiều cao của dòng header tự động
            dataGridView_TaiKhoan.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            // Chỉnh định dạng cho font của header
            dataGridView_TaiKhoan.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 12, FontStyle.Bold);

            // Chỉnh định dạng cho dữ liệu DataGridView
            dataGridView_TaiKhoan.DefaultCellStyle.Font = new Font("Tahoma", 13, FontStyle.Regular);

            //Tắt viền cho toàn bộ DataGridView
            //dataGridView_TaiKhoan.CellBorderStyle = DataGridViewCellBorderStyle.None;

            dataGridView_TaiKhoan.AllowUserToAddRows = false;
        }


        public void responsive_TaiKhoan(int newWidthForm)
        {

            if (newWidthForm == 1920)
            {
                pnlReponsiveTaiKhoan.Size = new Size(1395 + 310, 71);
                pnlContainTableTaiKhoan.Size = new Size(1389 + 300, 589 + 305);
            }
            else if (newWidthForm == 1615)
            {
                pnlReponsiveTaiKhoan.Size = new Size(1395, 71);
                pnlContainTableTaiKhoan.Size = new Size(1389, 632);

            }

        }


        private void btnThemTaiKhoan_Click(object sender, EventArgs e)
        {
            if (txtma.Text == "" || txtmatkhau.Text == "" || txtnhaplaimk.Text == "")
            {
                MessageBox.Show("vui lòng nhập đầy đủ thông tin", "thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
            else if (txtmatkhau.Text != txtnhaplaimk.Text)
            {

                MessageBox.Show("Mật khẩu không trùng khớp!!", "thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
            else if (cbbQuyenNV.SelectedIndex == 0)
            {
                MessageBox.Show("Vui lòng chọn!!", "thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                string[] row = new string[] { txtma.Text, txtmatkhau.Text, txtnhaplaimk.Text };

                string strQuyen = "";

                if (cbbQuyenNV.SelectedIndex == 1)
                {
                    strQuyen = "QL";
                }
                else if (cbbQuyenNV.SelectedIndex == 2)
                {
                    strQuyen = "NV";
                }


                // Tạo đối tượng SqlConnection
                using (sqlConnection = new SqlConnection(connectionString))
                {
                    // Mở kết nối
                    sqlConnection.Open();
                    // Tạo câu lệnh SQL để thêm dữ liệu cho cơ sở dữ liệu
                    string insertQuery = "INSERT INTO QL_Users (TaiKhoan, TenNhanVien, MatKhau,Quyen) " +
                        "VALUES (@TaiKhoan, @TenNhanVien,@MatKhau, @Quyen)";

                    // khởi tạo đối tượng thực thi
                    using (SqlCommand cmd = new SqlCommand(insertQuery, sqlConnection))
                    {
                        // truyền dữ liệu vào sql
                        cmd.Parameters.AddWithValue("@TaiKhoan", txtma.Text);
                        cmd.Parameters.AddWithValue("@TenNhanVien", txtTenNhanvien.Text);
                        cmd.Parameters.AddWithValue("@MatKhau", txtmatkhau.Text);
                        cmd.Parameters.AddWithValue("@Quyen", strQuyen);

                        // thực thi các lệnh thêm
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            txtma.Clear();
            txtTenNhanvien.Clear();
            txtmatkhau.Clear();
            txtnhaplaimk.Clear();
            cbbQuyenNV.SelectedIndex = 0;
            txtma.Focus();

            LoadDataSQL();
        }

        private void btnXoaTaiKhoan_Click(object sender, EventArgs e)
        {
            try
            {
                // Tạo đối tượng SqlConnection
                using (sqlConnection = new SqlConnection(connectionString))
                {
                    // Mở kết nối
                    sqlConnection.Open();
                    // Tạo câu lệnh SQL để xóa dòng đang chọn tương ứng trong cơ sở dữ liệu
                    string deleteQuery = "DELETE FROM QL_Users " +
                        "WHERE TaiKhoan = '" + (dataGridView_TaiKhoan.SelectedRows[0].Cells[0].Value).ToString() + "'";

                    using (SqlCommand command = new SqlCommand(deleteQuery, sqlConnection))
                    {
                        // Thực thi câu lệnh SQL
                        int rowsAffected = command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có
                MessageBox.Show("Error: " + ex.Message);
            }



            txtma.Clear();
            txtTenNhanvien.Clear();
            txtmatkhau.Clear();
            txtnhaplaimk.Clear();
            cbbQuyenNV.SelectedIndex = 0;
            txtma.Focus();

            LoadDataSQL();
        }

        private void btnSuaTaiKhoan_Click(object sender, EventArgs e)
        {
            if (dataGridView_TaiKhoan.SelectedRows.Count > 0)
            {
                if (txtma.Text == "" || txtmatkhau.Text == "" || txtnhaplaimk.Text == "")
                {
                    MessageBox.Show("vui lòng nhập đầy đủ thông tin", "thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
                else if (txtmatkhau.Text != txtnhaplaimk.Text)
                {

                    MessageBox.Show("Mật khẩu không trùng khớp!!", "thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
                else if (cbbQuyenNV.SelectedIndex == 0)
                {
                    MessageBox.Show("Vui lòng chọn!!", "thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {


                    string strQuyen = "";
                    if (cbbQuyenNV.SelectedIndex == 1)
                    {
                        strQuyen = "QL";
                    }
                    else if (cbbQuyenNV.SelectedIndex == 2)
                    {
                        strQuyen = "NV";
                    }



                    // Tạo đối tượng SqlConnection
                    using (sqlConnection = new SqlConnection(connectionString))
                    {
                        // Mở kết nối
                        sqlConnection.Open();

                        string deleteQuery = "UPDATE QL_Users SET TenNhanVien = N'" + txtTenNhanvien.Text + "', " +
                            "MatKhau = N'" + txtmatkhau.Text + "', Quyen = '" + strQuyen + "' WHERE TaiKhoan = '" + txtma.Text + "';";

                        using (SqlCommand command = new SqlCommand(deleteQuery, sqlConnection))
                        {
                            // Thực thi câu lệnh SQL
                            int rowsAffected = command.ExecuteNonQuery();
                        }
                    }
                }
            }




            txtma.Clear();
            txtTenNhanvien.Clear();
            txtmatkhau.Clear();
            txtnhaplaimk.Clear();
            cbbQuyenNV.SelectedIndex = 0;
            txtma.Focus();

            LoadDataSQL();
        }

        private void dataGridView_TaiKhoan_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // Lấy dữ liệu từ dòng được chọn
                DataGridViewRow row = dataGridView_TaiKhoan.Rows[e.RowIndex];

                // Hiển thị dữ liệu từ dòng vào các TextBox
                txtma.Text = row.Cells[1].Value.ToString();
                txtTenNhanvien.Text = row.Cells[2].Value.ToString();
                txtmatkhau.Text = row.Cells[3].Value.ToString();
                txtnhaplaimk.Text = row.Cells[3].Value.ToString();

                if (cbbQuyenNV.Items.Contains(row.Cells[4].Value.ToString()))
                {
                    cbbQuyenNV.SelectedItem = row.Cells[4].Value.ToString();
                }

                if (row.Cells[4].Value.ToString() == "QL")
                {
                    cbbQuyenNV.SelectedIndex = 1;
                }
                else if (row.Cells[4].Value.ToString() == "NV")
                {
                    cbbQuyenNV.SelectedIndex = 2;
                }
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            // Lọc dữ liệu trực tiếp trên DataGridView
            string searchText = txtSearch.Text;

            foreach (DataGridViewRow row in dataGridView_TaiKhoan.Rows)
            {
                bool rowVisible = false;

                foreach (DataGridViewCell cell in row.Cells)
                {
                    // Nếu một trong các ô chứa dữ liệu giống với nội dung tìm kiếm
                    if (cell.Value != null && cell.Value.ToString().ToLower().Contains(searchText))
                    {
                        rowVisible = true;
                        break;
                    }
                }

                // fix lỗi hiển thị ẩn dòng trong datagridview khi truyền sql vào
                CurrencyManager currencyManager1 = (CurrencyManager)BindingContext[dataGridView_TaiKhoan.DataSource];
                currencyManager1.SuspendBinding();
                row.Visible = false;
                currencyManager1.ResumeBinding();


                // Hiển thị hoặc ẩn dòng tùy thuộc vào kết quả tìm kiếm
                row.Visible = rowVisible;
            }
        }

        private void LoadDataSQL()
        {
            using (sqlConnection = new SqlConnection())
            {
                sqlConnection.ConnectionString = connectionString; // Truyền chuỗi kết nối
                sqlConnection.Open(); // Mở kết nối

                // Truy vấn SQL để lấy dữ liệu từ bảng
                string query = "SELECT QL_Users.IdUser, QL_Users.MaNhanVien,QL_NhanVien.TenNhanVien,QL_Users.MatKhau, QL_Users.Quyen\r\nFROM QL_Users, QL_NhanVien\r\nWhere QL_Users.MaNhanVien = QL_NhanVien.MaNhanVien;";
                using (SqlCommand command = new SqlCommand(query, sqlConnection))
                {
                    // Tạo đối tượng SqlDataAdapter để lấy dữ liệu từ SQL Server
                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter(command))
                    {
                        // Tạo đối tượng DataTable để lưu trữ dữ liệu
                        DataTable dataTable = new DataTable();

                        // Đổ dữ liệu từ SqlDataAdapter vào DataTable
                        dataAdapter.Fill(dataTable);

                        // Gán DataTable làm nguồn dữ liệu cho DataGridView
                        dataGridView_TaiKhoan.DataSource = dataTable;

                        dataGridView_TaiKhoan.Columns["IdUser"].HeaderText = "ID";
                        dataGridView_TaiKhoan.Columns["MaNhanVien"].HeaderText = "Mã Nhân Viên";
                        dataGridView_TaiKhoan.Columns["TenNhanVien"].HeaderText = "Tên Nhân Viên";
                        dataGridView_TaiKhoan.Columns["MatKhau"].HeaderText = "Mật khẩu";
                        dataGridView_TaiKhoan.Columns["Quyen"].HeaderText = "Chức vụ";
                    }
                }
            }
        }
    }
}

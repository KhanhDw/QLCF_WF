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
    public partial class KhachHang : UserControl
    {

        private SqlConnection sqlConnection = null;// đối tượng kết nối
        //string connectionString = "Data Source=DESKTOP-39BMSC8\\GIAKHANH;Initial Catalog=QL_QuanCF;Integrated Security=True";
        string connectionString = LoadDatabase.connectionString;

        public KhachHang()
        {
            InitializeComponent();
            
        }

        private void KhachHang_Load(object sender, EventArgs e)
        {
            LoadDataSQL();
            ThemDuLieuVaoDataGridView();
            AutoClickButton();
        }

        private void ThemDuLieuVaoDataGridView()
        {
            dataGridView_KhachHang.ScrollBars = ScrollBars.Vertical; // Ẩn thanh cuộn ngan

            // Điều chỉnh độ rộng của cột tự động lấp đầy bảng
            dataGridView_KhachHang.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Điều chỉnh chiều cao của dòng header tự động
            dataGridView_KhachHang.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            // Chỉnh định dạng cho font của header
            dataGridView_KhachHang.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 12, FontStyle.Bold);

            // Chỉnh định dạng cho dữ liệu DataGridView
            dataGridView_KhachHang.DefaultCellStyle.Font = new Font("Tahoma", 13, FontStyle.Regular);


            // Tắt viền cho toàn bộ DataGridView
            dataGridView_KhachHang.CellBorderStyle = DataGridViewCellBorderStyle.None;

            dataGridView_KhachHang.AllowUserToAddRows = false;
        }

        //respontive form
        public void responsive_KhachHang(int newWidthForm)
        {

            if (newWidthForm == 1920)
            {
                pnlReponsiveKhachHang.Size = new Size(1395 + 310, 114);
                pnlContainTableKhachHang.Size = new Size(1389 + 300, 589 + 305);
            }
            else //if (newWidthForm == 1615)
            {
                pnlReponsiveKhachHang.Size = new Size(1395, 114);
                pnlContainTableKhachHang.Size = new Size(1389, 589);

            }

        }

        private void SearchInDataGridView(string searchText)
        {
            foreach (DataGridViewRow row in dataGridView_KhachHang.Rows)
            {
                row.Selected = false;
            }

            foreach (DataGridViewRow row in dataGridView_KhachHang.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    if (cell.Value != null && cell.Value.ToString().IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        // Tìm thấy chuỗi tìm kiếm trong ô
                        row.Selected = true;
                        break;
                    }
                }
            }
        }

        private void btnHomNay_Click(object sender, EventArgs e)
        {
            // Lấy ngày và giờ hiện tại
            DateTime currentDate = DateTime.Now;
            //lbDateBox.Text = currentDate.ToString("dd/MM/yyyy");
            //string yearSearch = lbDateBox.Text;
            // Lấy 4 ký tự ở cuối chuỗi
           // string getYear4Char = yearSearch.Substring(yearSearch.Length - 4);
            lb_NgayBatDau.Text = currentDate.ToString("dd/MM/yyyy");
            SearchInDataGridView(lb_NgayBatDau.Text);
        }

        private void btnNextDate_Click(object sender, EventArgs e)
        {
            // Lấy giá trị hiện tại từ DateTimePicker
            DateTime currentDate = dateTimePicker_NgayBatDau.Value;

            // Tăng giá trị ngày lên 1 ngày để chuyển đến ngày tiếp theo
            DateTime nextDate = currentDate.AddDays(1);

            // Thiết lập giá trị mới cho DateTimePicker
            dateTimePicker_NgayBatDau.Value = nextDate;

            //thay đổi giá trị hiển thị
            lb_NgayBatDau.Text = nextDate.ToString("dd/MM/yyyy");
        }

        private void btnLastDate_Click(object sender, EventArgs e)
        {
            // Lấy giá trị hiện tại từ DateTimePicker
            DateTime currentDate = dateTimePicker_NgayBatDau.Value;

            // Tăng giá trị ngày lên 1 ngày để chuyển đến ngày tiếp theo
            DateTime nextDate = currentDate.AddDays(-1);

            // Thiết lập giá trị mới cho DateTimePicker
            dateTimePicker_NgayBatDau.Value = nextDate;

            //thay đổi giá trị hiển thị
            lb_NgayBatDau.Text = nextDate.ToString("dd/MM/yyyy");
        }

        

        private void btnXoaKhachHang_Click(object sender, EventArgs e)
        {
            try
            {
                // Tạo đối tượng SqlConnection
                using (sqlConnection = new SqlConnection(connectionString))
                {
                    // Mở kết nối
                    sqlConnection.Open();
                    // Tạo câu lệnh SQL để xóa dòng tương ứng trong cơ sở dữ liệu
                    string deleteQuery = "DELETE FROM QL_KhachHang WHERE IDKhachHang = '" + (dataGridView_KhachHang.SelectedRows[0].Cells[0].Value).ToString() + "'";

                    using (SqlCommand command = new SqlCommand(deleteQuery, sqlConnection))
                    {
                        // Thực thi câu lệnh SQL
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            dataGridView_KhachHang.Rows.RemoveAt(dataGridView_KhachHang.SelectedRows[0].Index);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có
                MessageBox.Show("Error: " + ex.Message);
            }


            //Console.WriteLine("Heleo "+(dataGridView_TaiKhoan.SelectedRows[0].Cells[0].Value).ToString());

            txtID.Clear();
            txtHoTen.Clear();
            txtSDT.Clear();
            txtID.Focus();

            LoadDataSQL();
        }

        private void btnSuaKhachHang_Click(object sender, EventArgs e)
        {
            using (sqlConnection = new SqlConnection(connectionString))
            {
                // Mở kết nối
                sqlConnection.Open();
                // Tạo câu lệnh SQL để xóa dòng tương ứng trong cơ sở dữ liệu
                //string deleteQuery = "DELETE FROM QL_Users WHERE TaiKhoan = '" + (dataGridView_TaiKhoan.SelectedRows[0].Cells[0].Value).ToString() + "'";

                //dataGridView_TaiKhoan.SelectedRows[0].Cells[0].Value = strQuyen;

                string deleteQuery = "UPDATE QL_KhachHang SET IDKhachHang = N'" + txtID.Text + "', MaKhachHang = N'" + txtMaKH.Text + "', TenKhachHang = '" + txtHoTen.Text + "',SoDienThoai='" + txtSDT.Text + "' WHERE IDKhachHang = '" + txtID.Text + "';";

                using (SqlCommand command = new SqlCommand(deleteQuery, sqlConnection))
                {
                    // Thực thi câu lệnh SQL
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        dataGridView_KhachHang.Rows.RemoveAt(dataGridView_KhachHang.SelectedRows[0].Index);
                    }

                }

            }

            txtID.Clear();
            txtHoTen.Clear();
            txtMaKH.Clear();
            txtSDT.Clear();

            txtID.Focus();

            LoadDataSQL();

        }

        private void btnThemKhachHang_Click(object sender, EventArgs e)
        {
            if (txtID.Text == "" || txtMaKH.Text == "" || txtHoTen.Text == "" || txtSDT.Text == "")
            {
                MessageBox.Show("vui lòng nhập đầy đủ thông tin", "thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                // Tạo đối tượng SqlConnection
                using (sqlConnection = new SqlConnection(connectionString))
                {
                    // Mở kết nối
                    sqlConnection.Open();
                    // Tạo câu lệnh SQL để xóa dòng tương ứng trong cơ sở dữ liệu
                    string insertQuery = "INSERT INTO QL_KhachHang (IDKhachHang,MaKhachHang,TenKhachHang,SoDienThoai) VALUES (@IDKhachHang,@MaKhachHang,@TenKhachHang,@SoDienThoai)";

                    using (SqlCommand cmd = new SqlCommand(insertQuery, sqlConnection))
                    {
                        cmd.Parameters.AddWithValue("@IDKhachHang", txtID.Text);
                        cmd.Parameters.AddWithValue("@MaKhachHang", txtMaKH.Text);
                        cmd.Parameters.AddWithValue("@TenKhachHang", txtHoTen.Text);
                        cmd.Parameters.AddWithValue("@SoDienThoai", txtSDT.Text);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            txtID.Clear();
            txtMaKH.Clear();
            txtHoTen.Clear();
            txtSDT.Clear();

            txtID.Focus();

            LoadDataSQL();
        }

        private void AutoClickButton()
        {
            // Gọi phương thức xử lý sự kiện của Button
            btnHomNay_Click(this, EventArgs.Empty);
            DateTime currentDate = DateTime.Now;
            lb_NgayBatDau.Text = currentDate.ToString("dd/MM/yyyy");
        }

        private void LoadDataSQL()
        {
            using (sqlConnection = new SqlConnection())
            {
                sqlConnection.ConnectionString = connectionString; // Truyền chuỗi kết nối
                sqlConnection.Open(); // Mở kết nối

                // Truy vấn SQL để lấy dữ liệu từ bảng
                string query = "SELECT * FROM QL_KhachHang";
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
                        dataGridView_KhachHang.DataSource = dataTable;
                        dataGridView_KhachHang.Columns["IDKhachHang"].HeaderText = "ID khách hàng";
                        dataGridView_KhachHang.Columns["MaKhachHang"].HeaderText = "Mã khách hàng";
                        dataGridView_KhachHang.Columns["TenKhachHang"].HeaderText = "Tên khách hàng";
                        dataGridView_KhachHang.Columns["SoDienThoai"].HeaderText = "Số điện thoại";
                    }
                }
            }
        }

        private void dataGridView_KhachHang_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // Lấy dữ liệu từ dòng được chọn
                DataGridViewRow row = dataGridView_KhachHang.Rows[e.RowIndex];

                // Hiển thị dữ liệu từ dòng vào các TextBox
                txtID.Text = row.Cells["IDKhachHang"].Value.ToString();
                txtMaKH.Text = row.Cells["MaKhachHang"].Value.ToString();
                txtHoTen.Text = row.Cells["TenKhachHang"].Value.ToString();
                txtSDT.Text = row.Cells["SoDienThoai"].Value.ToString();
            }
        }

        private void txtSearch_TextChanged_1(object sender, EventArgs e)
        {
            // Lọc dữ liệu trực tiếp trên DataGridView
            string searchText = txtSearch.Text;

            foreach (DataGridViewRow row in dataGridView_KhachHang.Rows)
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
                CurrencyManager currencyManager1 = (CurrencyManager)BindingContext[dataGridView_KhachHang.DataSource];
                currencyManager1.SuspendBinding();
                row.Visible = false;
                currencyManager1.ResumeBinding();

                // Hiển thị hoặc ẩn dòng tùy thuộc vào kết quả tìm kiếm
                row.Visible = rowVisible;
            }
        }
    }
}

//using QLCF.MainForm;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;

namespace QLCF
{
    public partial class NhanVien : UserControl
    {

        private SqlConnection sqlConnection = null;// đối tượng kết nối
        //string connectionString = "Data Source=DESKTOP-39BMSC8\\GIAKHANH;Initial Catalog=QL_QuanCF;Integrated Security=True";
        string connectionString = LoadDatabase.connectionString;

        public NhanVien()
        {
            InitializeComponent();
        }

        private void NhanVien_Load(object sender, EventArgs e)
        {
            LoadDataSQL();
            ThemDuLieuVaoDataGridView();

        }


        private void ThemDuLieuVaoDataGridView()
        {
            dataGridView_NhanVien.ScrollBars = ScrollBars.Vertical; // Ẩn thanh cuộn ngan

            // Điều chỉnh độ rộng của cột tự động lấp đầy bảng
            dataGridView_NhanVien.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Điều chỉnh chiều cao của dòng header tự động
            dataGridView_NhanVien.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            // Chỉnh định dạng cho font của header
            dataGridView_NhanVien.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 8, FontStyle.Bold);

            // Chỉnh định dạng cho dữ liệu DataGridView
            dataGridView_NhanVien.DefaultCellStyle.Font = new Font("Tahoma", 7, FontStyle.Regular);


            // Tắt viền cho toàn bộ DataGridView
            dataGridView_NhanVien.CellBorderStyle = DataGridViewCellBorderStyle.None;

            dataGridView_NhanVien.AllowUserToAddRows = false;
        }

        private void LoadDataSQL()
        {
            using (sqlConnection = new SqlConnection())
            {
                sqlConnection.ConnectionString = connectionString; // Truyền chuỗi kết nối
                sqlConnection.Open(); // Mở kết nối

                // Truy vấn SQL để lấy dữ liệu từ bảng
                string query = "SELECT * FROM QL_NhanVien";
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
                        dataGridView_NhanVien.DataSource = dataTable;

                        dataGridView_NhanVien.Columns["MaNhanVien"].HeaderText = "Mã Nhân Viên";
                        dataGridView_NhanVien.Columns["TenNhanVien"].HeaderText = "Tên Nhân Viên";
                        dataGridView_NhanVien.Columns["GioiTinh"].HeaderText = "Giới Tính";
                        dataGridView_NhanVien.Columns["NamSinh"].HeaderText = "Nam Sinh";
                        dataGridView_NhanVien.Columns["SoDienThoai"].HeaderText = "Số Điện Thoai";
                        dataGridView_NhanVien.Columns["ChucVu"].HeaderText = "Chức Vụ";
                        dataGridView_NhanVien.Columns["LoaiNhanVien"].HeaderText = "Loại Nhân Viên";
                        dataGridView_NhanVien.Columns["Luong"].HeaderText = "Lương";
                        dataGridView_NhanVien.Columns["NgayNhanViec"].HeaderText = "Ngày Nhận Việc";
                        dataGridView_NhanVien.Columns["CaLamViec"].HeaderText = "Ca Làm Việc";


                    }
                }
            }
        }

        //respontive form
        public void responsive_NhanVien(int newWidthForm)
        {

            if (newWidthForm == 1920)
            {
                pnlReponsiveNhanVien.Size = new Size(1395+310, 71);
                pnlContainTableNhanVien.Size = new Size(1389 + 300, 589 + 305);
            }
            else if (newWidthForm == 1615)
            {
                pnlReponsiveNhanVien.Size = new Size(1395, 71);
                pnlContainTableNhanVien.Size = new Size(1389, 632);

            }

        }

        private void btnThemNhanVien_Click(object sender, EventArgs e)
        {
            if (txtma.Text == "" || txtten.Text == "" || txtdt.Text == "" || txtchucvu.Text == "" || txtloai.Text == "" || txtluong.Text == "" || txtNgayNhanViec.Text == "" || txtcalam.Text == "")
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
                    string insertQuery = "INSERT INTO QL_NhanVien (MaNhanVien,TenNhanVien,GioiTinh,NamSinh,SoDienThoai,ChucVu,LoaiNhanVien,Luong,NgayNhanViec,CaLamViec) VALUES (@MaNhanVien,@TenNhanVien,@GioiTinh,@NamSinh,@SoDienThoai,@ChucVu,@LoaiNhanVien,@Luong,@NgayNhanViec,@CaLamViec)";


                    using (SqlCommand cmd = new SqlCommand(insertQuery, sqlConnection))
                    {
                        cmd.Parameters.AddWithValue("@MaNhanVien", txtma.Text);
                        cmd.Parameters.AddWithValue("@TenNhanVien", txtten.Text);
                        cmd.Parameters.AddWithValue("@GioiTinh", radNam.Checked ? "Nam" : "Nữ");
                        cmd.Parameters.AddWithValue("@NamSinh", datangaysinh.Value.ToString());
                        cmd.Parameters.AddWithValue("@SoDienThoai", txtdt.Text);
                        cmd.Parameters.AddWithValue("@ChucVu", txtchucvu.Text);
                        cmd.Parameters.AddWithValue("@LoaiNhanVien", txtloai.Text);
                        cmd.Parameters.AddWithValue("@Luong", txtluong.Text);
                        cmd.Parameters.AddWithValue("@NgayNhanViec", txtNgayNhanViec.Text);
                        cmd.Parameters.AddWithValue("@CaLamViec", txtNgayNhanViec.Text);


                        cmd.ExecuteNonQuery();
                    }
                }


            }
            txtma.Clear();
            txtten.Clear();
            txtdt.Clear();
            txtchucvu.Clear();
            txtloai.Clear();
            txtluong.Clear();
            txtNgayNhanViec.Clear();
            txtNgayNhanViec.Clear();
            txtma.Focus();
            LoadDataSQL();
        }

        private void btnSuaNhanVien_Click(object sender, EventArgs e)
        {
            // Tạo đối tượng SqlConnection
            using (sqlConnection = new SqlConnection(connectionString))
            {// Tạo câu lệnh SQL để xóa dòng tương ứng trong cơ sở dữ liệu
                //string deleteQuery = "DELETE FROM QL_Users WHERE TaiKhoan = '" + (dataGridView_TaiKhoan.SelectedRows[0].Cells[0].Value).ToString() + "'";

                string Gioitinh = radNam.Checked ? "Nam" : "Nữ";
                string deleteQuery = "UPDATE QL_NhanVien SET MaNhanVien = '" + txtma.Text + "', TenNhanVien = N'" + txtten.Text + "',GioiTinh = '" + Gioitinh + "', NamSinh = '" + datangaysinh.Value.ToString() + "', SoDienThoai = '" + txtdt.Text + "',ChucVu = '" + txtchucvu.Text + "', LoaiNhanVien = N'" + txtloai.Text + "', Luong = '" + txtluong + "',NgayNhanViec = '" + txtNgayNhanViec.Text + "', CaLamViec = '" + txtcalam.Text + "' WHERE MaNhanVien = '" + dataGridView_NhanVien.SelectedRows[0].Cells[0].Value + "';";

                using (SqlCommand command = new SqlCommand(deleteQuery, sqlConnection))
                {
                    // Thực thi câu lệnh SQL
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        dataGridView_NhanVien.Rows.RemoveAt(dataGridView_NhanVien.SelectedRows[0].Index);
                    }

                }
            }

            txtma.Clear();
            txtten.Clear();
            txtdt.Clear();
            txtchucvu.Clear();
            txtloai.Clear();
            txtluong.Clear();
            txtNgayNhanViec.Clear();
            txtNgayNhanViec.Clear();
            txtma.Focus();
            LoadDataSQL();
        }

        private void datNgaySinh_ValueChanged(object sender, EventArgs e)
        {
            DateTime dateNgaySinhSelect = datangaysinh.Value;
            // Định dạng chuỗi theo ý muốn, ở đây là ngày giờ đầy đủ
            string formattedDateTime = dateNgaySinhSelect.ToString("dd/MM/yyyy");
            // Gán chuỗi đã định dạng vào Label
            datangaysinh.Text = formattedDateTime;
        }

        private void dataGridView_NhanVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (e.RowIndex >= 0)
                {
                    // Lấy dữ liệu từ dòng được chọn
                    DataGridViewRow row = dataGridView_NhanVien.Rows[e.RowIndex];

                    txtma.Text = row.Cells[0].Value.ToString();

                    txtten.Text = row.Cells[1].Value.ToString();

                    if (row.Cells[2].Value.ToString() == "Nam")
                    {
                        radNam.Checked = true;
                        radNu.Checked = false;
                        Console.WriteLine(row.Cells[2].Value.ToString());
                    }
                    else
                    {
                        radNu.Checked = true;
                        radNam.Checked = false;
                    }

                    datangaysinh.Text = row.Cells[3].Value.ToString();

                    txtdt.Text = row.Cells[4].Value.ToString();
                    txtchucvu.Text = row.Cells[5].Value.ToString();
                    txtloai.Text = row.Cells[6].Value.ToString();
                    txtluong.Text = row.Cells[7].Value.ToString();
                    txtNgayNhanViec.Text = row.Cells[8].Value.ToString();
                    txtcalam.Text = row.Cells[9].Value.ToString();

                }
            }
        }

        private void btnXoaNhanVien_Click(object sender, EventArgs e)
        {
            DialogResult xacnhan = MessageBox.Show("Bạn có chắc muốn xóa không?", "Cảnh báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (xacnhan == DialogResult.Yes)
            {
                try
                {
                    // Tạo đối tượng SqlConnection
                    using (sqlConnection = new SqlConnection(connectionString))
                    {
                        // Mở kết nối
                        sqlConnection.Open();
                        // Tạo câu lệnh SQL để xóa dòng tương ứng trong cơ sở dữ liệu
                        string deleteQuery = "DELETE FROM QL_KhachHang WHERE IDKhachHang = '" + (dataGridView_NhanVien.SelectedRows[0].Cells[0].Value).ToString() + "'";

                        using (SqlCommand command = new SqlCommand(deleteQuery, sqlConnection))
                        {
                            // Thực thi câu lệnh SQL
                            int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                dataGridView_NhanVien.Rows.RemoveAt(dataGridView_NhanVien.SelectedRows[0].Index);
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

                txtma.Clear();
                txtten.Clear();
                txtdt.Clear();
                txtchucvu.Clear();
                txtloai.Clear();
                txtluong.Clear();
                txtNgayNhanViec.Clear();
                txtNgayNhanViec.Clear();
                txtma.Focus();
                LoadDataSQL();
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string textSearch = txtSearch.Text;
            SearchInDataGridView(textSearch);
        }

        private void SearchInDataGridView(string searchText)
        {
            foreach (DataGridViewRow row in dataGridView_NhanVien.Rows)
            {
                row.Selected = false;
            }

            foreach (DataGridViewRow row in dataGridView_NhanVien.Rows)
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

        string checkstring = "";
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            checkstring = txtSearch.Text;
        }

        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            string textValue = checkstring;

            if (!string.IsNullOrEmpty(textValue))
            {
                // Kiểm tra xem phím nhấn có phải là Enter không
                if (e.KeyChar == (char)Keys.Enter && txtSearch.Focused)
                {
                    // Thực hiện click vào button khi nhấn Enter
                    btnSearch.PerformClick();
                }
                else if (txtSearch.Focused == false &&  e.KeyChar == (char)Keys.Enter)
                {
                    MessageBox.Show("Không có dữ liệu để thực hiện tìm kiếm\nVui lòng đừng nhấn nữa.", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        
    }
}

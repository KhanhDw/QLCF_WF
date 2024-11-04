using QLCF.NhanVienForm;
using QLCF.NhanVienForm.user_SanPham;
using System;
//using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace QLCF
{
    public partial class HoaDon : UserControl
    {

        private SqlConnection sqlConnection = null;// đối tượng kết nối
        //string connectionString = "Data Source=DESKTOP-39BMSC8\\GIAKHANH;Initial Catalog=QL_QuanCF;Integrated Security=True";
        string connectionString = LoadDatabase.connectionString;

        public static HoaDon instanceHoaDon;
        private int rowCount = 0;
        private string ngayHienTai;
        private string thangHienTai;
        private string namHienTai;


        public HoaDon()
        {
            InitializeComponent();
            instanceHoaDon = this;
        }

        public void HoaDon_Load(object sender, EventArgs e)
        {
            LoadDataSQL();
            ThemDuLieuVaoDataGridView();
            AutoClickButton();
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

                string query = "\r\nSELECT \r\n\t\tQL_HoaDon.IDHoaDon,\r\n\t\tQL_HoaDon.MaHoaDon, \r\n\t\tQL_HoaDon.IDKhachHang, \r\n\t\tQL_KhachHang.MaKhachHang,\r\n\t\tQL_KhachHang.TenKhachHang,\r\n\t\tQL_KhachHang.SoDienThoai,\r\n\t\tQL_HoaDon.MaSanPham, \r\n\t\tQL_SanPham.TenSanPham, \r\n\t\tQL_SanPham.GiaSanPham, \r\n\t\tQL_HoaDon.SoLuongSanPham, \r\n\t\tQL_HoaDon.TongGiaSanPham, \r\n\t\tQL_HoaDon.MaNhanVien, \r\n\t\tQL_NhanVien.TenNhanVien,\r\n\t\tQL_HoaDon.NgayLapHoaDon\r\n\r\nFROM QL_HoaDon, QL_SanPham, QL_KhachHang, QL_NhanVien\r\nWhere QL_HoaDon.MaSanPham = QL_SanPham.MaSanPham \r\n\t\tand\r\n\t\tQL_HoaDon.IDKhachHang = QL_KhachHang.IDKhachHang\r\n\t\tand \r\n\t\tQL_HoaDon.MaNhanVien = QL_NhanVien.MaNhanVien;";
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
                        dataGridView_HoaDon.DataSource = dataTable;

                        dataGridView_HoaDon.Columns["IDHoaDon"].HeaderText = "IDHD";
                        dataGridView_HoaDon.Columns["MaHoaDon"].HeaderText = "Mã HD";
                        dataGridView_HoaDon.Columns["IDKhachHang"].HeaderText = "IDKH";
                        dataGridView_HoaDon.Columns["MaKhachHang"].HeaderText = "Mã KH";
                        dataGridView_HoaDon.Columns["TenKhachHang"].HeaderText = "Tên KH";
                        dataGridView_HoaDon.Columns["SoDienThoai"].HeaderText = "SĐT";
                        dataGridView_HoaDon.Columns["MaSanPham"].HeaderText = "Mã SP";
                        dataGridView_HoaDon.Columns["TenSanPham"].HeaderText = "Tên SP";
                        dataGridView_HoaDon.Columns["GiaSanPham"].HeaderText = "Giá SP";
                        dataGridView_HoaDon.Columns["SoLuongSanPham"].HeaderText = "SL";
                        dataGridView_HoaDon.Columns["TongGiaSanPham"].HeaderText = "Tổng giá";
                        dataGridView_HoaDon.Columns["MaNhanVien"].HeaderText = "Mã NV";
                        dataGridView_HoaDon.Columns["TenNhanVien"].HeaderText = "Tên NV";
                        dataGridView_HoaDon.Columns["NgayLapHoaDon"].HeaderText = "Ngày Lập HD";

                        //// điều chỉnh độ rộng của cột 5 thành 150 pixels
                        dataGridView_HoaDon.Columns[0].Width = 60;


                        // Đặt căn giữ cho header của cột trong DataGridView
                        dataGridView_HoaDon.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dataGridView_HoaDon.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dataGridView_HoaDon.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dataGridView_HoaDon.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dataGridView_HoaDon.Columns[4].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dataGridView_HoaDon.Columns[5].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dataGridView_HoaDon.Columns[6].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dataGridView_HoaDon.Columns[7].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dataGridView_HoaDon.Columns[8].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dataGridView_HoaDon.Columns[9].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dataGridView_HoaDon.Columns[10].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dataGridView_HoaDon.Columns[11].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dataGridView_HoaDon.Columns[12].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dataGridView_HoaDon.Columns[13].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

                        // Đặt căn giữ cho cột trong dữ liệu trong DataGridView
                        dataGridView_HoaDon.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dataGridView_HoaDon.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dataGridView_HoaDon.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dataGridView_HoaDon.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dataGridView_HoaDon.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                        dataGridView_HoaDon.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dataGridView_HoaDon.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dataGridView_HoaDon.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                        dataGridView_HoaDon.Columns[8].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dataGridView_HoaDon.Columns[9].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dataGridView_HoaDon.Columns[10].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dataGridView_HoaDon.Columns[11].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dataGridView_HoaDon.Columns[12].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                        dataGridView_HoaDon.Columns[13].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                        //dataGridView_HoaDon.Columns[13].DefaultCellStyle.Format = "dd/MM/yyyy";


                        dataGridView_HoaDon.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 7, FontStyle.Bold);
                        dataGridView_HoaDon.DefaultCellStyle.Font = new Font("Tahoma", 7, FontStyle.Regular);
                    }
                }

                using (sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open(); // Mở kết nối

                    // Thực hiện truy vấn SQL để lấy tổng giá trị từ cột
                    string querySUM = "SELECT SUM(SoLuongSanPham) FROM QL_HoaDon";

                    using (SqlCommand cmd = new SqlCommand(querySUM, sqlConnection))
                    {
                        // Thực hiện truy vấn và nhận kết quả
                        int soluongLy = (int)cmd.ExecuteScalar();
                        //slly = soluongLy;
                        lbSLLyBan.Text = soluongLy.ToString();
                    }


                    int countUnique = 0;
                    // Thực hiện truy vấn SQL để lấy dữ liệu từ cơ sở dữ liệu.
                    string query1 = "SELECT MaHoaDon FROM QL_HoaDon";
                    using (SqlCommand command = new SqlCommand(query1, sqlConnection))
                    {
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            DataTable dataTable = new DataTable();
                            adapter.Fill(dataTable);

                            // Tạo Dictionary để theo dõi các giá trị đã xuất hiện.
                            Dictionary<string, int> valueCounts = new Dictionary<string, int>();

                            foreach (DataRow row in dataTable.Rows)
                            {
                                // Lấy giá trị từ cột trong DataTable.
                                string valueInColumn = row["MaHoaDon"].ToString();

                                // Kiểm tra xem giá trị đã xuất hiện trước đó chưa.
                                if (valueCounts.ContainsKey(valueInColumn))
                                {
                                    // Giá trị đã xuất hiện trước đó, tăng số lần xuất hiện.
                                    //valueCounts[valueInColumn]++;
                                }
                                else
                                {
                                    // Giá trị chưa xuất hiện trước đó, thêm vào Dictionary.
                                    valueCounts.Add(valueInColumn, 1);

                                    // Tăng số lượng giá trị không trùng.
                                    countUnique++;
                                }
                            }
                            // Hiển thị kết quả.
                            lbSoluongDonBan.Text = string.Empty;
                            lbSoluongDonBan.Text = countUnique.ToString();
                        }
                    }
                }
            }

            
        }

        private void ThemDuLieuVaoDataGridView()
        {
            dataGridView_HoaDon.ScrollBars = ScrollBars.Vertical; // Ẩn thanh cuộn ngan

            // Điều chỉnh độ rộng của cột tự động lấp đầy bảng
            dataGridView_HoaDon.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Điều chỉnh chiều cao của dòng header tự động
            dataGridView_HoaDon.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            // Điều chỉnh chiều cao cho tất cả các ô trong dòng
            dataGridView_HoaDon.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            // Chỉnh định dạng cho font của header
            dataGridView_HoaDon.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 10, FontStyle.Bold);

            // Chỉnh định dạng cho dữ liệu DataGridView
            dataGridView_HoaDon.DefaultCellStyle.Font = new Font("Tahoma", 8, FontStyle.Regular);
            
            // không cho người dùng nhập dòng
            dataGridView_HoaDon.AllowUserToAddRows = false;


        }

        //respontive form
        public void responsive_HoaDon(int newWidthForm)
        {

            if (newWidthForm == 1920)
            {
                pnlReponsiveHoaDon.Size = new Size(1395 + 310, 114);
                pnlContainTableHoaDon.Size = new Size(1389 + 300, 589 + 305);
            }
            else //if (newWidthForm == 1615)
            {
                pnlReponsiveHoaDon.Size = new Size(1395, 114);
                pnlContainTableHoaDon.Size = new Size(1389, 589);

            }

        }


        private void btnXoaHoaDon_Click(object sender, EventArgs e)
        {
            DialogResult xacnhan = MessageBox.Show("Dữ liệu không thể khôi phục sau khi xóa\nBạn có chắc muốn xóa không?", "Cảnh báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
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
                        string deleteQuery = "DELETE FROM QL_HoaDon WHERE IDHoaDon = '" + (dataGridView_HoaDon.SelectedRows[0].Cells[0].Value).ToString() + "'";

                        using (SqlCommand command = new SqlCommand(deleteQuery, sqlConnection))
                        {
                            // Thực thi câu lệnh SQL
                            int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                dataGridView_HoaDon.Rows.RemoveAt(dataGridView_HoaDon.SelectedRows[0].Index);
                            }

                        }
                    }
                }
                catch (Exception ex)
                {
                    // Xử lý lỗi nếu có
                    MessageBox.Show("Error: " + ex.Message);
                }
            }

            LoadDataSQL();

            string nhanPhanDoan = "";
            string nhanNgay = "";
            string nhanThang = "";
            string nhanNam = "";

            foreach (char character in lb_NgayBatDau.Text)
            {
                if (character == '/')
                {
                    nhanPhanDoan += '/';
                }
                else if (nhanPhanDoan == "/")
                {
                    nhanThang += character.ToString();
                }
                else if (nhanPhanDoan == "//")
                {
                    nhanNam += character.ToString();
                }
                else
                {
                    nhanNgay += character.ToString();
                }
            }
            SearchInDataGridView(nhanThang + "/" + nhanNgay + "/" + nhanNam);
        }


        private void SearchInDataGridView(string searchText)
        {

            int hoaDontrongngay = 0;
            int solybanduoctrongngay = 0;
            float doanhthutrongngay = 0;
            


            foreach (DataGridViewRow row in dataGridView_HoaDon.Rows)
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
                CurrencyManager currencyManager1 = (CurrencyManager)BindingContext[dataGridView_HoaDon.DataSource];
                currencyManager1.SuspendBinding();
                row.Visible = false;
                currencyManager1.ResumeBinding();

                // Hiển thị hoặc ẩn dòng tùy thuộc vào kết quả tìm kiếm
                row.Visible = rowVisible;


                // Tạo Dictionary để theo dõi các giá trị đã xuất hiện.
                Dictionary<string, int> valueCounts = new Dictionary<string, int>();


                if (rowVisible)
                {
                    DataGridViewCell cellA = row.Cells[1]; // đơn bán
                    DataGridViewCell cellB = row.Cells[9]; // số ly
                    DataGridViewCell cellC = row.Cells[10]; // doanh thu

                    if (cellA.Value != null)
                    {
                        // Kiểm tra xem giá trị đã xuất hiện trước đó chưa.
                        if (valueCounts.ContainsKey(cellA.ToString()))
                        {
                            // Giá trị đã xuất hiện trước đó, không làm gì cả
                        }
                        else
                        {
                            // Giá trị chưa xuất hiện trước đó, thêm vào Dictionary.
                            valueCounts.Add(cellA.ToString(), 1);
                            // tăng biến đếm lên 1
                            hoaDontrongngay += 1;
                        }
                    }

                    if (cellB.Value != null)
                    {

                        solybanduoctrongngay += int.Parse(cellB.Value.ToString()); // chuyển từ object -> string -> int

                    }

                    if (cellC.Value != null)
                    {
                        doanhthutrongngay += float.Parse(cellC.Value.ToString()); // chuyển từ object -> string -> float
                    }
                }

                // Hiển thị kết quả.
                lbSoluongDonBan.Text = string.Empty;
                lbSLLyBan.Text = string.Empty;
                lbDoanhThu.Text = string.Empty;

                lbSoluongDonBan.Text = hoaDontrongngay.ToString();
                lbSLLyBan.Text = solybanduoctrongngay.ToString();
                lbDoanhThu.Text = doanhthutrongngay.ToString();
                
            }
        }


        

  
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            SearchInDataGridView(txtSearch.Text);
        }

        private void btnHomNay_Click(object sender, EventArgs e)
        {
            // Lấy ngày và giờ hiện tại
            DateTime currentDate = DateTime.Now;
            lbDateBox.Text = currentDate.ToString("dd/MM/yyyy");
            lb_NgayBatDau.Text = currentDate.ToString("dd/MM/yyyy");

            string ngay = currentDate.ToString("dd");
            string thangs = currentDate.ToString("MM");
            string nam = currentDate.ToString("yyyy");

            if (int.Parse(ngay) < 10)
            {
                ngay = ngay.Substring(1);
            }
            else if (int.Parse(thangs) < 10)
            {
                thangs = thangs.Substring(1);
            }

            string dateFinish = thangs + "/" + ngay + "/" + nam;

            //Console.WriteLine(dateFinish);

            ngayHienTai = ngay;
            thangHienTai = thangs;
            namHienTai = nam;


            SearchInDataGridView(dateFinish);

        }


        



        private void btnNextDate_Click(object sender, EventArgs e)
        {
            // Lấy giá trị hiện tại từ DateTimePicker
            DateTime currentDate = dateTimePicker_NgayBatDau.Value;

            // Tăng giá trị ngày lên 1 ngày để chuyển đến ngày tiếp theo
            DateTime nextDate = currentDate.AddDays(1);

            // Thiết lập giá trị mới cho DateTimePicker
            dateTimePicker_NgayBatDau.Value = nextDate;


            string ngay = nextDate.ToString("dd");
            string thangs = nextDate.ToString("MM");
            string nam = nextDate.ToString("yyyy");


            if (int.Parse(ngay) < 10)
            {
                ngay = ngay.Substring(1);
            }
            else if (int.Parse(thangs) < 10)
            {
                thangs = thangs.Substring(1);
            }


            //thay đổi giá trị hiển thị
            lb_NgayBatDau.Text = nextDate.ToString("dd/MM/yyyy");

            //ngayHienTai = ngay;
            //thangHienTai = thangs;
            //namHienTai = nam;

            string dateFinish = thangs + "/" + ngay + "/" + nam;

            SearchInDataGridView(dateFinish);
        }

        private void btnLastDate_Click(object sender, EventArgs e)
        {
            // Lấy giá trị hiện tại từ DateTimePicker
            DateTime currentDate = dateTimePicker_NgayBatDau.Value;

            // Tăng giá trị ngày lên 1 ngày để chuyển đến ngày tiếp theo
            DateTime nextDate = currentDate.AddDays(-1);

            // Thiết lập giá trị mới cho DateTimePicker
            dateTimePicker_NgayBatDau.Value = nextDate;

            string ngay = nextDate.ToString("dd");
            string thangs = nextDate.ToString("MM");
            string nam = nextDate.ToString("yyyy");


            if (int.Parse(ngay) < 10)
            {
                ngay = ngay.Substring(1);
            }
            else if (int.Parse(thangs) < 10)
            {
                thangs = thangs.Substring(1);
            }


            //thay đổi giá trị hiển thị
            lb_NgayBatDau.Text = nextDate.ToString("dd/MM/yyyy");

            //ngayHienTai = ngay;
            //thangHienTai = thangs;
            //namHienTai = nam;

            string dateFinish = thangs + "/" + ngay + "/" + nam;

            SearchInDataGridView(dateFinish);

        }

        private void btnShowAll_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView_HoaDon.Rows)
            {
                // Hiển thị dòng 
                row.Visible = true;
            }
        }
    }
}

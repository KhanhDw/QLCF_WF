using QLCF.NhanVienForm.user_SanPham;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

namespace QLCF.NhanVienForm
{


    public partial class User_Donban : UserControl
    {

        private SqlConnection sqlConnection = null;// đối tượng kết nối
        //string connectionString = "Data Source=DESKTOP-39BMSC8\\GIAKHANH;Initial Catalog=QL_QuanCF;Integrated Security=True";
        string connectionString = LoadDatabase.connectionString;
        private Timer timer = new Timer();
        private int slly;


        public static User_Donban instanceDonBan;

        public User_Donban()
        {
            InitializeComponent();

            instanceDonBan = this;

            //tạo bảng cho datagridview đơn bán
            ThemDuLieuVaoDataGridView();
            // tạo đồng hồ và ngày hiện tại
            KhoiTaoNgayThangHienTai();
            // Đăng ký sự kiện DataChanged từ DataTransfer
            TruyenData.Instance.DataChanged += DataTransfer_DataChanged_datagridview;
        }

        public User_Donban(int KichHoatSuKienClick)
        {
            InitializeComponent();

            // Đăng ký sự kiện DataChanged từ DataTransfer
            if (KichHoatSuKienClick== 1) { 
                //tạo bảng cho datagridview đơn bán
                ThemDuLieuVaoDataGridView();
                // tạo đồng hồ và ngày hiện tại
                KhoiTaoNgayThangHienTai();
            }
            LoadDataSQL();
            timer.Start();
            lbSoLuongDonBan.Text = "0";
        }

        public void User_Donban_Load(object sender, EventArgs e)
        {
            LoadDataSQL();
            timer.Start();
            //lbSoLuongDonBan.Text = "0";
            DemSoLuong();
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
                        dataGridView_DonBan.DataSource = dataTable;

                        dataGridView_DonBan.Columns["IDHoaDon"].HeaderText = "IDHD";
                        dataGridView_DonBan.Columns["MaHoaDon"].HeaderText = "Mã HD";
                        dataGridView_DonBan.Columns["IDKhachHang"].HeaderText = "IDKH";
                        dataGridView_DonBan.Columns["MaKhachHang"].HeaderText = "Mã KH";
                        dataGridView_DonBan.Columns["TenKhachHang"].HeaderText = "Tên KH";
                        dataGridView_DonBan.Columns["SoDienThoai"].HeaderText = "SĐT";
                        dataGridView_DonBan.Columns["MaSanPham"].HeaderText = "Mã SP";
                        dataGridView_DonBan.Columns["TenSanPham"].HeaderText = "Tên SP";
                        dataGridView_DonBan.Columns["GiaSanPham"].HeaderText = "Giá SP";
                        dataGridView_DonBan.Columns["SoLuongSanPham"].HeaderText = "SL";
                        dataGridView_DonBan.Columns["TongGiaSanPham"].HeaderText = "Tổng giá";
                        dataGridView_DonBan.Columns["MaNhanVien"].HeaderText = "Mã NV";
                        dataGridView_DonBan.Columns["TenNhanVien"].HeaderText = "Tên NV";
                        dataGridView_DonBan.Columns["NgayLapHoaDon"].HeaderText = "Ngày Lập HD";

                        // Đặt căn giữ cho header của cột trong DataGridView
                        dataGridView_DonBan.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dataGridView_DonBan.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dataGridView_DonBan.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dataGridView_DonBan.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dataGridView_DonBan.Columns[4].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dataGridView_DonBan.Columns[5].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dataGridView_DonBan.Columns[6].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dataGridView_DonBan.Columns[7].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dataGridView_DonBan.Columns[8].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dataGridView_DonBan.Columns[9].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dataGridView_DonBan.Columns[10].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dataGridView_DonBan.Columns[11].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dataGridView_DonBan.Columns[12].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dataGridView_DonBan.Columns[13].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

                        // Đặt căn giữ cho cột trong dữ liệu trong DataGridView
                        dataGridView_DonBan.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dataGridView_DonBan.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dataGridView_DonBan.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dataGridView_DonBan.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dataGridView_DonBan.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                        dataGridView_DonBan.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dataGridView_DonBan.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dataGridView_DonBan.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                        dataGridView_DonBan.Columns[8].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dataGridView_DonBan.Columns[9].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dataGridView_DonBan.Columns[10].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dataGridView_DonBan.Columns[11].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dataGridView_DonBan.Columns[12].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                        dataGridView_DonBan.Columns[13].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;


                        dataGridView_DonBan.Columns[13].DefaultCellStyle.Format = "dd/MM/yyyy";

                        dataGridView_DonBan.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 7, FontStyle.Bold);
                        dataGridView_DonBan.DefaultCellStyle.Font = new Font("Tahoma", 7, FontStyle.Regular);
                    }
                }
                DemSoLuong();
            }
        }

        int index = 0;
        // Phương thức xử lý sự kiện DataChanged - thêm dữ liệu cho bảng đơn bán của nhân viên
        private void DataTransfer_DataChanged_datagridview(object sender, EventArgs e)
        {
            // không cho người dùng nhập dữ liệu. làm ẩn dòng bị rỗng ở cuối bảng
            dataGridView_DonBan.AllowUserToAddRows = false;
            //làm rỗng datasource trong datagridview tránh ghi đè dữ liệu
            dataGridView_DonBan.DataSource = null;
            // truyền dữ liệu nhận được từ formThanToan vào DataTable


            if (TruyenData.Instance.SharedData.Count > 1)
            {
                foreach (string[] item in TruyenData.Instance.SharedData)
                {
                    // Tạo bản sao của mảng dữ liệu - tránh việc bị ghi đè dữ liệu 
                    string[] rowData = (string[])item.Clone();

                    DateTime currentTime = DateTime.Now;
                    // Tạo đối tượng SqlConnection
                    using (sqlConnection = new SqlConnection(connectionString))
                    {
                        // Mở kết nối
                        sqlConnection.Open();
                        // Bắt đầu transaction
                        string insertQueryKH = "INSERT INTO QL_KhachHang (IDKhachHang,MaKhachHang, TenKhachHang, SoDienThoai) VALUES (@IDKhachHang, @MaKhachHang,@TenKhachHang,@SoDienThoai)";

                        using (SqlCommand thucthi = new SqlCommand(insertQueryKH, sqlConnection))
                        {
                            thucthi.Parameters.AddWithValue("@IDKhachHang", int.Parse(rowData[2]));
                            thucthi.Parameters.AddWithValue("@MaKhachHang", rowData[3]);
                            thucthi.Parameters.AddWithValue("@TenKhachHang", rowData[4]);
                            thucthi.Parameters.AddWithValue("@SoDienThoai", rowData[5]);
                            thucthi.ExecuteNonQuery();
                        }
                    }
                    break;
                }

                foreach (string[] item in TruyenData.Instance.SharedData)
                {
                    // Tạo bản sao của mảng dữ liệu - tránh việc bị ghi đè dữ liệu 
                    string[] rowData = (string[])item.Clone();

                    DateTime currentTime = DateTime.Now;

                    using (sqlConnection = new SqlConnection(connectionString))
                    {
                        // Mở kết nối
                        sqlConnection.Open();

                        // Bước 1: Truy vấn giá trị hiện tại của IDHoaDon từ SQL Server
                        string selectMaxIDQuery = "SELECT MAX(IDHoaDon) FROM QL_HoaDon";

                        using (SqlCommand selectMaxIDCmd = new SqlCommand(selectMaxIDQuery, sqlConnection))
                        {
                            object result = selectMaxIDCmd.ExecuteScalar();

                            // Bước 2: Tăng giá trị đó lên 1 để có giá trị mới cho tangID
                            int tangID = (result == DBNull.Value) ? 1 : Convert.ToInt32(result) + 1;

                            // Bước 3: Thêm dữ liệu mới với giá trị tangID vào SQL
                            string insertQueryHD = "INSERT INTO QL_HoaDon (IDHoaDon,MaHoaDon, IDKhachHang, MaSanPham, SoLuongSanPham, TongGiaSanPham, MaNhanVien, NgayLapHoaDon) VALUES (@IDHoaDon,@MaHoaDon, @IDKhachHang, @MaSanPham, @SoLuongSanPham, @TongGiaSanPham, @MaNhanVien, @NgayLapHoaDon)";

                            using (SqlCommand cmd = new SqlCommand(insertQueryHD, sqlConnection))
                            {
                                cmd.Parameters.AddWithValue("@IDHoaDon", tangID);
                                cmd.Parameters.AddWithValue("@MaHoaDon", rowData[1]);
                                cmd.Parameters.AddWithValue("@IDKhachHang", rowData[2]);
                                cmd.Parameters.AddWithValue("@MaSanPham", rowData[6]);
                                cmd.Parameters.AddWithValue("@SoLuongSanPham", rowData[9]);
                                cmd.Parameters.AddWithValue("@TongGiaSanPham", float.Parse(rowData[10]));
                                cmd.Parameters.AddWithValue("@MaNhanVien", rowData[11]);
                                cmd.Parameters.AddWithValue("@NgayLapHoaDon", currentTime);
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                }
            }
            else
            {
                foreach (string[] item in TruyenData.Instance.SharedData)
                {
                    // Tạo bản sao của mảng dữ liệu - tránh việc bị ghi đè dữ liệu 
                    string[] rowData = (string[])item.Clone();

                    DateTime currentTime = DateTime.Now;
                    // Tạo đối tượng SqlConnection
                    using (sqlConnection = new SqlConnection(connectionString))
                    {
                        // Mở kết nối
                        sqlConnection.Open();
                        // Bắt đầu transaction
                        string insertQueryKH = "INSERT INTO QL_KhachHang (IDKhachHang,MaKhachHang, TenKhachHang, SoDienThoai) VALUES (@IDKhachHang, @MaKhachHang,@TenKhachHang,@SoDienThoai)";

                        using (SqlCommand thucthi = new SqlCommand(insertQueryKH, sqlConnection))
                        {
                            thucthi.Parameters.AddWithValue("@IDKhachHang", int.Parse(rowData[2]) + 1);
                            thucthi.Parameters.AddWithValue("@MaKhachHang", rowData[3]);
                            thucthi.Parameters.AddWithValue("@TenKhachHang", rowData[4]);
                            thucthi.Parameters.AddWithValue("@SoDienThoai", rowData[5]);


                            // Commit transaction khi mọi thứ thành công
                            thucthi.ExecuteNonQuery();

                        }
                        string insertQueryHD = "INSERT INTO QL_HoaDon (IDHoaDon,MaHoaDon, IDKhachHang, MaSanPham, SoLuongSanPham, TongGiaSanPham, MaNhanVien, NgayLapHoaDon) VALUES (@IDHoaDon,@MaHoaDon, @IDKhachHang, @MaSanPham, @SoLuongSanPham, @TongGiaSanPham, @MaNhanVien, @NgayLapHoaDon)";

                        using (SqlCommand cmd = new SqlCommand(insertQueryHD, sqlConnection))
                        {

                            cmd.Parameters.AddWithValue("@IDHoaDon", int.Parse(rowData[0]));
                            cmd.Parameters.AddWithValue("@MaHoaDon", rowData[1]);
                            cmd.Parameters.AddWithValue("@IDKhachHang", int.Parse(rowData[2])+1);
                            cmd.Parameters.AddWithValue("@MaSanPham", rowData[6]);
                            cmd.Parameters.AddWithValue("@SoLuongSanPham", rowData[9]);
                            cmd.Parameters.AddWithValue("@TongGiaSanPham", rowData[10]+".000");
                            cmd.Parameters.AddWithValue("@MaNhanVien", rowData[11]);
                            cmd.Parameters.AddWithValue("@NgayLapHoaDon", currentTime);
                            cmd.ExecuteNonQuery();
                        }

                        index++;

                    }
                }
            } 
            LoadDataSQL();


           // ContainData.Instance._dataContain = TruyenData.Instance.SharedData;
        }   

        private void ThemDuLieuVaoDataGridView()
        {
            dataGridView_DonBan.ScrollBars = ScrollBars.Vertical; // Ẩn thanh cuộn ngan
            

            // Điều chỉnh độ rộng của cột tự động lấp đầy bảng
            dataGridView_DonBan.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Điều chỉnh chiều cao của dòng header tự động
            dataGridView_DonBan.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            // Điều chỉnh chiều rộng cho tất cả các ô trong dòng
            dataGridView_DonBan.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            // Chỉnh định dạng cho font của header
            dataGridView_DonBan.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 8, FontStyle.Bold);

            // Chỉnh định dạng cho dữ liệu DataGridView
            dataGridView_DonBan.DefaultCellStyle.Font = new Font("Tahoma", 8, FontStyle.Regular);

            dataGridView_DonBan.AllowUserToAddRows =false;

        }

        private void DemSoLuong()
        {
            // Lấy ngày và giờ hiện tại
            DateTime currentDate = DateTime.Now;
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



            SearchInDataGridView(dateFinish);
        }

        private void SearchInDataGridView(string searchText)
        {
            int sodondatban = 0;

            foreach (DataGridViewRow row in dataGridView_DonBan.Rows)
            {
                row.Selected = false;
            }

            foreach (DataGridViewRow row in dataGridView_DonBan.Rows)
            {
                bool rowVisible = false;

                foreach (DataGridViewCell cell in row.Cells)
                {
                    if (cell.Value != null && cell.Value.ToString().IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        // Tìm thấy chuỗi tìm kiếm trong ô
                        rowVisible = true;
                        break;
                    }
                }

                // fix lỗi hiển thị ẩn dòng trong datagridview khi truyền sql vào
                CurrencyManager currencyManager1 = (CurrencyManager)BindingContext[dataGridView_DonBan.DataSource];
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
                            sodondatban += 1;
                        }
                    }
                }

                // Hiển thị kết quả.
                lbSoLuongDonBan.Text = string.Empty;
                lbSoLuongDonBan.Text = sodondatban.ToString();
            }
        }



        //respontive form
        public void responsive_Donban(int newWidthForm)
        {

            if (newWidthForm == 1920)
            {
                pnlReponsiveDonBan.Size = new Size(1395 + 310, 71);
                pnlContainTableDonBan.Size = new Size(1389 + 300, 589 + 370);
            }
            else
            {
                pnlReponsiveDonBan.Size = new Size(1615 - 320, 71); //1389, 681
                pnlContainTableDonBan.Size = new Size(1615 - 315, 681 + 50);

            }

        }


        private void dataGridView_DonBan_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            // Lấy dòng mới được thêm vào
            DataGridViewRow newRow = dataGridView_DonBan.Rows[e.RowIndex];
            int dataCount = 0;
            if (dataGridView_DonBan.Rows.Count > 0)
            {
                // Kiểm tra từng dòng trong cột A
                foreach (DataGridViewRow row in dataGridView_DonBan.Rows)
                {
                    // Kiểm tra xem giá trị của ô trong cột A có tồn tại và không rỗng
                    if (row.Cells[0].Value != null && !string.IsNullOrEmpty(row.Cells[0].Value.ToString()))
                    {
                        dataCount++;
                    }
                }

                
                UpdateLabelValueDonBan(dataCount);

            }
        }   

        private void UpdateLabelValueDonBan(int count)
        {
            // Cập nhật giá trị Label với số lượng dòng hiện tại trong DataGridView
            lbSoLuongDonBan.Text = (count+1).ToString();
        }

        // lấy ngày giờ hiện tại
        private void KhoiTaoNgayThangHienTai()
        {
            timer = new Timer();
            timer.Interval = 1000; // Cập nhật mỗi 1 giây
            timer.Tick += Timer_Tick;
        }

        // định dạng ngày và giờ khi hiển thị
        private void Timer_Tick(object sender, EventArgs e)
        {
            DateTime currentTime = DateTime.Now;
            string timeText = currentTime.ToString("HH:mm:ss");
            string dateText = currentTime.ToString("dd/MM/yyyy");

            lbDate.Text = timeText + "  " + dateText;
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            // Lọc dữ liệu trực tiếp trên DataGridView
            string searchText = txtSearch.Text;

            foreach (DataGridViewRow row in dataGridView_DonBan.Rows)
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
                CurrencyManager currencyManager1 = (CurrencyManager)BindingContext[dataGridView_DonBan.DataSource];
                currencyManager1.SuspendBinding();
                row.Visible = false;
                currencyManager1.ResumeBinding();

                // Hiển thị hoặc ẩn dòng tùy thuộc vào kết quả tìm kiếm
                row.Visible = rowVisible;
            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Xml.Linq;


namespace QLCF
{
    public partial class TongQuan : UserControl
    {
        private SqlConnection sqlConnection = null;// đối tượng kết nối
        //string connectionString = "Data Source=DESKTOP-39BMSC8\\GIAKHANH;Initial Catalog=QL_QuanCF;Integrated Security=True";
        string connectionString = LoadDatabase.connectionString;

        private int userControlWidth;
        private int userControlHeight;

        private Timer timer = new Timer();
        public List<Panel> panelList;

        private List<string> ngayBieuDo;
        private List<float> doanhthuBieuDo;
        private List<string> ngayBieuDoThang;
        private List<float> doanhthuBieuDoThang;
        float varHoanhThuBieuDo = 0;
        float varHoanhThuBieuDoThang = 0;

        public static TongQuan instanceTongQuan;

        public TongQuan()
        {
            InitializeComponent();
            //
            instanceTongQuan = this;

            KhoiTaoNgayThangHienTai();
            //(nếu tham số là 1920 sẽ phóng to (max) cửa sổ - nếu không thì cửa sô loại nhỏ(min))
            dieuChinhKichThuoc4BoxPnl2(1);
            //khỏi tạo các item panel trong list đánh giá

        }

        private void TongQuan_Load(object sender, EventArgs e)
        {
            timer.Start();
            btnNgayThongke.PerformClick();
            ngayHienTai();
            LoadData();
            ThemDuLieuVaoDataGridView();
            HoaDon.instanceHoaDon.HoaDon_Load(sender, e);
            DoanhThu.instanceDoanhThu.DoanhThu_Load(sender, e);
            // tải dữ liệu cho biểu đồ tháng 
            //DateTime dateTime = DateTime.Now;
            //string dateText1 = dateTime.ToString("yyyy");
            //TinhVaXuatDuLieuBieuDoThang(dateText1);
        }

        float varHoanhThu = 0;
        int varSoLy = 0;
        int varhoadon = 0;

        private void TachNgayThangNam(string ngaythangVaNam)
        {
            string nhanPhanDoan = "";
            string nhanNgay1 = "";
            string nhanThang1 = "";
            string nhanNam1 = "";

            foreach (char character in ngaythangVaNam)
            {
                if (character == '/')
                {
                    nhanPhanDoan += '/';
                }
                else if (nhanPhanDoan == "/")
                {
                    nhanThang1 += character.ToString();
                }
                else if (nhanPhanDoan == "//")
                {
                    nhanNam1 += character.ToString();
                }
                else
                {
                    nhanNgay1 += character.ToString();
                }
            }

            //nhanNgay = nhanNgay1;
            //nhanThang = nhanThang1;
            //nhanNam = nhanNam1;

            LoadDataSQLTheoNgay(int.Parse(nhanNam1), int.Parse(nhanThang1), int.Parse(nhanNgay1));
            //TinhVaXuatDuLieu(int.Parse(nhanNam1), int.Parse(nhanThang1), int.Parse(nhanNgay1));

        }

        private void LoadDataSQLTheoNgay(int nhanNam, int nhanThang, int nhanNgay)
        {
            //Console.WriteLine($"thang -->:{nhanNgay} - {nhanThang} - {nhanNam}");

            string sumGia = "";
            string sumLy = "";
            int countUnique = 0;

            using (sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open(); // Mở kết nối

                // Tạo câu truy vấn SQL
                string query1 = "SELECT SUM(SoLuongSanPham) AS TotalB FROM QL_HoaDon " +
                    "WHERE DATEPART(DAY, NgayLapHoaDon) = " + nhanNgay +
                    "AND DATEPART(MONTH, NgayLapHoaDon) = " + nhanThang +
                    "AND DATEPART(YEAR, NgayLapHoaDon) = " + nhanNam + ";";

                // Tạo đối tượng Command và thi hành câu truy vấn
                using (SqlCommand command = new SqlCommand(query1, sqlConnection))
                {
                    // Đọc kết quả từ truy vấn
                    object nhanvv = command.ExecuteScalar();
                    sumLy = nhanvv.ToString();
                    Console.WriteLine(sumLy + "--------");
                }

                // Tạo câu truy vấn SQL
                string query11 = "SELECT SUM(TongGiaSanPham) AS TotalB FROM QL_HoaDon " +
                    "WHERE DATEPART(DAY, NgayLapHoaDon) = '" + nhanNgay + "' " +
                    "AND DATEPART(MONTH, NgayLapHoaDon) = '" + nhanThang + "' " +
                    "AND DATEPART(YEAR, NgayLapHoaDon) = '" + nhanNam + "';";

                // Tạo đối tượng Command và thi hành câu truy vấn
                using (SqlCommand command = new SqlCommand(query11, sqlConnection))
                {
                    // Đọc kết quả từ truy vấn
                    object nhanvv = command.ExecuteScalar();
                    sumGia = nhanvv.ToString();
                }



                // Thực hiện truy vấn SQL để lấy dữ liệu từ cơ sở dữ liệu.
                string query111 = "SELECT MaHoaDon FROM QL_HoaDon " +
                    "WHERE DATEPART(DAY, NgayLapHoaDon) = '" + nhanNgay + "' " +
                    "AND DATEPART(MONTH, NgayLapHoaDon) = '" + nhanThang + "' " +
                    "AND DATEPART(YEAR, NgayLapHoaDon) = '" + nhanNam + "';";

                using (SqlCommand command = new SqlCommand(query111, sqlConnection))
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
                            }
                            else
                            {
                                // Giá trị chưa xuất hiện trước đó, thêm vào Dictionary.
                                valueCounts.Add(valueInColumn, 1);

                                // Tăng số lượng giá trị không trùng.
                                countUnique++;
                            }
                        }
                    }
                }



            }

            if (countUnique != 0)
                varhoadon = countUnique;
            else varhoadon = 0;

            if (sumLy != "")
                varSoLy = int.Parse(sumLy);
            else varSoLy = 0;

            if (sumGia != "")
            {
                varHoanhThu = float.Parse(sumGia);
                Math.Round((double)varHoanhThu, 3);
            }
            else varHoanhThu = 0;

            lbSLLy_QL.Text = varSoLy.ToString();
            //lbThongTinKH_QL.Text = 
            lbHoaDon_QL.Text = varhoadon.ToString();
            float doanhthuformat = float.Parse((varHoanhThu.ToString()+ "000").Replace(".", ""));
            lbDoanhThuHomNay.Text = doanhthuformat.ToString("N2");

            // Loại bỏ phần thập phân
            int indexOfDecimalSeparator = lbDoanhThuHomNay.Text.IndexOf('.');
            string formattedAmountWithoutDecimals = (indexOfDecimalSeparator >= 0)
                ? lbDoanhThuHomNay.Text.Substring(0, indexOfDecimalSeparator)
                : lbDoanhThuHomNay.Text;

            lbDoanhThuHomNay.Text = formattedAmountWithoutDecimals;
        }

        private void ngayHienTai()
        {
            DateTime currentTime = DateTime.Now;
            string dateText = "Hôm nay " + currentTime.ToString("dd/MM");
            lbHn1.Text = dateText;
            //lbHn2.Text = dateText;
            lbHn3.Text = dateText;
            lbHn4.Text = dateText;
            string dateText1 = currentTime.ToString("dd/MM/yyyy");

            TachNgayThangNam(dateText1);
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

            txtNgayThangHienTai.Text = timeText + "  " + dateText;
        }

        // điều chỉnh kích thức khi thu-phóng cửa sổ của flowLayoutPanel 2
        public void dieuChinhKichThuoc4BoxPnl2(int newWidthForm)
        {
            if (newWidthForm == 1920)
            {
                flowLayoutPnl2.Size = new Size(1700, 156);
                panel1.Size = new Size(910 + 300, 627 + 263);
                pnlLuotDanhGia.Size = new Size(267 + 60, 150);
                //pnlKhachHang.Size = new Size(262 + 60, 150);
                pnlDonHang.Size = new Size(240 + 100, 150);
                pnlDoanhThu.Size = new Size(336 + 85, 150);

            }
            else //if (userControlWidth == 1395)
            {

                flowLayoutPnl2.Size = new Size(1395, 119);
                panel1.Size = new Size(910, 627);
                pnlLuotDanhGia.Size = new Size(253 + 20, 103);
                //pnlKhachHang.Size = new Size(261, 103);
                pnlDonHang.Size = new Size(251, 103);
                pnlDoanhThu.Size = new Size(336, 103);
            }
        }

        //- điều chỉnh kích thức khi thu-phóng cửa sổ của flowLayoutPanel 3
        public void dieuChinhKichThuoc3BoxPnl3(int newWidthForm)
        {

            if (newWidthForm == 1920)
            {
                //flowLayoutPanel1.Size = new Size(910, 111);

                flowLayoutPnl3.Size = new Size(1700, 790);

                panel6.Size = new Size(910, 161);

                pnlBieuDoAndDanhGia.Size = new Size(891 + 302, 730);
                panelBieuDo.Size = new Size(891, 335 + 100 + 300);
                chart1.Size = new Size(891, 304 + 100 + 300);
                pnlMonBanChay.Size = new Size(470, 628 + 263);

            }
            else
            {
                flowLayoutPnl3.Size = new Size(1395, 514);

                panel6.Size = new Size(910, 111);


                pnlBieuDoAndDanhGia.Size = new Size(891, 498);
                panelBieuDo.Size = new Size(891, 498);
                chart1.Size = new Size(891, 466);
                pnlMonBanChay.Size = new Size(469, 623);
            }

        }

        public void responsive_TongQuan(int newWidthForm)
        {
            dieuChinhKichThuoc4BoxPnl2(newWidthForm);
            dieuChinhKichThuoc3BoxPnl3(newWidthForm);
        }

        private void ThemDuLieuVaoDataGridView()
        {
            dataGridView_MonBanChay.ScrollBars = ScrollBars.Vertical; // Ẩn chỉ thanh cuộn ngan

            dataGridView_MonBanChay.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 10, FontStyle.Bold);
            dataGridView_MonBanChay.DefaultCellStyle.Font = new Font("Tahoma", 10, FontStyle.Regular);
        }



        private void LoadData()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Thực hiện truy vấn SQL
                    string sqlQuery = "SELECT QL_HoaDon.MaSanPham, QL_SanPham.TenSanPham, COUNT(*) AS Occurrences " +
                        " FROM QL_HoaDon" +
                        " JOIN QL_SanPham ON QL_HoaDon.MaSanPham = QL_SanPham.MaSanPham" +
                        " GROUP BY QL_HoaDon.MaSanPham, QL_SanPham.TenSanPham" +
                        " ORDER BY Occurrences DESC;";

                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            // Tạo DataTable để lưu trữ dữ liệu từ cơ sở dữ liệu
                            DataTable dataTable = new DataTable();
                            adapter.Fill(dataTable);

                            // Gán DataTable làm nguồn dữ liệu cho DataGridView
                            dataGridView_MonBanChay.DataSource = dataTable;

                            dataGridView_MonBanChay.Columns["MaSanPham"].HeaderText = "Mã sản phẩm";
                            dataGridView_MonBanChay.Columns["TenSanPham"].HeaderText = "Tên sản phẩm";
                            dataGridView_MonBanChay.Columns["Occurrences"].HeaderText = "Số lượng bán";

                            // Đặt căn giữ cho header của cột trong DataGridView
                            dataGridView_MonBanChay.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                            dataGridView_MonBanChay.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                            dataGridView_MonBanChay.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

                            // Đặt căn giữ cho cột trong dữ liệu trong DataGridView
                            dataGridView_MonBanChay.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                            dataGridView_MonBanChay.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                            dataGridView_MonBanChay.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                            
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // biểu đồ barchart
        // thêm thư viện System.Windows.Forms.DataVisualization vào dự án của mình.
        // Để làm điều này, cần thực hiện các bước sau:
        // a.Nhấp chuột phải vào tên dự án trong Solution Explorer.
        // b.Chọn "Add" và sau đó chọn "Reference."
        // c.Trong cửa sổ Reference Manager, tìm và chọn "System.Windows.Forms.DataVisualization."
        // d.Nhấp vào nút "OK" để thêm thư viện này vào dự án
        public void bieudo(String newSeries)
        {
            // Xóa dữ liệu cũ (nếu có)
            chart1.Series.Clear();

            // Thêm dữ liệu vào biểu đồ
            chart1.Series.Add(newSeries);

            // Cài đặt biểu đồ cột
            chart1.Series[newSeries].ChartType = SeriesChartType.Column;

            // hiện số trên mỗi cột
            chart1.Series[newSeries].IsValueShownAsLabel = true;

            // Vô hiệu hóa vị trí tự động chú thích
            chart1.Legends["Khách hàng"].Position.Auto = false;

            //Đặt tên cho trục
            chart1.ChartAreas["Khách hàng"].AxisX.Title = "Ngày"; // Đặt tên trục X
            chart1.ChartAreas["Khách hàng"].AxisY.Title = "Số lượng khách"; // Đặt tên trục Y


        }

        private void btnNgayThongke_Click(object sender, EventArgs e)
        {
            // Xóa dữ liệu cũ (nếu có)
            chart1.Series.Clear();

            DateTime dateTime = DateTime.Now;
            string dateText1 = dateTime.ToString("MM/yyyy");

            TinhVaXuatDuLieuBieuDo(dateText1);


            bieudo("Doanh thu trong ngày");

            chart1.Series["Doanh thu trong ngày"].Points.DataBindXY(ngayBieuDo, doanhthuBieuDo);

            lbTitleThongKe.Text = "Thống kê lượng Doanh thu - trong ngày";
        }

        private void btnThangThongke_Click(object sender, EventArgs e)
        {
            // Xóa dữ liệu cũ (nếu có)
            chart1.Series.Clear();

            // Tạo dữ liệu mẫu cho ngày
            ngayBieuDoThang = new List<string>() { "Tháng 1", "Tháng 2", "Tháng 3", "Tháng 4", "Tháng 5", "Tháng 6", "Tháng 7", "Tháng 8", "Tháng 9", "Tháng 10", "Tháng 11", "Tháng 12" };

            int[] customers = { 10, 34, 54, 12, 64, 10, 34, 54, 12, 64, 12, 64 };

            DateTime dateTime = DateTime.Now;
            string dateText1 = dateTime.ToString("yyyy");
            TinhVaXuatDuLieuBieuDoThang(dateText1);
                
            // Thêm dữ liệu vào biểu đồ
            //chart1.Series.Add("Khách hàng trong ngày");

            bieudo("Doanh thu trong Tháng");

            chart1.Series["Doanh thu trong Tháng"].Points.DataBindXY(ngayBieuDoThang, doanhthuBieuDoThang);

            lbTitleThongKe.Text = "Thống kê lượng doanh thu - trong tháng";
        }


        // theo ngày

        private void TachNgayThangNamBieuDo(string ngaythangVaNam)
        {
            string nhanPhanDoan = "";
            string nhanNgay1 = "";
            string nhanThang1 = "";
            string nhanNam1 = "";

            foreach (char character in ngaythangVaNam)
            {
                if (character == '/')
                {
                    nhanPhanDoan += '/';
                }
                else if (nhanPhanDoan == "/")
                {
                    nhanThang1 += character.ToString();
                }
                else if (nhanPhanDoan == "//")
                {
                    nhanNam1 += character.ToString();
                }
                else
                {
                    nhanNgay1 += character.ToString();
                }
            }

            //nhanNgay = nhanNgay1;
            //nhanThang = nhanThang1;
            //nhanNam = nhanNam1;

            LoadDataSQLTheoNgayBieuDo(int.Parse(nhanNam1), int.Parse(nhanThang1), int.Parse(nhanNgay1));
            //TinhVaXuatDuLieu(int.Parse(nhanNam1), int.Parse(nhanThang1), int.Parse(nhanNgay1));

        }

        private void LoadDataSQLTheoNgayBieuDo(int nhanNam, int nhanThang, int nhanNgay)
        {
            //Console.WriteLine($"thang -->:{nhanNgay} - {nhanThang} - {nhanNam}");

            string sumGia = "";

            using (sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open(); // Mở kết nối

                // Tạo câu truy vấn SQL
                string query11 = "SELECT SUM(TongGiaSanPham) AS TotalB FROM QL_HoaDon " +
                    "WHERE DATEPART(DAY, NgayLapHoaDon) = '" + nhanNgay + "' " +
                    "AND DATEPART(MONTH, NgayLapHoaDon) = '" + nhanThang + "' " +
                    "AND DATEPART(YEAR, NgayLapHoaDon) = '" + nhanNam + "';";

                // Tạo đối tượng Command và thi hành câu truy vấn
                using (SqlCommand command = new SqlCommand(query11, sqlConnection))
                {
                    // Đọc kết quả từ truy vấn
                    object nhanvv = command.ExecuteScalar();
                    sumGia = nhanvv.ToString();
                }
            }

            if (sumGia != "")
            {
                varHoanhThuBieuDo = float.Parse(sumGia);
                Math.Round((double)varHoanhThuBieuDo, 3);
            }
            else varHoanhThuBieuDo = 0;

        }

        private void TinhVaXuatDuLieuBieuDo(string thangnam)
        {
            string nhanPhanDoan = "";
            string nhanThang1 = "";
            string nhanNam1 = "";

            foreach (char character in thangnam)
            {
                if (character == '/')
                {
                    nhanPhanDoan += '/';
                }
                else if (nhanPhanDoan == "/")
                {
                    nhanNam1 += character.ToString();
                }
                else
                {
                    nhanThang1 += character.ToString();
                }
            }

            int thang = int.Parse(nhanThang1);
            int nam = int.Parse(nhanNam1);


            // Lấy ngày đầu tiên của tháng hiện tại
            DateTime firstDayOfMonth = new DateTime(nam, thang, 1);

            // Lấy số ngày trong tháng
            int daysInMonth = DateTime.DaysInMonth(firstDayOfMonth.Year, firstDayOfMonth.Month);

            ngayBieuDo = new List<string>();
            doanhthuBieuDo = new List<float>();

            // Thêm số ngày vào DataTable
            for (int i = 1; i <= daysInMonth; i++)
            {

                string ngaythangnamw = i + "/" + firstDayOfMonth.Month.ToString() + "/" + firstDayOfMonth.Year.ToString();


                // Thêm dữ liệu vào ô cụ thể
                ngayBieuDo.Add(ngaythangnamw);
                TachNgayThangNamBieuDo(ngaythangnamw);

                

                if (varHoanhThuBieuDo != 0)
                {
                    string formattientes = ((int.Parse(varHoanhThuBieuDo.ToString().Replace(".", "") + "000"))).ToString("N2");

                    doanhthuBieuDo.Add(float.Parse(formattientes));
                }
                else doanhthuBieuDo.Add(varHoanhThuBieuDo);

            }
        }



        // theo Tháng

        private void TachNgayThangNamBieuDoThang(string ngaythangVaNam)
        {
            string nhanPhanDoan = "";
            string nhanNam1 = "";
            string nhanThang1 = "";

            foreach (char character in ngaythangVaNam)
            {
                if (character == '/')
                {
                    nhanPhanDoan += '/';
                }
                else if (nhanPhanDoan == "/")
                {
                    nhanNam1 += character.ToString();
                }
                else
                {
                    nhanThang1 += character.ToString();
                }
            }


            LoadDataSQLTheoNgayBieuDoThang(int.Parse(nhanNam1), int.Parse(nhanThang1));
            //TinhVaXuatDuLieu(int.Parse(nhanNam1), int.Parse(nhanThang1), int.Parse(nhanNgay1));

        }

        private void LoadDataSQLTheoNgayBieuDoThang(int nhanNam, int nhanThang)
        {
            //Console.WriteLine($"thang -->:{nhanNgay} - {nhanThang} - {nhanNam}");

            string sumGia = "";

            using (sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open(); // Mở kết nối

                // Tạo câu truy vấn SQL
                string query11 = "SELECT SUM(TongGiaSanPham) AS TotalB FROM QL_HoaDon " +
                    "WHERE DATEPART(MONTH, NgayLapHoaDon) = " + nhanThang + " " +
                    "AND DATEPART(YEAR, NgayLapHoaDon) = " + nhanNam + ";";

                // Tạo đối tượng Command và thi hành câu truy vấn
                using (SqlCommand command = new SqlCommand(query11, sqlConnection))
                {
                    // Đọc kết quả từ truy vấn
                    object nhanvv = command.ExecuteScalar();
                    sumGia = nhanvv.ToString();
                }
            }

            if (sumGia != "")
            {
                varHoanhThuBieuDoThang = float.Parse(sumGia);
                Math.Round((double)varHoanhThuBieuDoThang, 3);
            }
            else varHoanhThuBieuDoThang = 0;

        }

        private void TinhVaXuatDuLieuBieuDoThang(string thangnam)
        {


            doanhthuBieuDoThang = new List<float>();
            // Thêm số ngày vào DataTable
            for (int i = 1; i <= 12; i++)
            {

                string ngaythangnamw = i + "/" + thangnam;

                

                // Thêm dữ liệu vào ô cụ thể
                //ngayBieuDoThang.Add(ngaythangnamw);
                TachNgayThangNamBieuDoThang(ngaythangnamw);



                if (varHoanhThuBieuDo != 0)
                {
                    string formattientes = ((int.Parse(varHoanhThuBieuDoThang.ToString().Replace(".", "") + "000"))).ToString();

                    Console.WriteLine("mmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmm: "+formattientes);
                    doanhthuBieuDoThang.Add(float.Parse(formattientes));

                }
                else doanhthuBieuDoThang.Add(varHoanhThuBieuDoThang);

            }
        }
    }
}


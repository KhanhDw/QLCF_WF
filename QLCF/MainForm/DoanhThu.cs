using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

namespace QLCF
{
    public partial class DoanhThu : UserControl
    {
        private DateTime ngayThangHienTai;

        private SqlConnection sqlConnection = null;// đối tượng kết nối
        //string connectionString = "Data Source=DESKTOP-39BMSC8\\GIAKHANH;Initial Catalog=QL_QuanCF;Integrated Security=True";
        string connectionString = LoadDatabase.connectionString;

        public static DoanhThu instanceDoanhThu;

        public DoanhThu()
        {
            InitializeComponent();
            instanceDoanhThu = this;
            createTable();

        }

        public void DoanhThu_Load(object sender, EventArgs e)
        {
            // Khởi tạo ngày tháng hiện tại
            ngayThangHienTai = DateTime.Now;
            thangHienTai();

            //LoadDataSQLTheoNgay("2/2023");
        }


        DataTable dataTable = new System.Data.DataTable();
        private void createTable()
        {
            // Tạo DataTable để lưu trữ dữ liệu
            
            dataTable.Columns.Add("Ngày", typeof(string));
            dataTable.Columns.Add("Số Lượng Hóa Đơn", typeof(int));
            dataTable.Columns.Add("Số Lượng Ly Đã Bán", typeof(int));
            dataTable.Columns.Add("Tổng Doanh Thu", typeof(string));


            // Điều chỉnh header để ở giữ
            DataGridViewCellStyle columnHeaderStyle = new DataGridViewCellStyle();
            columnHeaderStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView_DoanhThu.ColumnHeadersDefaultCellStyle = columnHeaderStyle;


            dataGridView_DoanhThu.ScrollBars = ScrollBars.Vertical; // Ẩn thanh cuộn ngan

            // Điều chỉnh độ rộng của cột tự động lấp đầy bảng
            dataGridView_DoanhThu.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Điều chỉnh chiều cao của dòng header tự động
            dataGridView_DoanhThu.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            // Điều chỉnh chiều rộng cho tất cả các ô trong dòng
            dataGridView_DoanhThu.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            // Chỉnh định dạng cho font của header
            dataGridView_DoanhThu.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 12, FontStyle.Bold);

            // Chỉnh định dạng cho dữ liệu DataGridView
            dataGridView_DoanhThu.DefaultCellStyle.Font = new Font("Tahoma", 10, FontStyle.Regular);

            dataGridView_DoanhThu.AllowUserToAddRows = false;


            // Điều chỉnh dữ liệu để giữ
            DataGridViewCellStyle dataCellStyle = new DataGridViewCellStyle();
            dataCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // Áp dụng điều chỉnh cho tất cả các cột
            foreach (DataGridViewColumn column in dataGridView_DoanhThu.Columns)
            {
                column.DefaultCellStyle = dataCellStyle;
            }


            

        }



        private string nhanNgay = "";
        string nhanThang = "";
        string nhanNam = "";
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
                    "WHERE DATEPART(DAY, NgayLapHoaDon) = '" + nhanNgay + "' " +
                    "AND DATEPART(MONTH, NgayLapHoaDon) = '" + nhanThang + "' " +
                    "AND DATEPART(YEAR, NgayLapHoaDon) = '" + nhanNam + "';";

                // Tạo đối tượng Command và thi hành câu truy vấn
                using (SqlCommand command = new SqlCommand(query1, sqlConnection))
                {
                    // Đọc kết quả từ truy vấn
                    object nhanvv = command.ExecuteScalar();
                    sumLy = nhanvv.ToString();
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

        }

        private void TinhVaXuatDuLieu(string thangnam)
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

            // Thêm số ngày vào DataTable
            for (int i = 1; i <= daysInMonth; i++)
            {

                string ngaythangnamw = i + "/" + firstDayOfMonth.Month.ToString() + "/" + firstDayOfMonth.Year.ToString();
                
                // Tạo một hàng mới
                DataRow newRow = dataTable.NewRow();

                // Thêm dữ liệu vào ô cụ thể
                newRow["Ngày"] = ngaythangnamw;
                TachNgayThangNam(ngaythangnamw);

                newRow["Số Lượng Hóa Đơn"] = varhoadon; 
                newRow["Số Lượng Ly Đã Bán"] = varSoLy; 

                if (varHoanhThu != 0)
                {
                    string formattientes = ((int.Parse(varHoanhThu.ToString().Replace(".", "") + "000"))).ToString("N2");

                    // Loại bỏ phần thập phân
                    int indexOfDecimalSeparator = formattientes.IndexOf('.');
                    string formattedAmountWithoutDecimals = (indexOfDecimalSeparator >= 0)
                        ? formattientes.Substring(0, indexOfDecimalSeparator)
                        : formattientes;

                    formattientes = formattedAmountWithoutDecimals;
                    
                    newRow["Tổng Doanh Thu"] = formattientes;
                }
                else newRow["Tổng Doanh Thu"] = varHoanhThu;

                // Thêm hàng vào DataTable
                dataTable.Rows.Add(newRow);
            }

            // Gán DataTable cho DataGridView
            dataGridView_DoanhThu.DataSource = dataTable;

        }

       
        public void responsive_DoanhThu(int newWidthForm)
        {
            
            if (newWidthForm == 1920)
            {
                pnlReponsive.Size = new Size(445+110, 100);
                pnlContainTableDoanhThu.Size = new Size(1389+300, 589+265);
                
            }
            else if (newWidthForm == 1615)
            {
                pnlReponsive.Size = new Size(445, 100);
                pnlContainTableDoanhThu.Size = new Size(1389, 589);
                
            }

        }

        private void SearchInDataGridView(string searchText)
        {

            int hoadonLable = 0;
            int solyLable = 0;
            float doanhthuLable = 0;

            foreach (DataGridViewRow row in dataGridView_DoanhThu.Rows)
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
                CurrencyManager currencyManager1 = (CurrencyManager)BindingContext[dataGridView_DoanhThu.DataSource];
                currencyManager1.SuspendBinding();
                row.Visible = false;
                currencyManager1.ResumeBinding();

                // Hiển thị hoặc ẩn dòng tùy thuộc vào kết quả tìm kiếm
                row.Visible = rowVisible;

                if (rowVisible)
                {
                    DataGridViewCell cellA = row.Cells[1]; // hoa don   
                    DataGridViewCell cellB = row.Cells[2]; // so ly
                    DataGridViewCell cellC = row.Cells[3]; // doanh thu

                    if (cellA.Value != null)
                    {
                        hoadonLable += int.Parse(cellA.Value.ToString());
                    }

                    if (cellB.Value != null)
                    {
                        solyLable += int.Parse(cellB.Value.ToString());
                    }

                    if (cellC.Value != null)
                    {
                        doanhthuLable += float.Parse(cellC.Value.ToString().Replace(".", ""));
                    }
                }
            }

            lbSLHoaDon.Text = hoadonLable.ToString();
            lbLy.Text = solyLable.ToString();
            lbTongDoanhThu.Text = doanhthuLable.ToString("N2"); // định dạng tiền tệ    

            // Loại bỏ phần thập phân
            int indexOfDecimalSeparator = lbTongDoanhThu.Text.IndexOf('.');
            string formattedAmountWithoutDecimals = (indexOfDecimalSeparator >= 0)
                ? lbTongDoanhThu.Text.Substring(0, indexOfDecimalSeparator)
                : lbTongDoanhThu.Text;

             lbTongDoanhThu.Text =formattedAmountWithoutDecimals;
        }


        private void btnAll_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView_DoanhThu.Rows)
            {
                // Hiển thị dòng 
                row.Visible = true;
            }
        }

        private void thangHienTai()
        {

            // Giả sử DataGridView1.DataSource là một DataTable
            if (dataGridView_DoanhThu.DataSource is DataTable dataTable)
            {
                // Xóa tất cả dữ liệu từ DataTable
                dataTable.Clear();

                // Hoặc nếu bạn muốn xóa cả cấu trúc (các cột) của DataTable
                // dataTable.Reset();
            }

            string thangFormat = ngayThangHienTai.ToString("MM/yyyy");
            lbThang.Text = thangFormat;
            SearchInDataGridView(thangFormat);

            TinhVaXuatDuLieu(thangFormat);
            SearchInDataGridView(thangFormat);


        }

        private void btnTang_Click(object sender, EventArgs e)
        {
            // Tăng thêm 1 tháng
            ngayThangHienTai = ngayThangHienTai.AddMonths(1);
            thangHienTai();
        }

        private void btnGiam_Click(object sender, EventArgs e)
        {
            // Giảm đi 1 tháng
            ngayThangHienTai = ngayThangHienTai.AddMonths(-1);
            // Hiển thị ngày tháng sau khi tăng Giảm 1 tháng
            thangHienTai();
            string date = lbThang.Text;
            SearchInDataGridView(date);
        }

    }
}

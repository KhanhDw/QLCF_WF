using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Text;
using System.Globalization;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace QLCF.NhanVienForm
{
    public partial class User_DatBan : UserControl
    {
        private SqlConnection sqlConnection = null;// đối tượng kết nối
        //string connectionString = "Data Source=DESKTOP-39BMSC8\\GIAKHANH;Initial Catalog=QL_QuanCF;Integrated Security=True";
        string connectionString = LoadDatabase.connectionString;
        string maDatBanSQL = string.Empty;


        public static User_DatBan instanceDatBan;


        public User_DatBan()
        {
            InitializeComponent();
            instanceDatBan = this;
        }

        public void User_DatBan_Load(object sender, EventArgs e)
        {
            ThemDuLieuVaoDataGridView();
            LoadDataSQL();
            DieeChinhGioChoDateTimePicker();
            LoadMaBan(); // tải dữ liệu mã bàn cho combobox chỗ chọn bàn
            //DemSoLuongDonDatBan();
            AutoClickButton();

        }





        private void ThemDuLieuVaoDataGridView()
        {
            dataGridView_DatBan.ScrollBars = ScrollBars.Vertical; // Ẩn thanh cuộn ngan

            // Điều chỉnh độ rộng của cột tự động lấp đầy bảng
            dataGridView_DatBan.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Điều chỉnh chiều cao của dòng header tự động
            dataGridView_DatBan.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            // ngăn dòng cuối rỗng hiện ra và cũng không cho người dùng nhập dữ liệu trực tiếp vào bảng
            dataGridView_DatBan.AllowUserToAddRows = false;


        }

        private void LoadDataSQL()
        {
            using (sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open(); // Mở kết nối

                // Truy vấn SQL để lấy dữ liệu từ bảng
                string query = "\r\nSELECT QL_DatCho.MaDatCho,\r\n\t\tQL_DatCho.IDKhachHang,\r\n\t\tQL_KhachHang.MaKhachHang,\r\n\t\tQL_KhachHang.TenKhachHang,\r\n\t\tQL_KhachHang.SoDienThoai,\r\n\t\tQL_DatCho.SoLuongKhach,\r\n\t\tQL_DatCho.MaBan,\r\n\t\tQL_DatCho.ThoiGian\r\nFROM QL_DatCho , QL_KhachHang , QL_Ban\r\nWhere QL_DatCho.IDKhachHang = QL_KhachHang.IDKhachHang \r\n\t\tand\r\n\t\tQL_DatCho.MaBan = QL_Ban.MaBan;\r\n\t\t";
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
                        dataGridView_DatBan.DataSource = dataTable;


                        dataGridView_DatBan.Columns["MaDatCho"].HeaderText = "Mã Đặt Bàn";
                        dataGridView_DatBan.Columns["IDKhachHang"].HeaderText = "IDKH";
                        dataGridView_DatBan.Columns["MaKhachHang"].HeaderText = "Mã KH";
                        dataGridView_DatBan.Columns["TenKhachHang"].HeaderText = "Tên KH";
                        dataGridView_DatBan.Columns["SoDienThoai"].HeaderText = "SĐT";
                        dataGridView_DatBan.Columns["SoLuongKhach"].HeaderText = "SL";
                        dataGridView_DatBan.Columns["MaBan"].HeaderText = "Mã bàn";
                        dataGridView_DatBan.Columns["ThoiGian"].HeaderText = "Thời Gian";


                        //// Đặt căn giữ cho header của cột trong DataGridView
                        dataGridView_DatBan.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dataGridView_DatBan.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dataGridView_DatBan.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dataGridView_DatBan.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dataGridView_DatBan.Columns[4].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dataGridView_DatBan.Columns[5].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dataGridView_DatBan.Columns[6].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dataGridView_DatBan.Columns[7].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

                        //// Đặt căn giữ cho cột trong dữ liệu trong DataGridView
                        dataGridView_DatBan.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                        dataGridView_DatBan.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dataGridView_DatBan.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dataGridView_DatBan.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                        dataGridView_DatBan.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dataGridView_DatBan.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dataGridView_DatBan.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dataGridView_DatBan.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                        dataGridView_DatBan.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 7, FontStyle.Bold);
                        dataGridView_DatBan.DefaultCellStyle.Font = new Font("Tahoma", 7, FontStyle.Regular);
                        //dataGridView_DatBan.AllowUserToAddRows = false;
                    }
                }

            }

            //DemSoLuongDonDatBan();

        }


        private void LoadMaBan()
        {
            using (sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                // Truy vấn SQL để lấy dữ liệu từ cơ sở dữ liệu
                string queryTruyenDataCBB = "SELECT MaBan FROM QL_Ban";
                using (SqlCommand command = new SqlCommand(queryTruyenDataCBB, sqlConnection))
                {
                    SqlDataReader reader = command.ExecuteReader();
                    // Đọc dữ liệu và thêm vào ComboBox
                    while (reader.Read())
                    {
                        string value = reader["MaBan"].ToString();
                        cbbMadatban.Items.Add(value);
                    }
                }
            }
        }



        public void responsive_DatBan(int newWidthForm)
        {

            if (newWidthForm == 1920)
            {
                pnlReponsiveDatBan.Size = new Size(1395 + 310, 114);
                pnlContainTableDatBan.Size = new Size(1389 + 300, 589 + 330);
            }
            else
            {
                pnlReponsiveDatBan.Size = new Size(1615-320, 114);
                pnlContainTableDatBan.Size = new Size(1615 - 320, 589+100);

            }

        }

        private void btnThemKhachHang_Click(object sender, EventArgs e)
        {
            if ( txtTenKH.Text == "" || txtSDT.Text == "" || txtSoluong.Text == "" ||  txtDateBook.Text == "")
            {
                MessageBox.Show("vui lòng nhập thông tin", "thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }else if (txtSDT.Text.Length > 10)
            {
                MessageBox.Show("Số điện thoại phải 10!!", "thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                // Tạo đối tượng SqlConnection
                using (sqlConnection = new SqlConnection(connectionString))
                {
                    // Mở kết nối
                    sqlConnection.Open();


                    //----Lấy số ID lớn nhất trong bảng khách hàng
                    int strIdKhachHang;
                    object maxValueIDKhachHang;
                    string queryMaxIDKhachHang = $"SELECT MAX(IDKhachHang) AS MaxValue FROM QL_KhachHang";
                    // Tạo và thực thi đối tượng Command
                    using (SqlCommand command = new SqlCommand(queryMaxIDKhachHang, sqlConnection))
                    {
                        // Đọc giá trị lớn nhất từ kết quả truy vấn
                        maxValueIDKhachHang = command.ExecuteScalar();
                    }
                    strIdKhachHang = int.Parse(maxValueIDKhachHang.ToString()) + 1;

                    //----Lấy số Mã khách hàng lớn nhất trong bảng khách hàng
                    string strMaKhachHang;
                    object maxValueMaKhachHang;
                    string queryMaKhachHang = "SELECT TOP 1 MaKhachHang FROM QL_KhachHang\r\nORDER BY MaKhachHang DESC;";
                    // Tạo và thực thi đối tượng Command
                    using (SqlCommand command = new SqlCommand(queryMaKhachHang, sqlConnection))
                    {
                        // Đọc giá trị lớn nhất từ kết quả truy vấn
                        maxValueMaKhachHang = command.ExecuteScalar();
                    }
                    strMaKhachHang = maxValueMaKhachHang.ToString();
                    strMaKhachHang = "KH" + (int.Parse(strMaKhachHang.Substring(2)) + 1).ToString();



                    //----thêm dữ liệu khách hàng vào bảng khách hàng
                    string insertQueryKH = "INSERT INTO QL_KhachHang (IDKhachHang, MaKhachHang, TenKhachHang, SoDienThoai) VALUES (@IDKhachHang, @MaKhachHang, @TenKhachHang,@SoDienThoai)";

                    using (SqlCommand cmd = new SqlCommand(insertQueryKH, sqlConnection))
                    {
                        cmd.Parameters.AddWithValue("@IDKhachHang", strIdKhachHang);
                        cmd.Parameters.AddWithValue("@MaKhachHang", strMaKhachHang);
                        cmd.Parameters.AddWithValue("@TenKhachHang", txtTenKH.Text);
                        cmd.Parameters.AddWithValue("@SoDienThoai", txtSDT.Text);

                        cmd.ExecuteNonQuery();
                    }


                    //----lấy giá trị lớn nhất trong bảng đặt bàn
                    string strMaDatBan;
                    object maxValueMaDatBan;
                    string queryMaDatBan = "SELECT TOP 1 MaDatCho FROM QL_DatCho\r\nORDER BY MaDatCho DESC;";
                    // Tạo và thực thi đối tượng Command
                    using (SqlCommand command = new SqlCommand(queryMaDatBan, sqlConnection))
                    {
                        // Đọc giá trị lớn nhất từ kết quả truy vấn
                        maxValueMaDatBan = command.ExecuteScalar();
                    }
                    strMaDatBan = maxValueMaDatBan.ToString();


                    int intMadatcho = int.Parse(strMaDatBan.Substring(2)) + 1;



                    string tamo1 = "000";

                    if (intMadatcho > 9)
                    {
                        tamo1 = "00";
                    }
                    else if (intMadatcho > 99)
                    {
                        tamo1 = "0";
                    }
                    else if (intMadatcho > 999)
                    {
                        tamo1 = "";
                    }


                    strMaDatBan = "DC" + tamo1 + intMadatcho.ToString();




                    //
                    string insertQueryHD = "INSERT INTO QL_DatCho (MaDatCho, IDKhachHang, MaBan, SoLuongKhach, ThoiGian) VALUES (@MaDatCho, @IDKhachHang, @MaBan, @SoLuongKhach, @ThoiGian)";

                    using (SqlCommand cmd = new SqlCommand(insertQueryHD, sqlConnection))
                    {
                        cmd.Parameters.AddWithValue("@MaDatCho", strMaDatBan.Trim());
                        cmd.Parameters.AddWithValue("@IDKhachHang", strIdKhachHang);
                        cmd.Parameters.AddWithValue("@MaBan", cbbMadatban.SelectedItem.ToString().Trim());
                        cmd.Parameters.AddWithValue("@SoLuongKhach", int.Parse(txtSoluong.Text.Trim()));
                        cmd.Parameters.AddWithValue("@ThoiGian", txtDateBook.Text + " "+ dateGioDatBan.Value.ToString("HH:mm"));
                        cmd.ExecuteNonQuery();
                    }


                    //txtDatcho.Clear();
                    //txtMaKH.Clear();
                    txtTenKH.Clear();
                    txtSDT.Clear();
                    txtSoluong.Clear();
                    cbbMadatban.SelectedItem = "Chọn Bàn";
                    txtDateBook.Clear();
                    //txtMaKH.Focus();
                    LoadDataSQL();
                }

            }
            btnHomNay_Click(this, EventArgs.Empty);
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
                    string deleteQuery = "DELETE FROM QL_DatCho WHERE MaDatCho = '" + (dataGridView_DatBan.SelectedRows[0].Cells[0].Value).ToString() + "'";

                    using (SqlCommand command = new SqlCommand(deleteQuery, sqlConnection))
                    {
                        // Thực thi câu lệnh SQL
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            dataGridView_DatBan.Rows.RemoveAt(dataGridView_DatBan.SelectedRows[0].Index);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có
                MessageBox.Show("Error: " + ex.Message);
            }
            txtTenKH.Clear();
            txtSDT.Clear();
            txtSoluong.Clear();
            txtDateBook.Clear();
            LoadDataSQL();
            btnHomNay_Click(this, EventArgs.Empty);
        }

        private void btnSuaKhachHang_Click(object sender, EventArgs e)
        {
            if (dataGridView_DatBan.SelectedRows.Count > 0)
            {
                //dataGridView_TaiKhoan.SelectedRows[0].Cells[0].Value = txtma.Text;
                //dataGridView_TaiKhoan.SelectedRows[0].Cells[1].Value = txtmatkhau.Text;
                //dataGridView_TaiKhoan.SelectedRows[0].Cells[2].Value = txtnhaplaimk.Text;

                if (txtTenKH.Text == "" || txtSDT.Text == "" || txtSoluong.Text == "" ||  txtDateBook.Text == "")
                {
                    MessageBox.Show("vui lòng nhập đầy đủ thông tin", "thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
                else
                {


                    //// Tạo đối tượng SqlConnection
                    using (sqlConnection = new SqlConnection(connectionString))
                    {
                        // Mở kết nối
                        sqlConnection.Open();

                        int idkhachhang = int.Parse(dataGridView_DatBan.SelectedRows[0].Cells[1].Value.ToString());


                        string deleteQuery = "UPDATE QL_KhachHang SET TenKhachHang = N'" + txtTenKH.Text + "' , SoDienThoai = '" + txtSDT.Text + "' WHERE IDKhachHang = '" + idkhachhang + "';";

                        using (SqlCommand command = new SqlCommand(deleteQuery, sqlConnection))
                        {
                            // Thực thi câu lệnh SQL
                            int rowsAffected = command.ExecuteNonQuery();

                            //if (rowsAffected > 0)
                            //{
                            //    dataGridView_DatBan.Rows.RemoveAt(dataGridView_DatBan.SelectedRows[0].Index);
                            //}
                        }
                        

                        if (maDatBanSQL != null)
                        {
                            string editDatbanQuery = "UPDATE QL_DatCho SET MaBan = '" + cbbMadatban.SelectedItem.ToString() + "' , SoLuongKhach = " + int.Parse(txtSoluong.Text) + " , ThoiGian = '" + "2023-10-15 14:00:00.000" + "' WHERE MaDatCho = '" + maDatBanSQL + "';";
                        

                            using (SqlCommand command = new SqlCommand(editDatbanQuery, sqlConnection))
                            {
                                // Thực thi câu lệnh SQL
                                int rowsAffected = command.ExecuteNonQuery();

                                //if (rowsAffected > 0)
                                //{
                                //    dataGridView_DatBan.Rows.RemoveAt(dataGridView_DatBan.SelectedRows[0].Index);
                                //}

                            }
                        }

                        maDatBanSQL = string.Empty;
                    }


                    
                    
                }


            }
            LoadDataSQL();
            txtTenKH.Clear();
            txtSDT.Clear();
            txtSoluong.Clear();
            txtDateBook.Clear();
            txtTenKH.Focus();
            btnHomNay_Click(this, EventArgs.Empty);
        }
        //
        private void SearchInDataGridView(string searchText)
        {
            int sodondatban = 0;


            foreach (DataGridViewRow row in dataGridView_DatBan.Rows)
            {
                row.Selected = false;
            }

            foreach (DataGridViewRow row in dataGridView_DatBan.Rows)
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
                CurrencyManager currencyManager1 = (CurrencyManager)BindingContext[dataGridView_DatBan.DataSource];
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
                lbSoluongDatBan.Text = string.Empty;

                lbSoluongDatBan.Text = sodondatban.ToString();
            }
        }
        //
        private void btnSearch_Click(object sender, EventArgs e)
        {
            string textSearch = txtSearch.Text;
            SearchInDataGridView(textSearch);
        }
       
        private void dateNgaySinh_ValueChanged(object sender, EventArgs e)
        {

            // Lấy giá trị ngày giờ từ DateTimePicker
            DateTime selectedDateTime = dateBook.Value;

            // Định dạng chuỗi theo ý muốn, ở đây là ngày giờ đầy đủ
            string formattedDateTime = selectedDateTime.ToString("dd/MM/yyyy");

            // Gán chuỗi đã định dạng vào Label
            txtDateBook.Text = formattedDateTime;
        }

        private void AutoClickButton()
        {
            // Gọi phương thức xử lý sự kiện của Button
            btnHomNay_Click(this, EventArgs.Empty);
            btnHomnayThem_Click(this, EventArgs.Empty);

            DateTime currentDate = DateTime.Now;
            lb_NgayBatDau.Text = currentDate.ToString("dd/MM/yyyy");

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

            string dateFinish = ngay + "/" + thangs + "/" + nam;



            SearchInDataGridView(dateFinish);
            
        }

        private void dateTimePicker_NgayDat_ValueChanged(object sender, EventArgs e)
        {
            // Lấy giá trị ngày giờ từ DateTimePicker
            DateTime selectedDateTime = dateTimePicker_NgayDat.Value;

            // Định dạng chuỗi theo ý muốn, ở đây là ngày giờ đầy đủ
            string formattedDateTime = selectedDateTime.ToString("dd/MM/yyyy");

            // Gán chuỗi đã định dạng vào Label
            lb_NgayBatDau.Text = formattedDateTime;

            string yearSearch = formattedDateTime;

            SearchInDataGridView(yearSearch);
        }

       

        

        private void btnHomnayThem_Click(object sender, EventArgs e)
        {
            // Lấy ngày và giờ hiện tại
            DateTime currentDate = DateTime.Now;
            txtDateBook.Text = currentDate.ToString("dd/MM/yyyy");
            
        }

        private void btnNextDate_Click(object sender, EventArgs e)
        {
            // Lấy giá trị hiện tại từ DateTimePicker
            DateTime currentDate = dateTimePicker_NgayDat.Value;

            // Tăng giá trị ngày lên 1 ngày để chuyển đến ngày tiếp theo
            DateTime nextDate = currentDate.AddDays(1);

            // Thiết lập giá trị mới cho DateTimePicker
            dateTimePicker_NgayDat.Value = nextDate;


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

            string dateFinish = ngay + "/" + thangs + "/" + nam;

            SearchInDataGridView(dateFinish);
        }

        private void btnLastDate_Click(object sender, EventArgs e)
        {
            // Lấy giá trị hiện tại từ DateTimePicker
            DateTime currentDate = dateTimePicker_NgayDat.Value;

            // Tăng giá trị ngày lên 1 ngày để chuyển đến ngày tiếp theo
            DateTime nextDate = currentDate.AddDays(-1);

            // Thiết lập giá trị mới cho DateTimePicker
            dateTimePicker_NgayDat.Value = nextDate;


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

            string dateFinish = ngay + "/" + thangs + "/" + nam;

            SearchInDataGridView(dateFinish);
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            //// Lọc dữ liệu trực tiếp trên DataGridView
           string searchText = txtSearch.Text;

            SearchInDataGridView(searchText);
        }

        private void DieeChinhGioChoDateTimePicker()
        {
            // Đặt Format của DateTimePicker thành Custom
            dateGioDatBan.Format = DateTimePickerFormat.Custom;

            // Đặt CustomFormat để chỉ hiển thị giờ và phút
            dateGioDatBan.CustomFormat = "h:mm:ss tt";

            // Hiển thị mũi tên điều chỉnh giờ và phút
            dateGioDatBan.ShowUpDown = true;
        }


        private void dataGridView_DatBan_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // Lấy dữ liệu từ dòng được chọn
                DataGridViewRow row = dataGridView_DatBan.Rows[e.RowIndex];

                txtTenKH.Text = row.Cells[3].Value.ToString();
                txtSDT.Text = row.Cells[4].Value.ToString();
                txtSoluong.Text = row.Cells[5].Value.ToString();

                if (cbbMadatban.Items.Contains(row.Cells[6].Value.ToString()))
                {
                    int index = cbbMadatban.Items.IndexOf(row.Cells[6].Value.ToString());
                    cbbMadatban.SelectedIndex = index;
                }

                maDatBanSQL = row.Cells[0].Value.ToString();

                string timenaddate = row.Cells[7].Value.ToString();

                string[] ngayvagio = timenaddate.Split(' ');

                //txtDateBook.Text =datePart.ToString("MM/dd/yyyy");
                txtDateBook.Text = ngayvagio[0];

                //// Điều chỉnh giao diện của DateTimePicker
                DieeChinhGioChoDateTimePicker(ngayvagio[1] + " " + ngayvagio[2]);
            }
        }

        private void DieeChinhGioChoDateTimePicker(string gioNhanDuoc)
        {

            // Chuyển đổi chuỗi thành DateTime sử dụng định dạng tùy chỉnh
            DateTime parsedTime = DateTime.ParseExact(gioNhanDuoc, "h:mm:ss tt", null);

            // Đặt giá trị của DateTimePicker
            dateGioDatBan.Value = parsedTime;
        }


        private void DemSoLuongDonDatBan()
        {
            string query = "SELECT COUNT(*) AS TotalRows FROM QL_DatCho";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Thực hiện truy vấn và lấy giá trị đếm
                    int rowCount = (int)command.ExecuteScalar();

                    // Gán giá trị đếm cho lable
                    lbSoluongDatBan.Text = rowCount.ToString();

                }
            }
        }

        private void btnAll_Click(object sender, EventArgs e)
        {
            DemSoLuongDonDatBan();

            foreach (DataGridViewRow row in dataGridView_DatBan.Rows)
            {
                // Hiển thị dòng 
                row.Visible = true;
            }
        }
    }
    
}

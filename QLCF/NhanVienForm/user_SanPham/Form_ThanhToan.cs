using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Runtime.InteropServices.ComTypes;
using static QLCF.NhanVienForm.User_Donban;
using System.Windows.Forms.DataVisualization.Charting;
using System.Data.SqlClient;


namespace QLCF.NhanVienForm.user_SanPham
{
    //* nơi lấy dữ liệu  và truyền dũ liệu(phải để ngoài class và trong namesapace)
    //public delegate void AddDataDelegate(string data);


    public partial class Form_ThanhToan : Form
    {
        private SqlConnection sqlConnection = null;// đối tượng kết nối
        string connectionString = "Data Source=DESKTOP-39BMSC8\\GIAKHANH;Initial Catalog=QL_QuanCF;Integrated Security=True";

        private static Form_ThanhToan _instance_Form_ThanhToan;

        public string DataFromChildForm { get; set; }
        // Sự kiện để thông báo rằng dữ liệu đã thay đổi
        public event Action<string> DataSend;
        public event EventHandler<string> EvSend;

        // Biến để giữ lại dữ liệu trước khi form đóng
        private string dataToSend;

        public string NhanDuLieuThanhToan;
        string giabandau;

        List<string> dataDonBan;

        public List<string> _dataDonBan
        {
            get { return dataDonBan; }
            set { dataDonBan = value; }
        }

        int indexLuotMua;
        int sdtTemp = 1000000000;
        int soluongHoaDon;
        int strIdKhachHang;
        string strSDTKhachHang;
       

        public Form_ThanhToan()
        {
            InitializeComponent();
            _instance_Form_ThanhToan = this;
            createTabel();
            // Gắn sự kiện TextChanged cho cả hai TextBox
            //txtNameKhachHang.TextChanged += giamGiaChoThongTinKH;
            //txtSdtKhachHang.TextChanged += giamGiaChoThongTinKH;
            // khởi tạo giá ban đầu khi nhận dữ liệu từ User_DonBan
            giabandau = lbSoTienCanThanhToan.Text;
        }
        

        public virtual void SendataUserControlSELL(string variable)
        {
            EvSend?.Invoke(this, variable);
        }

       


        private void Form_ThanhToan_Load(object sender, EventArgs e)
        {
            giabandau = lbSoTienCanThanhToan.Text;
            indexLuotMua = TruyenData.Instance._LuotMuaHang;
            LoadDataSQL_UserDonBan();
            TenNhanVienThanhToan();
        }

        private void TenNhanVienThanhToan()
        {
            string maNV = TruyenData.Instance._LoginTK;
            lbTenThuNgan.Text = maNV;
        }

        private void LoadDataSQL_UserDonBan()
        {
            using (sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open(); // Mở kết nối

                object maxValueIDHoaDon;

                string query = $"SELECT MAX(IDHoaDon) AS MaxValue FROM QL_HoaDon";

                // Tạo và thực thi đối tượng Command
                using (SqlCommand command = new SqlCommand(query, sqlConnection))
                {
                    // Đọc giá trị lớn nhất từ kết quả truy vấn
                    maxValueIDHoaDon = command.ExecuteScalar();
                }


                soluongHoaDon = int.Parse(maxValueIDHoaDon.ToString());

                //----
                object maxValueIDKhachHang;

                string queryMaxIDKhachHang = $"SELECT MAX(IDKhachHang) AS MaxValue FROM QL_KhachHang";

                // Tạo và thực thi đối tượng Command
                using (SqlCommand command = new SqlCommand(queryMaxIDKhachHang, sqlConnection))
                {
                    // Đọc giá trị lớn nhất từ kết quả truy vấn
                    maxValueIDKhachHang = command.ExecuteScalar();
                }


                strIdKhachHang = int.Parse(maxValueIDKhachHang.ToString());


                //----
                string queryMaxSDTKhachHang = $"SELECT MAX(SoDienThoai) AS MaxValue FROM QL_KhachHang";

                // Tạo và thực thi đối tượng Command
                using (SqlCommand command = new SqlCommand(queryMaxSDTKhachHang, sqlConnection))
                {
                    // Đọc giá trị lớn nhất từ kết quả truy vấn
                    maxValueIDKhachHang = command.ExecuteScalar();
                }

                strSDTKhachHang = maxValueIDKhachHang.ToString();



                // Thực hiện truy vấn SQL để lấy tổng giá trị từ cột
                string query1 = "SELECT SUM(SoLuongSanPham) FROM QL_HoaDon";

                using (SqlCommand cmd = new SqlCommand(query1, sqlConnection))
                {
                    // Thực hiện truy vấn và nhận kết quả
                    int soluongLy = (int)cmd.ExecuteScalar();
                }

            }
        }



        //khởi tạo table
        private void createTabel()
        {
            // Đặt tên cột <Biến tên cột>,<name cột>
            gridFormthanhtoan.Columns.Add("colmaMon", "Mã món");
            gridFormthanhtoan.Columns.Add("colnameMon", "Tên món");
            gridFormthanhtoan.Columns.Add("colslBan", "SL");
            gridFormthanhtoan.Columns.Add("coldongia", "Đơn giá");
            gridFormthanhtoan.Columns.Add("colthanhtien", "Thành tiền");

            // Đặt căn giữ cho header của cột thứ 2 (index 1) trong DataGridView
            gridFormthanhtoan.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            gridFormthanhtoan.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            gridFormthanhtoan.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            gridFormthanhtoan.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            gridFormthanhtoan.Columns[4].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;


            // Đặt căn giữ cho cột trong DataGridView
            gridFormthanhtoan.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            gridFormthanhtoan.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            gridFormthanhtoan.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            gridFormthanhtoan.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            gridFormthanhtoan.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // Điều chỉnh độ rộng của cột tự động lấp đầy bảng
            gridFormthanhtoan.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Điều chỉnh chiều cao của dòng header tự động
            gridFormthanhtoan.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            // Chỉnh định dạng cho font của header
            gridFormthanhtoan.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 11, FontStyle.Bold);

            // Chỉnh định dạng cho dữ liệu DataGridView
            gridFormthanhtoan.DefaultCellStyle.Font = new Font("Tahoma", 10, FontStyle.Regular);

            // Tắt viền cho toàn bộ DataGridView
            gridFormthanhtoan.CellBorderStyle = DataGridViewCellBorderStyle.None;

            //gridFormthanhtoan.Enabled = false;
            gridFormthanhtoan.Columns[2].Width = 60;


            //gridFormthanhtoan.Rows.Add("9090", "9090", "9090", "9090");


            locationlbTongTienThanhToan(lbSoTienCanThanhToan);


        }
        
        // cài đặt vị trí cho label nằm đúng 1 chỗ
        public void locationlbTongTienThanhToan(Label name)
        {

            int right_lbchucvu = name.Width;
            int right_lbchucvuNew = name.Width;
            int right_lbchucvu_ = right_lbchucvuNew - right_lbchucvu;
            int right_lbchucvu__ = name.Location.X - right_lbchucvu_;
            int right_lbchucvu_Y = name.Location.Y;
            name.Location = new Point(right_lbchucvu__, right_lbchucvu_Y);

        }

        private void thucHienLuuData()
        {
            TruyenData.Instance._LuotMuaHang += 1;
            User_Sell.instanceUSell.clearItemSelect();
            DateTime currentTime = DateTime.Now;


            string dateText = currentTime.ToString("ddMMyyyy");
            string dateLapHD = currentTime.ToString("dd-MM-yyyy");
            List<string> data1 = new List<string>() { "HD" + dateText + "-" + TruyenData.Instance._LuotMuaHang, "KH" + dateText + TruyenData.Instance._LuotMuaHang, txtNameKhachHang.Text, txtSdtKhachHang.Text, "TRA012", "Trà đào", "23", "1", "55", "NV001", "Nguyến Văn A", "22/11/2023" };
            // chuyển list string sang string 
            string strdata = string.Join(", ", data1);
            _dataDonBan = data1;

            // Gán dữ liệu từ TextBox vào biến tạm thời
            dataToSend = strdata;

            // Khởi tạo list để lưu dữ liệu các string[]
            List<string[]> danhSachMangChuoi = new List<string[]>();
            // Khởi tạo mảng string[] để lưu dữ liệu từ DataGridView
            int soluongpt = 14;


            if (gridFormthanhtoan.RowCount > 1)
            {
                soluongHoaDon++;
                TruyenData.Instance._LuotMuaHang = soluongHoaDon;
                string tamo1 = "000";
                if (soluongHoaDon > 9)
                {
                    tamo1 = "00";
                }
                else if (soluongHoaDon > 99)
                {
                    tamo1 = "0";
                }
                else if (soluongHoaDon > 999)
                {
                    tamo1 = "";
                }

                strIdKhachHang =  strIdKhachHang % 1000000000;
                strIdKhachHang++;

                string[] data = new string[soluongpt];
                data[0] = soluongHoaDon.ToString();//id hóa đơn
                data[1] = "HD" + tamo1 + soluongHoaDon; // mã hóa đơn
                data[2] = strIdKhachHang.ToString(); // id khách hàng
                data[3] = "KH" + tamo1 + soluongHoaDon; // mã khách hàng
                data[4] = (string.IsNullOrEmpty(txtNameKhachHang.Text)) ? "Không có dữ liệu" : txtNameKhachHang.Text;
                data[5] = (string.IsNullOrEmpty(txtSdtKhachHang.Text)) ? "" : txtSdtKhachHang.Text;
                data[6] = gridFormthanhtoan.Rows[0].Cells[0].Value.ToString();
                data[7] = gridFormthanhtoan.Rows[0].Cells[1].Value.ToString();
                data[8] = gridFormthanhtoan.Rows[0].Cells[3].Value.ToString();
                data[9] = gridFormthanhtoan.Rows[0].Cells[2].Value.ToString();
                data[10] = gridFormthanhtoan.Rows[0].Cells[3].Value.ToString();
                data[11] = TruyenData.Instance._LoginTK;// mã nhân viên đăng nhập vào
                data[12] = "để tạm"; // tên nhân viên đăng nhập vào
                data[13] = dateLapHD;


                // thêm phần tử đầu tiên trong với dữ liệu đầy đủ nhất. các dữ  liệu sau chỉ hiển thi món và đơn giá và số lượng
                danhSachMangChuoi.Add(data);


                int viTri = 0, indexRow = 1;

                string[] dataa = new string[soluongpt];
                while (viTri < soluongpt)
                {
                    if (viTri < 6 || viTri > 9)
                    {
                        dataa[viTri] = "";
                        dataa[0] = soluongHoaDon.ToString();//id hóa đơn
                        dataa[1] = "HD" + tamo1 + soluongHoaDon; // mã hóa đơn
                        dataa[2] = strIdKhachHang.ToString(); // id khách hàng
                        dataa[3] = "KH" + tamo1 + soluongHoaDon; // mã khách hàng
                        dataa[4] = (string.IsNullOrEmpty(txtNameKhachHang.Text)) ? "Không có dữ liệu" : txtNameKhachHang.Text;
                        dataa[5] = (string.IsNullOrEmpty(txtSdtKhachHang.Text)) ? "" : txtSdtKhachHang.Text;
                        dataa[10] = (double.Parse(gridFormthanhtoan.Rows[indexRow].Cells[3].Value.ToString()) * int.Parse(gridFormthanhtoan.Rows[indexRow].Cells[2].Value.ToString())).ToString();//lbSoTienCanThanhToan.Text;
                        dataa[11] = TruyenData.Instance._LoginTK;// mã nhân viên đăng nhập vào
                        dataa[12] = "để tạm";
                        dataa[13] = dateLapHD;
                    }
                    else
                    {
                        dataa[6] = gridFormthanhtoan.Rows[indexRow].Cells[0].Value.ToString();// mã sản phẩm
                        dataa[7] = gridFormthanhtoan.Rows[indexRow].Cells[1].Value.ToString();// tên sản phẩm
                        dataa[8] = gridFormthanhtoan.Rows[indexRow].Cells[3].Value.ToString();// đơn giá 
                        dataa[9] = gridFormthanhtoan.Rows[indexRow].Cells[2].Value.ToString();// số lượng chọn món đó
                    }

                    viTri++;
                    if (viTri == soluongpt)
                    {
                        indexRow += 1;
                        if (indexRow == gridFormthanhtoan.RowCount)
                        {
                            // Tạo bản sao của mảng dữ liệu11  -- tránh ghi đè dữ liệu nên mới phải dùng clone. tạo bản copy
                            danhSachMangChuoi.Add(dataa.Clone() as string[]);
                            break;
                        }

                        if (indexRow == gridFormthanhtoan.RowCount - 1)
                        {
                            // Tạo bản sao của mảng dữ liệu11
                            danhSachMangChuoi.Add(dataa.Clone() as string[]);
                        }
                        else
                        {
                            // Tạo bản sao của mảng dữ liệu11
                            string[] rowData = (string[])dataa.Clone();
                            danhSachMangChuoi.Add(rowData);
                        }

                        viTri = 0;

                    }

                }


            }
            else
            {
                soluongHoaDon++;
                TruyenData.Instance._LuotMuaHang = soluongHoaDon;
                string tamo = "000";
                if (soluongHoaDon > 9)
                {
                    tamo = "00";
                }
                else if (soluongHoaDon > 99)
                {
                    tamo = "0";
                }
                else if (soluongHoaDon > 999)
                {
                    tamo = "";
                }

                strIdKhachHang = strIdKhachHang % 1000000000;
                strIdKhachHang++;
                int sodienthoai = sdtTemp + strIdKhachHang;

                string[] data = new string[soluongpt];
                data[0] = soluongHoaDon.ToString();//id hóa đơn
                data[1] = "HD" + tamo + soluongHoaDon; // mã hóa đơn
                data[2] = strIdKhachHang.ToString(); // id khách hàng
                data[3] = "KH" + tamo + soluongHoaDon; // mã khách hàng
                data[4] = (string.IsNullOrEmpty(txtNameKhachHang.Text)) ? "Không có dữ liệu" : txtNameKhachHang.Text;
                data[5] = (string.IsNullOrEmpty(txtSdtKhachHang.Text)) ?  "" : txtSdtKhachHang.Text;
                data[6] = gridFormthanhtoan.Rows[0].Cells[0].Value.ToString();
                data[7] = gridFormthanhtoan.Rows[0].Cells[1].Value.ToString();
                data[8] = gridFormthanhtoan.Rows[0].Cells[3].Value.ToString();
                data[9] = gridFormthanhtoan.Rows[0].Cells[2].Value.ToString();
                data[10] = (double.Parse(gridFormthanhtoan.Rows[0].Cells[3].Value.ToString()) * int.Parse(gridFormthanhtoan.Rows[0].Cells[2].Value.ToString())).ToString();//lbSoTienCanThanhToan.Text;
                data[11] = TruyenData.Instance._LoginTK;// mã nhân viên đăng nhập vào
                data[12] = "để tạm"; // tên nhân viên đăng nhập vào
                data[13] = dateLapHD;

                danhSachMangChuoi.Add(data);
            }

           
            TruyenData.Instance.SharedData = danhSachMangChuoi;
            danhSachMangChuoi.Clear();
            this.Close();
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (txtSdtKhachHang.Text.Length == 10)
            {
                thucHienLuuData();
            }
            else if (txtSdtKhachHang.Text == string.Empty)
            {
                thucHienLuuData();
            }
            else if(txtSdtKhachHang.Text.Length > 0 || txtNameKhachHang.Text.Length > 0) 
            {
                if (0 < txtSdtKhachHang.Text.Length && txtSdtKhachHang.Text.Length < 11)
                {
                    MessageBox.Show("Số điện thoại phải 10 số!!", "thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Số điện thoại khách hàng nhập chưa đủ!\nVui lòng nhập lại", "THÔNG BÁO", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }


        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        //đặt dấu chấm cho tiền thanh toán
        static string FormatNumberWithDot(int number)
        {

            string formattedNumber;
            if (number.ToString().Length > 3)
            {
                // Chuyển số thành chuỗi
                string numberAsString = number.ToString();

                // Xác định vị trí để chèn dấu chấm
                int dotPosition = numberAsString.Length % 3;

                // Nếu dotPosition là 0, thì chèn dấu chấm tại vị trí cuối cùng
                if (dotPosition == 0)
                {
                    dotPosition = 3;
                }

                // Chèn dấu chấm mỗi 3 chữ số
                formattedNumber = numberAsString.Insert(dotPosition, ".");

                return formattedNumber;
            }
            else if (number.ToString().Length <= 3)
            {
                return number.ToString();
            }

            return "0";
        }

        private void txtSdtKhachHang_TextChanged(object sender, EventArgs e)
        {
            if (txtSdtKhachHang.Text.Length == 10)
            {
                txtSdtKhachHang.ForeColor = Color.SteelBlue;
            }
            else
            {
                txtSdtKhachHang.ForeColor = Color.Black;
            }
        }
    }
}

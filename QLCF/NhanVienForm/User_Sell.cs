using QLCF.NhanVienForm.user_SanPham;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace QLCF.NhanVienForm
{
    public partial class User_Sell : UserControl
    {
        private SqlConnection sqlConnection = null;// đối tượng kết nối
        //string connectionString = "Data Source=DESKTOP-39BMSC8\\GIAKHANH;Initial Catalog=QL_QuanCF;Integrated Security=True";
        string connectionString = LoadDatabase.connectionString;
        public event Action<string> DataReceived;
        public List<string> tammm = new List<string>();
        public string tammm1;
        private Form_ThanhToan form_thanhtoan;
        private User_Donban user_Donban;
        public string uslNamekey = "";//
        public string uslNameGia;//

        public static User_Sell instanceUSell;

        // Hàm xử lý sự kiện để nhận giá trị từ formA
        private void FormTT_ValuePassed(object sender, string e)
        {
            // Xử lý giá trị nhận được từ formA ở đây
            MessageBox.Show($"Nhận được giá trị từ formA: {e}");
        }


        public User_Sell()
        {
            InitializeComponent();
            instanceUSell = this;
            ThemDuLieuVaoDataGridView();
            //rdTienMat.Checked = true;
            
        }


        


        private void ThemDuLieuVaoDataGridView()
        {
            grid.ScrollBars = ScrollBars.Vertical; // Ẩn thanh cuộn ngan

            // Đặt tên cột <Biến tên cột>,<name cột>
            grid.Columns.Add("colmaMon", "Mã món");
            grid.Columns.Add("colnameMon", "Tên món");
            grid.Columns.Add("colslBan", "Số lượng");
            grid.Columns.Add("coldongia", "Đơn giá");
            grid.Columns.Add("coltonggia", "Tổng giá");


            // Đặt căn giữ cho header của cột thứ 2 (index 1) trong DataGridView
            grid.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            grid.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            grid.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            grid.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            grid.Columns[4].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;


            // Đặt căn giữ cho cột trong DataGridView
            grid.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            grid.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            grid.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            grid.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            grid.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;


            // điều chỉnh độ rộng của cột 1 thành 100 pixels    
            //grid.Columns[0].Width = 190;
            // điều chỉnh độ rộng của cột 2 --> auto 
            grid.Columns[1].Width = 100;
            // điều chỉnh độ rộng của cột 3 thành 150 pixels
            grid.Columns[2].Width = 60;

            //grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            //grid.RowTemplate.Height = 90; 
        }


        // chuyển chuổi nhị phân thành ảnh
        public System.Drawing.Image ByteArrayToImage(byte[] byteArray)
        {
            using (MemoryStream ms = new MemoryStream(byteArray))
            {
                System.Drawing.Image image = System.Drawing.Image.FromStream(ms);
                return image;
            }
        }


        public void AddItem(string tag, string name, string dongia, string TagName)//, byte[] binaryPic)
        {
            var mon = new User_SanPham()
            {
                _lbTag = tag, // mã sản phẩm  
                _lbNameSP = name,
                _lbDonGia = dongia,
                Tag = TagName, // Tag là thuộc tính vốn có của form hoặc usercontrol. Tag là phẩn tử có sẳn. tag còn có nghĩa là mã định danh của usercontrol đó hoặc form đó
                Margin = new Padding(8, 0, 0, 8),
                BackColor = System.Drawing.Color.FromArgb(147, 177, 166),
                Size = new System.Drawing.Size(363 - 5, 135),
                //_arrayBinaryImage = binaryPic// mảng dữ liệu nhị phân của hình ảnh
            };
            flowLayoutPanel_Contain_SanPham.Controls.Add(mon);

            mon.OnSelect += (ss, ee) =>
            {
                var monSe = (User_SanPham)ss;
                string tonggia = monSe.lbDonGia.Text;
                foreach (DataGridViewRow item in grid.Rows)
                {
                    if (item.Cells[0].Value != null && item.Cells[0].Value.ToString() == monSe.lbTag.Text)
                    {   
                        item.Cells[1].Value = monSe.lbNameSP.Text;
                        item.Cells[2].Value = int.Parse(item.Cells[2].Value.ToString()) + 1;
                        Console.WriteLine("Hello: "+monSe.lbDonGia.Text);
                        item.Cells[4].Value = FormatNumberWithDot(int.Parse(item.Cells[2].Value.ToString())* int.Parse(monSe.lbDonGia.Text.Replace(".000", ""))).ToString()+".000";//monSe.lbDonGia.Text.Replace("$", ""))).ToString("C2")
                        CalculateTotal();
                        return;
                    }
                    else
                    {
                        Console.WriteLine(monSe);
                    }
                    
                }
                // đưa các món đã chọn vào datagridview của chọn món
                grid.Rows.Add(new object[] { monSe.lbTag.Text, monSe.lbNameSP.Text, 1, monSe.lbDonGia.Text, tonggia});

                // tính tổng giá của tất cả sản phẩm đã chọn
                CalculateTotal();

                //điều chỉnh lại vị trí của lable khi thêm dữ liệu
                locationlbTongTienThanhToan(lbSoTienCanThanhToan);
            };


            
        }


        void CalculateTotal()
        {
            int tot = 0;
            foreach (DataGridViewRow item in grid.Rows)
            {
                // Kiểm tra xem item.Cells[2].Value có phải là null hay không
                if (item.Cells[2].Value != null)
                {
                    string cellValue = item.Cells[4].Value.ToString().Replace(".000", "");// xóa bỏ .000 của Label tổng thanh toán
                   
                    

                    // Kiểm tra xem sau khi thay thế, chuỗi có trống không
                    if (!string.IsNullOrEmpty(cellValue))
                    {
                        cellValue=cellValue.Replace(".", string.Empty);
                        tot += int.Parse(cellValue);
                    }
                }
                else
                {
                    Console.WriteLine("CalculateTotal");
                }
            }


            //hiển thị tổng số tiền khách cần thanh toán
            lbSoTienCanThanhToan.Text = FormatNumberWithDot(int.Parse(tot.ToString())) + ".000";
            
        }



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


       


        public void User_Sell_Load(object sender, EventArgs e)
        {
            // tải trước dữ liệu của form
            User_Donban.instanceDonBan.User_Donban_Load(sender, e);
            // tải trước dữ liệu của form
            User_DatBan.instanceDatBan.User_DatBan_Load(sender, e);


            form_thanhtoan = new Form_ThanhToan();
            User_SanPham user_SanPham = new User_SanPham();
            DataTable dataTable1 = new DataTable();

            using (sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open(); // Mở kết nối

                // Truy vấn SQL để lấy dữ liệu từ bảng
                string query = "select MaSanPham,TenSanPham,GiaSanPham, NSX, NHH from QL_SanPham ";

                using (SqlCommand command = new SqlCommand(query, sqlConnection))
                {
                    SqlDataReader data = command.ExecuteReader();
                    while (data.Read())
                    {
                        if (data.HasRows == true)
                        {
                            //3 ô đây hiểu là 3 cột mã số . tên . đơn giá
                            string baKyTuDau = data[0].ToString();
                            baKyTuDau = baKyTuDau.Substring(0, 3);
                            user_SanPham._lbTag = data[0].ToString();
                            user_SanPham._lbNameSP = data[1].ToString();
                            user_SanPham._lbDonGia = data[2].ToString();

                            // Thực hiện truy vấn và nhận dữ liệu ảnh dưới dạng byte[]
                            //byte[] imageData = (byte[])data[3];

                            AddItem(user_SanPham._lbTag, user_SanPham._lbNameSP, user_SanPham._lbDonGia, baKyTuDau);//, imageData);

                            //Console.WriteLine("-->>> " + user_SanPham._lbTag + "-" + user_SanPham._lbNameSP + "-" + user_SanPham._lbDonGia + "-" + baKyTuDau, imageData);

                        }
                    }

                    
                }
            }
           
        }

        public void responsive(int newWidthForm)
        {

            //count = flowLayoutPanel_MonSelect.Controls.Count;
            if (newWidthForm == 1920)
            {
                pnlContain_ThanhToan.Size = new Size(410 + 170, 1080);
                grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                btnThanhToan.Size = new Size(404+170, 75);

            }
            else
            {
                pnlContain_ThanhToan.Size = new Size(550, 733);
                grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                btnThanhToan.Size = new Size(544, 75);

            }
        }

        
        // xóa tất cả các món được chọn
        private void btnClearAll_ItemSelect_Click(object sender, EventArgs e)
        {
            clearItemSelect();

        }
        
        public void clearItemSelect()
        {
            // Xóa tất cả các dòng trong DataGridView
            grid.Rows.Clear();
            lbSoTienCanThanhToan.Text = "0";
        }

        
        // chức năng tìm kiếm món
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            foreach (var item in flowLayoutPanel_Contain_SanPham.Controls)
            {
                var mon = (User_SanPham)item;
                mon.Visible = mon.lbNameSP.Text.ToLower().ToLower().Contains(txtSearch.Text.Trim().ToLower());
            }
        }



        // phân loại món theo tag
        private void btnTagMonCF_Click(object sender, EventArgs e)
        {
            ShowControlsByTag("CFE");
        }

        private void btnTagMonAV_Click(object sender, EventArgs e)
        {
            ShowControlsByTag("DAV");
        }

        private void btnTagMonTS_Click(object sender, EventArgs e)
        {
            ShowControlsByTag("TSU");
        }

        private void btnTagMonTra_Click(object sender, EventArgs e)
        {
            ShowControlsByTag("TRA");
        }

        private void btnTagMonNCG_Click(object sender, EventArgs e)
        {
            ShowControlsByTag("NCG");
        }

        private void btnTagMonALL_Click(object sender, EventArgs e)
        {
            ShowControlsByTag("");
        }



        // Hiển thị hoặc ẩn các UserControl dựa trên Tag
        private void ShowControlsByTag(string tag)
        {
            foreach (Control control in flowLayoutPanel_Contain_SanPham.Controls)
            {
                if (control is User_SanPham monControl && monControl.Tag != null)
                {
                    if (monControl.Tag.ToString() == tag)
                    {
                        monControl.Visible = true;
                    }
                    else
                    {
                        monControl.Visible = false;
                    }

                    if (tag == "")
                    {
                        monControl.Visible = true;
                    }
                }
            }
        }


        public string loaithanhtoan = "";
        int indexHD = 1;
        private void btnThanhToan_Click(object sender, EventArgs e)
        {


            //// Kiểm tra từng RadioButton trong nhóm
            //if (rdTienMat.Checked)
            //{
            //    loaithanhtoan += "tiền mặt";
            //}
            //else
            //{
            //    loaithanhtoan += "chuyển khoản";
            //}
            
            // Kiểm tra xem có dữ liệu trong DataGridView không
            if (grid.Rows.Count > 0)
            {
                // Tạo chuỗi để lưu trữ dữ liệu từ DataGridView
                string data = "";

                // Lặp qua từng dòng trong DataGridView
                foreach (DataGridViewRow row in grid.Rows)
                {
                    // Lặp qua từng ô trong dòng
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        // Thêm giá trị của ô vào chuỗi dữ liệu
                        data += cell.Value.ToString() + "\t";
                    }

                    // Thêm ký tự mới cho mỗi dòng
                    data += "\n";
                }

                // Lấy ngày và giờ hiện tại
                DateTime currentDateTime = DateTime.Now;
                // Định dạng ngày và giờ theo định dạng mong muốn 
                string formattedDateTime = currentDateTime.ToString("HH:mm:ss - dd/MM/yyyy");
                string formattedDate = currentDateTime.ToString("ddMMyyyy");

                form_thanhtoan = new Form_ThanhToan();
                form_thanhtoan.NhanDuLieuThanhToan = data;
                truyendulieutabel();
                form_thanhtoan.lbMaHD.Text = "DH" + formattedDate +"-"+ indexHD++;
                form_thanhtoan.lbSoTienCanThanhToan.Text = lbSoTienCanThanhToan.Text;
                form_thanhtoan.lbGioVaNgayLapHD.Text = formattedDateTime;
                ShowFormThanhToan();

            }
            else
            {
                MessageBox.Show("Chưa chọn món\nKhông thể thanh toán", "THÔNG BÁO", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void ShowFormThanhToan()
        {
            try
            {
                form_thanhtoan.Show();
            }
            catch (ObjectDisposedException e)
            {
                Console.WriteLine(e.Message);
                form_thanhtoan = new Form_ThanhToan();

                // Tạo chuỗi để lưu trữ dữ liệu từ DataGridView
                string data = "";

                // Lặp qua từng dòng trong DataGridView
                foreach (DataGridViewRow row in grid.Rows)
                {
                    // Lặp qua từng ô trong dòng
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        // Thêm giá trị của ô vào chuỗi dữ liệu
                        data += cell.Value.ToString() + "\t";
                    }

                    // Thêm ký tự mới cho mỗi dòng
                    data += "\n";
                }

                // Hiển thị dữ liệu trong MessageBox
                //MessageBox.Show(data+ loaithanhtoan, "Dữ liệu từ DataGridView");
                form_thanhtoan.NhanDuLieuThanhToan = data;

                truyendulieutabel();

                form_thanhtoan.Show();
            }

        }



        public void truyendulieutabel()
        {
            try
            {
                /// Lặp qua từng hàng của dataGridView1 bằng vòng lặp foreach
                foreach (DataGridViewRow row in grid.Rows)
                {
                    // Tạo một hàng mới trong dataGridView2
                    int rowIndex = form_thanhtoan.gridFormthanhtoan.Rows.Add();

                    // Lặp qua từng ô trong hàng của dataGridView1 và sao chép giá trị sang dataGridView2
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        // rowIndex là chỉ số của hàng mới đã thêm vào dataGridView2
                        // cell.ColumnIndex là chỉ số của cột tương ứng trong dataGridView1
                        form_thanhtoan.gridFormthanhtoan.Rows[rowIndex].Cells[cell.ColumnIndex].Value = cell.Value;
                    }
                }
            }catch (Exception ex)
            {
                Console.WriteLine (ex.Message);

                form_thanhtoan.gridFormthanhtoan = new DataGridView();

                // Đặt tên cột <Biến tên cột>,<name cột>
                form_thanhtoan.gridFormthanhtoan.Columns.Add("colnameMon", "Tên món");
                form_thanhtoan.gridFormthanhtoan.Columns.Add("colslBan", "Số lượng");
                form_thanhtoan.gridFormthanhtoan.Columns.Add("coldongia", "Đơn giá");
                form_thanhtoan.gridFormthanhtoan.Columns.Add("colthanhtien", "Thành tiền");

                // Đặt căn giữ cho header của cột thứ 2 (index 1) trong DataGridView
                form_thanhtoan.gridFormthanhtoan.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                form_thanhtoan.gridFormthanhtoan.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                form_thanhtoan.gridFormthanhtoan.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                form_thanhtoan.gridFormthanhtoan.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;


                // Đặt căn giữ cho cột trong DataGridView
                form_thanhtoan.gridFormthanhtoan.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                form_thanhtoan.gridFormthanhtoan.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                form_thanhtoan.gridFormthanhtoan.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                form_thanhtoan.gridFormthanhtoan.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                // Điều chỉnh độ rộng của cột tự động lấp đầy bảng
                form_thanhtoan.gridFormthanhtoan.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                // Điều chỉnh chiều cao của dòng header tự động
                form_thanhtoan.gridFormthanhtoan.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

                // Chỉnh định dạng cho font của header
                form_thanhtoan.gridFormthanhtoan.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 11, FontStyle.Bold);

                // Chỉnh định dạng cho dữ liệu DataGridView
                form_thanhtoan.gridFormthanhtoan.DefaultCellStyle.Font = new Font("Tahoma", 10, FontStyle.Regular);

                // Tắt viền cho toàn bộ DataGridView
                form_thanhtoan.gridFormthanhtoan.CellBorderStyle = DataGridViewCellBorderStyle.None;
            }
            
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

        
    }
}   

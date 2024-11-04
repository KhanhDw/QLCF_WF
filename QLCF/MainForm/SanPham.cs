//using QLCF.MainForm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace QLCF
{
    public partial class SanPham : UserControl
    {
        private SqlConnection sqlConnection = null;// đối tượng kết nối
        //string connectionString = "Data Source=DESKTOP-39BMSC8\\GIAKHANH;Initial Catalog=QL_QuanCF;Integrated Security=True";
        string connectionString = LoadDatabase.connectionString;

        private Byte[] result;
        private string imgBinary { get; set; }

        PictureBox nnn;

        public static SanPham instanceTaiKhoan;

        public SanPham()
        {
            InitializeComponent();
            instanceTaiKhoan = this;

        }

        public void SanPham_Load(object sender, EventArgs e)
        {
            LoadDataSQL();
            ThemDuLieuVaoDataGridView();

        }

        private void LoadDataSQL()
        {
            using (sqlConnection = new SqlConnection())
            {
                sqlConnection.ConnectionString = connectionString; // Truyền chuỗi kết nối
                sqlConnection.Open(); // Mở kết nối

                // Truy vấn SQL để lấy dữ liệu từ bảng
                string query = "SELECT MaSanPham,TenSanPham,GiaSanPham, NSX, NHH  FROM QL_SanPham";
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
                        dataGridView_SanPham.DataSource = dataTable;

                        dataGridView_SanPham.Columns["MaSanPham"].HeaderText = "Mã Sản Phẩm";
                        dataGridView_SanPham.Columns["TenSanPham"].HeaderText = "Mã Tên Phẩm";
                        dataGridView_SanPham.Columns["GiaSanPham"].HeaderText = "Giá Sản Phẩm";
                        //dataGridView_SanPham.Columns["HinhAnh"].HeaderText = "Hình Ảnh";
                        dataGridView_SanPham.Columns["NSX"].HeaderText = "Ngày Sản Xuất";
                        dataGridView_SanPham.Columns["NHH"].HeaderText = "Ngày Hết Hạn";
                    }


                    //using (SqlDataReader reader = command.ExecuteReader())
                    //{
                    //    while (reader.Read())
                    //    {
                    //        if (reader[3] != DBNull.Value)
                    //        {
                    //            byte[] imageData = (byte[])reader[3];
                    //            // PictureBox có thể đặt tên là pictureBox1
                    //            pictureBox1.Image = ByteArrayToImage(imageData);
                    //        }
                    //        else {}
                    //    }
                    //}
                }
            }
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


        

        public byte[] ImageToByteArray(System.Drawing.Image imageIn)
        {

            using (var ms = new MemoryStream())
            {
                ImageFormat format = ImageFormat.Jpeg;
                imageIn.Save(ms, format);
                return ms.ToArray();
            }
        }


        // Phương thức chuyển đổi chuỗi hexa thành mảng byte -> từ mảng byte sẽ chuyển thành dạng ảnh
        static byte[] StringToByteArray(string hex)
        {
            hex = hex.Substring(2); // Loại bỏ "0x" từ đầu chuỗi
            int length = hex.Length / 2;
            byte[] byteArray = new byte[length];

            for (int i = 0; i < length; i++)
            {
                byteArray[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            }

            return byteArray;
        }



        private void ThemDuLieuVaoDataGridView()
        {
            dataGridView_SanPham.ScrollBars = ScrollBars.Vertical; // Ẩn thanh cuộn ngan

            //// Đặt tên cột <Biến tên cột>,<name cột>
            //dataGridView_SanPham.Columns.Add("colidmon", "Mã món");
            //dataGridView_SanPham.Columns.Add("colnameMon", "Tên món");
            //dataGridView_SanPham.Columns.Add("coldongia", "Đơn giá");


            //// Đặt căn giữ cho header của cột trong DataGridView
            //dataGridView_SanPham.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //dataGridView_SanPham.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //dataGridView_SanPham.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;


            //// Đặt căn giữ cho cột trong dữ liệu trong DataGridView
            //dataGridView_SanPham.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //dataGridView_SanPham.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            //dataGridView_SanPham.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;


            //// điều chỉnh độ rộng của cột 1   
            //dataGridView_SanPham.Columns[0].Width = 200;
            //// điều chỉnh độ rộng của cột 2 
            //dataGridView_SanPham.Columns[1].Width = 400;
            //// điều chỉnh độ rộng của cột 3 thành 150 pixels
            ////dataGridView_SanPham.Columns[2].Width = 100;

            // Điều chỉnh độ rộng của cột tự động lấp đầy bảng
            dataGridView_SanPham.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Điều chỉnh chiều cao của dòng header tự động
            dataGridView_SanPham.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize; 

            // Chỉnh định dạng cho font của header
            dataGridView_SanPham.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 12, FontStyle.Bold);

            // Chỉnh định dạng cho dữ liệu DataGridView
            dataGridView_SanPham.DefaultCellStyle.Font = new Font("Tahoma", 13, FontStyle.Regular);


            // Tắt viền cho toàn bộ DataGridView
            dataGridView_SanPham.CellBorderStyle = DataGridViewCellBorderStyle.Single;

            dataGridView_SanPham.AllowUserToAddRows = false;

        }

        //respontive form
        public void responsive_SanPham(int newWidthForm)
        {

            if (newWidthForm == 1920)
            {
                pnlReponsiveSanPham.Size = new Size(1395 + 310, 71);
                pnlContainTableSanPham.Size = new Size(1389 + 300, 589 + 305);
            }
            else if (newWidthForm == 1615)
            {
                pnlReponsiveSanPham.Size = new Size(1395, 71);
                pnlContainTableSanPham.Size = new Size(1389, 632);

            }

        }

        private void btnThemSanPham_Click(object sender, EventArgs e)
        {
            if (txtmamon.Text == "" || txttenmon.Text == "" || txtdongia.Text == "")
            {
                MessageBox.Show("vui lòng nhập đầy đủ thông tin", "thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
            else if (!double.TryParse(txtdongia.Text, out _))
            {
                MessageBox.Show("Đơn giá phải là số!!", "thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {


                byte[] result1 = ImageToByteArray(pictureBox1.Image);

                // Tạo đối tượng SqlConnection
                using (sqlConnection = new SqlConnection(connectionString))
                {
                    // Mở kết nối
                    sqlConnection.Open();
                    // Tạo câu lệnh SQL để xóa dòng tương ứng trong cơ sở dữ liệu
                    string insertQuery = "INSERT INTO QL_SanPham (MaSanPham, TenSanPham, GiaSanPham) VALUES (@MaSanPham, @TenSanPham, @GiaSanPham)";

                    using (SqlCommand cmd = new SqlCommand(insertQuery, sqlConnection))
                    {
                        cmd.Parameters.AddWithValue("@MaSanPham", txtmamon.Text);
                        cmd.Parameters.AddWithValue("@TenSanPham", txttenmon.Text);
                        cmd.Parameters.AddWithValue("@GiaSanPham", decimal.Parse(txtdongia.Text));
                        //cmd.Parameters.AddWithValue("@HinhAnh", result1);
                        cmd.ExecuteNonQuery();
                    }
                }

                txtmamon.Clear();
                txttenmon.Clear();
                txtdongia.Clear();
                txtmamon.Focus();
            }


            LoadDataSQL();
        }

        private void btnSuaSanPham_Click(object sender, EventArgs e)
        {
            if (dataGridView_SanPham.SelectedRows.Count > 0)
            {
                //dataGridView_SanPham.SelectedRows[0].Cells[0].Value = txtmamon.Text;
                //dataGridView_SanPham.SelectedRows[0].Cells[1].Value = txttenmon.Text;
                //dataGridView_SanPham.SelectedRows[0].Cells[2].Value = txtdongia.Text;

                if (txtmamon.Text == "" || txttenmon.Text == "" || txtdongia.Text == "")
                {
                    MessageBox.Show("vui lòng nhập đầy đủ thông tin", "thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
                else if (!double.TryParse(txtdongia.Text, out _))
                {
                    MessageBox.Show("Đơn giá phải là số!!", "thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {

                    // Tạo đối tượng SqlConnection
                    using (sqlConnection = new SqlConnection(connectionString))
                    {
                        // Mở kết nối
                        sqlConnection.Open();

                        string deleteQuery = "UPDATE QL_SanPham SET MaSanPham = N'" + txtmamon.Text + "', TenSanPham = N'" + txttenmon.Text + "', GiaSanPham = '" + double.Parse(txtdongia.Text) + "' WHERE MaSanPham = '" + dataGridView_SanPham.SelectedRows[0].Cells[0].Value + "';";

                        using (SqlCommand command = new SqlCommand(deleteQuery, sqlConnection))
                        {
                            // Thực thi câu lệnh SQL
                            int rowsAffected = command.ExecuteNonQuery();

                            //if (rowsAffected > 0)
                            //{
                            //    dataGridView_SanPham.Rows.RemoveAt(dataGridView_SanPham.SelectedRows[0].Index);
                            //}

                        }
                    }

                    
                    
                    





                    txtmamon.Clear();
                    txttenmon.Clear();
                    txtdongia.Clear();
                    txtmamon.Focus();
                }


                LoadDataSQL();
            }
            
        }

        private void btnXoaSanPham_Click(object sender, EventArgs e)
        {
            try
            {
                // Tạo đối tượng SqlConnection
                using (sqlConnection = new SqlConnection(connectionString))
                {
                    // Mở kết nối
                    sqlConnection.Open();
                    // Tạo câu lệnh SQL để xóa dòng tương ứng trong cơ sở dữ liệu
                    string deleteQuery = "DELETE FROM QL_SanPham WHERE MaSanPham = '" + (dataGridView_SanPham.SelectedRows[0].Cells[0].Value).ToString() + "'";

                    using (SqlCommand command = new SqlCommand(deleteQuery, sqlConnection))
                    {
                        // Thực thi câu lệnh SQL
                        int rowsAffected = command.ExecuteNonQuery();

                        //if (rowsAffected > 0)
                        //{
                        //    dataGridView_SanPham.Rows.RemoveAt(dataGridView_SanPham.SelectedRows[0].Index);
                        //}

                    }
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có
                MessageBox.Show("Error: " + ex.Message);
            }

            LoadDataSQL();
        }

        private void dataGridView_SanPham_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // Lấy dữ liệu từ dòng được chọn
                DataGridViewRow row = dataGridView_SanPham.Rows[e.RowIndex];
                // Hiển thị dữ liệu từ dòng vào các TextBox
                txtmamon.Text = row.Cells[0].Value.ToString();
                txttenmon.Text = row.Cells[1].Value.ToString();
                txtdongia.Text = row.Cells[2].Value.ToString();

                if (row.Cells[3].Value != DBNull.Value)
                {
                    byte[] imageData = (byte[])row.Cells[3].Value;
                    // PictureBox có thể đặt tên là pictureBox1
                    pictureBox1.Image = ByteArrayToImage(imageData);
                }
                else { }
            }
        }

        private void SearchInDataGridView(string searchText)
        {
            foreach (DataGridViewRow row in dataGridView_SanPham.Rows)
            {
                row.Selected = false;
            }

            foreach (DataGridViewRow row in dataGridView_SanPham.Rows)
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

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchText = txtSearch.Text;
            SearchInDataGridView(searchText);
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            // Lọc dữ liệu trực tiếp trên DataGridView
            string searchText = txtSearch.Text;

            foreach (DataGridViewRow row in dataGridView_SanPham.Rows)
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
                CurrencyManager currencyManager1 = (CurrencyManager)BindingContext[dataGridView_SanPham.DataSource];
                currencyManager1.SuspendBinding();
                row.Visible = false;
                currencyManager1.ResumeBinding();

                // Hiển thị hoặc ẩn dòng tùy thuộc vào kết quả tìm kiếm
                row.Visible = rowVisible;
            }
        }

        private void btnChangePic_Click(object sender, EventArgs e)
        {
            string imgLocation1;
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "jpeg|*.jpg|png|*.png|bmp|*.bmp|all files|*.*";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                // lấy địa chỉ ảnh
                imgLocation1 = dialog.FileName.ToString();

                //hiển thị ảnh trên khung 
                System.Drawing.Image inputImage = System.Drawing.Image.FromFile(imgLocation1);
                // Tạo một đối tượng ảnh mới với kích thước đã cho
                System.Drawing.Image resizedImage = new Bitmap(inputImage, pictureBox1.Width, pictureBox1.Height);

                //nnn  = new PictureBox();
                //nnn.Image = resizedImage;
                // Gán ảnh đã thay đổi kích thước vào PictureBox
                pictureBox1.Image = resizedImage;

            }
            else {}

            
        }

       

    }
}
    
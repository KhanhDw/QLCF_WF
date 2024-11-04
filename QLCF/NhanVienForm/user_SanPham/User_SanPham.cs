using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace QLCF.NhanVienForm.user_SanPham
{
    public partial class User_SanPham : UserControl
    {
        // video. khởi tạo biến sự kiện cho việc lick chuột
        public event EventHandler OnSelect = null;

        public Image anhMon;

        //video. lưu giữu giá trị của 2 thuộc tính của sản phẩm được hiển thị qua Label
        public string _lbTag { get => lbTag.Text; set => lbTag.Text = value; }
        public string _lbNameSP { get => lbNameSP.Text; set => lbNameSP.Text = value; }
        public string _lbDonGia { get => lbDonGia.Text; set => lbDonGia.Text = value; }

        public byte[] _arrayBinaryImage { get; set; }

        public System.Drawing.Image _pictureBox_Mon { get => anhMon; set => anhMon = value; }

        public User_SanPham()
        {
            InitializeComponent();
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



        private void User_SanPham_Load(object sender, EventArgs e)
        {
            locationlbTongTienThanhToan(lbDonGia);
            //SetImage();
        }


        // điều chỉnh kích thước ảnh sao cho đúng với kích thước của picturebox
        private void DieuChinhKichThuocAnh()
        {
            Image image = pictureBox_Mon.Image;

            // Lấy kích thước của PictureBox
            int width = pictureBox_Mon.Width;
            int height = pictureBox_Mon.Height;

            // Tạo một đối tượng ảnh mới với kích thước đã cho
            Image resizedImage = new Bitmap(image, width, height);

            // Gán ảnh đã thay đổi kích thước vào PictureBox
            pictureBox_Mon.Image = resizedImage;
            //DieuChinhKichThuocAnh();
        }



        public void SetImage()
        {
            // Chuyển đổi dữ liệu nhị phân thành hình ảnh
            Image image = ByteArrayToImage(_arrayBinaryImage);

            // Đặt hình ảnh cho PictureBox
            pictureBox_Mon.Image = image;

            DieuChinhKichThuocAnh();
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



        //--> click chọn đối tượng  

        private void pictureBox_Mon_Click(object sender, EventArgs e)
        {
            OnSelect?.Invoke(this, e);
        }

        private void lbNameSP_Click(object sender, EventArgs e)
        {
            OnSelect?.Invoke(this, e);
        }

        private void pnlBoXSanPham_Click(object sender, EventArgs e)
        {
            OnSelect?.Invoke(this, e);
        }

        private void lbDonGia_Click(object sender, EventArgs e)
        {
            OnSelect?.Invoke(this, e);
        }

        private void lbTag_Click(object sender, EventArgs e)
        {
            OnSelect?.Invoke(this, e);
        }
    }
}
    
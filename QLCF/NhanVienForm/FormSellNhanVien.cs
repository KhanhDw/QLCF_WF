using QLCF.NhanVienForm;
using QLCF.NhanVienForm.user_SanPham;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLCF
{
    public partial class FormSellNhanVien : Form
    {
        //biến dùng để xác định cho việc di chuyển form
        private bool isDragging = false;
        private int mouseX;
        private int mouseY;
        private Boolean enable = false;
        //private int userControlCurrenly = 0;
        private Button currentButton;
        private int newWidthForm = 0;

        User_Sell userControl_Sell = new User_Sell();
        User_DatBan userControl_DatBan = new User_DatBan();
        User_Donban userControl_Donban = new User_Donban();
        User_Setting userControl_Setting = new User_Setting();


        public static FormSellNhanVien instanceFormSellNhanVien;

        public FormSellNhanVien()
        {
            InitializeComponent();
            instanceFormSellNhanVien = this;
            // user. biến sự kiện += hàm cần thực hiện (hàm tại form này)
            // công dụng: thay đổi giá trị giá trị của đối tượng của usercontrol này từ usercontrol từ một usercontrol khác
            userControl_Setting.LogoutClicked += formSellNhanVien_LogoutClicked;
        }


        // lấy giá trị mỗi khi kích cở form thay đổi
        public void FormSellNhanVien_Load(object sender, EventArgs e)
        {
            addUserControlForPanel(userControl_Sell);
            this.SizeChanged += FormSellNhanVien_SizeChanged;
            FormSellNhanVien_SizeChanged(sender, e);
        }




        // Thêm usercontrol vào panel để hiển thị
        private void addUserControlForPanel(UserControl userControl)
        {
            userControl.Dock = DockStyle.Fill;
            pnlContainUserControl.Controls.Clear();
            pnlContainUserControl.Controls.Add(userControl);
            userControl.BackColor = Color.FromArgb(205, 245, 253);
            //userControl.Location = new Point(157, 34);
            userControl.BringToFront();

        }

        // đổi màu button cửa sổ đang bật - áp dụng cho thanh menu bên trái
        private void ActivateButton(object btnSender)
        {
            if (btnSender != null)
            {
                if (currentButton != (Button)btnSender)
                {
                    DisableButton();
                    currentButton = (Button)btnSender;
                    currentButton.BackColor = Color.FromArgb(0, 119, 179);
                    currentButton.ForeColor = Color.Honeydew;
                }
            }
        }

        // trả lại màu cũ khi button không còn được kích hoạt - áp dụng cho thanh menu bên trái
        private void DisableButton()
        {
            foreach (Control previousBtn in LeftBarControll.Controls)
            {
                if (previousBtn.GetType() == typeof(Button))
                {
                    previousBtn.ForeColor = Color.Black;
                    previousBtn.BackColor = Color.DarkSeaGreen;
                }
            }
        }








        // điều chỉnh kích cở của usercontrol khi thu phóng
        public void reponsive(int newWidthForm)
        {
            //pnlContain_ThanhToan
            userControl_Sell.responsive(newWidthForm);
            userControl_DatBan.responsive_DatBan(newWidthForm);
            userControl_Donban.responsive_Donban(newWidthForm);
            userControl_Setting.responsive_Setting(newWidthForm);

        }

        // lấy kích thước của form  mỗi khi thay đổi kích thước
        private void FormSellNhanVien_SizeChanged(object sender, EventArgs e)
        {
            //lấy độ rộng của form mỗi khi thay đổi kích thức của form
            newWidthForm = this.Width;
            Console.WriteLine(newWidthForm.ToString());

            if (newWidthForm == 1920)
            {
                this.Size = new Size(1920, 1080);
                reponsive(newWidthForm);
            }
            else 
            {
                this.Size = new Size(1525, 852);// 1387, 788);
                reponsive(newWidthForm);
            }
        }






        // đóng chương trình 
        private void btnclose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }




        // thu nhỏ và phóng to cửa sổ
        private void btnmax_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
            {
                // Nếu cửa sổ đang ở trạng thái bình thường, thì chuyển sang trạng thái phóng to (maximized).
                this.WindowState = FormWindowState.Maximized;
            }
            else if (this.WindowState == FormWindowState.Maximized)
            {
                // Nếu cửa sổ đang ở trạng thái phóng to, thì chuyển về trạng thái bình thường.
                this.WindowState = FormWindowState.Normal;
            }
        }



        // thu cửa sổ 
        private void btnmin_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }



        //===> di chuyển cửa sổ (form)
        // khi người dùng nhấn giữ chuột và kéo chuột
        private void FormSellNhanVien_MouseDown(object sender, MouseEventArgs e)
        {
            if (enable)
            {
                this.isDragging = true;
                this.mouseX = e.X;
                this.mouseY = e.Y;
            }
        }

        // lấy giá trị của chuột khi kéo và di chuyển form theo vị trí của chuột
        private void FormSellNhanVien_MouseMove(object sender, MouseEventArgs e)
        {
            if (enable)
            {
                if (isDragging)
                {
                    int deltaX = e.X - this.mouseX;
                    int deltaY = e.Y - this.mouseY;
                    this.Location = new System.Drawing.Point(this.Location.X + deltaX, this.Location.Y + deltaY);
                }
            }
        }

        // khi chuột không nhấn trả mọi thứ về false
        private void FormSellNhanVien_MouseUp(object sender, MouseEventArgs e)
        {
            if (enable)
                this.isDragging = false;
        }






        // di chuyển cửa sổ (btn move)
        private void btnmove_MouseMown(object sender, MouseEventArgs e)
        {
            enable = true;
            FormSellNhanVien_MouseDown(sender, e);
            enable = false;
        }

        private void btnmove_MouseMove(object sender, MouseEventArgs e)
        {
            enable = true;
            FormSellNhanVien_MouseMove(sender, e);
            enable = false;
        }

        private void btnmove_MouseUp(object sender, MouseEventArgs e)
        {
            enable = true;
            FormSellNhanVien_MouseUp(sender, e);
        }





        // button usercontrol
        private void btnKhachGoiMon_Click(object sender, EventArgs e)
        {
            ActivateButton(sender);
            addUserControlForPanel(userControl_Sell);
        }

        private void btnDatBan_Click(object sender, EventArgs e)
        {
            ActivateButton(sender);
            addUserControlForPanel(userControl_DatBan);

        }

        private void btnDonBan_Click(object sender, EventArgs e)
        {
            ActivateButton(sender);
            addUserControlForPanel(userControl_Donban);

        }

        private void btnSetting_Click(object sender, EventArgs e)
        {
            ActivateButton(sender);
            addUserControlForPanel(userControl_Setting);
        }



        // button thoát chương trình
        private void formSellNhanVien_LogoutClicked(object sender, EventArgs e)
        {
            // Khi sự kiện đăng xuất được kích hoạt, đóng FormC
            DangNhap dangNhap = new DangNhap();
            dangNhap.Show();

            this.Close();


        }
    }
}

//using QLCF.MainForm;
using QLCF.NhanVienForm;
using QLCF.NhanVienForm.user_SanPham;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLCF
{
    internal static class Program
    {
        
        
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);



            Application.Run(new DangNhap());//FormSellNhanVien()MainQuanLy()DK_TaiKhoan()DangNhap()DK_TaiKhoan()DK_DatBan()

        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLCF.NhanVienForm.user_SanPham
{
    internal class TruyenData
    {
        private static TruyenData instance;


        // Sự kiện để thông báo khi dữ liệu thay đổi
        public event EventHandler DataChanged;

        //private string[] sharedData;
        private List<string[]> sharedData;
        
        //lượt mua hàng sẽ tăng theo số lượng hóa đơn được lập
        private int LuotMuaHang = 1;
        public int _LuotMuaHang { get; set; }

        //lưu tài khoản (mã nhân viên)  từ form đăng nhập -> hiển thị trong form cài đặt
        private string loginTK;
        public string _LoginTK { get; set; }
        private string tenNhanVien;
        public string _TenNhanVien { get; set; }

        // truyền dữ liệu của bảng hóa đơn của form nhân viên qua bảng hóa đơn của form quản lý
        private DataTable dataHoaDonNV;
        public DataTable _DataHoaDonNV { get; set; }


        // Dữ liệu cần truyền giữa các form
        //public string[] SharedData
        public List<string[]> SharedData
        {
            get => sharedData;
            set
            {
                sharedData = value;
                // Khi dữ liệu thay đổi, kích hoạt sự kiện DataChanged
                OnDataChanged();
            }
        }

        // Đảm bảo không thể tạo nhiều thể hiện của lớp này
        private TruyenData()
        {
        }

        // Phương thức để lấy thể hiện của lớp
        public static TruyenData Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TruyenData();
                }
                return instance;
            }
        }

        // Phương thức để kích hoạt sự kiện DataChanged
        protected virtual void OnDataChanged()
        {
            DataChanged?.Invoke(this, EventArgs.Empty);
            //Console.WriteLine("tại sao con khóc");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;
using QLCF.NhanVienForm.user_SanPham;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.Data;

namespace QLCF
{
    internal class LoadDatabase
    {
        // Biến tĩnh để giữ thể hiện duy nhất của lớp
        public static LoadDatabase instance;

        public SqlConnection sqlConnect = null;// đối tượng kết nối
        public static string connectionString = "Data Source=DESKTOP-39BMSC8\\GIAKHANH;Initial Catalog=QL_QuanCF;Integrated Security=True";


        // Để tránh việc tạo thể hiện thông qua việc new từ bên ngoài
        private LoadDatabase() { }


        // Phương thức để lấy thể hiện của lớp
        public static LoadDatabase Instance
        {
            get
            {
                // Nếu chưa có thể hiện nào được tạo, tạo mới một thể hiện
                if (instance == null)
                {
                    instance = new LoadDatabase();
                }
                // Trả về thể hiện duy nhất
                return instance;
            }
        }

    }
}

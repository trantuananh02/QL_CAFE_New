using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace QL_CAFE.Controllers
{  // Biến kết nối SQL
    public class KetNoiCSDL
    {
        // Đảm bảo kết nối là readonly và sử dụng chuỗi kết nối đúng
        protected SqlConnection conn;

        // Constructor
        public KetNoiCSDL()
        {
            try
            {
                // Chuỗi kết nối mới
                string connectionString = @"Data Source=STORKTRAN;Initial Catalog=quanly_quancafe ;Integrated Security=True";

                // Khởi tạo kết nối
                conn = new SqlConnection(connectionString);

                // Mở kết nối
                conn.Open();
                Console.WriteLine("Kết nối thành công");
            }
            catch (Exception ex)
            {
                // Xử lý lỗi kết nối
                Console.WriteLine("Lỗi kết nối: " + ex.Message);
                Console.WriteLine(ex.StackTrace); // In chi tiết lỗi để dễ dàng xác định
                Console.WriteLine("Lỗi kết nối: " + ex.Message);
                Console.WriteLine(ex.StackTrace); // In chi tiết lỗi để dễ dàng xác định
                 Console.WriteLine("Lỗi kết nối: " + ex.Message);
                Console.WriteLine(ex.StackTrace); // In chi tiết lỗi để dễ dàng xác định
            }
        }

        // Phương thức trả về kết nối
        public SqlConnection GetConnection()
        {
            return conn;
        }


    }
}
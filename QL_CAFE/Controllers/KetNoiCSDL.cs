using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace QL_CAFE.Controllers
{
    public class KetNoiCSDL
    {
        // Biến kết nối SQL
        protected SqlConnection conn;

        // Phương thức khởi tạo đối tượng kết nối
        public void ketNoiCSDL()
        {
            // Chuỗi kết nối đến cơ sở dữ liệu
            string connectionString = "Data Source=MSI\\SQLEXPRESS;Initial Catalog=quanly_quancafe;Integrated Security=True;";

            // Tạo đối tượng kết nối
            conn = new SqlConnection(connectionString);

            try
            {
                // Mở kết nối
                conn.Open();
                Console.WriteLine("Kết nối cơ sở dữ liệu thành công!");
            }
            catch (Exception ex)
            {
                // Xử lý lỗi khi kết nối
                Console.WriteLine("Lỗi khi kết nối cơ sở dữ liệu: " + ex.Message);
            }
        }
    }
        
    }
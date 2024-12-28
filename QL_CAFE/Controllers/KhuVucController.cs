using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QL_CAFE.Models;

namespace QL_CAFE.Controllers
{
    public class KhuVucController : KetNoiCSDL
    {
        public List<string> HienThiDanhSachKhuVuc()
        {
            // Khai báo danh sách các khu vực
            List<string> danhSachKhuVuc = new List<string>();

            try
            {
                // Câu truy vấn để lấy danh sách khu vực
                string query = "SELECT TenKhuVuc FROM KhuVuc";

                // Thực thi truy vấn
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        // Đọc dữ liệu và thêm tên khu vực vào danh sách
                        while (reader.Read())
                        {
                            // Thêm tên khu vực vào danh sách
                            danhSachKhuVuc.Add(reader.GetString(0));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Có lỗi xảy ra: {ex.Message}");
            }
            return danhSachKhuVuc;
        }
    }
}
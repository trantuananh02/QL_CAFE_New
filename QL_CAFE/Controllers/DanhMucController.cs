using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QL_CAFE.Models;

namespace QL_CAFE.Controllers
{
    public class DanhMucController : KetNoiCSDL
    {
        public List<string> HienThiDanhSachDanhMuc()
        {
            // Khai báo danh sách các danh mục
            List<string> danhSachDanhMuc = new List<string>();

            try
            {
                // Câu truy vấn để lấy danh sách danh mục
                string query = "SELECT TenDanhMuc FROM DanhMuc";

                // Thực thi truy vấn
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        // Đọc dữ liệu và thêm tên danh mục vào danh sách
                        while (reader.Read())
                        {
                            // Thêm tên danh mục vào danh sách
                            danhSachDanhMuc.Add(reader.GetString(0));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Có lỗi xảy ra: {ex.Message}");
            }
            return danhSachDanhMuc;
        }
    }
}

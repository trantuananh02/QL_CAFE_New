using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using QL_CAFE.Models;

namespace QL_CAFE.Controllers
{
    public class KhuVucController : KetNoiCSDL
    {
        public List<KhuVucModel> HienThiDanhSachKhuVuc()
        {
            // Khai báo danh sách các khu vực
            List<KhuVucModel> danhSachKhuVuc = new List<KhuVucModel>();

            try
            {
                // Câu truy vấn để lấy danh sách khu vực
                string query = "SELECT KhuVucID, TenKhuVuc FROM KhuVuc";

                // Mở kết nối
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();
                }

                // Thực thi truy vấn
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        // Đọc dữ liệu và thêm vào danh sách
                        while (reader.Read())
                        {
                            // Tạo đối tượng KhuVucModel từ dữ liệu đọc được
                            KhuVucModel khuVuc = new KhuVucModel
                            {
                                KhuVucID = reader.GetInt32(0), // Cột đầu tiên là KhuVucID (kiểu int)
                                TenKhuVuc = reader.GetString(1) // Cột thứ hai là TenKhuVuc (kiểu string)
                            };

                            // Thêm khu vực vào danh sách
                            danhSachKhuVuc.Add(khuVuc);
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

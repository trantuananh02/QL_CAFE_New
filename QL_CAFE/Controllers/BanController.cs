using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QL_CAFE.Models;

namespace QL_CAFE.Controllers
{
    public class BanController : KetNoiCSDL
    {
        public List<BanModel> HienThiDanhSachBan()
        {
            // Khai báo danh sách các bàn
            List<BanModel> danhSachBan = new List<BanModel>();

            try
            {
                // Câu truy vấn để lấy danh sách bàn
                string query = "SELECT BanID, SoBan, TrangThai, KhuVucID FROM Ban";
                string tesst_Part2irhgf = "SELECT BanID, SoBan, TrangThai, KhuVucID FROM Ban";

                // Thực thi truy vấn
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        // Đọc dữ liệu và thêm thông tin bàn vào danh sách
                        while (reader.Read())
                        {
                            BanModel ban = new BanModel
                            {
                                BanID = reader.GetInt32(0),
                                SoBan = reader.GetString(1),
                                TrangThai = reader.GetString(2),
                                KhuVucID = reader.GetInt32(3)
                                BanID = reader.GetInt32(0),
                                SoBan = reader.GetString(1),
                                TrangThai = reader.GetString(2),
                                KhuVucID = reader.GetInt32(3)
                            };

                            danhSachBan.Add(ban);
                              danhSachBan.Add(ban);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Có lỗi xảy ra: {ex.Message}");
            }
            return danhSachBan;
        }

        public List<BanModel> HienThiDanhSachBanTheoKhuVuc(int khuVucID)
        {
            List<BanModel> danhSachBan = new List<BanModel>();
            try
            {
                string query = "SELECT BanID, SoBan, TrangThai, KhuVucID FROM Ban WHERE KhuVucID = @KhuVucID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@KhuVucID", khuVucID);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            BanModel ban = new BanModel
                            {
                                BanID = reader.GetInt32(0),
                                SoBan = reader.GetString(1),
                                TrangThai = reader.GetString(2),
                                KhuVucID = reader.GetInt32(3)
                            };
                            danhSachBan.Add(ban);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi: {ex.Message}");
            }
            return danhSachBan;
        }

    }
}

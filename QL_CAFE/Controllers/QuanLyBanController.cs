using QL_CAFE.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Runtime.Remoting.Contexts;
using System.Windows.Forms;

namespace QL_CAFE.Controllers
{
    public class QuanLyBanController : KetNoiCSDL
    {
        public List<BanModel> HienThiDanhSachBan()
        {
            // Khai báo danh sách các bàn
            List<BanModel> danhSachBan = new List<BanModel>();

            try
            {
                // Câu truy vấn để lấy danh sách bàn
                string query = "SELECT BanID, SoBan, TrangThai, KhuVucID FROM Ban";

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
                            };

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

        public List<BanModel> HienThiDanhSachBanTheoKhuVuc(int? khuVucID)
        {
            List<BanModel> danhSachBan = new List<BanModel>();
            try
            {
                string query;
                if (khuVucID.HasValue)
                {
                    // Truy vấn theo khu vực
                    query = "SELECT BanID, SoBan, TrangThai, KhuVucID FROM Ban WHERE KhuVucID = @KhuVucID";
                }
                else
                {
                    // Truy vấn tất cả
                    query = "SELECT BanID, SoBan, TrangThai, KhuVucID FROM Ban";
                }

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (khuVucID.HasValue)
                    {
                        cmd.Parameters.AddWithValue("@KhuVucID", khuVucID.Value);
                    }

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



        public bool ThemBan(BanModel ban)
        {
            string sql = "INSERT INTO Ban (SoBan, TrangThai, KhuVucID) VALUES (@SoBan, @TrangThai, @KhuVucID)";
            try
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    if (conn.State == System.Data.ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    cmd.Parameters.AddWithValue("@SoBan", ban.SoBan);
                    cmd.Parameters.AddWithValue("@TrangThai", ban.TrangThai);
                    cmd.Parameters.AddWithValue("@KhuVucID", ban.KhuVucID);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi thêm bàn: " + ex.Message);
                return false;
            }
        }
        public bool KiemTraBanExist(int maBan)
        {
            string query = "SELECT COUNT(*) FROM Ban WHERE BanID = @BanID";
            int count = ExecuteScalar(query, new SqlParameter("@BanID", maBan));

            return count > 0;
        }
        private int ExecuteScalar(string query, SqlParameter parameter)
        {

            using (var command = new SqlCommand(query, conn))
            {
                command.Parameters.Add(parameter);
                return (int)command.ExecuteScalar();  // Trả về số lượng dòng được trả về từ câu truy vấn
            }
        }
        public bool XoaBan(int banID)
        {
            string sql = "DELETE FROM Ban WHERE BanID = @BanID";

            try
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    if (conn.State == System.Data.ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    cmd.Parameters.AddWithValue("@BanID", banID);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi khi xóa bàn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public bool CapNhatBan(BanModel ban)
        {
            string sql = "UPDATE Ban SET SoBan = @SoBan, TrangThai = @TrangThai, KhuVucID = @KhuVucID WHERE BanID = @BanID";
            try
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    if (conn.State == System.Data.ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    cmd.Parameters.AddWithValue("@SoBan", ban.SoBan);
                    cmd.Parameters.AddWithValue("@TrangThai", ban.TrangThai);
                    cmd.Parameters.AddWithValue("@KhuVucID", ban.KhuVucID);
                    cmd.Parameters.AddWithValue("@BanID", ban.BanID);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi cập nhật bàn: " + ex.Message);
                return false;
            }
        }

        public bool KiemTraBanTonTai(int banID)
        {
            string sql = "SELECT COUNT(*) FROM Ban WHERE BanID = @BanID";
            try
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    if (conn.State == System.Data.ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    cmd.Parameters.AddWithValue("@BanID", banID);
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi kiểm tra bàn tồn tại: " + ex.Message);
                return false;
            }
        }
    }
}

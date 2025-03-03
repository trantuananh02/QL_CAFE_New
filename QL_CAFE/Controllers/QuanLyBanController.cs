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
        public void GopBan(int ban1ID, int ban2ID)
        {
            try
            {
                // 1. Lấy hóa đơn của bàn 1
                string queryHoaDonBan1 = "SELECT HoaDonID, TongTien FROM HoaDon WHERE BanID = @Ban1ID AND TrangThai = 'Chưa Thanh Toán'";
                SqlCommand cmdHoaDonBan1 = new SqlCommand(queryHoaDonBan1, conn);
                cmdHoaDonBan1.Parameters.AddWithValue("@Ban1ID", ban1ID);

                HoaDonModel hoaDonBan1 = null;

                using (SqlDataReader reader = cmdHoaDonBan1.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        hoaDonBan1 = new HoaDonModel
                        {
                            HoaDonID = reader.GetInt32(0),
                            TongTien = reader.GetDecimal(1)
                        };
                    }
                }

                if (hoaDonBan1 == null)
                {
                    MessageBox.Show("Không tìm thấy hóa đơn cho bàn 1.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 2. Lấy hoặc tạo hóa đơn cho bàn 2
                string queryHoaDonBan2 = "SELECT HoaDonID, TongTien FROM HoaDon WHERE BanID = @Ban2ID AND TrangThai = 'Chưa Thanh Toán'";
                SqlCommand cmdHoaDonBan2 = new SqlCommand(queryHoaDonBan2, conn);
                cmdHoaDonBan2.Parameters.AddWithValue("@Ban2ID", ban2ID);

                HoaDonModel hoaDonBan2 = null;

                using (SqlDataReader reader = cmdHoaDonBan2.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        hoaDonBan2 = new HoaDonModel
                        {
                            HoaDonID = reader.GetInt32(0),
                            TongTien = reader.GetDecimal(1)
                        };
                    }
                }

                if (hoaDonBan2 == null)
                {
                    MessageBox.Show("Không tìm thấy hóa đơn cho bàn 2.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 3. Lấy tất cả các món ăn trong chi tiết hóa đơn của bàn 1
                string queryChiTietBan1 = "SELECT DoAnUongID, SoLuong, Gia FROM ChiTietHoaDon WHERE HoaDonID = @HoaDonID";
                SqlCommand cmdChiTietBan1 = new SqlCommand(queryChiTietBan1, conn);
                cmdChiTietBan1.Parameters.AddWithValue("@HoaDonID", hoaDonBan1.HoaDonID);

                List<ChiTietHoaDonModel> chiTietHoaDonBan1 = new List<ChiTietHoaDonModel>();

                using (SqlDataReader reader = cmdChiTietBan1.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        chiTietHoaDonBan1.Add(new ChiTietHoaDonModel
                        {
                            DoAnUongID = reader.GetInt32(0),
                            SoLuong = reader.GetInt32(1),
                            Gia = reader.GetDecimal(2)
                        });
                    }
                }

                // 4. Thêm các món ăn từ bàn 1 vào hóa đơn bàn 2
                foreach (var chiTiet in chiTietHoaDonBan1)
                {
                    // Kiểm tra xem món ăn đã có trong hóa đơn bàn 2 chưa
                    string queryCheckExist = "SELECT COUNT(*) FROM ChiTietHoaDon WHERE HoaDonID = @HoaDonID AND DoAnUongID = @DoAnUongID";
                    SqlCommand cmdCheckExist = new SqlCommand(queryCheckExist, conn);
                    cmdCheckExist.Parameters.AddWithValue("@HoaDonID", hoaDonBan2.HoaDonID);
                    cmdCheckExist.Parameters.AddWithValue("@DoAnUongID", chiTiet.DoAnUongID);

                    int count = (int)cmdCheckExist.ExecuteScalar();

                    if (count > 0)
                    {
                        // Nếu món ăn đã có trong hóa đơn bàn 2, chỉ cần cập nhật số lượng
                        string queryUpdateSoLuong = "UPDATE ChiTietHoaDon SET SoLuong = SoLuong + @SoLuong WHERE HoaDonID = @HoaDonID AND DoAnUongID = @DoAnUongID";
                        SqlCommand cmdUpdateSoLuong = new SqlCommand(queryUpdateSoLuong, conn);
                        cmdUpdateSoLuong.Parameters.AddWithValue("@HoaDonID", hoaDonBan2.HoaDonID);
                        cmdUpdateSoLuong.Parameters.AddWithValue("@DoAnUongID", chiTiet.DoAnUongID);
                        cmdUpdateSoLuong.Parameters.AddWithValue("@SoLuong", chiTiet.SoLuong);

                        cmdUpdateSoLuong.ExecuteNonQuery();
                    }
                    else
                    {
                        // Nếu món ăn chưa có trong hóa đơn bàn 2, thì thêm món ăn mới
                        string queryInsertChiTiet = "INSERT INTO ChiTietHoaDon (HoaDonID, DoAnUongID, SoLuong, Gia) VALUES (@HoaDonID, @DoAnUongID, @SoLuong, @Gia)";
                        SqlCommand cmdInsertChiTiet = new SqlCommand(queryInsertChiTiet, conn);
                        cmdInsertChiTiet.Parameters.AddWithValue("@HoaDonID", hoaDonBan2.HoaDonID);
                        cmdInsertChiTiet.Parameters.AddWithValue("@DoAnUongID", chiTiet.DoAnUongID);
                        cmdInsertChiTiet.Parameters.AddWithValue("@SoLuong", chiTiet.SoLuong);
                        cmdInsertChiTiet.Parameters.AddWithValue("@Gia", chiTiet.Gia);

                        cmdInsertChiTiet.ExecuteNonQuery();
                    }
                }

                using (SqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // Kiểm tra nếu bảng ChiTietHoaDon có các bản ghi tham chiếu đến HoaDonID của bàn 1
                        string queryCheckChiTiet = "SELECT COUNT(*) FROM ChiTietHoaDon WHERE HoaDonID = @HoaDonID";
                        SqlCommand cmdCheckChiTiet = new SqlCommand(queryCheckChiTiet, conn, transaction);
                        cmdCheckChiTiet.Parameters.AddWithValue("@HoaDonID", hoaDonBan1.HoaDonID);

                        int count = (int)cmdCheckChiTiet.ExecuteScalar();
                        if (count > 0)
                        {
                            // Nếu có bản ghi trong ChiTietHoaDon, xóa các bản ghi này trước
                            string queryXoaChiTietHoaDon = "DELETE FROM ChiTietHoaDon WHERE HoaDonID = @HoaDonID";
                            SqlCommand cmdXoaChiTietHoaDon = new SqlCommand(queryXoaChiTietHoaDon, conn, transaction);
                            cmdXoaChiTietHoaDon.Parameters.AddWithValue("@HoaDonID", hoaDonBan1.HoaDonID);
                            cmdXoaChiTietHoaDon.ExecuteNonQuery();
                        }

                        // Xóa hóa đơn của bàn 1
                        string queryXoaHoaDonBan1 = "DELETE FROM HoaDon WHERE HoaDonID = @HoaDonID";
                        SqlCommand cmdXoaHoaDonBan1 = new SqlCommand(queryXoaHoaDonBan1, conn, transaction);
                        cmdXoaHoaDonBan1.Parameters.AddWithValue("@HoaDonID", hoaDonBan1.HoaDonID);
                        cmdXoaHoaDonBan1.ExecuteNonQuery();

                        // Cập nhật trạng thái bàn 1 thành "Trống"
                        string queryCapNhatTrangThaiBan = "UPDATE Ban SET TrangThai = N'Trống' WHERE BanID = @BanID";
                        SqlCommand cmdCapNhatTrangThaiBan = new SqlCommand(queryCapNhatTrangThaiBan, conn, transaction);
                        cmdCapNhatTrangThaiBan.Parameters.AddWithValue("@BanID", ban1ID);
                        cmdCapNhatTrangThaiBan.ExecuteNonQuery();

                        // Cam kết giao dịch sau khi xóa xong
                        transaction.Commit();

                        MessageBox.Show("Gộp bàn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        // Nếu có lỗi, hủy bỏ giao dịch
                        transaction.Rollback();
                        MessageBox.Show($"Đã xảy ra lỗi khi gộp bàn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

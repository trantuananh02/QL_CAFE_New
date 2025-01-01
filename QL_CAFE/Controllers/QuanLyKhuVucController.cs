using QL_CAFE.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace QL_CAFE.Controllers
{
    public class QuanLyKhuVucController : KetNoiCSDL
    {
        // Hiển thị danh sách khu vực
        public List<KhuVucModel> HienThiDanhSachKhuVuc()
        {
            List<KhuVucModel> danhSachKhuVuc = new List<KhuVucModel>();

            string sql = "SELECT KhuVucID, TenKhuVuc FROM KhuVuc";

            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();
                }

                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    KhuVucModel khuVuc = new KhuVucModel
                    {
                        KhuVucID = reader.GetInt32(reader.GetOrdinal("KhuVucID")),
                        TenKhuVuc = reader.GetString(reader.GetOrdinal("TenKhuVuc"))
                    };

                    danhSachKhuVuc.Add(khuVuc);
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi: " + ex.Message);
            }

            return danhSachKhuVuc;
        }

        // Thêm khu vực
        public bool ThemKhuVuc(KhuVucModel khuVuc)
        {
            string sql = "INSERT INTO KhuVuc (TenKhuVuc) VALUES (@TenKhuVuc)";

            try
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    if (conn.State == System.Data.ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    cmd.Parameters.AddWithValue("@TenKhuVuc", khuVuc.TenKhuVuc);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi thêm khu vực: " + ex.Message);
                return false;
            }
        }

        // Xóa khu vực
        public bool XoaKhuVuc(int khuVucID)
        {
            string sql = "DELETE FROM KhuVuc WHERE KhuVucID = @KhuVucID";

            try
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    if (conn.State == System.Data.ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    cmd.Parameters.AddWithValue("@KhuVucID", khuVucID);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi khi xóa khu vực: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        // Cập nhật khu vực
        public bool CapNhatKhuVuc(KhuVucModel khuVuc)
        {
            string sql = "UPDATE KhuVuc SET TenKhuVuc = @TenKhuVuc WHERE KhuVucID = @KhuVucID";

            try
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    if (conn.State == System.Data.ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    cmd.Parameters.AddWithValue("@TenKhuVuc", khuVuc.TenKhuVuc);
                    cmd.Parameters.AddWithValue("@KhuVucID", khuVuc.KhuVucID);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi cập nhật khu vực: " + ex.Message);
                return false;
            }
        }

        // Kiểm tra khu vực tồn tại
        public bool KiemTraKhuVucTonTai(int khuVucID)
        {
            string sql = "SELECT COUNT(*) FROM KhuVuc WHERE KhuVucID = @KhuVucID";

            try
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    if (conn.State == System.Data.ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    cmd.Parameters.AddWithValue("@KhuVucID", khuVucID);
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi kiểm tra khu vực tồn tại: " + ex.Message);
                return false;
            }
        }

        // Kiểm tra khu vực có bàn hay không
        public bool KiemTraKhuVucCoBan(int khuVucID)
        {
            string sql = "SELECT COUNT(*) FROM Ban WHERE KhuVucID = @KhuVucID";

            try
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    if (conn.State == System.Data.ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    cmd.Parameters.AddWithValue("@KhuVucID", khuVucID);
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi kiểm tra khu vực có bàn: " + ex.Message);
                return false;
            }
        }

        // Xóa tất cả bàn trong khu vực
        public bool XoaTatCaBanTrongKhuVuc(int khuVucID)
        {
            string sql = "DELETE FROM Ban WHERE KhuVucID = @KhuVucID";

            try
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    if (conn.State == System.Data.ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    cmd.Parameters.AddWithValue("@KhuVucID", khuVucID);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi xóa tất cả bàn trong khu vực: " + ex.Message);
                return false;
            }
        }
    }
}

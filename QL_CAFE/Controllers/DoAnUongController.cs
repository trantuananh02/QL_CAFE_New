using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using QL_CAFE.Models;

namespace QL_CAFE.Controllers
{
    public class DoAnUongController : KetNoiCSDL
    {
        // Hiển thị danh sách các món ăn uống
        public List<DoAnUongModel> HienThiDanhSachDoAnUong()
        {
            List<DoAnUongModel> danhSachDoAnUong = new List<DoAnUongModel>();

            try
            {
                // Câu truy vấn để lấy danh sách đồ ăn uống
                string query = "SELECT DoAnUongID, TenDoAnUong, Gia, DanhMucID FROM DoAnUong";

                // Thực thi truy vấn
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        // Đọc dữ liệu và thêm thông tin món ăn uống vào danh sách
                        while (reader.Read())
                        {
                            DoAnUongModel doAnUong = new DoAnUongModel
                            {
                                DoAnUongID = reader.GetInt32(0),
                                TenDoAnUong = reader.GetString(1),
                                Gia = reader.GetDecimal(2),
                                DanhMucID = reader.GetInt32(3)
                            };

                            danhSachDoAnUong.Add(doAnUong);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Có lỗi xảy ra: {ex.Message}");
            }

            return danhSachDoAnUong;
        }

        // Hiển thị danh sách món ăn uống theo danh mục
        public List<DoAnUongModel> HienThiDanhSachDoAnUongTheoDanhMuc(int? danhMucID)
        {
            List<DoAnUongModel> dsDoAnUong = new List<DoAnUongModel>();
            try
            {
                string query;

                if (danhMucID.HasValue)
                {
                    // Truy vấn theo danh mục
                    query = "SELECT DoAnUongID, TenDoAnUong, Gia, DanhMucID FROM DoAnUong WHERE DanhMucID = @DanhMucID";
                }
                else
                {
                    // Truy vấn tất cả
                    query = "SELECT DoAnUongID, TenDoAnUong, Gia, DanhMucID FROM DoAnUong";
                }

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (danhMucID.HasValue)
                    {
                        cmd.Parameters.AddWithValue("@DanhMucID", danhMucID.Value);
                    }

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DoAnUongModel doAnUong = new DoAnUongModel
                            {
                                DoAnUongID = reader.GetInt32(0),
                                TenDoAnUong = reader.GetString(1),
                                Gia = reader.GetDecimal(2),  // Đọc giá từ cơ sở dữ liệu
                                DanhMucID = reader.GetInt32(3)  // Đọc DanhMucID
                            };
                            dsDoAnUong.Add(doAnUong);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi: {ex.Message}");
            }

            return dsDoAnUong;
        }

        public bool KiemTraDoAnUongTonTai(int doAnUongID)
        {
            try
            {
                string query = "SELECT COUNT(*) FROM DoAnUong WHERE DoAnUongID = @DoAnUongID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@DoAnUongID", doAnUongID);
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi kiểm tra món ăn/uống tồn tại: " + ex.Message);
            }
        }
        public bool ThemDoAnUong(DoAnUongModel doAnUong)
        {
            try
            {
                string query = "INSERT INTO DoAnUong (TenDoAnUong, Gia, DanhMucID) VALUES (@TenDoAnUong, @Gia, @DanhMucID)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@TenDoAnUong", doAnUong.TenDoAnUong);
                    cmd.Parameters.AddWithValue("@Gia", doAnUong.Gia);
                    cmd.Parameters.AddWithValue("@DanhMucID", doAnUong.DanhMucID);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi thêm món ăn/uống: " + ex.Message);
            }
        }
        public bool CapNhatDoAnUong(DoAnUongModel doAnUong)
        {
            try
            {
                string query = "UPDATE DoAnUong SET TenDoAnUong = @TenDoAnUong, Gia = @Gia, DanhMucID = @DanhMucID WHERE DoAnUongID = @DoAnUongID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@TenDoAnUong", doAnUong.TenDoAnUong);
                    cmd.Parameters.AddWithValue("@Gia", doAnUong.Gia);
                    cmd.Parameters.AddWithValue("@DanhMucID", doAnUong.DanhMucID);
                    cmd.Parameters.AddWithValue("@DoAnUongID", doAnUong.DoAnUongID);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi cập nhật món ăn/uống: " + ex.Message);
            }
        }
        public bool XoaDoAnUong(int doAnUongID)
        {
            try
            {
                string query = "DELETE FROM DoAnUong WHERE DoAnUongID = @DoAnUongID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@DoAnUongID", doAnUongID);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi xóa món ăn/uống: " + ex.Message);
            }
        }

    }
}

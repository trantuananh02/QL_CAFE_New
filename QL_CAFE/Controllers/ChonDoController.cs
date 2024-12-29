using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using QL_CAFE.Models;

namespace QL_CAFE.Controllers
{
    public class ChonDoController : KetNoiCSDL
    {
        // Hiển thị danh sách các món ăn uống
        public List<ChonDoModel> HienThiDanhSachDoAnUong()
        {
            List<ChonDoModel> danhSachDoAnUong = new List<ChonDoModel>();

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
                            ChonDoModel doAnUong = new ChonDoModel
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
        public List<ChonDoModel> HienThiDanhSachDoAnUongTheoDanhMuc(int danhMucID)
        {
            List<ChonDoModel> dsDoAnUong = new List<ChonDoModel>();

            // Truy vấn cơ sở dữ liệu để lấy danh sách đồ ăn uống theo DanhMucID
            string query = "SELECT * FROM DoAnUong WHERE DanhMucID = @DanhMucID";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@DanhMucID", danhMucID);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ChonDoModel doAnUong = new ChonDoModel
                        {
                            DoAnUongID = Convert.ToInt32(reader["DoAnUongID"]),
                            TenDoAnUong = reader["TenDoAnUong"].ToString(),
                        };
                        dsDoAnUong.Add(doAnUong);
                    }
                }

                return dsDoAnUong;
            }

        }
    }
}

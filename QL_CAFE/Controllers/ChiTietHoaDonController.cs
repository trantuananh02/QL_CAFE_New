using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QL_CAFE.Models;

namespace QL_CAFE.Controllers
{
    public class ChiTietHoaDonController : KetNoiCSDL
    {
        // Hàm lấy danh sách món ăn đã chọn của bàn
        public List<ChiTietHoaDonModel> LayMonAnDaChonCuaBan(int banID)
        {
            List<ChiTietHoaDonModel> chiTietHoaDonList = new List<ChiTietHoaDonModel>();
            try
            {
                // Sử dụng kết nối từ lớp KetNoiCSDL
                string query = @"
                    SELECT ct.ChiTietID, ct.HoaDonID, ct.DoAnUongID, ct.SoLuong, ct.Gia
                    FROM ChiTietHoaDon ct
                    INNER JOIN HoaDon h ON ct.HoaDonID = h.HoaDonID
                    WHERE h.BanID = @BanID AND h.TrangThai = 'Chưa Thanh Toán'
                ";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    // Thêm tham số cho câu truy vấn
                    cmd.Parameters.AddWithValue("@BanID", banID);

                    // Thực thi câu truy vấn và lấy kết quả
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ChiTietHoaDonModel chiTiet = new ChiTietHoaDonModel
                            {
                                ChiTietID = Convert.ToInt32(reader["ChiTietID"]),
                                HoaDonID = Convert.ToInt32(reader["HoaDonID"]),
                                DoAnUongID = Convert.ToInt32(reader["DoAnUongID"]),
                                SoLuong = Convert.ToInt32(reader["SoLuong"]),
                                Gia = Convert.ToDecimal(reader["Gia"])
                            };

                            chiTietHoaDonList.Add(chiTiet);
                        }
                    }
                }

                return chiTietHoaDonList;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi lấy dữ liệu: " + ex.Message);
                return null;
            }
        }
    }
}

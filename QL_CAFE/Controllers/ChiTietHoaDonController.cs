using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QL_CAFE.Models;

namespace QL_CAFE.Controllers
{
    public class ChiTietHoaDonController : KetNoiCSDL
    {
        // Hàm lấy danh sách món ăn đã chọn của bàn
        public List<ChiTietHoaDonModel> LayChiTietHoaDonTheoBan(int banID)
        {
            List<ChiTietHoaDonModel> chiTietHoaDonList = new List<ChiTietHoaDonModel>();

            string query = "SELECT cthd.ChiTietID, cthd.HoaDonID, da.TenDoAnUong, cthd.SoLuong, cthd.Gia " +
                           "FROM ChiTietHoaDon cthd " +
                           "JOIN HoaDon hd ON cthd.HoaDonID = hd.HoaDonID " +
                           "JOIN DoAnUong da ON cthd.DoAnUongID = da.DoAnUongID " +
                           "JOIN Ban b ON hd.BanID = b.BanID " +
                           "WHERE b.BanID = @BanID AND hd.TrangThai = N'Chưa Thanh Toán'";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@BanID", banID);

            try
            {
                
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ChiTietHoaDonModel chiTiet = new ChiTietHoaDonModel
                    {
                        ChiTietID = (int)reader["ChiTietID"],
                        HoaDonID = (int)reader["HoaDonID"],
                        TenDoAnUong = (string)reader["TenDoAnUong"],
                        SoLuong = (int)reader["SoLuong"],
                        Gia = (decimal)reader["Gia"]
                    };
                    chiTietHoaDonList.Add(chiTiet);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lấy chi tiết hóa đơn theo bàn: {ex.Message}");
            }
           

            return chiTietHoaDonList;
        }
        public bool ThemMonVaoChiTietHoaDon(int hoaDonID, int doAnUongID, int soLuong)
        {
            try
            {
                decimal gia = 0;

                // Lấy giá của món ăn từ bảng DoAnUong
                string queryGia = "SELECT Gia FROM DoAnUong WHERE DoAnUongID = @DoAnUongID";
                SqlCommand cmdGia = new SqlCommand(queryGia, conn);
                cmdGia.Parameters.AddWithValue("@DoAnUongID", doAnUongID);

                // Thực thi câu lệnh và lấy giá trị
                SqlDataReader readerGia = cmdGia.ExecuteReader();
                if (readerGia.Read()) // Kiểm tra nếu có dữ liệu trả về
                {
                    gia = (decimal)readerGia["Gia"];
                }
                readerGia.Close(); // Đóng SqlDataReader sau khi sử dụng

                // Kiểm tra xem giá có hợp lệ không
                if (gia == 0)
                {
                    MessageBox.Show("Không tìm thấy giá của món ăn hoặc giá không hợp lệ.");
                    return false; // Trả về false nếu không có giá hợp lệ
                }

                // Thêm món ăn vào ChiTietHoaDon
                string queryInsert = "INSERT INTO ChiTietHoaDon (HoaDonID, DoAnUongID, SoLuong, Gia) " +
                                     "VALUES (@HoaDonID, @DoAnUongID, @SoLuong, @Gia)";
                SqlCommand cmdInsert = new SqlCommand(queryInsert, conn);
                cmdInsert.Parameters.AddWithValue("@HoaDonID", hoaDonID);
                cmdInsert.Parameters.AddWithValue("@DoAnUongID", doAnUongID);
                cmdInsert.Parameters.AddWithValue("@SoLuong", soLuong);
                cmdInsert.Parameters.AddWithValue("@Gia", gia);

                // Thực thi câu lệnh và kiểm tra kết quả
                int rowsAffected = cmdInsert.ExecuteNonQuery();

                // Cập nhật tổng tiền trong HoaDon
                if (rowsAffected > 0)
                {
                    string queryUpdateTongTien = "UPDATE HoaDon " +
                                                 "SET TongTien = (SELECT SUM(SoLuong * Gia) FROM ChiTietHoaDon WHERE HoaDonID = @HoaDonID) " +
                                                 "WHERE HoaDonID = @HoaDonID";
                    SqlCommand cmdUpdateTongTien = new SqlCommand(queryUpdateTongTien, conn);
                    cmdUpdateTongTien.Parameters.AddWithValue("@HoaDonID", hoaDonID);
                    cmdUpdateTongTien.ExecuteNonQuery();
                }

                return rowsAffected > 0; // Trả về true nếu thêm thành công, ngược lại trả về false
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm món vào chi tiết hóa đơn: {ex.Message}");
                return false; // Trả về false nếu có lỗi
            }
        }
        public bool XoaMonKhoiChiTietHoaDon(int chiTietID, int hoaDonID)
        {
            try
            {
                // Xóa món khỏi ChiTietHoaDon
                string queryDelete = "DELETE FROM ChiTietHoaDon WHERE ChiTietID = @ChiTietID";
                SqlCommand cmdDelete = new SqlCommand(queryDelete, conn);
                cmdDelete.Parameters.AddWithValue("@ChiTietID", chiTietID);

                // Thực thi câu lệnh xóa
                int rowsAffected = cmdDelete.ExecuteNonQuery();

                // Nếu xóa thành công, cập nhật lại tổng tiền trong hóa đơn
                if (rowsAffected > 0)
                {
                    string queryUpdateTongTien = "UPDATE HoaDon " +
                                                 "SET TongTien = (SELECT SUM(SoLuong * Gia) FROM ChiTietHoaDon WHERE HoaDonID = @HoaDonID) " +
                                                 "WHERE HoaDonID = @HoaDonID";
                    SqlCommand cmdUpdateTongTien = new SqlCommand(queryUpdateTongTien, conn);
                    cmdUpdateTongTien.Parameters.AddWithValue("@HoaDonID", hoaDonID);
                    cmdUpdateTongTien.ExecuteNonQuery();
                }

                return rowsAffected > 0; // Trả về true nếu xóa thành công
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa món khỏi chi tiết hóa đơn: {ex.Message}");
                return false; // Trả về false nếu có lỗi
            }
        }

        public int LayChiTietIDTheoHoaDon(int hoaDonID)
        {
            int chiTietID = -1; // Giá trị mặc định nếu không tìm thấy

            string query = "SELECT ChiTietID FROM ChiTietHoaDon WHERE HoaDonID = @HoaDonID";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@HoaDonID", hoaDonID);

            try
            {
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    chiTietID = (int)reader["ChiTietID"];
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lấy chi tiết ID theo hóa đơn: {ex.Message}");
            }

            return chiTietID;
        }



    }
}

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
        public int LayHoaDonIDTheoBan(int banID)
        {
            int hoaDonID = 0;
            string query = "SELECT HoaDonID FROM HoaDon WHERE BanID = @BanID AND TrangThai = N'Chưa Thanh Toán'";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@BanID", banID);

            try
            {
                object result = cmd.ExecuteScalar();
                if (result != null)
                {
                    hoaDonID = Convert.ToInt32(result);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lấy hóa đơn theo bàn: {ex.Message}");
            }

            return hoaDonID;
        }

        public bool ThemMonVaoHoaDon(int banID, int doAnUongID, int soLuong, string nhanVienID, int hoaDonID)
        {
            try
            {
                HoaDonController hoaDonController = new HoaDonController();

                // Nếu chưa có hóa đơn, tạo mới hóa đơn
                if (hoaDonID == 0)
                {
                    hoaDonID = hoaDonController.TaoMoiHoaDon(banID, nhanVienID);
                    if (hoaDonID == 0)
                    {
                        MessageBox.Show("Không thể tạo hóa đơn mới.");
                        return false;
                    }
                }

                // Lấy giá của món ăn
                decimal gia = 0;
                string queryGetGia = @"
SELECT Gia 
FROM DoAnUong 
WHERE DoAnUongID = @DoAnUongID";

                using (SqlCommand cmdGia = new SqlCommand(queryGetGia, conn))
                {
                    cmdGia.Parameters.AddWithValue("@DoAnUongID", doAnUongID);
                    gia = Convert.ToDecimal(cmdGia.ExecuteScalar());
                }

                // Kiểm tra xem món ăn đã tồn tại trong ChiTietHoaDon hay chưa
                string queryCheckExist = @"
SELECT COUNT(*) 
FROM ChiTietHoaDon 
WHERE HoaDonID = @HoaDonID AND DoAnUongID = @DoAnUongID";

                int count = 0;
                using (SqlCommand cmdCheckExist = new SqlCommand(queryCheckExist, conn))
                {
                    cmdCheckExist.Parameters.AddWithValue("@HoaDonID", hoaDonID);
                    cmdCheckExist.Parameters.AddWithValue("@DoAnUongID", doAnUongID);
                    count = (int)cmdCheckExist.ExecuteScalar();
                }

                if (count > 0) // Nếu món ăn đã tồn tại, cập nhật số lượng
                {
                    string queryUpdate = @"
UPDATE ChiTietHoaDon 
SET SoLuong = SoLuong + @SoLuong 
WHERE HoaDonID = @HoaDonID AND DoAnUongID = @DoAnUongID";

                    using (SqlCommand cmdUpdate = new SqlCommand(queryUpdate, conn))
                    {
                        cmdUpdate.Parameters.AddWithValue("@HoaDonID", hoaDonID);
                        cmdUpdate.Parameters.AddWithValue("@DoAnUongID", doAnUongID);
                        cmdUpdate.Parameters.AddWithValue("@SoLuong", soLuong);
                        cmdUpdate.ExecuteNonQuery();
                    }
                }
                else // Nếu món ăn chưa tồn tại, thêm mới món ăn vào chi tiết hóa đơn
                {
                    string queryAddChiTiet = @"
INSERT INTO ChiTietHoaDon (HoaDonID, DoAnUongID, SoLuong, Gia) 
VALUES (@HoaDonID, @DoAnUongID, @SoLuong, @Gia)";

                    using (SqlCommand cmdAdd = new SqlCommand(queryAddChiTiet, conn))
                    {
                        cmdAdd.Parameters.AddWithValue("@HoaDonID", hoaDonID);
                        cmdAdd.Parameters.AddWithValue("@DoAnUongID", doAnUongID);
                        cmdAdd.Parameters.AddWithValue("@SoLuong", soLuong);
                        cmdAdd.Parameters.AddWithValue("@Gia", gia);
                        cmdAdd.ExecuteNonQuery();
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                // Log hoặc xử lý ngoại lệ
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
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

                // Kiểm tra xem món ăn đã có trong ChiTietHoaDon chưa
                string queryCheckExist = "SELECT COUNT(*) FROM ChiTietHoaDon WHERE HoaDonID = @HoaDonID AND DoAnUongID = @DoAnUongID";
                SqlCommand cmdCheckExist = new SqlCommand(queryCheckExist, conn);
                cmdCheckExist.Parameters.AddWithValue("@HoaDonID", hoaDonID);
                cmdCheckExist.Parameters.AddWithValue("@DoAnUongID", doAnUongID);

                int count = (int)cmdCheckExist.ExecuteScalar(); // Sử dụng ExecuteScalar để lấy số lượng món ăn đã có

                if (count > 0) // Nếu món ăn đã có, cập nhật số lượng
                {
                    string queryUpdate = "UPDATE ChiTietHoaDon " +
                                         "SET SoLuong = SoLuong + @SoLuong " +
                                         "WHERE HoaDonID = @HoaDonID AND DoAnUongID = @DoAnUongID";
                    SqlCommand cmdUpdate = new SqlCommand(queryUpdate, conn);
                    cmdUpdate.Parameters.AddWithValue("@HoaDonID", hoaDonID);
                    cmdUpdate.Parameters.AddWithValue("@DoAnUongID", doAnUongID);
                    cmdUpdate.Parameters.AddWithValue("@SoLuong", soLuong);

                    // Thực thi câu lệnh cập nhật
                    cmdUpdate.ExecuteNonQuery();
                }
                else // Nếu món ăn chưa có, thêm mới
                {
                    string queryInsert = "INSERT INTO ChiTietHoaDon (HoaDonID, DoAnUongID, SoLuong, Gia) " +
                                         "VALUES (@HoaDonID, @DoAnUongID, @SoLuong, @Gia)";
                    SqlCommand cmdInsert = new SqlCommand(queryInsert, conn);
                    cmdInsert.Parameters.AddWithValue("@HoaDonID", hoaDonID);
                    cmdInsert.Parameters.AddWithValue("@DoAnUongID", doAnUongID);
                    cmdInsert.Parameters.AddWithValue("@SoLuong", soLuong);
                    cmdInsert.Parameters.AddWithValue("@Gia", gia);

                    // Thực thi câu lệnh thêm mới
                    cmdInsert.ExecuteNonQuery();
                }

                return true; // Trả về true nếu thực hiện thành công
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
                // Xóa món khỏi chi tiết hóa đơn
                string queryDeleteChiTiet = @"
        DELETE FROM ChiTietHoaDon 
        WHERE ChiTietID = @ChiTietID AND HoaDonID = @HoaDonID";

                using (SqlCommand cmdDeleteChiTiet = new SqlCommand(queryDeleteChiTiet, conn))
                {
                    cmdDeleteChiTiet.Parameters.AddWithValue("@ChiTietID", chiTietID);
                    cmdDeleteChiTiet.Parameters.AddWithValue("@HoaDonID", hoaDonID);
                    cmdDeleteChiTiet.ExecuteNonQuery();
                }

                // Cập nhật tổng tiền của hóa đơn sau khi xóa món
                string queryUpdateTongTien = @"
        UPDATE HoaDon
        SET TongTien = (
            SELECT ISNULL(SUM(SoLuong * Gia), 0)
            FROM ChiTietHoaDon
            WHERE HoaDonID = @HoaDonID
        )
        WHERE HoaDonID = @HoaDonID";

                using (SqlCommand cmdUpdateTongTien = new SqlCommand(queryUpdateTongTien, conn))
                {
                    cmdUpdateTongTien.Parameters.AddWithValue("@HoaDonID", hoaDonID);
                    cmdUpdateTongTien.ExecuteNonQuery();
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa món khỏi chi tiết hóa đơn: {ex.Message}");
                return false;
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

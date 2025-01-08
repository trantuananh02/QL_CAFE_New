using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QL_CAFE.Controllers
{
    public class HoaDonController:KetNoiCSDL
    {
        // Hàm kiểm tra bàn có hóa đơn chưa
        public bool KiemTraBanCoHoaDon(int banID)
        {
            try
            {
                // Sử dụng truy vấn SQL để kiểm tra xem bàn có hóa đơn hay chưa
                string query = "SELECT COUNT(*) FROM HoaDon WHERE BanID = @BanID AND TrangThai = N'Chưa Thanh Toán'";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@BanID", banID);

                        // Kiểm tra số lượng hóa đơn có trạng thái 'Chưa Thanh Toán' cho bàn
                        int count = (int)cmd.ExecuteScalar();

                        // Nếu count > 0 thì có hóa đơn chưa thanh toán
                        return count > 0;
                    }
                
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ nếu có lỗi
                MessageBox.Show("Lỗi: " + ex.Message);
                return false;
            }
        }

        public int TaoMoiHoaDon(int banID, string nhanVienID)
        {
            int hoaDonID = 0;
            string queryCreateHoaDon = @"
        INSERT INTO HoaDon (BanID, NhanVienID, TongTien, TrangThai) 
        VALUES (@BanID, @NhanVienID, 0, N'Chưa Thanh Toán');
        SELECT SCOPE_IDENTITY();";

            try
            {
                using (SqlCommand cmdCreate = new SqlCommand(queryCreateHoaDon, conn))
                {
                    cmdCreate.Parameters.AddWithValue("@BanID", banID);
                    cmdCreate.Parameters.AddWithValue("@NhanVienID", nhanVienID);
                    object result = cmdCreate.ExecuteScalar();
                    if (result != null)
                    {
                        hoaDonID = Convert.ToInt32(result);
                    }
                }

                // Cập nhật trạng thái bàn thành "Đang sử dụng"
                string queryUpdateBan = @"
        UPDATE Ban
        SET TrangThai = N'Đang Sử Dụng'
        WHERE BanID = @BanID";

                using (SqlCommand cmdUpdateBan = new SqlCommand(queryUpdateBan, conn))
                {
                    cmdUpdateBan.Parameters.AddWithValue("@BanID", banID);
                    cmdUpdateBan.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tạo mới hóa đơn: {ex.Message}");
            }

            return hoaDonID;
        }
        public bool ThanhToanHoaDon(int hoaDonID)
        {
            try
            {
                // Cập nhật trạng thái hóa đơn là "Đã Thanh Toán"
                string queryUpdateHoaDon = @"
        UPDATE HoaDon 
        SET TrangThai = N'Đã Thanh Toán', NgayThanhToan = GETDATE() 
        WHERE HoaDonID = @HoaDonID";

                using (SqlCommand cmdUpdateHoaDon = new SqlCommand(queryUpdateHoaDon, conn))
                {
                    cmdUpdateHoaDon.Parameters.AddWithValue("@HoaDonID", hoaDonID);
                    cmdUpdateHoaDon.ExecuteNonQuery();
                }

                // Lấy BanID của hóa đơn
                int banID = 0;
                string queryGetBanID = @"
        SELECT BanID 
        FROM HoaDon 
        WHERE HoaDonID = @HoaDonID";

                using (SqlCommand cmdGetBanID = new SqlCommand(queryGetBanID, conn))
                {
                    cmdGetBanID.Parameters.AddWithValue("@HoaDonID", hoaDonID);
                    object result = cmdGetBanID.ExecuteScalar();
                    if (result != null)
                    {
                        banID = Convert.ToInt32(result);
                    }
                }

                // Cập nhật trạng thái bàn về 'Trống'
                if (banID > 0)
                {
                    string queryUpdateBan = @"
            UPDATE Ban
            SET TrangThai = N'Trống'
            WHERE BanID = @BanID";

                    using (SqlCommand cmdUpdateBan = new SqlCommand(queryUpdateBan, conn))
                    {
                        cmdUpdateBan.Parameters.AddWithValue("@BanID", banID);
                        cmdUpdateBan.ExecuteNonQuery();
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thanh toán hóa đơn: {ex.Message}");
                return false;
            }
        }


        public decimal LayTongTienHoaDon(int hoaDonID)
        {
            decimal tongTien = 0;
            string query = "SELECT TongTien FROM HoaDon WHERE HoaDonID = @HoaDonID";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@HoaDonID", hoaDonID);
                object result = cmd.ExecuteScalar();
                if (result != null)
                {
                    tongTien = Convert.ToDecimal(result);
                }
            }

            return tongTien;
        }


        public bool ChuyenHoaDon(int banNguonID, int banDichID)
        {
            try
            {
                // Kiểm tra xem bàn nguồn có hóa đơn chưa thanh toán không
                int hoaDonNguonID = LayHoaDonIDTheoBan(banNguonID);
                if (hoaDonNguonID == 0)
                {
                    MessageBox.Show("Không tìm thấy hóa đơn chưa thanh toán ở bàn nguồn!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                // Kiểm tra xem bàn đích có hóa đơn chưa thanh toán không
                int hoaDonDichID = LayHoaDonIDTheoBan(banDichID);
                if (hoaDonDichID > 0)
                {
                    MessageBox.Show("Bàn đích đã có hóa đơn chưa thanh toán!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                // Cập nhật BanID của hóa đơn sang bàn đích
                string queryUpdateHoaDon = @"
            UPDATE HoaDon
            SET BanID = @BanDichID
            WHERE HoaDonID = @HoaDonNguonID";

                using (SqlCommand cmdUpdateHoaDon = new SqlCommand(queryUpdateHoaDon, conn))
                {
                    cmdUpdateHoaDon.Parameters.AddWithValue("@BanDichID", banDichID);
                    cmdUpdateHoaDon.Parameters.AddWithValue("@HoaDonNguonID", hoaDonNguonID);
                    cmdUpdateHoaDon.ExecuteNonQuery();
                }

                // Cập nhật trạng thái bàn
                string queryUpdateBanNguon = @"
            UPDATE Ban
            SET TrangThai = N'Trống'
            WHERE BanID = @BanNguonID";

                string queryUpdateBanDich = @"
            UPDATE Ban
            SET TrangThai = N'Đang Sử Dụng'
            WHERE BanID = @BanDichID";

                using (SqlCommand cmdUpdateBanNguon = new SqlCommand(queryUpdateBanNguon, conn))
                {
                    cmdUpdateBanNguon.Parameters.AddWithValue("@BanNguonID", banNguonID);
                    cmdUpdateBanNguon.ExecuteNonQuery();
                }

                using (SqlCommand cmdUpdateBanDich = new SqlCommand(queryUpdateBanDich, conn))
                {
                    cmdUpdateBanDich.Parameters.AddWithValue("@BanDichID", banDichID);
                    cmdUpdateBanDich.ExecuteNonQuery();
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi chuyển hóa đơn: {ex.Message}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
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

    }

}

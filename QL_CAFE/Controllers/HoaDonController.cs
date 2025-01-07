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
        public int LayHoaDonIDTheoBan(int banID)
        {
            try
            {
                // Sử dụng truy vấn SQL để lấy HoaDonID của bàn có trạng thái 'Chưa Thanh Toán'
                string query = "SELECT HoaDonID FROM HoaDon WHERE BanID = @BanID AND TrangThai = N'Chưa Thanh Toán'";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@BanID", banID);

                    // Thực thi truy vấn và lấy HoaDonID
                    object result = cmd.ExecuteScalar();

                    // Kiểm tra nếu có kết quả trả về
                    if (result != null)
                    {
                        return (int)result; // Trả về HoaDonID
                    }
                    else
                    {
                        return -1; // Nếu không tìm thấy hóa đơn, trả về -1 (hoặc giá trị bạn muốn xử lý)
                    }
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có
                MessageBox.Show("Lỗi khi lấy HoaDonID: " + ex.Message);
                return -1; // Trả về -1 nếu có lỗi
            }
        }

    }

}

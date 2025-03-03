using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QL_CAFE.Models
{
    public class HoaDonModel
    {
        public int HoaDonID { get; set; }  // ID hóa đơn
        public DateTime NgayTao { get; set; }  // Ngày tạo hóa đơn
        public DateTime? NgayThanhToan { get; set; }  // Ngày thanh toán hóa đơn (nullable)
        public decimal TongTien { get; set; }  // Tổng tiền của hóa đơn
        public string NhanVienID { get; set; }  // Mã nhân viên
        public int BanID { get; set; }  // ID bàn
        public string TrangThai { get; set; }  // Trạng thái thanh toán

        // Constructor mặc định
        public HoaDonModel()
        {
            // Thiết lập giá trị mặc định
            NgayTao = DateTime.Now;
            TongTien = 0;
            TrangThai = "Chưa Thanh Toán";  // Giá trị mặc định cho trạng thái
        }
    }

}

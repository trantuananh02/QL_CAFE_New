using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QL_CAFE.Models
{
    public class ChiTietHoaDonModel
    {
        public int ChiTietID { get; set; }  // ID chi tiết hóa đơn
        public int HoaDonID { get; set; }  // ID hóa đơn
        public int DoAnUongID { get; set; }  // ID món ăn/uống
        public int SoLuong { get; set; }  // Số lượng món ăn/uống
        public decimal Gia { get; set; }  // Giá của món ăn/uống
    }
}

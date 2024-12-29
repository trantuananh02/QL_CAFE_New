using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QL_CAFE.Models
{
    public class NhanVienModel
    {
        // Thuộc tính của bảng Nhân Viên
        public int NhanVienID { get; set; } // Primary Key
        public string HoTen { get; set; } // Tên của nhân viên
        public DateTime NgaySinh { get; set; } // Ngày sinh (có thể null)
        public string SoDienThoai { get; set; } // Số điện thoại
        public string DiaChi { get; set; } // Địa chỉ
        public string TenTK { get; set; } // Tên tài khoản, khóa ngoại tham chiếu bảng NguoiDung

    }
}

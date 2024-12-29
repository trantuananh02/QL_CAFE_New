using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QL_CAFE.Models
{
    public class NhanVienModel
    {
        // Mã nhân viên (Primary Key)
        public string NhanVienID { get; set; }

        // Họ và tên (Không được để trống)
        public string HoTen { get; set; }

        // Ngày sinh
        public DateTime NgaySinh { get; set; }

        // Số điện thoại
        public string SoDienThoai { get; set; }

        // Địa chỉ
        public string DiaChi { get; set; }

        // Tên tài khoản (Duy nhất)
        public string TenTK { get; set; }

        // Mật khẩu (Không được để trống)
        public string MatKhau { get; set; }

        // Vai trò (Chỉ nhận giá trị 'Admin' hoặc 'User')
        public string VaiTro { get; set; }
    }
}

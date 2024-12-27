using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QL_CAFE.Models
{
    public class TaiKhoan
    {
        // Họ tên của người dùng
        public string HoTen { get; set; }

        // Số điện thoại của người dùng
        public string SoDienThoai { get; set; }

        // Tên tài khoản (khóa chính)
        public string TenTK { get; set; }

        // Mật khẩu đã được mã hóa
        public string MatKhau { get; set; }

        // Vai trò: Admin hoặc User
        public string VaiTro { get; set; }

        // Constructor mặc định
        public TaiKhoan() { }

        // Constructor đầy đủ
        public TaiKhoan(string hoTen, string soDienThoai, string tenTK, string matKhau, string vaiTro)
        {
            HoTen = hoTen;
            SoDienThoai = soDienThoai;
            TenTK = tenTK;
            MatKhau = matKhau;
            VaiTro = vaiTro;
        }

        // Phương thức hiển thị thông tin tài khoản (ví dụ minh họa)
        public override string ToString()
        {
            return $"TenTK: {TenTK}, VaiTro: {VaiTro}, HoTen: {HoTen}, SoDienThoai: {SoDienThoai}";
        }
    }

}

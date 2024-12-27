using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QL_CAFE.Models
{
    public class TaiKhoan
    {
        // Tên tài khoản (khóa chính)
        public string TenTK { get; set; }

        // Mật khẩu đã được mã hóa
        public string MatKhau { get; set; }

        // Vai trò: Admin hoặc User
        public string VaiTro { get; set; }

        // Salt để bảo mật thêm cho mật khẩu
        public string Salt { get; set; }

        // Constructor mặc định
        public TaiKhoan() { }
        
        // Constructor đầy đủ
        public TaiKhoan(string tenTK, string matKhau, string vaiTro, string salt)
        {
            TenTK = tenTK;
            MatKhau = matKhau;
            VaiTro = vaiTro;
            Salt = salt;
        }

        // Phương thức hiển thị thông tin tài khoản (ví dụ minh họa)
        public override string ToString()
        {
            return $"TenTK: {TenTK}, VaiTro: {VaiTro}";
        }
    }

}

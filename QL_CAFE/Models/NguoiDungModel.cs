using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QL_CAFE.Models
{
    public class NguoiDungModel
    {
        // Thuộc tính tương ứng với các cột trong bảng NguoiDung
        public string TenTK { get; set; } // Tên tài khoản (khóa chính)
        public string MatKhau { get; set; } // Mật khẩu (đã mã hóa)
        public string VaiTro { get; set; } // Vai trò (Admin hoặc User)
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QL_CAFE.Models
{
    public class BanModel
    {
        // Thuộc tính của bảng Ban
        public int BanID { get; set; } // Primary Key
        public string SoBan { get; set; } // Số bàn, không được null
        public string TrangThai { get; set; } = "Trống"; // Trạng thái, giá trị mặc định là "Trống"
        public int KhuVucID { get; set; } // Khóa ngoại tham chiếu đến bảng KhuVuc

        // Phương thức kiểm tra trạng thái hợp lệ
        public bool IsValidTrangThai()
        {
            return TrangThai == "Trống" || TrangThai == "Đang Sử Dụng";
        }
    }
}

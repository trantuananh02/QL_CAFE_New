using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QL_CAFE.Controllers;

namespace QL_CAFE.Models
{
    public class DoAnUongModel : KetNoiCSDL
    {
        // Định nghĩa lớp tương ứng với bảng DoAnUong trong cơ sở dữ liệu
        public int DoAnUongID { get; set; } // Khóa chính
        public string TenDoAnUong { get; set; } // Tên đồ ăn uống, không được null
        public decimal Gia { get; set; } // Giá tiền
        public string HinhAnh { get; set; } // Hình ảnh có thể null hoặc chuỗi rỗng
        public int DanhMucID { get; set; } // ID danh mục, khóa ngoại
    }
}

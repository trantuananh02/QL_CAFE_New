using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QL_CAFE.Controllers;

namespace QL_CAFE.Models
{
    public class DanhMucModel : KetNoiCSDL
    {
        // Định nghĩa lớp tương ứng với bảng DanhMuc trong cơ sở dữ liệu
        public int DanhMucID { get; set; } // Khóa chính
        public string TenDanhMuc { get; set; } // Tên danh mục, không được null
    }
}

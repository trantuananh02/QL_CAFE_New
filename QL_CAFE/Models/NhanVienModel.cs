using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QL_CAFE.Models
{
    public class NhanVienModel
    {
        public string NhanVienID { get; set; } // NVARCHAR(100)

        public string HoTen { get; set; } // NVARCHAR(100)

        public DateTime? NgaySinh { get; set; } // DATE, nullable

        public string SoDienThoai { get; set; } // NVARCHAR(15)

        public string DiaChi { get; set; } // NVARCHAR(255)

        public string TenTK { get; set; } // VARCHAR(100), foreign key
    }
}

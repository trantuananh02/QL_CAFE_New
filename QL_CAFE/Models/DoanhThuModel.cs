using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QL_CAFE.Controllers;

namespace QL_CAFE.Models
{
    public class DoanhThuModel : KetNoiCSDL
    {
        public int HoaDonID { get; set; }
        public DateTime NgayTao { get; set; }
        public decimal TongTien { get; set; }
        public string TrangThai { get; set; }
        public string NhanVien { get; set; }
        public string SoBan { get; set; }
    }
}

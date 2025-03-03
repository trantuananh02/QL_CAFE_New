using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QL_CAFE.Models;

namespace QL_CAFE.Controllers
{
    public class DoanhThuController : KetNoiCSDL
    {

        public List<DoanhThuModel> GetDoanhThu(DateTime ngayBatDau, DateTime ngayKetThuc)
        {
            List<DoanhThuModel> danhSachDoanhThu = new List<DoanhThuModel>();

            {
                string query = @"SELECT h.HoaDonID, h.NgayTao, h.TongTien, h.TrangThai, nv.HoTen AS NhanVien, b.SoBan 
                                 FROM HoaDon h
                                 JOIN NhanVien nv ON h.NhanVienID = nv.NhanVienID
                                 JOIN Ban b ON h.BanID = b.BanID
                                 WHERE h.NgayTao BETWEEN @NgayBatDau AND @NgayKetThuc AND h.TrangThai = N'Đã Thanh Toán'";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@NgayBatDau", ngayBatDau);
                    cmd.Parameters.AddWithValue("@NgayKetThuc", ngayKetThuc);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        danhSachDoanhThu.Add(new DoanhThuModel
                        {
                            HoaDonID = reader.GetInt32(0),
                            NgayTao = reader.GetDateTime(1),
                            TongTien = reader.GetDecimal(2),
                            TrangThai = reader.GetString(3),
                            NhanVien = reader.GetString(4),
                            SoBan = reader.GetString(5)
                        });
                    }
                }
            }
            return danhSachDoanhThu;
        }
    }
}

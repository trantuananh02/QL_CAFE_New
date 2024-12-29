using QL_CAFE.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QL_CAFE.Controllers
{
    public class NhanVienController : KetNoiCSDL
    {
        public List<NhanVienModel> HienThiDanhSachNhanVien()
        {
            List<NhanVienModel> danhSachNhanVien = new List<NhanVienModel>();

            // Câu lệnh SQL để lấy danh sách nhân viên
            string sql = "SELECT NhanVienID, HoTen, NgaySinh, SoDienThoai, DiaChi, TenTK FROM NhanVien";

            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();
                }
                SqlCommand cmd = new SqlCommand(sql, conn);

                SqlDataReader reader = cmd.ExecuteReader(); // Thực thi câu lệnh SQL và nhận kết quả

                while (reader.Read())
                {
                    // Tạo đối tượng NhanVienModel và gán giá trị từ cơ sở dữ liệu
                    NhanVienModel nhanVien = new NhanVienModel
                    {
                        NhanVienID = reader.GetInt32(reader.GetOrdinal("NhanVienID")),
                        HoTen = reader.GetString(reader.GetOrdinal("HoTen")),
                        NgaySinh = reader.GetDateTime(reader.GetOrdinal("NgaySinh")),
                        SoDienThoai = reader.GetString(reader.GetOrdinal("SoDienThoai")),
                        DiaChi = reader.GetString(reader.GetOrdinal("DiaChi")),
                        TenTK = reader.GetString(reader.GetOrdinal("TenTK"))
                    };

                    // Thêm nhân viên vào danh sách
                    danhSachNhanVien.Add(nhanVien);
                }

                reader.Close(); // Đóng SqlDataReader
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi: " + ex.Message);
            }

            return danhSachNhanVien; // Trả về danh sách nhân viên
        }


    }
}

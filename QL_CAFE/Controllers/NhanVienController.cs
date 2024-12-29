using QL_CAFE.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QL_CAFE.Controllers
{
    public class NhanVienController : KetNoiCSDL
    {
        public List<NhanVienModel> HienThiDanhSachNhanVien()
        {
            List<NhanVienModel> danhSachNhanVien = new List<NhanVienModel>();

            // Câu lệnh SQL để lấy danh sách nhân viên
            string sql = "SELECT NhanVienID, HoTen, NgaySinh, SoDienThoai, DiaChi, TenTK, MatKhau, VaiTro FROM NhanVien";

            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();
                }

                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    // Tạo đối tượng NhanVienModel và gán giá trị từ cơ sở dữ liệu
                    NhanVienModel nhanVien = new NhanVienModel
                    {
                        NhanVienID = reader.GetString(reader.GetOrdinal("NhanVienID")),
                        HoTen = reader.GetString(reader.GetOrdinal("HoTen")),
                        NgaySinh = reader.GetDateTime(reader.GetOrdinal("NgaySinh")).Date,
                        SoDienThoai = reader.GetString(reader.GetOrdinal("SoDienThoai")),
                        DiaChi = reader.GetString(reader.GetOrdinal("DiaChi")),
                        TenTK = reader.GetString(reader.GetOrdinal("TenTK")),
                        MatKhau = reader.GetString(reader.GetOrdinal("MatKhau")),
                        VaiTro = reader.GetString(reader.GetOrdinal("VaiTro"))
                    };

                    danhSachNhanVien.Add(nhanVien);
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi: " + ex.Message);
            }

            return danhSachNhanVien;
        }

        // Phương thức kiểm tra Nhân viên có tồn tại hay không
        public bool KiemTraNhanVienExist(string nhanVienID)
        {
           
            string query = "SELECT COUNT(*) FROM NhanVien WHERE NhanVienID = @NhanVienID";
            int count = ExecuteScalar(query, new SqlParameter("@NhanVienID", nhanVienID));

            return count > 0;
        }

        // Giả sử có phương thức ExecuteScalar để thực thi truy vấn trả về một giá trị duy nhất
        private int ExecuteScalar(string query, SqlParameter parameter)
        {
           
            using (var command = new SqlCommand(query, conn))
            {
                command.Parameters.Add(parameter);
                return (int)command.ExecuteScalar();  // Trả về số lượng dòng được trả về từ câu truy vấn
            }
        }
        public bool ThemNhanVien(NhanVienModel nhanVien)
        {
            // Câu lệnh SQL để thêm nhân viên
            string sql = "INSERT INTO NHANVIEN (NhanVienID, HoTen, NgaySinh, SoDienThoai, DiaChi, TenTK, MatKhau, Vaitro) " +
                         "VALUES (@NhanVienID, @HoTen, @NgaySinh, @SoDienThoai, @DiaChi, @TenTK, @MatKhau, @VaiTro)";
            try
            {
                using (SqlCommand command = new SqlCommand(sql, conn))
                {
                    // Gắn giá trị từ model vào các tham số
                    command.Parameters.AddWithValue("@NhanVienID", nhanVien.NhanVienID);
                    command.Parameters.AddWithValue("@HoTen", nhanVien.HoTen);
                    command.Parameters.AddWithValue("@NgaySinh", nhanVien.NgaySinh != DateTime.MinValue ? (object)nhanVien.NgaySinh : DBNull.Value);
                    command.Parameters.AddWithValue("@SoDienThoai", nhanVien.SoDienThoai);
                    command.Parameters.AddWithValue("@DiaChi", nhanVien.DiaChi);
                    command.Parameters.AddWithValue("@TenTK", nhanVien.TenTK);
                    command.Parameters.AddWithValue("@MatKhau", nhanVien.MatKhau); // Gắn mật khẩu
                    command.Parameters.AddWithValue("@VaiTro", nhanVien.VaiTro); // Gắn vai trò

                    // Thực thi câu lệnh
                    int rowsAffected = command.ExecuteNonQuery();

                    // Kiểm tra nếu có ít nhất 1 hàng được thêm
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                // Log lỗi (nếu cần thiết)
                Console.WriteLine("Lỗi khi thêm nhân viên: " + ex.Message);
                return false;
            }
        }
        public bool Xoa(string nhanVienID)
        {
            try
            {
                
                    string query = "DELETE FROM NhanVien WHERE NhanVienID = @NhanVienID";
                    SqlCommand command = new SqlCommand(query, conn);
                    command.Parameters.AddWithValue("@NhanVienID", nhanVienID);

                    int rowsAffected = command.ExecuteNonQuery();

                    return rowsAffected > 0; // Nếu có ít nhất một dòng bị xóa, trả về true
                
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi khi xóa nhân viên: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        public bool LuuNhanVien(NhanVienModel nhanVien)
        {
            // Câu lệnh SQL để cập nhật thông tin nhân viên
            string sql = "UPDATE NHANVIEN " +
                         "SET HoTen = @HoTen, NgaySinh = @NgaySinh, SoDienThoai = @SoDienThoai, DiaChi = @DiaChi, " +
                         "TenTK = @TenTK, MatKhau = @MatKhau, VaiTro = @VaiTro " + // Updated to include MatKhau and VaiTro
                         "WHERE NhanVienID = @NhanVienID";
            try
            {
                using (SqlCommand command = new SqlCommand(sql, conn))
                {
                    // Gắn giá trị từ model vào các tham số
                    command.Parameters.Add("@HoTen", System.Data.SqlDbType.NVarChar).Value = nhanVien.HoTen;
                    command.Parameters.Add("@NgaySinh", System.Data.SqlDbType.DateTime).Value = nhanVien.NgaySinh != DateTime.MinValue ? (object)nhanVien.NgaySinh : DBNull.Value;
                    command.Parameters.Add("@SoDienThoai", System.Data.SqlDbType.NVarChar).Value = nhanVien.SoDienThoai;
                    command.Parameters.Add("@DiaChi", System.Data.SqlDbType.NVarChar).Value = nhanVien.DiaChi;
                    command.Parameters.Add("@TenTK", System.Data.SqlDbType.NVarChar).Value = nhanVien.TenTK;
                    command.Parameters.Add("@MatKhau", System.Data.SqlDbType.NVarChar).Value = nhanVien.MatKhau; // Added MatKhau
                    command.Parameters.Add("@VaiTro", System.Data.SqlDbType.NVarChar).Value = nhanVien.VaiTro; // Added VaiTro
                    command.Parameters.Add("@NhanVienID", System.Data.SqlDbType.NVarChar).Value = nhanVien.NhanVienID;

                    // Thực thi câu lệnh
                    int rowsAffected = command.ExecuteNonQuery();

                    // Kiểm tra nếu có ít nhất 1 hàng được cập nhật
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi lưu nhân viên: " + ex.Message);
                return false;
            }
        }


    }
}

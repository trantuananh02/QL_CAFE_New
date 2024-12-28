using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QL_CAFE.Controllers
{
    public class DangNhapController : KetNoiCSDL
    {
        // Hàm đăng nhập không mã hóa mật khẩu
        public bool DangNhap(string tenTK, string matKhau)
        {
            // Kết nối cơ sở dữ liệu
                try
            { 

                    // SQL truy vấn để tìm tài khoản và mật khẩu trong cơ sở dữ liệu
                    string sql = "SELECT TenTK, MatKhau, VaiTro FROM NguoiDung WHERE TenTK = @TenTK";

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@TenTK", tenTK);

                    // Thực thi truy vấn và lấy kết quả
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();

                        // Lấy mật khẩu từ cơ sở dữ liệu
                        string matKhauDB = reader["MatKhau"].ToString();
                        string vaiTro = reader["VaiTro"].ToString();

                        // Kiểm tra mật khẩu có khớp không
                        if (matKhau == matKhauDB)
                        {
                            // Nếu mật khẩu chính xác, có thể trả về vai trò và thông tin tài khoản nếu cần
                            // Ví dụ: Lưu vai trò vào session hoặc trả về true
                            Console.WriteLine("Đăng nhập thành công. Vai trò: " + vaiTro);
                            return true;
                        }
                        else
                        {
                            // Nếu mật khẩu không chính xác
                            Console.WriteLine("Mật khẩu không chính xác.");
                            return false;
                        }
                    }
                    else
                    {
                        // Nếu không tìm thấy tài khoản
                        Console.WriteLine("Tài khoản không tồn tại.");
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    // Xử lý lỗi kết nối CSDL
                    Console.WriteLine("Lỗi kết nối cơ sở dữ liệu: " + ex.Message);
                    return false;
                }
            }
        public bool KiemTraEmail(String email)
        {
            string sql = "SELECT COUNT(*) FROM NguoiDung WHERE email = @Email";
            try
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Email", email);

                int count = (int)cmd.ExecuteScalar();
                return count > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi kiểm tra email: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
    }
   
    
}

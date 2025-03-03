using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Collections.Specialized.BitVector32;

namespace QL_CAFE.Controllers
{
    public class DangNhapController : KetNoiCSDL
    {
        // Hàm đăng nhập dựa trên NguoiDungModel
        
        public bool DangNhap(string tenTK, string matKhau )
        {
            try
            {
                {

                    // SQL truy vấn để tìm tài khoản và mật khẩu trong cơ sở dữ liệu
                    string sql = "SELECT TenTK, MatKhau, VaiTro FROM NguoiDung WHERE TenTK = @TenTK";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@TenTK", tenTK);

                        // Thực thi truy vấn và lấy kết quả
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                reader.Read();

                                // Lấy mật khẩu từ cơ sở dữ liệu
                                string matKhauDB = reader["MatKhau"].ToString();
                                string vaiTro = reader["VaiTro"].ToString();

                                // Kiểm tra mật khẩu có khớp không
                                if (matKhau == matKhauDB)
                                {
                                    Console.WriteLine("Đăng nhập thành công. Vai trò: " + vaiTro);

                                    // Lưu tên tài khoản vào Session
                                    Session.TenTaiKhoan = tenTK;
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
                    }
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi kết nối CSDL
                Console.WriteLine("Lỗi kết nối cơ sở dữ liệu: " + ex.Message);
                return false;
            }
        }
    }
}
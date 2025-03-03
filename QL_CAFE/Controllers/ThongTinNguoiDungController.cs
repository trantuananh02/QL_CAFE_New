using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QL_CAFE.Controllers
{
    public class ThongTinNguoiDungController : KetNoiCSDL
    {
        public void LayThongTinNguoiDung(string tenTaiKhoan, TextBox txtHoTen, TextBox txtSoDienThoai, TextBox txtDiaChi, TextBox txtNgaySinh, TextBox txtTenTaiKhoan)
        {
            try
            {
                {

                    // Câu lệnh SQL để lấy thông tin người dùng
                    string query = @"SELECT nv.HoTen, nv.SoDienThoai, nv.DiaChi, nv.NgaySinh, nd.TenTK
                                     FROM NhanVien nv
                                     INNER JOIN NguoiDung nd ON nv.TenTK = nd.TenTK
                                     WHERE nd.TenTK = @TenTK";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        // Thêm tham số để tránh SQL Injection
                        cmd.Parameters.AddWithValue("@TenTK", tenTaiKhoan);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Gán giá trị vào các TextBox
                                txtHoTen.Text = reader["HoTen"].ToString();
                                txtSoDienThoai.Text = reader["SoDienThoai"].ToString();
                                txtDiaChi.Text = reader["DiaChi"].ToString();

                                // Xử lý giá trị ngày sinh có thể null
                                txtNgaySinh.Text = reader["NgaySinh"] != DBNull.Value
                                    ? Convert.ToDateTime(reader["NgaySinh"]).ToString("yyyy-MM-dd")
                                    : string.Empty;

                                txtTenTaiKhoan.Text = reader["TenTK"].ToString();
                            }
                            else
                            {
                                MessageBox.Show("Không tìm thấy thông tin người dùng.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lấy thông tin người dùng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void CapNhatThongTinNguoiDung(string tenTaiKhoan, TextBox txtHoTen, TextBox txtSoDienThoai, TextBox txtDiaChi, TextBox txtNgaySinh)
        {
            try
            {
                

                // Câu lệnh SQL để cập nhật thông tin người dùng
                string query = @"UPDATE NhanVien
                                 SET HoTen = @HoTen,
                                     SoDienThoai = @SoDienThoai,
                                     DiaChi = @DiaChi,
                                     NgaySinh = @NgaySinh
                                 WHERE TenTK = @TenTK";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    // Thêm tham số để tránh SQL Injection
                    cmd.Parameters.AddWithValue("@HoTen", txtHoTen.Text);
                    cmd.Parameters.AddWithValue("@SoDienThoai", txtSoDienThoai.Text);
                    cmd.Parameters.AddWithValue("@DiaChi", txtDiaChi.Text);

                    // Xử lý giá trị ngày sinh nếu để trống
                    if (string.IsNullOrEmpty(txtNgaySinh.Text))
                    {
                        cmd.Parameters.AddWithValue("@NgaySinh", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@NgaySinh", Convert.ToDateTime(txtNgaySinh.Text));
                    }

                    cmd.Parameters.AddWithValue("@TenTK", tenTaiKhoan);

                    // Thực thi câu lệnh
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Cập nhật thông tin thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy tài khoản để cập nhật.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật thông tin người dùng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void LayTenTaiKhoan(string tenTaiKhoan, TextBox txtTenTaiKhoan)
        {
            try
            {
                // Câu lệnh SQL chỉ lấy tên tài khoản
                string query = @"SELECT TenTK
                         FROM NguoiDung
                         WHERE TenTK = @TenTK";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    // Thêm tham số để tránh SQL Injection
                    cmd.Parameters.AddWithValue("@TenTK", tenTaiKhoan);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Gán tên tài khoản vào TextBox
                            txtTenTaiKhoan.Text = reader["TenTK"].ToString();
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy tài khoản.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lấy tên tài khoản: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        public void DoiMatKhau(string tenTaiKhoan, TextBox txtMatKhauCu, TextBox txtMatKhauMoi, TextBox txtNhapLaiMatKhau)
        {
            try
            {
                // Kiểm tra mật khẩu mới và mật khẩu xác nhận có giống nhau không
                if (txtMatKhauMoi.Text != txtNhapLaiMatKhau.Text)
                {
                    MessageBox.Show("Mật khẩu mới và mật khẩu nhập lại không khớp.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Câu lệnh SQL để kiểm tra mật khẩu cũ
                string queryCheckOldPassword = @"SELECT COUNT(*) FROM NguoiDung WHERE TenTK = @TenTK AND MatKhau = @MatKhauCu";

                using (SqlCommand cmd = new SqlCommand(queryCheckOldPassword, conn))
                {
                    // Thêm tham số để tránh SQL Injection
                    cmd.Parameters.AddWithValue("@TenTK", tenTaiKhoan);
                    cmd.Parameters.AddWithValue("@MatKhauCu", txtMatKhauCu.Text);

                    int count = (int)cmd.ExecuteScalar();
                    if (count == 0)
                    {
                        MessageBox.Show("Mật khẩu cũ không chính xác.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                // Câu lệnh SQL để cập nhật mật khẩu mới
                string queryUpdatePassword = @"UPDATE NguoiDung
                                       SET MatKhau = @MatKhauMoi
                                       WHERE TenTK = @TenTK";

                using (SqlCommand cmd = new SqlCommand(queryUpdatePassword, conn))
                {
                    // Thêm tham số để tránh SQL Injection
                    cmd.Parameters.AddWithValue("@MatKhauMoi", txtMatKhauMoi.Text);
                    cmd.Parameters.AddWithValue("@TenTK", tenTaiKhoan);

                    // Thực thi câu lệnh
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Đổi mật khẩu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Không thể đổi mật khẩu. Vui lòng thử lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi đổi mật khẩu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}

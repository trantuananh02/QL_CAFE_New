using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QL_CAFE.Models;

namespace QL_CAFE.Controllers
{
    public class DanhMucController : KetNoiCSDL
    {
        // Hiển thị danh sách danh mục
        // Hiển thị danh sách danh mục
        public List<DanhMucModel> HienThiDanhSachDanhMuc()
        {
            List<DanhMucModel> danhSachDanhMuc = new List<DanhMucModel>();

            try
            {
                string query = "SELECT DanhMucID, TenDanhMuc FROM DanhMuc";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DanhMucModel danhMuc = new DanhMucModel
                            {
                                DanhMucID = reader.GetInt32(0),
                                TenDanhMuc = reader.GetString(1)
                            };
                            danhSachDanhMuc.Add(danhMuc);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Có lỗi xảy ra: {ex.Message}");
            }
            return danhSachDanhMuc;
        }


        // Hiển thị bảng danh mục (bao gồm Mã và Tên)
        public List<DanhMucModel> HienThiBangDanhMuc()
        {
            List<DanhMucModel> danhSachDanhMuc = new List<DanhMucModel>();

            try
            {
                string query = "SELECT DanhMucID, TenDanhMuc FROM DanhMuc";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DanhMucModel danhMuc = new DanhMucModel
                            {
                                DanhMucID = reader.GetInt32(0),
                                TenDanhMuc = reader.GetString(1)
                            };
                            danhSachDanhMuc.Add(danhMuc);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Có lỗi xảy ra: {ex.Message}");
            }
            return danhSachDanhMuc;
        }

        // Kiểm tra xem danh mục đã tồn tại trong cơ sở dữ liệu chưa
        public bool KiemTraDanhMucTonTai(int maDanhMuc)
        {
            bool exists = false;

            try
            {
                string query = "SELECT COUNT(1) FROM DanhMuc WHERE DanhMucID = @DanhMucID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@DanhMucID", maDanhMuc);
                    exists = Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Có lỗi xảy ra: {ex.Message}");
            }

            return exists;
        }

        // Thêm danh mục mới vào cơ sở dữ liệu
        public void ThemDanhMuc(DanhMucModel danhMuc)
        {
            try
            {
                string query = "INSERT INTO DanhMuc (TenDanhMuc) VALUES (@TenDanhMuc)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@TenDanhMuc", danhMuc.TenDanhMuc);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Có lỗi xảy ra khi thêm danh mục: {ex.Message}");
            }
        }

        // Xóa danh mục
        public bool XoaDanhMuc(int maDanhMuc)
        {
            try
            {
                // Kiểm tra xem danh mục có tồn tại hay không
                if (!KiemTraDanhMucTonTai(maDanhMuc))
                {
                    MessageBox.Show("Danh mục không tồn tại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                // Câu truy vấn xóa danh mục
                string query = "DELETE FROM DanhMuc WHERE DanhMucID = @DanhMucID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@DanhMucID", maDanhMuc);

                    // Thực thi truy vấn
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        return true; // Xóa thành công
                    }
                    else
                    {
                        return false; // Không có dòng nào bị xóa
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Có lỗi xảy ra: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        // Cập nhật thông tin danh mục trong cơ sở dữ liệu
        public void CapNhatDanhMuc(DanhMucModel danhMuc)
        {
            try
            {
                string query = "UPDATE DanhMuc SET TenDanhMuc = @TenDanhMuc WHERE DanhMucID = @DanhMucID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@DanhMucID", danhMuc.DanhMucID);
                    cmd.Parameters.AddWithValue("@TenDanhMuc", danhMuc.TenDanhMuc);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Có lỗi xảy ra khi cập nhật danh mục: {ex.Message}");
            }
        }
    }
}

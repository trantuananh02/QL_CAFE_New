using QL_CAFE.Controllers;
using QL_CAFE.Models;
using QL_CAFE.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;


namespace QL_CAFE
{

    public partial class FormQuanLyNhanVien : Form
    {
        List<NhanVienModel> dsnv = new List<NhanVienModel>();
        public FormQuanLyNhanVien()
        {
            InitializeComponent();
            KhoiTaoBangNhanVien();
            HienThiDanhSachNhanVien();
        }
        private void HienThiDanhSachNhanVien()
        {
            try
            {
                // Khởi tạo controller và lấy danh sách nhân viên
                NhanVienController nhanVienController = new NhanVienController();
                List<NhanVienModel> dsnv = nhanVienController.HienThiDanhSachNhanVien();

                // Xóa tất cả dòng trong bảng trước khi thêm mới
                tblNhanVien.Rows.Clear();

                // Lặp qua danh sách nhân viên và thêm vào DataGridView
                for (int i = 0; i < dsnv.Count; i++)
                {
                    NhanVienModel nv = dsnv[i];
                    string maNhanVien = nv.NhanVienID;
                    string hoTen = nv.HoTen;
                    string ngaySinh = nv.NgaySinh.ToShortDateString(); // Hiển thị chỉ ngày
                    string soDienThoai = nv.SoDienThoai;
                    string diaChi = nv.DiaChi;
                    string tenTaiKhoan = nv.TenTK;
                    string matKhau = nv.MatKhau;  // Mật khẩu
                    string vaiTro = nv.VaiTro;    // Vai trò

                    // Thêm dòng vào DataGridView
                    tblNhanVien.Rows.Add(
                        i + 1, // Số thứ tự
                        maNhanVien,
                        hoTen,
                        ngaySinh, // Chỉ hiển thị ngày
                        soDienThoai,
                        diaChi,
                        tenTaiKhoan,
                        matKhau,   // Hiển thị mật khẩu
                        vaiTro     // Hiển thị vai trò
                    );
                }
            }
            catch (Exception ex)
            {
                // Hiển thị thông báo lỗi nếu có
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void KhoiTaoBangNhanVien()
        {
            // Xóa hết các cột và dòng hiện tại
            tblNhanVien.Columns.Clear();
            tblNhanVien.Rows.Clear();

            // Cột Số TT
            DataGridViewTextBoxColumn colSTT = new DataGridViewTextBoxColumn();
            colSTT.HeaderText = "Số TT";
            colSTT.Name = "STT";
            colSTT.ReadOnly = true;  // Không cho phép chỉnh sửa số thứ tự
            colSTT.Width = 100;
            tblNhanVien.Columns.Add(colSTT);

            // Cột Mã nhân viên
            DataGridViewTextBoxColumn colMaNhanVien = new DataGridViewTextBoxColumn
            {
                HeaderText = "Mã Nhân Viên",
                Name = "MaNhanVien",
                ReadOnly = false,
                Width = 120
            };
            tblNhanVien.Columns.Add(colMaNhanVien);

            // Cột Họ và tên
            DataGridViewTextBoxColumn colHoTen = new DataGridViewTextBoxColumn
            {
                HeaderText = "Họ và Tên",
                Name = "HoTen",
                ReadOnly = false,
                Width = 200
            };
            tblNhanVien.Columns.Add(colHoTen);

            // Cột Ngày sinh
            DataGridViewTextBoxColumn colNgaySinh = new DataGridViewTextBoxColumn
            {
                HeaderText = "Ngày Sinh",
                Name = "NgaySinh",
                ReadOnly = false,
                Width = 150
            };
            tblNhanVien.Columns.Add(colNgaySinh);

            // Cột Số điện thoại
            DataGridViewTextBoxColumn colSoDienThoai = new DataGridViewTextBoxColumn
            {
                HeaderText = "Số Điện Thoại",
                Name = "SoDienThoai",
                ReadOnly = false,
                Width = 150
            };
            tblNhanVien.Columns.Add(colSoDienThoai);

            // Cột Địa chỉ
            DataGridViewTextBoxColumn colDiaChi = new DataGridViewTextBoxColumn
            {
                HeaderText = "Địa Chỉ",
                Name = "DiaChi",
                ReadOnly = false,
                Width = 250
            };
            tblNhanVien.Columns.Add(colDiaChi);

            // Cột Tên tài khoản
            DataGridViewTextBoxColumn colTenTaiKhoan = new DataGridViewTextBoxColumn
            {
                HeaderText = "Tên Tài Khoản",
                Name = "TenTaiKhoan",
                ReadOnly = false,
                Width = 150
            };
            tblNhanVien.Columns.Add(colTenTaiKhoan);

            // Cột Mật khẩu
            DataGridViewTextBoxColumn colMatKhau = new DataGridViewTextBoxColumn
            {
                HeaderText = "Mật Khẩu",
                Name = "MatKhau",
                ReadOnly = false,
                Width = 150
            };
            tblNhanVien.Columns.Add(colMatKhau);

            // Cột Vai trò
            DataGridViewTextBoxColumn colVaiTro = new DataGridViewTextBoxColumn
            {
                HeaderText = "Vai Trò",
                Name = "VaiTro",
                ReadOnly = false,
                Width = 150
            };
            tblNhanVien.Columns.Add(colVaiTro);

            // Thiết lập DataGridView
            tblNhanVien.AllowUserToAddRows = false;  // Không cho phép thêm dòng mới khi không có dữ liệu
            tblNhanVien.AllowUserToDeleteRows = false; // Không cho phép xóa dòng
            tblNhanVien.AllowUserToOrderColumns = true; // Cho phép sắp xếp các cột
            tblNhanVien.SelectionMode = DataGridViewSelectionMode.FullRowSelect; // Chọn toàn bộ dòng khi click
            tblNhanVien.MultiSelect = false; // Không cho phép chọn nhiều dòng
            tblNhanVien.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; // Tự động điều chỉnh kích thước cột
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void FormQuanLyNhanVien_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            // Kiểm tra nếu DataGridView đã có một dòng trống
            if (tblNhanVien.Rows.Count > 0)
            {
                DataGridViewRow lastRow = tblNhanVien.Rows[tblNhanVien.Rows.Count - 1];
                if (lastRow.Cells["MaNhanVien"].Value == null || string.IsNullOrWhiteSpace(lastRow.Cells["MaNhanVien"].Value.ToString()))
                {
                    // Nếu dòng cuối cùng trống, không thêm dòng mới
                    return;
                }
            }

            // Thêm dòng trống vào DataGridView
            tblNhanVien.Rows.Add(
                tblNhanVien.Rows.Count + 1, // Số TT
                "",                         // Mã nhân viên (trống)
                "",                         // Họ và tên (trống)
                "",                         // Ngày sinh (trống)
                "",                         // Số điện thoại (trống)
                "",                         // Địa chỉ (trống)
                "",                         // Tên tài khoản (trống)
                "",                         // Mật khẩu (trống)
                ""                          // Vai trò (trống)
            );

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            try
            {
                bool isSuccess = true;
                NhanVienController nhanVienController = new NhanVienController();

                // Đảm bảo commit các thay đổi từ DataGridView
                tblNhanVien.EndEdit();

                for (int row = 0; row < tblNhanVien.Rows.Count; row++)
                {
                    DataGridViewRow dgvRow = tblNhanVien.Rows[row];

                    // Lấy giá trị từ các cột trong DataGridView
                    string nhanVienID = dgvRow.Cells[1].Value?.ToString();
                    string hoTen = dgvRow.Cells[2].Value?.ToString();
                    DateTime ngaySinh = dgvRow.Cells[3].Value != null ? Convert.ToDateTime(dgvRow.Cells["NgaySinh"].Value) : DateTime.MinValue;
                    string soDienThoai = dgvRow.Cells[4].Value?.ToString();
                    string diaChi = dgvRow.Cells[5].Value?.ToString();
                    string tenTK = dgvRow.Cells[6].Value?.ToString();
                    string matKhau = dgvRow.Cells[7].Value?.ToString();
                    string vaiTro = dgvRow.Cells[8].Value?.ToString();

                    // Kiểm tra dữ liệu nhập
                    if (string.IsNullOrWhiteSpace(nhanVienID))
                    {
                        MessageBox.Show($"Mã nhân viên không được để trống tại dòng {row + 1}", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Kiểm tra xem nhân viên đã tồn tại chưa
                    bool nhanVienExist = nhanVienController.KiemTraNhanVienExist(nhanVienID);

                    // Tạo đối tượng NhanVienModel
                    NhanVienModel nhanVienModel = new NhanVienModel
                    {
                        NhanVienID = nhanVienID,
                        HoTen = hoTen,
                        NgaySinh = ngaySinh,
                        SoDienThoai = soDienThoai,
                        DiaChi = diaChi,
                        TenTK = tenTK,
                        MatKhau = matKhau,
                        VaiTro = vaiTro
                    };

                    bool result;

                    if (!nhanVienExist) // Nếu nhân viên đã tồn tại, gọi phương thức Luu
                    {
                        result = nhanVienController.ThemNhanVien(nhanVienModel);
                    }
                    else // Nếu nhân viên chưa tồn tại, gọi phương thức Them
                    {
                        result = nhanVienController.LuuNhanVien(nhanVienModel);
                    }

                    // Kiểm tra kết quả thực hiện
                    if (!result) // Nếu result == false, có lỗi xảy ra
                    {
                        isSuccess = false;
                        MessageBox.Show($"Có lỗi xảy ra khi lưu nhân viên tại dòng {row + 1}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break; // Dừng lại khi có lỗi
                    }
                }

                // Hiển thị thông báo thành công nếu mọi thứ đều thành công
                if (isSuccess)
                {
                    MessageBox.Show("Lưu dữ liệu thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra nếu có dòng nào được chọn
                if (tblNhanVien.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Vui lòng chọn nhân viên cần xóa!", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    return;
                }

                // Lấy dòng đã chọn
                DataGridViewRow selectedRow = tblNhanVien.SelectedRows[0];
                int selectedRowIndex = selectedRow.Index;

                // Hiển thị hộp thoại xác nhận xóa
                DialogResult confirm = MessageBox.Show("Bạn có chắc chắn muốn xóa nhân viên này?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirm == DialogResult.No)
                {
                    return;  // Nếu người dùng chọn "No", không thực hiện xóa
                }

                // Lấy mã nhân viên từ cột "NhanVienID" của dòng đã chọn
                string nhanVienID = selectedRow.Cells[1].Value.ToString();

                if (string.IsNullOrEmpty(nhanVienID))
                {
                    MessageBox.Show("Mã nhân viên không hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Tạo controller và gọi phương thức xóa
                NhanVienController nhanVienController = new NhanVienController();
                bool result = nhanVienController.Xoa(nhanVienID);

                // Kiểm tra kết quả xóa
                if (result)
                {
                    MessageBox.Show("Xóa nhân viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // Cập nhật lại danh sách và DataGridView sau khi xóa thành công
                    tblNhanVien.Rows.RemoveAt(selectedRowIndex);  // Xóa dòng trong DataGridView
                }
                else
                {
                    MessageBox.Show("Xóa nhân viên thất bại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {

            try
    {
        bool isSuccess = true;
        NhanVienController nhanVienController = new NhanVienController();

        // Commit any changes made in the DataGridView
        tblNhanVien.EndEdit();

        for (int row = 0; row < tblNhanVien.Rows.Count; row++)
        {
            DataGridViewRow dgvRow = tblNhanVien.Rows[row];

            // Extract values from the current row
            string nhanVienID = dgvRow.Cells[1].Value?.ToString();
            string hoTen = dgvRow.Cells[2].Value?.ToString();
            DateTime ngaySinh = dgvRow.Cells[3].Value != null ? Convert.ToDateTime(dgvRow.Cells["NgaySinh"].Value) : DateTime.MinValue;
            string soDienThoai = dgvRow.Cells[4].Value?.ToString();
            string diaChi = dgvRow.Cells[5].Value?.ToString();
            string tenTK = dgvRow.Cells[6].Value?.ToString();
            string matKhau = dgvRow.Cells[7].Value?.ToString();
            string vaiTro = dgvRow.Cells[8].Value?.ToString();

            // Check for missing employee ID
            if (string.IsNullOrWhiteSpace(nhanVienID))
            {
                MessageBox.Show($"Mã nhân viên không được để trống tại dòng {row + 1}", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Check if employee already exists
            bool nhanVienExist = nhanVienController.KiemTraNhanVienExist(nhanVienID);

            NhanVienModel nhanVienModel = new NhanVienModel
            {
                NhanVienID = nhanVienID,
                HoTen = hoTen,
                NgaySinh = ngaySinh,
                SoDienThoai = soDienThoai,
                DiaChi = diaChi,
                TenTK = tenTK,
                MatKhau = matKhau,
                VaiTro = vaiTro
            };

            bool result;

            if (!nhanVienExist) // If employee does not exist, call method to add
            {
                result = nhanVienController.ThemNhanVien(nhanVienModel);
            }
            else // If employee exists, call method to update
            {
                result = nhanVienController.LuuNhanVien(nhanVienModel);
            }

            // If saving fails, stop and show error
            if (!result)
            {
                isSuccess = false;
                MessageBox.Show($"Có lỗi xảy ra khi lưu nhân viên tại dòng {row + 1}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                break;
            }
        }

        // Show success message
        if (isSuccess)
        {
            MessageBox.Show("Lưu dữ liệu thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
    catch (Exception ex)
    {
        MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            HienThiDanhSachNhanVien();
        }
    }
}

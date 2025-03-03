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
using OfficeOpenXml;
using OfficeOpenXml.Style;

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
                foreach (var nv in dsnv)
                {
                    string maNhanVien = nv.NhanVienID;
                    string hoTen = nv.HoTen;
                    string ngaySinh = nv.NgaySinh.HasValue ? nv.NgaySinh.Value.ToShortDateString() : ""; // Kiểm tra null
                    string soDienThoai = nv.SoDienThoai;
                    string diaChi = nv.DiaChi;
                    string tenTaiKhoan = nv.TenTK;

                    // Thêm dòng vào DataGridView
                    tblNhanVien.Rows.Add(
                        tblNhanVien.Rows.Count + 1, // Số thứ tự tự động tăng
                        maNhanVien,
                        hoTen,
                        ngaySinh, // Hiển thị ngày nếu có
                        soDienThoai,
                        diaChi,
                        tenTaiKhoan
                    );
                }
            }
            catch (Exception ex)
            {
                // Hiển thị thông báo lỗi nếu có
                MessageBox.Show("Lỗi: " + ex.Message, "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                        TenTK = tenTK
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
                    MessageBox.Show("Đã cập nhật dữ liệu thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        // xuất excel

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Title = "Chọn nơi lưu file Excel";
                saveFileDialog.Filter = "Excel Files (*.xlsx)|*.xlsx";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = saveFileDialog.FileName;

                    try
                    {
                        OfficeOpenXml.ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

                        using (var package = new OfficeOpenXml.ExcelPackage())
                        {
                            var sheet = package.Workbook.Worksheets.Add("Danh sách nhân viên");

                            // Tạo tiêu đề
                            sheet.Cells["A1:G1"].Merge = true; // Gộp các ô từ A1 đến G1
                            sheet.Cells["A1"].Value = "DANH SÁCH NHÂN VIÊN";
                            sheet.Cells["A1"].Style.Font.Size = 16;
                            sheet.Cells["A1"].Style.Font.Bold = true;
                            sheet.Cells["A1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                            // Tạo header
                            string[] headers = { "Số TT", "Mã Nhân Viên", "Họ và Tên", "Ngày Sinh", "Số Điện Thoại", "Địa Chỉ", "Tên Tài Khoản" };
                            for (int i = 0; i < headers.Length; i++)
                            {
                                var headerCell = sheet.Cells[2, i + 1];
                                headerCell.Value = headers[i];
                                headerCell.Style.Font.Bold = true;
                                headerCell.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                headerCell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                                headerCell.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                headerCell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            }

                            // Ghi dữ liệu từ DataGridView vào Excel
                            for (int row = 0; row < tblNhanVien.RowCount; row++)
                            {
                                for (int col = 0; col < tblNhanVien.ColumnCount; col++)
                                {
                                    var cell = tblNhanVien.Rows[row].Cells[col].Value;
                                    var excelCell = sheet.Cells[row + 3, col + 1]; // Bắt đầu từ hàng thứ 3
                                    excelCell.Value = cell != null ? cell.ToString() : "";
                                    excelCell.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                }
                            }

                            // Tự động căn chỉnh cột
                            sheet.Cells[sheet.Dimension.Address].AutoFitColumns();

                            // Lưu file Excel
                            package.SaveAs(new FileInfo(filePath));
                        }

                        MessageBox.Show("Xuất file Excel thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Mở file Excel vừa tạo
                        if (File.Exists(filePath))
                        {
                            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                            {
                                FileName = filePath,
                                UseShellExecute = true
                            });
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi khi xuất file: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }


        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            HienThiDanhSachNhanVien();
        }

        private void tblNhanVien_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}

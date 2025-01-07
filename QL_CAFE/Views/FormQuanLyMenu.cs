using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QL_CAFE.Controllers;
using QL_CAFE.Models;

namespace QL_CAFE.Views
{
    public partial class FormQuanLyMenu : Form
    {
        List<DanhMucModel> dsdm = new List<DanhMucModel>();
        List<DoAnUongModel> dsBan = new List<DoAnUongModel>();
        public FormQuanLyMenu()
        {
            InitializeComponent();
            KhoiTaoBangDanhMuc();
            HienThiDanhSachDanhMucQuanLy();
            KhoiTaoBangDoAn();
            HienThiDSDoAnTheoDanhMuc();
            HienThiMonDeQuanLyTheoDanhSach();
            HienThiDanhMuc();

        }
        // Hiển thị danh sách danh mục
        private void HienThiDanhSachDanhMucQuanLy()
        {
            try
            {
                DanhMucController danhMucController = new DanhMucController();

                // Lấy danh sách danh mục từ controller
                dsdm  = danhMucController.HienThiBangDanhMuc();

                // Xóa dữ liệu cũ trong bảng
                tblDanhMuc.Rows.Clear();

                // Thêm từng danh mục vào bảng
                for (int i = 0; i < dsdm.Count; i++)
                {
                    tblDanhMuc.Rows.Add(
                        i + 1,                             // Số thứ tự
                        dsdm[i].DanhMucID,      // Mã danh mục
                        dsdm[i].TenDanhMuc      // Tên danh mục
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }


        // Khởi tạo bảng danh mục
        private void KhoiTaoBangDanhMuc()
        {
            tblDanhMuc.Columns.Clear();
            tblDanhMuc.Rows.Clear();

            // Cột số thứ tự
            tblDanhMuc.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Số TT",
                Name = "STT",
                ReadOnly = true,
                Width = 100
            });

            // Cột mã danh mục
            tblDanhMuc.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Mã Danh Mục",
                Name = "DanhMucID",
                ReadOnly = false,
                Width = 150
            });

            // Cột tên danh mục
            tblDanhMuc.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Tên Danh Mục",
                Name = "TenDanhMuc",
                ReadOnly = false,
                Width = 250
            });

            tblDanhMuc.AllowUserToAddRows = false;
            tblDanhMuc.AllowUserToDeleteRows = false;
            tblDanhMuc.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            tblDanhMuc.MultiSelect = false;
        }

        private void pcbThemDongDanhMuc_Click(object sender, EventArgs e)
        {
            if (tblDanhMuc.Rows.Count > 0)
            {
                DataGridViewRow lastRow = tblDanhMuc.Rows[tblDanhMuc.Rows.Count - 1];
                if (lastRow.Cells["DanhMucID"].Value == null || string.IsNullOrWhiteSpace(lastRow.Cells["DanhMucID"].Value.ToString()))
                {
                    return;
                }
            }

            tblDanhMuc.Rows.Add(tblDanhMuc.Rows.Count + 1, "", "");
        }

        private void pcbXoaDanhMuc_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra nếu không có dòng nào được chọn
                if (tblDanhMuc.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Vui lòng chọn danh mục cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Lấy dòng được chọn
                DataGridViewRow selectedRow = tblDanhMuc.SelectedRows[0];

                // Lấy mã danh mục từ dòng được chọn
                int maDanhMuc = int.Parse(selectedRow.Cells["DanhMucID"].Value.ToString());

                // Xác nhận xóa danh mục
                DialogResult confirm = MessageBox.Show("Bạn có chắc chắn muốn xóa danh mục này?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (confirm == DialogResult.Yes)
                {
                    DanhMucController danhMucController = new DanhMucController();

                    // Gọi phương thức xóa danh mục từ controller
                    if (danhMucController.XoaDanhMuc(maDanhMuc))
                    {
                        MessageBox.Show("Xóa danh mục thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        tblDanhMuc.Rows.RemoveAt(selectedRow.Index); // Xóa dòng trong bảng
                    }
                    else
                    {
                        MessageBox.Show("Xóa danh mục thất bại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            HienThiDanhSachDanhMucQuanLy();
        }

        private void pcbLuuDanhMuc_Click(object sender, EventArgs e)
        {
            try
            {
                // Tạo controller để quản lý danh mục
                DanhMucController danhMucController = new DanhMucController();

                // Lấy dữ liệu từ bảng tblDanhMuc
                foreach (DataGridViewRow row in tblDanhMuc.Rows)
                {
                    // Bỏ qua dòng mới (chưa có dữ liệu)
                    if (row.IsNewRow) continue;

                    // Lấy mã và tên danh mục từ các ô trong dòng
                    int maDanhMuc;
                    string tenDanhMuc = row.Cells["TenDanhMuc"].Value?.ToString();

                    // Kiểm tra mã danh mục hợp lệ
                    if (!int.TryParse(row.Cells["DanhMucID"].Value?.ToString(), out maDanhMuc))
                    {
                        MessageBox.Show($"Mã danh mục không hợp lệ tại dòng {row.Index + 1}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Kiểm tra tên danh mục không rỗng
                    if (string.IsNullOrWhiteSpace(tenDanhMuc))
                    {
                        MessageBox.Show($"Tên danh mục không được để trống tại dòng {row.Index + 1}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Tạo đối tượng DanhMucModel
                    DanhMucModel danhMuc = new DanhMucModel
                    {
                        DanhMucID = maDanhMuc,
                        TenDanhMuc = tenDanhMuc
                    };

                    // Kiểm tra nếu danh mục đã tồn tại
                    if (danhMucController.KiemTraDanhMucTonTai(maDanhMuc))
                    {
                        // Nếu tồn tại, cập nhật danh mục
                        danhMucController.CapNhatDanhMuc(danhMuc);
                    }
                    else
                    {
                        // Nếu không tồn tại, thêm mới danh mục
                        danhMucController.ThemDanhMuc(danhMuc);
                    }
                }

                MessageBox.Show("Lưu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                HienThiDanhSachDanhMucQuanLy(); // Cập nhật lại danh sách sau khi lưu
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // quản lý bàn
        private void KhoiTaoBangDoAn()
        {
            tblMonTheoDanhMuc.Columns.Clear();
            tblMonTheoDanhMuc.Rows.Clear();

            tblMonTheoDanhMuc.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Số TT", Name = "STT", ReadOnly = true, Width = 100 });
            tblMonTheoDanhMuc.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Mã Đồ Ăn/Uống", Name = "DoAnUongID", ReadOnly = false, Width = 150 });
            tblMonTheoDanhMuc.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Tên Đồ Ăn/Uống", Name = "TenDoAnUong", ReadOnly = false, Width = 150 });
            tblMonTheoDanhMuc.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Giá", Name = "Gia", ReadOnly = false, Width = 150 });
            tblMonTheoDanhMuc.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Mã Danh Mục", Name = "DanhMucID", ReadOnly = false, Width = 150 });

            tblMonTheoDanhMuc.AllowUserToAddRows = false;
            tblMonTheoDanhMuc.AllowUserToDeleteRows = false;
            tblMonTheoDanhMuc.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            tblMonTheoDanhMuc.MultiSelect = false;
        }
        private void HienThiDSDoAnTheoDanhMuc()
        {
            try
            {
                int selectedIndex = cboDanhMuc.SelectedIndex;

                if (selectedIndex >= 0)
                {
                    int? danhMucID = null;

                    // Nếu không phải "Tất cả", lấy DanhMucID từ danh sách
                    if (selectedIndex != 0) // Giả sử mục "Tất cả" nằm ở index 0
                    {
                        danhMucID = dsdm[selectedIndex - 1].DanhMucID; // Sử dụng dsdm thay vì dsDanhMuc
                    }

                    Console.WriteLine("Hiển thị danh mục: " + (danhMucID.HasValue ? danhMucID.Value.ToString() : "Tất cả"));

                    // Gọi controller để lấy danh sách đồ ăn/uống theo danh mục (hoặc tất cả nếu danhMucID là null)
                    DoAnUongController controller = new DoAnUongController();
                    List<DoAnUongModel> danhSachDoAnUongTheoDanhMuc = controller.HienThiDanhSachDoAnUongTheoDanhMuc(danhMucID);

                    tblMonTheoDanhMuc.Rows.Clear(); // Xóa dữ liệu cũ trong bảng

                    if (danhSachDoAnUongTheoDanhMuc.Count > 0)
                    {
                        // Hiển thị danh sách đồ ăn/uống
                        for (int i = 0; i < danhSachDoAnUongTheoDanhMuc.Count; i++)
                        {
                            DoAnUongModel doAnUong = danhSachDoAnUongTheoDanhMuc[i];
                            tblMonTheoDanhMuc.Rows.Add(
                                i + 1, // Số thứ tự
                                doAnUong.DoAnUongID,
                                doAnUong.TenDoAnUong,
                                doAnUong.Gia,  // Thêm thông tin giá vào bảng
                                doAnUong.DanhMucID  // Thêm DanhMucID vào bảng
                            );
                        }
                    }
                    else
                    {
                        // Trả về dữ liệu trống và thông báo
                        MessageBox.Show("Không có đồ ăn/uống trong danh mục được chọn.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void HienThiMonDeQuanLyTheoDanhSach()
        {
            try
            {
                DoAnUongController doAnUongController = new DoAnUongController();
                List<DoAnUongModel> dsDoAnUong = doAnUongController.HienThiDanhSachDoAnUong();

                tblMonTheoDanhMuc.Rows.Clear();

                for (int i = 0; i < dsDoAnUong.Count; i++)
                {
                    DoAnUongModel doAnUong = dsDoAnUong[i];
                    tblMonTheoDanhMuc.Rows.Add(
                        i + 1, // Số thứ tự
                        doAnUong.DoAnUongID,
                        doAnUong.TenDoAnUong,
                        doAnUong.Gia,
                        doAnUong.DanhMucID
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void HienThiDanhMuc()
        {
            try
            {
                DanhMucController controllerdm = new DanhMucController();
                List<DanhMucModel> dsdm = controllerdm.HienThiDanhSachDanhMuc(); // Lấy danh sách danh mục

                cboDanhMuc.Items.Clear();

                // Thêm mục "Tất cả" vào đầu tiên
                cboDanhMuc.Items.Add("Tất cả");

                // Duyệt qua danh sách danh mục và thêm tên vào ComboBox
                foreach (DanhMucModel danhmuc in dsdm)
                {
                    cboDanhMuc.Items.Add(danhmuc.TenDanhMuc); // Chỉ hiển thị tên danh mục
                }

                // Chọn mục đầu tiên làm mặc định
                cboDanhMuc.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void pcbThemDongMon_Click(object sender, EventArgs e)
        {
            // Kiểm tra nếu DataGridView đã có một dòng trống
            if (tblMonTheoDanhMuc.Rows.Count > 0)
            {
                DataGridViewRow lastRow = tblMonTheoDanhMuc.Rows[tblMonTheoDanhMuc.Rows.Count - 1];
                if (lastRow.Cells["DoAnUongID"].Value == null || string.IsNullOrWhiteSpace(lastRow.Cells["DoAnUongID"].Value.ToString()))
                {
                    // Nếu dòng cuối cùng trống, không thêm dòng mới
                    return;
                }
            }

            // Thêm dòng trống vào DataGridView
            tblMonTheoDanhMuc.Rows.Add(
                tblMonTheoDanhMuc.Rows.Count + 1,
                "",
                "",
                "",
                ""

            );
        }

        private void pcbLuuThongTinMon_Click(object sender, EventArgs e)
        {
            try
            {
                bool isSuccess = true;
                DoAnUongController doAnUongController = new DoAnUongController();

                // Lặp qua từng dòng trong bảng
                foreach (DataGridViewRow row in tblMonTheoDanhMuc.Rows)
                {
                    if (row.IsNewRow) continue;

                    int doAnUongID;
                    if (!int.TryParse(row.Cells["DoAnUongID"].Value?.ToString(), out doAnUongID))
                    {
                        MessageBox.Show($"Mã đồ ăn/uống không hợp lệ tại dòng {row.Index + 1}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    string tenDoAnUong = row.Cells["TenDoAnUong"].Value?.ToString();
                    decimal gia;
                    if (!decimal.TryParse(row.Cells["Gia"].Value?.ToString(), out gia))
                    {
                        MessageBox.Show($"Giá không hợp lệ tại dòng {row.Index + 1}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    int danhMucID;
                    if (!int.TryParse(row.Cells["DanhMucID"].Value?.ToString(), out danhMucID))
                    {
                        MessageBox.Show($"Mã danh mục không hợp lệ tại dòng {row.Index + 1}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Tạo đối tượng ChonDoModel
                    DoAnUongModel doAnUong = new DoAnUongModel
                    {
                        DoAnUongID = doAnUongID,
                        TenDoAnUong = tenDoAnUong,
                        Gia = gia,
                        DanhMucID = danhMucID
                    };

                    // Kiểm tra xem món ăn/uống đã tồn tại chưa
                    if (doAnUongController.KiemTraDoAnUongTonTai(doAnUongID))
                    {
                        // Nếu có, cập nhật món ăn/uống
                        if (!doAnUongController.CapNhatDoAnUong(doAnUong))
                        {
                            isSuccess = false;
                            MessageBox.Show($"Có lỗi khi cập nhật món ăn/uống tại dòng {row.Index + 1}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                        }
                    }
                    else
                    {
                        // Nếu không, thêm món ăn/uống mới
                        if (!doAnUongController.ThemDoAnUong(doAnUong))
                        {
                            isSuccess = false;
                            MessageBox.Show($"Có lỗi khi thêm món ăn/uống tại dòng {row.Index + 1}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                        }
                    }
                }

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

        private void pcbXoaMon_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra xem có món ăn/uống nào được chọn không
                if (tblMonTheoDanhMuc.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Vui lòng chọn món cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Lấy thông tin món ăn/uống được chọn
                DataGridViewRow selectedRow = tblMonTheoDanhMuc.SelectedRows[0];
                int doAnUongID = int.Parse(selectedRow.Cells["DoAnUongID"].Value.ToString());

                // Xác nhận trước khi xóa
                DialogResult confirm = MessageBox.Show("Bạn có chắc chắn muốn xóa món ăn/uống này?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirm == DialogResult.Yes)
                {
                    DoAnUongController doAnUongController = new DoAnUongController();

                    // Gọi phương thức xóa món ăn/uống từ controller
                    if (doAnUongController.XoaDoAnUong(doAnUongID))
                    {
                        MessageBox.Show("Xóa món ăn/uống thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        tblMonTheoDanhMuc.Rows.RemoveAt(selectedRow.Index); // Xóa dòng trong bảng
                    }
                    else
                    {
                        MessageBox.Show("Xóa món ăn/uống thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void pcbTimKiem_Click(object sender, EventArgs e)
        {
            HienThiDSDoAnTheoDanhMuc();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            HienThiMonDeQuanLyTheoDanhSach();
        }
    }
}

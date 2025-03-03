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
    public partial class FormQuanLyBan : Form
    {
        List<KhuVucModel> dskv = new List<KhuVucModel>();
        List<BanModel> dsBan = new List<BanModel>();
        public FormQuanLyBan()
        {
            InitializeComponent();
            KhoiTaoBangKhuVuc();
            HienThiKhuVuc();
            KhoiTaoBangBan();  // Khởi tạo bảng bàn
            HienThiBanDeQuanLyTheoDanhSach();
            HienThiDanhSachKhuVuc();
        }
        
        private void HienThiDanhSachKhuVuc()
        {
            try
            {
                QuanLyKhuVucController khuVucController = new QuanLyKhuVucController();
                
                dskv = khuVucController.HienThiDanhSachKhuVuc();

                tblKhuVuc.Rows.Clear();

                for (int i = 0; i < dskv.Count; i++)
                {
                    KhuVucModel kv = dskv[i];
                    tblKhuVuc.Rows.Add(
                        i + 1, // Số thứ tự
                        kv.KhuVucID,
                        kv.TenKhuVuc
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void KhoiTaoBangKhuVuc()
        {
            tblKhuVuc.Columns.Clear();
            tblKhuVuc.Rows.Clear();

            tblKhuVuc.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Số TT", Name = "STT", ReadOnly = true, Width = 100 });
            tblKhuVuc.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Mã Khu Vực", Name = "MaKhuVuc", ReadOnly = false, Width = 200 });
            tblKhuVuc.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Tên Khu Vực", Name = "TenKhuVuc", ReadOnly = false, Width = 350 });

            tblKhuVuc.AllowUserToAddRows = false;
            tblKhuVuc.AllowUserToDeleteRows = false;
            tblKhuVuc.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            tblKhuVuc.MultiSelect = false;
        }
        private void FormQuanLyBan_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
        }

        private void pcbThemDongKhuVuc_Click(object sender, EventArgs e)
        {
            if (tblKhuVuc.Rows.Count > 0)
            {
                DataGridViewRow lastRow = tblKhuVuc.Rows[tblKhuVuc.Rows.Count - 1];
                if (lastRow.Cells["MaKhuVuc"].Value == null || string.IsNullOrWhiteSpace(lastRow.Cells["MaKhuVuc"].Value.ToString()))
                {
                    return;
                }
            }

            tblKhuVuc.Rows.Add(tblKhuVuc.Rows.Count + 1, "", "");
        }
        private void pcbLuuKhuVuc_Click(object sender, EventArgs e)
        {
            try
            {
                QuanLyKhuVucController khuVucController = new QuanLyKhuVucController();
                tblKhuVuc.EndEdit();

                foreach (DataGridViewRow row in tblKhuVuc.Rows)
                {
                    if (row.IsNewRow) continue;

                    int maKhuVuc;
                    if (!int.TryParse(row.Cells["MaKhuVuc"].Value?.ToString(), out maKhuVuc))
                    {
                        MessageBox.Show($"Mã khu vực không hợp lệ tại dòng {row.Index + 1}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    string tenKhuVuc = row.Cells["TenKhuVuc"].Value?.ToString();
                    if (string.IsNullOrWhiteSpace(tenKhuVuc))
                    {
                        MessageBox.Show($"Tên khu vực không được để trống tại dòng {row.Index + 1}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    KhuVucModel khuVuc = new KhuVucModel { KhuVucID = maKhuVuc, TenKhuVuc = tenKhuVuc };

                    if (khuVucController.KiemTraKhuVucTonTai(maKhuVuc))
                    {
                        khuVucController.CapNhatKhuVuc(khuVuc);
                    }
                    else
                    {
                        khuVucController.ThemKhuVuc(khuVuc);
                    }
                }

                MessageBox.Show("Lưu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                HienThiDanhSachKhuVuc();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void pcbXoaKhuVuc_Click(object sender, EventArgs e)
        {
            try
            {
                if (tblKhuVuc.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Vui lòng chọn khu vực cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DataGridViewRow selectedRow = tblKhuVuc.SelectedRows[0];
                int maKhuVuc = int.Parse(selectedRow.Cells["MaKhuVuc"].Value.ToString());

                // Kiểm tra xem khu vực có bàn hay không
                QuanLyKhuVucController khuVucController = new QuanLyKhuVucController();
                if (khuVucController.KiemTraKhuVucCoBan(maKhuVuc))
                {
                    // Nếu có bàn, yêu cầu người dùng xác nhận xóa tất cả bàn
                    DialogResult confirm = MessageBox.Show("Khu vực này có bàn. Bạn có muốn xóa tất cả bàn trong khu vực này không?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (confirm == DialogResult.Yes)
                    {
                        // Xóa các bàn trong khu vực trước khi xóa khu vực
                        if (khuVucController.XoaTatCaBanTrongKhuVuc(maKhuVuc))
                        {
                            // Xóa khu vực
                            if (khuVucController.XoaKhuVuc(maKhuVuc))
                            {
                                MessageBox.Show("Xóa khu vực và bàn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                tblKhuVuc.Rows.RemoveAt(selectedRow.Index);
                            }
                            else
                            {
                                MessageBox.Show("Xóa khu vực thất bại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Xóa bàn thất bại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else
                {
                    // Nếu không có bàn, xóa khu vực bình thường
                    DialogResult confirm = MessageBox.Show("Bạn có chắc chắn muốn xóa khu vực này?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (confirm == DialogResult.Yes)
                    {
                        if (khuVucController.XoaKhuVuc(maKhuVuc))
                        {
                            MessageBox.Show("Xóa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            tblKhuVuc.Rows.RemoveAt(selectedRow.Index);
                        }
                        else
                        {
                            MessageBox.Show("Xóa thất bại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        // Khởi tạo bảng bàn
        private void KhoiTaoBangBan()
        {
            tblBanTheoKhuVuc.Columns.Clear();
            tblBanTheoKhuVuc.Rows.Clear();

            tblBanTheoKhuVuc.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Số TT", Name = "STT", ReadOnly = true, Width = 100 });
            tblBanTheoKhuVuc.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Mã Bàn", Name = "MaBan", ReadOnly = false, Width = 150 });
            tblBanTheoKhuVuc.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Tên Bàn", Name = "TenBan", ReadOnly = false, Width = 180 });
            tblBanTheoKhuVuc.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Trạng Thái", Name = "TrangThai", ReadOnly = false, Width = 150 });
            tblBanTheoKhuVuc.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Mã Khu Vực", Name = "MaKhuVuc", ReadOnly = false, Width = 220 });

            tblBanTheoKhuVuc.AllowUserToAddRows = false;
            tblBanTheoKhuVuc.AllowUserToDeleteRows = false;
            tblBanTheoKhuVuc.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            tblBanTheoKhuVuc.MultiSelect = false;
        }
        private void HienThiDSBanTheoKhuVuc()
        {
            try
            {
                int selectedIndex = cboKhuVuc.SelectedIndex;

                if (selectedIndex >= 0)
                {
                    int? khuVucID = null;

                    // Nếu không phải "Tất cả", lấy KhuVucID từ danh sách
                    if (selectedIndex != 0) // Giả sử mục "Tất cả" nằm ở index 0
                    {
                        khuVucID = dskv[selectedIndex - 1].KhuVucID;
                    }

                    Console.WriteLine("Hiển thị khu vực: " + (khuVucID.HasValue ? khuVucID.Value.ToString() : "Tất cả"));

                    // Gọi controller để lấy danh sách bàn
                    QuanLyBanController controllerBan = new QuanLyBanController();
                    List<BanModel> danhSachBanTheoKhuVuc = controllerBan.HienThiDanhSachBanTheoKhuVuc(khuVucID);

                    tblBanTheoKhuVuc.Rows.Clear(); // Xóa dữ liệu cũ trong bảng

                    if (danhSachBanTheoKhuVuc.Count > 0)
                    {
                        // Hiển thị danh sách bàn
                        for (int i = 0; i < danhSachBanTheoKhuVuc.Count; i++)
                        {
                            BanModel ban = danhSachBanTheoKhuVuc[i];
                            tblBanTheoKhuVuc.Rows.Add(
                                i + 1, // Số thứ tự
                                ban.BanID,
                                ban.SoBan,
                                ban.TrangThai,
                                ban.KhuVucID
                            );
                        }
                    }
                    else
                    {
                        // Trả về dữ liệu trống và thông báo
                        MessageBox.Show("Không có bàn trong khu vực được chọn.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void HienThiBanDeQuanLyTheoDanhSach()
        {
            try
            {
                QuanLyBanController banController = new QuanLyBanController();
                List<BanModel> dsBan = banController.HienThiDanhSachBan();

                tblBanTheoKhuVuc.Rows.Clear();

                for (int i = 0; i < dsBan.Count; i++)
                {
                    BanModel ban = dsBan[i];
                    tblBanTheoKhuVuc.Rows.Add(
                        i + 1, // Số thứ tự
                        ban.BanID,
                        ban.SoBan,
                        ban.TrangThai,
                        ban.KhuVucID
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void pcbThemDongBan_Click(object sender, EventArgs e)
        {
            // Kiểm tra nếu DataGridView đã có một dòng trống
            if (tblBanTheoKhuVuc.Rows.Count > 0)
            {
                DataGridViewRow lastRow = tblBanTheoKhuVuc.Rows[tblBanTheoKhuVuc.Rows.Count - 1];
                if (lastRow.Cells["MaBan"].Value == null || string.IsNullOrWhiteSpace(lastRow.Cells["MaBan"].Value.ToString()))
                {
                    // Nếu dòng cuối cùng trống, không thêm dòng mới
                    return;
                }
            }

            // Thêm dòng trống vào DataGridView
            tblBanTheoKhuVuc.Rows.Add(
                tblBanTheoKhuVuc.Rows.Count + 1, 
                "",                         
                "",                         
                "",                         
                ""                        
                
            );
        }

        private void pcbLuuThongTinBan_Click(object sender, EventArgs e)
        {
            try
            {
                bool isSuccess = true;
                QuanLyBanController banController = new QuanLyBanController();

                // Đảm bảo commit các thay đổi từ DataGridView
                tblBanTheoKhuVuc.EndEdit();

                for (int row = 0; row < tblBanTheoKhuVuc.Rows.Count; row++)
                {
                    DataGridViewRow dgvRow = tblBanTheoKhuVuc.Rows[row];

                    // Lấy giá trị từ các cột trong DataGridView
                    int maBan;
                    if (!int.TryParse(dgvRow.Cells["MaBan"].Value?.ToString(), out maBan))
                    {
                        MessageBox.Show($"Mã bàn không hợp lệ tại dòng {row + 1}", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    string tenBan = dgvRow.Cells["TenBan"].Value?.ToString();
                    string trangThai = dgvRow.Cells["TrangThai"].Value?.ToString();
                    int maKhuVuc;
                    if (!int.TryParse(dgvRow.Cells["MaKhuVuc"].Value?.ToString(), out maKhuVuc))
                    {
                        MessageBox.Show($"Mã khu vực không hợp lệ tại dòng {row + 1}", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Kiểm tra dữ liệu nhập
                    if (string.IsNullOrWhiteSpace(tenBan))
                    {
                        MessageBox.Show($"Tên bàn không được để trống tại dòng {row + 1}", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Tạo đối tượng BanModel
                    BanModel banModel = new BanModel
                    {
                        BanID = maBan,
                        SoBan = tenBan,
                        TrangThai = trangThai,
                        KhuVucID = maKhuVuc
                    };

                    // Kiểm tra xem bàn đã tồn tại chưa, nếu chưa thì thêm mới, nếu đã tồn tại thì cập nhật
                    bool banExist = banController.KiemTraBanExist(maBan);
                    bool result;
                    if (!banExist) // Nếu bàn chưa tồn tại, thêm mới
                    {
                        result = banController.ThemBan(banModel);
                    }
                    else // Nếu bàn đã tồn tại, cập nhật
                    {
                        result = banController.CapNhatBan(banModel);
                    }

                    // Kiểm tra kết quả thực hiện
                    if (!result) // Nếu result == false, có lỗi xảy ra
                    {
                        isSuccess = false;
                        MessageBox.Show($"Có lỗi xảy ra khi lưu bàn tại dòng {row + 1}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        private void HienThiKhuVuc()
        {
            try
            {
                KhuVucController controllerkv = new KhuVucController();
                dskv = controllerkv.HienThiDanhSachKhuVuc(); // Lấy danh sách khu vực

                cboKhuVuc.Items.Clear();

                // Thêm mục "Tất cả" vào đầu tiên
                cboKhuVuc.Items.Add("Tất cả");

                // Duyệt qua danh sách khu vực và thêm tên vào ComboBox
                foreach (KhuVucModel khuvuc in dskv)
                {
                    cboKhuVuc.Items.Add(khuvuc.TenKhuVuc); // Chỉ hiển thị tên khu vực
                }

                // Chọn mục đầu tiên làm mặc định
                cboKhuVuc.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void pcbXoaBan_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra xem có bàn nào được chọn hay không
                if (tblBanTheoKhuVuc.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Vui lòng chọn bàn cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Lấy thông tin bàn được chọn
                DataGridViewRow selectedRow = tblBanTheoKhuVuc.SelectedRows[0];
                int maBan = int.Parse(selectedRow.Cells[1].Value.ToString());

                // Xác nhận trước khi xóa
                DialogResult confirm = MessageBox.Show("Bạn có chắc chắn muốn xóa bàn này?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirm == DialogResult.Yes)
                {
                    QuanLyBanController banController = new QuanLyBanController();

                    // Gọi phương thức xóa bàn từ controller
                    bool isDeleted = banController.XoaBan(maBan);

                    if (isDeleted)
                    {
                        MessageBox.Show("Xóa bàn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Xóa dòng bàn trong DataGridView
                        tblBanTheoKhuVuc.Rows.RemoveAt(selectedRow.Index);
                    }
                    else
                    {
                        MessageBox.Show("Xóa bàn thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            HienThiBanDeQuanLyTheoDanhSach();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            HienThiDanhSachKhuVuc();
        }

        private void pictureBox12_Click(object sender, EventArgs e)
        {
            HienThiDSBanTheoKhuVuc();
        }

        private void cboKhuVuc_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}

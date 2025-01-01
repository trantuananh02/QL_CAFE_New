using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QL_CAFE.Controllers;
using QL_CAFE.Models;

namespace QL_CAFE.Views
{
    public partial class FormMain : Form
    {
        List<KhuVucModel> dskv = new List<KhuVucModel>(); // Giữ nguyên kiểu List<string>
        List<string> dsDanhMuc = new List<string>(); // Danh sách danh mục
        List<BanModel> dsBan = new List<BanModel>(); // Danh sách bàn
        List<ChonDoModel> dsDoAnUong = new List<ChonDoModel>(); // Danh sách đồ ăn uống

        public FormMain()
        {
            InitializeComponent();
            HienThiKhuVuc();
            HienThiDanhMuc(); // Gọi hàm hiển thị danh mục
            HienThiBan(); // Gọi hàm hiển thị bàn
            HienThiDoAnUong(); // Gọi hàm hiển thị đồ ăn uống
        }

        private void HienThiDoAnUong()
        {
            try
            {
                ChonDoController controllerDoAn = new ChonDoController();
                dsDoAnUong = controllerDoAn.HienThiDanhSachDoAnUong(); // Lấy danh sách đồ ăn uống

                pnlDoAnUong.Controls.Clear();

                int x = 10; // Vị trí X ban đầu
                int y = 10; // Vị trí Y ban đầu
                int itemWidth = 120; // Chiều rộng của item (chỉ có tên món ăn)
                int itemHeight = 120; // Chiều cao của item
                int margin = 10; // Khoảng cách giữa các món ăn

                foreach (ChonDoModel doAnUong in dsDoAnUong)
                {
                    // Tạo Panel cho mỗi món ăn
                    Panel panelItem = new Panel();
                    panelItem.Size = new Size(itemWidth, itemHeight);
                    panelItem.Location = new Point(x, y);
                    panelItem.BorderStyle = BorderStyle.FixedSingle;
                    panelItem.BackColor = Color.White;

                    // Tạo Label để hiển thị tên món ăn
                    Label lblTenDoAn = new Label();
                    lblTenDoAn.Text = doAnUong.TenDoAnUong;
                    lblTenDoAn.Location = new Point(5, (itemHeight - 20) / 2); // Canh giữa tên món ăn
                    lblTenDoAn.Size = new Size(itemWidth - 10, 20); // Chiều cao cho tên món ăn
                    lblTenDoAn.TextAlign = ContentAlignment.MiddleCenter;

                    // Thêm Label vào Panel
                    panelItem.Controls.Add(lblTenDoAn);

                    // Thêm Panel vào pnlDoAnUong
                    pnlDoAnUong.Controls.Add(panelItem);

                    // Tính toán vị trí của item tiếp theo
                    x += itemWidth + margin;
                    if (x + itemWidth > pnlDoAnUong.Width)
                    {
                        x = 10;
                        y += itemHeight + margin;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void HienThiDoAnUongTheoDanhMuc(List<ChonDoModel> dsDoAnTheoDanhMuc)
        {
            try
            {
                pnlDoAnUong.Controls.Clear(); // Làm sạch Panel trước khi hiển thị lại

                int x = 10; // Vị trí X ban đầu
                int y = 10; // Vị trí Y ban đầu
                int itemWidth = 120; // Chiều rộng của item (chỉ có tên món ăn)
                int itemHeight = 120; // Chiều cao của item
                int margin = 10; // Khoảng cách giữa các món ăn

                foreach (ChonDoModel doAnUong in dsDoAnTheoDanhMuc)
                {
                    // Tạo Panel cho mỗi món ăn
                    Panel panelItem = new Panel();
                    panelItem.Size = new Size(itemWidth, itemHeight);
                    panelItem.Location = new Point(x, y);
                    panelItem.BorderStyle = BorderStyle.FixedSingle;
                    panelItem.BackColor = Color.White;

                    // Tạo Label để hiển thị tên món ăn
                    Label lblTenDoAn = new Label();
                    lblTenDoAn.Text = doAnUong.TenDoAnUong;
                    lblTenDoAn.Location = new Point(5, (itemHeight - 20) / 2); // Canh giữa tên món ăn
                    lblTenDoAn.Size = new Size(itemWidth - 10, 20); // Chiều cao cho tên món ăn
                    lblTenDoAn.TextAlign = ContentAlignment.MiddleCenter;

                    // Thêm Label vào Panel
                    panelItem.Controls.Add(lblTenDoAn);

                    // Thêm Panel vào pnlDoAnUong
                    pnlDoAnUong.Controls.Add(panelItem);

                    // Tính toán vị trí của item tiếp theo
                    x += itemWidth + margin;
                    if (x + itemWidth > pnlDoAnUong.Width)
                    {
                        x = 10;
                        y += itemHeight + margin;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void HienThiKhuVuc()
        {
            try
            {
                KhuVucController controllerkv = new KhuVucController();
                dskv = controllerkv.HienThiDanhSachKhuVuc(); // Lấy danh sách khu vực

                cbKhuVuc.Items.Clear();

                // Duyệt qua danh sách khu vực và thêm tên vào ComboBox
                foreach (KhuVucModel khuvuc in dskv)
                {
                    cbKhuVuc.Items.Add(khuvuc.TenKhuVuc); // Chỉ hiển thị tên khu vực
                }

                // Nếu cần, chọn mục đầu tiên làm mặc định
                if (cbKhuVuc.Items.Count > 0)
                {
                    cbKhuVuc.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void HienThiDanhMuc()
        {
            try
            {
                DanhMucController controllerdm = new DanhMucController();
                List<DanhMucModel> danhMucList = controllerdm.HienThiDanhSachDanhMuc();

                dsDanhMuc.Clear(); // Làm sạch danh sách trước khi thêm

                // Duyệt qua danh sách danh mục và thêm tên vào List<string>
                foreach (DanhMucModel danhMuc in danhMucList)
                {
                    dsDanhMuc.Add(danhMuc.TenDanhMuc); // Chỉ thêm tên danh mục vào dsDanhMuc
                }

                // Làm mới ComboBox (giả sử bạn có ComboBox tên cbDanhMuc)
                cbDanhMucDo.Items.Clear();

                // Thêm dữ liệu vào ComboBox
                foreach (string tenDanhMuc in dsDanhMuc)
                {
                    cbDanhMucDo.Items.Add(tenDanhMuc);
                }

                // Chọn mục đầu tiên nếu có
                if (cbDanhMucDo.Items.Count > 0)
                {
                    cbDanhMucDo.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }


        private void HienThiBan()
        {
            try
            {
                QuanLyBanController controllerBan = new QuanLyBanController();
                dsBan = controllerBan.HienThiDanhSachBan();

                // Làm sạch Panel trước khi thêm các nút bàn
                pnlBan.Controls.Clear();

                int x = 10; // Vị trí X ban đầu
                int y = 10; // Vị trí Y ban đầu
                int buttonWidth = 100; // Chiều rộng của nút
                int buttonHeight = 100; // Chiều cao của nút
                int margin = 10; // Khoảng cách giữa các nút

                foreach (BanModel ban in dsBan)
                {
                    // Tạo nút mới cho mỗi bàn
                    Button btnBan = new Button();
                    btnBan.Text = $"{ban.SoBan}\n{ban.TrangThai}";
                    btnBan.Size = new Size(buttonWidth, buttonHeight);
                    btnBan.Location = new Point(x, y);

                    // Đổi màu theo tình trạng
                    if (ban.TrangThai == "Trống")
                        btnBan.BackColor = Color.LightGreen;
                    else if (ban.TrangThai == "Đang Sử Dụng")
                        btnBan.BackColor = Color.LightCoral;

                    // Thêm sự kiện click để hiển thị số bàn vào txtBanDangChon
                    btnBan.Click += (s, e) =>
                    {
                        txtBanDangChon.Text = ban.SoBan.ToString(); // Cập nhật số bàn vào TextBox
                    };

                    // Thêm nút vào Panel
                    pnlBan.Controls.Add(btnBan);

                    // Tính toán vị trí của nút tiếp theo
                    x += buttonWidth + margin;
                    if (x + buttonWidth > pnlBan.Width)
                    {
                        x = 10;
                        y += buttonHeight + margin;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}");
            }
        }

        private void quảnLýNhânViênToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormQuanLyNhanVien form = new FormQuanLyNhanVien();
            form.ShowDialog();
        }

        private void cbKhuVuc_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                // Lấy khu vực được chọn
                int selectedIndex = cbKhuVuc.SelectedIndex;
                if (selectedIndex >= 0 && selectedIndex < dskv.Count)
                {
                    int khuVucID = dskv[selectedIndex].KhuVucID; // Lấy ID của khu vực

                    // Gọi controller để lấy danh sách bàn theo khu vực ID
                    QuanLyBanController controllerBan = new QuanLyBanController();
                    List<BanModel> danhSachBanTheoKhuVuc = controllerBan.HienThiDanhSachBanTheoKhuVuc(khuVucID);

                    // Hiển thị danh sách bàn
                    HienThiBanTheoDanhSach(danhSachBanTheoKhuVuc);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}");
            }
        }

        private void HienThiBanTheoDanhSach(List<BanModel> danhSachBan)
        {
            // Làm sạch Panel trước khi thêm các nút bàn
            pnlBan.Controls.Clear();

            int x = 10; // Vị trí X ban đầu
            int y = 10; // Vị trí Y ban đầu
            int buttonWidth = 100; // Chiều rộng của nút
            int buttonHeight = 100; // Chiều cao của nút
            int margin = 10; // Khoảng cách giữa các nút

            foreach (BanModel ban in danhSachBan)
            {
                // Tạo nút mới cho mỗi bàn
                Button btnBan = new Button();
                btnBan.Text = $"{ban.SoBan}\n{ban.TrangThai}";
                btnBan.Size = new Size(buttonWidth, buttonHeight);
                btnBan.Location = new Point(x, y);

                // Đổi màu theo tình trạng
                if (ban.TrangThai == "Trống")
                    btnBan.BackColor = Color.LightGreen;
                else if (ban.TrangThai == "Đang Sử Dụng")
                    btnBan.BackColor = Color.LightCoral;

                // Thêm sự kiện click để hiển thị số bàn vào txtBanDangChon
                btnBan.Click += (s, e) =>
                {
                    txtBanDangChon.Text = ban.SoBan.ToString(); // Cập nhật số bàn vào TextBox
                };

                // Thêm nút vào Panel
                pnlBan.Controls.Add(btnBan);

                // Tính toán vị trí của nút tiếp theo
                x += buttonWidth + margin;
                if (x + buttonWidth > pnlBan.Width)
                {
                    x = 10;
                    y += buttonHeight + margin;
                }
            }
        }

        private void btnTatCaKhuVuc_Click(object sender, EventArgs e)
        {
            HienThiBan();
        }

        private void cbDanhMucDo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                // Lấy DanhMucID của danh mục được chọn từ danh sách dsDanhMuc
                string selectedCategory = cbDanhMucDo.SelectedItem.ToString();
                int danhMucID = dsDanhMuc.IndexOf(selectedCategory) + 1; // Giả sử DanhMucID là chỉ số (1-based)

                // Gọi controller để lấy danh sách đồ ăn uống theo DanhMucID
                ChonDoController controllerDoAn = new ChonDoController();
                List<ChonDoModel> dsDoAnTheoDanhMuc = controllerDoAn.HienThiDanhSachDoAnUongTheoDanhMuc(danhMucID);

                // Hiển thị danh sách đồ ăn uống tương ứng
                HienThiDoAnUongTheoDanhMuc(dsDoAnTheoDanhMuc);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}");
            }
        }

        private void btnTatCaDoAn_Click(object sender, EventArgs e)
        {
            HienThiDoAnUong();
        }

        private void quảnLýBànToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormQuanLyBan form = new FormQuanLyBan();
            form.ShowDialog();
        }

        private void quảnLýMenuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormQuanLyMenu form = new FormQuanLyMenu();
            form.ShowDialog();
        }
    }
}


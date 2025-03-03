using QL_CAFE.Controllers;
using QL_CAFE.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QL_CAFE.Views
{
    public partial class FormGopBan : Form
    {
        List<KhuVucModel> dskv = new List<KhuVucModel>(); // Giữ nguyên kiểu List<string>
        List<BanModel> dsBan = new List<BanModel>(); // Danh sách bàn
        public static int selectedBanID;
        public static int selectedBanID2;
        public FormGopBan()
        {
            InitializeComponent();
            HienThiKhuVuc();
            HienThiBanCoNguoi();
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
        public void HienThiBanCoNguoi()
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
                    // Chỉ xử lý các bàn có trạng thái "Đang Sử Dụng"
                    if (ban.TrangThai != "Đang Sử Dụng")
                        continue;

                    // Tạo nút mới cho mỗi bàn
                    Button btnBan = new Button();
                    btnBan.Text = $"{ban.SoBan}\n{ban.TrangThai}";
                    btnBan.Size = new Size(buttonWidth, buttonHeight);
                    btnBan.Location = new Point(x, y);
                    btnBan.BackColor = Color.LightCoral; // Màu cho trạng thái "Đang Sử Dụng"

                    // Thêm sự kiện click để lưu BanID vào biến
                    btnBan.Click += (s, e) =>
                    {
                        // Lưu BanID vào biến selectedBanID
                        selectedBanID = ban.BanID;  // Giả sử BanModel có thuộc tính BanID
                        Console.WriteLine("Bàn nè ae:" + selectedBanID);
                        // Hiển thị thông tin bàn trong TextBox
                        txtBanDangChon.Text = ban.SoBan.ToString();
                        lblTable.Text = ban.SoBan.ToString();
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


        private void HienThiBanTheoDanhSach(List<BanModel> danhSachBan)
        {
            // Làm sạch Panel trước khi thêm các nút bàn
            pnlBanMuonChuyen.Controls.Clear();

            int x = 10; // Vị trí X ban đầu
            int y = 10; // Vị trí Y ban đầu
            int buttonWidth = 100; // Chiều rộng của nút
            int buttonHeight = 100; // Chiều cao của nút
            int margin = 10; // Khoảng cách giữa các nút

            foreach (BanModel ban in danhSachBan)
            {
                // Chỉ xử lý các bàn có trạng thái "Đang Sử Dụng"
                if (ban.TrangThai != "Đang Sử Dụng")
                    continue;

                // Tạo nút mới cho mỗi bàn
                Button btnBan = new Button();
                btnBan.Text = $"{ban.SoBan}\n{ban.TrangThai}";
                btnBan.Size = new Size(buttonWidth, buttonHeight);
                btnBan.Location = new Point(x, y);
                btnBan.BackColor = Color.LightCoral; // Màu cho trạng thái "Đang Sử Dụng"

                // Thêm sự kiện click để hiển thị số bàn vào lblRight
                btnBan.Click += (s, e) =>
                {
                    // Lưu BanID vào biến selectedBanID
                    selectedBanID2 = ban.BanID; // Giả sử BanModel có thuộc tính BanID
                    Console.WriteLine("Bàn nè ae:" + selectedBanID2);
                    lblRight.Text = ban.SoBan.ToString(); // Hiển thị thông tin bàn trong Label
                };

                // Thêm nút vào Panel
                pnlBanMuonChuyen.Controls.Add(btnBan);

                // Tính toán vị trí của nút tiếp theo
                x += buttonWidth + margin;
                if (x + buttonWidth > pnlBan.Width)
                {
                    x = 10;
                    y += buttonHeight + margin;
                }
            }
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

        private void btnAccept_Click(object sender, EventArgs e)
        {
            try
            {
                if (selectedBanID == 0 || selectedBanID2 == 0)
                {
                    MessageBox.Show("Vui lòng chọn hai bàn để gộp!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                QuanLyBanController controller = new QuanLyBanController();
                controller.GopBan(selectedBanID, selectedBanID2);

                HienThiBanCoNguoi();
                HienThiBanTheoDanhSach(dsBan);

                // Lấy FormMain từ danh sách các form đang mở
                FormMain formMain = Application.OpenForms["FormMain"] as FormMain;
                if (formMain != null)
                {
                    formMain.HienThiBan();
                    formMain.HienThiChiTietHoaDonTheoBan();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi khi gộp bàn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

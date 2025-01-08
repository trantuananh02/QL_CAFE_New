using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
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
        List<DoAnUongModel> dsDoAnUong = new List<DoAnUongModel>(); // Danh sách đồ ăn uống
        List<ChiTietHoaDonModel> danhSachChiTiet = new List<ChiTietHoaDonModel>();
        public static int selectedBanID ;
        public static int hoaDonID=0;
        public static int ChiTietID;
        public static int DoAnID;
        private ContextMenuStrip contextMenu;
        private ToolStripMenuItem menuItemXoa;
        private string tenMonDuocChon;
        private decimal giaMonDuocChon; // Giá của món được chọn
        public FormMain()
        {
            InitializeComponent();
            // Tạo ContextMenuStrip
            contextMenu = new ContextMenuStrip();

            // Tạo mục "Xóa"
            menuItemXoa = new ToolStripMenuItem("Xóa");

            // Thêm mục vào menu
            contextMenu.Items.Add(menuItemXoa);

            // Gán sự kiện click cho mục "Xóa"
            menuItemXoa.Click += MenuItemXoa_Click;

            // Gán ContextMenuStrip cho DataGridView
            dtgvDoDaChon.ContextMenuStrip = contextMenu;
            HienThiKhuVuc();
            HienThiDanhMuc(); // Gọi hàm hiển thị danh mục
            HienThiBan(); // Gọi hàm hiển thị bàn
            HienThiDoAnUong(); // Gọi hàm hiển thị đồ ăn uống
            KhoiTaoBangDoDaChon();
           


        }

        private void MenuItemXoa_Click(object sender, EventArgs e)
        {
            // Lấy dòng được chọn trong DataGridView
            int rowIndex = dtgvDoDaChon.SelectedCells[0].RowIndex;

            // Lấy ChiTietID từ danh sách danhSachChiTiet dựa trên rowIndex
            if (rowIndex >= 0 && rowIndex < danhSachChiTiet.Count)
            {
                ChiTietHoaDonModel chiTiet = danhSachChiTiet[rowIndex];
                int chiTietID = chiTiet.ChiTietID; // Lấy ChiTietID từ đối tượng ChiTietHoaDonModel
                Console.WriteLine(chiTietID);

                // Xác nhận trước khi xóa
                var result = MessageBox.Show("Bạn có chắc chắn muốn xóa món này?", "Xác nhận", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    // Khởi tạo controller để gọi hàm xóa
                    ChiTietHoaDonController cthdController = new ChiTietHoaDonController();

                    // Gọi hàm xóa món khỏi ChiTietHoaDon
                    bool isSuccess = cthdController.XoaMonKhoiChiTietHoaDon(chiTietID, chiTiet.HoaDonID); // Thêm cả HoaDonID vào đây

                    // Kiểm tra kết quả và thông báo
                    if (isSuccess)
                    {
                        MessageBox.Show("Đã xóa món thành công.");

                        // Xóa món khỏi danh sách chi tiết và DataGridView
                        danhSachChiTiet.RemoveAt(rowIndex);
                        dtgvDoDaChon.Rows.RemoveAt(rowIndex);

                        // Cập nhật lại tổng tiền của hóa đơn
                        HoaDonController hoaDonController = new HoaDonController();
                        decimal tongTien = hoaDonController.LayTongTienHoaDon(chiTiet.HoaDonID);
                        labTongTien.Text = tongTien.ToString("N0"); // Hiển thị tổng tiền dưới dạng số nguyên với dấu phân cách
                    }
                    else
                    {
                        MessageBox.Show("Có lỗi xảy ra khi xóa món.");
                    }
                }
            }
            else
            {
                MessageBox.Show("Dòng được chọn không hợp lệ.");
            }
        }
        private void MoFormThemMon(string tenDoAn, decimal gia)
        {
            // Tạo form và truyền dữ liệu món ăn
            FormThemDoUongVaoHoaDon form = new FormThemDoUongVaoHoaDon(tenDoAn, gia);

            // Hiển thị form dưới dạng dialog (modal)
            form.ShowDialog();
        }

        private void HienThiDoAnUong()
        {
            try
            {
                DoAnUongController controllerDoAn = new DoAnUongController();
                dsDoAnUong = controllerDoAn.HienThiDanhSachDoAnUong(); // Lấy danh sách đồ ăn uống

                // Làm sạch Panel trước khi thêm các nút đồ ăn uống
                pnlDoAnUong.Controls.Clear();

                int x = 10; // Vị trí X ban đầu
                int y = 10; // Vị trí Y ban đầu
                int buttonWidth = 120; // Chiều rộng của nút
                int buttonHeight = 120; // Chiều cao của nút
                int margin = 10; // Khoảng cách giữa các nút

                foreach (DoAnUongModel doAnUong in dsDoAnUong)
                {
                    // Tạo nút mới cho mỗi món ăn
                    Button btnDoAn = new Button();
                    btnDoAn.Text = $"{doAnUong.TenDoAnUong}\n{(int)doAnUong.Gia:N0} VND"; // Hiển thị tên và giá
                    btnDoAn.Size = new Size(buttonWidth, buttonHeight);
                    btnDoAn.Location = new Point(x, y);
                    btnDoAn.BackColor = Color.White;
                    btnDoAn.TextAlign = ContentAlignment.MiddleCenter;

                    // Gắn sự kiện click
                    btnDoAn.Click += (s, e) =>
                    {
                        // Kiểm tra nếu bàn chưa được chọn
                        if (selectedBanID == 0) // Giả sử selectedBanID là biến lưu thông tin bàn đã chọn
                        {
                            // Hiển thị thông báo yêu cầu người dùng chọn bàn
                            MessageBox.Show("Hãy chọn bàn trước khi chọn món!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            // Nếu đã chọn bàn, tiến hành chọn món
                            DoAnID = doAnUong.DoAnUongID;
                            MoFormThemMon(doAnUong.TenDoAnUong, doAnUong.Gia);
                        }
                    };



                    // Thêm nút vào Panel
                    pnlDoAnUong.Controls.Add(btnDoAn);

                    // Tính toán vị trí của nút tiếp theo
                    x += buttonWidth + margin;
                    if (x + buttonWidth > pnlDoAnUong.Width)
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

        private void HienThiDoAnUongTheoDanhMuc(List<DoAnUongModel> dsDoAnTheoDanhMuc)
        {
            try
            {
                // Làm sạch Panel trước khi hiển thị lại
                pnlDoAnUong.Controls.Clear();

                int x = 10; // Vị trí X ban đầu
                int y = 10; // Vị trí Y ban đầu
                int buttonWidth = 120; // Chiều rộng của nút
                int buttonHeight = 120; // Chiều cao của nút
                int margin = 10; // Khoảng cách giữa các nút

                foreach (DoAnUongModel doAnUong in dsDoAnTheoDanhMuc)
                {
                    // Tạo nút mới cho mỗi món ăn
                    Button btnDoAn = new Button
                    {
                        Text = $"{doAnUong.TenDoAnUong}\n{(int)doAnUong.Gia:N0} VND", // Hiển thị tên và giá
                        Size = new Size(buttonWidth, buttonHeight),
                        Location = new Point(x, y),
                        BackColor = Color.White,
                        TextAlign = ContentAlignment.MiddleCenter
                    };

                    // Gắn sự kiện click
                    btnDoAn.Click += (s, e) =>
                    {
                        // Kiểm tra nếu bàn chưa được chọn
                        if (selectedBanID == 0) // Giả sử selectedBanID là biến lưu thông tin bàn đã chọn
                        {
                            // Hiển thị thông báo yêu cầu người dùng chọn bàn
                            MessageBox.Show("Hãy chọn bàn trước khi chọn món!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            // Nếu đã chọn bàn, tiến hành chọn món
                            DoAnID = doAnUong.DoAnUongID;
                            MoFormThemMon(doAnUong.TenDoAnUong, doAnUong.Gia);
                        }
                    };

                    // Thêm nút vào Panel
                    pnlDoAnUong.Controls.Add(btnDoAn);

                    // Tính toán vị trí của nút tiếp theo
                    x += buttonWidth + margin;
                    if (x + buttonWidth > pnlDoAnUong.Width)
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

        public  void HienThiDanhMuc()
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

        // Biến toàn cục lưu BanID khi click vào bàn
       
        public void HienThiBan()
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

                    // Thêm sự kiện click để lưu BanID vào biến
                    btnBan.Click += (s, e) =>
                    {
                        // Lưu BanID vào biến selectedBanID
                        selectedBanID = ban.BanID;  // Giả sử BanModel có thuộc tính BanID
                        Console.WriteLine("Bàn nè ae:"+selectedBanID);
                        // Hiển thị thông tin bàn trong TextBox
                        txtBanDangChon.Text = ban.SoBan.ToString();
                         HienThiChiTietHoaDonTheoBan();
                        LayHoaDonID();
                        HienThiTongTien();
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
        //public void LayChiTietHoaDon()
        //{
        //    ChiTietHoaDonController controller = new ChiTietHoaDonController();
        //    if(hoaDonID!=0)
        //    {
        //        ChiTietID = controller.LayChiTietIDTheoHoaDon(hoaDonID);
        //    }
        //}
        public void LayHoaDonID()
        {
            HoaDonController hoadonCtrl = new HoaDonController();
            if (hoadonCtrl.KiemTraBanCoHoaDon(selectedBanID))
            {
                hoaDonID = hoadonCtrl.LayHoaDonIDTheoBan(selectedBanID);
                Console.WriteLine("Hoa don id:"+hoaDonID);
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
                    // Lưu BanID vào biến selectedBanID
                    selectedBanID = ban.BanID;  // Giả sử BanModel có thuộc tính BanID
                   // Console.WriteLine(selectedBanID);
                    // Hiển thị thông tin bàn trong TextBox
                    txtBanDangChon.Text = ban.SoBan.ToString();
                    HienThiChiTietHoaDonTheoBan();
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
                DoAnUongController controllerDoAn = new DoAnUongController();
                List<DoAnUongModel> dsDoAnTheoDanhMuc = controllerDoAn.HienThiDanhSachDoAnUongTheoDanhMuc(danhMucID);

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

        //hiển thị món ăn đã chọn 
        private void KhoiTaoBangDoDaChon()
        {
            // Xóa hết các cột và dòng hiện tại
            dtgvDoDaChon.Columns.Clear();
            dtgvDoDaChon.Rows.Clear();

            // Cột Đồ ăn uống
            DataGridViewTextBoxColumn colDoAnUong = new DataGridViewTextBoxColumn
            {
                HeaderText = "Tên món",
                Name = "DoAnUong",
                ReadOnly = false,
                Width = 200
            };
            dtgvDoDaChon.Columns.Add(colDoAnUong);

            // Cột Số lượng
            DataGridViewTextBoxColumn colSoLuong = new DataGridViewTextBoxColumn
            {
                HeaderText = "Số lượng",
                Name = "SoLuong",
                ReadOnly = false,
                Width = 20
            };
            dtgvDoDaChon.Columns.Add(colSoLuong);

            // Cột Giá tiền
            DataGridViewTextBoxColumn colGiaTien = new DataGridViewTextBoxColumn
            {
                HeaderText = "Giá tiền",
                Name = "GiaTien",
                ReadOnly = false,
                Width = 170
            };
            dtgvDoDaChon.Columns.Add(colGiaTien);

            // Cột Thành tiền
            DataGridViewTextBoxColumn colThanhTien = new DataGridViewTextBoxColumn
            {
                HeaderText = "Thành tiền",
                Name = "ThanhTien",
                ReadOnly = true, // Không cho phép chỉnh sửa vì giá trị này tính tự động
                Width = 170
            };
            dtgvDoDaChon.Columns.Add(colThanhTien);

            // Thiết lập DataGridView
            dtgvDoDaChon.AllowUserToAddRows = false; // Không cho phép thêm dòng mới khi không có dữ liệu
            dtgvDoDaChon.AllowUserToDeleteRows = false; // Không cho phép xóa dòng
            dtgvDoDaChon.AllowUserToOrderColumns = true; // Cho phép sắp xếp các cột
            dtgvDoDaChon.SelectionMode = DataGridViewSelectionMode.FullRowSelect; // Chọn toàn bộ dòng khi click
            dtgvDoDaChon.MultiSelect = false; // Không cho phép chọn nhiều dòng
            dtgvDoDaChon.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; // Tự động điều chỉnh kích thước cột
        }

        public void HienThiChiTietHoaDonTheoBan()
        {
            ChiTietHoaDonController controller = new ChiTietHoaDonController();
            danhSachChiTiet = controller.LayChiTietHoaDonTheoBan(selectedBanID);

            dtgvDoDaChon.Rows.Clear(); // Xóa dữ liệu cũ

            foreach (var chiTiet in danhSachChiTiet)
            {
                dtgvDoDaChon.Rows.Add(
                    chiTiet.TenDoAnUong,
                    chiTiet.SoLuong,
                    chiTiet.Gia.ToString("N0") + " VND", // Định dạng giá
                    (chiTiet.SoLuong * chiTiet.Gia).ToString("N0") + " VND" // Định dạng thành tiền
                );
            }
            HienThiTongTien();
        }



        private void btnThemDoDaChon_Click(object sender, EventArgs e)
        {
            
        }

        private void dtgvDoDaChon_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                // Lấy vị trí của chuột trên DataGridView
                var hitTest = dtgvDoDaChon.HitTest(e.X, e.Y);

                // Nếu click vào một dòng hợp lệ
                if (hitTest.RowIndex >= 0)
                {
                    // Chọn dòng hiện tại
                    dtgvDoDaChon.ClearSelection();
                    dtgvDoDaChon.Rows[hitTest.RowIndex].Selected = true;

                    // Hiển thị ContextMenuStrip
                    contextMenu.Show(dtgvDoDaChon, new Point(e.X, e.Y));
                }
            }
        }

        private void btnThanhToan_Click(object sender, EventArgs e)
        {
            try
            {
                if (FormMain.selectedBanID == 0)
                {
                    MessageBox.Show("Vui lòng chọn bàn trước khi thanh toán.");
                    return;
                }

                DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn thanh toán bàn này?", "Xác nhận thanh toán", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    HoaDonController hoaDonController = new HoaDonController();

                    // Lấy idHoaDon của bàn đang chọn
                    int hoaDonID = hoaDonController.LayHoaDonIDTheoBan(FormMain.selectedBanID);

                    if (hoaDonID > 0)
                    {
                        bool isSuccess = hoaDonController.ThanhToanHoaDon(hoaDonID);

                        if (isSuccess)
                        {
                            MessageBox.Show("Thanh toán thành công.");

                            // Cập nhật trạng thái bàn
                            HienThiBan();
                            HienThiChiTietHoaDonTheoBan();

                            // Làm trống các giá trị trong labTongTien, txtKhachTra và labTienThua
                            labTongTien.Text = "0";
                            txtKhachTra.Text = "";
                            labTienThua.Text = "0";
                        }
                        else
                        {
                            MessageBox.Show("Có lỗi xảy ra khi thanh toán.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy hóa đơn cho bàn này.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        // tính tiền
        private void HienThiTongTien()
        {
            HoaDonController hoaDonController = new HoaDonController();

            // Lấy idHoaDon của bàn đang chọn
            int hoaDonID = hoaDonController.LayHoaDonIDTheoBan(FormMain.selectedBanID);

            if (hoaDonID > 0)
            {
                decimal tongTien = hoaDonController.LayTongTienHoaDon(hoaDonID);
                labTongTien.Text = tongTien.ToString("N0") + " VND"; // Hiển thị tổng tiền dưới dạng số nguyên với dấu phân cách và đơn vị VND
            }
            else
            {
                labTongTien.Text = "0 VND"; // Nếu không có hóa đơn, hiển thị 0
            }
        }


        private void txtKhachTra_TextChanged(object sender, EventArgs e)
        {
            decimal tongTien = 0;
            decimal khachTra = 0;

            // Lấy tổng tiền từ labTongTien
            if (!decimal.TryParse(labTongTien.Text, out tongTien))
            {
                tongTien = 0;
            }

            // Lấy tiền khách trả từ txtKhachTra
            if (!decimal.TryParse(txtKhachTra.Text, out khachTra))
            {
                khachTra = 0;
            }

            // Tính tiền thừa
            decimal tienThua = khachTra - tongTien;

            // Hiển thị tiền thừa
            labTienThua.Text = tienThua.ToString("N0"); // Hiển thị tiền thừa dưới dạng số nguyên với dấu phân cách
        }

    }
}


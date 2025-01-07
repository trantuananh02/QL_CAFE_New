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
                        // Cập nhật lại giao diện DataGridView sau khi xóa món
                        danhSachChiTiet.RemoveAt(rowIndex);
                        dtgvDoDaChon.Rows.RemoveAt(rowIndex); // Xóa dòng đã xóa từ DataGridView
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
                    btnDoAn.Text = $"{doAnUong.TenDoAnUong}\n{doAnUong.Gia} VND"; // Hiển thị tên và giá
                    btnDoAn.Size = new Size(buttonWidth, buttonHeight);
                    btnDoAn.Location = new Point(x, y);
                    btnDoAn.BackColor = Color.White;
                    btnDoAn.TextAlign = ContentAlignment.MiddleCenter;

                    // Gắn sự kiện click
                    btnDoAn.Click += (s, e) =>
                    {
                        DoAnID = doAnUong.DoAnUongID;
                        MoFormThemMon(doAnUong.TenDoAnUong, doAnUong.Gia);
                        Console.WriteLine(DoAnID);
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

        
        private void MoFormThemMon(string tenDoAn, decimal gia)
        {
            // Tạo form và truyền dữ liệu món ăn
            FormThemDoUongVaoHoaDon form = new FormThemDoUongVaoHoaDon(tenDoAn, gia);

            // Hiển thị form dưới dạng dialog (modal)
            form.ShowDialog();
        }

        private void HienThiDoAnUongTheoDanhMuc(List<DoAnUongModel> dsDoAnTheoDanhMuc)
        {
            try
            {
                pnlDoAnUong.Controls.Clear(); // Làm sạch Panel trước khi hiển thị lại

                int x = 10; // Vị trí X ban đầu
                int y = 10; // Vị trí Y ban đầu
                int itemWidth = 120; // Chiều rộng của item (chỉ có tên món ăn)
                int itemHeight = 120; // Chiều cao của item
                int margin = 10; // Khoảng cách giữa các món ăn

                foreach (DoAnUongModel doAnUong in dsDoAnTheoDanhMuc)
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
                    Label lblGiaDoAn = new Label();
                    lblGiaDoAn.Text = $"{doAnUong.Gia}"; // Hiển thị giá
                    lblGiaDoAn.Location = new Point(5, 30);
                    lblGiaDoAn.Size = new Size(itemWidth - 10, 20);
                    lblGiaDoAn.TextAlign = ContentAlignment.MiddleCenter;

                    // Gắn sự kiện click

                    panelItem.Click += (s, e) => MoFormThemMon(doAnUong.TenDoAnUong, doAnUong.Gia);
                    lblTenDoAn.Click += (s, e) => MoFormThemMon(doAnUong.TenDoAnUong, doAnUong.Gia);
                    lblGiaDoAn.Click += (s, e) => MoFormThemMon(doAnUong.TenDoAnUong, doAnUong.Gia);

                    panelItem.Controls.Add(lblTenDoAn);
                    panelItem.Controls.Add(lblGiaDoAn);
                    pnlDoAnUong.Controls.Add(panelItem);

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

                    // Thêm sự kiện click để lưu BanID vào biến
                    btnBan.Click += (s, e) =>
                    {
                        // Lưu BanID vào biến selectedBanID
                        selectedBanID = ban.BanID;  // Giả sử BanModel có thuộc tính BanID
                        //Console.WriteLine(selectedBanID);
                        // Hiển thị thông tin bàn trong TextBox
                        txtBanDangChon.Text = ban.SoBan.ToString();
                         HienThiChiTietHoaDonTheoBan();
                        LayHoaDonID();
                        
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
           // Console.WriteLine(selectedBanID);
            ChiTietHoaDonController controller = new ChiTietHoaDonController();
          danhSachChiTiet = controller.LayChiTietHoaDonTheoBan(selectedBanID);

            dtgvDoDaChon.Rows.Clear(); // Xóa dữ liệu cũ

            foreach (var chiTiet in danhSachChiTiet)
            {
                dtgvDoDaChon.Rows.Add(
                    chiTiet.TenDoAnUong,
                    chiTiet.SoLuong,
                    chiTiet.Gia,
                    chiTiet.SoLuong * chiTiet.Gia // Tính thành tiền
                );
            }
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
    }
}


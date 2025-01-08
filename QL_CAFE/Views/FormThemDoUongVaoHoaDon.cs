using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.Utils.Gesture;
using QL_CAFE.Controllers;

namespace QL_CAFE.Views
{
    public partial class FormThemDoUongVaoHoaDon : Form
    {
        public FormThemDoUongVaoHoaDon(string tenDoAn, decimal gia)
        {
            InitializeComponent();

            // Gán giá trị cho các label hoặc textbox trên form
            labTenMon.Text = tenDoAn; // Giả sử bạn có một label hiển thị tên đồ ăn
            labGia.Text = $"{gia:N0} VND"; // Hiển thị giá tiền (định dạng tiền tệ)
        }

        private void btnTru_Click(object sender, EventArgs e)
        {
            if (numericUpDown1.Value > numericUpDown1.Minimum)
            {
                numericUpDown1.Value -= 1;
            }
        }

        private void btnCong_Click(object sender, EventArgs e)
        {
            numericUpDown1.Value++;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra xem bàn có hóa đơn chưa
                HoaDonController hoaDonController = new HoaDonController();

                // Lấy số lượng từ numericUpDown
                int sl = (int)numericUpDown1.Value;
                string nhanVienID = "NV001"; // Luôn sử dụng NV001

                // Lấy idHoaDon của bàn mới
                int newHoaDonID = hoaDonController.LayHoaDonIDTheoBan(FormMain.selectedBanID);

                // Nếu chưa có hóa đơn, tạo mới hóa đơn
                if (newHoaDonID == 0)
                {
                    newHoaDonID = hoaDonController.TaoMoiHoaDon(FormMain.selectedBanID, nhanVienID);
                }

                // Thêm món vào chi tiết hóa đơn
                Console.WriteLine(FormMain.selectedBanID + " " + FormMain.DoAnID + " " + sl + " " + nhanVienID + " " + newHoaDonID);
                ChiTietHoaDonController cthdController = new ChiTietHoaDonController();
                bool isSuccess = cthdController.ThemMonVaoHoaDon(FormMain.selectedBanID, FormMain.DoAnID, sl, nhanVienID, newHoaDonID);

                if (isSuccess)
                {
                    MessageBox.Show("Món đã được thêm vào chi tiết hóa đơn thành công.");

                    // Quay lại FormMain và gọi hàm HienThiChiTietHoaDonTheoBan
                    this.Close(); // Đóng form hiện tại

                    // Lấy instance của FormMain từ danh sách các form đã mở
                    FormMain formMain = (FormMain)Application.OpenForms["FormMain"];

                    if (formMain != null)
                    {
                        // Gọi hàm HienThiChiTietHoaDonTheoBan() trong FormMain
                        formMain.HienThiChiTietHoaDonTheoBan();
                        formMain.HienThiBan();
                        formMain.Show(); // Hiển thị lại FormMain nếu nó đã bị ẩn
                    }
                }
                else
                {
                    MessageBox.Show("Có lỗi xảy ra khi thêm món vào chi tiết hóa đơn.");
                }
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

    }

}

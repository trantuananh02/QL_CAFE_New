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
            labGia.Text = gia.ToString("C"); // Hiển thị giá tiền (định dạng tiền tệ)
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

                // Thêm món vào chi tiết hóa đơn
                ChiTietHoaDonController cthdController = new ChiTietHoaDonController();
                bool isSuccess = cthdController.ThemMonVaoChiTietHoaDon(FormMain.hoaDonID, FormMain.DoAnID, sl);

                if (isSuccess)
                {
                    MessageBox.Show("Món đã được thêm vào chi tiết hóa đơn thành công.");
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

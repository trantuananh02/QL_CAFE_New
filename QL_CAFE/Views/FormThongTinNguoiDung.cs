using QL_CAFE.Controllers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Collections.Specialized.BitVector32;

namespace QL_CAFE.Views
{
    public partial class FormThongTinNguoiDung : Form
    {
        public FormThongTinNguoiDung()
        {
            InitializeComponent();
        }

        private void FormThongTinNguoiDung_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Session.TenTaiKhoan))
            {
                ThongTinNguoiDungController controller = new ThongTinNguoiDungController();

                // Gọi hàm lấy thông tin người dùng
                controller.LayThongTinNguoiDung(
                    Session.TenTaiKhoan,
                    txtHoTen,
                    txtSoDienThoai,
                    txtDiaChi,
                    txtNgaySinh,
                    txtTenTaiKhoan
                );
            }
            else
            {
                MessageBox.Show("Tên tài khoản không tồn tại. Vui lòng đăng nhập lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Session.TenTaiKhoan))
            {
                ThongTinNguoiDungController controller = new ThongTinNguoiDungController();

                // Gọi hàm cập nhật thông tin người dùng
                controller.CapNhatThongTinNguoiDung(
                    Session.TenTaiKhoan,
                    txtHoTen,
                    txtSoDienThoai,
                    txtDiaChi,
                    txtNgaySinh
                );
            }
            else
            {
                MessageBox.Show("Tên tài khoản không tồn tại. Vui lòng đăng nhập lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

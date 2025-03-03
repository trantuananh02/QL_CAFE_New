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

namespace QL_CAFE.Views
{
    public partial class FormQuenMatKhau : Form
    {
        // Khai báo đối tượng của controller ThongTinNguoiDungController
        private ThongTinNguoiDungController controller;
        public FormQuenMatKhau()
        {
            InitializeComponent();
            controller = new ThongTinNguoiDungController();  // Khởi tạo controller
        }

        private void FormQuenMatKhau_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Session.TenTaiKhoan))
            {
                ThongTinNguoiDungController controller = new ThongTinNguoiDungController();

                // Gọi hàm chỉ lấy tên tài khoản và hiển thị vào txtTenTaiKhoan
                controller.LayTenTaiKhoan(Session.TenTaiKhoan, txtTenTaiKhoan);
            }
            else
            {
                MessageBox.Show("Tên tài khoản không tồn tại. Vui lòng đăng nhập lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Kiểm tra các trường thông tin đầu vào (ví dụ: kiểm tra các ô nhập mật khẩu có trống hay không)
            if (string.IsNullOrEmpty(txtMatKhauCu.Text) || string.IsNullOrEmpty(txtMatKhauMoi.Text) || string.IsNullOrEmpty(txtNhapLaiMatKhau.Text))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Gọi hàm đổi mật khẩu từ controller
            controller.DoiMatKhau(Session.TenTaiKhoan, txtMatKhauCu, txtMatKhauMoi, txtNhapLaiMatKhau);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

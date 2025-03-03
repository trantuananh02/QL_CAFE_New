using DevExpress.XtraWaitForm;
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

namespace QL_CAFE.Views
{
    public partial class DangNhap : Form
    {
        private bool isPasswordVisible = false;
        public static string username; // Biến static lưu tên tài khoản

       

        public DangNhap()
        {
            InitializeComponent();
            txtMatKhau.UseSystemPasswordChar = true;
            pictureBox4.Image = Properties.Resources.eye;

            // Đặt giá trị mặc định
            txtTenTaiKhoan.Text = "Nhập tên tài khoản";
            txtTenTaiKhoan.ForeColor = Color.Gray;

            txtMatKhau.Text = "Nhập mật khẩu";
            txtMatKhau.ForeColor = Color.Gray;

           
        }

        private void button1_Click(object sender, EventArgs e)
        {

            string tenTaiKhoan = txtTenTaiKhoan.Text.Trim();
            string matKhau = txtMatKhau.Text;

            // Kiểm tra nếu vẫn còn placeholder thì báo lỗi
            if (tenTaiKhoan == "Nhập tên tài khoản" || string.IsNullOrEmpty(tenTaiKhoan))
            {
                MessageBox.Show("Vui lòng nhập tên tài khoản!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenTaiKhoan.Focus();
                return;
            }
            if (matKhau == "Nhập mật khẩu" || string.IsNullOrEmpty(matKhau))
            {
                MessageBox.Show("Vui lòng nhập mật khẩu!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMatKhau.Focus();
                return;
            }

            // Kiểm tra đăng nhập (Thay thế bằng truy vấn CSDL thực tế)
            if (tenTaiKhoan == "admin" && matKhau == "123")
            {
                username = tenTaiKhoan; // Lưu tài khoản đăng nhập
                Console.WriteLine("uẻname ne hihi:",username);
                MessageBox.Show("Đăng nhập thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.Hide();
                using (FormMain mainForm = new FormMain())
                {
                    mainForm.ShowDialog();
                }
                this.Close();
            }
            else
            {
                MessageBox.Show("Tên tài khoản hoặc mật khẩu không đúng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        
        private void pictureBox4_Click(object sender, EventArgs e)
        {// Đổi trạng thái hiển thị mật khẩu
            isPasswordVisible = !isPasswordVisible;

            txtMatKhau.UseSystemPasswordChar = !isPasswordVisible;
            pictureBox4.Image = isPasswordVisible ? Properties.Resources.hidden : Properties.Resources.eye;
        
        }
    

        private void txtTenTaiKhoan_MouseClick(object sender, MouseEventArgs e)
        {
            if (txtTenTaiKhoan.Text == "Nhập tên tài khoản")
            {
                txtTenTaiKhoan.Text = "";
                txtTenTaiKhoan.ForeColor = Color.Black;
            }

        }

        private void txtMatKhau_MouseClick(object sender, MouseEventArgs e)
        {
            if (txtMatKhau.Text == "Nhập mật khẩu")
            {
                txtMatKhau.Text = "";
                txtMatKhau.ForeColor = Color.Black;
                txtMatKhau.UseSystemPasswordChar = !isPasswordVisible;
            }

        }

        private void txtMatKhau_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMatKhau.Text))
            {
                txtMatKhau.ForeColor = Color.Gray;
                txtMatKhau.Text = "Nhập mật khẩu";
                txtMatKhau.UseSystemPasswordChar = false;
            }
            
        }

        private void txtTenTaiKhoan_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTenTaiKhoan.Text))
            {
                txtTenTaiKhoan.ForeColor = Color.Gray;
                txtTenTaiKhoan.Text = "Nhập tên tài khoản";
            }

        }
    }
}

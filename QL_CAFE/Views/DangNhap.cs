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
       
        public DangNhap()
        {
            InitializeComponent();
            txtMatKhau.UseSystemPasswordChar = true;
            pictureBox4.Image = Properties.Resources.eye; 
        }
        
    private void label3_Click(object sender, EventArgs e)
        {
            FormQuenMatKhau form=new FormQuenMatKhau();
            form.ShowDialog();
            this.Hide();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            try
            {
                // Check if the username and password fields are empty or have placeholder text
                if (txtTenTaiKhoan.Text == "Nhập tên tài khoản" || string.IsNullOrEmpty(txtTenTaiKhoan.Text))
                {
                    MessageBox.Show("Vui lòng nhập tên tài khoản.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (txtMatKhau.Text == "Nhập mật khẩu" || string.IsNullOrEmpty(txtMatKhau.Text))
                {
                    MessageBox.Show("Vui lòng nhập mật khẩu.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Create an instance of DangNhapController and get the values from the textboxes
                DangNhapController controller = new DangNhapController();
                string tentk = txtTenTaiKhoan.Text;
                string pass = txtMatKhau.Text;

                // Call the DangNhap method in the controller to check login
                bool isLoggedIn = controller.DangNhap(tentk, pass);

                // Check login result
                if (!isLoggedIn)
                {
                    MessageBox.Show("Tên tài khoản hoặc mật khẩu không đúng. Vui lòng thử lại.", "Lỗi đăng nhập", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("Đăng nhập thành công!", "Chào mừng", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    FormMain main= new FormMain();
                    main.Show();
                    this.Hide(); // Hide the current login form
                }
            }
            catch (Exception ex)
            {
                // Catch any exceptions and show an error message
                MessageBox.Show("Đã xảy ra lỗi khi đăng nhập: " + ex.Message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtMatKhau_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void txtDangNhap_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        
        private void pictureBox4_Click(object sender, EventArgs e)
        {// Đổi trạng thái hiển thị mật khẩu
            isPasswordVisible = !isPasswordVisible;

            if (isPasswordVisible)
            {
                // Hiện mật khẩu
                txtMatKhau.UseSystemPasswordChar = false;

                // Đổi hình ảnh sang mắt mở
                pictureBox4.Image = Properties.Resources.hidden;
            }
            else
            {
                // Ẩn mật khẩu
                txtMatKhau.UseSystemPasswordChar = true;

                // Đổi hình ảnh sang mắt đóng
                pictureBox4.Image = Properties.Resources.eye;
            }
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
            {
                txtMatKhau.Text = "";
                txtMatKhau.ForeColor = Color.Black;

            }
                
        }

        private void txtMatKhau_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMatKhau.Text))
            {
                txtMatKhau.ForeColor = Color.LightGray;
                txtMatKhau.Text = "Nhập mật khẩu";  // Giá trị mặc định
                
            }
        }

        private void txtTenTaiKhoan_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTenTaiKhoan.Text))
            {
                txtTenTaiKhoan.ForeColor = Color.LightGray;
                txtTenTaiKhoan.Text = "Nhập tên tài khoản";  // Giá trị mặc định
            }

        }
    }
}

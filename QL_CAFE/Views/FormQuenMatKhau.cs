using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QL_CAFE.Controllers;
using System.Data.SqlClient;

namespace QL_CAFE.Views
{
    public partial class FormQuenMatKhau : Form
    {
        private string verificationCode; // Lưu trữ mã xác nhận
        private string userEmail; // Lưu trữ email người dùng
        public FormQuenMatKhau()
        {
            InitializeComponent();
        }

        private void btnGuiMa_Click(object sender, EventArgs e)
        {
            userEmail = txtEmail.Text.Trim();
            if (string.IsNullOrEmpty(userEmail))
            {
                MessageBox.Show("Vui lòng nhập email!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Kiểm tra email có tồn tại không
            DangNhapController dangNhapController = new DangNhapController();
            if (!dangNhapController.KiemTraEmail(userEmail))
            {
                MessageBox.Show("Email không tồn tại trong hệ thống!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Tạo mã xác nhận ngẫu nhiên
                Random random = new Random();
                verificationCode = random.Next(100000, 999999).ToString();

                // Cấu hình SMTP
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("your-email@gmail.com", "your-email-password"),
                    EnableSsl = true
                };

                // Tạo email
                MailMessage mail = new MailMessage
                {
                    From = new MailAddress("laylaimatkhauquancafe@gmail.com"),
                    Subject = "Mã xác nhận quên mật khẩu",
                    Body = $"Mã xác nhận của bạn là: {verificationCode}",
                    IsBodyHtml = false
                };
                mail.To.Add(userEmail);

                // Gửi email
                smtpClient.Send(mail);
                MessageBox.Show("Mã xác nhận đã được gửi tới email!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi gửi email: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

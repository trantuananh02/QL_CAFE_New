using QL_CAFE.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace QL_CAFE
{
    public partial class FormTrangCho : Form
    {
        public FormTrangCho()
        {
            InitializeComponent();
            Load();
        }
        private void Load()
        {
            // Khởi tạo Timer để cập nhật ProgressBar
            Timer timer = new Timer();
            timer.Interval = 50; // Tốc độ tăng (mỗi 50ms)
            timer.Tick += (s, args) =>
            {
                if (prgLoad.Value < 100)
                {
                    prgLoad.Value += 2; // Tăng giá trị ProgressBar
                }
                else
                {
                    timer.Stop(); // Dừng Timer khi đạt 100
                    this.Hide(); // Ẩn form hiện tại
                    DangNhap formDangNhap = new DangNhap();
                    formDangNhap.Show(); // Hiển thị Form đăng nhập
                }
            };

            timer.Start(); // Bắt đầu Timer
        
    }
        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}

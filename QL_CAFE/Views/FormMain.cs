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
using QL_CAFE.Models;

namespace QL_CAFE.Views
{
    public partial class FormMain : Form
    {
        List<string> dskv = new List<string>(); // Giữ nguyên kiểu List<string>

        public FormMain()
        {
            InitializeComponent();
            HienThiKhuVuc();
        }

        private void HienThiKhuVuc()
        {
            try
            {
                KhuVucController controllerkv = new KhuVucController();
                dskv = controllerkv.HienThiDanhSachKhuVuc(); // Vẫn giữ kiểu string trong danh sách

                // Đảm bảo ComboBox được làm mới trước khi thêm dữ liệu mới
                cbKhuVuc.Items.Clear();

                // Duyệt qua danh sách khu vực và thêm vào ComboBox
                foreach (string tenKhuVuc in dskv)
                {
                    Console.WriteLine(tenKhuVuc);
                    cbKhuVuc.Items.Add(tenKhuVuc); // Thêm tên khu vực vào ComboBox
                }

                // Nếu cần, chọn mục đầu tiên làm mặc định (tuỳ chọn)
                if (cbKhuVuc.Items.Count > 0)
                {
                    cbKhuVuc.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                // Hiển thị thông báo lỗi nếu có
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void quảnLýNhânViênToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormQuanLyNhanVien form=new FormQuanLyNhanVien();
            form.ShowDialog();
            this.Hide();
        }
    }
}


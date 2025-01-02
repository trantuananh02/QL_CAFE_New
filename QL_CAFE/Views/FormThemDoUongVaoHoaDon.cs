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

namespace QL_CAFE.Views
{
    public partial class FormThemDoUongVaoHoaDon : Form
    {
        public FormThemDoUongVaoHoaDon()
        {
            InitializeComponent();
        }

        // Phương thức cập nhật tên món ăn
        public void CapNhatTenMon(string tenMon, decimal gia)
        {
            labTenMon.Text = tenMon; // Cập nhật tên món vào Label
            labGia.Text = gia.ToString(); // Cập nhật tên món vào Label
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
            if (numericUpDown1.Value < numericUpDown1.Maximum)
            {
                numericUpDown1.Value += 1;
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {

        }
    }
}

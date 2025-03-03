using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QL_CAFE.Models;
using QL_CAFE.Controllers;

namespace QL_CAFE.Views
{
    public partial class FormQuanLyDoanhThu : Form
    {

        private DoanhThuController doanhThuController;
        public FormQuanLyDoanhThu()
        {
            InitializeComponent();
            doanhThuController = new DoanhThuController();
            InitDataGridView();
        }
        private void InitDataGridView()
        {
            dtgvDoanhThu.ColumnCount = 6;
            dtgvDoanhThu.Columns[0].Name = "Mã Hóa Đơn";
            dtgvDoanhThu.Columns[1].Name = "Ngày Tạo";
            dtgvDoanhThu.Columns[2].Name = "Tổng Tiền";
            dtgvDoanhThu.Columns[3].Name = "Trạng Thái";
            dtgvDoanhThu.Columns[4].Name = "Nhân Viên";
            dtgvDoanhThu.Columns[5].Name = "Số Bàn";

            dtgvDoanhThu.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void TinhTongTien()
        {
            decimal tongTien = 0;

            foreach (DataGridViewRow row in dtgvDoanhThu.Rows)
            {
                if (row.Cells[2].Value != null) // Kiểm tra giá trị không null
                {
                    decimal tien;
                    if (decimal.TryParse(row.Cells[2].Value.ToString(), out tien)) // Chuyển đổi thành số
                    {
                        tongTien += tien;
                    }
                }
            }

            labTongTien.Text = $"Tổng Tiền: {tongTien:N0} VND"; // Hiển thị với định dạng tiền tệ
        }

        private void btnThongKe_Click(object sender, EventArgs e)
        {
            DateTime ngayBatDau = dtpBatDau.Value.Date;
            DateTime ngayKetThuc = dtpKetThuc.Value.Date.AddDays(1).AddSeconds(-1);

            List<DoanhThuModel> danhSachDoanhThu = doanhThuController.GetDoanhThu(ngayBatDau, ngayKetThuc);
            dtgvDoanhThu.Rows.Clear();

            foreach (var doanhThu in danhSachDoanhThu)
            {
                dtgvDoanhThu.Rows.Add(doanhThu.HoaDonID, doanhThu.NgayTao, doanhThu.TongTien, doanhThu.TrangThai, doanhThu.NhanVien, doanhThu.SoBan);
            }

            TinhTongTien(); // Cập nhật tổng tiền
        }
    }
}

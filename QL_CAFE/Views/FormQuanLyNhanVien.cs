using QL_CAFE.Controllers;
using QL_CAFE.Models;
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

namespace QL_CAFE
{
    
    public partial class FormQuanLyNhanVien : Form
    {
        List<NhanVienModel> dsnv = new List<NhanVienModel>();
        public FormQuanLyNhanVien()
        {
            InitializeComponent();
            KhoiTaoBangNhanVien();
            HienThiDanhSachNhanVien();
        }
        private void HienThiDanhSachNhanVien()
        {
            try
            {
                // Khởi tạo controller và lấy danh sách nhân viên
                NhanVienController nhanVienController = new NhanVienController();
                List<NhanVienModel> dsnv = nhanVienController.HienThiDanhSachNhanVien();

                // Xóa tất cả dòng trong bảng trước khi thêm mới
                tblNhanVien.Rows.Clear();

                // Lặp qua danh sách nhân viên và thêm vào DataGridView
                for (int i = 0; i < dsnv.Count; i++)
                {
                    NhanVienModel nv = dsnv[i];
                    int maNhanVien = nv.NhanVienID;
                    string hoTen = nv.HoTen;
                    string ngaySinh = nv.NgaySinh.ToShortDateString(); // Hiển thị chỉ ngày
                    string soDienThoai = nv.SoDienThoai;
                    string diaChi = nv.DiaChi;
                    string tenTaiKhoan = nv.TenTK;

                    // Thêm dòng vào DataGridView
                    tblNhanVien.Rows.Add(
                        i + 1, // Số thứ tự
                        maNhanVien,
                        hoTen,
                        ngaySinh, // Chỉ hiển thị ngày
                        soDienThoai,
                        diaChi,
                        tenTaiKhoan
                    );
                }
            }
            catch (Exception ex)
            {
                // Hiển thị thông báo lỗi nếu có
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }


        private void KhoiTaoBangNhanVien()

        {
            // Xóa hết các cột và dòng hiện tại
            tblNhanVien.Columns.Clear();
            tblNhanVien.Rows.Clear();

            // Cột Số TT
            DataGridViewTextBoxColumn colSTT = new DataGridViewTextBoxColumn();
            colSTT.HeaderText = "Số TT";
            colSTT.Name = "STT";
            colSTT.ReadOnly = true;  // Không cho phép chỉnh sửa số thứ tự
            colSTT.Width = 100;
            tblNhanVien.Columns.Add(colSTT);

            // Cột Mã nhân viên
            DataGridViewTextBoxColumn colMaNhanVien = new DataGridViewTextBoxColumn
            {
                HeaderText = "Mã Nhân Viên",
                Name = "MaNhanVien",
                ReadOnly = true,
                Width = 120
            };
            tblNhanVien.Columns.Add(colMaNhanVien);

            // Cột Họ và tên
            DataGridViewTextBoxColumn colHoTen = new DataGridViewTextBoxColumn
            {
                HeaderText = "Họ và Tên",
                Name = "HoTen",
                ReadOnly = true,
                Width = 200
            };
            tblNhanVien.Columns.Add(colHoTen);

            // Cột Ngày sinh
            DataGridViewTextBoxColumn colNgaySinh = new DataGridViewTextBoxColumn
            {
                HeaderText = "Ngày Sinh",
                Name = "NgaySinh",
                ReadOnly = true,
                Width = 150
            };
            tblNhanVien.Columns.Add(colNgaySinh);

            // Cột Số điện thoại
            DataGridViewTextBoxColumn colSoDienThoai = new DataGridViewTextBoxColumn
            {
                HeaderText = "Số Điện Thoại",
                Name = "SoDienThoai",
                ReadOnly = true,
                Width = 150
            };
            tblNhanVien.Columns.Add(colSoDienThoai);

            // Cột Địa chỉ
            DataGridViewTextBoxColumn colDiaChi = new DataGridViewTextBoxColumn
            {
                HeaderText = "Địa Chỉ",
                Name = "DiaChi",
                ReadOnly = true,
                Width = 250
            };
            tblNhanVien.Columns.Add(colDiaChi);

            // Cột Tên tài khoản
            DataGridViewTextBoxColumn colTenTaiKhoan = new DataGridViewTextBoxColumn
            {
                HeaderText = "Tên Tài Khoản",
                Name = "TenTaiKhoan",
                ReadOnly = true,
                Width = 150
            };
            tblNhanVien.Columns.Add(colTenTaiKhoan);

            // Thiết lập DataGridView
            tblNhanVien.AllowUserToAddRows = false;  // Không cho phép thêm dòng mới khi không có dữ liệu
            tblNhanVien.AllowUserToDeleteRows = false; // Không cho phép xóa dòng
            tblNhanVien.AllowUserToOrderColumns = true; // Cho phép sắp xếp các cột
            tblNhanVien.SelectionMode = DataGridViewSelectionMode.FullRowSelect; // Chọn toàn bộ dòng khi click
            tblNhanVien.MultiSelect = false; // Không cho phép chọn nhiều dòng
            tblNhanVien.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; // Tự động điều chỉnh kích thước cột


        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void FormQuanLyNhanVien_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
        }

        
    }
}

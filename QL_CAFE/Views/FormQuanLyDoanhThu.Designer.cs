namespace QL_CAFE.Views
{
    partial class FormQuanLyDoanhThu
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dtpBatDau = new System.Windows.Forms.DateTimePicker();
            this.dtpKetThuc = new System.Windows.Forms.DateTimePicker();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnThongKe = new System.Windows.Forms.Button();
            this.dtgvDoanhThu = new System.Windows.Forms.DataGridView();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label4 = new System.Windows.Forms.Label();
            this.labTongTien = new System.Windows.Forms.Label();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtgvDoanhThu)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // dtpBatDau
            // 
            this.dtpBatDau.Font = new System.Drawing.Font("Times New Roman", 10.2F);
            this.dtpBatDau.Location = new System.Drawing.Point(90, 114);
            this.dtpBatDau.Name = "dtpBatDau";
            this.dtpBatDau.Size = new System.Drawing.Size(278, 27);
            this.dtpBatDau.TabIndex = 0;
            // 
            // dtpKetThuc
            // 
            this.dtpKetThuc.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.dtpKetThuc.Location = new System.Drawing.Point(469, 114);
            this.dtpKetThuc.Name = "dtpKetThuc";
            this.dtpKetThuc.Size = new System.Drawing.Size(292, 27);
            this.dtpKetThuc.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.LightBlue;
            this.panel2.Controls.Add(this.pictureBox1);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Location = new System.Drawing.Point(1, 6);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1934, 86);
            this.panel2.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 22.2F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(129, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(335, 42);
            this.label1.TabIndex = 0;
            this.label1.Text = "Quản lý Doanh Thu";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Times New Roman", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label2.Location = new System.Drawing.Point(32, 115);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 26);
            this.label2.TabIndex = 4;
            this.label2.Text = "Từ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Times New Roman", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label3.Location = new System.Drawing.Point(392, 113);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(51, 26);
            this.label3.TabIndex = 5;
            this.label3.Text = "Đến";
            // 
            // btnThongKe
            // 
            this.btnThongKe.Font = new System.Drawing.Font("Times New Roman", 10.2F);
            this.btnThongKe.Location = new System.Drawing.Point(797, 109);
            this.btnThongKe.Name = "btnThongKe";
            this.btnThongKe.Size = new System.Drawing.Size(109, 37);
            this.btnThongKe.TabIndex = 6;
            this.btnThongKe.Text = "Thống kê";
            this.btnThongKe.UseVisualStyleBackColor = true;
            this.btnThongKe.Click += new System.EventHandler(this.btnThongKe_Click);
            // 
            // dtgvDoanhThu
            // 
            this.dtgvDoanhThu.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtgvDoanhThu.Location = new System.Drawing.Point(12, 152);
            this.dtgvDoanhThu.Name = "dtgvDoanhThu";
            this.dtgvDoanhThu.RowHeadersWidth = 51;
            this.dtgvDoanhThu.RowTemplate.Height = 24;
            this.dtgvDoanhThu.Size = new System.Drawing.Size(1900, 722);
            this.dtgvDoanhThu.TabIndex = 7;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::QL_CAFE.Properties.Resources.grouping;
            this.pictureBox1.Location = new System.Drawing.Point(3, 5);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(120, 73);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Times New Roman", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label4.Location = new System.Drawing.Point(1286, 907);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(137, 32);
            this.label4.TabIndex = 8;
            this.label4.Text = "Tổng tiền:";
            // 
            // labTongTien
            // 
            this.labTongTien.AutoSize = true;
            this.labTongTien.Font = new System.Drawing.Font("Times New Roman", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.labTongTien.Location = new System.Drawing.Point(1454, 907);
            this.labTongTien.Name = "labTongTien";
            this.labTongTien.Size = new System.Drawing.Size(28, 32);
            this.labTongTien.TabIndex = 9;
            this.labTongTien.Text = "0";
            // 
            // FormQuanLyDoanhThu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1924, 956);
            this.Controls.Add(this.labTongTien);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.dtgvDoanhThu);
            this.Controls.Add(this.btnThongKe);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.dtpKetThuc);
            this.Controls.Add(this.dtpBatDau);
            this.Name = "FormQuanLyDoanhThu";
            this.Text = "FormQuanLyDoanhThu";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtgvDoanhThu)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker dtpBatDau;
        private System.Windows.Forms.DateTimePicker dtpKetThuc;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnThongKe;
        private System.Windows.Forms.DataGridView dtgvDoanhThu;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label labTongTien;
    }
}
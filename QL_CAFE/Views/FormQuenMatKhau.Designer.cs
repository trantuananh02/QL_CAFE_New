namespace QL_CAFE.Views
{
    partial class FormQuenMatKhau
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.btnGuiMa = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 12F);
            this.label1.Location = new System.Drawing.Point(62, 151);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 22);
            this.label1.TabIndex = 0;
            this.label1.Text = "Nhập email";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Times New Roman", 22.2F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(206, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(343, 42);
            this.label2.TabIndex = 1;
            this.label2.Text = "QUÊN MẬT KHẨU";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.LightCyan;
            this.panel1.Controls.Add(this.btnGuiMa);
            this.panel1.Controls.Add(this.txtEmail);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(755, 306);
            this.panel1.TabIndex = 2;
            // 
            // txtEmail
            // 
            this.txtEmail.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEmail.ForeColor = System.Drawing.SystemColors.ScrollBar;
            this.txtEmail.Location = new System.Drawing.Point(181, 143);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(475, 30);
            this.txtEmail.TabIndex = 3;
            this.txtEmail.Text = "Nhập email của bạn";
            // 
            // btnGuiMa
            // 
            this.btnGuiMa.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGuiMa.Location = new System.Drawing.Point(510, 213);
            this.btnGuiMa.Name = "btnGuiMa";
            this.btnGuiMa.Size = new System.Drawing.Size(146, 60);
            this.btnGuiMa.TabIndex = 5;
            this.btnGuiMa.Text = "Gửi mã";
            this.btnGuiMa.UseVisualStyleBackColor = true;
            this.btnGuiMa.Click += new System.EventHandler(this.btnGuiMa_Click);
            // 
            // FormQuenMatKhau
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(755, 306);
            this.Controls.Add(this.panel1);
            this.Name = "FormQuenMatKhau";
            this.Text = "FormQuenMatKhau";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Button btnGuiMa;
    }
}
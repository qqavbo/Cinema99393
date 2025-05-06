namespace Cinema
{
    partial class Input
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Input));
            this.entry = new System.Windows.Forms.Button();
            this.tb_password = new System.Windows.Forms.TextBox();
            this.l_password = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.l_mail = new System.Windows.Forms.Label();
            this.tb_mail = new System.Windows.Forms.TextBox();
            this.registration = new System.Windows.Forms.Button();
            this.anonim = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // entry
            // 
            this.entry.BackColor = System.Drawing.Color.White;
            this.entry.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.entry.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.entry.Location = new System.Drawing.Point(25, 184);
            this.entry.Name = "entry";
            this.entry.Size = new System.Drawing.Size(242, 52);
            this.entry.TabIndex = 0;
            this.entry.Text = "войти в учетную запись";
            this.entry.UseVisualStyleBackColor = false;
            this.entry.UseWaitCursor = true;
            this.entry.Click += new System.EventHandler(this.entry_Click);
            // 
            // tb_password
            // 
            this.tb_password.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tb_password.Location = new System.Drawing.Point(106, 128);
            this.tb_password.Name = "tb_password";
            this.tb_password.Size = new System.Drawing.Size(263, 27);
            this.tb_password.TabIndex = 1;
            this.tb_password.UseSystemPasswordChar = true;
            this.tb_password.UseWaitCursor = true;
            // 
            // l_password
            // 
            this.l_password.AutoSize = true;
            this.l_password.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.l_password.Location = new System.Drawing.Point(21, 134);
            this.l_password.Name = "l_password";
            this.l_password.Size = new System.Drawing.Size(79, 20);
            this.l_password.TabIndex = 4;
            this.l_password.Text = "пароль: ";
            this.l_password.UseWaitCursor = true;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.checkBox1.Location = new System.Drawing.Point(390, 134);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(18, 17);
            this.checkBox1.TabIndex = 5;
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.UseWaitCursor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // l_mail
            // 
            this.l_mail.AutoSize = true;
            this.l_mail.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.l_mail.Location = new System.Drawing.Point(21, 93);
            this.l_mail.Name = "l_mail";
            this.l_mail.Size = new System.Drawing.Size(69, 20);
            this.l_mail.TabIndex = 6;
            this.l_mail.Text = "почта: ";
            this.l_mail.UseWaitCursor = true;
            // 
            // tb_mail
            // 
            this.tb_mail.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tb_mail.Location = new System.Drawing.Point(106, 86);
            this.tb_mail.Name = "tb_mail";
            this.tb_mail.Size = new System.Drawing.Size(302, 27);
            this.tb_mail.TabIndex = 7;
            this.tb_mail.UseWaitCursor = true;
            // 
            // registration
            // 
            this.registration.BackColor = System.Drawing.Color.White;
            this.registration.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.registration.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.registration.Location = new System.Drawing.Point(273, 186);
            this.registration.Name = "registration";
            this.registration.Size = new System.Drawing.Size(135, 49);
            this.registration.TabIndex = 8;
            this.registration.Text = "регистрация";
            this.registration.UseVisualStyleBackColor = false;
            this.registration.UseWaitCursor = true;
            this.registration.Click += new System.EventHandler(this.registration_Click);
            // 
            // anonim
            // 
            this.anonim.BackColor = System.Drawing.Color.White;
            this.anonim.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.anonim.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.anonim.Location = new System.Drawing.Point(25, 242);
            this.anonim.Name = "anonim";
            this.anonim.Size = new System.Drawing.Size(383, 50);
            this.anonim.TabIndex = 9;
            this.anonim.Text = "войти без учетной записи";
            this.anonim.UseVisualStyleBackColor = false;
            this.anonim.UseWaitCursor = true;
            this.anonim.Click += new System.EventHandler(this.anonim_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.MediumPurple;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(0, -15);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(448, 81);
            this.panel1.TabIndex = 15;
            this.panel1.UseWaitCursor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(186, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 25);
            this.label1.TabIndex = 13;
            this.label1.Text = "ВХОД";
            this.label1.UseWaitCursor = true;
            // 
            // Input
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(440, 313);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.anonim);
            this.Controls.Add(this.registration);
            this.Controls.Add(this.tb_mail);
            this.Controls.Add(this.l_mail);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.l_password);
            this.Controls.Add(this.tb_password);
            this.Controls.Add(this.entry);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Input";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "вход в учетную запись";
            this.UseWaitCursor = true;
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button entry;
        private System.Windows.Forms.TextBox tb_password;
        private System.Windows.Forms.Label l_password;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label l_mail;
        private System.Windows.Forms.TextBox tb_mail;
        private System.Windows.Forms.Button registration;
        private System.Windows.Forms.Button anonim;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
    }
}


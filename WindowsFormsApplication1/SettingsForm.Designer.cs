namespace WindowsFormsApplication1
{
    partial class SettingsForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbStop = new System.Windows.Forms.ComboBox();
            this.cbParity = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.tbBaudRate = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.tbDataBits = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.tbFrequenza = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tbCorrPostDx = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tbCorrAntSx = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tbCorrPostSx = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbCorrAntDx = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbLetture = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbAutoStart = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbPort = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbStop);
            this.groupBox1.Controls.Add(this.cbParity);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.tbBaudRate);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.tbDataBits);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.tbFrequenza);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.tbCorrPostDx);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.tbCorrAntSx);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.tbCorrPostSx);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.tbCorrAntDx);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.tbLetture);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(12, 71);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(263, 277);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Bilancia";
            // 
            // cbStop
            // 
            this.cbStop.FormattingEnabled = true;
            this.cbStop.Location = new System.Drawing.Point(163, 243);
            this.cbStop.Name = "cbStop";
            this.cbStop.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cbStop.Size = new System.Drawing.Size(76, 21);
            this.cbStop.TabIndex = 25;
            // 
            // cbParity
            // 
            this.cbParity.FormattingEnabled = true;
            this.cbParity.Location = new System.Drawing.Point(163, 198);
            this.cbParity.Name = "cbParity";
            this.cbParity.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cbParity.Size = new System.Drawing.Size(76, 21);
            this.cbParity.TabIndex = 24;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(18, 250);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(48, 13);
            this.label8.TabIndex = 23;
            this.label8.Text = "Stop bits";
            // 
            // tbBaudRate
            // 
            this.tbBaudRate.Location = new System.Drawing.Point(163, 176);
            this.tbBaudRate.Name = "tbBaudRate";
            this.tbBaudRate.Size = new System.Drawing.Size(76, 20);
            this.tbBaudRate.TabIndex = 20;
            this.tbBaudRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(18, 182);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(53, 13);
            this.label9.TabIndex = 21;
            this.label9.Text = "Baud rate";
            // 
            // tbDataBits
            // 
            this.tbDataBits.Location = new System.Drawing.Point(163, 221);
            this.tbDataBits.Name = "tbDataBits";
            this.tbDataBits.Size = new System.Drawing.Size(76, 20);
            this.tbDataBits.TabIndex = 18;
            this.tbDataBits.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(18, 227);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(49, 13);
            this.label10.TabIndex = 19;
            this.label10.Text = "Data bits";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(18, 204);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(47, 13);
            this.label11.TabIndex = 17;
            this.label11.Text = "Parity bit";
            // 
            // tbFrequenza
            // 
            this.tbFrequenza.Location = new System.Drawing.Point(163, 43);
            this.tbFrequenza.Name = "tbFrequenza";
            this.tbFrequenza.Size = new System.Drawing.Size(76, 20);
            this.tbFrequenza.TabIndex = 14;
            this.tbFrequenza.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(18, 50);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(111, 13);
            this.label7.TabIndex = 15;
            this.label7.Text = "Frequenza letture (ms)";
            // 
            // tbCorrPostDx
            // 
            this.tbCorrPostDx.Location = new System.Drawing.Point(163, 143);
            this.tbCorrPostDx.Name = "tbCorrPostDx";
            this.tbCorrPostDx.Size = new System.Drawing.Size(76, 20);
            this.tbCorrPostDx.TabIndex = 12;
            this.tbCorrPostDx.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(18, 150);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(115, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = "Correzione post. destra";
            // 
            // tbCorrAntSx
            // 
            this.tbCorrAntSx.Location = new System.Drawing.Point(163, 77);
            this.tbCorrAntSx.Name = "tbCorrAntSx";
            this.tbCorrAntSx.Size = new System.Drawing.Size(76, 20);
            this.tbCorrAntSx.TabIndex = 10;
            this.tbCorrAntSx.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(18, 84);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(113, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Correzione ant. sinistra";
            // 
            // tbCorrPostSx
            // 
            this.tbCorrPostSx.Location = new System.Drawing.Point(163, 121);
            this.tbCorrPostSx.Name = "tbCorrPostSx";
            this.tbCorrPostSx.Size = new System.Drawing.Size(76, 20);
            this.tbCorrPostSx.TabIndex = 8;
            this.tbCorrPostSx.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(18, 128);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(118, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Correzione post. sinistra";
            // 
            // tbCorrAntDx
            // 
            this.tbCorrAntDx.Location = new System.Drawing.Point(163, 99);
            this.tbCorrAntDx.Name = "tbCorrAntDx";
            this.tbCorrAntDx.Size = new System.Drawing.Size(76, 20);
            this.tbCorrAntDx.TabIndex = 6;
            this.tbCorrAntDx.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 106);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(110, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Correzione ant. destra";
            // 
            // tbLetture
            // 
            this.tbLetture.Location = new System.Drawing.Point(163, 21);
            this.tbLetture.Name = "tbLetture";
            this.tbLetture.Size = new System.Drawing.Size(76, 20);
            this.tbLetture.TabIndex = 5;
            this.tbLetture.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Letture per media";
            // 
            // cbAutoStart
            // 
            this.cbAutoStart.AutoSize = true;
            this.cbAutoStart.Location = new System.Drawing.Point(12, 24);
            this.cbAutoStart.Name = "cbAutoStart";
            this.cbAutoStart.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cbAutoStart.Size = new System.Drawing.Size(73, 17);
            this.cbAutoStart.TabIndex = 2;
            this.cbAutoStart.Text = "Auto Start";
            this.cbAutoStart.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(135, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Server Port";
            // 
            // tbPort
            // 
            this.tbPort.Location = new System.Drawing.Point(212, 22);
            this.tbPort.Name = "tbPort";
            this.tbPort.Size = new System.Drawing.Size(63, 20);
            this.tbPort.TabIndex = 4;
            this.tbPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(33, 414);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(108, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "Ripristina default";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(200, 414);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 6;
            this.button2.Text = "Annulla";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(294, 414);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 7;
            this.button3.Text = "Salva";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(420, 473);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.tbPort);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbAutoStart);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsForm";
            this.Text = "Impostazioni";
            this.Load += new System.EventHandler(this.SettingsForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox cbAutoStart;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbPort;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbLetture;
        private System.Windows.Forms.TextBox tbFrequenza;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tbCorrPostDx;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tbCorrAntSx;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbCorrPostSx;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbCorrAntDx;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tbBaudRate;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox tbDataBits;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.ComboBox cbStop;
        private System.Windows.Forms.ComboBox cbParity;
    }
}
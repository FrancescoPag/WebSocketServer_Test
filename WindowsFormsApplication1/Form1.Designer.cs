namespace WindowsFormsApplication1
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cbBilanciaPort = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cbBorlottoPort = new System.Windows.Forms.ComboBox();
            this.btnRun = new System.Windows.Forms.Button();
            this.btnTestBorlotto = new System.Windows.Forms.Button();
            this.btnTestBilancia = new System.Windows.Forms.Button();
            this.btnSettings = new System.Windows.Forms.Button();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(38, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Stato";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(95, 34);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(47, 13);
            this.lblStatus.TabIndex = 1;
            this.lblStatus.Text = "Stopped";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(38, 66);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Bilancia";
            // 
            // cbBilanciaPort
            // 
            this.cbBilanciaPort.FormattingEnabled = true;
            this.cbBilanciaPort.Location = new System.Drawing.Point(95, 62);
            this.cbBilanciaPort.Name = "cbBilanciaPort";
            this.cbBilanciaPort.Size = new System.Drawing.Size(121, 21);
            this.cbBilanciaPort.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(38, 98);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(43, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Borlotto";
            // 
            // cbBorlottoPort
            // 
            this.cbBorlottoPort.FormattingEnabled = true;
            this.cbBorlottoPort.Location = new System.Drawing.Point(95, 94);
            this.cbBorlottoPort.Name = "cbBorlottoPort";
            this.cbBorlottoPort.Size = new System.Drawing.Size(121, 21);
            this.cbBorlottoPort.TabIndex = 5;
            // 
            // btnRun
            // 
            this.btnRun.Location = new System.Drawing.Point(236, 29);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(75, 23);
            this.btnRun.TabIndex = 6;
            this.btnRun.Text = "Start";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // btnTestBorlotto
            // 
            this.btnTestBorlotto.Location = new System.Drawing.Point(236, 92);
            this.btnTestBorlotto.Name = "btnTestBorlotto";
            this.btnTestBorlotto.Size = new System.Drawing.Size(75, 23);
            this.btnTestBorlotto.TabIndex = 7;
            this.btnTestBorlotto.Text = "Test";
            this.btnTestBorlotto.UseVisualStyleBackColor = true;
            // 
            // btnTestBilancia
            // 
            this.btnTestBilancia.Location = new System.Drawing.Point(236, 61);
            this.btnTestBilancia.Name = "btnTestBilancia";
            this.btnTestBilancia.Size = new System.Drawing.Size(75, 23);
            this.btnTestBilancia.TabIndex = 8;
            this.btnTestBilancia.Text = "Test";
            this.btnTestBilancia.UseVisualStyleBackColor = true;
            // 
            // btnSettings
            // 
            this.btnSettings.Location = new System.Drawing.Point(41, 136);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(124, 23);
            this.btnSettings.TabIndex = 9;
            this.btnSettings.Text = "Impostazioni avanzate";
            this.btnSettings.UseVisualStyleBackColor = true;
            // 
            // notifyIcon
            // 
            this.notifyIcon.Text = "BilanciaBorlotto";
            this.notifyIcon.Visible = true;
            this.notifyIcon.DoubleClick += new System.EventHandler(this.notifyIcon_DoubleClick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(361, 194);
            this.Controls.Add(this.btnSettings);
            this.Controls.Add(this.btnTestBilancia);
            this.Controls.Add(this.btnTestBorlotto);
            this.Controls.Add(this.btnRun);
            this.Controls.Add(this.cbBorlottoPort);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cbBilanciaPort);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "BaloccoBilanciaBorlotto";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbBilanciaPort;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbBorlottoPort;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.Button btnTestBorlotto;
        private System.Windows.Forms.Button btnTestBilancia;
        private System.Windows.Forms.Button btnSettings;
        private System.Windows.Forms.NotifyIcon notifyIcon;
    }
}


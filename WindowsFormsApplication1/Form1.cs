using BaloccoBilanciaBorlotto;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        private bool forceClosing = false;
        public Form1()
        {
            InitializeComponent();
            BilanciaBorlotto.RunEvent += LoadRunningStatus;

            notifyIcon.Icon = Properties.Resources.WebSocketsLogo;
            MenuItem miClose = new MenuItem("Termina", new EventHandler(async (caller, args) =>
            {
                await Task.Run(() => BilanciaBorlotto.Stop());
                forceClosing = true;
                this.Close();
                Application.Exit();
            }));
            MenuItem miOpen = new MenuItem("Mostra", new EventHandler((caller, args) => { this.Show(); }));
            ContextMenu cm = new ContextMenu(new MenuItem[] { miOpen, miClose });
            notifyIcon.ContextMenu = cm;
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            if (BilanciaBorlotto.IsRunning)
                Task.Run(() => BilanciaBorlotto.Stop());
            else
                Task.Run(() => BilanciaBorlotto.Start());
            btnRun.Enabled = false;
        }

        #region Form Events
        private void Form1_Load(object sender, EventArgs e)
        {
            LoadRunningStatus(BilanciaBorlotto.IsRunning);
            //Task.Run(() => BilanciaBorlotto.Start());
            this.Hide();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!forceClosing)
            {
                e.Cancel = true;
                MinimizeToTray();
            }
        }

        private void MinimizeToTray()
        {
            notifyIcon.Visible = true;
            this.Hide();
        }

        private void LoadRunningStatus(bool isRunning)
        {
            if (InvokeRequired)
            {
                BeginInvoke((Action)(() =>
                {
                    if (isRunning)
                    {
                        lblStatus.Text = "Running";
                        btnRun.Text = "Stop";
                        notifyIcon.Text = "BilanciaBorlotto - Running";
                    }
                    else
                    {
                        lblStatus.Text = "Stopped";
                        btnRun.Text = "Run";
                        notifyIcon.Text = "BilanciaBorlotto - Stopped";
                    }
                    btnRun.Enabled = true;
                }));
            }
            else
            {
                if (isRunning)
                {
                    lblStatus.Text = "Running";
                    btnRun.Text = "Stop";
                    notifyIcon.Text = "BilanciaBorlotto - Running";
                }
                else
                {
                    lblStatus.Text = "Stopped";
                    btnRun.Text = "Run";
                    notifyIcon.Text = "BilanciaBorlotto - Stopped";
                }
                btnRun.Enabled = true;
            }
        }
        #endregion

        private void notifyIcon_DoubleClick(object sender, EventArgs e)
        {
            this.Show();
        }

    }
}

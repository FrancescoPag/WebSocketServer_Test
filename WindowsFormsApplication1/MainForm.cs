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
    public partial class MainForm : Form
    {
        private bool forceClosing = false;
        private BilanciaBorlotto _bilanciaBorlotto = null;
        private SettingsForm _settingsForm = null;

        public MainForm()
        {
            InitializeComponent();
            _bilanciaBorlotto = new BilanciaBorlotto(Program.Settings.ServerPort, Program.Settings.BilanciaSettings);
            _bilanciaBorlotto.RunEvent += LoadRunningStatus;

            // TODO gestione cambiamento settings?
            Program.SettingsChanged += () =>
            {
                Task.Run(() =>
                {
                    _bilanciaBorlotto.ChangeSettings(Program.Settings.ServerPort);
                    //bool wasRunning = _bilanciaBorlotto.IsRunning;
                    //this._bilanciaBorlotto.Stop();
                    //if (wasRunning) _bilanciaBorlotto.Start(Program.Settings.serverPort);
                });
            };

            notifyIcon.Icon = Properties.Resources.WebSocketsLogo;
            MenuItem miClose = new MenuItem("Termina", new EventHandler(async (caller, args) =>
            {
                await Task.Run(() => this._bilanciaBorlotto.Stop());
                forceClosing = true;
                notifyIcon.Visible = false;
                this.Close();
                Application.Exit();
            }));
            MenuItem miOpen = new MenuItem("Mostra", new EventHandler((caller, args) => { this.Show(); }));
            ContextMenu cm = new ContextMenu(new MenuItem[] { miOpen, miClose });
            notifyIcon.ContextMenu = cm;

            //BindingList<string> bl = new BindingList<string>(System.IO.Ports.SerialPort.GetPortNames());
            //cbBilanciaPort.DataSource = System.IO.Ports.SerialPort.GetPortNames();
            //cbBorlottoPort.DataSource = System.IO.Ports.SerialPort.GetPortNames();
            BindingSource bs = new BindingSource();
            List<string> portNames = System.IO.Ports.SerialPort.GetPortNames().ToList();
            portNames.Add("COM1000");
            portNames.Add("COM6281");
            bs.DataSource = portNames;
            cbBilanciaPort.DataSource = bs;
            
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            if (_bilanciaBorlotto.IsRunning)
                Task.Run(() => _bilanciaBorlotto.Stop());
            else
                Task.Run(() => _bilanciaBorlotto.Start());
            btnRun.Enabled = false;
        }

        #region Form Events
        private void Form1_Load(object sender, EventArgs e)
        {
            LoadRunningStatus(_bilanciaBorlotto.IsRunning);
            //Task.Run(() => BilanciaBorlotto.Start());

            this.Hide();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!forceClosing)
            {
                e.Cancel = true;
                this.Hide();
            }
        }

        private void LoadRunningStatus(bool isRunning)
        {
            if (InvokeRequired)
            {
                BeginInvoke((Action)(() =>
                {
                    if (isRunning)
                    {
                        lblStatus.Text = $"Running (Port {Program.Settings.ServerPort})";
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

        private void btnSettings_Click(object sender, EventArgs e)
        {
            if (_settingsForm == null)
            {
                _settingsForm = new SettingsForm();
                _settingsForm.FormClosed += ((s, ea) => _settingsForm = null);
                _settingsForm.Show();
            }
            else
                _settingsForm.BringToFront();
        }
    }
}

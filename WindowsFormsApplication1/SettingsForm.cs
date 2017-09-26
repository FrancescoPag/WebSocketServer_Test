using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class SettingsForm : Form
    {
        private StopBits[] stop = (StopBits[])Enum.GetValues(typeof(StopBits));
        private Parity[] parity = (Parity[])Enum.GetValues(typeof(Parity));
        private Settings _currentSettings;

        public SettingsForm()
        {
            InitializeComponent();
            cbParity.DataSource = parity;
            cbStop.DataSource = stop;
            _currentSettings = Program.Settings;
            LoadSettings(_currentSettings);
        }

        private void LoadSettings(Settings settings)
        {
            cbAutoStart.Checked = settings.autoStart;
            tbPort.Text = settings.serverPort.ToString();

            tbLetture.Text = settings.bilanciaSettings.LetturePerMedia.ToString();
            tbFrequenza.Text = settings.bilanciaSettings.FrequenzaLettura.ToString();
            tbCorrAntSx.Text = settings.bilanciaSettings.CorrezioneAntSx.ToString();
            tbCorrAntDx.Text = settings.bilanciaSettings.CorrezioneAntDx.ToString();
            tbCorrPostSx.Text = settings.bilanciaSettings.CorrezionePostSx.ToString();
            tbCorrPostDx.Text = settings.bilanciaSettings.CorrezionePostDx.ToString();
            tbBaudRate.Text = settings.bilanciaSettings.BaudRate.ToString();
            tbDataBits.Text = settings.bilanciaSettings.DataBits.ToString();

            cbParity.SelectedItem = settings.bilanciaSettings.ParityBit;
            cbStop.SelectedItem = settings.bilanciaSettings.StopBits;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _currentSettings = Settings.Default;
            LoadSettings(_currentSettings);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                _currentSettings.autoStart = cbAutoStart.Checked;
                _currentSettings.serverPort = int.Parse(tbPort.Text);
                _currentSettings.bilanciaSettings.LetturePerMedia = int.Parse(tbLetture.Text);
                _currentSettings.bilanciaSettings.FrequenzaLettura = int.Parse(tbFrequenza.Text);
                _currentSettings.bilanciaSettings.CorrezioneAntSx = double.Parse(tbCorrAntSx.Text);
                _currentSettings.bilanciaSettings.CorrezioneAntDx = double.Parse(tbCorrAntDx.Text);
                _currentSettings.bilanciaSettings.CorrezionePostDx = double.Parse(tbCorrPostDx.Text);
                _currentSettings.bilanciaSettings.CorrezionePostSx = double.Parse(tbCorrPostSx.Text);
                _currentSettings.bilanciaSettings.BaudRate = int.Parse(tbBaudRate.Text);
                _currentSettings.bilanciaSettings.DataBits = int.Parse(tbDataBits.Text);
                _currentSettings.bilanciaSettings.ParityBit = (Parity)cbParity.SelectedItem;
                _currentSettings.bilanciaSettings.StopBits = (StopBits)cbStop.SelectedItem;
                _currentSettings.Save();
                Program.Settings = _currentSettings;
                Program.SaveAutoStartSetting();
                this.Close();
            }
            catch(Exception)
            {
                LoadSettings(_currentSettings);
            }
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            this.ActiveControl = button2;
        }
    }
}

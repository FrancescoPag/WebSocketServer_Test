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
            ShowSettings(_currentSettings);
        }

        private void ShowSettings(Settings settings)
        {
            cbAutoStart.Checked = settings.AutoStart;
            tbPort.Text = settings.ServerPort.ToString();

            tbLetture.Text = settings.BilanciaSettings.LetturePerMedia.ToString();
            tbFrequenza.Text = settings.BilanciaSettings.FrequenzaLettura.ToString();
            tbCorrAntSx.Text = settings.BilanciaSettings.CorrezioneAntSx.ToString();
            tbCorrAntDx.Text = settings.BilanciaSettings.CorrezioneAntDx.ToString();
            tbCorrPostSx.Text = settings.BilanciaSettings.CorrezionePostSx.ToString();
            tbCorrPostDx.Text = settings.BilanciaSettings.CorrezionePostDx.ToString();
            tbBaudRate.Text = settings.BilanciaSettings.BaudRate.ToString();
            tbDataBits.Text = settings.BilanciaSettings.DataBits.ToString();

            cbParity.SelectedItem = settings.BilanciaSettings.ParityBit;
            cbStop.SelectedItem = settings.BilanciaSettings.StopBits;
        }

        private void button2_Click(object sender, EventArgs e)  // Annulla
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)  // Ripristina default
        {
            _currentSettings = Settings.Default;
            ShowSettings(_currentSettings);
        }

        private void button3_Click(object sender, EventArgs e)  // Salva
        {
            try
            {
                _currentSettings.AutoStart = cbAutoStart.Checked;
                _currentSettings.ServerPort = int.Parse(tbPort.Text);
                _currentSettings.BilanciaSettings.LetturePerMedia = int.Parse(tbLetture.Text);
                _currentSettings.BilanciaSettings.FrequenzaLettura = int.Parse(tbFrequenza.Text);
                _currentSettings.BilanciaSettings.CorrezioneAntSx = double.Parse(tbCorrAntSx.Text);
                _currentSettings.BilanciaSettings.CorrezioneAntDx = double.Parse(tbCorrAntDx.Text);
                _currentSettings.BilanciaSettings.CorrezionePostDx = double.Parse(tbCorrPostDx.Text);
                _currentSettings.BilanciaSettings.CorrezionePostSx = double.Parse(tbCorrPostSx.Text);
                _currentSettings.BilanciaSettings.BaudRate = int.Parse(tbBaudRate.Text);
                _currentSettings.BilanciaSettings.DataBits = int.Parse(tbDataBits.Text);
                _currentSettings.BilanciaSettings.ParityBit = (Parity)cbParity.SelectedItem;
                _currentSettings.BilanciaSettings.StopBits = (StopBits)cbStop.SelectedItem;
                _currentSettings.Save();
                Program.Settings = _currentSettings;
                //Program.SaveAutoStartSetting();
                this.Close();
            }
            catch(Exception)
            {
                ShowSettings(_currentSettings);
            }
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            this.ActiveControl = button2;   // Toglie focus da textbox
        }
    }
}

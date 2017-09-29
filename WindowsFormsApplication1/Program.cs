using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using BaloccoBilanciaBorlotto;
using Microsoft.Win32;

namespace WindowsFormsApplication1
{
    static class Program
    {
        private const string RUN_LOCATION = @"Software\Microsoft\Windows\CurrentVersion\Run";
        private const string RUN_REGISTRY_KEY = @"BilanciaBorlotto";

        private static Settings _settings;
        public static Settings Settings { get { return _settings; } set { _settings = value; SaveSettings(); } }
        public static Action SettingsChanged;

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Settings = Settings.Load();
            SaveAutoStartSetting();     // a primo avvio metto subito autostart
            Application.Run(new MainForm());
        }

        private static void SaveSettings()
        {
            _settings.Save();
            SaveAutoStartSetting();
            SettingsChanged?.Invoke();
        }

        private static void SaveAutoStartSetting()
        {
            if (Settings.AutoStart != IsAutoStartEnabled())
            {
                if (Settings.AutoStart)
                    EnableAutoStart();
                else
                    DisableAutoStart();
            }
        }

        private static void EnableAutoStart()
        {
            RegistryKey key = Registry.CurrentUser.CreateSubKey(RUN_LOCATION);
            key.SetValue(RUN_REGISTRY_KEY, System.Reflection.Assembly.GetExecutingAssembly().Location);
        }

        private static bool IsAutoStartEnabled()
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(RUN_LOCATION);
            if (key == null)
                return false;

            string value = (string)key.GetValue(RUN_REGISTRY_KEY);
            if (value == null)
                return false;

            return (value == System.Reflection.Assembly.GetExecutingAssembly().Location);
        }

        private static void DisableAutoStart()
        {
            RegistryKey key = Registry.CurrentUser.CreateSubKey(RUN_LOCATION);
            key.DeleteValue(RUN_REGISTRY_KEY);
        }
    }
}

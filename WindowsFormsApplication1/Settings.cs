using BaloccoBilanciaBorlotto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    [Serializable]
    class Settings
    {
        private static readonly string FILE_NAME = "config.dat";
        public static Settings Default { get; private set; }
        public BilanciaSettings BilanciaSettings;
        public bool AutoStart;
        public int ServerPort;
        //public string logFileName;
        
        static Settings()
        {
            Default = new Settings();
        }

        private Settings()
        {
            BilanciaSettings = new BilanciaSettings();
            AutoStart = true;
            ServerPort = 81;
            //logFileName = "log.txt";
        }

        public static Settings Load(bool loadDefault = false)
        {
            if (loadDefault || !File.Exists(FILE_NAME))
                return Default;
            try
            {
                using(Stream fileStream = File.Open(FILE_NAME, FileMode.Open))
                {
                    var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    return (Settings)binaryFormatter.Deserialize(fileStream);
                }
            }
            catch(Exception)
            {
                return Default;
            }
        }

        public bool Save()
        {
            try
            {
                using (Stream fileStream = File.Create(FILE_NAME))
                {
                    var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    binaryFormatter.Serialize(fileStream, this);
                }
                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }
    }
}

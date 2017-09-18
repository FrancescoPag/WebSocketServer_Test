using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaloccoBilanciaBorlotto
{
    public enum Tire
    {
        ANT_SX = 0,
        ANT_DX = 1,
        POST_SX = 2,
        POST_DX = 3
    }

    static class Bilancia
    {
        private const int READ_LIMIT_SIZE = 33;
        private const int NUMERO_LETTURE = 3;

        static int readPeriod = 200;
        static SerialPort spBilancia = new SerialPort("COM1", 9600, Parity.None, 8, StopBits.One);
        static char[] readBuffer = new char[36];

        static double[,] reads;
        static bool readAvailable;
        static volatile float antSx, antDx, postSx, postDx;


        static void Start()
        {
            spBilancia.ReadTimeout = 30000;
            reads = new double[4,NUMERO_LETTURE];
            readAvailable = false;

        }

        static void Read()
        {
            try
            {
                spBilancia.Open();

                int readCount = 0;
                Array.Clear(readBuffer, 0, 36);

                spBilancia.Write("#01A\r");
                while (readCount < READ_LIMIT_SIZE && readBuffer[readCount] != '\r')
                    readBuffer[readCount++] = (char)spBilancia.ReadChar();

                spBilancia.Close();

                antSx = int.Parse(new string(readBuffer, 4, 6));
                antDx = int.Parse(new string(readBuffer, 11, 6));
                postSx = int.Parse(new string(readBuffer, 18, 6));
                postDx = int.Parse(new string(readBuffer, 25, 6));
            }
            catch(Exception ex)
            {
                // log
            }
            finally
            {
                try { if (spBilancia.IsOpen) spBilancia.Close(); } catch(Exception ex) { }
            }
        }

    }
}

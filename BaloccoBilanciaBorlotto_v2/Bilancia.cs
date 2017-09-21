using Alchemy.Classes;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bilancia
{
    class Settings
    {
        public int NUMERO_LETTURE = 3;
        public int TIMER_MS = 200;

        public double CORREZIONE_ANT_SX = 1.0;
        public double CORREZIONE_ANT_DX = 1.0;
        public double CORREZIONE_POST_SX = 1.0;
        public double CORREZIONE_POST_DX = 1.0;

        public int PORTA_COM = 1;
        public int BAUD_RATE = 9600;
        public Parity PARITY_BIT = Parity.None;
        public int DATA_BITS = 8;
        public StopBits STOP_BITS = StopBits.One;
    }

    class Manager
    {
        private static HashSet<UserContext> _users;
        private static object _lock;

    }

    class Producer
    {

    }

    class Consumer
    {

    }

    class Product
    {
        public double ant_sx, ant_dx, post_sx, post_dx;     // nomi che verranno serializzati in JSON

        public Product(double antSx, double antDx, double postSx, double postDx)
        {
            this.ant_sx = antSx;
            this.ant_dx = antDx;
            this.post_sx = postSx;
            this.post_dx = postDx;
        }
    }
}

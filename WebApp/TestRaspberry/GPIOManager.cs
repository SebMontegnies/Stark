using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace TestRaspberry
{
    public enum Pin
    {
        GPIO_17 = 17,
        GPIO_18 = 18,
        GPIO_22 = 22,
        GPIO_25 = 25,
        GPIO_24 = 24,
        GPIO_23 = 23,
        GPIO_27 = 27,
    }

    public enum Direction { In, Out }

    public class GPIO
    {
        [DllImport("libbcm2835.so", EntryPoint = "bcm2835_init")]
        static extern bool bcm2835_init();

        [DllImport("libbcm2835.so", EntryPoint = "bcm2835_gpio_lev")]
        static extern bool GPIORead(Pin pin);

        static GPIO()
        {
            try
            {
                if (!bcm2835_init())
                    throw new Exception("Unable to initialize bcm2835.so library");
            }
            catch
            {
                Console.Write("ERREUR : DLL bcm2835 non chargé (GPIOManager.Init())");
            }
        }

        Direction _direction = Direction.In;
        Pin Pin { get; set; }
        Direction Direction
        {
            get { return _direction; }
            set
            {
                _direction = value;
                SendCmd(" -g mode " + (int)Pin + " " + (value == Direction.In ? "in" : "out"));
            }
        }

        public bool Value
        {
            get
            {
                try
                {
                    return GPIORead(Pin);
                }
                catch
                {
                    Console.Write("ERREUR : Impossible de lire la valeur du GPIO " + Pin.ToString() + " - (GPIOManager.Value)");
                    return false;
                }
            }
            set
            {
                SendCmd(" -g write " + (int)Pin + " " + (value ? "1" : "0"));
            }
        }

        public GPIO(Pin pin, Direction direction)
        {
            Pin = pin;
            Direction = direction;
        }

        private void SendCmd(string cmd)
        {
            try
            {
                Process proc = new Process();
                proc.EnableRaisingEvents = false;
                proc.StartInfo.FileName = "gpio";
                proc.StartInfo.Arguments = cmd;
                proc.Start();
                proc.WaitForExit();
            }
            catch
            {
                Console.Write("ERREUR : Impossible d'écrire sur le GPIO " + Pin.ToString() + " - (GPIOManager.SendCmd())");
            }
        }
    }
}

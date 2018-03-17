using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestRaspberry
{
    public class Max30100
    {

        byte INT_STATUS = 0x00;  // Which interrupts are tripped
        byte INT_ENABLE = 0x01;  // Which interrupts are active
        byte FIFO_WR_PTR = 0x02;  // Where data is being written
        byte OVRFLOW_CTR = 0x03;  // Number of lost samples
        byte FIFO_RD_PTR = 0x04;  // Where to read from
        byte FIFO_DATA = 0x05;  // Ouput data buffer
        byte MODE_CONFIG = 0x06;  // Control register
        byte SPO2_CONFIG = 0x07;  // Oximetry settings
        byte LED_CONFIG = 0x09;  // Pulse width and power of LEDs
        byte TEMP_INTG = 0x16;  // Temperature value, whole number
        byte TEMP_FRAC = 0x17;  // Temperature value, fraction
        byte REV_ID = 0xFE;  // Part revision
        byte PART_ID = 0xFF;  // Part ID, normally 0x11

        byte I2C_ADDRESS = 0x57;  // I2C address of the MAX30100 device


        public enum PULSE_WIDTH
        {
            P200 = 0,
            P400 = 1,
            P800 = 2,
            P1600 = 3,
        }

        public enum SAMPLE_RATE
        {
            R50 = 0,
            R100 = 1,
            R167 = 2,
            R200 = 3,
            R400 = 4,
            R600 = 5,
            R800 = 6,
            R1000 = 7,
        }

        public enum LED_CURRENT
        {
            L0 = 0,
            L4_4 = 1,
            L7_6 = 2,
            L11_0 = 3,
            L14_2 = 4,
            L17_4 = 5,
            L20_8 = 6,
            L24_0 = 7,
            L27_1 = 8,
            L30_6 = 9,
            L33_8 = 10,
            L37_0 = 11,
            L40_2 = 12,
            L43_6 = 13,
            L46_8 = 14,
            L50_0 = 15
        }

        //def _get_valid(d, value):
        //    try:
        //        return d[value]
        //    except KeyError:
        //        raise KeyError("Value %s not valid, use one of: %s" % (value, ', '.join([str(s) for s in d.keys()])))

        private int _twos_complement(int val, int bits)
        {
            if ((val & (1 << (bits - 1))) != 0) // if sign bit is set e.g., 8bit: 128-255
                val = val - (1 << bits);
            return val;
        }


        int INTERRUPT_SPO2 = 0;
        int INTERRUPT_HR = 1;
        int INTERRUPT_TEMP = 2;
        int INTERRUPT_FIFO = 3;

        const byte MODE_HR = 0x02;
        const byte MODE_SPO2 = 0x03;
        const byte DEVICE_ADDRESS = 0x57;

        public bool HasFinger = false;
        public int IR = 0;
        public int RED = 0;
        public bool isStart = false;

        public Max30100(byte mode = MODE_HR,
                         SAMPLE_RATE sample_rate = SAMPLE_RATE.R100,
                         LED_CURRENT led_current_red = LED_CURRENT.L11_0,
                         LED_CURRENT led_current_ir = LED_CURRENT.L11_0,
                         PULSE_WIDTH pulse_width = PULSE_WIDTH.P1600,
                         int max_buffer_len = 10000
                         )
        {

            new Thread(new ThreadStart(delegate
            {

                var list = new List<int[]>();
                int count = 0;
                while(Program.appRun)
                {
                    if (Program.pause)
                    {
                        Thread.Sleep(2000);
                        if (isStart)
                            shutdown();
                    }
                    else
                    {
                        if (!isStart)
                            start();
                        var data = read_sensor();
                        if (data == null) continue;
                        list.Insert(0, data);
                        list = list.Take(10).ToList();
                        var hasfinger = list.Count(i => i[0] == 0) < 2;
                        HasFinger = hasfinger;

                        if (count == 10)
                        {
                            if (HasFinger)
                            {
                                var moyenIR = (int)list.Average(i => i[0]);
                                var ir = moyenIR / 100 - 40;
                                IR = ir > 90 ? 90 : ir < 40 ? 0 : ir;
                                

                                var red = (int)(((double)(data[1] / 10000.0) - 0.05) * 100.0);
                                RED = IR == 0 ? 0 : (red > 100 ? 100 : red < 0 ? 0 : red) - 1;
                            }
                            else
                            {
                                IR = RED = 0;
                            }
                        }
                        count++;
                        if (count >= 11) count = 0;
                    }
                }

            })).Start();
            

        }

        private void set_led_current(LED_CURRENT led_current_red = LED_CURRENT.L11_0, LED_CURRENT led_current_ir = LED_CURRENT.L11_0)
        {
            Set(LED_CONFIG, (byte)(((int)led_current_red << 4) | (int)led_current_ir));
            Console.WriteLine("LED config : " + Get(LED_CONFIG));
        }

        private void set_mode(byte mode)
        {
            var val = Get(MODE_CONFIG);
            Set(MODE_CONFIG, (byte)(val & 0x74));
            Set(MODE_CONFIG, (byte)(val | mode));
            Console.Write("MODE : " + Get(MODE_CONFIG));
        }


        public void set_spo_config(SAMPLE_RATE sample_rate = SAMPLE_RATE.R100, PULSE_WIDTH pulse_width = PULSE_WIDTH.P1600)
        {
            var reg = Get(SPO2_CONFIG);
            reg = reg & 0xFC;  // Set LED pulsewidth to 00
            Set(SPO2_CONFIG, (byte)(reg | (int)pulse_width));
        }


        public void enable_spo2()
        {
            set_mode(MODE_SPO2);
        }

        public void disable_spo2()
        {
            set_mode(MODE_HR);
        }
        /*    def enable_interrupt(self, interrupt_type):
                self.i2c.write_byte_data(I2C_ADDRESS, INT_ENABLE, (interrupt_type + 1)<<4)
                self.i2c.read_byte_data(I2C_ADDRESS, INT_STATUS)

            def get_number_of_samples(self):
                write_ptr = self.i2c.read_byte_data(I2C_ADDRESS, FIFO_WR_PTR)
                read_ptr = self.i2c.read_byte_data(I2C_ADDRESS, FIFO_RD_PTR)
                return abs(16+write_ptr - read_ptr) % 16

            */

        public int[] read_sensor()
        {
            try
            {
                Process proc = new Process();
                proc.StartInfo.WindowStyle = new ProcessWindowStyle();
                proc.StartInfo.CreateNoWindow = false;
                proc.StartInfo.FileName = "python";
                proc.StartInfo.Arguments = Path.GetDirectoryName(Environment.CommandLine) + "/reader.py";
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.Start();

                proc.WaitForExit();
                var res = proc.StandardOutput.ReadToEnd();

                var data = res.Replace("(","").Replace(")","").Replace(" ","").Split(',');
               // Console.WriteLine(data[0] + " - " + data[1]);
                return data.Select(i => int.Parse(i.Trim())).ToArray();
            }
            catch (Exception ex)
            {
               // Console.Write("ERREUR: Impossible de récupérer les données - (read_sensor()) : " + ex);
            }
            return null;
        }

        public void start()
        {
            set_led_current(LED_CURRENT.L11_0, LED_CURRENT.L11_0);
            set_spo_config(SAMPLE_RATE.R100, PULSE_WIDTH.P1600);
            enable_spo2();
            isStart = true;
        }

        public void shutdown()
        {
            isStart = false;
            var reg = Get(MODE_CONFIG);
            Set(MODE_CONFIG, (byte)(reg | 0x80));
        }

        public void reset()
        {
            var reg = Get(MODE_CONFIG);
            Set(MODE_CONFIG, (byte)(reg | 0x40));
        }

        public void refresh_temperature()
        {
            var reg = Get(MODE_CONFIG);
            Set(MODE_CONFIG, (byte)(reg | (1 << 3)));
        }

        public double get_temperature()
        {
            var intg = Get(TEMP_INTG);
            var frac = Get(TEMP_FRAC);
            return intg + (frac * 0.0625);
        }

        public int Get(byte dataAddress) => I2CNativeLib.Get("1", "0x57", dataAddress.ToString("x"));

        public int Set(byte dataAddress, byte dataValue) => I2CNativeLib.Set("1", "0x57", dataAddress.ToString("x"), dataValue.ToString("x"), 1);
    }
}

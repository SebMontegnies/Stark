using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestRaspberry
{
    public class TEMP
    {
        public double T = 0;

        public TEMP()
        {
            new Thread(new ThreadStart(delegate
            {
                while(Program.appRun)
                {
                    if (Program.pause)
                        Thread.Sleep(2000);
                    else
                    {
                        var d = Read();
                        d = d ?? 250;

                        var t = d / 50;
                        T = d > 230 ? 0 : (36.5 + ((double)t / 10.0));
                        Thread.Sleep(1000);
                    }
                }

            })).Start();
        }

        public int? Read()
        {
            try
            {
                Process proc = new Process();
                proc.StartInfo.WindowStyle = new ProcessWindowStyle();
                proc.StartInfo.CreateNoWindow = false;
                proc.StartInfo.FileName = "python";
                proc.StartInfo.Arguments = Path.GetDirectoryName(Environment.CommandLine) + "/reader2.py";
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.Start();

                proc.WaitForExit();
                var res = proc.StandardOutput.ReadToEnd();
                return int.Parse(res.Trim());
            }
            catch (Exception ex)
            {
                // Console.Write("ERREUR: Impossible de récupérer les données - (read_sensor()) : " + ex);
            }
            return null;
        }




        public int Get(byte dataAddress) => I2CNativeLib.Get("1", "0x5b", dataAddress.ToString("x"));

        public int Set(byte dataAddress, byte dataValue) => I2CNativeLib.Set("1", "0x5b", dataAddress.ToString("x"), dataValue.ToString("x"), 1);
    }
}

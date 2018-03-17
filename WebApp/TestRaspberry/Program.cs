using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestRaspberry
{
    class Program
    {
        public static bool appRun = true;
        public static bool pause = true;

        private static GPIO gpio17 = new GPIO(Pin.GPIO_17, Direction.Out);
        private static GPIO gpio27 = new GPIO(Pin.GPIO_27, Direction.Out);
        private static bool c = false;

        static void Main(string[] args)
        {
            try
            {
                Console.Clear();
                appRun = true;
                var m = new Max30100();
                var t = new TEMP();
       
                new Thread(new ThreadStart(delegate
                {
                    while (appRun)
                    {
                        Console.Clear();

                        if (pause)
                            Console.WriteLine("PAUSE");
                        else
                        {
                            Console.WriteLine(m.IR + "bpm");
                            Console.WriteLine(m.RED + "%");
                            Console.WriteLine(m.IR == 0 ? "-" : t.T + "°c");
                            PostInfoAsync(m.IR, m.RED, m.IR == 0 ? 0 : t.T);
                        }
                        GetActiveAsync();
                        Thread.Sleep(1000);
                    }
                })).Start();

                Console.Read();
                appRun = false;
                m.shutdown();
                gpio17.Value = false;
                gpio27.Value = false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private static async void PostInfoAsync(int bpm, int oxy, double temp)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://diseaseit.azurewebsites.net");
                    var res = await client.PostAsync("api/health", new StringContent($"temperature={temp.ToString().Replace(",",".")}&bloodoxygenation={oxy}&heartbeat={bpm}", Encoding.UTF8, "application/x-www-form-urlencoded"));
                    var str = await res.Content.ReadAsStringAsync();
                    Console.WriteLine(str);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static async void GetActiveAsync()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://diseaseit.azurewebsites.net");
                    var res = await client.GetAsync("api/active");
                    var str = await res.Content.ReadAsStringAsync();
                    pause = !bool.Parse(str);

                    gpio17.Value = pause ? false : c = !c; //Verify system ok
                    gpio27.Value = pause ? c = !c : false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                pause = true;
            }
        }
    }
}

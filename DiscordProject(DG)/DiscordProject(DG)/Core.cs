using DiscordRPC;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace DiscordRichPrecence
{
    public class Core
    {
        private static DiscordRpcClient client;
        private static Thread worker;
        private static bool running = false;
        private static List<string> Log = new List<string>();
        public static void Start(string data)
        {
            if (running)
            {
                SaveLog("It Is Already Running!");
                return;
            }

            client = new DiscordRpcClient("1281587039887691838");
            client.Initialize();

            client.SetPresence(new RichPresence()
            {
                Details = "Playing Dark Rust 🕹️",
                State = data,
                Type = ActivityType.Playing,
                Assets = new Assets()
                {
                    LargeImageKey = "rusticon",
                    LargeImageText = "Rust207",
                    SmallImageKey = "dglogo",
                    SmallImageText = "Dark Gaming"
                },
                Buttons = new Button[]
                {
                    new Button() 
                    {
                        Label = "Join Game",
                        Url = "https://dark-game.ir/shop/index.php/serverrust/",
                        
                    }
                }
            });
            SaveLog("RichPrecense Activated!");
            running = true;
            worker = new Thread(() =>
            {
                while (running)
                {
                    client.Invoke();
                    Thread.Sleep(1500);
                }
            });

            worker.IsBackground = true;
            worker.Start();
        }

        public static void Stop()
        {
            if (!running)
            {
                SaveLog("Stopped Working(RichPrecence)");
                return;
            }


            running = false;
            worker.Join();
            
            SaveLog("Stopped Working(RichPrecence)");
            client.ClearPresence();
            client.Dispose();
            client = null;
        }

        public static void SaveLog(string log)
        {
            Log.Add(log);
            string[] myarray = Log.ToArray();
            //File.WriteAllLines("RPC(Log).txt", myarray);
        }
    }
}

using Discord;
using Discord.WebSocket;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

class Program
{
    private static DiscordSocketClient _client;

    class Configjson
    {
        public string token = "apitoken";
        public string eventapi = "http://5.42.223.21:2570/?pass=MyNigger";
        public string primaryapi = "http://5.42.223.21:2569/?pass=MyNigger";
    }

    private static List<string> onlineMessages = new List<string>
    {
        "Server is online, enjoy your game!",
        "We're live! Hop in and have fun!",
        "Server is running smoothly – happy gaming!",
        "Online and ready – join the fun!",
        "The server’s up – time to play!",
        "Server online! Let the games begin!",
        "All systems go – enjoy your session!",
        "You’re good to go! Server is online.",
        "Live and kicking – game on!",
        "The server is up – dive into the action!"
    };
    private static int imessage = 0;
    static async Task Main(string[] args)
    {
        string cfg = "";

        try
        {
            cfg = File.ReadAllText("config.json");
        }
        catch
        {
            Configjson defaultcfg = new Configjson();
            string defaultcfgs = JsonConvert.SerializeObject(defaultcfg);
            cfg = defaultcfgs;
            File.WriteAllText("config.json", defaultcfgs);
        }


        var dcfg = JsonConvert.DeserializeObject<Configjson>(cfg);


        if(dcfg.token == "apitoken")
        {
            System.Console.WriteLine("Please Edit the config.json file !");
            return;
        }

        _client = new DiscordSocketClient();

        _client.Log += LogAsync;

        await _client.LoginAsync(TokenType.Bot, dcfg.token);
        await _client.StartAsync();

        while (true)
        {
            bool eventon = await CheckEvent(dcfg.eventapi);
            string statusMessage = await GetStatusFromUrl(dcfg.primaryapi, eventon);
            

            if (!string.IsNullOrEmpty(statusMessage))
            {
                await SetBotStatus(statusMessage);
            }

            await Task.Delay(TimeSpan.FromMinutes(1));
        }
    }

    static async Task<string> GetStatusFromUrl(string url, bool Event)
    {
        try
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();

                var json = JObject.Parse(responseBody);
                if(Event)
                {
                    return "The Event’s On — Hop On The Bandwagon!";
                }
                if (((int)json["CurrentPlayers"]) < 10)
                {
                    string message = onlineMessages[imessage];
                    if(imessage < onlineMessages.Count-1)
                    {
                        imessage++;
                    }
                    else
                    {
                        imessage = 0;
                    }
                    return message;
                }
                string build = $"({json["CurrentPlayers"].ToString()}/{json["MaxPlayers"].ToString()}) Players";
                if(json["JoiningPlayers"].ToString() != "0")
                {
                    build += $" ⇌ Joining({json["JoiningPlayers"].ToString()})";
                }
                return build;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching status from {url}: {ex.Message}");
            return "Offline";
        }
    }

    static async Task<bool> CheckEvent(string url)
    {
        try
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();

                return true;
            }
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    static async Task SetBotStatus(string statusMessage)
    {
        try
        {
            await _client.SetGameAsync(statusMessage , null , ActivityType.Watching);
            Console.WriteLine($"Bot status updated to: {statusMessage}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error setting bot status: {ex.Message}");
        }
    }

    static Task LogAsync(LogMessage log)
    {
        Console.WriteLine(log);
        return Task.CompletedTask;
    }
}

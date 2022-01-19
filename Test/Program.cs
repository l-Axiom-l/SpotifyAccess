using System;
using System.Diagnostics;
using System.Text.Json;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Search(args[0], args[1]);
        }

        static void RequestAccessToken()
        {

        }

        static void Search(string searchWord, string AccessToken)
        {
            SpotifySearch Search;
            searchWord = searchWord.Replace(" ", "%20", StringComparison.Ordinal);
            string command = "curl -X \"GET\" \"https://api.spotify.com/v1/search?q=" + searchWord + "&type=track%2Cartist\" -H \"Accept: application/json\" -H \"Content-Type: application/json\" -H \"Authorization: Bearer " + AccessToken + "\"";
            Console.WriteLine(command);

            var cliProcess = new Process()
            {
                StartInfo = new ProcessStartInfo("CMD.exe", "/c " + command)
                {
                    UseShellExecute = false,
                    RedirectStandardOutput = true
                }
            };

            cliProcess.Start();
            Search = JsonSerializer.Deserialize<SpotifySearch>(cliProcess.StandardOutput.ReadToEnd());
            Console.WriteLine(cliProcess.StandardOutput.ReadToEnd());
            Console.WriteLine("Test:" + Search.tracks.items[0].uri);

            string temp = Search.tracks.items[0].uri.Replace(":", "%3A");
            //Console.WriteLine("CMD.exe " + "/c curl -X \"POST\" \"https://api.spotify.com/v1/me/player/queue?uri=" + temp + "&device_id=" + device.devices[0].id + "\" -H \"Accept: application/json\" -H \"Content-Type: application/json\" -H \"Authorization: Bearer " + AccessToken + "\"");
            Process.Start("CMD.exe", "/c curl -d \"POST\" \"https://api.spotify.com/v1/me/player/queue?uri=" + temp + "\" -H \"Content-Lenght: 0\" -H \"Accept: application/json\" -H \"Content-Type: application/json\" -H \"Authorization: Bearer " + AccessToken + "\"");
            cliProcess.WaitForExit();
            cliProcess.Close();
        }
    }
}

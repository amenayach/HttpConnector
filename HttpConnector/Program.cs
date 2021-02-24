using System;
using System.Threading.Tasks;

namespace HttpConnector
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var command = string.Empty;
            var httpService = new HttpService();

            do
            {
                "Please enter the command number as following:".Print();
                "1. Http Get without proxy".Print();
                "2. Setup proxy".Print();
                "3. Http Get with proxy".Print();
                "4. Http Get with timeout".Print();
                "5. Http Post without proxy".Print();
                "6. Http Post with proxy".Print();
                "7. Http Post with timeout".Print();
                "x. Exit".Print();

                command = Console.ReadLine();

                switch (command.Trim())
                {
                    case "1":
                        "Please enter the URL:".Print(ConsoleColor.Yellow);
                        await httpService.Get(Console.ReadLine(), false, TimeSpan.FromSeconds(0));
                        break;
                    case "2":
                        SetupProxy(httpService);
                        break;
                    case "3":
                        "Please enter the URL:".Print(ConsoleColor.Yellow);
                        await httpService.Get(Console.ReadLine(), true, TimeSpan.FromSeconds(0));
                        break;
                    case "4":
                        await HttpGetWithTimeout(async (url, useProxy, timeout) =>
                        {
                            await httpService.Get(url, useProxy, timeout);
                        });
                        break;
                    case "5":
                        "Please enter the URL:".Print(ConsoleColor.Yellow);
                        await httpService.Post(Console.ReadLine(), false, TimeSpan.FromSeconds(0), GetJsonPayload());
                        break;
                    case "6":
                        "Please enter the URL:".Print(ConsoleColor.Yellow);
                        await httpService.Post(Console.ReadLine(), true, TimeSpan.FromSeconds(0), GetJsonPayload());
                        break;
                    case "7":
                        await HttpGetWithTimeout(async (url, useProxy, timeout) =>
                        {
                            await httpService.Post(url, useProxy, timeout, GetJsonPayload());
                        });
                        break;
                    default:
                        break;
                }

                Console.WriteLine();
                "===============================================================".Print();
            } while (!command.Trim().ToLower().Equals("x"));
        }

        private static string GetJsonPayload()
        {
            "Please enter the json payload:".Print(ConsoleColor.Yellow);
            return Console.ReadLine();
        }

        private static async Task HttpGetWithTimeout(Func<string, bool, TimeSpan, Task> httpServiceAction)
        {
            "Please enter the URL:".Print(ConsoleColor.Yellow);
            var url = Console.ReadLine();
            "Please enter the timeout in seconds:".Print(ConsoleColor.Yellow);
            var timeout = TimeSpan.FromSeconds(int.Parse(Console.ReadLine()));
            "Use proxy (yes/no):".Print(ConsoleColor.Yellow);
            var useProxyAnswer = Console.ReadLine().ToLower()?.Trim();
            var useProxy = useProxyAnswer == "yes" || useProxyAnswer == "y";
            await httpServiceAction(url, useProxy, timeout);
        }

        private static void SetupProxy(HttpService httpService)
        {
            "Proxy IP or host with port like http://somedomain.com:8080:".Print(ConsoleColor.Yellow);
            var proxyHost = Console.ReadLine();

            "Use default creds (yes/no):".Print(ConsoleColor.Yellow);
            var proxyUseDefaultCreds = Console.ReadLine().Trim().ToLower() == "yes";

            if (proxyUseDefaultCreds)
            {
                httpService.SetupProxy(proxyHost, proxyUseDefaultCreds, string.Empty, string.Empty, string.Empty);
            }
            else
            {
                "Proxy domain:".Print(ConsoleColor.Yellow);
                var proxyDomain = Console.ReadLine();

                "Proxy username:".Print(ConsoleColor.Yellow);
                var proxyUsername = Console.ReadLine();

                "Proxy password:".Print(ConsoleColor.Yellow);
                var proxyPassword = Console.ReadLine();

                httpService.SetupProxy(proxyHost, proxyUseDefaultCreds, proxyDomain, proxyUsername, proxyPassword);
            }
        }
    }
}

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
                "x. Exit".Print();

                command = Console.ReadLine();

                switch (command.Trim())
                {
                    case "1":
                        "Please enter the URL:".Print(ConsoleColor.Yellow);
                        await httpService.Get(Console.ReadLine(), false);
                        break;
                    case "2":
                        SetupProxy(httpService);
                        break;
                    case "3":
                        "Please enter the URL:".Print(ConsoleColor.Yellow);
                        await httpService.Get(Console.ReadLine(), true);
                        break;
                    default:
                        break;
                }

                Console.WriteLine();
                "===============================================================".Print();
            } while (!command.Trim().ToLower().Equals("x"));
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

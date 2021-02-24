namespace HttpConnector
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Net;

    public class HttpService
    {
        private string proxyHost;
        private bool proxyUseDefaultCreds;
        private string proxyDomain;
        private string proxyUsername;
        private string proxyPassword;

        public void SetupProxy(
            string proxyHost,
            bool proxyUseDefaultCreds,
            string proxyDomain,
            string proxyUsername,
            string proxyPassword
            )
        {
            this.proxyHost = proxyHost;
            this.proxyUseDefaultCreds = proxyUseDefaultCreds;
            this.proxyDomain = proxyDomain;
            this.proxyUsername = proxyUsername;
            this.proxyPassword = proxyPassword;
        }

        public async Task Get(string url, bool useProxy, TimeSpan timeout)
        {
            try
            {
                using (var httpClient = GetHttpClient(useProxy))
                {
                    if (timeout.TotalSeconds != 0)
                    {
                        httpClient.Timeout = timeout;
                    }

                    var response = await httpClient.GetAsync(url);

                    $"Status code: {response.StatusCode}".Print();

                    var rawContent = await response.Content.ReadAsStringAsync();

                    Console.WriteLine();
                    $"Response Content: {rawContent}".Print(response.IsSuccessStatusCode ? ConsoleColor.Green : ConsoleColor.Red);
                }
            }
            catch (Exception ex)
            {
                "Exception:".Print(ConsoleColor.Red);
                ex.Message.Print(ConsoleColor.Red);
                ex.StackTrace.Print(ConsoleColor.Red);
                if (ex.InnerException != null)
                {
                    ex.Message.Print(ConsoleColor.Red);
                }
            }
        }

        public async Task Post(string url, bool useProxy, TimeSpan timeout, string jsonText)
        {
            try
            {
                using (var httpClient = GetHttpClient(useProxy))
                {
                    if (timeout.TotalSeconds != 0)
                    {
                        httpClient.Timeout = timeout;
                    }

                    var content = new StringContent(jsonText, Encoding.UTF8, "application/json");

                    var response = await httpClient.PostAsync(url, content);

                    $"Status code: {response.StatusCode}".Print();

                    var rawContent = await response.Content.ReadAsStringAsync();

                    Console.WriteLine();
                    $"Response Content: {rawContent}".Print(response.IsSuccessStatusCode ? ConsoleColor.Green : ConsoleColor.Red);
                }
            }
            catch (Exception ex)
            {
                "Exception:".Print(ConsoleColor.Red);
                ex.Message.Print(ConsoleColor.Red);
                ex.StackTrace.Print(ConsoleColor.Red);
                if (ex.InnerException != null)
                {
                    ex.Message.Print(ConsoleColor.Red);
                }
            }
        }

        private HttpClient GetHttpClient(bool useProxy)
        {
            if (!useProxy)
            {
                return new HttpClient(new HttpClientHandler { UseProxy = false });
            }

            var handler = new HttpClientHandler()
            {
                UseProxy = true,
                Proxy = new System.Net.WebProxy(proxyHost),
            };

            var httpClient = new HttpClient(handler);

            if (proxyUseDefaultCreds)
            {
                handler.UseDefaultCredentials = true;
            }
            else if (!string.IsNullOrWhiteSpace(proxyUsername) && !string.IsNullOrWhiteSpace(proxyPassword))
            {
                handler.PreAuthenticate = false;
                handler.UseDefaultCredentials = false;

                if (!string.IsNullOrWhiteSpace(proxyDomain))
                {
                    handler.Credentials = new NetworkCredential(proxyUsername, proxyPassword, proxyDomain);
                }
                else
                {
                    handler.Credentials = new NetworkCredential(proxyUsername, proxyPassword);
                }
            }

            return httpClient;
        }
    }
}

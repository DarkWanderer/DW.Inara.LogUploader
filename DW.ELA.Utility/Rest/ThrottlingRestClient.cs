namespace DW.ELA.Utility
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web;
    using DW.ELA.Interfaces;
    using DW.ELA.Utility.Log;

    public class ThrottlingRestClient : IRestClient
    {
        private readonly string baseUrl;
        private readonly HttpClient client = new HttpClient();
        private readonly object @lock = new object();

        private DateTime lastRequestTimestamp = DateTime.MinValue;
        private int requestCounter;

        static ThrottlingRestClient()
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }

        internal ThrottlingRestClient(string url)
        {
            baseUrl = url;
            client.DefaultRequestHeaders.Add("X-Requested-With", "EliteLogAgent");
        }

        public async Task<string> PostAsync(string input)
        {
            ThrowIfQuotaExceeded();
            var httpContent = new StringContent(input, Encoding.UTF8, "application/json");
            using (new LoggingTimer("Making request to " + baseUrl))
            {
                var response = await client.PostAsync(baseUrl, httpContent);
                return await response.EnsureSuccessStatusCode().Content.ReadAsStringAsync();
            }
        }

        public async Task<string> PostAsync(IDictionary<string, string> values)
        {
            ThrowIfQuotaExceeded();

            string encodedPost = string.Join("&", values.Select(kvp => kvp.Key + "=" + HttpUtility.UrlEncode(kvp.Value)));

            var httpContent = new StringContent(encodedPost, Encoding.UTF8, "application/x-www-form-urlencoded");

            using (new LoggingTimer("Making request to " + baseUrl))
            {
                var response = await client.PostAsync(baseUrl, httpContent);
                return await response.EnsureSuccessStatusCode().Content.ReadAsStringAsync();
            }
        }

        public async Task<string> GetAsync(string url)
        {
            ThrowIfQuotaExceeded();
            if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
                url = baseUrl + url;
            using (new LoggingTimer("Making request to " + url))
            {
                var response = await client.GetAsync(url);
                return await response.EnsureSuccessStatusCode().Content.ReadAsStringAsync();
            }
        }

        private void ThrowIfQuotaExceeded()
        {
            lock (@lock)
            {
                var now = DateTime.UtcNow;
                int secondsSinceLastCall = 0;

                if (lastRequestTimestamp != DateTime.MinValue)
                    secondsSinceLastCall = (int)(now - lastRequestTimestamp).TotalSeconds;

                int decayedRequestCounter = Math.Max(0, requestCounter - (secondsSinceLastCall / 5));

                lastRequestTimestamp = now;
                requestCounter = decayedRequestCounter + 1;
            }
        }

        public class Factory : IRestClientFactory
        {
            public IRestClient CreateRestClient(string url) => new ThrottlingRestClient(url);
        }
    }
}

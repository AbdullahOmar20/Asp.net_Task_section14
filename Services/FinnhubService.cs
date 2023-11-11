using StockApp.ServicesContract;
using System.Text.Json;

namespace StockApp.Services
{
    public class FinnhubService : IFinnhubService
    {
        private readonly IHttpClientFactory _httpclient;
        private readonly IConfiguration _configuration;
        public FinnhubService(IHttpClientFactory httpclient, IConfiguration config)
        {
            _configuration = config;
            _httpclient = httpclient;
        }
        public async Task<Dictionary<string, object>?> GetCompanyProfile(string stocksymbol)
        {
            using (HttpClient httpclient = _httpclient.CreateClient())
            {
                HttpRequestMessage httprequest = new HttpRequestMessage()
                {
                    RequestUri = new Uri($"https://finnhub.io/api/v1/stock/profile2?symbol={stocksymbol}&token={_configuration["Token"]}"),
                    Method = HttpMethod.Get
                };
                HttpResponseMessage httpresponse = await httpclient.SendAsync(httprequest);
                Stream stream = httpresponse.Content.ReadAsStream();
                StreamReader reader = new StreamReader(stream);
                string response = await reader.ReadToEndAsync();
                Dictionary<string,object>? ResponseDic = JsonSerializer.Deserialize<Dictionary<string,object>>(response);
                if(ResponseDic == null)
                {
                    throw new InvalidOperationException("no response");
                }
                if (ResponseDic.ContainsKey("error"))
                {
                    throw new InvalidOperationException("error");
                }
                return ResponseDic;
            }
        }

        public async Task<Dictionary<string, object>?> GetStockPrice(string stocksymbol)
        {
            using (HttpClient httpclient = _httpclient.CreateClient())
            {
                HttpRequestMessage httprequest = new HttpRequestMessage()
                {
                    RequestUri = new Uri($"https://finnhub.io/api/v1/quote?symbol={stocksymbol}&token={_configuration["Token"]}"),
                    Method = HttpMethod.Get
                };
                HttpResponseMessage httpresponse = await httpclient.SendAsync(httprequest);
                Stream stream = httpresponse.Content.ReadAsStream();
                StreamReader reader = new StreamReader(stream);
                string response = await reader.ReadToEndAsync();
                Dictionary<string, object>? ResponseDic = JsonSerializer.Deserialize<Dictionary<string, object>>(response);
                if (ResponseDic == null)
                {
                    throw new InvalidOperationException("no response");
                }
                if (ResponseDic.ContainsKey("error"))
                {
                    throw new InvalidOperationException("error");
                }
                return ResponseDic;
            }
        }
    }
}

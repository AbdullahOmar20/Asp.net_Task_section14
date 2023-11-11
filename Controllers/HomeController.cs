using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using StockApp.Models;
using StockApp.Services;
using System.Diagnostics;

namespace StockApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IOptions<TradingOption> _options;
        private readonly FinnhubService _finnhubService;

        public HomeController(ILogger<HomeController> logger, IOptions<TradingOption> options, FinnhubService service)
        {
            _logger = logger;
            _options = options;
            _finnhubService = service;
        }
        [Route("/")]
        public async Task<IActionResult> Index()
        {
            if (_options.Value.DefaultOption == null)
            {
                _options.Value.DefaultOption = "MSFT";
            }
            Dictionary<string, object>? responseprice = await _finnhubService.GetStockPrice(_options.Value.DefaultOption);
            Dictionary<string, object>? responseprofile = await _finnhubService.GetCompanyProfile(_options.Value.DefaultOption);
            StockTrade stocktrade = new StockTrade()
            {
                StockName = responseprofile["name"].ToString(),
                StockSymbol = responseprofile["ticker"].ToString(),
                Price = Convert.ToDouble(responseprice["c"].ToString())
                //Quantity = Convert.ToUInt32(responseprofile["marketCapitalization"].ToString())
            };
            return View(stocktrade);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
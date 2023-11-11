namespace StockApp.ServicesContract
{
    public interface IFinnhubService
    {
        public Task<Dictionary<string, object>?> GetCompanyProfile(string stocksymbol);
        public Task<Dictionary<string, object>?> GetStockPrice(string stocksymbol);
    }
}

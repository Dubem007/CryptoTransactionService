namespace CryptoTransaction.API.Common.Utils.Interface
{
    public interface IGenericApiClient
    {
        Task<T> GetAsync<T>(string url);
    }
}

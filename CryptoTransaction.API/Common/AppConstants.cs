namespace CryptoTransaction.API.Common
{
    public static class AppConstants
    {
        public static string AppSystem { get; set; } = $"Upwork-Identity{nameof(AppSystem)}";
        public static readonly string FailedRequestError = "failed request due to error. Please try again";
        public static readonly string DataRetrieveSuccessResponse = "Data successfully Retrieved";
        public static readonly string DataRetrieveFailureResponse = "Failed to Retrieve data as no record exists";
    }
}

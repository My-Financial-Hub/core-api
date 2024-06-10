namespace FinancialHub.Core.Infra.Caching.Configurations
{
    internal class CacheConfiguration
    {
        public const string Cache = "Cache";

        public int ExpirationTime { get; set; }
    }
}

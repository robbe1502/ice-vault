namespace IceVault.Common;

public static class IceVaultConstant
{
    public static class Tracing
    {
        public static string CorrelationHeaderName => "X-Correlation-Id";
    }

    public static class Claim
    {
        public static string TimeZone => "time_zone";

        public static string Locale => "locale";

        public static string Currency => "currency";

        public static string FullName => "full_name";

        public static string Id => "sub";

        public static string Email => "email";
    }
}
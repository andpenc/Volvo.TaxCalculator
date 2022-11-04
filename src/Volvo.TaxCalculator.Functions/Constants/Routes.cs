namespace Volvo.TaxCalculator.Functions.Constants
{
    public static class Routes
    {
        // contant interpolated strings are not available :(  otherwise : $"{Versions.V1}/CongestionTax" - much cleaner
        public const string CONGESTION_TAX = Versions.V1 + "/CongestionTax";
    }

    public static class Versions
    {
        public const string V1 = "v1";
    }
}

using System.Globalization;

namespace SubtitlesTranslator.Application.Helpers {
    public static class CurrencySymbolProvider {
        private static readonly Dictionary<string, string> _cache = new(StringComparer.OrdinalIgnoreCase);

        public static string GetSymbol(string isoCode) 
        {
            if (string.IsNullOrWhiteSpace(isoCode))
                return string.Empty;

            isoCode = isoCode.Trim().ToUpperInvariant();

            // Try to find it in cache
            if (_cache.TryGetValue(isoCode, out var symbol))
                return symbol;

            // Try to find a matching RegionInfo by ISO currency code
            symbol = CultureInfo
                .GetCultures(CultureTypes.SpecificCultures)
                .Select(ci => {
                    try {
                        // Use name‐based constructor to avoid LCID errors
                        return new RegionInfo(ci.Name);
                    } catch {
                        // Skip custom / invalid cultures
                        return null;
                    }
                })
                .FirstOrDefault(ri =>
                    ri != null && ri.ISOCurrencySymbol.Equals(isoCode, StringComparison.OrdinalIgnoreCase))?.CurrencySymbol ?? isoCode;

            // Cache for next time
            _cache[isoCode] = symbol;
            return symbol;
        }
    }
}

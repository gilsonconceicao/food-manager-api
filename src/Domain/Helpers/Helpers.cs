using System.Text.Json;

namespace Domain.Helpers;

public static class Helpers
{
    public static long? GetInt64Safe(JsonElement json, string propertyName) =>
    json.TryGetProperty(propertyName, out var prop) && prop.ValueKind == JsonValueKind.Number && prop.TryGetInt64(out var value)
        ? value
        : (long?)null;

    public static int? GetIntSafe(JsonElement json, string propertyName) =>
      json.TryGetProperty(propertyName, out var prop) && prop.ValueKind == JsonValueKind.Number && prop.TryGetInt32(out var value)
          ? value
          : (int?)null;
    public static decimal? GetDecimalSafe(JsonElement json, string propertyName) =>
        json.TryGetProperty(propertyName, out var prop) && prop.TryGetDecimal(out var value)
            ? value
            : (decimal?)null;

    public static bool? GetBoolSafe(JsonElement json, string propertyName) =>
        json.TryGetProperty(propertyName, out var prop) &&
        (prop.ValueKind == JsonValueKind.True || prop.ValueKind == JsonValueKind.False)
            ? prop.GetBoolean()
            : (bool?)null;

    public static string? GetStringSafe(JsonElement json, string propertyName) =>
        json.TryGetProperty(propertyName, out var prop) && prop.ValueKind != JsonValueKind.Null
            ? prop.GetString()
            : null;

    public static DateTime? GetDateTimeSafe(JsonElement json, string propertyName) =>
        json.TryGetProperty(propertyName, out var prop) && prop.ValueKind == JsonValueKind.String
        && DateTime.TryParse(prop.GetString(), out var date)
            ? date
            : null;

    public static async Task<T?> RetryAsync<T>(Func<Task<T?>> action, int maxRetries = 5, int delayMilliseconds = 1000, CancellationToken cancellationToken = default) where T : class
    {
        for (int attempt = 1; attempt <= maxRetries; attempt++)
        {
            var result = await action();

            if (result != null)
                return result;

            await Task.Delay(delayMilliseconds, cancellationToken);
        }

        return null;
    }

}

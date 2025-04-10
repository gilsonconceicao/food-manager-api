using System.Text.RegularExpressions;

namespace Domain.Extensions;

public static class GenericExtenstions
{
    public static TimeZoneInfo GetBrazilTimeZone()
    {
        try
        {
            return TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");
        }
        catch
        {
            return TimeZoneInfo.FindSystemTimeZoneById("America/Sao_Paulo"); 
        }
    }
}
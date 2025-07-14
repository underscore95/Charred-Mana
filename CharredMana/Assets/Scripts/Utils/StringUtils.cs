
using System.Globalization;
using UnityEngine.Assertions;

public static class StringUtils
{
    public static string CommaSeperatedNumber(float number, int precision = 0)
    {
        Assert.IsTrue(precision >= 0);
        return number.ToString($"N{precision}", CultureInfo.InvariantCulture);
    }

    private static readonly (float Threshold, string Suffix)[] Abbreviations = new[]
   {
        (1_000_000_000_000_000f, "Q"),
        (1_000_000_000_000f,     "T"),
        (1_000_000_000f,         "B"),
        (1_000_000f,             "M"),
        (1_000f,                 "K")
    };

    public static string ShortNumber(float number)
    {
        foreach (var (threshold, suffix) in Abbreviations)
        {
            if (number >= threshold)
            {
                float value = number / threshold;
                string format = value >= 100 ? "0" : value >= 10 ? "0.#" : "0.##";
                return value.ToString(format, CultureInfo.InvariantCulture) + suffix;
            }
        }

        return ((int)number).ToString(CultureInfo.InvariantCulture);
    }

    public enum NumberFormat
    {
        COMMA_SEPERATED, SHORT, RAW
    }

    public static string FormatNumber(float number, NumberFormat numberFormat)
    {
        return numberFormat switch
        {
            NumberFormat.SHORT => ShortNumber(number),
            NumberFormat.COMMA_SEPERATED => CommaSeperatedNumber(number),
            _ => number.ToString(),
        };
    }
}
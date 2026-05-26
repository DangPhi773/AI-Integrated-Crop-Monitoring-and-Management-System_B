using System.Globalization;
using System.Text;

namespace CMMS.BLL.Helpers;

public static class VietQrHelper
{
    private const string BaseUrl = "https://img.vietqr.io/image";
    private const string Template = "compact";

    public static string? BuildImageUrl(
        string? bankBin,
        string? bankAccount,
        decimal amount,
        string? accountHolder,
        string? addInfo)
    {
        if (string.IsNullOrWhiteSpace(bankBin) || string.IsNullOrWhiteSpace(bankAccount) || amount <= 0)
            return null;

        var sb = new StringBuilder();
        sb.Append(BaseUrl).Append('/')
          .Append(Uri.EscapeDataString(bankBin)).Append('-')
          .Append(Uri.EscapeDataString(bankAccount)).Append('-')
          .Append(Template).Append(".png");

        var query = new List<string>
        {
            "amount=" + ((long)amount).ToString(CultureInfo.InvariantCulture)
        };

        if (!string.IsNullOrWhiteSpace(addInfo))
            query.Add("addInfo=" + Uri.EscapeDataString(RemoveDiacritics(addInfo)));

        if (!string.IsNullOrWhiteSpace(accountHolder))
            query.Add("accountName=" + Uri.EscapeDataString(RemoveDiacritics(accountHolder)));

        sb.Append('?').Append(string.Join('&', query));
        return sb.ToString();
    }

    private static string RemoveDiacritics(string text)
    {
        var normalized = text.Normalize(NormalizationForm.FormD);
        var sb = new StringBuilder(normalized.Length);
        foreach (var c in normalized)
        {
            var category = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c);
            if (category != System.Globalization.UnicodeCategory.NonSpacingMark)
                sb.Append(c);
        }
        return sb.ToString().Normalize(NormalizationForm.FormC)
            .Replace('Đ', 'D').Replace('đ', 'd');
    }
}

using System.Text.RegularExpressions;
using MAUI.WebViewInteropHelper.Json;
using Newtonsoft.Json;

namespace MAUI.WebViewInteropHelper.Extensions
{
    internal static class StringExtensions
    {
        internal static ResultJson DeserializeWebResult(this string jsonResult)
        {
            jsonResult = Regex.Unescape(jsonResult);
            return JsonConvert.DeserializeObject<ResultJson>(jsonResult);
        }
    }
}

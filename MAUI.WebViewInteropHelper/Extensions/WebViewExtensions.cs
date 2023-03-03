using System.Net;
using MAUI.WebViewInteropHelper.Exceptions;

namespace MAUI.WebViewInteropHelper.Extensions
{
    internal static class WebViewExtensions
    {
        internal static async Task<TResultType> EvaluateAsynchronousJavaScriptAsync<TResultType>(
            this WebView webView, string asynchronousJavascriptFunction, List<string> args)
            where TResultType : class
        {
            var taskCompletionSourceId =
                WebTaskManager.GenerateTaskCompletionSource(out var taskCompletionSource);
            args.Add($"{taskCompletionSourceId}");
            var resultJson = await webView.EvaluateJavaScriptAsync(
                $"{asynchronousJavascriptFunction}({string.Join(",", args)})");
            var result = resultJson.DeserializeWebResult();

            if (!result.Success)
            {
                WebTaskManager.GetAndRemoveTaskCompletionSource(taskCompletionSourceId);
                throw new WebScriptExecutionException(asynchronousJavascriptFunction, result.Error);
            }

            try
            {
                return await taskCompletionSource.Task as TResultType;
            }
            catch (Exception ex)
            {
                throw new WebScriptExecutionException(asynchronousJavascriptFunction, ex.Message);
            }
        }

        internal static async Task<TResultType> EvaluateSynchronousJavaScriptAsync<TResultType>(
            this WebView webView, string synchronousJavascriptFunction, string[] args)
            where TResultType : class
        {
            var resultJson = await webView.EvaluateJavaScriptAsync(
                $"{synchronousJavascriptFunction}({string.Join(",", args)})");
            var result = resultJson.DeserializeWebResult();

            if (!result.Success)
            {
                throw new WebScriptExecutionException(synchronousJavascriptFunction, result.Error);
            }

            return result.Result as TResultType;
        }
    }
}

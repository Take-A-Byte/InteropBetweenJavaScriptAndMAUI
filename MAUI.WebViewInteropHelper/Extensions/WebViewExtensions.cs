using System.Net;
using MAUI.WebViewInteropHelper.Exceptions;

namespace MAUI.WebViewInteropHelper.Extensions
{
    public static class WebViewExtensions
    {
        private static string _communicationUrl;

        public static async Task<TResultType> EvaluateAsynchronousJavaScriptAsync<TResultType>(
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

        public static async Task<TResultType> EvaluateSynchronousJavaScriptAsync<TResultType>(
            this WebView webView, string synchronousJavascriptFunction, List<string> args)
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

        public static void InitializeFullInteropMode(this WebView webView, string communicationUrl)
        {
            _communicationUrl = communicationUrl; 
            webView.Navigating += OnWebViewNavigating;
            webView.Navigated += OnWebViewNavigated;
        }

        private static void OnWebViewNavigated(object sender, WebNavigatedEventArgs e)
        {
            var webView = (WebView)sender;
            webView.Eval($"initializeFullInteroperability(\"{_communicationUrl}\")");
            webView.Navigated -= OnWebViewNavigated;
        }

        private static async void OnWebViewNavigating(object sender, WebNavigatingEventArgs e)
        {
            var webView = (WebView)sender;
            
            var urlParts = e.Url.Split("?", 2);
            if (!urlParts[0].Equals(_communicationUrl))
            {
                return;
            }

            // prevent the navigation to complete
            e.Cancel = true;
            int id = int.Parse(urlParts[1]);
            var taskCompletionSource = WebTaskManager.GetAndRemoveTaskCompletionSource(id);

            var resultJson = await webView.EvaluateJavaScriptAsync($"getResult({id})");
            var result = resultJson.DeserializeWebResult();
            if (result.Success)
            {
                taskCompletionSource.SetResult(result.Result);
            }
            else
            {
                taskCompletionSource.SetException(new WebScriptExecutionException(null, result.Error));
            }
        }
    }
}

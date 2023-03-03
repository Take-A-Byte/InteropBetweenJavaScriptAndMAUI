using System;
using System.Runtime.Serialization;

namespace MAUI.WebViewInteropHelper.Exceptions
{
    [Serializable]
    public class WebScriptExecutionException : Exception
    {
        public WebScriptExecutionException(string functionName, string exceptionFromWeb)
            : base($"An error occurred while executing {functionName} : {exceptionFromWeb}") { }

        protected WebScriptExecutionException(
            SerializationInfo info,
            StreamingContext context) : base(info, context) { }
    }
}

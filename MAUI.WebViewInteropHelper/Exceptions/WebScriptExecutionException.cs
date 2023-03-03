using System;
using System.Runtime.Serialization;

namespace MAUI.WebViewInteropHelper.Exceptions
{
    [Serializable]
    internal class WebScriptExecutionException : Exception
    {
        public WebScriptExecutionException(string functionName, string exceptionFromWeb)
            : base($"An error occurred while executing {functionName} : {exceptionFromWeb}") { }

        protected WebScriptExecutionException(
            SerializationInfo info,
            StreamingContext context) : base(info, context) { }
    }
}

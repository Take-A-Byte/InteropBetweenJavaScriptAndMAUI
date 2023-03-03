namespace MAUI.WebViewInteropHelper.Json
{
    public struct ResultJson
    {
        private string _result;

        public string Result
        {
            get => _result;
            set
            {
                _result = value;
                Success = true;
            }
        }
        public string Error { get; set; }
        public bool Success { get; set; }
    }
}

namespace InteropBetweenJavaScriptAndMAUI;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        WebView.InitializeFullInteropMode("http://taskcompleted/");
    }
    
    private async void OnCallSynchronousFunctionClicked(object sender, EventArgs e)
    {
        ResultDisplay.Text = string.Empty;
        ResultDisplay.Text = await WebView.EvaluateSynchronousJavaScriptAsync<string>
        ("resolveImmediately", new List<string>());
    }

    private async void OnCallAsynchronousFunctionClicked(object sender, EventArgs e)
    {
        ResultDisplay.Text = string.Empty;
        ResultDisplay.Text = await WebView.EvaluateAsynchronousJavaScriptAsync<string>
            ("resolveAfter2Seconds", new List<string>());
    }
}
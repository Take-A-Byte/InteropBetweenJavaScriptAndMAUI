# InteropBetweenJavaScriptAndMAUI

Currently, MAUI only supports calling JavaScript functions from C# but not the other way around. I have created this helper library to close the gap and have full interoperability between C# and JS with MAUI.


## Usage
- Add reference to `MAUI.WebViewInteropHelper`
- In the HTMl which you would be hosting in WebView, add `<script type="text/javascript" src="interopHelper.js"></script>`
- Before calling any asynchronous function, initialize full interoperability for the web view by calling `WebView.InitializeFullInteropMode("urlUsedForCommunication")`.
- The javascript functions to be called from C# should follow the following style:
	- **For synchronous function** - 
	``` js
	function synchronousFunction() {
		...
		return {
			result: resultConvertedToString
		};
	}
	```
	- **For asynchronous function** - 
	``` js
	// asynchronous functions should always have last argument as id: number
	function asynchronousFunction(id) {
		...
		somePromise.then(result => {
				...
                setResult(id, resultAsAString);
		}.catch(error => {
			setError(id, resultAsAString);
		});

		return communicateSuccessfulCallToAsyncFunction;
	}
	```

**Note:** 
Full interoperability between C# and JS is achieved by communicating the completion of asynchronous function using navigation query with a unique id representing the function call. The navigation url used for communication should be unique as API results are identified by this url.

#if USES_WEBVIEW
using UnityEngine;
using System.Collections;

namespace VoxelBusters.NativePlugins.Internal
{
	public interface IWebViewPlatform 
	{
		#region Methods

		void Create (string _tag, Rect _frame);
		void Destroy (string _tag);

		void Show (string _tag);
		void Hide (string _tag);

		void LoadRequest (string _tag, string _URL);
		void LoadHTMLString (string _tag, string _HTMLString, string _baseURL);
		void LoadData (string _tag, byte[] _byteArray, string _MIMEType, string _textEncodingName, string _baseURL);
		void EvaluateJavaScriptFromString (string _tag, string _javaScript);
		void Reload (string _tag);
		void StopLoading (string _tag);

		void SetCanHide (string _tag, bool _canHide);
		void SetCanBounce (string _tag, bool _canBounce);
		void SetControlType (string _tag, eWebviewControlType _type);
		void SetShowSpinnerOnLoad (string _tag, bool _showSpinner);
		void SetAutoShowOnLoadFinish (string _tag, bool _autoShow);
		void SetScalesPageToFit (string _tag, bool _scaleToFit);
		void SetFrame (string _tag, Rect _frame);
		void SetBackgroundColor (string _tag, Color _color);

		void AddNewURLSchemeName (string _tag, string _newURLSchemeName);
		void ClearCache ();

		void ParseLoadFinishedEventData (IDictionary _JSONDict, out string _tag, out string _URL);
		void ParseLoadFailedEventData (IDictionary _JSONDict, out string _tag, out string _URL, out string _error);
		void ParseEvalJSEventData (IDictionary _JSONDict, out string _tag, out string _result);
		void ParseMessageReceivedEventData (IDictionary _JSONDict, out string _tag, out WebViewMessage _message);

		#endregion
	}
}
#endif
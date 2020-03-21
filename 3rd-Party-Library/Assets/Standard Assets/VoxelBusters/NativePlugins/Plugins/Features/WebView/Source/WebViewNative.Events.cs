#if USES_WEBVIEW
using UnityEngine;
using System.Collections;
using VoxelBusters.Utility;

namespace VoxelBusters.NativePlugins.Internal
{
	public partial class WebViewNative : MonoBehaviour 
	{	
		#region Constants

		// Events
		private const string kOnDidShowEvent						= "OnDidShow";
		private const string kOnDidHideEvent						= "OnDidHide";
		private const string kOnDidDestroyEvent						= "OnDidDestroy";
		private const string kOnDidStartLoadEvent					= "OnDidStartLoad";
		private const string kOnDidFinishLoadEvent					= "OnDidFinishLoad";
		private const string kOnDidFailLoadWithErrorEvent			= "OnDidFailLoadWithError";
		private const string kOnDidFinishEvaluatingJavaScriptEvent	= "OnDidFinishEvaluatingJavaScript";
		private const string kOnDidReceiveMessageEvent				= "OnDidReceiveMessage";

		#endregion

		#region Native Callback Methods

		private void WebViewDidShow (string _tag)
		{
			WebView		_webView	= GetWebViewWithTag(_tag);
			WebViewDidShow(_webView);
		}

		private void WebViewDidShow (WebView _webView)
		{
			// Send event
			if (_webView != null)
			{
				_webView.InvokeMethod(kOnDidShowEvent);
			}
		}
		
		private void WebViewDidHide (string _tag)
		{
			WebView		_webView	= GetWebViewWithTag(_tag);
			WebViewDidHide(_webView);
		}
		
		private void WebViewDidHide (WebView _webView)
		{
			// Send event
			if (_webView != null)
			{
				_webView.InvokeMethod(kOnDidHideEvent);
			}
		}

		private void WebViewDidDestroy (string _tag)
		{
			WebView		_webView	= GetWebViewWithTag(_tag);
			WebViewDidDestroy(_webView);
		}

		private void WebViewDidDestroy (WebView _webView)
		{
			// Send event
			if (_webView != null)
			{
				_webView.InvokeMethod(kOnDidDestroyEvent);
			}

			RemoveWebViewFromCollection(_webView.UniqueID);
			Destroy(_webView.gameObject);
		}
		
		private void WebViewDidStartLoad (string _tag)
		{
			WebView		_webView	= GetWebViewWithTag(_tag);
			WebViewDidStartLoad(_webView);
		}

		private void WebViewDidStartLoad (WebView _webView)
		{
			// Send event
			if (_webView != null)
			{
				_webView.InvokeMethod(kOnDidStartLoadEvent);
			}
		}
		
		private void WebViewDidFinishLoad (string _JSONString)
		{
			IDictionary _JSONDict	= JSONUtility.FromJSON(_JSONString) as IDictionary;

			string 		_tag;
			string 		_URL;
			Platform.ParseLoadFinishedEventData(_JSONDict, out _tag, out _URL);
			
			WebView		_webView	= GetWebViewWithTag(_tag);
			WebViewDidFinishLoad(_webView, _URL);
		}

		private void WebViewDidFinishLoad (WebView _webView, string _URL)
		{
			// Send event
			if (_webView != null)
			{
				_webView.InvokeMethod(
					_method: kOnDidFinishLoadEvent, 
					_argValues: new object[] 
					{
						_URL
					},
					_argTypes: new System.Type[] 
					{
						typeof(string)
					}
				);
			}
		}
		
		private void WebViewDidFailLoadWithError (string _JSONString)
		{	
			IDictionary _JSONDict	= JSONUtility.FromJSON(_JSONString) as IDictionary;

			string 		_tag;
			string 		_URL;
			string 		_error;
			Platform.ParseLoadFailedEventData(_JSONDict, out _tag, out _URL, out _error);

			WebView		_webView	= GetWebViewWithTag(_tag);
			WebViewDidFailLoadWithError(_webView, _URL, _error);
		}

		private void WebViewDidFailLoadWithError (WebView _webView, string _URL, string _error)
		{
			// Send event
			if (_webView != null)
			{
				_webView.InvokeMethod(
					_method: kOnDidFailLoadWithErrorEvent,
					_argValues: new object[] 
					{
						_URL,
						_error
					},
					_argTypes: new System.Type[] 
					{
						typeof(string),
						typeof(string)
					}
				);
			}
		}
		
		private void WebViewDidFinishEvaluatingJS (string _JSONString)
		{
			IDictionary _JSONDict	= JSONUtility.FromJSON(_JSONString) as IDictionary;

			string 		_tag;
			string 		_result;
			Platform.ParseEvalJSEventData(_JSONDict, out _tag, out _result);

			WebView		_webView	= GetWebViewWithTag(_tag);
			WebViewDidFinishEvaluatingJS(_webView, _result);
		}

		private void WebViewDidFinishEvaluatingJS (WebView _webView, string _result)
		{
			// Send event
			if (_webView != null)
			{
				_webView.InvokeMethod(
					_method: kOnDidFinishLoadEvent, 
					_argValues: new object[] 
					{
						_result
					},
					_argTypes: new System.Type[] 
					{
						typeof(string)
					}
				);
			}
		}

		private void WebViewDidReceiveMessage (string _JSONString)
		{
			IDictionary 	_JSONDict	= JSONUtility.FromJSON(_JSONString) as IDictionary;

			string 			_tag;
			WebViewMessage	_message;
			Platform.ParseMessageReceivedEventData(_JSONDict, out _tag, out _message);

			WebView			_webView	= GetWebViewWithTag(_tag);
			WebViewDidReceiveMessage(_webView, _message);
		}

		private void WebViewDidReceiveMessage (WebView _webView, WebViewMessage _message)
		{
			// Send event
			if (_webView != null)
			{
				_webView.InvokeMethod(kOnDidReceiveMessageEvent, _message);
			}
		}

		#endregion
	}
}
#endif
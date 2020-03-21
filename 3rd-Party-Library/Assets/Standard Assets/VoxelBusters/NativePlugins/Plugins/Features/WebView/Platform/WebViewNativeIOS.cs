#if USES_WEBVIEW && UNITY_IOS
using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System.IO;

namespace VoxelBusters.NativePlugins.Internal
{
	public partial class WebViewNativeIOS : IWebViewPlatform
	{
		#region Binding Methods

		[DllImport("__Internal")]
		private static extern void webviewCreate (string _tag, NormalisedRect _noramlisedRect);

		[DllImport("__Internal")]
		private static extern void webviewDestroy (string _tag);
		
		[DllImport("__Internal")]
		private static extern void webviewShow (string _tag);
		
		[DllImport("__Internal")]
		private static extern void webviewHide (string _tag);
		
		[DllImport("__Internal")]
		private static extern void webviewLoadRequest (string _URL, string _tag);
		
		[DllImport("__Internal")]
		private static extern void webviewLoadHTMLString (string _HTMLString, string _baseURL, string _tag);
		
		[DllImport("__Internal")]
		private static extern void webviewLoadData (byte[] _dataArray, int _dataArrayLength, string _MIMEType, string _textEncodingName, string _baseURL, string _tag);

		[DllImport("__Internal")]
		private static extern void webviewEvaluateJavaScriptFromString (string _javaScript, string _tag);
		
		[DllImport("__Internal")]
		private static extern void webviewStopLoading (string _tag);
		
		[DllImport("__Internal")]
		private static extern void webviewReload (string _tag);

		[DllImport("__Internal")]
		private static extern void webviewSetCanHide (bool _canHide, string _tag);
		
		[DllImport("__Internal")]
		private static extern void webviewSetCanBounce (bool _canBounce, string _tag);
		
		[DllImport("__Internal")]
		private static extern void webviewSetControlType (int _type, string _tag);

		[DllImport("__Internal")]
		private static extern void webviewSetShowSpinnerOnLoad (bool _showSpinner,	string _tag);

		[DllImport("__Internal")]
		private static extern void webviewSetAutoShowOnLoadFinish (bool _autoShow, string _tag);
		
		[DllImport("__Internal")]
		private static extern void webviewSetScalesPageToFit (bool _scaleToFit, string _tag);

		[DllImport("__Internal")]
		private static extern void webviewSetNormalisedFrame (NormalisedRect _noramlisedRect, string _tag);
		
		[DllImport("__Internal")]
		private static extern void webviewSetBackgroundColor (float _r, float _g, float _b, float _alpha, string _tag);
		
		[DllImport("__Internal")]
		private static extern void webviewAddNewURLScheme (string _newURLScheme, string _tag);
		
		[DllImport("__Internal")]
		private static extern void webviewClearCache ();

		#endregion

		#region Lifecycle Methods

		public void Create (string _tag, Rect _frame)
		{
			webviewCreate(_tag, new NormalisedRect(_frame));
		}
		
		public void Destroy (string _tag)
		{
			webviewDestroy(_tag);
		}
		
		public void Show (string _tag)
		{
			webviewShow(_tag);
		}
		
		public void Hide (string _tag)
		{
			webviewHide(_tag);
		}

		#endregion

		#region Load Methods

		public void LoadRequest (string _tag, string _URL)
		{
			webviewLoadRequest(_URL, _tag);
		}

		public void LoadHTMLString (string _tag, string _HTMLString, string _baseURL)
		{
			webviewLoadHTMLString(_HTMLString, _baseURL, _tag);
		}
		
		public void LoadData (string _tag, byte[] _byteArray, string _MIMEType, string _textEncodingName, string _baseURL)
		{
			webviewLoadData(_byteArray, _byteArray.Length, _MIMEType, _textEncodingName, _baseURL, _tag);
		}

		public void EvaluateJavaScriptFromString (string _tag, string _javaScript)
		{
			webviewEvaluateJavaScriptFromString(_javaScript, _tag);
		}
		
		public void Reload (string _tag)
		{
			webviewReload(_tag);
		}
		
		public void StopLoading (string _tag)
		{
			webviewStopLoading(_tag);
		}

		#endregion

		#region Property Modifier Methods

		public void SetCanHide (string _tag, bool _canHide)
		{
			webviewSetCanHide(_canHide, _tag);
		}
		
		public void SetCanBounce (string _tag, bool _canBounce)
		{
			webviewSetCanBounce(_canBounce, _tag);
		}

		public void SetControlType (string _tag, eWebviewControlType _type)
		{
			webviewSetControlType((int)_type, _tag);
		}
	
		public void SetShowSpinnerOnLoad (string _tag, bool _showSpinner)
		{
			webviewSetShowSpinnerOnLoad(_showSpinner, _tag);
		}
		
		public void SetAutoShowOnLoadFinish (string _tag, bool _autoShow)
		{
			webviewSetAutoShowOnLoadFinish(_autoShow, _tag);
		}
		
		public void SetScalesPageToFit (string _tag, bool _scaleToFit)
		{
			webviewSetScalesPageToFit(_scaleToFit, _tag);
		}

		public void SetFrame (string _tag, Rect _frame)
		{
			webviewSetNormalisedFrame(new NormalisedRect(_frame), _tag);
		}

		public void SetBackgroundColor (string _tag, Color _color)
		{
			webviewSetBackgroundColor(_color.r, _color.g, _color.b, _color.a, _tag);
		}

		#endregion

		#region Communication Methods
		
		public void AddNewURLSchemeName (string _tag, string _newURLSchemeName)
		{
			webviewAddNewURLScheme(_newURLSchemeName, _tag);
		}

		#endregion
		
		#region Cache Methods

		public void ClearCache ()
		{
			webviewClearCache();
		}

		#endregion

		#region Nested Types

		private struct NormalisedRect
		{
			#region Properties

			public 	float 	x;
			public 	float 	y;
			public 	float 	width;
			public 	float 	height;

			#endregion

			#region Constructor

			public NormalisedRect (Rect _rect) : this ()
			{
				this.x		= (_rect.x / (float)Screen.width);
				this.y		= (_rect.y / (float)Screen.height);
				this.width	= (_rect.width / (float)Screen.width);
				this.height	= (_rect.height / (float)Screen.height);
			}

			#endregion
		}

		#endregion
	}
}
#endif
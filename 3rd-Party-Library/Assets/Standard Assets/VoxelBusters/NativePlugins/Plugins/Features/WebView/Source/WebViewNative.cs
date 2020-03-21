#if USES_WEBVIEW
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace VoxelBusters.NativePlugins.Internal
{
	public partial class WebViewNative : MonoBehaviour 
	{
		#region Fields

		private Dictionary<string, WebView>	m_webviewCollection	= new Dictionary<string, WebView>();

		#endregion

		#region Properties

		public IWebViewPlatform Platform
		{
			get;
			private set;
		}

		#endregion

		#region Unity Methods

		private void Awake ()
		{
			// Set properties
			Platform	= CreateNativeBindingObject();
		}

		#endregion

		#region Public Methods
		
		public void Create (WebView _webview, Rect _frame)
		{
			AddWebViewToCollection(_webview.UniqueID, _webview);

			Platform.Create(_webview.UniqueID, _frame);
		}
		
		#endregion
		
		#region Private Methods

		private IWebViewPlatform CreateNativeBindingObject ()
		{
#if UNITY_EDITOR
			return new WebViewNativeUnsupported();
#elif UNITY_IOS
			return new WebViewNativeIOS();
#elif UNITY_ANDROID
			return new WebViewNativeAndroid();
#else
			return new WebViewNativeUnsupported();
#endif
		}
		
		private WebView GetWebViewWithTag (string _tag)
		{
			WebView	_webView;
			m_webviewCollection.TryGetValue(_tag, out _webView);
			
			return _webView;
		}
		
		private void AddWebViewToCollection (string _tag, WebView _webview)
		{
			m_webviewCollection[_tag]	= _webview;
		}
		
		private void RemoveWebViewFromCollection (string _tag)
		{
			if (m_webviewCollection.ContainsKey(_tag))
				m_webviewCollection.Remove(_tag);
		}
		
		#endregion
	}
}
#endif
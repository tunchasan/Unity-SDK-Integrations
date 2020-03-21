#if USES_WEBVIEW && UNITY_ANDROID
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace VoxelBusters.NativePlugins.Internal
{
	public sealed partial class WebViewNativeAndroid : IWebViewPlatform
	{
		#region Fields

		private Dictionary<eWebviewControlType,string> m_controlTypes = null;

		private Dictionary<eWebviewControlType,string>  ControlTypes
		{
			get
			{
				if(m_controlTypes == null)
				{
					m_controlTypes = new Dictionary<eWebviewControlType,string>();
					m_controlTypes.Add(eWebviewControlType.NO_CONTROLS, 	"NONE");
					m_controlTypes.Add(eWebviewControlType.CLOSE_BUTTON, 	"CLOSE_OPTION");
					m_controlTypes.Add(eWebviewControlType.TOOLBAR, 		"TOOLBAR_OPTION");
				}
				return m_controlTypes;
			}
		}

		#endregion

		#region Constructors

		public WebViewNativeAndroid ()
		{
			Plugin = AndroidPluginUtility.GetSingletonInstance(NativeInfo.Class.NAME);
		}

		#endregion

		#region Lifecycle Methods

		public void Create (string _tag, Rect _frame)
		{
			Plugin.Call(NativeInfo.Methods.CREATE_WEB_VIEW, _tag);
            SetFrame(_tag, _frame);
		}

		public void Destroy (string _tag)
		{
			Plugin.Call(NativeInfo.Methods.DESTROY_WEB_VIEW,_tag);
		}

		public void Show (string _tag)
		{
			Plugin.Call(NativeInfo.Methods.SHOW_WEB_VIEW,_tag);
		}

		public void Hide (string _tag)
		{
			Plugin.Call(NativeInfo.Methods.HIDE_WEB_VIEW,_tag);
		}

		#endregion

		#region Load Methods

		public void LoadRequest (string _tag, string _URL)
		{
			Plugin.Call(NativeInfo.Methods.LOAD_REQUEST, _URL, _tag);
		}

		public void LoadHTMLString (string _tag, string _HTMLString, string _baseURL)
		{
			Plugin.Call(NativeInfo.Methods.LOAD_HTML_STRING, _HTMLString, _baseURL, _tag);
		}

		public void LoadData (string _tag, byte[] _byteArray, string _MIMEType, string _textEncodingName, string _baseURL)
		{
			Plugin.Call(NativeInfo.Methods.LOAD_DATA, _byteArray, _byteArray.Length, _MIMEType, _textEncodingName, _baseURL,	_tag);
		}

		public void EvaluateJavaScriptFromString (string _tag, string _javaScript)
		{
			Plugin.Call(NativeInfo.Methods.EVALUATE_JS_FROM_STRING, _javaScript, _tag);
		}

		public void Reload (string _tag)
		{
			Plugin.Call(NativeInfo.Methods.RELOAD_WEB_VIEW, _tag);
		}

		public void StopLoading (string _tag)
		{
			Plugin.Call(NativeInfo.Methods.STOP_LOADING_WEB_VIEW, _tag);
		}

		#endregion

		#region Property Access Methods

		public void SetCanHide (string _tag, bool _canHide)
		{
			Plugin.Call(NativeInfo.Methods.SET_CAN_HIDE, _canHide, _tag);
		}

		public void SetCanBounce (string _tag, bool _canBounce)
		{
			Plugin.Call(NativeInfo.Methods.SET_CAN_BOUNCE, _canBounce, _tag);
		}

		public void SetControlType (string _tag, eWebviewControlType _type)
		{
			Plugin.Call(NativeInfo.Methods.SET_CONTROL_TYPE, ControlTypes[_type], _tag);
		}

		public void SetShowSpinnerOnLoad (string _tag, bool _showSpinner)
		{
			Plugin.Call(NativeInfo.Methods.SET_SHOW_LOADING_SPINNER, _showSpinner, _tag);
		}

		public void SetAutoShowOnLoadFinish (string _tag, bool _autoShow)
		{
			Plugin.Call(NativeInfo.Methods.SET_AUTO_SHOW_WHEN_LOAD_COMPLETE, _autoShow, _tag);
		}

		public void SetScalesPageToFit (string _tag, bool _scaleToFit)
		{
			Plugin.Call(NativeInfo.Methods.SET_SCALES_PAGE_TO_FIT, _scaleToFit, _tag);
		}

		public void SetFrame (string _tag, Rect _frame)
		{
			Plugin.Call(NativeInfo.Methods.SET_FRAME, _frame.x/Screen.width, _frame.y/Screen.height, _frame.width/Screen.width, _frame.height/Screen.height , _tag);

			if (NPSettings.WebViewSettings.Android.UseNewActivityForFullScreenWebView)
			{
				bool _isFullScreen	= ((int)_frame.width == Screen.width) && ((int)_frame.height == Screen.height);
				Plugin.Call (NativeInfo.Methods.SET_FULL_SCREEN_MODE, _tag, _isFullScreen);
			}
		}

		public void SetBackgroundColor (string _tag, Color _color)
		{
			Plugin.Call(NativeInfo.Methods.SET_BACKGROUND_COLOR, _color.r, _color.g, _color.b, _color.a, _tag);
		}

		#endregion

		#region URL Scheme

		public void AddNewURLSchemeName (string _tag, string _newURLSchemeName)
		{
			Plugin.Call(NativeInfo.Methods.ADD_NEW_SCHEME, _newURLSchemeName, _tag);
		}

		#endregion

		#region Cache Clearance Methods

		public void ClearCache ()
		{
			Plugin.Call(NativeInfo.Methods.CLEAR_CACHE);
		}

		#endregion
	}
}
#endif

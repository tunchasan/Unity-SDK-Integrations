#if USES_WEBVIEW && UNITY_ANDROID
using UnityEngine;
using System.Collections;

namespace VoxelBusters.NativePlugins.Internal
{
	public sealed partial class WebViewNativeAndroid : IWebViewPlatform
	{
		#region Platform Native Info
		
		private class NativeInfo
		{
			// Handler class name
			internal class Class
			{
				internal const string NAME									= "com.voxelbusters.nativeplugins.features.webview.WebViewHandler";
			}
			
			// For holding method names
			internal class Methods
			{
				internal const string CREATE_WEB_VIEW		 				= "createNativeWebView";
				internal const string DESTROY_WEB_VIEW		 				= "destoryWebViewWithTag";
				internal const string SHOW_WEB_VIEW		 					= "showWebViewWithTag";
				internal const string HIDE_WEB_VIEW 						= "hideWebViewWithTag";
				internal const string LOAD_REQUEST 							= "loadRequest";
				internal const string LOAD_HTML_STRING 						= "loadHTMLString";
				internal const string LOAD_DATA 							= "loadData";
				internal const string EVALUATE_JS_FROM_STRING 				= "evaluateJavaScriptFromString";
				internal const string RELOAD_WEB_VIEW 						= "reloadWebViewWithTag";
				internal const string STOP_LOADING_WEB_VIEW 				= "stopLoadingWebViewWithTag";
				internal const string SET_CAN_HIDE							= "setCanHide";
				internal const string SET_CAN_BOUNCE						= "setCanBounce";
				internal const string SET_CONTROL_TYPE						= "setControlType";
				internal const string SET_SHOW_TOOLBAR 						= "setShowToolBar";
				internal const string SET_SHOW_LOADING_SPINNER 				= "setShowLoadingSpinner";
				internal const string SET_AUTO_SHOW_WHEN_LOAD_COMPLETE 		= "setAutoShowWhenLoadComplete";
				internal const string SET_SCALES_PAGE_TO_FIT 				= "setScalesPageToFit";
				internal const string SET_FRAME 							= "setFrame";
				internal const string SET_BACKGROUND_COLOR 					= "setBackgroundColor";
				internal const string SET_ALLOW_MEDIA_PLAYBACK 				= "setAllowMediaPlayback";
				internal const string ADD_NEW_SCHEME 						= "addNewScheme";
				internal const string CLEAR_CACHE 							= "clearCache";
				internal const string SET_FULL_SCREEN_MODE					= "setFullScreenMode";
			}
		}
		
		#endregion

		#region  Native Access Variables
		
		private AndroidJavaObject  	Plugin
		{
			get;
			set;
		}
		
		#endregion
	}
}
#endif
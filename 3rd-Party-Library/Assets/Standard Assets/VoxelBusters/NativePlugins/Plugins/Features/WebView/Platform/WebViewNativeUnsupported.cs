#if USES_WEBVIEW
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using VoxelBusters.UASUtils;

namespace VoxelBusters.NativePlugins.Internal
{
	public sealed partial class WebViewNativeUnsupported : IWebViewPlatform 
	{
		#region Lifecycle Methods
		
		public void Create (string _tag, Rect _frame)
		{
			DebugUtility.Logger.LogWarning(Constants.kDebugTag, Constants.kNotSupportedInEditor);
		}
		
		public void Destroy (string _tag)
		{
			DebugUtility.Logger.LogWarning(Constants.kDebugTag, Constants.kNotSupportedInEditor);
		}
		
		public void Show (string _tag)
		{
			DebugUtility.Logger.LogWarning(Constants.kDebugTag, Constants.kNotSupportedInEditor);
		}
		
		public void Hide (string _tag)
		{
			DebugUtility.Logger.LogWarning(Constants.kDebugTag, Constants.kNotSupportedInEditor);
		}
		
		#endregion
		
		#region Load Methods
		
		public void LoadRequest (string _tag, string _URL)
		{
			DebugUtility.Logger.LogWarning(Constants.kDebugTag, Constants.kNotSupportedInEditor);
		}
		
		public void LoadHTMLString (string _tag, string _HTMLString, string _baseURL)
		{
			DebugUtility.Logger.LogWarning(Constants.kDebugTag, Constants.kNotSupportedInEditor);
		}
		
		public void LoadData (string _tag, byte[] _byteArray, string _MIMEType, string _textEncodingName, string _baseURL)
		{
			DebugUtility.Logger.LogWarning(Constants.kDebugTag, Constants.kNotSupportedInEditor);
		}
		
		public void EvaluateJavaScriptFromString (string _tag, string _javaScript)
		{
			DebugUtility.Logger.LogWarning(Constants.kDebugTag, Constants.kNotSupportedInEditor);
		}
		
		public void Reload (string _tag)
		{
			DebugUtility.Logger.LogWarning(Constants.kDebugTag, Constants.kNotSupportedInEditor);
		}
		
		public void StopLoading (string _tag)
		{
			DebugUtility.Logger.LogWarning(Constants.kDebugTag, Constants.kNotSupportedInEditor);
		}
		
		#endregion
		
		#region Property Modifier Methods
		
		public void SetCanHide (string _tag, bool _canHide)
		{
			DebugUtility.Logger.LogWarning(Constants.kDebugTag, Constants.kNotSupportedInEditor);
		}
		
		public void SetCanBounce (string _tag, bool _canBounce)
		{
			DebugUtility.Logger.LogWarning(Constants.kDebugTag, Constants.kNotSupportedInEditor);
		}
		
		public void SetControlType (string _tag, eWebviewControlType _type)
		{
			DebugUtility.Logger.LogWarning(Constants.kDebugTag, Constants.kNotSupportedInEditor);
		}
		
		public void SetShowSpinnerOnLoad (string _tag, bool _showSpinner)
		{
			DebugUtility.Logger.LogWarning(Constants.kDebugTag, Constants.kNotSupportedInEditor);
		}
		
		public void SetAutoShowOnLoadFinish (string _tag, bool _autoShow)
		{
			DebugUtility.Logger.LogWarning(Constants.kDebugTag, Constants.kNotSupportedInEditor);
		}
		
		public void SetScalesPageToFit (string _tag, bool _scaleToFit)
		{
			DebugUtility.Logger.LogWarning(Constants.kDebugTag, Constants.kNotSupportedInEditor);
		}
		
		public void SetFrame (string _tag, Rect _frame)
		{
			DebugUtility.Logger.LogWarning(Constants.kDebugTag, Constants.kNotSupportedInEditor);
		}
		
		public void SetBackgroundColor (string _tag, Color _color)
		{
			DebugUtility.Logger.LogWarning(Constants.kDebugTag, Constants.kNotSupportedInEditor);
		}
		
		#endregion
		
		#region Communication Methods

		public void AddNewURLSchemeName (string _tag, string _newURLSchemeName)
		{
			DebugUtility.Logger.LogWarning(Constants.kDebugTag, Constants.kNotSupportedInEditor);
		}
		
		#endregion
		
		#region Cache Methods
		
		public void ClearCache ()
		{
			DebugUtility.Logger.LogWarning(Constants.kDebugTag, Constants.kNotSupportedInEditor);
		}
		
		#endregion
	}
}
#endif
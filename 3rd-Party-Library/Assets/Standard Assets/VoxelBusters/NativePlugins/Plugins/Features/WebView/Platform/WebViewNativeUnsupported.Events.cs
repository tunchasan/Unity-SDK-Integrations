#if USES_WEBVIEW
using UnityEngine;
using System.Collections;

namespace VoxelBusters.NativePlugins.Internal
{
	public sealed partial class WebViewNativeUnsupported : IWebViewPlatform 
	{
		#region Parse Methods

		public void ParseLoadFinishedEventData (IDictionary _JSONDict, out string _tag, out string _URL)
		{
			// Set default values
			_tag		= null;
			_URL		= null;
		}

		public void ParseLoadFailedEventData (IDictionary _JSONDict, out string _tag, out string _URL, out string _error)
		{
			// Set default values
			_tag		= null;
			_URL		= null;
			_error		= null;
		}

		public void ParseEvalJSEventData (IDictionary _JSONDict, out string _tag, out string _result)
		{
			// Set default values
			_tag		= null;
			_result		= null;
		}

		public void ParseMessageReceivedEventData (IDictionary _JSONDict, out string _tag, out WebViewMessage _message)
		{
			// Set default values
			_tag		= null;
			_message	= null;
		}

		#endregion
	}
}
#endif
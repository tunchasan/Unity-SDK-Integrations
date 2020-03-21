#if USES_WEBVIEW && UNITY_IOS
using UnityEngine;
using System.Collections;
using VoxelBusters.Utility;

namespace VoxelBusters.NativePlugins.Internal
{
	public sealed partial class WebViewNativeIOS : IWebViewPlatform
	{
		#region Constants

		private		const 	string		kTagKey		= "tag";
		private		const 	string		kURLKey		= "url";
		private		const 	string		kErrorKey	= "error";
		private		const 	string		kResultKey	= "result";
		private		const 	string		kMessageKey	= "message-data";

		#endregion

		#region Parse Methods

		public void ParseLoadFinishedEventData (IDictionary _JSONDict, out string _tag, out string _URL)
		{
			_tag		= _JSONDict[kTagKey] as string;
			_URL		= _JSONDict.GetIfAvailable<string>(kURLKey);
		}

		public void ParseLoadFailedEventData (IDictionary _JSONDict, out string _tag, out string _URL, out string _error)
		{
			_tag		= _JSONDict[kTagKey] as string;
			_URL		= _JSONDict.GetIfAvailable<string>(kURLKey);
			_error		= _JSONDict.GetIfAvailable<string>(kErrorKey);
		}
		
		public void ParseEvalJSEventData (IDictionary _JSONDict, out string _tag, out string _result)
		{
			_tag		= _JSONDict[kTagKey] as string;
			_result		= _JSONDict.GetIfAvailable<string>(kResultKey);
		}
		
		public void ParseMessageReceivedEventData (IDictionary _JSONDict, out string _tag, out WebViewMessage _message)
		{
			_tag		= _JSONDict[kTagKey] as string;
			_message	= new iOSWebViewMessage(_JSONDict[kMessageKey] as IDictionary);
		}
	
		#endregion
	}
}
#endif
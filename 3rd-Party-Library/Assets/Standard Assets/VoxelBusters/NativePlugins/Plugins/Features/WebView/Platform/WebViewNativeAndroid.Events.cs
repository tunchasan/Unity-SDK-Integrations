#if USES_WEBVIEW && UNITY_ANDROID
using UnityEngine;
using System.Collections;
using VoxelBusters.Utility;

namespace VoxelBusters.NativePlugins.Internal
{
	public sealed partial class WebViewNativeAndroid : IWebViewPlatform
	{
		#region Parse Constants

		private const string	kTag			= "tag";
		private const string	kURL			= "url";
		private const string	kError			= "error";
		private const string	kResult			= "result";
		private const string	kMessageData	= "message-data";
					
		#endregion

		#region Parse Methods
		
		public void ParseLoadFinishedEventData (IDictionary _JSONDict, out string _tag, out string _URL)
		{
			_tag		= _JSONDict[kTag] as string;
			_URL		= _JSONDict.GetIfAvailable<string>(kURL);
		}
		
		public void ParseLoadFailedEventData (IDictionary _JSONDict, out string _tag, out string _URL, out string _error)
		{
			_tag		= _JSONDict[kTag] as string;
			_URL		= _JSONDict.GetIfAvailable<string>(kURL);
			_error		= _JSONDict.GetIfAvailable<string>(kError);
		}
		
		public void ParseEvalJSEventData (IDictionary _JSONDict, out string _tag, out string _result)
		{
			_tag		= _JSONDict[kTag] as string;
			_result		= _JSONDict[kResult] as string;
		}

		public void ParseMessageReceivedEventData (IDictionary _JSONDict, out string _tag, out WebViewMessage _message)
		{
			_tag		= _JSONDict[kTag] as string;
			_message	= new AndroidWebViewMessage(_JSONDict[kMessageData] as IDictionary);
		}
	
		#endregion
	}
}
#endif
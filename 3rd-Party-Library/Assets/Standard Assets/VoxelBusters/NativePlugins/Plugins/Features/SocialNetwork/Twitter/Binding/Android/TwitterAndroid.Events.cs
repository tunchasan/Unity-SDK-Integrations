using UnityEngine;
using System.Collections;

#if USES_TWITTER && UNITY_ANDROID
using System.Collections.Generic;

namespace VoxelBusters.NativePlugins
{
	using Internal;

	public partial class TwitterAndroid : Twitter 
	{
		#region Parse constants

		private const string	kComposerDone		= "done";
		private const string	kComposerCancelled	= "cancelled";

		private static Dictionary<string, eTwitterComposerResult> kComposerResultParseMap = new Dictionary<string, eTwitterComposerResult>()
		{
			{kComposerDone, eTwitterComposerResult.DONE},
			{kComposerCancelled, eTwitterComposerResult.CANCELLED}	
		};

		#endregion

		#region Parse Methods

		protected override void ParseSessionData (IDictionary _sessionDict, out TwitterSession _session)
		{
			_session	= new AndroidTwitterSession(_sessionDict);
		}
		
		protected override void ParseTweetComposerDismissedData (string _resultStr, out eTwitterComposerResult _result)
		{
			_result		= kComposerResultParseMap[_resultStr];
		}
		
		protected override void ParseUserData (IDictionary _userDict, out TwitterUser _user)
		{
			_user		= new AndroidTwitterUser(_userDict);
		}
		
		#endregion
	}
}
#endif
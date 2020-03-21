using UnityEngine;
using System.Collections;

#if USES_TWITTER && UNITY_IOS
namespace VoxelBusters.NativePlugins
{
	using Internal;

	public partial class TwitterIOS : Twitter 
	{
		private enum TWTRComposerResult
		{
			TWTRComposerResultCancelled,
			TWTRComposerResultDone
		}

		#region Parse Methods
		
		protected override void ParseSessionData (IDictionary _sessionDict, out TwitterSession _session)
		{
			_session	= new iOSTwitterSession(_sessionDict);
		}
		
		protected override void ParseTweetComposerDismissedData (string _resultStr, out eTwitterComposerResult _result)
		{
			TWTRComposerResult _iOSResult	= (TWTRComposerResult)int.Parse(_resultStr);

			// Set result
			_result		= ConvertComposerResultToUnityFormat(_iOSResult);
		}
		
		protected override void ParseUserData (IDictionary _userDict, out TwitterUser _user)
		{
			_user		= new iOSTwitterUser(_userDict);
		}
		
		#endregion

		#region Misc. Methods

		private eTwitterComposerResult ConvertComposerResultToUnityFormat (TWTRComposerResult _iOSResult)
		{
			switch (_iOSResult)
			{
			case TWTRComposerResult.TWTRComposerResultCancelled:
				return eTwitterComposerResult.CANCELLED;
				
			case TWTRComposerResult.TWTRComposerResultDone:
				return eTwitterComposerResult.DONE;
			}

			return eTwitterComposerResult.CANCELLED;
		}

		#endregion
	}
}
#endif
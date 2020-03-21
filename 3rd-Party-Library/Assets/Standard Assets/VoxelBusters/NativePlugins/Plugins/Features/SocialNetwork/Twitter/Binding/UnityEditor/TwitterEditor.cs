#if USES_TWITTER && UNITY_EDITOR
using UnityEngine;
using System.Collections;
using VoxelBusters.UASUtils;

namespace VoxelBusters.NativePlugins
{
	using Internal;

	public class TwitterEditor : Twitter 
	{
		#region Init API's

		public override bool Initialise ()
		{
			base.Initialise();

			return false;
		}

		#endregion

		#region Account API's
		
		public override void Login (bool _requiresEmailAccess, TWTRLoginCompletion _onCompletion)
		{
			base.Login(_requiresEmailAccess, _onCompletion);

			TwitterLoginFailed(Constants.kNotSupportedInEditor);
		}
		
		public override void Logout ()
		{
			base.Logout();
			DebugUtility.Logger.LogError(Constants.kDebugTag, Constants.kNotSupportedInEditor);
			TwitterLogoutFinished();
		}
		
		public override bool IsLoggedIn ()
		{
			DebugUtility.Logger.LogError(Constants.kDebugTag, Constants.kNotSupportedInEditor);
			return base.IsLoggedIn();
		}

		#endregion

		#region Tweet API's
		
		public override void ShowTweetComposer (string _message, string _URL, byte[] _imgByteArray, TWTRTweetCompletion _onCompletion)
		{
			base.ShowTweetComposer(_message, _URL, _imgByteArray, _onCompletion);

			DebugUtility.Logger.LogError(Constants.kDebugTag, Constants.kNotSupportedInEditor);

			TweetComposerDismissed(((int)eTwitterComposerResult.CANCELLED).ToString());
		}
		
		#endregion
		
		#region Request API's
		
		public override void RequestAccountDetails (TWTRAccountDetailsCompletion _onCompletion)
		{			
			base.RequestAccountDetails(_onCompletion);

			RequestAccountDetailsFailed(Constants.kNotSupportedInEditor);
		}
		
		public override void RequestEmailAccess (TWTREmailAccessCompletion _onCompletion)
		{
			base.RequestEmailAccess(_onCompletion);

			RequestEmailAccessFailed(Constants.kNotSupportedInEditor);
		}
		
		protected override void SendURLRequest (string _methodType, string _URL, IDictionary _parameters, TWTRResponse _onCompletion)
		{			
			base.SendURLRequest(_methodType, _URL, _parameters, _onCompletion);

			TwitterURLRequestFailed(Constants.kNotSupportedInEditor);
		}
		
		#endregion
	}
}
#endif
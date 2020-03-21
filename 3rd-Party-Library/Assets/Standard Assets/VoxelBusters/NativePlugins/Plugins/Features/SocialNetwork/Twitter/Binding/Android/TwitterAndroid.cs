#if USES_TWITTER && UNITY_ANDROID
using UnityEngine;
using System.Collections;
using VoxelBusters.Utility;
using VoxelBusters.UASUtils;

namespace VoxelBusters.NativePlugins
{
	using Internal;

	public partial class TwitterAndroid : Twitter 
	{	
		#region Constructors
		
		TwitterAndroid()
		{
			Plugin = AndroidPluginUtility.GetSingletonInstance(Native.Class.NAME);
		}
		
		#endregion

		#region Init API's

		public override bool Initialise ()
		{
			if (base.Initialise())
			{
				TwitterSettings _twitterSettings	= NPSettings.SocialNetworkSettings.TwitterSettings;

				Plugin.Call(Native.Methods.INITIALIZE, _twitterSettings.ConsumerKey, _twitterSettings.ConsumerSecret);
				return true;
			}
			

			return false;
		}

		#endregion

		#region Account API's
			
		public override void Login (bool _requiresEmailAccess, TWTRLoginCompletion _onCompletion)
		{
			if (IsInitialised())
			{
				base.Login(_requiresEmailAccess, _onCompletion);

				// Native method is called
				Plugin.Call(Native.Methods.LOGIN);
			}
			else
			{
				DebugUtility.Logger.LogError(Constants.kDebugTag, "Twitter is not yet initialised! Call Initialise method first.");
			}
		}
		
		public override void Logout ()
		{
			if (!IsLoggedIn())
			{
				DebugUtility.Logger.LogWarning(Constants.kDebugTag, "Already logged out.");
				return;
			}

			base.Logout();

			// Native method is called
			Plugin.Call(Native.Methods.LOGOUT);
			TwitterLogoutFinished();
		}
		
		public override bool IsLoggedIn ()
		{
			bool _isLoggedIn	= Plugin.Call<bool>(Native.Methods.IS_LOGGED_IN);
			DebugUtility.Logger.Log(Constants.kDebugTag, "[Twitter] IsLoggedIn=" + _isLoggedIn);
			
			return _isLoggedIn;
		}

		public override TwitterAuthSession GetSessionWithUserID (string _userID = null)
		{
			string _sessionJSONString	= Plugin.Call<string>(Native.Methods.GET_AUTH_SESSION, _userID);
			if (string.IsNullOrEmpty(_sessionJSONString))
				return null;
			
			return new AndroidTwitterSession((IDictionary)JSONUtility.FromJSON(_sessionJSONString));
		}

		#endregion

		#region Tweet API's
		
		public override void ShowTweetComposer (string _message, string _URL, byte[] _imgByteArray, TWTRTweetCompletion _onCompletion)
		{
			base.ShowTweetComposer(_message, _URL, _imgByteArray, _onCompletion);

			// Get byte array length
			int _arrayLength	= (_imgByteArray == null) ? 0 : _imgByteArray.Length;
			
			// Native method is called
			Plugin.Call(Native.Methods.SHOW_TWEET_COMPOSER, _message, _URL, _imgByteArray, _arrayLength);
		}
		
		#endregion
		
		#region Request API's
		
		public override void RequestAccountDetails (TWTRAccountDetailsCompletion _onCompletion)
		{
			base.RequestAccountDetails(_onCompletion);

			// Native method is called
			Plugin.Call(Native.Methods.REQUEST_ACCOUNT_DETAILS);
		}
		
		public override void RequestEmailAccess (TWTREmailAccessCompletion _onCompletion)
		{
			base.RequestEmailAccess(_onCompletion);

			// Native method is called
			Plugin.Call(Native.Methods.REQUEST_EMAIL_ACCESS);
		}
		
		protected override void SendURLRequest (string _methodType, string _URL, IDictionary _parameters, TWTRResponse _onCompletion)
		{
			base.SendURLRequest(_methodType, _URL, _parameters, _onCompletion);

			// Native method is called
			Plugin.Call(Native.Methods.URL_REQUEST, _methodType, _URL, _parameters.ToJSON());
		}
		
		#endregion
	}
}
#endif
#if USES_TWITTER && UNITY_IOS
using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using VoxelBusters.Utility;
using VoxelBusters.UASUtils;

namespace VoxelBusters.NativePlugins
{
	using Internal;

	public partial class TwitterIOS : Twitter
	{
		#region Native Methods

		[DllImport("__Internal")]
		private static extern void cpnpTwitterInitTwitterKit (string _consumerKey, string _consumerSecret);
		
		[DllImport("__Internal")]
		private static extern void cpnpTwitterLogin (bool _requiresEmailAccess);

		[DllImport("__Internal")]
		private static extern void cpnpTwitterLogoutUserID (string _userID);

		[DllImport("__Internal")]
		private static extern bool cpnpTwitterIsUserLoggedIn (string _userID);
		
		[DllImport("__Internal")]
		private static extern string cpnpTwitterGetSessionDictionaryWithUserID (string _userID);
		
		[DllImport("__Internal")]
		private static extern void cpnpTwitterShowTweetComposer (string _message, string _URLString, byte[] _imgByteArray, int _imgByteArrayLength);
		
		[DllImport("__Internal")]
		private static extern void cpnpTwitterRequestAccountDetailsWithUserID (string _userID);
		
		[DllImport("__Internal")]
		private static extern void cpnpTwitterRequestEmailWithUserID (string _userID);
		
		[DllImport("__Internal")]
		private static extern void cpnpTwitterSendURLRequest (string _userID, string _methodType, string _URLString, string _parameters);

		#endregion

		#region Init API's

		public override bool Initialise ()
		{
			if (base.Initialise())
			{
				TwitterSettings _twitterSettings	= NPSettings.SocialNetworkSettings.TwitterSettings;
				cpnpTwitterInitTwitterKit(_twitterSettings.ConsumerKey, _twitterSettings.ConsumerSecret);

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

				cpnpTwitterLogin(_requiresEmailAccess);
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

			cpnpTwitterLogoutUserID(m_activeSessionUserID);
			TwitterLogoutFinished();
		}
		
		public override bool IsLoggedIn ()
		{
			bool _isLoggedIn	= cpnpTwitterIsUserLoggedIn(m_activeSessionUserID);
			DebugUtility.Logger.Log(Constants.kDebugTag, "[Twitter] IsLoggedIn=" + _isLoggedIn);
			
			return _isLoggedIn;
		}
		
		public override TwitterAuthSession GetSessionWithUserID (string _userID)
		{	
			string _sessionJSONString	= cpnpTwitterGetSessionDictionaryWithUserID(_userID);
			if (_sessionJSONString == null)
				return null;

			return new iOSTwitterAuthSession((IDictionary)JSONUtility.FromJSON(_sessionJSONString));
		}

		#endregion

		#region Tweet API's

		public override void ShowTweetComposer (string _message, string _URL, byte[] _imgByteArray, TWTRTweetCompletion _onCompletion)
		{
			base.ShowTweetComposer(_message, _URL, _imgByteArray, _onCompletion);

			// Get byte array length
			int _arrayLength	= 0;
			if (_imgByteArray != null)
				_arrayLength	= _imgByteArray.Length;
			
			cpnpTwitterShowTweetComposer(_message, _URL, _imgByteArray, _arrayLength);
		}
		
		#endregion
		
		#region Request API's
		
		public override void RequestAccountDetails (TWTRAccountDetailsCompletion _onCompletion)
		{
			base.RequestAccountDetails(_onCompletion);

			cpnpTwitterRequestAccountDetailsWithUserID(m_activeSessionUserID);
		}
		
		public override void RequestEmailAccess (TWTREmailAccessCompletion _onCompletion)
		{
			base.RequestEmailAccess(_onCompletion);

			cpnpTwitterRequestEmailWithUserID(m_activeSessionUserID);
		}
		
		protected override void SendURLRequest (string _methodType, string _URL, IDictionary _parameters, TWTRResponse _onCompletion)
		{
			base.SendURLRequest(_methodType, _URL, _parameters, _onCompletion);

			cpnpTwitterSendURLRequest(m_activeSessionUserID, _methodType, _URL, _parameters.ToJSON());
		}
		
		#endregion
	}
}
#endif
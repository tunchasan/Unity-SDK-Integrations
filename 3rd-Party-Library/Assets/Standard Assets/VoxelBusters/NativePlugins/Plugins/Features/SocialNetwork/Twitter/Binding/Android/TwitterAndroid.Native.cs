using UnityEngine;
using System.Collections;

#if USES_TWITTER && UNITY_ANDROID
using System.Collections.Generic;

namespace VoxelBusters.NativePlugins
{
	using Internal;

	public partial class TwitterAndroid : Twitter 
	{
		#region Platform Native Info
		
		private class Native
		{
			// Handler class name
			internal class Class
			{
				internal const string NAME									= "com.voxelbusters.nativeplugins.features.socialnetwork.twitter.TwitterHandler";
			}
			
			// For holding method names
			internal class Methods
			{
				internal const string INITIALIZE		 					= "initialize";
				internal const string LOGIN		 							= "login";
				internal const string LOGOUT		 						= "logout";
				internal const string IS_LOGGED_IN		 					= "isLoggedIn";
				internal const string GET_AUTH_SESSION		 				= "getAuthSession";
				internal const string GET_AUTH_TOKEN		 				= "getAuthToken";
				internal const string GET_AUTH_TOKEN_SECRET		 			= "getAuthTokenSecret";
				internal const string GET_USER_ID		 					= "getUserId";
				internal const string GET_USER_NAME		 					= "getUserName";
				internal const string SHOW_TWEET_COMPOSER					= "showTweetComposer";
				internal const string REQUEST_ACCOUNT_DETAILS 				= "requestAccountDetails";
				internal const string REQUEST_EMAIL_ACCESS 					= "requestEmailAccess";
				internal const string URL_REQUEST 							= "urlRequest";
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
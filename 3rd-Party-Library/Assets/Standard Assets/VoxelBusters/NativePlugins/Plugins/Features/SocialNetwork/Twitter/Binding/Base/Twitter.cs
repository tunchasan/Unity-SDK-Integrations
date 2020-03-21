#if USES_TWITTER
using UnityEngine;
using System.Collections;
using VoxelBusters.Utility;
using VoxelBusters.UASUtils;

namespace VoxelBusters.NativePlugins
{
	using Internal;

	/// <summary>
	/// Provides cross-platform interface to Twitter login and sending requests on the behalf of the user. Additionally, it includes methods to compose and send Tweet messages.
	/// </summary>
	/// <description>
	///	You will need a consumer key and a consumer secret to use Twitter SDK within your application.
	/// And for this, please follow the instructions provided by <a href="https://docs.fabric.io/ios/twitter/configure-twitter-app.html">Twitter</a> and set it in NPSettings.
	/// </description>
	public partial class Twitter : MonoBehaviour 
	{
		#region Fields

		protected	string 		m_activeSessionUserID;

		private		bool		m_lastTweetWasTextOnly;
		private		bool		m_isInitialised;

		#endregion

		#region Init API

		/// <summary>
		/// Initialises the SDK with the credentials set in NPSettings.
		/// </summary>
		/// <returns><c>true</c> if SDK got initialised; otherwise, <c>false</c>.</returns>
		///	<remarks> 
		/// \note You need to call this method, before using any features. 
		/// This method requires that you have set up your consumerKey and consumerSecret in NPSettings. 
		/// </remarks>
		public virtual bool Initialise ()
		{
			TwitterSettings _twitterSettings	= NPSettings.SocialNetworkSettings.TwitterSettings;
			
			if (string.IsNullOrEmpty(_twitterSettings.ConsumerKey) || string.IsNullOrEmpty(_twitterSettings.ConsumerSecret))
			{
				DebugUtility.Logger.LogError(Constants.kDebugTag, "[Twitter] Twitter initialize failed. Please configure Consumer Key and Consumer Secret in NPSettings.");
				m_isInitialised = false;
			}
			else
			{
				m_isInitialised = true;
			}

			return m_isInitialised;
		}

		protected bool IsInitialised ()
		{
			return m_isInitialised;
		}

		#endregion
		
		#region Account API's

		/// <summary>
		/// Authenticates the app user with Twitter.
		/// </summary>
		/// <description>
		/// This method falls back to presenting an OAuth flow, if it fails to find saved login credentials.
		/// </description>
		/// <param name="_requiresEmailAccess">The value indicates whether application needs access to email address information of the logged-in user.</param>
		/// <param name="_onCompletion">Callback that will be called after operation is completed.</param>
		/// <remarks>
		/// \note User authentication is required for API requests that require a user context, for example: Tweeting or following other users.
		/// Also, requesting a user’s email address requires your application to be whitelisted by Twitter. To request access, please visit https://support.twitter.com/forms/platform.
		/// </remarks>
		/// <example>
		/// The following code shows how to use login method.
		/// <code>
		/// using UnityEngine;
		/// using System.Collections;
		/// using VoxelBusters.NativePlugins;
		/// 
		/// public class ExampleClass : MonoBehaviour 
		/// {
		/// 	public void Login ()
		/// 	{
		/// 		NPBinding.Twitter.Login(false, OnLoginFinished);
		/// 	}
		/// 
		/// 	private void OnLoginFinished (TwitterSession _session, string _error)
		/// 	{
		/// 		if (_error == null)
		/// 		{
		/// 			// Logged in successfully
		/// 		}
		/// 		else
		/// 		{
		/// 			// Login failed
		/// 		}
		/// 	}
		/// }
		/// </code>
		/// </example>
		public virtual void Login (bool _requiresEmailAccess, TWTRLoginCompletion _onCompletion)
		{
			// Pause unity player
			this.PauseUnity();

			// Cache callback
			OnLoginFinished	= _onCompletion;

#if USES_SOOMLA_GROW
			NPBinding.SoomlaGrowService.ReportOnSocialLoginStarted(eSocialProvider.TWITTER);
#endif
		}

		/// <summary>
		/// Deletes the Twitter user session from this app.
		/// </summary>
		public virtual void Logout ()
		{
#if USES_SOOMLA_GROW
			NPBinding.SoomlaGrowService.ReportOnSocialLogoutStarted(eSocialProvider.TWITTER);
#endif
		}

		/// <summary>
		/// Determines whether user is currently logged in to Twitter.
		/// </summary>
		/// <returns><c>true</c> if user is currently logged into Twitter; otherwise, <c>false</c>.</returns>
		public virtual bool IsLoggedIn ()
		{
			bool _isLoggedIn	= false;
			DebugUtility.Logger.Log(Constants.kDebugTag, "[Twitter] IsLoggedIn=" + _isLoggedIn);
			
			return _isLoggedIn;
		}
		
		/// <summary>
		/// Fetches the last logged-in user session information.
		/// </summary>
		public TwitterAuthSession GetSession ()
		{
			return GetSessionWithUserID(m_activeSessionUserID);
		}

		/// <summary>
		/// Fetches the saved session information of the specified user.
		/// </summary>
		/// <param name="_userID">The user ID to fetch session for.</param>
		public virtual TwitterAuthSession GetSessionWithUserID (string _userID)
		{
			return null;
		}

		/// <summary>
		/// Set the session information to be used while making Twitter request.
		/// </summary>
		/// <param name="_session">_session.</param>
		public void SetActiveSession (TwitterAuthSession _session)
		{
			m_activeSessionUserID	= _session.UserID;
		}

		#endregion
		
		#region Tweet API's

		/// <summary>
		/// Shows a view to compose and send Tweet message.
		/// </summary>
		/// <param name="_message">The initial text for the Tweet.</param>
		/// <param name="_onCompletion">Callback that will be called after operation is completed.</param>
		public void ShowTweetComposerWithMessage (string _message, TWTRTweetCompletion _onCompletion)
		{
			ShowTweetComposer(_message, null, null, _onCompletion);
		}
		
		/// <summary>
		/// Shows a view to compose and send Tweet message.
		/// </summary>
		/// <param name="_message">The initial text for the Tweet.</param>
		/// <param name="_onCompletion">Callback that will be called after operation is completed.</param>
		public void ShowTweetComposerWithScreenshot (string _message, TWTRTweetCompletion _onCompletion)
		{
			// First capture screenshot, once its done tweet about it
			StartCoroutine(TextureExtensions.TakeScreenshot((_texture)=>{
				
				ShowTweetComposerWithImage(_message, _texture, _onCompletion);
			}));
		}

		/// <summary>
		/// Shows a view to compose and send Tweet message.
		/// </summary>
		/// <param name="_message">The initial text for the Tweet.</param>
		/// <param name="_texture">Unity texture object that has to be shared in the Tweet.</param>
		/// <param name="_onCompletion">Callback that will be called after operation is completed.</param>
		public void ShowTweetComposerWithImage (string _message, Texture2D _texture, TWTRTweetCompletion _onCompletion)
		{
			byte[] _imgByteArray	= null;

			// Convert texture into byte array
			if (_texture != null)
			{
				_imgByteArray = _texture.EncodeToPNG();
			}
			else
			{
				DebugUtility.Logger.LogWarning(Constants.kDebugTag, "[Twitter] Showing tweet composer with message only, texure is null");
			}

			// Show tweet composer
			ShowTweetComposer(_message, null, _imgByteArray, _onCompletion);
		}

		/// <summary>
		/// Shows a view to compose and send Tweet message.
		/// </summary>
		/// <param name="_message">The initial text for the Tweet.</param>
		/// <param name="_URL">URL that has to be shared in the Tweet.</param>
		/// <param name="_onCompletion">Callback that will be called after operation is completed.</param>
		public void ShowTweetComposerWithLink (string _message, string _URL, TWTRTweetCompletion _onCompletion)
		{
			if (string.IsNullOrEmpty(_URL))
			{
				DebugUtility.Logger.LogWarning(Constants.kDebugTag, "[Twitter] Showing tweet composer with message only, URL is null/empty");
			}

			// Show tweet composer
			ShowTweetComposer(_message, _URL, null, _onCompletion);
		}
		
		/// <summary>
		/// Shows a view to compose and send Tweet message.
		/// </summary>
		/// <param name="_message">The initial text for the Tweet.</param>
		/// <param name="_URL">URL that has to be shared in the Tweet.</param>
		/// <param name="_imgByteArray">Raw image data that has to be shared in the Tweet.</param>
		/// <param name="_onCompletion">Callback that will be called after operation is completed.</param>
		public virtual void ShowTweetComposer (string _message, string _URL, byte[] _imgByteArray, TWTRTweetCompletion _onCompletion)
		{
			// Pause unity player
			this.PauseUnity();

			// Cache callback
			OnTweetComposerClosed		=	_onCompletion;

#if USES_SOOMLA_GROW
			bool	_tweetingMessage	= (_URL == null && _imgByteArray == null);
			eSocialActionType	_actionType	= _tweetingMessage ? eSocialActionType.UPDATE_STATUS : eSocialActionType.UPDATE_STORY;

			// Cache information
			m_lastTweetWasTextOnly		= _tweetingMessage;

			// Report this event
			NPBinding.SoomlaGrowService.ReportOnSocialActionStarted(_actionType, eSocialProvider.TWITTER);
#endif
		}

		#endregion

		#region Request API's

		/// <summary>
		/// Requests access to the current Twitter user account details.
		/// </summary>
		/// <param name="_onCompletion">Callback that will be called after operation is completed.</param>
		/// <example>
		/// The following code snippet shows how to fetch account details of current user.
		/// <code>
		/// using UnityEngine;
		/// using System.Collections;
		/// using VoxelBusters.NativePlugins;
		/// 
		/// public class ExampleClass : MonoBehaviour 
		/// {
		/// 	public void FetchAccountDetails ()
		/// 	{
		/// 		NPBinding.Twitter.RequestAccountDetails(OnRequestFinished);
		/// 	}
		/// 
		/// 	private void OnRequestFinished (TwitterUser _user, string _error)
		/// 	{
		/// 		if (_error == null)
		/// 		{
		/// 			Debug.Log("Logged in user name is: " + _user.Name);
		/// 		}
		/// 		else
		/// 		{
		/// 			// Something went wrong
		/// 		}
		/// 	}
		/// }
		/// </code>
		/// </example>
		public virtual void RequestAccountDetails (TWTRAccountDetailsCompletion _onCompletion)
		{
			// Cache callback
			OnRequestAccountDetailsFinished	= _onCompletion;
		}

		/// <summary>
		/// Requests access to the email address associated with current Twitter user.
		/// </summary>
		/// <param name="_onCompletion">Callback that will be called after operation is completed.</param>
		/// <example>
		/// The following code snippet shows how to fetch email address of current user.
		/// <code>
		/// using UnityEngine;
		/// using System.Collections;
		/// using VoxelBusters.NativePlugins;
		/// 
		/// public class ExampleClass : MonoBehaviour 
		/// {
		/// 	public void FetchEmailAddress ()
		/// 	{
		/// 		NPBinding.Twitter.RequestEmailAccess(OnRequestFinished);
		/// 	}
		/// 
		/// 	private void OnRequestFinished (string _email, string _error)
		/// 	{
		/// 		if (_error == null)
		/// 		{
		/// 			Debug.Log("Email id: " + _email);
		/// 		}
		/// 		else
		/// 		{
		/// 			// Something went wrong
		/// 		}
		/// 	}
		/// }
		/// </code>
		/// </example>
		public virtual void RequestEmailAccess (TWTREmailAccessCompletion _onCompletion)
		{
			// Pause unity player
			this.PauseUnity();

			// Cache callback
			OnRequestEmailAccessFinished	= _onCompletion;
		}

		/// <summary>
		/// Sends a signed Twitter Get request.
		/// </summary>
		/// <param name="_URL">Request URL. This is the full Twitter API URL. E.g. https://api.twitter.com/1.1/statuses/user_timeline.json.</param>
		/// <param name="_parameters">Request parameters.</param>
		/// <param name="_onCompletion">Callback that will be called after operation is completed.</param>
		/// <example>
		/// The following code shows how to send request to Twitter.
		/// <code>
		/// using UnityEngine;
		/// using System.Collections;
		/// using System.Collections.Generic;
		/// using VoxelBusters.NativePlugins;
		/// 
		/// public class ExampleClass : MonoBehaviour 
		/// {
		/// 	public void SendRequest ()
		/// 	{
		/// 		string 		_URL 	= "https://api.twitter.com/1.1/statuses/show.json";
		/// 		IDictionary _params = new Dictionary<string, string>(){
		/// 			{"id", "20"}
		/// 		};
		/// 		
		/// 		NPBinding.Twitter.SendGetURLRequest(_URL, _params, OnRequestFinished);
		/// 	}
		/// 
		/// 	private void OnRequestFinished (object _responseData, string _error)
		/// 	{
		/// 		if (_error == null)
		/// 		{
		/// 			IDictionary	_JSONDict	= (IDictionary)_responseData;
		/// 
		/// 			// Extract values from JSON object
		/// 		}
		/// 		else
		/// 		{
		/// 			// Something went wrong
		/// 		}
		/// 	}
		/// }
		/// </code>
		/// </example>
		public void SendGetURLRequest (string _URL,	IDictionary _parameters, TWTRResponse _onCompletion)
		{
			SendURLRequest("GET", _URL, _parameters, _onCompletion);
		}

		/// <summary>
		/// Sends a signed Twitter Post request.
		/// </summary>
		/// <param name="_URL">Request URL. This is the full Twitter API URL. E.g. https://api.twitter.com/1.1/statuses/user_timeline.json.</param>
		/// <param name="_parameters">Request parameters.</param>
		/// <param name="_onCompletion">Callback that will be called after operation is completed.</param>
		public void SendPostURLRequest (string _URL,	IDictionary _parameters, TWTRResponse _onCompletion)
		{
			SendURLRequest("POST", _URL, _parameters, _onCompletion);
		}

		/// <summary>
		/// Sends a signed Twitter Put request.
		/// </summary>
		/// <param name="_URL">Request URL. This is the full Twitter API URL. E.g. https://api.twitter.com/1.1/statuses/user_timeline.json.</param>
		/// <param name="_parameters">Request parameters.</param>
		/// <param name="_onCompletion">Callback that will be called after operation is completed.</param>
		public void SendPutURLRequest (string _URL,	IDictionary _parameters, TWTRResponse _onCompletion)
		{
			SendURLRequest("PUT", _URL, _parameters, _onCompletion);
		}

		/// <summary>
		/// Sends a signed Twitter Delete request.
		/// </summary>
		/// <param name="_URL">Request URL. This is the full Twitter API URL. E.g. https://api.twitter.com/1.1/statuses/user_timeline.json.</param>
		/// <param name="_parameters">Request parameters.</param>
		/// <param name="_onCompletion">Callback that will be called after operation is completed.</param>
		public void SendDeleteURLRequest (string _URL,	IDictionary _parameters, TWTRResponse _onCompletion)
		{
			SendURLRequest("DELETE", _URL, _parameters, _onCompletion);
		}
		
		protected virtual void SendURLRequest (string _methodType, string _URL,	IDictionary _parameters, TWTRResponse _onCompletion)
		{
			// Cache callback
			OnTwitterURLRequestFinished	= _onCompletion;
		}

		#endregion

		#region Deprecated Methods

		[System.Obsolete("This method is deprecated. Instead use Login(_requiresEmailAccess, _onCompletion).")]
		public void Login (TWTRLoginCompletion _onCompletion)
		{
			Login(false, _onCompletion);
		}

		[System.Obsolete("This method is deprecated. Instead use GetSessionWithUserID method.")]
		public virtual string GetAuthToken ()
		{
			return null;
		}
		
		[System.Obsolete("This method is deprecated. Instead use GetSessionWithUserID method.")]
		public virtual string GetAuthTokenSecret ()
		{
			return null;
		}
		
		[System.Obsolete("This method is deprecated. Instead use GetSessionWithUserID method.")]
		public virtual string GetUserID ()
		{
			return null;
		}
		
		[System.Obsolete("This method is deprecated. Instead use RequestAccountDetails method.")]
		public virtual string GetUserName ()
		{
			return null;
		}

		[System.Obsolete("This method is deprecated. Instead use SendGetURLRequest method.")]
		public void GetURLRequest (string _URL,	IDictionary _parameters, TWTRResponse _onCompletion)
		{
			SendGetURLRequest(_URL, _parameters, _onCompletion);
		}

		[System.Obsolete("This method is deprecated. Instead use SendPostURLRequest method.")]
		public void PostURLRequest (string _URL, IDictionary _parameters, TWTRResponse _onCompletion)
		{
			SendPostURLRequest(_URL, _parameters, _onCompletion);
		}

		[System.Obsolete("This method is deprecated. Instead use SendPutURLRequest method.")]
		public void PutURLRequest (string _URL,	IDictionary _parameters, TWTRResponse _onCompletion)
		{
			SendPutURLRequest(_URL, _parameters, _onCompletion);
		}

		[System.Obsolete("This method is deprecated. Instead use SendDeleteURLRequest method.")]
		public void DeleteURLRequest (string _URL,	IDictionary _parameters, TWTRResponse _onCompletion)
		{
			SendDeleteURLRequest(_URL, _parameters, _onCompletion);
		}

		#endregion
	}
}
#endif
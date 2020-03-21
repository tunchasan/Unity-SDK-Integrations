#if USES_TWITTER
using UnityEngine;
using System.Collections;
using VoxelBusters.Utility;
using VoxelBusters.UASUtils;

namespace VoxelBusters.NativePlugins
{
	using Internal;

	public partial class Twitter : MonoBehaviour 
	{
		#region Delegates

		/// <summary>
		/// Delegate that will be called when authentication process is complete.
		/// </summary>
		/// <param name="_session">Contains the OAuth tokens and minimal information associated with the logged in user or nil.</param>
		/// <param name="_error">If the operation was successful, this value is nil; otherwise, this parameter holds the description of the problem that occurred.</param>
		public delegate void TWTRLoginCompletion (TwitterSession _session, string _error);

		/// <summary>
		/// Delegate that will be called when Tweet composer is dismissed.
		/// </summary>
		public delegate void TWTRTweetCompletion (eTwitterComposerResult _result);

		/// <summary>
		/// Delegate that will be called when user account details are retrieved.
		/// </summary>
		/// <param name="_user">The logged in Twitter user.</param>
		/// <param name="_error">If the operation was successful, this value is nil; otherwise, this parameter holds the description of the problem that occurred.</param>
		public delegate void TWTRAccountDetailsCompletion (TwitterUser _user, string _error);

		/// <summary>
		/// Delegate that will be called when email access request completes.
		/// </summary>
		/// <param name="_email">The logged in Twitter user's email address. This value is <c>null</c> if the user does not grant access to their email address or your application is not allowed to request email addresses.</param>
		/// <param name="_error">If the operation was successful, this value is <c>null</c>; otherwise, this parameter holds the description of the problem that occurred.</param>
		public delegate void TWTREmailAccessCompletion (string _email, string _error);

		/// <summary>
		/// Delegate that will be called when Twitter request completes.
		/// </summary>
		/// <param name="_responseData">Response to a Twitter request.</param>
		/// <param name="_error">If the operation was successful, this value is nil; otherwise, this parameter holds the description of the problem that occurred.</param>
		public delegate void TWTRResponse (object _responseData, string _error);

		#endregion

		#region Events
		
		protected TWTRLoginCompletion				OnLoginFinished;
		protected TWTRTweetCompletion				OnTweetComposerClosed;
		protected TWTRAccountDetailsCompletion		OnRequestAccountDetailsFinished;
		protected TWTREmailAccessCompletion			OnRequestEmailAccessFinished;
		protected TWTRResponse						OnTwitterURLRequestFinished;
		
		#endregion

		#region Native Callback Methods

		protected void TwitterLoginSuccess (string _sessionJsonStr)
		{
			// Resume unity player
			this.ResumeUnity();

			// Get session json object
			IDictionary _sessionJsonDict	= JSONUtility.FromJSON(_sessionJsonStr) as IDictionary;
			TwitterSession _session;

			// Parse received data
			ParseSessionData(_sessionJsonDict, out _session);
			DebugUtility.Logger.Log(Constants.kDebugTag, "[Twitter] Login success, Session=" + _session);
			
			// Invoke handler
			TwitterLoginFinished(_session, null);
			SetActiveSession(_session);
		}
		
		protected void TwitterLoginFailed (string _error)
		{
			DebugUtility.Logger.Log(Constants.kDebugTag, "[Twitter] Login failed");

			// Resume unity player
			this.ResumeUnity();

			// Invoke handler
			TwitterLoginFinished(null, _error);
		}

		private void TwitterLoginFinished (TwitterSession _session, string _error)
		{
			if (OnLoginFinished != null)
				OnLoginFinished(_session, _error);

#if USES_SOOMLA_GROW
			if (string.IsNullOrEmpty(_error))
			{
				NPBinding.SoomlaGrowService.ReportOnSocialLoginFinished(eSocialProvider.TWITTER, _session.UserID);
			}
			else
			{
				NPBinding.SoomlaGrowService.ReportOnSocialLoginFailed(eSocialProvider.TWITTER);
			}
#endif
		}

		protected void TwitterLogoutFinished ()
		{
#if USES_SOOMLA_GROW
			NPBinding.SoomlaGrowService.ReportOnSocialLogoutFinished(eSocialProvider.TWITTER);
#endif
			m_activeSessionUserID	= null;
		}
		
		protected void TweetComposerDismissed (string _resultStr)
		{
			// Resume unity player
			this.ResumeUnity();

			eTwitterComposerResult _result;

			// Parse received data
			ParseTweetComposerDismissedData(_resultStr, out _result);
			DebugUtility.Logger.Log(Constants.kDebugTag, "[Twitter] Tweet composer was dismissed, Result=" + _result);

			if (OnTweetComposerClosed != null)
				OnTweetComposerClosed(_result);

#if USES_SOOMLA_GROW
			eSocialActionType	_actionType	= m_lastTweetWasTextOnly ? eSocialActionType.UPDATE_STATUS : eSocialActionType.UPDATE_STORY;

			if (_result == eTwitterComposerResult.DONE)
			{
				NPBinding.SoomlaGrowService.ReportOnSocialActionFinished(_actionType, eSocialProvider.TWITTER);
			}
			else
			{
				NPBinding.SoomlaGrowService.ReportOnSocialActionCancelled(_actionType, eSocialProvider.TWITTER);
			}
#endif
		}
		
		protected void RequestAccountDetailsSuccess (string _userJsonStr)
		{
			// Get twitter user json object
			IDictionary _userJsonDict	= JSONUtility.FromJSON(_userJsonStr) as IDictionary;
			TwitterUser _user;

			// Parse received data
			ParseUserData(_userJsonDict, out _user);
			DebugUtility.Logger.Log(Constants.kDebugTag, "[Twitter] Accessed account details, User=" + _user);

			// Trigger event
			if (OnRequestAccountDetailsFinished != null)
				OnRequestAccountDetailsFinished(_user, null);
		}
		
		protected void RequestAccountDetailsFailed (string _error)
		{
			DebugUtility.Logger.Log(Constants.kDebugTag, "[Twitter] Accessing account details failed, Error=" + _error);

			if (OnRequestAccountDetailsFinished != null)
				OnRequestAccountDetailsFinished(null, _error);
		}
		
		protected void RequestEmailAccessSuccess (string _email)
		{
			DebugUtility.Logger.Log(Constants.kDebugTag, "[Twitter] Access to Email is permitted, Email=" + _email);

			// Resume unity player
			this.ResumeUnity();

			if (OnRequestEmailAccessFinished != null)
				OnRequestEmailAccessFinished(_email, null);
		}
		
		protected void RequestEmailAccessFailed (string _error)
		{
			DebugUtility.Logger.Log(Constants.kDebugTag, "[Twitter] Access to Email is not permitted, Error=" + _error);

			// Resume unity player
			this.ResumeUnity();

			if (OnRequestEmailAccessFinished != null)
				OnRequestEmailAccessFinished(null, _error);
		}
		
		protected void TwitterURLRequestSuccess (string _responseJsonStr)
		{
			DebugUtility.Logger.Log(Constants.kDebugTag, "[Twitter] URL request success, Response=" + _responseJsonStr);

			if (OnTwitterURLRequestFinished != null)
			{
				// Get response data
				object 	_responseData	= JSONUtility.FromJSON(_responseJsonStr);

				// Trigger event
				OnTwitterURLRequestFinished(_responseData, null);
			}
		}
		
		protected void TwitterURLRequestFailed (string _error)
		{
			DebugUtility.Logger.Log(Constants.kDebugTag, "[Twitter] URL request failed, Error=" + _error);

			if (OnTwitterURLRequestFinished != null)
				OnTwitterURLRequestFinished(null, _error);
		}

		#endregion

		#region Parse Methods

		protected virtual void ParseSessionData (IDictionary _sessionDict, out TwitterSession _session)
		{
			_session	= null;
		}
		
		protected virtual void ParseTweetComposerDismissedData (string _resultStr, out eTwitterComposerResult _result)
		{
			_result		= (eTwitterComposerResult)int.Parse(_resultStr);
		}
		
		protected virtual void ParseUserData (IDictionary _userDict, out TwitterUser _user)
		{
			_user		= null;
		}

		#endregion
	}
}
#endif
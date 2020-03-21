using UnityEngine;
using System.Collections;

#if USES_GAME_SERVICES
using System;
using System.Collections.Generic;
using VoxelBusters.Utility;

namespace VoxelBusters.NativePlugins
{
	using Internal;

	public partial class GameServices : MonoBehaviour 
	{
		#region  Constants

		// Local user
		private 	const 	string		kLocalUserAuthFinishedEvent			= "AuthenticationFinished";
		private 	const 	string		kLocalUserLoadFriendsFinishedEvent	= "LoadFriendsFinished";
		private 	const 	string		kLocalUserSignOutFinishedEvent		= "SignOutFinished";

		// Leaderboard
		private 	const 	string		kLeaderboardLoadScoresFinishedEvent	= "LoadScoresFinished";

		// Achievement
		private 	const 	string		kAchievementReportProgressFinishedEvent		= "ReportProgressFinished";

		// Achievement description
		private		const	string		kDescriptionRequestForImageFinishedEvent	= "RequestForImageFinished";

		// Score
		private		const	string		kScoreReportScoreFinished			= "ReportScoreFinished";

		// User
		private		const 	string 		kUserRequestForImageFinished		= "RequestForImageFinished";

		#endregion

		#region Delegates

		///	<summary>
		///	Delegate that will be called when game service user interface is closed.
		///	</summary>
		/// <param name="_error">If the operation was successful, this value is nil; otherwise, this parameter holds the description of the problem that occurred.</param>
		public delegate void GameServiceViewClosed (string _error);

		public delegate void LoadExternalAuthenticationCredentialsCompletion (ExternalAuthenticationCredentials _credentials, string _error);

		#endregion

		#region Events

		// Achievement events
		private event Achievement.LoadAchievementsCompletion LoadAchievementsFinishedEvent;

		// Achievement description events
		private event AchievementDescription.LoadAchievementDescriptionsCompletion LoadAchievementDescriptionsFinishedEvent;

		// User events
		private event User.LoadUsersCompletion LoadUsersFinishedEvent;

		// UI events
		private event GameServiceViewClosed ShowLeaderboardViewFinishedEvent;
		private event GameServiceViewClosed ShowAchievementViewFinishedEvent;

		// Misc Events
		private event LoadExternalAuthenticationCredentialsCompletion LoadExternalAuthenticationCredentialsFinishedEvent;

		#endregion

		#region Local User Callback Methods

		private void AuthenticationFinished (string _dataStr)
		{
			IDictionary 	_dataDict	= (IDictionary)JSONUtility.FromJSON(_dataStr);

			// Invoke handler
			AuthenticationFinished(_dataDict);
		}

		private void AuthenticationFinished (IDictionary _dataDict)
		{
			LocalUser		_localUser	= LocalUser;

			// Invoke event method
			_localUser.InvokeMethod(kLocalUserAuthFinishedEvent, new object[] {
				_dataDict
			}, new Type[] {
				typeof(IDictionary)
			});
		}

		private void LoadFriendsFinished (string _dataStr)
		{
			IDictionary 	_dataDict	= (IDictionary)JSONUtility.FromJSON(_dataStr);

			// Invoke handler
			LoadFriendsFinished(_dataDict);
		}

		private void LoadFriendsFinished (IDictionary _dataDict)
		{
			LocalUser		_localUser	= LocalUser;

			// Invoke event method
			_localUser.InvokeMethod(kLocalUserLoadFriendsFinishedEvent, new object[] {
				_dataDict
			}, new Type[] {
				typeof(IDictionary)
			});
		}

		private void SignOutFinished (string _dataStr)
		{
			IDictionary 	_dataDict	= (IDictionary)JSONUtility.FromJSON(_dataStr);
			SignOutFinished(_dataDict);	
		}

		private void SignOutFinished (IDictionary _dataDict)
		{
			LocalUser		_localUser	= LocalUser;
			
			// Invoke event method
			_localUser.InvokeMethod(kLocalUserSignOutFinishedEvent, new object[] {
				_dataDict
			}, new Type[] {
				typeof(IDictionary)
			});
		}

		#endregion
		
		#region Leaderboard Callback Methods

		protected void LoadScoresFinished (string _dataStr)
		{
			IDictionary		_dataDict	= (IDictionary)JSONUtility.FromJSON(_dataStr);
			
			// Invoke handler
			LoadScoresFinished(_dataDict);
		}
		
		protected virtual void LoadScoresFinished (IDictionary _dataDict)
		{}
		
		protected void LoadScoresFinished (string _instanceID, IDictionary _dataDict)
		{
			Leaderboard 	_leaderboard	= NPObjectManager.GetObjectWithInstanceID<Leaderboard>(_instanceID, NPObjectManager.eCollectionType.GAME_SERVICES);
			
			if (_leaderboard == null)
				return;
			
			// Invoke response handler
			_leaderboard.InvokeMethod(kLeaderboardLoadScoresFinishedEvent, new object[] {
				_dataDict
			}, new Type[] {
				typeof(IDictionary)
			});
		}
		
		#endregion
		
		#region Achievement Description Callback Methods

		protected void LoadAchievementDescriptionsFinished (string _dataStr)
		{
			IDictionary		_dataDict	= (IDictionary)JSONUtility.FromJSON(_dataStr);

			// Invoke handler
			LoadAchievementDescriptionsFinished(_dataDict);
		}

		protected virtual void LoadAchievementDescriptionsFinished (IDictionary _dataDict)
		{}

		protected void LoadAchievementDescriptionsFinished (AchievementDescription[] _descriptions, string _error)
		{
			AchievementHandler.SetAchievementDescriptionList(_descriptions);
			
			// Send callback
			if (LoadAchievementDescriptionsFinishedEvent != null)
				LoadAchievementDescriptionsFinishedEvent(_descriptions, _error);
		}

		protected void RequestForAchievementImageFinished (string _dataStr)
		{
			IDictionary		_dataDict	= (IDictionary)JSONUtility.FromJSON(_dataStr);

			// Invoke handler
			RequestForAchievementImageFinished(_dataDict);
		}
		
		protected virtual void RequestForAchievementImageFinished (IDictionary _dataDict)
		{}

		protected void RequestForAchievementImageFinished (string _instanceID, IDictionary _dataDict)
		{
			AchievementDescription _description	= NPObjectManager.GetObjectWithInstanceID<AchievementDescription>(_instanceID, NPObjectManager.eCollectionType.GAME_SERVICES);
			
			if (_description == null)
				return;
			
			// Invoke response handler
			_description.InvokeMethod(kDescriptionRequestForImageFinishedEvent, new object[] {
				_dataDict
			}, new Type[] {
				typeof(IDictionary)
			});
		}
		
		#endregion

		#region Achievement Callback Methods

		protected void LoadAchievementsFinished (string _dataStr)
		{
			IDictionary		_dataDict	= (IDictionary)JSONUtility.FromJSON(_dataStr);
			
			// Invoke handler
			LoadAchievementsFinished(_dataDict);
		}
		
		protected virtual void LoadAchievementsFinished (IDictionary _dataDict)
		{}

		protected void LoadAchievementsFinished (Achievement[] _achievements, string _error)
		{
			if (LoadAchievementsFinishedEvent != null)
				LoadAchievementsFinishedEvent(_achievements, _error);
		}

		protected void ReportProgressFinished (string _dataStr)
		{
			IDictionary		_dataDict	= (IDictionary)JSONUtility.FromJSON(_dataStr);

			// Invoke handler
			ReportProgressFinished(_dataDict);
		}

		protected virtual void ReportProgressFinished (IDictionary _dataDict)
		{}

		protected void ReportProgressFinished (string _instanceID, IDictionary _dataDict)
		{
			Achievement 	_achievement	= NPObjectManager.GetObjectWithInstanceID<Achievement>(_instanceID, NPObjectManager.eCollectionType.GAME_SERVICES);
			
			if (_achievement == null)
				return;
			
			// Invoke response handler
			_achievement.InvokeMethod(kAchievementReportProgressFinishedEvent, new object[] {
				_dataDict
			}, new Type[] {
				typeof(IDictionary)
			});
		}

		#endregion

		#region User Callback Methods

		protected void LoadUsersFinished (string _dataStr)
		{
			IDictionary		_dataDict	= (IDictionary)JSONUtility.FromJSON(_dataStr);

			// Invoke handler
			LoadUsersFinished(_dataDict);
		}

		protected virtual void LoadUsersFinished (IDictionary _dataDict)
		{}

		protected void LoadUsersFinished (User[] _users, string _error)
		{
			if (LoadUsersFinishedEvent != null)
				LoadUsersFinishedEvent(_users, _error);
		}

		protected void RequestForUserImageFinished (string _dataStr)
		{
			IDictionary		_dataDict	= (IDictionary)JSONUtility.FromJSON(_dataStr);

			// Invoke handler
			RequestForUserImageFinished(_dataDict);
		}

		protected virtual void RequestForUserImageFinished (IDictionary _dataDict)
		{}

		protected void RequestForUserImageFinished (string _instanceID, IDictionary _dataDict)
		{
			User 	_user	= NPObjectManager.GetObjectWithInstanceID<User>(_instanceID, NPObjectManager.eCollectionType.GAME_SERVICES);
			
			if (_user == null)
				return;
			
			// Invoke response handler
			_user.InvokeMethod(kUserRequestForImageFinished, new object[] {
				_dataDict
			}, new Type[] {
				typeof(IDictionary)
			});
		}

		#endregion

		#region Score Callback Methods

		protected void ReportScoreFinished (string _dataStr)
		{
			IDictionary		_dataDict	= (IDictionary)JSONUtility.FromJSON(_dataStr);
			
			// Invoke handler
			ReportScoreFinished(_dataDict);
		}
		
		protected virtual void ReportScoreFinished (IDictionary _dataDict)
		{}

		protected void ReportScoreFinished (string _instanceID, IDictionary _dataDict)
		{
			Score 	_score	= NPObjectManager.GetObjectWithInstanceID<Score>(_instanceID, NPObjectManager.eCollectionType.GAME_SERVICES);
			
			if (_score == null)
				return;
			
			// Invoke response handler
			_score.InvokeMethod(kScoreReportScoreFinished, new object[] {
				_dataDict
			}, new Type[] {
				typeof(IDictionary)
			});
		}

		#endregion
		
		#region UI Callback Methods

		protected virtual void ShowLeaderboardViewFinished (string _error)
		{
			// Resume unity player
			this.ResumeUnity();

			// Send callback
			if (ShowLeaderboardViewFinishedEvent != null)
				ShowLeaderboardViewFinishedEvent(string.IsNullOrEmpty(_error) ? null : _error);
		}

		protected virtual void ShowAchievementViewFinished (string _error)
		{
			// Resume unity player
			this.ResumeUnity();
			
			// Send callback
			if (ShowAchievementViewFinishedEvent != null)
				ShowAchievementViewFinishedEvent(string.IsNullOrEmpty(_error) ? null : _error);
		}

		#endregion

		#region Misc Callback Methods
		protected void LoadExternalAuthenticationCredentialsFinished (string _dataStr)
		{
			IDictionary		_dataDict	= (IDictionary)JSONUtility.FromJSON(_dataStr);
			Debug.Log("LoadExternalAuthenticationCredentialsFinished" + _dataStr);
			// Invoke handler
			LoadExternalAuthenticationCredentialsFinished(_dataDict);
		}

		protected virtual void LoadExternalAuthenticationCredentialsFinished (IDictionary _dataDict)
		{}

		protected void LoadExternalAuthenticationCredentialsFinished(IDictionary _credentialsData, string _error)
		{
			ExternalAuthenticationCredentials _credentials = null;

			if (string.IsNullOrEmpty(_error))
			{
				_credentials = new ExternalAuthenticationCredentials(_credentialsData);
			}

			if (LoadExternalAuthenticationCredentialsFinishedEvent != null)
			{
				LoadExternalAuthenticationCredentialsFinishedEvent(_credentials, _error);
			}
		}
		#endregion
	}
}
#endif
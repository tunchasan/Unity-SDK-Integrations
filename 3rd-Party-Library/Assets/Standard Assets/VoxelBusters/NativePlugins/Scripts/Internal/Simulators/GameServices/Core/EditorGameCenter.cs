#if USES_GAME_SERVICES && UNITY_EDITOR
using UnityEngine;
using UnityEngine.SocialPlatforms;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.IO;
using VoxelBusters.Utility;
using VoxelBusters.UASUtils;

namespace VoxelBusters.NativePlugins.Internal
{
	[Serializable]
	public partial class EditorGameCenter : SharedScriptableObject<EditorGameCenter>
	{
		#region Fields

		// Related to User
		[SerializeField]
		private					List<EGCUser>						m_registeredUsers				= new List<EGCUser>();
		[NonSerialized]
		private					EGCLocalUser						m_localUser;

		// Related to Leaderboard
		[SerializeField]
		private					List<EGCLeaderboard>				m_leaderboardList				= new List<EGCLeaderboard>();

		// Related to Achievement
		[SerializeField]
		private					List<EGCAchievement>				m_achievementsList				= new List<EGCAchievement>();
		[SerializeField]
		private					List<EGCAchievementDescription>		m_achievementDescriptionList	= new List<EGCAchievementDescription>();

		// Related to UI
		[NonSerialized]
		private					EditorGameCenterUI					m_gameCenterUI					= null;
		[NonSerialized]
		private					bool								m_canShowAchievementBanner		= true;

		#endregion

		#region Methods

		protected override void OnEnable ()
		{
			base.OnEnable ();

			// Update data
			Refresh();
		}

		public void Refresh ()
		{
			List<EGCLeaderboard>			_newLeaderboardList				= new List<EGCLeaderboard>();
			List<EGCAchievementDescription>	_newAchievementDescriptionList	= new List<EGCAchievementDescription>();
			List<EGCAchievement>			_newAchievementList				= new List<EGCAchievement>();

			// Update leaderboard info
			foreach (LeaderboardMetadata _leaderboardMetadata in NPSettings.GameServicesSettings.LeaderboardMetadataCollection)
			{
				string			_curLeaderboardID	= _leaderboardMetadata.GetCurrentPlatformID();
				string			_curLeaderboardGID	= _leaderboardMetadata.GlobalID;

				// Ignore null/empty identifiers
				if (string.IsNullOrEmpty(_curLeaderboardID))
					continue;

				EGCLeaderboard 	_curLeaderboard		= GetLeaderboardWithID(_curLeaderboardID);

				if (_curLeaderboard == null)
					_curLeaderboard					= new EGCLeaderboard(_identifier: _curLeaderboardID, _title: _curLeaderboardGID);

				// Add this entry to the new list
				_newLeaderboardList.Add(_curLeaderboard);
			}

			// Update descriptions and achievements info
			foreach (AchievementMetadata _achievementMetadata in NPSettings.GameServicesSettings.AchievementMetadataCollection)
			{
				string			_curAchievementID	= _achievementMetadata.GetCurrentPlatformID();
				string			_curAchievementGID	= _achievementMetadata.GlobalID;

				// Ignore null/empty identifiers
				if (string.IsNullOrEmpty(_curAchievementID))
					continue;

				EGCAchievementDescription	_curDescription	= GetAchievementDescription(_curAchievementID);
				EGCAchievement	_curAchievement		= GetAchievementWithID(_curAchievementID);

				if (_curDescription == null)
				{
					_curDescription					= new EGCAchievementDescription(_identifier: _curAchievementID, 
					                                                    _title: _curAchievementGID, 
					                                                    _achievedDescription: "Achieved description",
					                                                    _unachievedDescription: "Unachieved description",
					                                                    _maxPoints: 1,
					                                                    _image: null,
					                                                    _isHidden: false);
				}

				// Add entries to the new list
				_newAchievementDescriptionList.Add(_curDescription);

				if (_curAchievement != null)
					_newAchievementList.Add(_curAchievement);
			}

			// Set these new values
			m_leaderboardList				= _newLeaderboardList;
			m_achievementsList				= _newAchievementList;
			m_achievementDescriptionList	= _newAchievementDescriptionList;
		}

		public void ResetAchievements ()
		{
			if (m_achievementsList != null)
				m_achievementsList.Clear();
		}

		public EGCUser GetLocalUserInfo ()
		{
			if (!VerifyUser())
				return null;

			return m_localUser.Info;
		}

		#endregion

		#region Accounts Methods

		public void Authenticate ()		
		{
			// Check if user has already authenticated
			if (m_localUser != null && m_localUser.IsAuthenticated)
			{
				OnAuthenticationFinished(m_localUser, null);
				return;
			}

			// As user isnt logged in, show login prompt
			NPBinding.UI.ShowLoginPromptDialog("Editor Game Center", "Login to start using Game Center.", "user identifier", "password", new string[] { 
				"Log in", "Cancel"
			}, (string _button, string _loginID, string _password)=> {

				string	_error		= null;

				if (_button.Equals("Cancel"))
				{
					DebugUtility.Logger.LogWarning(Constants.kDebugTag, "[GameServices] User cancelled login prompt.");
					_error			= "The operation couldnot be completed because user cancelled the login prompt.";
				}
				else
				{
					if (string.IsNullOrEmpty(_loginID))
						_loginID	= "urgent_user";

					EGCUser _regUserInfo	= GetUserWithID(_loginID);
					
					// Copy details of logged in user
					if (_regUserInfo == null)
					{
						_regUserInfo		= new EGCUser(_loginID);
						
						// Add it to registered user list
						m_registeredUsers.Add(_regUserInfo);
					}
					
					// Update local user info
					m_localUser				= new EGCLocalUser(_regUserInfo, true);		
				}

				OnAuthenticationFinished(m_localUser, _error);
				return;
			});
		} 

		private void OnAuthenticationFinished (EGCLocalUser _localUserInfo, string _error)
		{
			IDictionary		_dataDict	= new Dictionary<string, object>();	
			
			if (_error != null)
				_dataDict[kErrorKey]	= _error;

			if (_localUserInfo != null)
				_dataDict[kLocalUserInfoKey]	= _localUserInfo;
			
			if (NPBinding.GameServices != null)
				NPBinding.GameServices.InvokeMethod(kAuthenticationFinishedEvent, new object[] {
					_dataDict
				}, new Type[] {
					typeof(IDictionary)
				});
		}

		public void LoadFriends ()
		{
			// Check if user has logged in
			if (!VerifyUser())
			{
				OnLoadFriendsFinished(null, Constants.kGameServicesUserAuthMissingError);
				return;
			}

			// Load all friends info
			EGCUser[] _friends	= GetUserInfo(m_localUser.Info.Friends);

			OnLoadFriendsFinished(_friends, null);
		}

		private void OnLoadFriendsFinished (EGCUser[] _friends, string _error)
		{
			IDictionary		_dataDict	= new Dictionary<string, object>();	
			
			if (_error != null)
				_dataDict[kErrorKey]	= _error;
			
			if (_friends != null)
				_dataDict[kFriendUsersKey]	= _friends;
			
			if (NPBinding.GameServices != null)
				NPBinding.GameServices.InvokeMethod(kLoadFriendsFinishedEvent, new object[] {
					_dataDict
				}, new Type[] {
					typeof(IDictionary)
				});
		}
		
		public void SignOut ()
		{
			if (!VerifyUser())
				return;
			
			// Signing out
			m_localUser.IsAuthenticated	= false;
		}

		public bool IsAuthenticated ()
		{
			if (m_localUser == null)
				return false;

			return m_localUser.IsAuthenticated;
		}

		private bool VerifyUser ()
		{
			if (m_localUser == null || !m_localUser.IsAuthenticated)
			{
				DebugUtility.Logger.LogError(Constants.kDebugTag, "[GameServices] User needs to authenticate before using services.");
				return false;
			}

			return true;
		}

		#endregion

		#region User Methods

		public void GetUserImage (EditorUser _userInfo)
		{
			EGCUser 		_gcUser 	= GetUserWithID(_userInfo.Identifier);
			IDictionary		_dataDict	= new Dictionary<string, object>();
			
			if (_gcUser == null)
				_dataDict[kErrorKey] 	= Constants.kGameServicesIdentifierInfoNotFoundError;
			else if (_gcUser.Image == null)
				_dataDict[kErrorKey] 	= "Image not found.";
			else
				_dataDict[kImageKey] 	= _gcUser.Image;
			
			// Send event
			if (NPBinding.GameServices != null)
				NPBinding.GameServices.InvokeMethod(kRequestForUserImageFinishedEvent, new object[]{
					_userInfo.GetInstanceID(),
					_dataDict
				}, new Type[] {
					typeof(string),
					typeof(IDictionary)
				});
		}
		
		public void LoadUsers (string[] _userIDList)
		{
			// Check if user has logged in
			if (!VerifyUser())
			{
				OnLoadUsersFinished(null, Constants.kGameServicesUserAuthMissingError);
				return;
			}
			
			// Load all friends info
			EGCUser[] _users	= GetUserInfo(_userIDList);
			
			OnLoadUsersFinished(_users, null);
		}

		private void OnLoadUsersFinished (EGCUser[] _users, string _error)
		{
			IDictionary		_dataDict	= new Dictionary<string, object>();
			
			if (_error != null)
				_dataDict[kErrorKey] 	= _error;

			if (_users != null)
				_dataDict[kUsersListKey]	= _users;
			
			// Send event
			if (NPBinding.GameServices != null)
				NPBinding.GameServices.InvokeMethod(kLoadUsersFinishedEvent, new object[]{
					_dataDict
				}, new Type[] {
					typeof(IDictionary)
				});
		}
	
		private EGCUser[] GetUserInfo (string[] _userIDList)
		{
			// Check if input is valid
			if (_userIDList == null)
			{
				DebugUtility.Logger.LogError(Constants.kDebugTag, "[GameServices] Failed to user info.");
				return null;
			}

			// Fetch user info
			List<EGCUser> 	_userList	 	= new List<EGCUser>();
			
			foreach (string _curUserID in _userIDList)
			{
				EGCUser 	_curUserInfo	= GetUserWithID(_curUserID);
				
				if (_curUserInfo != null)
					_userList.Add(_curUserInfo);
			}
			
			return _userList.ToArray();
		}

		public EGCUser GetUserWithID (string _id)
		{
			return m_registeredUsers.FirstOrDefault(_regUser => _regUser.Identifier.Equals(_id));
		}

		#endregion

		#region Leaderboard Methods

		public void LoadTopScores (EditorLeaderboard _leaderboard)
		{
			string 	_instanceID 	= _leaderboard.GetInstanceID();

			// Check if user has logged in
			if (!VerifyUser())
			{
				OnLoadScoresFinished(_instanceID, null, Constants.kGameServicesUserAuthMissingError);
				return;
			}

			// Get leaderboard
			EGCLeaderboard	_gcLeaderboard	= GetLeaderboardWithID(_leaderboard.Identifier);

			if (_gcLeaderboard == null)
			{
				OnLoadScoresFinished(_instanceID, null, Constants.kGameServicesIdentifierInfoNotFoundError);
				return;
			}

			// Load scores
			string  _error;

			_gcLeaderboard.LoadTopScores(_leaderboard.TimeScope, _leaderboard.UserScope, _leaderboard.MaxResults, out _error);
			OnLoadScoresFinished(_instanceID, _gcLeaderboard, _error);
		}

		public void LoadPlayerCenteredScores (EditorLeaderboard _leaderboard)
		{
			string 	_instanceID 	= _leaderboard.GetInstanceID();

			// Check if user has logged in
			if (!VerifyUser())
			{
				OnLoadScoresFinished(_instanceID, null, Constants.kGameServicesUserAuthMissingError);
				return;
			}

			// Get leaderboard
			EGCLeaderboard	_gcLeaderboard	= GetLeaderboardWithID(_leaderboard.Identifier);
			
			if (_gcLeaderboard == null)
			{
				OnLoadScoresFinished(_instanceID, null, Constants.kGameServicesIdentifierInfoNotFoundError);
				return;
			}

			// Load scores
			string  _error;
			
			_gcLeaderboard.LoadPlayerCenteredScores(_leaderboard.TimeScope, _leaderboard.UserScope, _leaderboard.MaxResults, out _error);
			OnLoadScoresFinished(_instanceID, _gcLeaderboard, _error);
		}

		public void LoadMoreScores (EditorLeaderboard _leaderboard, eLeaderboardPageDirection _pageDirection)
		{
			string 	_instanceID 	= _leaderboard.GetInstanceID();

			// Check if user has logged in
			if (!VerifyUser())
			{
				OnLoadScoresFinished(_instanceID, null, Constants.kGameServicesUserAuthMissingError);
				return;
			}

			// Get leaderboard
			EGCLeaderboard	_gcLeaderboard	= GetLeaderboardWithID(_leaderboard.Identifier);
			
			if (_gcLeaderboard == null)
			{
				OnLoadScoresFinished(_instanceID, null, Constants.kGameServicesIdentifierInfoNotFoundError);
				return;
			}

			// Load scores
			string  _error;

			_gcLeaderboard.LoadMoreScores(_leaderboard.TimeScope, _leaderboard.UserScope, _leaderboard.MaxResults, _pageDirection, out _error);
			OnLoadScoresFinished(_instanceID, _gcLeaderboard, _error);
		}

		private void OnLoadScoresFinished (string _callbackInstanceID, EGCLeaderboard _gcLeaderboardInfo, string _error)
		{
			IDictionary		_dataDict	= new Dictionary<string, object>();	

			if (_error != null)
				_dataDict[kErrorKey]	= _error;
			else 
				_dataDict[kLeaderboardInfoKey]	= _gcLeaderboardInfo;

			if (NPBinding.GameServices != null)
				NPBinding.GameServices.InvokeMethod(kLoadScoresFinishedEvent, new object[] {
					_callbackInstanceID, 
					_dataDict
				}, new Type[] {
					typeof(string),
					typeof(IDictionary)
				});
		}

		public void ReportScore (EditorScore _newScore)
		{
			string		_instanceID		= _newScore.GetInstanceID();

			// Couldnt verify user
			if (!VerifyUser())
			{
				OnReportScoreFinished(_instanceID, null, Constants.kGameServicesUserAuthMissingError);
				return;
			}

			// Get leaderboard info
			EGCLeaderboard	_gcLeaderboard	= GetLeaderboardWithID(_newScore.LeaderboardID);

			if (_gcLeaderboard == null)
			{
				OnReportScoreFinished(_instanceID, null, Constants.kGameServicesIdentifierInfoNotFoundError);
				return;
			}

			// Add score info
			EGCScore	_newGCScore		= new EGCScore(_gcLeaderboard.Identifier, _newScore.User.Identifier, _newScore.Value);

			_gcLeaderboard.AddNewScoreInfo(_newGCScore);
			OnReportScoreFinished(_instanceID, _newGCScore, null);
		}

		public void OnReportScoreFinished (string _callbackInstanceID, EGCScore _newGCScore, string _error)
		{
			IDictionary		_dataDict		= new Dictionary<string, object>();	
			
			if (_error != null)
				_dataDict[kErrorKey]		= _error;
		
			if (_newGCScore != null)
				_dataDict[kScoreInfoKey]	= _newGCScore;
			
			if (NPBinding.GameServices != null)
				NPBinding.GameServices.InvokeMethod(kReportScoreFinishedEvent, new object[] {
					_callbackInstanceID, 
					_dataDict
				}, new Type[] {
					typeof(string),
					typeof(IDictionary)
				});
		}
		
		private EGCLeaderboard GetLeaderboardWithID (string _leaderboardID)
		{
			if (_leaderboardID == null)
			{
				DebugUtility.Logger.LogError(Constants.kDebugTag, Constants.kGameServicesIdentifierNullError);
				return null;
			}
			
			return m_leaderboardList.FirstOrDefault(_curLeaderboard => _curLeaderboard.Identifier.Equals(_leaderboardID));
		}

		#endregion

		#region Achievement Description Methods

		public void LoadAchievementDescriptions ()
		{
			// Verify authentication state
			IDictionary		_dataDict	= new Dictionary<string, object>();
			
			if (m_achievementDescriptionList == null)
				_dataDict[kErrorKey]	= "The operation could not be completed because achievement description list is null.";
			else
				_dataDict[kAchievementDescriptionsListKey]	= m_achievementDescriptionList.ToArray();
			
			// Send event
			if (NPBinding.GameServices != null)
				NPBinding.GameServices.InvokeMethod(kLoadAchievementDescriptionsFinishedEvent, new object[]{
					_dataDict
				}, new Type[] {
					typeof(IDictionary)
				});
		}
		
		public void GetDescriptionImage (EditorAchievementDescription _description)
		{
			EGCAchievementDescription 	_curDescription = GetAchievementDescription(_description.Identifier);
			IDictionary					_dataDict		= new Dictionary<string, object>();

			if (_curDescription == null)
				_dataDict[kErrorKey] = Constants.kGameServicesIdentifierInfoNotFoundError;
			else if (_curDescription.Image == null)
				_dataDict[kErrorKey] = "Image not found.";
			else
				_dataDict[kImageKey] = _curDescription.Image;

			// Send event
			if (NPBinding.GameServices != null)
				NPBinding.GameServices.InvokeMethod(kRequestForAchievementImageFinishedEvent, new object[]{
					_description.GetInstanceID(),
					_dataDict
				}, new Type[] {
					typeof(string),
					typeof(IDictionary)
				});
		}
		
		private EGCAchievementDescription GetAchievementDescription (string _achievementID)
		{
			if (_achievementID == null)
			{
				DebugUtility.Logger.LogError(Constants.kDebugTag, Constants.kGameServicesIdentifierNullError);
				return null;
			}

			return m_achievementDescriptionList.FirstOrDefault(_curDescription=>_curDescription.Identifier.Equals(_achievementID));
		}

		#endregion

		#region Achievement Methods

		public void LoadAchievements ()
		{
			IDictionary		_dataDict	= new Dictionary<string, object>();
			
			if (m_achievementsList == null)
				_dataDict[kErrorKey]	= "The operation could not be completed because achievement list is null.";
			else
				_dataDict[kAchievementsListKey]	= m_achievementsList.ToArray();
			
			// Send event
			if (NPBinding.GameServices != null)
				NPBinding.GameServices.InvokeMethod(kLoadAchievementsFinishedEvent, new object[]{
					_dataDict
				}, new Type[] {
					typeof(IDictionary)
				});
		}

		public void ReportProgress (EditorAchievement _reportedAchievement)
		{
			string 			_instanceID	= _reportedAchievement.GetInstanceID();

			if (!VerifyUser())
			{
				OnReportProgressFinished(_instanceID, null, Constants.kGameServicesUserAuthMissingError);
				return;
			}

			EGCAchievementDescription	_reportedAchievementDescription	= GetAchievementDescription(_reportedAchievement.Identifier);

			if (_reportedAchievementDescription == null)
			{
				OnReportProgressFinished(_instanceID, null, Constants.kGameServicesIdentifierInfoNotFoundError);
				return;
			}

			double			_percentageCompleted	= _reportedAchievement.PercentageCompleted;
			string			_achievementID			= _reportedAchievement.Identifier;
			EGCAchievement	_gcAchievement			= GetAchievementWithID(_achievementID);

			if (_gcAchievement != null)
			{
				// Update values
				_gcAchievement.UpdateProgress(_percentageCompleted);
				
				// Invoke handler
				OnReportProgressFinished(_instanceID, _gcAchievement, null);
				return;
			}

			// Mostly its reported for first time
			EGCAchievementDescription	_gcDescription	= GetAchievementDescription(_achievementID);

			if (_gcDescription != null)
			{
				// Update properties of this description
				_gcDescription.IsHidden	= false;
				
				// Create new achievement entry
				EGCAchievement 	_newAchievement			= new EGCAchievement(_achievementID, _percentageCompleted);
				
				// Add it to the list
				m_achievementsList.Add(_newAchievement);
				
				// Action on finishing report
				OnReportProgressFinished(_instanceID, _newAchievement, null);
				return;
			}
		}

		private void OnReportProgressFinished (string _callbackInstanceID, EGCAchievement _gcAchievementInfo, string _error)
		{
			IDictionary 	_dataDict 		= new Dictionary<string, object>();
			
			if (_error != null)
				_dataDict[kErrorKey] 		= _error;

			if (_gcAchievementInfo != null)
			{
				_dataDict[kAchievementInfoKey] 	= _gcAchievementInfo;

				// Show banner if achievement is completed
				if (_gcAchievementInfo.Completed && m_canShowAchievementBanner)
					ShowAchievementBanner(GetAchievementDescription(_gcAchievementInfo.Identifier));
			}

			// Send notification
			if (NPBinding.GameServices != null)
				NPBinding.GameServices.InvokeMethod (kReportProgressFinishedEvent, new object[]{
					_callbackInstanceID,
					_dataDict
				}, new Type[] {
					typeof(string),
					typeof(IDictionary)
				});
		}

		private EGCAchievement GetAchievementWithID (string _achievementID)
		{
			if (_achievementID == null)
			{
				DebugUtility.Logger.LogError(Constants.kDebugTag, Constants.kGameServicesIdentifierNullError);
				return null;
			}

			return m_achievementsList.FirstOrDefault(_curAchievement=>_curAchievement.Identifier.Equals(_achievementID));
		}

		#endregion

		#region UI Methods

		private void CreateGameCenterUIInstance ()
		{
			GameObject 			_gameObject		= new GameObject();
			_gameObject.hideFlags				= HideFlags.HideInHierarchy;

			// UI component
			m_gameCenterUI						= _gameObject.AddComponent<EditorGameCenterUI>();
		}
		
		public void ShowLeaderboardUI (string _leaderboardID, eLeaderboardTimeScope _timeScope)
		{
			// Check if user has logged in
			if (!VerifyUser())
			{
				OnShowLeaderboardViewFinished(Constants.kGameServicesUserAuthMissingError);
				return;
			}
			
			// Application needs to be in play mode
			if (!Application.isPlaying)
			{				
				OnShowLeaderboardViewFinished("The operation could not be completed because view is available only in play mode.");
				return;
			}
			
			// Get leaderboard info
			EGCLeaderboard	_gcLeaderboard	= GetLeaderboardWithID(_leaderboardID);

			if (_gcLeaderboard == null)
			{
				OnShowLeaderboardViewFinished(Constants.kGameServicesIdentifierInfoNotFoundError);
				return;
			}

			// Set leaderboard score fetch range
			Range			_oldRange		= _gcLeaderboard.Range;
			Range			_newRange		= new Range(1, int.MaxValue);

			// Fetch scores from leaderboard
			_gcLeaderboard.FilterScoreList(_timeScope, eLeaderboardUserScope.GLOBAL, _newRange);

			// Reset range to old value
			_gcLeaderboard.Range			= _oldRange;

			// Show UI
			if (m_gameCenterUI == null)
				CreateGameCenterUIInstance();

			m_gameCenterUI.ShowLeaderboardUI(_gcLeaderboard.GetLastQueryResults(), ()=>{

				// Invoke handler
				OnShowLeaderboardViewFinished(null);
			});
		}

		private void OnShowLeaderboardViewFinished (string _error)
		{
			if (NPBinding.GameServices != null)
				NPBinding.GameServices.InvokeMethod(kShowLeaderboardViewFinishedEvent, new object[] {
					_error
				}, new Type[] {
					typeof(string)
				});
		}
		
		public void ShowAchievementsUI ()
		{
			// Check if user has logged in
			if (!VerifyUser()) 
			{
				OnShowAchievementViewFinished(Constants.kGameServicesUserAuthMissingError);
				return;
			}
			
			// Application needs to be in play mode
			if (!Application.isPlaying)
			{				
				OnShowAchievementViewFinished("The operation could not be completed because view is available only in play mode.");
				return;
			}
			
			// Gather data required to display properties
			Dictionary<EGCAchievementDescription, EGCAchievement> 	_gcAchievementMap	= new Dictionary<EGCAchievementDescription, EGCAchievement>();

			foreach (EGCAchievementDescription _gcAchievementDesc in m_achievementDescriptionList)
			{
				string			_gcDescriptionID	= _gcAchievementDesc.Identifier;
				EGCAchievement	_gcAchievement		= m_achievementsList.FirstOrDefault(_curAchievement => _curAchievement.Identifier.Equals(_gcDescriptionID));

				// Add each entry
				_gcAchievementMap.Add(_gcAchievementDesc, _gcAchievement);
			}

			// Show UI
			if (m_gameCenterUI == null)
				CreateGameCenterUIInstance();

			m_gameCenterUI.ShowAchievementUI(_gcAchievementMap, ()=>{

				// Invoke handler
				OnShowAchievementViewFinished(null);
			});
		}

		private void OnShowAchievementViewFinished (string _error)
		{
			if (NPBinding.GameServices != null)
				NPBinding.GameServices.InvokeMethod(kShowAchievementViewFinishedEvent, new object[] {
					_error
				}, new Type[] {
					typeof(string)
				});
		}

		private void ShowAchievementBanner (EGCAchievementDescription _description)
		{
			if (m_gameCenterUI == null)
				CreateGameCenterUIInstance();
			
			m_gameCenterUI.ShowAchievementBanner(_description);
		}

		#endregion
	}
}
#endif
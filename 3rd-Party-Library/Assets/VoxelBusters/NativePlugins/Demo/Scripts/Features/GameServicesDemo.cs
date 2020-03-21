using UnityEngine;
using System.Collections;
using VoxelBusters.Utility;

namespace VoxelBusters.NativePlugins.Demo
{
	using Internal;

#if !USES_GAME_SERVICES
	public class GameServicesDemo : NPDisabledFeatureDemo 
#else
	public class GameServicesDemo : NPDemoBase 
#endif
	{
		#region Fields

#pragma warning disable
		[SerializeField, Header("Leaderboard Properties")]
		private		eLeaderboardTimeScope	m_timeScope;
		[SerializeField]
		private		int					m_maxScoreResults			= 20;
		private		Leaderboard			m_curLeaderboard;
		private		string				m_curLeaderboardGID;
		private		int					m_curLeaderboardGIDIndex	= -1;
		private		string[] 			m_leaderboardGIDList		= new string[0];

		private		string				m_curAchievementGID;
		private		int					m_curAchievementGIDIndex	= -1;
		private		string[] 			m_achievementGIDList		= new string[0];
#pragma warning restore

		#endregion

#if !USES_GAME_SERVICES
	}
#else
		#region Unity Methods

		protected override void Start ()
		{
			base.Start();

			// Extract gid information
			ExtractGID();

			// Set additional info texts
			AddExtraInfoTexts(
				"You can configure this feature in NPSettings->Game Services Settings.",
				"Using platform specific identifier to access Achievements/Leaderboard object is very troublesome. " +
					"\nInstead, make use of global identifier for unified access of Achievements/Leaderboard irrespective of platform. " +
					"\nThis can be done by either adding identifier info in Game Services Settings or else manually set it at runtime using SetLeaderboardIDCollection & SetAchievementIDCollection API.",
				"For testing iOS build, set Game Center to Sandox mode in your device settings and then log in to Game Center application using sandbox test accounts. Once you are done with it, you can try testing this feature.", 
				"In Unity Editor, we are simulating this feature to help developers test funtionality in Editor itself. You can manually fill info by opening Editor Game Center from Menu (Window->Voxel Busters->Native Plugins) or use Game Services Settings to auto fill values.");
		}

		protected override void OnEnable ()
		{
			base.OnEnable ();

#if UNITY_EDITOR
			AddNewResult ("[NOTE] Simulating feature on Editor.");
#elif UNITY_ANDROID
			AddNewResult ("[NOTE] Google Play Service is being used.");
#elif UNITY_IOS
			AddNewResult ("[NOTE] Game Center is being used.");
#else 
			AddNewResult ("[NOTE] Feature is not supported.");
#endif

			// Leaderboard
			if (m_leaderboardGIDList.Length == 0)
				AppendResult ("Could not find leaderboard id information. Please configure it.");
			else if (m_curLeaderboardGIDIndex == -1)
				ChangeLeaderboardGID (true);

			// Achievement
			if (m_achievementGIDList.Length == 0)
				AppendResult ("Could not find achievement id information. Please configure it.");
			else if (m_curAchievementGIDIndex == -1)
				ChangeAchievementGID (true);
		}

		#endregion

		#region GUI Methods

		protected override void DisplayFeatureFunctionalities ()
		{
			base.DisplayFeatureFunctionalities ();

			if (GUILayout.Button ("Is Available"))
			{
				if (IsAvailable())
					AddNewResult("Game Services feature is supported.");
				else
					AddNewResult("Game Services feature is not supported.");
			}

			if (!IsAvailable())
			{
				GUILayout.Box("Sorry, Game Services feature is not supported on this device.");
				return;
			}
			
			if (GUILayout.Button ("Is Authenticated"))
			{
				if (IsAuthenticated ())
					AddNewResult ("Local user is authenticated.");
				else
					AddNewResult ("Local user is not yet authenticated!");
			}

			if (!IsAuthenticated())
			{
				if (GUILayout.Button ("Authenticate User"))
					AuthenticateUser ();

				GUILayout.Box("Sorry, user is currently not signed in to Game Services. Please authenticate the user before accessing Game Services features.");
				return;
			}
			else
			{
				DrawUserSection ();
				DrawLeaderboardSection ();
				DrawAchievementSection ();
				DrawUISection ();
				DrawMiscSection ();
			}
		}

		private void DrawUserSection ()
		{
			GUILayout.Label("Local User", kSubTitleStyle);

			if (GUILayout.Button("Sign Out"))
			{
				SignOut();
			}
			
			if (GUILayout.Button("Load Friends"))
			{
				LoadFriends();
			}

			if (GUILayout.Button("Load Users"))
			{
				string[] _userIDList = new string[] {
					NPBinding.GameServices.LocalUser.Identifier
				};

				LoadUsers(_userIDList);
			}
		}

		private void DrawLeaderboardSection ()
		{
			GUILayout.Label("Leaderboard", kSubTitleStyle);

			if (m_leaderboardGIDList.Length == 0)
			{
				GUILayout.Box("Could not find Leaderboard configuration in GameServices. If you want to access Leaderboard feature, then please configure it.");
			}
			else
			{
				GUILayout.BeginHorizontal();
				{
					if (GUILayout.Button("Previous Leaderboard"))
						ChangeLeaderboardGID(false);

					if (GUILayout.Button("Next Leaderboard"))
						ChangeLeaderboardGID(true);
				}
				GUILayout.EndHorizontal();

				GUILayout.Box(string.Format("Current Leaderboard GID= {0}.", m_curLeaderboardGID));

				if (GUILayout.Button("Create Leaderboard"))
				{
					Leaderboard	_leaderboard = CreateLeaderboardWithGlobalID(m_curLeaderboardGID);
					AddNewResult(string.Format("Leaderboard with global identifier {0} is created.", _leaderboard.GlobalIdentifier));	
				}
				
				if (GUILayout.Button("Report Score"))
				{
					ReportScoreWithGlobalID(m_curLeaderboardGID);
				}
				
				if (GUILayout.Button("Load Top Scores"))
				{
					LoadTopScores();
				}
				
				if (GUILayout.Button("Load Player Centered Scores"))
				{
					LoadPlayerCenteredScores();
				}
				
				GUILayout.BeginHorizontal();
				{
					if (GUILayout.Button("Load Previous Scores"))
					{
						LoadMoreScores(eLeaderboardPageDirection.PREVIOUS);
					}

					if (GUILayout.Button("Load Next Scores"))
					{
						LoadMoreScores(eLeaderboardPageDirection.NEXT);
					}
				}
				GUILayout.EndHorizontal();
			}
		}

		private void DrawAchievementSection ()
		{
			GUILayout.Label("Achievement", kSubTitleStyle);

			if (m_achievementGIDList.Length == 0)
			{
				GUILayout.Box("Could not find Achievement configuration in GameServices. If you want to access Achievement feature, then please configure it.");
			}
			else
			{
				if (GUILayout.Button("Load Achievement Descriptions"))
				{
					LoadAchievementDescriptions();
				}
				
				if (GUILayout.Button("Load Achievements"))
				{
					LoadAchievements();
				}

				GUILayout.BeginHorizontal();
				{
					if (GUILayout.Button("Previous Achievement"))
						ChangeAchievementGID(false);
					
					if (GUILayout.Button("Next Achievement"))
						ChangeAchievementGID(true);
				}
				GUILayout.EndHorizontal();

				GUILayout.Box(string.Format("Current achievement GID= {0}.", m_curAchievementGID));

				if (GUILayout.Button("Create Achievement"))
				{
					Achievement _achievement 	= CreateAchievementWithGlobalID(m_curAchievementGID);
					AddNewResult(string.Format("Achievement with global identifier {0} is created.", _achievement.GlobalIdentifier));	
				}

				if (GUILayout.Button("Get No Of Steps For Completing Achievement"))
				{
					int			_noOfSteps		= GetNoOfStepsForCompletingAchievement(m_curAchievementGID);
					AddNewResult(string.Format("No of steps required for completing achievement is {0}.", _noOfSteps));	
				}

				if (GUILayout.Button("Report Progress"))
				{
					ReportProgressWithGlobalID(m_curAchievementGID);
				}
			}
		}

		private void DrawUISection ()
		{
			bool 	_canShowLeaderboarAPI	= (m_leaderboardGIDList.Length != 0);
			bool	_canShowAchievementAPI	= (m_achievementGIDList.Length != 0);

			if (_canShowLeaderboarAPI || _canShowAchievementAPI)
				GUILayout.Label("UI", kSubTitleStyle);

			if (_canShowLeaderboarAPI)
			{
				if (GUILayout.Button("Show Leaderboard UI"))
					ShowLeaderboardUIWithGlobalID(m_curLeaderboardGID);
			}
			
			if (_canShowAchievementAPI)
			{
				if (GUILayout.Button("Show Achievements UI"))
					ShowAchievementsUI();
			}
		}

	private void DrawMiscSection ()
	{
		
		GUILayout.Label("Misc", kSubTitleStyle);
		
		if (GUILayout.Button("Load External Authentication Credentials"))
			LoadExternalAuthenticationCredentials();
		
	}
		
		#endregion
		
		#region API Methods
		
		private bool IsAvailable ()
		{
			return NPBinding.GameServices.IsAvailable();
		}

		#endregion

		#region Local User API Methods

		private bool IsAuthenticated ()
		{
			return NPBinding.GameServices.LocalUser.IsAuthenticated;
		}
		
		private void AuthenticateUser ()
		{
			NPBinding.GameServices.LocalUser.Authenticate((bool _success, string _error)=>{

				AddNewResult("Local user authentication finished.");
				AppendResult(string.Format("Error= {0}.", _error.GetPrintableString()));

				if (_success)
				{
					AppendResult(string.Format("Local user details= {0}.", NPBinding.GameServices.LocalUser));
				}
			});
		}

		private void SignOut ()
		{
			NPBinding.GameServices.LocalUser.SignOut((bool _success, string _error)=>{

				if (_success)
				{
					AddNewResult("Local user is signed out successfully!");
				}
				else
				{
					AddNewResult("Request to signout local user failed.");
					AppendResult(string.Format("Error= {0}.", _error.GetPrintableString()));
				}
			});
		}

		private void LoadFriends ()
		{
			NPBinding.GameServices.LocalUser.LoadFriends((User[] _friends, string _error) =>{

				AddNewResult("Load friends info request finished.");
				AppendResult(string.Format("Error= {0}.", _error.GetPrintableString()));

				if (_friends != null)
				{
					foreach (User _curFriend in _friends)
					{
						AppendResult(_curFriend.ToString());
					}
				}
			}); 
		}

		#endregion

		#region User API Methods

		private void LoadUsers (string[] _userIDList)
		{
			NPBinding.GameServices.LoadUsers(_userIDList, (User[] _users, string _error) =>{

				AddNewResult("Load users info request finished.");
				AppendResult(string.Format("Error= {0}.", _error.GetPrintableString()));

				if (_users != null)
				{
					foreach (User _curUser in _users)
					{
						AppendResult(_curUser.ToString());
					}
				}
			});
		}

		#endregion

		#region Leaderboard API Methods

		private Leaderboard CreateLeaderboardWithGlobalID (string _leaderboardGID)
		{
			m_curLeaderboard			= NPBinding.GameServices.CreateLeaderboardWithGlobalID(_leaderboardGID);
			m_curLeaderboard.MaxResults	= m_maxScoreResults;

			return m_curLeaderboard;
		}

		private void LoadPlayerCenteredScores ()
		{
			if (m_curLeaderboard == null)
			{
				AddNewResult("The requested operation could not be completed because leaderboard instance is null. Please create new leaderboard instance.");
				return;
			}
			
			// Load scores
			m_curLeaderboard.LoadPlayerCenteredScores(OnLoadScoresFinished);
		}
		
		private void LoadTopScores ()
		{
			if (m_curLeaderboard == null)
			{
				AddNewResult("The requested operation could not be completed because leaderboard instance is null. Please create new leaderboard instance.");
				return;
			}
			
			// Load scores
			m_curLeaderboard.LoadTopScores(OnLoadScoresFinished);
		}
		
		private void LoadMoreScores (eLeaderboardPageDirection _direction)
		{
			if (m_curLeaderboard == null)
			{
				AddNewResult("The requested operation could not be completed because leaderboard instance is null. Please create new leaderboard instance.");
				return;
			}
			
			// Load scores
			m_curLeaderboard.LoadMoreScores(_direction, OnLoadScoresFinished);
		}
		
		private void ReportScoreWithGlobalID (string _leaderboardGID)
		{
			int		_randomScore	= Random.Range(0, 100);
			
			NPBinding.GameServices.ReportScoreWithGlobalID(_leaderboardGID, _randomScore, (bool _success, string _error)=>{
				
				if (_success)
				{
					AddNewResult(string.Format("Request to report score to leaderboard with GID= {0} finished successfully.", _leaderboardGID));
					AppendResult(string.Format("New score= {0}.", _randomScore));
				}
				else
				{
					AddNewResult(string.Format("Request to report score to leaderboard with GID= {0} failed.", _leaderboardGID));
					AppendResult(string.Format("Error= {0}.", _error.GetPrintableString()));
				}
			});
		}

		#endregion

		#region Achievement API Methods

		private Achievement CreateAchievementWithGlobalID (string _achievementGID)
		{
			return NPBinding.GameServices.CreateAchievementWithGlobalID(_achievementGID);
		}

		private int GetNoOfStepsForCompletingAchievement (string _achievementGID)
		{
			return NPBinding.GameServices.GetNoOfStepsForCompletingAchievement(_achievementGID);
		}

		private void LoadAchievementDescriptions ()
		{
			NPBinding.GameServices.LoadAchievementDescriptions((AchievementDescription[] _descriptions, string _error)=>{

				AddNewResult("Request to load achievement descriptions finished.");
				AppendResult(string.Format("Error= {0}.", _error.GetPrintableString()));

				if (_descriptions != null)
				{
					int		_descriptionCount	= _descriptions.Length;

					AppendResult(string.Format("Total loaded descriptions= {0}.", _descriptionCount));

					for (int _iter = 0; _iter < _descriptionCount; _iter++)
					{
						AppendResult(string.Format("[Index {0}]: {1}", _iter, _descriptions[_iter]));
					}
				}
			});
		}

		private void LoadAchievements ()
		{
			NPBinding.GameServices.LoadAchievements((Achievement[] _achievements, string _error)=>{

				AddNewResult("Request to load achievements finished.");
				AppendResult(string.Format("Error= {0}.", _error.GetPrintableString()));

				if (_achievements != null)
				{
					int		_achievementCount	= _achievements.Length;

					AppendResult(string.Format("Total loaded achievements= {0}.", _achievementCount));

					for (int _iter = 0; _iter < _achievementCount; _iter++)
					{
						AppendResult(string.Format("[Index {0}]: {1}", _iter, _achievements[_iter]));
					}
				}
			});
		}

		private void ReportProgressWithGlobalID (string _achievementGID)
		{
			int 	_noOfSteps	= NPBinding.GameServices.GetNoOfStepsForCompletingAchievement(_achievementGID);
			int		_randomNo	= Random.Range(0, _noOfSteps+1);
			double	_progress	= ((double)_randomNo/_noOfSteps) * 100d;

			// If its an incremental achievement, make sure you send a incremented cumulative value everytime you call this method
			NPBinding.GameServices.ReportProgressWithGlobalID(_achievementGID, _progress, (bool _status, string _error)=>{

				if (_status)
				{
					AddNewResult(string.Format("Request to report progress of achievement with GID= {0} finished successfully.", _achievementGID));
					AppendResult(string.Format("Percentage completed= {0}.", _progress));
				}
				else
				{
					AddNewResult(string.Format("Request to report progress of achievement with GID= {0} failed.", _achievementGID));
					AppendResult(string.Format("Error= {0}.", _error.GetPrintableString()));
				}
			});
		}

		#endregion

		#region UI API Methods

		private void ShowAchievementsUI ()
		{
			AddNewResult("Sending request to show achievements view.");

			NPBinding.GameServices.ShowAchievementsUI((string _error)=>{
				AddNewResult("Achievements view dismissed.");
				AppendResult(string.Format("Error= {0}.", _error.GetPrintableString()));
			});
		}

		private void ShowLeaderboardUIWithGlobalID (string _leaderboadGID)
		{
			AddNewResult("Sending request to show leaderboard view.");

			NPBinding.GameServices.ShowLeaderboardUIWithGlobalID(_leaderboadGID, m_timeScope, (string _error)=>{
				AddNewResult("Leaderboard view dismissed.");
				AppendResult(string.Format("Error= {0}.", _error.GetPrintableString()));
			});
		}

		#endregion

		#region Misc API Methods

		private void LoadExternalAuthenticationCredentials ()
		{
			AddNewResult("Sending request to Load External Auth Credentials.");
			NPBinding.GameServices.LoadExternalAuthenticationCredentials((ExternalAuthenticationCredentials _credentials, string _error) => {
				AddNewResult("LoadExternalAuthenticationCredentials Finished");
				AppendResult(string.Format("Error= {0}.", _error.GetPrintableString()));

				if (_credentials != null)
				{
					AppendResult(_credentials.AndroidCredentials.ServerAuthCode);
				}
			});
		}
	
		#endregion

		#region API Callback Methods
		
		private void OnLoadScoresFinished (Score[] _scores, Score _localUserScore, string _error)
		{
			AddNewResult("Load leaderboard scores request finished.");
			AppendResult(string.Format("Error= {0}.", _error.GetPrintableString()));

			if (_scores != null)
			{
				int		_scoresCount	= _scores.Length;
				
				AppendResult(string.Format("Totally {0} score entries were loaded.", _scoresCount));
				AppendResult(string.Format("Local user score= {0}.", _localUserScore == null ? "NULL" : _localUserScore.ToString()));
				
				for (int _iter = 0; _iter < _scoresCount; _iter++)
				{
					AppendResult(string.Format("[Index {0}]: {1}.", _iter, _scores[_iter]));
				}
			}
		}

		#endregion

		#region Misc Methods
		
		private void ExtractGID ()
		{			
			// Extract leaderboard id infomation
			LeaderboardMetadata[] 	_leaderboardMetadataCollection	= NPSettings.GameServicesSettings.LeaderboardMetadataCollection;
			int			_leaderboardCount		= _leaderboardMetadataCollection.Length;
			m_leaderboardGIDList				= new string[_leaderboardCount];
			
			for (int _iter = 0; _iter < _leaderboardCount; _iter++)
				m_leaderboardGIDList[_iter]		= _leaderboardMetadataCollection[_iter].GlobalID;

			// Extract achievement id information
			AchievementMetadata[] 	_achievementMetadataCollection	= NPSettings.GameServicesSettings.AchievementMetadataCollection;
			int			_achievementCount		= _achievementMetadataCollection.Length;
			m_achievementGIDList				= new string[_achievementCount];
			
			for (int _iter = 0; _iter < _achievementCount; _iter++)
				m_achievementGIDList[_iter]		= _achievementMetadataCollection[_iter].GlobalID;
		}

		private void ChangeLeaderboardGID (bool _gotoNext)
		{
			int		 _identifierCount	= m_leaderboardGIDList.Length;

			if (_gotoNext)
			{
				// Move to next index
				m_curLeaderboardGIDIndex++;
				
				if (m_curLeaderboardGIDIndex >=  _identifierCount)
					m_curLeaderboardGIDIndex	= 0;
			}
			else
			{
				// Move to previous index
				m_curLeaderboardGIDIndex--;
				
				if (m_curLeaderboardGIDIndex < 0)
					m_curLeaderboardGIDIndex	=  _identifierCount - 1;
			}

			// Set id
			m_curLeaderboardGID			= m_leaderboardGIDList[m_curLeaderboardGIDIndex];
		}

		private void ChangeAchievementGID (bool _gotoNext)
		{
			int		 _identifierCount	= m_achievementGIDList.Length;
			
			if (_gotoNext)
			{
				// Move to next index
				m_curAchievementGIDIndex++;
				
				if (m_curAchievementGIDIndex >=  _identifierCount)
					m_curAchievementGIDIndex	= 0;
			}
			else
			{
				// Move to previous index
				m_curAchievementGIDIndex--;
				
				if (m_curAchievementGIDIndex < 0)
					m_curAchievementGIDIndex	=  _identifierCount - 1;
			}
			
			// Set id
			m_curAchievementGID	= m_achievementGIDList[m_curAchievementGIDIndex];
		}

		#endregion
	}
#endif
}
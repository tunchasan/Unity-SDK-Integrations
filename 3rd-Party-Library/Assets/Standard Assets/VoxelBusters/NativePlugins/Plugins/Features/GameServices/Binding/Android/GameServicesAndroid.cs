#if USES_GAME_SERVICES && UNITY_ANDROID
using UnityEngine;
using System.Collections;
using System;
using System.Runtime.InteropServices;
using VoxelBusters.Utility;

namespace VoxelBusters.NativePlugins
{
	using Internal;

	public sealed partial class GameServicesAndroid : GameServices 
	{
		#region  Variables

		private AndroidLocalUser m_localUser;
		public override LocalUser LocalUser 
		{
			get 
			{
				return m_localUser;
			}
			
			protected set 
			{
				m_localUser = value as AndroidLocalUser;
			}
		}
			
		#endregion
		
		#region Constructors
		
		GameServicesAndroid()
		{
			Plugin = AndroidPluginUtility.GetSingletonInstance(Native.Class.NAME);
		}
		
		#endregion

		#region Unity Methods

		protected override void Awake ()
		{
			base.Awake ();

			// Initialize
			LocalUser	= new AndroidLocalUser();

			// Register native API Service
			Plugin.Call(Native.Methods.REGISTER_SERVICE, NPSettings.Application.SupportedFeatures.UsesCloudServices, NPSettings.GameServicesSettings.Android.AllowAutoLogin);
			Plugin.Call(Native.Methods.SET_SHOW_DEFAULT_ERROR_DIALOGS, NPSettings.GameServicesSettings.Android.ShowDefaultErrorDialogs);
		}

		#endregion

		#region Methods

		public override bool IsAvailable ()
		{
			return (Plugin != null) && (Plugin.Call<bool>(Native.Methods.IS_SERVICE_AVAILABLE));
		}

		protected override Leaderboard CreateLeaderboard (string _leaderboarGID, string _leaderboardID)
		{
			// Verify auth status
			if (!VerifyUser())
				return null;

			// Check if identifier is valid
			if (string.IsNullOrEmpty(_leaderboardID))
				return null;

			return new AndroidLeaderboard(_leaderboarGID, _leaderboardID);
		}

		protected override Achievement CreateAchievement (string _achievementGID, string _achievementID)
		{
			// Verify auth status
			if (!VerifyUser())
				return null;

			// Check if identifier is valid
			if (string.IsNullOrEmpty(_achievementID))
				return null;

			return new AndroidAchievement(_achievementGID, _achievementID);
		}

		protected override void LoadAchievementDescriptions (bool _needsVerification, AchievementDescription.LoadAchievementDescriptionsCompletion _onCompletion)
		{
			base.LoadAchievementDescriptions(_needsVerification, _onCompletion);
			
			// Verify user authentication state before proceeding
			if (_needsVerification && !VerifyUser())
			{				
				return;
			}
			
			Plugin.Call(Native.Methods.LOAD_ACHIEVEMENT_DESCRIPTIONS);
		}
		
		public override void LoadAchievements (Achievement.LoadAchievementsCompletion _onCompletion)
		{
			base.LoadAchievements(_onCompletion);

			// Verify user authentication state before proceeding
			if (!VerifyUser())
			{	
				return;
			}

			Plugin.Call(Native.Methods.LOAD_ACHIEVEMENTS);
		}
		
		public override void LoadUsers (string[] _userIDs, User.LoadUsersCompletion _onCompletion)
		{
			base.LoadUsers(_userIDs, _onCompletion);

			if (_userIDs == null)
				return;

			if (!VerifyUser())
			{	
				return;
			}

			string _usersListJSON	   = _userIDs.ToJSON();			
			Plugin.Call(Native.Methods.LOAD_USERS, GetInstanceID().ToString(), _usersListJSON);
		}

		// Report score
		protected override Score CreateScoreForLocalUser (string _leaderboardGID, string _leaderboardID)
		{
			// Verify auth status
			if (!VerifyUser())
				return null ;
			
			// Verify id
			if (string.IsNullOrEmpty(_leaderboardID))
				return null;
			
			// Create instance
			AndroidScore		_instance		= new AndroidScore(_leaderboardGID, _leaderboardID, LocalUser);
			return _instance;
		}
		
		public override void ShowAchievementsUI (GameServiceViewClosed _onCompletion)
		{
			base.ShowAchievementsUI(_onCompletion);
			
			// Verify auth status
			if (!VerifyUser())
				return;
			
			// Native method call
			Plugin.Call(Native.Methods.SHOW_ACHIEVEMENTS_UI);
		}
		
		public override void ShowLeaderboardUIWithID (string _leaderboardID, eLeaderboardTimeScope _timeScope, GameServiceViewClosed _onCompletion)
		{
			base.ShowLeaderboardUIWithID(_leaderboardID, _timeScope, _onCompletion);
			
			// Verify auth status
			if (!VerifyUser())
				return;
			
			// Native method call
			string _timeScopeString = AndroidLeaderboard.kTimeScopeMap.GetKey<eLeaderboardTimeScope>(_timeScope);
			Plugin.Call(Native.Methods.SHOW_LEADERBOARD_UI, _leaderboardID, _timeScopeString);
		}

		public override void LoadExternalAuthenticationCredentials(LoadExternalAuthenticationCredentialsCompletion _onCompletion)
		{
			base.LoadExternalAuthenticationCredentials(_onCompletion);

			// Verify auth status
			if (!VerifyUser())
				return;
			
			// Native method call
			Plugin.Call(Native.Methods.LOAD_EXTERNAL_AUTHENTICATION_DETAILS, NPSettings.GameServicesSettings.Android.ServerClientID);
		}
		#endregion
	}
}
#endif
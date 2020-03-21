#if USES_GAME_SERVICES && UNITY_IOS
using UnityEngine;
using System.Collections;
using System;
using System.Runtime.InteropServices;
using VoxelBusters.Utility;

namespace VoxelBusters.NativePlugins
{
	using Internal;

	public sealed partial class GameServicesIOS : GameServices 
	{
		#region Fields

		private			iOSLocalUser	m_localUser;

		#endregion

		#region Properties

		public override LocalUser LocalUser 
		{
			get 
			{
				return m_localUser;
			}

			protected set 
			{
				m_localUser = value as iOSLocalUser;
			}
		}

		#endregion
		
		#region External Methods

		[DllImport("__Internal")]
		private static extern bool isGameCenterAvailable ();

		[DllImport("__Internal")]
		private static extern void loadAchievementDescriptions ();
		
		[DllImport("__Internal")]
		private static extern void loadAchievements ();

		[DllImport("__Internal")]
		private static extern void loadPlayers (string _identifiersJSON);

		[DllImport("__Internal")]
		private static extern void showDefaultAchievementCompletionBanner (bool _show);

		[DllImport("__Internal")]
		private static extern void showLeaderboardView (string _leaderboardID, int _timeScope);

		[DllImport("__Internal")]
		private static extern void showAchievementView ();

		#endregion 

		#region Unity Methods

		protected override void Awake ()
		{
			base.Awake();

			// Initialize
			LocalUser	= new iOSLocalUser();

			// Set Settings
			ShowDefaultAchievementCompletionBanner(NPSettings.GameServicesSettings.IOS.ShowDefaultAchievementCompletionBanner);
		}

		#endregion

		#region Methods

		public override bool IsAvailable ()
		{
			return isGameCenterAvailable();
		}

		#endregion
		
		#region Leaderboard Methods

		protected override Leaderboard CreateLeaderboard (string _leaderboarGID, string _leaderboardID)
		{
			// Verify auth status
			if (!VerifyUser())
				return null;

			// Check if identifier is valid
			if (string.IsNullOrEmpty(_leaderboardID))
				return null;
			
			return new iOSLeaderboard(_leaderboarGID, _leaderboardID);
		}

		#endregion

		#region Achievement Description Methods
		
		protected override void LoadAchievementDescriptions (bool _needsVerification, AchievementDescription.LoadAchievementDescriptionsCompletion _onCompletion)
		{
			base.LoadAchievementDescriptions(_needsVerification, _onCompletion);
			
			// Verify auth status
			if (_needsVerification && !VerifyUser())
				return;

			// Native method call
			loadAchievementDescriptions();
		}

		#endregion

		#region Achievement Methods

		protected override Achievement CreateAchievement (string _achievementGID, string _achievementID)
		{
			// Verify auth status
			if (!VerifyUser())
				return null;

			// Check if identifier is valid
			if (string.IsNullOrEmpty(_achievementID))
				return null;

			return new iOSAchievement(_achievementGID, _achievementID);
		}

		public override void LoadAchievements (Achievement.LoadAchievementsCompletion _onCompletion)
		{
			base.LoadAchievements(_onCompletion);

			// Verify auth status
			if (!VerifyUser())
				return;

			// Native method call
			loadAchievements();
		}
		
		#endregion
		
		#region User Methods

		public override void LoadUsers (string[] _userIDs, User.LoadUsersCompletion _onCompletion)
		{
			base.LoadUsers(_userIDs, _onCompletion);

			// Verify auth status
			if (!VerifyUser())
				return;

			// Verify id's
			if (_userIDs == null)
				return;

			loadPlayers(_userIDs.ToJSON());
		}

		#endregion

		#region Score Methods

		protected override Score CreateScoreForLocalUser (string _leaderboardGID, string _leaderboardID)
		{
			// Verify auth status
			if (!VerifyUser())
				return null ;
			
			// Verify id
			if (string.IsNullOrEmpty(_leaderboardID))
				return null;

			// Create instance
			return new iOSScore(_leaderboardGID, _leaderboardID, LocalUser);
		}

		#endregion

		#region UI Methods

		public override void ShowAchievementsUI (GameServiceViewClosed _onCompletion)
		{
			base.ShowAchievementsUI(_onCompletion);

			// Verify auth status
			if (!VerifyUser())
				return;

			// Native method call
			showAchievementView();
		}
		
		public override void ShowLeaderboardUIWithID (string _leaderboardID, eLeaderboardTimeScope _timeScope, GameServiceViewClosed _onCompletion)
		{
			base.ShowLeaderboardUIWithID(_leaderboardID, _timeScope, _onCompletion);
			
			// Verify auth status
			if (!VerifyUser())
				return;

			// Native method call
			showLeaderboardView(_leaderboardID, (int)_timeScope);
		}

		#endregion

		#region Private Methods

		private void ShowDefaultAchievementCompletionBanner (bool _canShow)
		{
			// Native method call
			showDefaultAchievementCompletionBanner(_canShow);
		}

		#endregion
	}
}
#endif
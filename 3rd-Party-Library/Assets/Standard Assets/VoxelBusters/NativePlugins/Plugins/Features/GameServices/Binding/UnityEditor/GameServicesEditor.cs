using UnityEngine;
using System.Collections;

#if USES_GAME_SERVICES && UNITY_EDITOR
using System;

namespace VoxelBusters.NativePlugins
{
	using Internal;

	public sealed partial class GameServicesEditor : GameServices 
	{
		#region Fields
		
		private		EditorLocalUser		m_localUser;
		
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
				m_localUser = (EditorLocalUser)value;
			}
		}
		
		#endregion

		#region Unity Methods

		protected override void Awake ()
		{
			base.Awake ();

			// Initialize
			LocalUser	= new EditorLocalUser();
		}

		#endregion
		
		#region Methods
		
		public override bool IsAvailable ()
		{
			return true;
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

			return new EditorLeaderboard(_leaderboarGID, _leaderboardID);
		}
		
		#endregion
		
		#region Achievement Description Methods
		
		protected override void LoadAchievementDescriptions (bool _needsVerification, AchievementDescription.LoadAchievementDescriptionsCompletion _onCompletion)
		{
			base.LoadAchievementDescriptions(_needsVerification, _onCompletion);
			
			// Verify auth status
			if (_needsVerification && !VerifyUser())
				return;
			
			EditorGameCenter.Instance.LoadAchievementDescriptions();
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
			
			return new EditorAchievement(_achievementGID, _achievementID);
		}
		
		public override void LoadAchievements (Achievement.LoadAchievementsCompletion _onCompletion)
		{
			base.LoadAchievements(_onCompletion);
			
			// Verify auth status
			if (!VerifyUser())
				return;
			
			EditorGameCenter.Instance.LoadAchievements();
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
			
			EditorGameCenter.Instance.LoadUsers(_userIDs);
		}
		
		#endregion
		
		#region Score Methods

		protected override Score CreateScoreForLocalUser (string _leaderboardGID, string _leaderboardID)
		{
			// Verify auth status
			if (!VerifyUser())
				return null;
			
			// Verify id
			if (string.IsNullOrEmpty(_leaderboardID))
				return null;

			return new EditorScore(_leaderboardGID, _leaderboardID, LocalUser);
		}
		
		#endregion
		
		#region UI Methods
		
		public override void ShowAchievementsUI (GameServiceViewClosed _onCompletion)
		{
			base.ShowAchievementsUI(_onCompletion);
			
			// Verify auth status
			if (!VerifyUser())
				return;
			
			EditorGameCenter.Instance.ShowAchievementsUI();
		}
		
		public override void ShowLeaderboardUIWithID (string _leaderboardID, eLeaderboardTimeScope _timeScope, GameServiceViewClosed _onCompletion)
		{
			base.ShowLeaderboardUIWithID(_leaderboardID, _timeScope, _onCompletion);
			
			// Verify auth status
			if (!VerifyUser())
				return;
			
			EditorGameCenter.Instance.ShowLeaderboardUI(_leaderboardID, _timeScope);
		}
		
		#endregion

		#region Misc Methods

		public override void LoadExternalAuthenticationCredentials(LoadExternalAuthenticationCredentialsCompletion _onCompletion)
		{
			base.LoadExternalAuthenticationCredentials(_onCompletion);
			
			// Verify auth status
			if (!VerifyUser())
				return;

			LoadExternalAuthenticationCredentialsFinished(null, Constants.kNotSupportedInEditor);	
		}

		#endregion
	}
}
#endif
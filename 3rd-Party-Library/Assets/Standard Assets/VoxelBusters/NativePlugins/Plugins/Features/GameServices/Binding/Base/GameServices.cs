#if USES_GAME_SERVICES
using UnityEngine;
using System.Collections;
using System;
using VoxelBusters.Utility;
using VoxelBusters.UASUtils;

namespace VoxelBusters.NativePlugins
{
	using Internal;

	/// <summary>
	/// Provides cross-platform interface to easily integrate popular social gaming functionalities such as achievements, leaderboards on your mobile games.
	/// </summary>
	/// <description>
	/// <para>
	/// Internally, game services feature uses native game servers for handling functionalities. 
	/// So while running your game on iOS devices, Game Center servers will be used. Whereas on Android platform, Play Game Services server will be used. 
	/// </para>
	/// <para>
	/// Goto <a href="https://developer.apple.com/library/ios/documentation/LanguagesUtilities/Conceptual/iTunesConnectGameCenter_Guide/AccessAndEnable/AccessAndEnable.html">iTune's Connect</a> to configure leaderboard, achievemnts for your iOS game. 
	/// And for Android, add records at <a href="https://developers.google.com/games/services/android/quickstart">Google Developer Console</a>.
	/// </para>
	/// </description>
	public partial class GameServices : MonoBehaviour 
	{
		#region Properties

		/// <summary>
		/// Retrieves the shared instance of the authenticated user running your game. (read-only)
		/// </summary>
		public virtual LocalUser LocalUser
		{
			get
			{
				return null;
			}

			protected set
			{
				throw new Exception("[GameServices] Only getter is supported.");
			}
		}

		#endregion

		#region Unity Methods

		protected virtual void Awake ()
		{
			// Initialise
			SetLeaderboardMetadataCollection(NPSettings.GameServicesSettings.LeaderboardMetadataCollection);
			SetAchievementMetadataCollection(NPSettings.GameServicesSettings.AchievementMetadataCollection);
		}

		#endregion

		#region Methods

		/// <summary>
		/// Determines whether the game services feature is supported.
		/// </summary>
		/// <returns><c>true</c> if this game services feature is available; otherwise, <c>false</c>.</returns>
		public virtual bool IsAvailable ()
		{
			return false;
		}

		#endregion

		#region Leaderboard Methods

		/// <summary>
		/// Loads the additional information of all the leaderboards that are used.
		/// </summary>
		/// <param name="_collection">An array of <see cref="LeaderboardMetadata"/> objects, that holds additional information of the leaderboards.</param>
		public void SetLeaderboardMetadataCollection (params LeaderboardMetadata[] _collection)
		{
			GameServicesUtils.leaderboardMetadataCollection	= _collection;
		}

		/// <summary>
		/// Creates a new instance of leaderboard object, using platform specific id.
		/// </summary>
		/// <param name="_leaderboardID">A string used to identify the leaderboard in the current platform.</param>
		public Leaderboard CreateLeaderboardWithID (string _leaderboardID)
		{
			string	_leaderboardGID	= GameServicesUtils.GetLeaderboardGID(_leaderboardID);

			return CreateLeaderboard(_leaderboardGID, _leaderboardID);
		}

		/// <summary>
		/// Creates a new instance of leaderboard object, using unified id.
		/// </summary>
		/// <param name="_leaderboardGID">An unified string internally used to identify the leaderboard across all the supported platforms.</param>
		/// <remarks>
		/// \note Works only if, leaderboard metadata was configured in NPSettings or else explicitely set using <see cref="SetLeaderboardMetadataCollection"/>.
		/// </remarks>
		public Leaderboard CreateLeaderboardWithGlobalID (string _leaderboardGID)
		{
			string	_leaderboardID	= GameServicesUtils.GetLeaderboardID(_leaderboardGID);

			return CreateLeaderboard(_leaderboardGID, _leaderboardID);
		}

		protected virtual Leaderboard CreateLeaderboard (string _leaderboardGID, string _leaderboardID)
		{
			return null;
		}

		#endregion

		#region Achievement Description Methods

		/// <summary>
		/// Loads the achievement descriptions from game server.
		/// </summary>
		/// <param name="_onCompletion">Callback that will be called after operation is completed.</param>
		public void LoadAchievementDescriptions (AchievementDescription.LoadAchievementDescriptionsCompletion _onCompletion)
		{
			LoadAchievementDescriptions(true, _onCompletion);
		}

		protected virtual void LoadAchievementDescriptions (bool _needsVerification, AchievementDescription.LoadAchievementDescriptionsCompletion _onCompletion)
		{
			// Cache callback
			LoadAchievementDescriptionsFinishedEvent = _onCompletion;

			// Verify user authentication state before proceeding
			if (_needsVerification && !VerifyUser())
			{
				LoadAchievementDescriptionsFinished(null, Constants.kGameServicesUserAuthMissingError);
				return;
			}
		}

		#endregion

		#region Achievement Methods

		/// <summary>
		/// Loads the additional information of all the achievements that are used.
		/// </summary>
		/// <param name="_collection">An array of <see cref="AchievementMetadata"/> objects, that holds additional information of the achievements.</param>
		public void SetAchievementMetadataCollection (params AchievementMetadata[] _collection)
		{
			GameServicesUtils.achievementMetadataCollection	= _collection;
		}
		
		/// <summary>
		/// Creates a new instance of achievement object, using platform specific id.
		/// </summary>
		/// <param name="_achievementID">A string used to identify achievement in the current platform.</param>
		public Achievement CreateAchievementWithID (string _achievementID)
		{
			string	_achievementGID	= GameServicesUtils.GetAchievementGID(_achievementID);
			
			return CreateAchievement(_achievementGID, _achievementID);
		}
		
		/// <summary>
		/// Creates a new instance of achievement object, using unified id.
		/// </summary>
		/// <param name="_achievementGID">An unified string internally used to identify the achievement across all the supported platforms.</param>
		/// <remarks>
		/// \note Works only if, achievement metadata was configured in NPSettings or else explicitely set using <see cref="SetAchievementMetadataCollection"/>.
		/// </remarks>
		public Achievement CreateAchievementWithGlobalID (string _achievementGID)
		{
			string	_achievementID	= GameServicesUtils.GetAchievementID(_achievementGID);
			
			return CreateAchievement(_achievementGID, _achievementID);
		}

		protected virtual Achievement CreateAchievement (string _achievementGID, string _achievementID)
		{
			return null;
		}

		/// <summary>
		/// Loads previously submitted achievement progress for the current local user.
		/// </summary>
		/// <param name="_onCompletion">Callback that will be called after operation is completed.</param>
		public virtual void LoadAchievements (Achievement.LoadAchievementsCompletion _onCompletion)
		{
			// Cache callback
			LoadAchievementsFinishedEvent = _onCompletion;

			// Verify auth status
			if (!VerifyUser())
			{
				LoadAchievementsFinished(null, Constants.kGameServicesUserAuthMissingError);
				return;
			}
		}

		/// <summary>
		/// Reports the local user's achievement progress to game server, using platform specific id.
		/// </summary>
		/// <param name="_achievementID">A string used to identify achievement in the current platform.</param>
		/// <param name="_percentageCompleted">The value indicates how far the player has progressed.</param>
		/// <param name="_onCompletion">Callback that will be called after operation is completed.</param>
		public void ReportProgressWithID (string _achievementID, double _percentageCompleted, Achievement.ReportProgressCompletion _onCompletion)
		{
			string	_achievementGID	= GameServicesUtils.GetAchievementGID(_achievementID);

			// Invoke handler
			ReportProgress(_achievementGID, _achievementID, _percentageCompleted, _onCompletion);
		}

		/// <summary>
		/// Reports the local user's achievement progress to game server, using unified id.
		/// </summary>
		/// <param name="_achievementGID">An unified string internally used to identify the achievement across all the supported platforms.</param>
		/// <param name="_percentageCompleted">The value indicates how far the player has progressed.</param>
		/// <param name="_onCompletion">Callback that will be called after operation is completed.</param>
		/// <remarks>
		/// \note Works only if, achievement metadata was configured in NPSettings or else explicitely set using <see cref="SetAchievementMetadataCollection"/>.
		/// </remarks>
		public void ReportProgressWithGlobalID (string _achievementGID, double _percentageCompleted, Achievement.ReportProgressCompletion _onCompletion)
		{
			string	_achievementID	= GameServicesUtils.GetAchievementID(_achievementGID);

			// Invoke handler
			ReportProgress(_achievementGID, _achievementID, _percentageCompleted, _onCompletion);
		}

		private void ReportProgress (string _achievementGID, string _achievementID, double _percentageCompleted, Achievement.ReportProgressCompletion _onCompletion)
		{
			Achievement	_newAchievement 	= CreateAchievement(_achievementGID, _achievementID);

			if (_newAchievement == null)
			{
				DebugUtility.Logger.LogError(Constants.kDebugTag, "[GameServices] Failed to report progress.");
				
				if (_onCompletion != null)
					_onCompletion(false, "The requested operation could not be completed because Game Service failed to create Achievement object.");
				
				return;
			}

			// Set the new progress value
			_newAchievement.PercentageCompleted	= _percentageCompleted;

			// Report it
			_newAchievement.ReportProgress(_onCompletion);
		}

		/// <summary>
		/// Returns the number of steps required for completing the specified achievement.
		/// </summary>
		/// <returns>The no of steps required to complete specified achievement.</returns>
		/// <param name="_achievementGID">An unified string internally used to identify the achievement across all the supported platforms.</param>
		/// <remarks>
		/// \note Works only if, achievement metadata was configured in NPSettings or else explicitely set using <see cref="SetAchievementMetadataCollection"/>.
		/// </remarks>
		public int GetNoOfStepsForCompletingAchievement (string _achievementGID)
		{
			AchievementMetadata _achievementMetadata	= (AchievementMetadata)GameServicesUtils.achievementMetadataCollection.FindObjectWithGlobalID(_achievementGID);

			if (_achievementMetadata == null)
				return -1;

			return _achievementMetadata.NoOfSteps;
		}

		#endregion

		#region User Methods

		/// <summary>
		/// Loads the user details from game server.
		/// </summary>
		/// <param name="_userIDs">An array of user id's whose details has to be retrieved from game server.</param>
		/// <param name="_onCompletion">Callback that will be called after operation is completed.</param>
		public virtual void LoadUsers (string[] _userIDs, User.LoadUsersCompletion _onCompletion)
		{
			// Cache callback
			LoadUsersFinishedEvent = _onCompletion;

			// Verify auth status
			if (!VerifyUser())
			{
				LoadUsersFinished(null, Constants.kGameServicesUserAuthMissingError);
				return;
			}

			// Check if user id's are valid
			if (_userIDs == null)
			{
				DebugUtility.Logger.LogError(Constants.kDebugTag, "[GameServices] UserID list is null.");
				LoadUsersFinished(null, "The requested operation could not be completed because user id list is null.");
				return;
			}
		}

		#endregion

		#region Score Methods

		protected virtual Score CreateScoreForLocalUser (string _leaderboardGID, string _leaderboardID)
		{
			return null;
		}

		/// <summary>
		/// Report a score to game server, using platform specific id.
		/// </summary>
		/// <param name="_leaderboardID">A string used to identify the leaderboard in the current platform.</param>
		/// <param name="_score">The score earned by the local user.</param>
		/// <param name="_onCompletion">Callback that will be called after operation is completed.</param>
		public void ReportScoreWithID (string _leaderboardID, long _score, Score.ReportScoreCompletion _onCompletion)
		{
			string	_leaderboardGID	= GameServicesUtils.GetLeaderboardGID(_leaderboardID);

			// Invoke handler
			ReportScore(_leaderboardGID, _leaderboardID, _score, _onCompletion);
		}

		/// <summary>
		/// Report a score to game server, using unified id.
		/// </summary>
		/// <param name="_leaderboardGID">An unified string internally used to identify the leaderboard across all the supported platforms.</param>
		/// <param name="_score">The score earned by the local user.</param>
		/// <param name="_onCompletion">Callback that will be called after operation is completed.</param>
		/// <remarks>
		/// \note Works only if, leaderboard metadata was configured in NPSettings or else explicitely set using <see cref="SetLeaderboardMetadataCollection"/>.
		/// </remarks>
		public void ReportScoreWithGlobalID (string _leaderboardGID, long _score, Score.ReportScoreCompletion _onCompletion)
		{
			string	_leaderboardID	= GameServicesUtils.GetLeaderboardID(_leaderboardGID);

			// Invoke handler
			ReportScore(_leaderboardGID, _leaderboardID, _score, _onCompletion);
		}

		private void ReportScore (string _leaderboardGID, string _leaderboardID, long _score, Score.ReportScoreCompletion _onCompletion)
		{
			Score	_newScore		= CreateScoreForLocalUser(_leaderboardGID, _leaderboardID);

			if (_newScore == null)
			{
				DebugUtility.Logger.LogError(Constants.kDebugTag, "[GameServices] Failed to report score.");

				if (_onCompletion != null)
					_onCompletion(false, "The requested operation could not be completed because Game Service failed to create Score object.");
				
				return;
			}

			// Set the new score value
			_newScore.Value			= _score;

			// Report
			_newScore.ReportScore(_onCompletion);
		}

		#endregion

		#region UI Methods

		/// <summary>
		/// Opens standard view to display achievement progress screen for the local player.
		/// </summary>
		/// <param name="_onCompletion">Callback that will be called after operation is completed.</param>
		public virtual void ShowAchievementsUI (GameServiceViewClosed _onCompletion)
		{
			// Cache callback
			ShowAchievementViewFinishedEvent	= _onCompletion;
			
			// Pause unity
			this.PauseUnity();
			
			// Check if valid account
			if (!VerifyUser())
			{
				ShowAchievementViewFinished(Constants.kGameServicesUserAuthMissingError);
				return;
			}
		}

		/// <summary>
		/// Opens standard view to display leaderboard scores corresponding to given platform specific id.
		/// </summary>
		/// <param name="_leaderboardID">A string used to identify the leaderboard in the current platform.</param>
		/// <param name="_timeScope">A time filter used to restrict which scores are displayed to the user.</param>
		/// <param name="_onCompletion">Callback that will be called after operation is completed.</param>
		/// <remarks>
		/// \note Incase, if you want to list out all the leaderboards that are used in your game, then pass <c>null</c> for leaderboard identifier.
		/// </remarks>
		public virtual void ShowLeaderboardUIWithID (string _leaderboardID, eLeaderboardTimeScope _timeScope, GameServiceViewClosed _onCompletion)
		{
			// Cache callback
			ShowLeaderboardViewFinishedEvent	= _onCompletion;

			// Pause unity
			this.PauseUnity();

			// Check if valid account
			if (!VerifyUser())
			{
				ShowLeaderboardViewFinished(Constants.kGameServicesUserAuthMissingError);
				return;
			}
		}

		/// <summary>
		/// Opens standard view to display leaderboard scores corresponding to given unified id.
		/// </summary>
		/// <param name="_leaderboardGID">An unified string internally used to identify the leaderboard across all the supported platforms.</param>
		/// <param name="_timeScope">A time filter used to restrict which scores are displayed to the user.</param>
		/// <param name="_onCompletion">Callback that will be called after operation is completed.</param>
		/// <remarks>
		/// Incase, if you want to list out all the leaderboards that are used in your game, then pass <c>null</c> for leaderboard identifier.
		/// \note Works only if, leaderboard metadata was configured in NPSettings or else explicitely set using <see cref="SetLeaderboardMetadataCollection"/>.
		/// </remarks>
		public void ShowLeaderboardUIWithGlobalID (string _leaderboardGID, eLeaderboardTimeScope _timeScope, GameServiceViewClosed _onCompletion)
		{
			string	_leaderboardID	= GameServicesUtils.GetLeaderboardID(_leaderboardGID);

			ShowLeaderboardUIWithID(_leaderboardID, _timeScope, _onCompletion);
		}

		#endregion

		#region Misc. Methods

		protected bool VerifyUser ()
		{
			if (LocalUser.IsAuthenticated)
				return true;

			DebugUtility.Logger.LogError(Constants.kDebugTag, "[GameServices] User not authenticated.");
			return false;
		}

		public virtual void LoadExternalAuthenticationCredentials (LoadExternalAuthenticationCredentialsCompletion _onCompletion)
		{
			LoadExternalAuthenticationCredentialsFinishedEvent = _onCompletion;

			if (!VerifyUser())
			{
				LoadExternalAuthenticationCredentialsFinished(null, Constants.kGameServicesUserAuthMissingError);
				return;
			}
		}

		#endregion

		#region Deprecated Methods

		[System.Obsolete("This method is deprecated. Instead use SetLeaderboardMetadataCollection.")]
		public void SetLeaderboardIDCollection (params IDContainer[] _idCollection)
		{
			int	_count	= _idCollection.Length;
			LeaderboardMetadata[]	_metadataCollection	= new LeaderboardMetadata[_count];

			for (int _iter = 0; _iter < _count; _iter++)
				_metadataCollection[_iter]	= LeaderboardMetadata.Create(_idCollection[_iter]);

			SetLeaderboardMetadataCollection(_metadataCollection);
		}

		[System.Obsolete("This method is deprecated. Instead use CreateLeaderboardWithID.")]
		public Leaderboard CreateLeaderboard (string _leaderboardID)
		{
			return CreateLeaderboardWithID(_leaderboardID);
		}

		[System.Obsolete("This method is deprecated. Instead use SetAchievementMetadataCollection.")]
		public void SetAchievementIDCollection (params IDContainer[] _idCollection)
		{
			int	_count	= _idCollection.Length;
			AchievementMetadata[]	_metadataCollection	= new AchievementMetadata[_count];
			
			for (int _iter = 0; _iter < _count; _iter++)
				_metadataCollection[_iter]	= AchievementMetadata.Create(_idCollection[_iter]);
			
			SetAchievementMetadataCollection(_metadataCollection);
		}

		[System.Obsolete("This method is deprecated. Instead use CreateAchievementWithID.")]
		public Achievement CreateAchievement (string _achievementID)
		{
			return CreateAchievementWithID(_achievementID);
		}

		[System.Obsolete("This method is deprecated.  Instead use ReportProgressWithID.")]
		public void ReportProgress (string _achievementID, int _pointsScored, Achievement.ReportProgressCompletion _onCompletion)
		{
			ReportProgressWithID(_achievementID, _pointsScored, _onCompletion);
		}

		[System.Obsolete("This method is deprecated. Use ReportProgressWithID which takes percentage value as progress.")]
		public void ReportProgressWithID (string _achievementID, int _pointsScored, Achievement.ReportProgressCompletion _onCompletion)
		{
			string	_achievementGID	= GameServicesUtils.GetAchievementGID(_achievementID);
		
			// Invoke handler
			ReportProgress(_achievementGID, _achievementID, _pointsScored, _onCompletion);
		}

		[System.Obsolete("This method is deprecated. Use ReportProgressWithGlobalID which takes percentage value as progress.")]
		public void ReportProgressWithGlobalID (string _achievementGID, int _pointsScored, Achievement.ReportProgressCompletion _onCompletion)
		{
			string	_achievementID	= GameServicesUtils.GetAchievementID(_achievementGID);

			// Invoke handler
			ReportProgress(_achievementGID, _achievementID, _pointsScored, _onCompletion);
		}

		private void ReportProgress (string _achievementGID, string _achievementID, int _pointsScored, Achievement.ReportProgressCompletion _onCompletion)
		{
			// Retrieve associated description 
			AchievementDescription	_description		= AchievementHandler.GetAchievementDescriptionWithGlobalID(_achievementGID);
			
			if (_description == null)
			{
				DebugUtility.Logger.LogError(Constants.kDebugTag, "[GameServices] Failed to report progress.");
				
				if (_onCompletion != null)
					_onCompletion(false, "The requested operation could not be completed because Game Services couldn't find description for given Achievement identifier.");
				
				return;
			}

			// Create achivement object
			Achievement				_newAchievement 	= CreateAchievement(_achievementGID, _achievementID);

			if (_newAchievement == null)
			{
				DebugUtility.Logger.LogError(Constants.kDebugTag, "[GameServices] Failed to report progress.");
				
				if (_onCompletion != null)
					_onCompletion(false, "The requested operation could not be completed because Game Service failed to create Achievement object.");
				
				return;
			}
			
			// Set the new progress value
#pragma warning disable
			_newAchievement.PercentageCompleted	= ((double)_pointsScored/_description.MaximumPoints) * 100;
#pragma warning restore

			// Report it
			_newAchievement.ReportProgress(_onCompletion);
		}
		
		[System.Obsolete("This method is deprecated. Instead use ReportScoreWithID.")]
		public void ReportScore (string _leaderboardID, long _score, Score.ReportScoreCompletion _onCompletion)
		{
			ReportScoreWithID(_leaderboardID, _score, _onCompletion);
		}

		[System.Obsolete("This method is deprecated. Instead use ShowLeaderboardUIWithID.")]
		public void ShowLeaderboardUI (string _leaderboardID, eLeaderboardTimeScope _timeScope, GameServiceViewClosed _onCompletion)
		{
			ShowLeaderboardUIWithID(_leaderboardID, _timeScope, _onCompletion);
		}

		#endregion
	}
}
#endif
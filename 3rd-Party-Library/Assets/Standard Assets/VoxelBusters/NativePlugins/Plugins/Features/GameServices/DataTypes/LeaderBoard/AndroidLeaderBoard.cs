using UnityEngine;
using System.Collections;

#if USES_GAME_SERVICES && UNITY_ANDROID
using System;
using System.Collections.Generic;
using UnityEngine.SocialPlatforms;
using VoxelBusters.Utility;

namespace VoxelBusters.NativePlugins.Internal
{
	internal sealed class AndroidLeaderboard : Leaderboard 
	{
		#region Constants
		
		private const string 	kLeaderboardInfo			= "leaderboard-info";
		private const string 	kLeaderboardScores			= "leaderboard-scores";
		private const string 	kLeaderboardLocalScore		= "leaderboard-local-score";
				
		private const string	kIdentifier					= "identifier";
		private const string	kUserScope					= "user-scope";
		private const string	kTimeScope					= "time-scope";
		private const string	kTitle						= "title";
		private const string	kScores						= "scores";
		private const string	kLocalUserScore				= "local-user-score";		
	
		private const string	kUserScopeGlobal			= "user-scope-gobal";
		private const string	kUserScopeFriendsOnly		= "user-scope-friends";

		private const string	kTimeScopeToday				= "time-scope-today";
		private const string	kTimeScopeWeek				= "time-scope-week";
		private const string	kTimeScopeAllTime			= "time-scope-all-time";
	
		
		#region Mapping For Parsing
		
		internal static Dictionary<string, eLeaderboardUserScope> kUserScopeMap = new Dictionary<string, eLeaderboardUserScope>()
		{
			{ kUserScopeGlobal, eLeaderboardUserScope.GLOBAL},
			{ kUserScopeFriendsOnly, eLeaderboardUserScope.FRIENDS_ONLY}
		};
		
		internal static Dictionary<string, eLeaderboardTimeScope> kTimeScopeMap = new Dictionary<string, eLeaderboardTimeScope>()
		{
			{ kTimeScopeToday, eLeaderboardTimeScope.TODAY},
			{ kTimeScopeWeek, eLeaderboardTimeScope.WEEK},
			{ kTimeScopeAllTime, eLeaderboardTimeScope.ALL_TIME},
		};
		
		#endregion

		#endregion

		#region Properties

		public override string Identifier
		{
			get;
			protected set;
		}
		
		public override eLeaderboardUserScope UserScope
		{
			get;
			set;
		}
		
		public override eLeaderboardTimeScope TimeScope
		{
			get;
			set;
		}
		
		public override string Title
		{
			get;
			protected set;
		}
		
		public override Score[] Scores
		{
			get;
			protected set;
		}
		
		public override Score LocalUserScore
		{
			get;
			protected set;
		}

		#endregion

		#region Constructors

		private AndroidLeaderboard ()
		{}

		internal AndroidLeaderboard (string _globalIdentifier, string _identifier) : base (_globalIdentifier, _identifier)
		{
		}
		
		internal AndroidLeaderboard (IDictionary _leaderboardData)
		{
			Identifier			= _leaderboardData.GetIfAvailable<string>(kIdentifier);	

			string _userScope		= _leaderboardData.GetIfAvailable<string>(kUserScope);
			UserScope				= kUserScopeMap[_userScope];

			string _timeScope		= _leaderboardData.GetIfAvailable<string>(kTimeScope);
			TimeScope				= kTimeScopeMap[_timeScope];

			Title					= _leaderboardData.GetIfAvailable<string>(kTitle);
			
			IList _scoresList		= _leaderboardData.GetIfAvailable<List<object>>(kScores);			
			Scores					= AndroidScore.ConvertScoreList(_scoresList);

			IDictionary _localScore	= _leaderboardData.GetIfAvailable<Dictionary<string, object>>(kLocalUserScore);			
			LocalUserScore			= AndroidScore.ConvertScore(_localScore);


			// Set global identifier
			GlobalIdentifier		= GameServicesUtils.GetLeaderboardGID(Identifier);
		}
		
		#endregion

		#region Methods
		
		public override	void LoadTopScores (LoadScoreCompletion _onCompletion)
		{
			base.LoadTopScores(_onCompletion);
			GameServicesAndroid.Plugin.Call(GameServicesAndroid.Native.Methods.LOAD_TOP_SCORES, GetInstanceID(), Identifier, GetTimeScopeString(), GetUserScopeString(), MaxResults);						
		}
		
		public override	void LoadPlayerCenteredScores (LoadScoreCompletion _onCompletion)
		{
			base.LoadPlayerCenteredScores(_onCompletion);
			GameServicesAndroid.Plugin.Call(GameServicesAndroid.Native.Methods.LOAD_PLAYER_CENTERED_SCORES, GetInstanceID(),Identifier, GetTimeScopeString(), GetUserScopeString(), MaxResults);						
		}
		
		public override	void LoadMoreScores (eLeaderboardPageDirection _pageDirection, LoadScoreCompletion _onCompletion)
		{
			base.LoadMoreScores(_pageDirection, _onCompletion);
			GameServicesAndroid.Plugin.Call(GameServicesAndroid.Native.Methods.LOAD_MORE_SCORES, GetInstanceID(), Identifier, (int)_pageDirection, MaxResults);	
		}

		#endregion

		#region Event Callback Methods

		protected override void LoadScoresFinished (IDictionary _dataDict)
		{
			string		_error		= _dataDict.GetIfAvailable<string>(GameServicesAndroid.kNativeMessageError);
			IDictionary _infoDict	= _dataDict.GetIfAvailable<IDictionary>(kLeaderboardInfo);

			if (_infoDict != null)
			{
				// Update scores
				IDictionary	_localScoreInfoDict	= _infoDict.GetIfAvailable<IDictionary>(kLeaderboardLocalScore);
				IList		_scoresJSONList		= _infoDict.GetIfAvailable<IList>(kLeaderboardScores);
				
				LocalUserScore = AndroidScore.ConvertScore(_localScoreInfoDict);
				Scores = AndroidScore.ConvertScoreList(_scoresJSONList);
			}
			
			// Call finish handler
			LoadScoresFinished(Scores, LocalUserScore, _error);
		}

		#endregion

		#region Helpers
		
		private string GetTimeScopeString()
		{
			return  kTimeScopeMap.GetKey<eLeaderboardTimeScope>(TimeScope);
		}
		
		private string GetUserScopeString()
		{
			return  kUserScopeMap.GetKey<eLeaderboardUserScope>(UserScope);
		}
		
		#endregion
	}
}
#endif
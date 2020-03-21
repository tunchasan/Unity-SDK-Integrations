#if USES_GAME_SERVICES && UNITY_EDITOR
using UnityEngine;
using System.Collections;
using System;
using VoxelBusters.Utility;

namespace VoxelBusters.NativePlugins.Internal
{
	public partial class EditorGameCenter : SharedScriptableObject<EditorGameCenter>
	{
		#region Keys
		
		// General
		public		const		string		kErrorKey						= "error";
		public		const		string		kObjectInstanceIDKey			= "object-id";
		public		const		string		kImageKey						= "image";
		
		// Achievements
		public		const 		string		kAchievementInfoKey				= "achievement-info";
		public		const		string		kAchievementsListKey			= "achievements";

		// Descriptions
		public		const		string		kAchievementDescriptionsListKey	= "descriptions";


		// Leaderboard
		public 		const 		string 		kLeaderboardInfoKey				= "leaderboard-info";

		// Score
		public 		const 		string 		kScoreInfoKey					= "score-info";

		// Local user
		public 		const 		string 		kLocalUserInfoKey				= "local-user-info";
		public 		const 		string 		kFriendUsersKey					= "friend-users";
		
		// Users
		public 		const 		string 		kUsersListKey					= "users";

		#endregion

		#region Event Names

		// Achievements
		public		const 		string		kLoadAchievementsFinishedEvent	= "LoadAchievementsFinished";
		public		const 		string		kReportProgressFinishedEvent	= "ReportProgressFinished";
		
		// Description
		public		const 		string		kLoadAchievementDescriptionsFinishedEvent	= "LoadAchievementDescriptionsFinished";
		public		const 		string		kRequestForAchievementImageFinishedEvent	="RequestForAchievementImageFinished";
		
		// Leaderboard
		public		const 		string		kLoadScoresFinishedEvent		= "LoadScoresFinished";
		
		// Score
		public		const 		string		kReportScoreFinishedEvent		= "ReportScoreFinished";
		
		// Local user
		public 		const 		string 		kAuthenticationFinishedEvent	= "AuthenticationFinished";
		public 		const 		string 		kLoadFriendsFinishedEvent		= "LoadFriendsFinished";
		
		// User
		public		const 		string		kRequestForUserImageFinishedEvent	="RequestForUserImageFinished";
		public		const		string		kLoadUsersFinishedEvent			= "LoadUsersFinished";

		// UI
		public		const 		string		kShowLeaderboardViewFinishedEvent	= "ShowLeaderboardViewFinished";
		public		const 		string		kShowAchievementViewFinishedEvent	= "ShowAchievementViewFinished";

		#endregion
	}
}
#endif
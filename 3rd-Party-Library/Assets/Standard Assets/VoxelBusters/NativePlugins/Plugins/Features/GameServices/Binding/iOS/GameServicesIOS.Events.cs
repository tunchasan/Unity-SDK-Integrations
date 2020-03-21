using UnityEngine;
using System.Collections;

#if USES_GAME_SERVICES && UNITY_IOS
using System;
using System.Collections.Generic;
using VoxelBusters.Utility;

namespace VoxelBusters.NativePlugins
{
	using Internal;
	
	public sealed partial class GameServicesIOS : GameServices 
	{
		#region Contansts
	
		// General
		public		const		string		kNativeMessageErrorKey		= "error";
		public		const		string		kObjectInstanceIDKey		= "object-id";

		// Achievement
		private		const 		string		kAchievementsListKey		= "achievements";

		// Achievement description
		private		const 		string		kAchievementDescriptionsListKey		= "descriptions";

		// Users
		private		const 		string		kPlayersListKey				= "players";

		#endregion

		#region Leaderboard Callback Methods
		
		protected override void LoadScoresFinished (IDictionary _dataDict)
		{
			string			_instanceID				= _dataDict.GetIfAvailable<string>(kObjectInstanceIDKey);

			// Invoke handler
			LoadScoresFinished(_instanceID, _dataDict);
		}

		#endregion

		#region Achievement Description Callback Methods

		protected override void LoadAchievementDescriptionsFinished (IDictionary _dataDict)
		{
			IList			_descriptionsJSONList	= _dataDict.GetIfAvailable<IList>(kAchievementDescriptionsListKey);
			string 			_error					= _dataDict.GetIfAvailable<string>(GameServicesIOS.kNativeMessageErrorKey);
			AchievementDescription[] _newDescriptions	= iOSAchievementDescription.ConvertAchievementDescriptionsList(_descriptionsJSONList);

			// Invoke handler
			LoadAchievementDescriptionsFinished(_newDescriptions, _error);
		}
		
		protected override void RequestForAchievementImageFinished (IDictionary _dataDict)
		{
			string			_instanceID				= _dataDict.GetIfAvailable<string>(kObjectInstanceIDKey);

			// Invoke handler
			RequestForAchievementImageFinished(_instanceID, _dataDict);
		}

		#endregion

		#region Achievement Callback Methods

		protected override void LoadAchievementsFinished (IDictionary _dataDict)
		{
			IList			_achievementsJSONList	= _dataDict.GetIfAvailable<IList>(kAchievementsListKey);
			string 			_error					= _dataDict.GetIfAvailable<string>(GameServicesIOS.kNativeMessageErrorKey);
			Achievement[]	_newAchievements		= iOSAchievement.ConvertAchievementsList(_achievementsJSONList);
			
			// Invoke handler
			LoadAchievementsFinished(_newAchievements, _error);
		}

		protected override void ReportProgressFinished (IDictionary _dataDict)
		{
			string			_instanceID				= _dataDict.GetIfAvailable<string>(kObjectInstanceIDKey);

			// Invoke handler
			ReportProgressFinished(_instanceID, _dataDict);
		}

		#endregion

		#region User Callback Methods

		protected override void LoadUsersFinished (IDictionary _dataDict)
		{
			IList			_usersJSONList			= _dataDict.GetIfAvailable<IList>(kPlayersListKey);
			string 			_error					= _dataDict.GetIfAvailable<string>(kNativeMessageErrorKey);
			User[]			_newUsers				= iOSUser.ConvertUsersList(_usersJSONList);
			
			// Invoke handler
			LoadUsersFinished(_newUsers, _error);
		}

		protected override void RequestForUserImageFinished (IDictionary _dataDict)
		{
			string			_instanceID				= _dataDict.GetIfAvailable<string>(kObjectInstanceIDKey);

			// Invoke handler
			RequestForUserImageFinished(_instanceID, _dataDict);
		}

		#endregion

		#region Score Callback Methods
		
		protected override void ReportScoreFinished (IDictionary _dataDict)
		{
			string			_instanceID				= _dataDict.GetIfAvailable<string>(kObjectInstanceIDKey);

			// Invoke handler
			ReportScoreFinished(_instanceID, _dataDict);
		}

		#endregion
	}
}
#endif
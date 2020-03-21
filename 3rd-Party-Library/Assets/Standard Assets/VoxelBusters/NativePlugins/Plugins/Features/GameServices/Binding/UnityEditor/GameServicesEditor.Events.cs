using UnityEngine;
using System.Collections;

#if USES_GAME_SERVICES && UNITY_EDITOR
using System;
using VoxelBusters.Utility;

namespace VoxelBusters.NativePlugins
{
	using Internal;

	public sealed partial class GameServicesEditor : GameServices 
	{
		#region Achievement Description Callback Methods

		protected override void LoadAchievementDescriptionsFinished (IDictionary _dataDict)
		{
			EGCAchievementDescription[]		_gcDescriptions		= _dataDict.GetIfAvailable<EGCAchievementDescription[]>(EditorGameCenter.kAchievementDescriptionsListKey);
			string 							_error				= _dataDict.GetIfAvailable<string>(EditorGameCenter.kErrorKey);
			AchievementDescription[]		_newDescriptions	= EditorAchievementDescription.ConvertAchievementDescriptionsList(_gcDescriptions);

			// Invoke handler
			LoadAchievementDescriptionsFinished(_newDescriptions, _error);
		}

		#endregion

		#region Achievement Callback Methods
		
		protected override void LoadAchievementsFinished (IDictionary _dataDict)
		{
			EGCAchievement[]			_gcAchievements			= _dataDict.GetIfAvailable<EGCAchievement[]>(EditorGameCenter.kAchievementsListKey);
			string 						_error					= _dataDict.GetIfAvailable<string>(EditorGameCenter.kErrorKey);
			Achievement[]				_newAchievements		= EditorAchievement.ConvertAchievementsList(_gcAchievements);
			
			// Invoke handler
			LoadAchievementsFinished(_newAchievements, _error);
		}

		#endregion

		#region User Callback Methods
		
		protected override void LoadUsersFinished (IDictionary _dataDict)
		{
			EGCUser[]			_gcUsers			= _dataDict.GetIfAvailable<EGCUser[]>(EditorGameCenter.kUsersListKey);
			string 				_error				= _dataDict.GetIfAvailable<string>(EditorGameCenter.kErrorKey);
			User[]				_newUsers			= EditorUser.ConvertUsersList(_gcUsers);
			
			// Invoke handler
			LoadUsersFinished(_newUsers, _error);
		}

		#endregion

		#region Misc Callback Methods

		protected override void LoadExternalAuthenticationCredentialsFinished (IDictionary _dataDict)
		{
			LoadExternalAuthenticationCredentialsFinished(null, "Not supported on Editor");
		}

		#endregion
	}
}
#endif
using UnityEngine;
using System.Collections;

#if USES_GAME_SERVICES && UNITY_ANDROID
using System;
using System.Collections.Generic;
using VoxelBusters.Utility;

namespace VoxelBusters.NativePlugins
{
	using Internal;
	
	public sealed partial class GameServicesAndroid : GameServices 
	{
		#region Contansts
	
		// General
		public		const		string		kNativeMessageError			= "error";
		public		const		string		kObjectInstanceID			= "instance-id";
		public 		const		string		kImagePath					= "image-path";

		// Achievement
		private		const 		string		kAchievementsList			= "achievements-list";

		// Users
		private		const 		string		kUsersList					= "users-list";

		private		const 		string		kCredentialsData			= "credentials-data";
		

		#endregion

		#region Leaderboard Callback Methods
		
		protected override void LoadScoresFinished (IDictionary _dataDict)
		{
			string	_instanceID		= _dataDict.GetIfAvailable<string>(kObjectInstanceID);

			LoadScoresFinished(_instanceID, _dataDict);//TODO
		}

		#endregion

		#region Achievement Description Callback Methods

		protected override void LoadAchievementDescriptionsFinished (IDictionary _dataDict)
		{
			IList			_descriptionsJSONList	= _dataDict.GetIfAvailable<List<object>>(kAchievementsList);//On Android desc's are also in Achievement details.
			string 			_error					= _dataDict.GetIfAvailable<string>(kNativeMessageError);
			AchievementDescription[]	_newDescriptions	= null;

			Debug.Log("LoadAchievementDescriptionsFinished [IDictionary] " + _descriptionsJSONList.ToJSON());
			
			if (_descriptionsJSONList != null)
			{
				_newDescriptions = AndroidAchievementDescription.ConvertAchievementDescriptionList(_descriptionsJSONList);
			}
			
			LoadAchievementDescriptionsFinished(_newDescriptions, _error);
		}
		
		protected override void RequestForAchievementImageFinished (IDictionary _dataDict)
		{
			string	_instanceID				= _dataDict.GetIfAvailable<string>(kObjectInstanceID);

			RequestForAchievementImageFinished(_instanceID, _dataDict);
		}

		#endregion

		#region Achievement Callback Methods

		protected override void LoadAchievementsFinished (IDictionary _dataDict)
		{
			IList			_achievementsJSONList	= _dataDict.GetIfAvailable<IList>(kAchievementsList);
			string 			_error					= _dataDict.GetIfAvailable<string>(kNativeMessageError);
			Achievement[]	_newAchievements		= null;
			

			if (_achievementsJSONList != null)
			{
				_newAchievements	=	AndroidAchievement.ConvertAchievementList(_achievementsJSONList);
			}
			
			LoadAchievementsFinished(_newAchievements, _error);
		}

		protected override void ReportProgressFinished (IDictionary _dataDict)
		{
			string		_instanceID		= _dataDict.GetIfAvailable<string>(kObjectInstanceID);

			ReportProgressFinished(_instanceID, _dataDict);
		}

		#endregion

		#region User Callback Methods

		protected override void LoadUsersFinished (IDictionary _dataDict)
		{
			IList			_usersJSONList			= _dataDict.GetIfAvailable<IList>(kUsersList);
			string 			_error					= _dataDict.GetIfAvailable<string>(kNativeMessageError);
			User[]			_newUsers				= null;
			
			
			if (_usersJSONList != null)
			{
				_newUsers = AndroidUser.ConvertToUserList(_usersJSONList);
			}
			
			LoadUsersFinished(_newUsers, _error);
		}

		protected override void RequestForUserImageFinished (IDictionary _dataDict)
		{
			string	_instanceID				= _dataDict.GetIfAvailable<string>(kObjectInstanceID);

			RequestForUserImageFinished(_instanceID, _dataDict);
		}

		#endregion

		#region Score Callback Methods
		
		protected override void ReportScoreFinished (IDictionary _dataDict)
		{
			string	_instanceID				= _dataDict.GetIfAvailable<string>(kObjectInstanceID);
			
			ReportScoreFinished(_instanceID, _dataDict);
		}

		#endregion

		#region Misc Callback Methods

		// USER_DISCONNECTED
		private void UserDisconnected (string _reason)
		{
			Debug.Log("User disconnected! " + _reason);
		}

		protected override void LoadExternalAuthenticationCredentialsFinished (IDictionary _dataDict)
		{
			IDictionary	_credentials	= _dataDict.GetIfAvailable<IDictionary>(kCredentialsData);
			
			string	_error				= _dataDict.GetIfAvailable<string>(kNativeMessageError);
			
			LoadExternalAuthenticationCredentialsFinished(_credentials, _error);
		}
		#endregion
	}
}
#endif
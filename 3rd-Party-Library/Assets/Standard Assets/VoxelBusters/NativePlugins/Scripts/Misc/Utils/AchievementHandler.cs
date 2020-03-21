using UnityEngine;
using System.Collections;
using System.Runtime.CompilerServices;
using VoxelBusters.UASUtils;

[assembly: InternalsVisibleTo("Assembly-CSharp")]
namespace VoxelBusters.NativePlugins
{
	using Internal;

	public class AchievementHandler 
	{
		#region Static Fields

		internal 	static		AchievementDescription[]			achievementDescriptionList	= null;
		internal	static		int									achievementDescriptionCount	= 0;

		#endregion

		#region Methods

		internal static void SetAchievementDescriptionList (AchievementDescription[] _descriptionList)
		{
			if (_descriptionList == null)
			{
				achievementDescriptionList	= null;
				achievementDescriptionCount	= 0;
			}
			else
			{
				achievementDescriptionList	= _descriptionList;
				achievementDescriptionCount	= _descriptionList.Length;
			}
		}

		public static AchievementDescription GetAchievementDescriptionWithID (string _achievementID)
		{
			if (achievementDescriptionList == null)
			{
				DebugUtility.Logger.LogError(Constants.kDebugTag, "[GameServices] Please fetch achievement description list before accessing achievement properties.");
				return null;
			}

			// Iterate through each description and find description which has matching identifier
			for (int _iter = 0; _iter < achievementDescriptionCount; _iter++)
			{
				AchievementDescription 	_curDescription		= achievementDescriptionList[_iter];
				string 					_curDescriptionID	= _curDescription.Identifier;

				if (_curDescriptionID.Equals(_achievementID))
					return _curDescription;
			}

			DebugUtility.Logger.LogError(Constants.kDebugTag, string.Format("[GameServices] Couldnt find achievement description with identifier= {0}.", _achievementID));
			return null;
		}

		public static AchievementDescription GetAchievementDescriptionWithGlobalID (string _achievementGID)
		{
			if (achievementDescriptionList == null)
			{
				DebugUtility.Logger.LogError(Constants.kDebugTag, "[GameServices] Please fetch achievement description list before accessing achievement properties.");
				return null;
			}
			
			// Iterate through each description and find description which has matching identifier
			for (int _iter = 0; _iter < achievementDescriptionCount; _iter++)
			{
				AchievementDescription 	_curDescription		= achievementDescriptionList[_iter];
				string 					_curDescriptionGID	= _curDescription.GlobalIdentifier;
				
				if (_curDescriptionGID.Equals(_achievementGID))
					return _curDescription;
			}
			
			DebugUtility.Logger.LogError(Constants.kDebugTag, string.Format("[GameServices] Couldnt find achievement description with global identifier= {0}.", _achievementGID));
			return null;
		}

		#endregion
	}
}
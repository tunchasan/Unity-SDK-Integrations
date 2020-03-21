using UnityEngine;
using System.Collections;

namespace VoxelBusters.NativePlugins.Internal
{
	public class GameServicesUtils 
	{
		#region Static Fields

		public		static		LeaderboardMetadata[]		leaderboardMetadataCollection;
		public		static		AchievementMetadata[]		achievementMetadataCollection;

		#endregion

		#region Static Methods
		
		public static string GetLeaderboardID (string _globalID)
		{
			return GetPlatformID(leaderboardMetadataCollection, _globalID);
		}
		
		public static string GetLeaderboardGID (string _platformID)
		{
			return GetGlobalID(leaderboardMetadataCollection, _platformID);
		}
		
		public static string GetAchievementID (string _globalID)
		{
			return GetPlatformID(achievementMetadataCollection, _globalID);
		}
		
		public static string GetAchievementGID (string _platformID)
		{
			return GetGlobalID(achievementMetadataCollection, _platformID);
		}

		public static string GetPlatformID (IIdentifierContainer[] _collection, string _globalID)
		{
			IIdentifierContainer	_object	= _collection.FindObjectWithGlobalID(_globalID);
			if (_object == null)
				return null;
			
			return _object.GetCurrentPlatformID();
		}
		
		public static string GetGlobalID (IIdentifierContainer[] _collection, string _platformID)
		{
			IIdentifierContainer	_object	= _collection.FindObjectWithPlatformID(_platformID);
			if (_object == null)
				return _platformID;
			
			return _object.GlobalID;
		}

		#endregion
	}
}
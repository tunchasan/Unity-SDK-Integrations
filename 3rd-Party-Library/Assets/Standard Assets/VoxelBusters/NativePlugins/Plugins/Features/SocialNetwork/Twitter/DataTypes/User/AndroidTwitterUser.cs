using UnityEngine;
using System.Collections;
using VoxelBusters.Utility;

namespace VoxelBusters.NativePlugins.Internal
{
	public sealed class AndroidTwitterUser : TwitterUser
	{
		#region Constants
		
		private const string kIsVerified			= "is-verified";
		private const string kUserID				= "user-identifier";
		private const string kName					= "name";
		private const string kProfileImageURL		= "profile-image-url";
		private const string kIsProtected			= "is-protected";
		
		#endregion

		#region Constructor
		
		public AndroidTwitterUser (IDictionary _userJsonDict)
		{
			UserID					= _userJsonDict[kUserID] as string;
			Name					= _userJsonDict[kName] as string;
			IsVerified				= _userJsonDict.GetIfAvailable<bool>(kIsVerified);
			IsProtected				= _userJsonDict.GetIfAvailable<bool>(kIsProtected);
			ProfileImageURL			= _userJsonDict[kProfileImageURL] as string;
		}
		
		#endregion
	}
}

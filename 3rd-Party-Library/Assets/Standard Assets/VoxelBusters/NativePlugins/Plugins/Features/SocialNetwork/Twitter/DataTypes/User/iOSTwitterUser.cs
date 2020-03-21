using UnityEngine;
using System.Collections;
using VoxelBusters.Utility;

namespace VoxelBusters.NativePlugins.Internal
{
	public sealed class iOSTwitterUser  : TwitterUser
	{
//		{
//			"is-verified": false,
//			"user-ID": "612693",
//			"name": "Joey",
//			"profile-image-URL": "https://pbs.twimg.com/profile_images/378000658775583/3e842117ffc4628ff20b9a05_normal.jpeg",
//			"is-protected": false
//	}

		#region Constants
		
		private const string kIsVerified			= "is-verified";
		private const string kUserID				= "user-ID";
		private const string kName					= "name";
		private const string kProfileImageURL		= "profile-image-URL";
		private const string kIsProtected			= "is-protected";
		
		#endregion
		
		#region Constructor

		public iOSTwitterUser (IDictionary _userJsonDict)
		{
			UserID					= _userJsonDict.GetIfAvailable<string>(kUserID);
			Name					= _userJsonDict.GetIfAvailable<string>(kName);
			IsVerified				= _userJsonDict.GetIfAvailable<bool>(kIsVerified);
			IsProtected				= _userJsonDict.GetIfAvailable<bool>(kIsProtected);
			ProfileImageURL			= _userJsonDict.GetIfAvailable<string>(kProfileImageURL);
		}

		#endregion
	}
}

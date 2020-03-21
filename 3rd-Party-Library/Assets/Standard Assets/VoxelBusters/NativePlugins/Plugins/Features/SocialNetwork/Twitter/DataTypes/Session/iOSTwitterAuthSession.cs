using UnityEngine;
using System.Collections;
using VoxelBusters.Utility;

namespace VoxelBusters.NativePlugins.Internal
{
	public sealed class iOSTwitterAuthSession : TwitterAuthSession 
	{	
//		{
//			"user-ID": "612693",
//			"auth-token": "61263-zXHyy1qcZVhk4cA2PIT5XqYmPBXAu8MypxUpkRj",
//			"auth-token-secret": "km6BstbUQ7acK1ht5XIO66MNmG13UHthPNZNGYCEyr2"
//		}

		#region Constants
		
		public const string	kUserID				= "user-ID";
		public const string	kAuthToken			= "auth-token";
		public const string	kAuthTokenSecret	= "auth-token-secret";
		
		#endregion
		
		#region Constructor
		
		public iOSTwitterAuthSession (IDictionary _sessionJsonDict)
		{
			// Set properties
			AuthToken		= _sessionJsonDict.GetIfAvailable<string>(kAuthToken);
			AuthTokenSecret	= _sessionJsonDict.GetIfAvailable<string>(kAuthTokenSecret);
			UserID			= _sessionJsonDict.GetIfAvailable<string>(kUserID);
		}
		
		#endregion
	}
}
using UnityEngine;
using System.Collections;

namespace VoxelBusters.NativePlugins.Internal
{
	public sealed class AndroidTwitterAuthSession : TwitterAuthSession 
	{
		#region Constants
		
		public const string	kUserID				= "user-identifier";
		public const string	kAuthToken			= "auth-token";
		public const string	kAuthTokenSecret	= "auth-token-secret";
		
		#endregion
		
		#region Constructor
		
		public AndroidTwitterAuthSession (IDictionary _sessionJsonDict)
		{
			// Set properties
			AuthToken		= _sessionJsonDict[kAuthToken] as string;
			AuthTokenSecret	= _sessionJsonDict[kAuthTokenSecret] as string;
			UserID			= _sessionJsonDict[kUserID] as string;
		}
		
		#endregion
	}
}
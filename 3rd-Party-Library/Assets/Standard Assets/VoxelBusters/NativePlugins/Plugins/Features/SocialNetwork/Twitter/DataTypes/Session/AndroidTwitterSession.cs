using UnityEngine;
using System.Collections;

namespace VoxelBusters.NativePlugins.Internal
{
	public sealed class AndroidTwitterSession : TwitterSession 
	{
		#region Constants
		
		private const string	kUserName			= "user-name";

		#endregion

		#region Constructor

		public AndroidTwitterSession (IDictionary _sessionJsonDict)
		{
			// Set properties
			AuthToken		= _sessionJsonDict[AndroidTwitterAuthSession.kAuthToken] as string;
			AuthTokenSecret	= _sessionJsonDict[AndroidTwitterAuthSession.kAuthTokenSecret] as string;
			UserID			= _sessionJsonDict[AndroidTwitterAuthSession.kUserID] as string;
			UserName		= _sessionJsonDict[kUserName] as string;
		}
		
		#endregion
	}
}

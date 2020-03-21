using UnityEngine;
using System.Collections;

namespace VoxelBusters.NativePlugins
{
	/// <summary>
	/// Represents an immutable object, that encapsulates user's session authenticated with the Twitter API.
	/// </summary>
	public class TwitterSession : TwitterAuthSession
	{
		#region Properties

		/// <summary>
		/// The username associated with the access token. (read-only)
		/// </summary>
		public string UserName 
		{ 
			get; 
			protected set; 
		}
		
		#endregion
		
		#region Constructor
		
		protected TwitterSession ()
		{
			// Set properties
			UserName	= null;
		}
		
		#endregion

		#region Overriden Methods

		public override string ToString ()
		{
			return string.Format("[TwitterSession: AuthToken={0}, AuthTokenSecret={1}, UserName={2}, UserID={3}]", 
			                     AuthToken, AuthTokenSecret, UserName, UserID);
		}
		
		#endregion
	}
}
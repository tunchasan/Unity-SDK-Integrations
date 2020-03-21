using UnityEngine;
using System.Collections;

namespace VoxelBusters.NativePlugins
{
	/// <summary>
	/// Represents an immutable object, that encapsulates the authorization details of an Twitter session.
	/// </summary>
	public class TwitterAuthSession 
	{
		#region Properties
		
		/// <summary>
		/// The authorization token. (read-only)
		/// </summary>
		public string AuthToken 
		{ 
			get; 
			protected set; 
		}
		
		/// <summary>
		/// The authorization token secret. (read-only)
		/// </summary>
		public string AuthTokenSecret 
		{ 
			get; 
			protected set; 
		}
		
		/// <summary>
		/// The user ID associated with the access token. (read-only)
		/// </summary>
		public string UserID 
		{ 
			get; 
			protected set; 
		}

		#endregion

		#region Constructors

		protected TwitterAuthSession ()
		{
			// Set properties
			AuthToken		= null;
			AuthTokenSecret	= null;
			UserID			= null;
		}

		#endregion

		#region Overriden Methods
		
		public override string ToString ()
		{
			return string.Format("[TwitterSession: AuthToken={0}, AuthTokenSecret={1}, UserID={2}]", 
			                     AuthToken, AuthTokenSecret, UserID);
		}
		
		#endregion
	}
}
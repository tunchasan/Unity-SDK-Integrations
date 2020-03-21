using UnityEngine;
using System.Collections;

namespace VoxelBusters.NativePlugins
{
	/// <summary>
	/// Represents an immutable object, that holds information about user on Twitter.
	/// </summary>
	public class TwitterUser  
	{
		#region Properties

		/// <summary>
		/// The ID of the Twitter User. (read-only)
		/// </summary>
		public string UserID 
		{ 
			get; 
			protected set; 
		}

		/// <summary>
		/// The user’s name as it appears on their profile. (read-only)
		/// </summary>
		public string Name 
		{ 
			get; 
			protected set; 
		}

		/// <summary>
		/// A bool value that indicates whether the user has been verified by Twitter. (read-only)
		/// </summary>
		public bool IsVerified  
		{ 
			get; 
			protected set; 
		}

		/// <summary>
		/// A bool value that indicates whether the user is protected. (read-only)
		/// </summary>
		public bool IsProtected  
		{ 
			get; 
			protected set; 
		}

		/// <summary>
		/// The URL of the user’s profile image. (read-only)
		/// </summary>
		public string ProfileImageURL  
		{ 
			get; 
			protected set; 
		}

		#endregion

		#region Constructor

		protected TwitterUser ()
		{
			UserID					= string.Empty;
			Name					= string.Empty;
			IsVerified				= false;
			IsProtected				= false;
			ProfileImageURL			= string.Empty;
		}

		#endregion

		#region Overriden Methods

		public override string ToString ()
		{
			return string.Format("[TwitterUser: UserID={0}, Name={1}, IsVerified={2}, IsProtected={3}, ProfileImageURL={4}]", 
			                     UserID, Name, IsVerified, IsProtected, ProfileImageURL);
		}
		
		#endregion
	}
}
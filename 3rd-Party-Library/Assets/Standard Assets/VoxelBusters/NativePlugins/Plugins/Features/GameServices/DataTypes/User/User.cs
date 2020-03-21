using UnityEngine;
using System.Collections;
using VoxelBusters.Utility;
using DownloadTexture = VoxelBusters.Utility.DownloadTexture;

namespace VoxelBusters.NativePlugins
{
	using Internal;

	/// <summary>
	/// Represents an object that holds information about a user playing your game.
	/// </summary>
	public abstract class User : NPObject
	{
		#region Fields
		
		private		Texture2D		m_image;
		
		#endregion

		#region Properties

		/// <summary>
		/// A string assigned by game service to uniquely identify a user. (read-only)
		/// </summary>
		public abstract string Identifier
		{
			get;
			protected set;
		}

		/// <summary>
		/// A string to display for the user. (read-only)
		/// </summary>
		/// <description>
		/// If the user is a friend of the local user, then the value returned is the actual name of the user. 
		/// And incase if he is not a friend, then user’s alias will be returned.
		/// </description>
		public abstract string Name
		{
			get;
			protected set;
		}

		/// <summary>
		/// A string chosen by the user to identify themselves to other users. (read-only)
		/// </summary>
		/// <description>
		/// This property is used when a user is not a friend of the local user. For displaying name on user interface, use the <see cref="Name"/> property.
		/// </description>
		public abstract string Alias
		{
			get;
			protected set;
		}
		
		#endregion

		#region Delegates

		/// <summary>
		/// Delegate that will called when user info are retrieved from game server.
		/// </summary>
		/// <param name="_users">An array of <see cref="User"/> objects, that holds information of requested users.</param>
		/// <param name="_error">If the operation was successful, this value is nil; otherwise, this parameter holds the description of the problem that occurred.</param>
		public delegate void LoadUsersCompletion (User[] _users, string _error);

		#endregion
		
		#region Events
	
		protected event DownloadTexture.Completion DownloadImageFinishedEvent;
		
		#endregion
		
		#region Constructors
		
		protected User () : base (NPObjectManager.eCollectionType.GAME_SERVICES)
		{}
		
		#endregion

		#region Methods

		/// <summary>
		/// Asynchronously downloads the profile picture of this user.
		/// </summary>
		/// <param name="_onCompletion">Callback that will be called after operation is completed.</param>
		public virtual void GetImageAsync (DownloadTexture.Completion _onCompletion)
		{
			// Cache callback
			DownloadImageFinishedEvent	= _onCompletion;
			
			// Using cached information
			if (m_image != null)
			{
				DownloadImageFinishedEvent(m_image, null);
				return;
			}
			
			// Request for image
			RequestForImage();
		}
		
		protected virtual void RequestForImage ()
		{}

		public override string ToString ()
		{
			return string.Format("[User: Name={0}]", Name);
		}

		#endregion

		#region Event Callback Methods

		protected virtual void RequestForImageFinished (IDictionary _dataDict)
		{}

		protected void RequestForImageFinished (URL _imagePathURL, string _error)
		{
			if (_error != null)
			{
				DownloadImageFinished(null, _error);
				return;
			}
			else
			{
				DownloadTexture _newRequest		= new DownloadTexture(_imagePathURL, true, true);
				_newRequest.OnCompletion		= (Texture2D _image, string _downloadError)=>{
					
					// Invoke handler
					DownloadImageFinished(_image, _downloadError);
				};
				_newRequest.StartRequest();
			}
		}
		
		protected void DownloadImageFinished (Texture2D _image, string _error)
		{
			// Set properties
			m_image	= _image;
			
			// Send callback
			if (DownloadImageFinishedEvent != null)
				DownloadImageFinishedEvent(_image, _error);
		}
		
		#endregion
	}
}
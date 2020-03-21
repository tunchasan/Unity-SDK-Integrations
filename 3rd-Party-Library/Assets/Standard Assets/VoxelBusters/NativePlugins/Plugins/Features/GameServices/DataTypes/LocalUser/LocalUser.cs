using UnityEngine;
using System.Collections;

#if USES_GAME_SERVICES 
using System;
using VoxelBusters.Utility;

namespace VoxelBusters.NativePlugins
{
	using Internal;

	/// <summary>
	/// Represents an object that holds information about the authenticated user running your game on the device. 
	/// </summary>
	/// <description>
	/// At any given time, only one user may be authenticated on the device, this user must log out before another user can log in.
	/// </description>
	/// <remarks>
	/// \note Your game must authenticate the local user before using any features.
	/// </remarks>
	public abstract class LocalUser	: User
	{
		#region Constants

		private 	const	string 		kInitErrorMessage		= "The requested operation could not be completed because GameServices component initialisation failed.";

		#endregion

		#region Properties

		/// <summary>
		/// A bool value that indicates whether a local user is currently signed in to game service. (read-only)
		/// </summary>
		public abstract bool IsAuthenticated
		{
			get;
			protected set;
		}

		/// <summary>
		/// An array of <see cref="User"/> objects, that are friends of the local user. (read-only)
		/// </summary>
		public abstract	User[] Friends 
		{
			get;
			protected set;
		}

		#endregion

		#region Delegates

		/// <summary>
		/// Delegate that will be called when authentication process is complete.
		/// </summary>
		/// <param name="_success">A bool value used to indicate operation status.</param>
		/// <param name="_error">If the operation was successful, this value is nil; otherwise, this parameter holds the description of the problem that occurred.</param>
		public delegate void AuthenticationCompletion (bool _success, string _error);

		/// <summary>
		/// Delegate that will be called when <see cref="LocalUser"/> friends information is retrieved.
		/// </summary>
		/// <param name="_users">An array of <see cref="User"/> objects, that are friends of the local user.</param>
		/// <param name="_error">If the operation was successful, this value is nil; otherwise, this parameter holds the description of the problem that occurred.</param>
		public delegate void LoadFriendsCompletion (User[] _users, string _error);

		/// <summary>
		/// Delegate that will be called when <see cref="LocalUser"/> is signed out of the game service.
		/// </summary>
		/// <param name="_success">A bool value used to indicate operation status.</param>
		/// <param name="_error">If the operation was successful, this value is nil; otherwise, this parameter holds the description of the problem that occurred.</param>
		public delegate void SignOutCompletion (bool _success, string _error);

		#endregion

		#region Events
		
		protected AuthenticationCompletion AuthenticationFinishedEvent;
		protected LoadFriendsCompletion	LoadFriendsFinishedEvent;
		protected SignOutCompletion	SignOutFinishedEvent;

		#endregion
		
		#region Constructor
		
		protected LocalUser () : base ()
		{}
		
		#endregion

		#region Methods

		/// <summary>
		/// Authenticates the local user on the device.
		/// </summary>
		/// <param name="_onCompletion">Callback that will be called after operation is completed.</param>
		public virtual void Authenticate (AuthenticationCompletion _onCompletion)
		{
			// Cache callback
			AuthenticationFinishedEvent	= _onCompletion;
		}

		protected virtual bool NeedsInit ()
		{
			return false;
		}

		protected void Init ()
		{
			NPBinding.GameServices.InvokeMethod("LoadAchievementDescriptions", new object[] { 
				false, 
				(AchievementDescription.LoadAchievementDescriptionsCompletion)((AchievementDescription[] _descriptionList, string _error)=>{
					
					if (_error == null)
						OnInitSuccess(null);
					else
						OnInitFail(kInitErrorMessage);
				}) 
			}, new Type[] { 
				typeof(bool), 
				typeof(AchievementDescription.LoadAchievementDescriptionsCompletion) 
			});
		}

		/// <summary>
		/// Retrieves friends info of the local user.
		/// </summary>
		/// <param name="_onCompletion">Callback that will be called after operation is completed.</param>
		public virtual void LoadFriends (LoadFriendsCompletion _onCompletion)
		{
			// Cache callback
			LoadFriendsFinishedEvent	= _onCompletion;

			if (!IsAuthenticated)
			{
				LoadFriendsFinished(null, Constants.kGameServicesUserAuthMissingError);
				return;
			}
		}

		/// <summary>
		/// Signs out the local user from the game service.
		/// </summary>
		/// <remarks>
		/// \note This works only on Android platform. In iOS, its a no-op call.
		/// </remarks>
		public virtual void SignOut (SignOutCompletion _onCompletion)
		{
			// Cache callback
			SignOutFinishedEvent	= _onCompletion;
		}

		public override string ToString ()
		{
			return string.Format("[LocalUser: Name={0}, IsAuthenticated={1}]", Name, IsAuthenticated);
		}

		#endregion

		#region Event Callback Methods

		protected virtual void AuthenticationFinished (IDictionary _dataDict)
		{}

		protected void AuthenticationFinished (bool _success, string _error)
		{
			// Reset properties
			Friends	= null;

			// Invoke action based on auth response
			if (_success)
			{
				if (NeedsInit())
					Init();
				else
					OnInitSuccess(_error);
			}
			else
			{
				OnInitFail(_error);
			}
        }
		
		protected virtual void OnInitSuccess (string _error)
		{
			// Send auth finished event
			if (AuthenticationFinishedEvent != null)
				AuthenticationFinishedEvent(true, _error);

            // unset here
            AuthenticationFinishedEvent = null;
        }
		
		protected virtual void OnInitFail (string _error)
		{
			// Send auth finished event
			if (AuthenticationFinishedEvent != null)
				AuthenticationFinishedEvent(false, _error);

            // unset here
            AuthenticationFinishedEvent = null;
        }

		protected virtual void LoadFriendsFinished (IDictionary _dataDict)
		{}

		protected void LoadFriendsFinished (User[] _users, string _error)
		{
			if (LoadFriendsFinishedEvent != null)
				LoadFriendsFinishedEvent(_users, _error);
		}

		protected virtual void SignOutFinished (IDictionary _dataDict)
		{}

		protected void SignOutFinished (bool _success, string _error)
		{
			if (SignOutFinishedEvent != null)
				SignOutFinishedEvent(_success, _error);
		}

		#endregion
	}
}
#endif
using UnityEngine;
using System.Collections;

#if USES_GAME_SERVICES && UNITY_EDITOR
using System;
using VoxelBusters.Utility;

namespace VoxelBusters.NativePlugins.Internal
{
	[Serializable]
	public sealed class EditorLocalUser : LocalUser
	{
		#region Fields
		
		private		EditorUser		m_userInfo;
		
		#endregion
		
		#region Properties
		
		public override string Identifier
		{
			get
			{
				if (m_userInfo == null)
					return null;
				
				return m_userInfo.Identifier;
			}
			
			protected set
			{
				throw new Exception("[GameServices] Only getter is supported.");
			}
		}
		
		public override string Name
		{
			get
			{
				if (m_userInfo == null)
					return null;
				
				return m_userInfo.Name;
			}
			
			protected set
			{
				throw new Exception("[GameServices] Only getter is supported.");
			}
		}
		
		public override string Alias
		{
			get
			{
				if (m_userInfo == null)
					return null;
				
				return m_userInfo.Alias;
			}
			
			protected set
			{
				throw new Exception("[GameServices] Only getter is supported.");
			}
		}
		
		public override bool IsAuthenticated
		{
			get
			{
				return EditorGameCenter.Instance.IsAuthenticated();
			}
			
			protected set
			{
				throw new Exception("[GameServices] Only getter is supported.");
			}
		}
		
		public override User[] Friends 
		{
			get;
			protected set;
		}
		
		#endregion

		#region Methods

		public EditorLocalUser ()
		{}

		#endregion

		#region Methods
		
		protected override void RequestForImage ()
		{
			if (m_userInfo == null)
			{
				DownloadImageFinished(null, Constants.kGameServicesUserAuthMissingError);
				return;
			}

			EditorGameCenter.Instance.GetUserImage(m_userInfo);
		}

		public override void Authenticate (AuthenticationCompletion _onCompletion)
		{
			base.Authenticate(_onCompletion);

			EditorGameCenter.Instance.Authenticate();
		}

		public override void LoadFriends (LoadFriendsCompletion _onCompletion)
		{
			base.LoadFriends(_onCompletion);
			
			// Verify user
			if (!IsAuthenticated)
				return;
			
			EditorGameCenter.Instance.LoadFriends();
		}

		public override void SignOut (SignOutCompletion _onCompletion)
		{
			base.SignOut(_onCompletion);

			// Invoke method
			EditorGameCenter.Instance.SignOut();
			SignOutFinished(true, null);
		}
		
		#endregion
		
		#region Event Callback Methods
		
		protected override void RequestForImageFinished (IDictionary _dataDict)
		{
			string		_error		= _dataDict.GetIfAvailable<string>(EditorGameCenter.kErrorKey);
			Texture2D	_image		= _dataDict.GetIfAvailable<Texture2D>(EditorGameCenter.kImageKey);
			
			DownloadImageFinished(_image, _error);
		}

		protected override void AuthenticationFinished (IDictionary _dataDict)
		{
			// Update properties using received information
			bool	_isAuthenticated	= IsAuthenticated;
			
			if (_isAuthenticated)
			{
				EGCLocalUser _localUserData	= _dataDict.GetIfAvailable<EGCLocalUser>(EditorGameCenter.kLocalUserInfoKey);
			
				m_userInfo			= new EditorUser(_localUserData.Info);
				Friends				= null;
			}
			else
			{
				m_userInfo			= null;
				Friends				= null;
			}

			// Invoke auth finished handler
			string		_error		= _dataDict.GetIfAvailable<string>(EditorGameCenter.kErrorKey);

			AuthenticationFinished(_isAuthenticated, _error);
		}
		
		protected override void LoadFriendsFinished (IDictionary _dataDict)
		{
			string		_error			= _dataDict.GetIfAvailable<string>(EditorGameCenter.kErrorKey);
			EGCUser[]	_gcFriendsList	= _dataDict.GetIfAvailable<EGCUser[]>(EditorGameCenter.kFriendUsersKey);
			
			if (_gcFriendsList != null)
			{
				int 			_count		= _gcFriendsList.Length;
				EditorUser[]	_friends	= new EditorUser[_count];
				
				for (int _iter = 0; _iter < _count; _iter++)
					_friends[_iter]		= new EditorUser(_gcFriendsList[_iter]);
				
				// Update property
				Friends					= _friends;
			}
			
			LoadFriendsFinished(Friends, _error);
		}

		#endregion
	}
}
#endif
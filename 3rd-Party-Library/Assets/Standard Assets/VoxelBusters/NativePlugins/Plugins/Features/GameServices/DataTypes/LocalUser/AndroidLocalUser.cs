using UnityEngine;
using System.Collections;


#if USES_GAME_SERVICES && UNITY_ANDROID
using System;
using System.Collections.Generic;
using VoxelBusters.Utility;
using VoxelBusters.NativePlugins.Internal;
using DownloadTexture = VoxelBusters.Utility.DownloadTexture;

namespace VoxelBusters.NativePlugins.Internal
{
	public sealed class AndroidLocalUser : LocalUser 
	{
		#region Constants

		private 	const 	string 		kLocalUserFriendsKey	= "local-user-friends";
		private 	const 	string 		kLocalUserInfoKey		= "local-user-info";

		#endregion

		#region Fields
		
		private 	AndroidUser 	m_user;
		private		IDictionary		m_authResponseData;

		#endregion

		#region Properties
		
		public override string Identifier
		{
			get
			{
				if (m_user == null)
					return null;
				
				return m_user.Identifier;
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
				if (m_user == null)
					return null;
				
				return m_user.Name;
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
				if (m_user == null)
					return null;
				
				return m_user.Alias;
			}
			
			protected set
			{
				throw new Exception("[GameServices] Only getter is supported.");
			}
		}

		public override	bool IsAuthenticated
		{
			get
			{
				AndroidJavaObject _plugin = GameServicesAndroid.Plugin;
				bool _isAuthFinished;

				if (_plugin	== null)
				{
					_isAuthFinished = false;
				}
				else
				{
					_isAuthFinished = GameServicesAndroid.Plugin.Call<bool>(GameServicesAndroid.Native.Methods.IS_LOCAL_USER_AUTHENTICATED);
				}
				
				return _isAuthFinished;
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
		
		public override void Authenticate (AuthenticationCompletion _onCompletion)
		{
			base.Authenticate(_onCompletion);
			
			// Request authentication
			GameServicesAndroid.Plugin.Call(GameServicesAndroid.Native.Methods.AUTHENTICATE_LOCAL_USER);
		}
		
		protected override bool NeedsInit ()
		{
			return true;
		}

		public override void SignOut (SignOutCompletion _onCompletion)
		{
			base.SignOut(_onCompletion);
			
			// Request signout
			GameServicesAndroid.Plugin.Call(GameServicesAndroid.Native.Methods.SIGN_OUT_LOCAL_USER);
		}
		
		public override void LoadFriends (LoadFriendsCompletion _onCompletion)
		{
			base.LoadFriends(_onCompletion);

			// Verify user
			if (!IsAuthenticated)
				return;

			GameServicesAndroid.Plugin.Call(GameServicesAndroid.Native.Methods.LOAD_LOCAL_USER_FRIENDS, false);
		}
		
		public override void GetImageAsync (DownloadTexture.Completion _onCompletion)
		{
			if (m_user == null)
			{
				if (_onCompletion != null)
					_onCompletion(null, Constants.kGameServicesUserAuthMissingError);
				
				return;
			}
			
			m_user.GetImageAsync(_onCompletion);
		}
		
		#endregion

		#region Event Callback Methods
		
		protected override void AuthenticationFinished (IDictionary _dataDict)
		{
			// Reset fields to default value
			m_user				= null;

			// Parse auth response data
			string	_error		= _dataDict.GetIfAvailable<string>(GameServicesAndroid.kNativeMessageError);
			bool	_success	= (_error == null);

			if (_success)
			{
				// Cache auth response data
				m_authResponseData	= _dataDict;
			}

			// Invoke auth finish handler
			AuthenticationFinished(_success, _error);
		}
		
		protected override void OnInitSuccess (string _error)
		{
			IDictionary	_infoDict	= m_authResponseData.GetIfAvailable<IDictionary>(kLocalUserInfoKey);
			
			// Update user info
			m_user					= new AndroidUser(_infoDict);
			
			// Release cached data
			m_authResponseData		= null;

			base.OnInitSuccess(_error);
		}

		protected override void OnInitFail (string _error)
		{
			// Reset attributes
			m_user					= null;
			m_authResponseData		= null;

			base.OnInitFail(_error);
		}

		protected override void SignOutFinished (IDictionary _dataDict)
		{
			string		_error		= 	_dataDict.GetIfAvailable<string>(GameServicesAndroid.kNativeMessageError);

			SignOutFinished(_error == null, _error);			
		}
		
		protected override void LoadFriendsFinished (IDictionary _dataDict)
		{
			string		_error			= _dataDict.GetIfAvailable<string>(GameServicesAndroid.kNativeMessageError);
			IList		_friendJSONList	= _dataDict.GetIfAvailable<List<object>>(kLocalUserFriendsKey);
			
			if (_friendJSONList != null)
			{
				// Update property
				Friends					= AndroidUser.ConvertToUserList(_friendJSONList);
			}
			
			LoadFriendsFinished(Friends, _error);
		}
		
		#endregion
	}
}
#endif
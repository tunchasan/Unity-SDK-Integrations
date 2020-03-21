using UnityEngine;
using System.Collections;

#if USES_GAME_SERVICES && UNITY_ANDROID
using System;
using UnityEngine.SocialPlatforms;
using VoxelBusters.Utility;
using DownloadTexture = VoxelBusters.Utility.DownloadTexture;

namespace VoxelBusters.NativePlugins.Internal
{
	public sealed class AndroidUser : User 
	{
		#region Constants
		
		internal const string	kIdentifier			= "identifier";
		internal const string	kName				= "name";
		internal const string	kHighResImageURL	= "high-res-image-url";
		internal const string	kIconImageURL		= "icon-image-url";
		internal const string	kTimeStamp			= "timestamp";
		internal const string 	kImageFilePath		= "image-file-path";

		#endregion

		#region Fields
	
		private string 		m_imagePath;

		#endregion

		#region Properties

		public override string Identifier
		{
			get;
			protected set;
		}
		
		public override string Name
		{
			get;
			protected set;
		}
		
		public override string Alias 
		{
			get;
			protected set;
		}

		#endregion
		
		#region Constructors
		
		internal AndroidUser (IDictionary _userProfileData)
		{
			if(_userProfileData != null)
			{
				Identifier		= _userProfileData.GetIfAvailable<string>(kIdentifier);
				Name			= _userProfileData.GetIfAvailable<string>(kName);
				m_imagePath		= _userProfileData.GetIfAvailable<string>(kHighResImageURL);
				Alias			= Name;
			}
		}
		
		#endregion
		
		#region Static Methods
		
		internal static AndroidUser ConvertToUser (IDictionary _user)
		{
			if (_user == null)
				return null;
			
			return new AndroidUser(_user);
		}
		
		internal static User[] ConvertToUserList (IList _userList)
		{
			if (_userList == null)
				return null;
			
			int					_count				= _userList.Count;
			User[]				_usersList			= new AndroidUser[_count];
			
			for (int _iter = 0; _iter < _count; _iter++)
				_usersList[_iter]			= new AndroidUser(_userList[_iter] as IDictionary);
			
			return _usersList;
		}
		
		#endregion

		#region Methods
		
		protected override void RequestForImage ()
		{
			string _error 	= null;

			if (string.IsNullOrEmpty(m_imagePath))
			{
				_error = "Image not available!";
				RequestForImageFinished(URL.URLWithString(null), _error);
				return;
			}
			
			string _newRequestID = GetInstanceID();	
			
			if(m_imagePath.ToLower().StartsWith("http"))
			{
				RequestForImageFinished(URL.URLWithString(m_imagePath), null);
			}
			else
			{
				GameServicesAndroid.Plugin.Call(GameServicesAndroid.Native.Methods.LOAD_PROFILE_PICTURE, _newRequestID, m_imagePath);
			}
		}
		
		#endregion
		
		#region Event Callback Methods

		protected override void RequestForImageFinished (IDictionary _dataDict)
		{
			string			_error				= _dataDict.GetIfAvailable<string>(GameServicesAndroid.kNativeMessageError);
			
			if (_error == null)
			{
				string _imagePath	=	_dataDict.GetIfAvailable<string>(kImageFilePath);
				RequestForImageFinished(URL.FileURLWithPath(_imagePath), null);
			}
			else
				RequestForImageFinished(URL.FileURLWithPath(null), _error);
		}
		
		#endregion

	}
}
#endif
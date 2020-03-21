using UnityEngine;
using System.Collections;

#if USES_GAME_SERVICES && UNITY_IOS
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using VoxelBusters.Utility;
using DownloadTexture = VoxelBusters.Utility.DownloadTexture;

namespace VoxelBusters.NativePlugins.Internal
{
	public sealed class iOSUser : User 
	{
		private enum GKPhotoSize
		{
			GKPhotoSizeSmall	= 0,
			GKPhotoSizeNormal
		}

		#region Constants

		private 	const 	string 		kPlayerIDKey	= "player-id";
		private 	const 	string 		kAliasKey		= "alias";
		private 	const 	string 		kDisplayNameKey	= "display-name";
		private		const 	string		kImagePathKey	= "image-path";

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

		public iOSUser (IDictionary _dataDict) : base ()
		{
			// Initialize properties
			Identifier		= _dataDict.GetIfAvailable<string>(kPlayerIDKey);
			Name			= _dataDict.GetIfAvailable<string>(kDisplayNameKey);
			Alias			= _dataDict.GetIfAvailable<string>(kAliasKey);
		}

		#endregion

		#region External Methods
		
		[DllImport("__Internal")]
		private static extern void loadPhoto (string _playerInfoJSON, int _photoSize);
		
		#endregion

		#region Static Methods
		
		public static iOSUser[] ConvertUsersList (IList _usersJSONList)
		{
			if (_usersJSONList == null)
				return null;
			
			int 			_count			= _usersJSONList.Count;
			iOSUser[]		_usersList		= new iOSUser[_count];
			
			for (int _iter = 0; _iter < _count; _iter++)
				_usersList[_iter]			= new iOSUser((IDictionary)_usersJSONList[_iter]);
			
			return _usersList;
		}
		
		#endregion

		#region Methods

		protected override void RequestForImage ()
		{
			// Native method call
			loadPhoto(GetUserInfoJSONObject().ToJSON(), (int)GKPhotoSize.GKPhotoSizeSmall);
		}
		
		public IDictionary GetUserInfoJSONObject ()
		{
			IDictionary		_JSONDict	= new Dictionary<string, object>();
			_JSONDict[kPlayerIDKey]		= Identifier;
			_JSONDict[GameServicesIOS.kObjectInstanceIDKey]	= GetInstanceID();

			return _JSONDict;
		}

		#endregion

		#region Event Callback Methods

		protected override void RequestForImageFinished (IDictionary _dataDict)
		{
			string			_error		= _dataDict.GetIfAvailable<string>(GameServicesIOS.kNativeMessageErrorKey);
			
			if (_error == null)
			{
				string _imagePath	= _dataDict.GetIfAvailable<string>(kImagePathKey);
				RequestForImageFinished(URL.FileURLWithPath(_imagePath), null);
			}
			else
				RequestForImageFinished(URL.FileURLWithPath(null), _error);
		}

		#endregion
	}
}
#endif
#if USES_NOTIFICATION_SERVICE && UNITY_ANDROID
using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using VoxelBusters.Utility;
using VoxelBusters.UASUtils;

namespace VoxelBusters.NativePlugins.Internal
{
	public sealed class AndroidNotificationPayload : CrossPlatformNotification 
	{
		#region Key Constants //The same keys are used  by Native code as well
		
		// Json object keys
		public  const string		kFireDate						= "fire-date";
		public  const string		kAlertBody						= "alert-body";
		public  const string		kCustomSound					= "custom-sound";
		public  const string		kLargeIcon						= "large-icon";
		public 	const string 		kRepeatIntervalKey				= "repeat-interval";
		public 	const string 		kIsRemoteNotification			= "is-remote-notification";
				
		public const string 		kUserInfo 						= "user-info";
		public const string 		kTickerText 					= "ticker-text";
		public const string 		kContentTitle 					= "content-title";
		public const string 		kContentText 					= "content-text";
		public const string 		kTag			 				= "notification-tag";
		public const string 		kCustomSoundKey					= "custom-sound";
	
		public const string			kBadgeCount						= "badge";
		
		
		public static string 		ContentTitleKey 
		{ 
			get
			{
				return NPSettings.Notification.Android.ContentTitleKey;
			}
		}

		public static string 		ContentTextKey
		{ 
			get
			{
				return NPSettings.Notification.Android.ContentTextKey;
			}
		}

		public static string 		TickerTextKey 
		{ 
			get
			{
				return NPSettings.Notification.Android.TickerTextKey;
			}
		}

		public static string 		TagKey
		{ 
			get
			{
				return NPSettings.Notification.Android.TagKey;
			}
		}

		public static string 		UserInfoKey 
		{ 
			get
			{
				return NPSettings.Notification.Android.UserInfoKey;
			}
		}
		
		#endregion

		#region Constructor
		
		public AndroidNotificationPayload (IDictionary _payloadDict)
		{
			AndroidProperties		= new AndroidSpecificProperties();
			
			// Alert
			if (_payloadDict.Contains(NPSettings.Notification.Android.ContentTextKey))
			{
				//Check here which key is being received.
				DebugUtility.Logger.Log(Constants.kDebugTag, "[AndroidNotificationPayload] " + _payloadDict.ToJSON());//TODO
				object _alertUnknownType	= _payloadDict[ContentTextKey] as object;
				
				// String type
				if ((_alertUnknownType as string) != null)
				{
					AlertBody	= _alertUnknownType as string;
				}
			}
						
			if (_payloadDict.Contains(UserInfoKey))
				UserInfo		= _payloadDict[UserInfoKey] as IDictionary;
			
			// Fire date
			long _secsFromNow	= _payloadDict.GetIfAvailable<long>(kFireDate);
				
			FireDate			= _secsFromNow.ToDateTimeFromJavaTime();

			if (_payloadDict.Contains(kRepeatIntervalKey))
				RepeatInterval	= (eNotificationRepeatInterval)_payloadDict.GetIfAvailable<int>(kRepeatIntervalKey);

			SoundName			= 	_payloadDict.GetIfAvailable<string>(kCustomSound);
			

			AndroidProperties.ContentTitle		=  	_payloadDict.GetIfAvailable<string>(ContentTitleKey);
			AndroidProperties.TickerText		=  	_payloadDict.GetIfAvailable<string>(TickerTextKey);
			AndroidProperties.Tag				=  	_payloadDict.GetIfAvailable<string>(TagKey);
			AndroidProperties.LargeIcon			= 	_payloadDict.GetIfAvailable<string>(kLargeIcon);
			AndroidProperties.BadgeCount		=	_payloadDict.GetIfAvailable<int>(kBadgeCount);
		}
		
		#endregion
		
		#region Static Methods
		
		public static IDictionary CreateNotificationPayload (CrossPlatformNotification _notification)
		{
			IDictionary _payloadDict						= new Dictionary<string, object>();
			AndroidSpecificProperties _androidProperties	= _notification.AndroidProperties;
			
			// Alert
			_payloadDict[ContentTextKey]		= _notification.AlertBody;
			
			// User info, fire date
			_payloadDict[UserInfoKey]			= _notification.UserInfo;
			_payloadDict[kFireDate]				= _notification.FireDate.ToJavaTimeFromDateTime();


			_payloadDict[kRepeatIntervalKey]	= (int)_notification.RepeatInterval;
			_payloadDict[kCustomSound]			= _notification.SoundName;
			
			// ContentTitle, TickerText, Tag
			if(_androidProperties != null)
			{
				_payloadDict[ContentTitleKey]	= _androidProperties.ContentTitle;
				_payloadDict[TickerTextKey]		= _androidProperties.TickerText;
				_payloadDict[TagKey]			= _androidProperties.Tag;
				_payloadDict[kLargeIcon]		= _androidProperties.LargeIcon;
				_payloadDict[kBadgeCount]		= _androidProperties.BadgeCount;
			}
			
			return _payloadDict;
		}


		public static Dictionary<string, string> GetNotificationKeyMap()
		{
			Dictionary<string, string> _data =  new Dictionary<string, string>();
			
			_data.Add(kUserInfo, 		UserInfoKey);
			_data.Add(kTickerText, 		TickerTextKey);
			_data.Add(kContentTitle, 	ContentTitleKey);
			_data.Add(kContentText, 	ContentTextKey);
			_data.Add(kTag, 			TagKey);

			return _data;
		}
		
		#endregion
	}
}
#endif
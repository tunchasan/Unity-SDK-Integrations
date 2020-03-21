using UnityEngine;
using System.Collections;

#if USES_NOTIFICATION_SERVICE && UNITY_IOS
using System.Collections.Generic;
using VoxelBusters.Utility;

#if  (UNITY_5 || UNITY_2017_1_OR_NEWER)
using CalendarUnit = UnityEngine.iOS.CalendarUnit;
#endif

namespace VoxelBusters.NativePlugins.Internal
{
	public sealed class iOSNotificationPayload : CrossPlatformNotification 
	{
		#region Constant

		private 	const 	string 		kAPS				= "aps";
		private 	const 	string 		kAlert				= "alert";
		private 	const 	string 		kBody				= "body";
		private 	const 	string 		kAction				= "action-loc-key";
		private 	const 	string 		kHasAction			= "has-action";
		private 	const 	string 		kLaunchImage		= "launch-image";
		private 	const 	string 		kFireDate			= "fire-date";
		private 	const 	string 		kRepeatIntervalKey	= "repeat-interval";
		private 	const 	string 		kBadge				= "badge";
		private 	const 	string 		kSound				= "sound";

		#endregion

		#region Constructor

		public iOSNotificationPayload (IDictionary _payloadDict)
		{
			iOSProperties					= new iOSSpecificProperties();
			string 		_userInfoKey		= NPSettings.Notification.iOS.UserInfoKey;
			IDictionary _apsDict			= _payloadDict[kAPS] as IDictionary;

			// Read alert info from aps dictionary
			if (_apsDict.Contains(kAlert))
			{
				object 	_alertUnknownType	= _apsDict[kAlert] as object;
			
				if (_alertUnknownType != null)
				{
					// String format
					if ((_alertUnknownType as string) != null)
					{
						AlertBody						= _alertUnknownType as string;
					}
					// Dictionary format
					else
					{
						IDictionary _alertDict			= _alertUnknownType as IDictionary;
						string		_alertAction		= _alertDict.GetIfAvailable<string>(kAction);
						bool		_hasAction			= false;

						if (_alertDict.Contains(kHasAction))
							_hasAction					= System.Convert.ToBoolean(_alertDict[kHasAction]);
						else
							_hasAction					= (_alertAction != null);

						// Set properties
						AlertBody						= _alertDict.GetIfAvailable<string>(kBody);
						iOSProperties.AlertAction		= _alertAction;
						iOSProperties.HasAction			= _hasAction;
						iOSProperties.LaunchImage		= _alertDict.GetIfAvailable<string>(kLaunchImage);
					}
				}
			}

			// Read sound, badge info from aps dictionary
			SoundName					=  _apsDict.GetIfAvailable<string>(kSound);
			iOSProperties.BadgeCount	=  _apsDict.GetIfAvailable<int>(kBadge);

			// Read user info from payload dictionary
			UserInfo					= _payloadDict.GetIfAvailable<IDictionary>(_userInfoKey);

			// Read fire date, repeat interval from payload dictionary
			string 	_fireDateStr		= _payloadDict.GetIfAvailable<string>(kFireDate);
			
			if (_fireDateStr != null)
				FireDate				= _fireDateStr.ToZuluFormatDateTimeLocal();
			
			RepeatInterval				= ConvertToRepeatInterval(_payloadDict.GetIfAvailable<CalendarUnit>(kRepeatIntervalKey));
		}

		#endregion

		#region Static Methods

		public static IDictionary CreateNotificationPayload (CrossPlatformNotification _notification)
		{
			IDictionary 			_payloadDict	= new Dictionary<string, object>();
			IDictionary 			_apsDict		= new Dictionary<string, object>();
			iOSSpecificProperties 	_iosProperties	= _notification.iOSProperties;
			string 					_userInfoKey	= NPSettings.Notification.iOS.UserInfoKey;

			// Add alert info
			IDictionary 			_alertDict		= new Dictionary<string, object>();

			if (_notification.AlertBody != null)
				_alertDict[kBody]					= _notification.AlertBody;

			if (_iosProperties.AlertAction != null)
				_alertDict[kAction]					= _iosProperties.AlertAction;

			if (_iosProperties.LaunchImage != null)
				_alertDict[kLaunchImage]			= _iosProperties.LaunchImage;

			_alertDict[kHasAction]					= System.Convert.ToInt32(_iosProperties.HasAction);

			// Add alert, badge, sound to "aps" dictionary
			_apsDict[kAlert]						= _alertDict;
			_apsDict[kBadge]						= _iosProperties.BadgeCount;

			if (_notification.SoundName != null)
				_apsDict[kSound]					= _notification.SoundName;

			// Add aps, user info, fire date, repeat interval to "payload" dictionary
			_payloadDict[kAPS]						= _apsDict;
			_payloadDict[_userInfoKey]				= _notification.UserInfo;
			_payloadDict[kFireDate]					= _notification.FireDate.ToStringUsingZuluFormat();
			_payloadDict[kRepeatIntervalKey]		= (int)ConvertToCalendarUnit(_notification.RepeatInterval);

			return _payloadDict;
		}

		public static eNotificationRepeatInterval ConvertToRepeatInterval (CalendarUnit _unit)
		{
			switch (_unit)
			{
			case CalendarUnit.Minute:
				return eNotificationRepeatInterval.MINUTE;

			case CalendarUnit.Day:
				return eNotificationRepeatInterval.DAY;

			case CalendarUnit.Week:
				return eNotificationRepeatInterval.WEEK;

			case CalendarUnit.Month:
				return eNotificationRepeatInterval.MONTH;

			case CalendarUnit.Year:
				return eNotificationRepeatInterval.YEAR;

			default:
				return eNotificationRepeatInterval.NONE;
			}
		}

		public static CalendarUnit ConvertToCalendarUnit (eNotificationRepeatInterval _repeatInterval)
		{
			switch (_repeatInterval)
			{
			case eNotificationRepeatInterval.MINUTE:
				return CalendarUnit.Minute;
				
			case eNotificationRepeatInterval.DAY:
				return CalendarUnit.Day;
				
			case eNotificationRepeatInterval.WEEK:
				return CalendarUnit.Week;
				
			case eNotificationRepeatInterval.MONTH:
				return CalendarUnit.Month;
				
			case eNotificationRepeatInterval.YEAR:
				return CalendarUnit.Year;
				
			default:
				return 0;
			}
		}

		#endregion
	}
}
#endif
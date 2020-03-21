#if USES_NOTIFICATION_SERVICE 
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using VoxelBusters.Utility;
using VoxelBusters.UASUtils;

namespace VoxelBusters.NativePlugins
{
	using Internal;

	/// <summary>
	/// Is a wrapper class that encapsulates details about the notification.
	/// </summary>
	public partial class CrossPlatformNotification
	{	
		#region Constants
		
		/// <summary>
		/// The default system sound. (Read Only)
		/// </summary>
		public	const	string		kDefaultSoundName	= "default.mp3";
		
		#endregion

		#region Properties

		/// <summary>
		/// The message displayed in the notification alert.
		/// </summary>
		public string AlertBody
		{ 
			get; 
			set; 
		}
		
		/// <summary>
		/// The date and time when the system should deliver the notification.
		/// </summary>
		public System.DateTime FireDate
		{ 
			get; 
			set; 
		}

		/// <summary>
		/// The interval at which the notification has to be rescheduled.
		/// </summary>
		public eNotificationRepeatInterval RepeatInterval
		{ 
			get; 
			set; 
		}

		/// <summary>
		/// A dictionary for passing custom information to the notified application.
		/// </summary>
		public IDictionary UserInfo
		{
			get; 
			set; 
		}
		
		/// <summary>
		/// The name of the sound file to play when an alert is displayed.
		/// </summary>
		/// <remarks>
		/// \note Specify the filename along with extension, eg: customsound.mp3.
		/// And incase if you are using sound other than default, then place the asset under folder <b>Assets/PluginResources</b>.
		/// </remarks>
		public string SoundName
		{
			get; 
			set;
		}

		/// <summary>
		/// The notification properties specific to iOS platform.
		/// </summary>
		public iOSSpecificProperties iOSProperties
		{
			get; 
			set; 
		}

		/// <summary>
		/// The notification properties specific to Android platform.
		/// </summary>
		public AndroidSpecificProperties AndroidProperties
		{
			get; 
			set; 
		}
		
		#endregion

		#region Constants	

		// Related to JSON representation
		private 	const 	string 		kAlertBodyKey			= "alert-body";
		private 	const 	string 		kFireDateKey			= "fire-date";
		private 	const 	string 		kRepeatIntervalKey		= "repeat-interval";
		private 	const 	string 		kUserInfoKey			= "user-info";
		private 	const 	string 		kSoundNameKey			= "sound-name";
		private 	const 	string 		kiOSPropertiesKey		= "ios-properties";
		private 	const	string 		kAndroidPropertiesKey	= "android-properties";

		internal 	const 	string 		kNotificationID			= "np-notification-identifier";

		#endregion

		#region Constructors
		
		public CrossPlatformNotification ()
		{
			AlertBody			= null;
			UserInfo			= null;
			RepeatInterval		= eNotificationRepeatInterval.NONE;
			SoundName			= null;
			iOSProperties		= null;
			AndroidProperties	= null;
		}

		internal CrossPlatformNotification (IDictionary _jsonDict)
		{
			// Get alert body
			AlertBody			= _jsonDict.GetIfAvailable<string>(kAlertBodyKey);

			// Get fire date, repeat interval
			string _fireDateStr	= _jsonDict.GetIfAvailable<string>(kFireDateKey);
			
			if (!string.IsNullOrEmpty(_fireDateStr))
				FireDate		= _fireDateStr.ToZuluFormatDateTimeLocal();

			RepeatInterval		= _jsonDict.GetIfAvailable<eNotificationRepeatInterval>(kRepeatIntervalKey);

			// Get user info
			UserInfo			= _jsonDict[kUserInfoKey] as IDictionary;

			// Get sound name
			SoundName			= _jsonDict.GetIfAvailable<string>(kSoundNameKey);

			// Get iOS specific properties, if included
			if (_jsonDict.Contains(kiOSPropertiesKey))
				iOSProperties		= new iOSSpecificProperties(_jsonDict[kiOSPropertiesKey] as IDictionary);

			// Get Android specific properties, if included
			if (_jsonDict.Contains(kAndroidPropertiesKey))
				AndroidProperties	= new AndroidSpecificProperties(_jsonDict[kAndroidPropertiesKey] as IDictionary);
		}
		
		#endregion
		
		#region Methods

		/// <summary>
		/// Returns a string used to uniquely identify the notification.
		/// </summary>
		public string GetNotificationID ()
		{
			if (UserInfo != null && UserInfo.Contains(kNotificationID))
				return UserInfo[kNotificationID] as string;
			
			return null;
		}
		
		internal string GenerateNotificationID ()
		{
			string _notificationID		= NPBinding.Utility.GetUUID();
			
			if (UserInfo == null)
				UserInfo	= new Dictionary<string, string>();
			
			UserInfo[kNotificationID]	= _notificationID;
			
			return _notificationID;
		}

		internal IDictionary JSONObject ()
		{
			Dictionary<string, object> _jsonDict	= new Dictionary<string, object>();
			_jsonDict[kAlertBodyKey]				= AlertBody;
			_jsonDict[kFireDateKey]					= FireDate.ToStringUsingZuluFormat();
			_jsonDict[kRepeatIntervalKey]			= (int)RepeatInterval;
			_jsonDict[kUserInfoKey]					= UserInfo;
			_jsonDict[kSoundNameKey]				= SoundName;

			if (iOSProperties != null)
				_jsonDict[kiOSPropertiesKey]		= iOSProperties.JSONObject();

			if (AndroidProperties != null)
				_jsonDict[kAndroidPropertiesKey]	= AndroidProperties.JSONObject();

			return _jsonDict;
		}
		
		#endregion
		
		#region Static Methods
		
		public static CrossPlatformNotification CreateNotificationFromPayload (string _notificationPayload)
		{
			IDictionary _payloadDict	= JSONUtility.FromJSON(_notificationPayload) as IDictionary;

			if (_payloadDict == null)
			{
				DebugUtility.Logger.LogError(Constants.kDebugTag, "[CrossPlatformNotification] Failed to create notification.");
				return null;
			}

#if UNITY_ANDROID
			return new AndroidNotificationPayload(_payloadDict);
#elif UNITY_IOS
			return new iOSNotificationPayload(_payloadDict);
#else
			return null;
#endif
		}
		
		#endregion
		
		#region Overriden Method

		public override string ToString ()
		{
			return string.Format ("CrossPlatformNotification: AlertBody={0}, FireDate={1}, RepeatInterval={2}", 
			                      AlertBody, FireDate, RepeatInterval);
		}
		
		#endregion
	}
}
#endif
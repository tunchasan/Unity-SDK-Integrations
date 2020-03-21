#if USES_NOTIFICATION_SERVICE && UNITY_ANDROID
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using VoxelBusters.Utility;

namespace VoxelBusters.NativePlugins.Internal
{
	public sealed partial class NotificationServiceAndroid : INotificationServicePlatform
	{
		#region Platform Native Info
		
		private class NativeInfo
		{
			// Handler class name
			internal class Class
			{
				internal const string NAME								= "com.voxelbusters.nativeplugins.features.notification.NotificationHandler";
			}
			
			// For holding method names
			internal class Methods
			{
				// Init
				internal const string INITIALIZE		 					= "initialize";

				// Common
				internal const string SET_NOTIFICATION_TYPES		 		= "setNotificationTypes";
				internal const string GET_ALLOWED_NOTIFICATION_TYPES		= "getAllowedNotificationTypes";
				internal const string CLEAR_ALL_NOTIFICATIONS				= "clearAllNotifications";
				internal const string GET_NOTIFICATION_SETTING_STATUS		= "areNotificationsAllowedByUser";

				// Local Notifications
				internal const string CANCEL_LOCAL_NOTIFICATION 			= "cancelLocalNotification";
				internal const string CANCEL_ALL_LOCAL_NOTIFICATIONS		= "cancelAllLocalNotifications";
				internal const string SCHEDULE_LOCAL_NOTIFICATION			= "scheduleLocalNotification";
				
				// Remote Notifications
				internal const string REGISTER_REMOTE_NOTIFICATIONS 		= "registerRemoteNotifications";
				internal const string UNREGISTER_REMOTE_NOTIFICATIONS 		= "unregisterRemoteNotifications";
			}
		}
		
		#endregion

		#region  Native Access Variables
		
		private AndroidJavaObject  	Plugin
		{
			get;
			set;
		}
		
		#endregion
	}
}
#endif
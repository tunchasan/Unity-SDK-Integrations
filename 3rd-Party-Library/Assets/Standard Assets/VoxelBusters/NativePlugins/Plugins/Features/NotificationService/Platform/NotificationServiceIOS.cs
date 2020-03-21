#if USES_NOTIFICATION_SERVICE && UNITY_IOS
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using VoxelBusters.Utility;

namespace VoxelBusters.NativePlugins.Internal
{
	public sealed partial class NotificationServiceIOS : INotificationServicePlatform
	{
		#region Binding Methods

		[DllImport("__Internal")]
		private static extern void initNotificationService (bool _captureLocalNotifications, bool _captureRemoteNotifications, string _keyForUserInfo);

		[DllImport("__Internal")]
		private static extern void registerNotificationTypes (int notificationTypes);

		[DllImport("__Internal")]
		private static extern int enabledNotificationTypes ();

		[DllImport("__Internal")]
		private static extern void scheduleLocalNotification (string _payload);

		[DllImport("__Internal")]
		private static extern void cancelLocalNotification (string _notificationID);

		[DllImport("__Internal")]
		private static extern void cancelAllLocalNotifications ();

		[DllImport("__Internal")]
		private static extern void clearNotifications ();
	
		[DllImport("__Internal")]
		private static extern void registerForRemoteNotifications ();

		[DllImport("__Internal")]
		private static extern void unregisterForRemoteNotifications ();

		#endregion

		#region Initialise Methods 
		
		public void Initialise (NotificationServiceSettings _settings)
		{
			string 	_keyForUserInfo				= _settings.iOS.UserInfoKey;
			bool	_captureLocalNotifications	= true;
			bool	_captureRemoteNotifications	= true;

			if (NPSettings.Application.SupportedAddonServices.UsesOneSignal)
				_captureRemoteNotifications		= false;

			initNotificationService(_captureLocalNotifications, _captureRemoteNotifications, _keyForUserInfo);
		}
		
		#endregion

		#region Notification Methods

		public void RegisterNotificationTypes (NotificationType _notificationTypes)
		{
			registerNotificationTypes((int)_notificationTypes);
		}

		public NotificationType EnabledNotificationTypes ()
		{
			return (NotificationType)enabledNotificationTypes();
		}

		#endregion

		#region Local Notification Methods

		public void ScheduleLocalNotification (CrossPlatformNotification _notification)
		{
			string	_payload	= iOSNotificationPayload.CreateNotificationPayload(_notification).ToJSON();
			scheduleLocalNotification(_payload);
		}
		
		public void CancelLocalNotification (string _notificationID)
		{
			cancelLocalNotification(_notificationID);
		}
		
		public void CancelAllLocalNotification ()
		{
			cancelAllLocalNotifications();
		}

		public void ClearNotifications ()
		{
			clearNotifications();
		}

		#endregion
		
		#region Remote Notification Methods

		public void RegisterForRemoteNotifications ()
		{		
			registerForRemoteNotifications();
		}

		public void UnregisterForRemoteNotifications ()
		{
			unregisterForRemoteNotifications();
		}

		#endregion
	}
}
#endif
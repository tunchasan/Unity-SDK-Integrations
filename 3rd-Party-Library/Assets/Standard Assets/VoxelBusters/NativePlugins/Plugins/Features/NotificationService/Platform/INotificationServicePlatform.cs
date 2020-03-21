#if USES_NOTIFICATION_SERVICE
using UnityEngine;
using System.Collections;

namespace VoxelBusters.NativePlugins.Internal
{
	public interface INotificationServicePlatform
	{
		#region Methods

		void Initialise (NotificationServiceSettings _settings);

		void RegisterNotificationTypes (NotificationType _notificationTypes);
		NotificationType EnabledNotificationTypes ();

		void ScheduleLocalNotification (CrossPlatformNotification _notification);
		void CancelLocalNotification (string _notificationID);
		void CancelAllLocalNotification ();
		void ClearNotifications ();

		void RegisterForRemoteNotifications ();
		void UnregisterForRemoteNotifications ();

		void ParseReceivedNotificationEventData (IDictionary _JSONDict, out CrossPlatformNotification _receivedNotification, out bool _isLaunchNotification);

		#endregion
	}
}
#endif
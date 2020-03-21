#if USES_NOTIFICATION_SERVICE
using UnityEngine;
using System.Collections;
using VoxelBusters.UASUtils;

namespace VoxelBusters.NativePlugins.Internal
{
	public class NotificationServiceUnsupported : INotificationServicePlatform 
	{
		#region Public Methods

		public void Initialise (NotificationServiceSettings _settings)
		{
			DebugUtility.Logger.LogWarning(Constants.kDebugTag, Constants.kNotSupportedInEditor);
		}

		public void RegisterNotificationTypes (NotificationType _notificationTypes)
		{
			DebugUtility.Logger.LogWarning(Constants.kDebugTag, Constants.kNotSupportedInEditor);
		}

		public NotificationType EnabledNotificationTypes ()
		{
			DebugUtility.Logger.LogWarning(Constants.kDebugTag, Constants.kNotSupportedInEditor);
			return (NotificationType)0;
		}

		public void ScheduleLocalNotification (CrossPlatformNotification _notification)
		{
			DebugUtility.Logger.LogWarning(Constants.kDebugTag, Constants.kNotSupportedInEditor);
		}
			
		public void CancelLocalNotification (string _notificationID)
		{
			DebugUtility.Logger.LogWarning(Constants.kDebugTag, Constants.kNotSupportedInEditor);
		}

		public void CancelAllLocalNotification ()
		{
			DebugUtility.Logger.LogWarning(Constants.kDebugTag, Constants.kNotSupportedInEditor);
		}

		public void ClearNotifications ()
		{
			DebugUtility.Logger.LogWarning(Constants.kDebugTag, Constants.kNotSupportedInEditor);
		}

		public void RegisterForRemoteNotifications ()
		{
			DebugUtility.Logger.LogWarning(Constants.kDebugTag, Constants.kNotSupportedInEditor);
		}

		public void UnregisterForRemoteNotifications ()
		{
			DebugUtility.Logger.LogWarning(Constants.kDebugTag, Constants.kNotSupportedInEditor);
		}

		public void ParseReceivedNotificationEventData (IDictionary _JSONDict, out CrossPlatformNotification _receivedNotification, out bool _isLaunchNotification)
		{
			_receivedNotification	= null;
			_isLaunchNotification	= false;
		}

		#endregion
	}
}
#endif
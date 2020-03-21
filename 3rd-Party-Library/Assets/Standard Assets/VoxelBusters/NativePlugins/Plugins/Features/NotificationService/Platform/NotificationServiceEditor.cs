#if USES_NOTIFICATION_SERVICE && UNITY_EDITOR
using UnityEngine;
using System.Collections;

namespace VoxelBusters.NativePlugins.Internal
{
	public sealed partial class NotificationServiceEditor : INotificationServicePlatform
	{
		#region Properties

		private 	EditorNotificationCenter	m_notificationCenter;

		#endregion

		#region Initialise Methods

		public void Initialise (NotificationServiceSettings _settings)
		{		
			// Cache reference
			m_notificationCenter	= EditorNotificationCenter.Instance;

			// Initialise notificaton center
			m_notificationCenter.Initialise();
		}

		#endregion

		#region Notification Methods

		public void RegisterNotificationTypes (NotificationType _notificationTypes)
		{
			m_notificationCenter.RegisterNotificationTypes(_notificationTypes);
		}

		public NotificationType EnabledNotificationTypes ()
		{
			return m_notificationCenter.EnabledNotificationTypes();
		}

		#endregion

		#region Local Notification Methods

		public void ScheduleLocalNotification (CrossPlatformNotification _notification)
		{
			m_notificationCenter.ScheduleLocalNotification(_notification);
		}

		public void CancelLocalNotification (string _notificationID)
		{
			IList 	_scheduledNotifications		= m_notificationCenter.ScheduledLocalNotifications;
			int 	_scheduledNotificationCount	= _scheduledNotifications.Count;

			for (int _iter = 0; _iter < _scheduledNotificationCount; _iter++)
			{
				CrossPlatformNotification _scheduledNotification	= _scheduledNotifications[_iter] as CrossPlatformNotification;
				string _scheduledNotificationID						= _scheduledNotification.GetNotificationID();

				// Cancel the notification which matches the given id
				if (!string.IsNullOrEmpty(_scheduledNotificationID) && _scheduledNotificationID.Equals(_notificationID))
				{
					m_notificationCenter.CancelLocalNotification(_scheduledNotification);
					break;
				}
			}
		}

		public void CancelAllLocalNotification ()
		{
			m_notificationCenter.CancelAllLocalNotifications();
		}
			
		public void ClearNotifications ()
		{
			m_notificationCenter.ClearNotifications();
		}
		
		#endregion

		#region Remote Notification Methods

		public void RegisterForRemoteNotifications ()
		{
			m_notificationCenter.RegisterForRemoteNotifications();
		}

		public void UnregisterForRemoteNotifications ()
		{
			m_notificationCenter.UnregisterForRemoteNotifications();
		}

		#endregion

		#region Event Methods

		public void ParseReceivedNotificationEventData (IDictionary _JSONDict, out CrossPlatformNotification _receivedNotification, out bool _isLaunchNotification)
		{
			// Set default values
			_receivedNotification	= null;
			_isLaunchNotification	= false;
		}

		#endregion
	}
}
#endif
using UnityEngine;
using System.Collections;

#if USES_NOTIFICATION_SERVICE
namespace VoxelBusters.NativePlugins
{
	public abstract class AbstractNotificatonServiceListener : MonoBehaviour 
	{
		#region Unity Methods
		
		protected virtual void OnEnable ()
		{
			// Register for events
			NotificationService.DidFinishRegisterForRemoteNotificationEvent += OnDidFinishRegisterForRemoteNotification;
			NotificationService.DidLaunchWithLocalNotificationEvent 		+= OnDidLaunchWithLocalNotification;
			NotificationService.DidReceiveLocalNotificationEvent 			+= OnDidReceiveLocalNotification;
			NotificationService.DidLaunchWithRemoteNotificationEvent 		+= OnDidLaunchWithRemoteNotification;
			NotificationService.DidReceiveRemoteNotificationEvent 			+= OnDidReceiveRemoteNotification;
		}
		
		protected virtual void OnDisable ()
		{
			// Unregister from events
			NotificationService.DidFinishRegisterForRemoteNotificationEvent -= OnDidFinishRegisterForRemoteNotification;
			NotificationService.DidLaunchWithLocalNotificationEvent 		-= OnDidLaunchWithLocalNotification;
			NotificationService.DidReceiveLocalNotificationEvent 			-= OnDidReceiveLocalNotification;
			NotificationService.DidLaunchWithRemoteNotificationEvent 		-= OnDidLaunchWithRemoteNotification;
			NotificationService.DidReceiveRemoteNotificationEvent 			-= OnDidReceiveRemoteNotification;
		}
		
		#endregion
		
		#region Abstract Methods
		
		public abstract void OnRemoteNotificationRegistrationSucceeded (string _deviceToken);
		
		public abstract void OnRemoteNotificationRegistrationFailed (string _error);
		
		public abstract void OnLocalNotificationReceived (CrossPlatformNotification _notification, bool _isLaunchNotification);
		
		public abstract void OnRemoteNotificationReceived (CrossPlatformNotification _notification, bool _isLaunchNotification);
		
		#endregion

		#region Callback Methods

		private void OnDidFinishRegisterForRemoteNotification (string _deviceToken, string _error)
		{
			if (_error == null)
				OnRemoteNotificationRegistrationSucceeded(_deviceToken);
			else
				OnRemoteNotificationRegistrationFailed(_error);
		}
		
		private void OnDidLaunchWithLocalNotification (CrossPlatformNotification _notification)
		{
			OnLocalNotificationReceived(_notification, true);
		}

		private void OnDidReceiveLocalNotification (CrossPlatformNotification _notification)
		{
			OnLocalNotificationReceived(_notification, false);
		}
		
		private void OnDidLaunchWithRemoteNotification (CrossPlatformNotification _notification)
		{
			OnRemoteNotificationReceived(_notification, true);
		}
		
		private void OnDidReceiveRemoteNotification (CrossPlatformNotification _notification)
		{
			OnRemoteNotificationReceived(_notification, false);
		}

		#endregion
	}
}
#endif
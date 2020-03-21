using UnityEngine;
using System.Collections;

// Add these Namespaces
using VoxelBusters.NativePlugins;
using VoxelBusters.Utility;

namespace VoxelBusters.NativePlugins.Demo
{
#if !USES_NOTIFICATION_SERVICE
	public class RemoteNotificationTest : NPDisabledFeatureDemo 
#else
	public class RemoteNotificationTest : MonoBehaviour 
#endif
	{
		#region Fields

#pragma warning disable
		[SerializeField, EnumMaskField(typeof(NotificationType))]
		private NotificationType	m_notificationType;
#pragma warning restore

		#endregion

#if !USES_NOTIFICATION_SERVICE
	}
#else
		#region Unity Methods
		
		private void Start()
		{
			NPBinding.NotificationService.RegisterNotificationTypes(m_notificationType);
		}

		private void OnEnable ()
		{
			// Register RemoteNotificated related callbacks
			NotificationService.DidFinishRegisterForRemoteNotificationEvent	+= DidFinishRegisterForRemoteNotificationEvent;
			NotificationService.DidReceiveRemoteNotificationEvent			+= DidReceiveRemoteNotificationEvent;

			//Add below for local notification
			//NotificationService.DidReceiveLocalNotificationEvent 			+= DidReceiveLocalNotificationEvent;
		}
		
		private void OnDisable ()
		{
			// Un-Register from callbacks
			NotificationService.DidFinishRegisterForRemoteNotificationEvent	-= DidFinishRegisterForRemoteNotificationEvent;
			NotificationService.DidReceiveRemoteNotificationEvent			-= DidReceiveRemoteNotificationEvent;
			
			//Add below for local notification
			//NotificationService.DidReceiveLocalNotificationEvent 			-= DidReceiveLocalNotificationEvent;
		}

		#endregion

		#region GUI Methods
		
		private void OnGUI () 
		{
			if(GUILayout.Button("Register for Remote Notifications", GUILayout.Width(Screen.width/2f),  GUILayout.Height(Screen.height * 0.2f)))
			{
				NPBinding.NotificationService.RegisterForRemoteNotifications(); //This triggers a event. so capture it by registering to that event.
			}
		}

		#endregion

		#region API Callback Methods
		
		private void DidReceiveLocalNotificationEvent (CrossPlatformNotification _notification)
		{
			Debug.Log("Received DidReceiveLocalNotificationEvent : " + _notification.ToString());
		}
		
		private void DidReceiveRemoteNotificationEvent (CrossPlatformNotification _notification)
		{
			Debug.Log("Received DidReceiveRemoteNotificationEvent : " + _notification.ToString());
		}
		
		private void DidFinishRegisterForRemoteNotificationEvent (string _deviceToken, string _error)
		{
			if(string.IsNullOrEmpty(_error))
			{
				Debug.Log("Device Token : " + _deviceToken);
			}
			else
			{
				Debug.Log("Error in registering for remote notifications : " + _deviceToken);
			}
		}

		#endregion
	}
#endif
}
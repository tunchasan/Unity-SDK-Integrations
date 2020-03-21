#if USES_NOTIFICATION_SERVICE 
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using VoxelBusters.Utility;
using VoxelBusters.UASUtils;

namespace VoxelBusters.NativePlugins
{
	using Internal;

	public sealed partial class NotificationService : MonoBehaviour 
	{
		#region Delegates

		/// <summary>
		/// Delegate that will be called when your app completes registration with Push Notification service.
		/// </summary>
		/// <param name="_deviceToken">A token that identifies the device to Push Notification service.</param>
		/// <param name="_error">If the operation was successful, this value is nil; otherwise, this parameter holds the description of the problem that occurred.</param>
		public delegate void RegisterForRemoteNotificationCompletion (string _deviceToken, string _error);

		/// <summary>
		/// Delegate that will be called when your app receives a notification.
		/// </summary>
		/// <param name="_notification">The <c>CrossPlatformNotification</c> object holds information about received notification.</param>
		public delegate void ReceivedNotificationResponse (CrossPlatformNotification _notification);

		#endregion

		#region Events

		/// <summary>
		/// Event that will be called when your app completes registration with Push Notification service.
		/// </summary>
		/// <description>
		/// After you call the <see cref="RegisterForRemoteNotifications"/> method, the app calls this method when device registration completes successfully or when there is an error in the registration process.
		/// When registration completes successfully, connect with your push notification server and give the device token to it. 
		/// Push notification server pushes notifications only to the device represented by the token.
		/// </description>
		/// <example>
		/// The following code example demonstrates how to use registration event.
		/// <code>
		/// using UnityEngine;
		/// using System.Collections;
		/// using VoxelBusters.NativePlugins;
		/// 
		/// public class ExampleClass : MonoBehaviour 
		/// {
		/// 	private void OnEnable ()
		/// 	{
		/// 		// Registering for event
		/// 	    NotificationService.DidFinishRegisterForRemoteNotificationEvent	+= OnFinishedRemoteNotificationRegistration;
		///     }
		/// 
		/// 	private void OnDisable ()
		/// 	{
		/// 		// Unregistering event
		/// 	    NotificationService.DidFinishRegisterForRemoteNotificationEvent	-= OnFinishedRemoteNotificationRegistration;
		/// 	}
		/// 
		/// 	private void OnFinishedRemoteNotificationRegistration (string _deviceToken, string _error)
		/// 	{
		/// 		if (_error == null)
		/// 		{
		/// 			// Send device token to your server
		/// 		}
		/// 		else
		/// 		{
		/// 			// Something went wrong
		/// 		}
		/// 	}
		/// }
		/// </code>
		/// </example>
		public static event RegisterForRemoteNotificationCompletion DidFinishRegisterForRemoteNotificationEvent;

		/// <summary>
		/// Event that will be called when your app was launched as a result of local notification.
		/// </summary>
		/// <example>
		/// The following code example demonstrates how to use launched with local notification event.
		/// <code>
		/// using UnityEngine;
		/// using System.Collections;
		/// using VoxelBusters.NativePlugins;
		/// 
		/// public class ExampleClass : MonoBehaviour 
		/// {
		/// 	private void OnEnable ()
		/// 	{
		/// 		// Registering for event
		/// 	    NotificationService.DidLaunchWithLocalNotificationEvent	+= OnLaunchedWithLocalNotification;
		///     }
		/// 
		/// 	private void OnDisable ()
		/// 	{
		/// 		// Unregistering event
		/// 	    NotificationService.DidLaunchWithLocalNotificationEvent	-= OnLaunchedWithLocalNotification;
		/// 	}
		/// 
		/// 	private void OnLaunchedWithLocalNotification (CrossPlatformNotification _notification)
		/// 	{
		/// 		// Handle received notification
		/// 	}
		/// }
		/// </code>
		/// </example>
		public static event ReceivedNotificationResponse DidLaunchWithLocalNotificationEvent;
		
		/// <summary>
		/// Event that will be called when your app was launched as a result of remote notification.
		/// </summary>
		/// <example>
		/// The following code example demonstrates how to use launched with remote notification event.
		/// <code>
		/// using UnityEngine;
		/// using System.Collections;
		/// using VoxelBusters.NativePlugins;
		/// 
		/// public class ExampleClass : MonoBehaviour 
		/// {
		/// 	private void OnEnable ()
		/// 	{
		/// 		// Registering for event
		/// 	    NotificationService.DidLaunchWithRemoteNotificationEvent	+= OnLaunchedWithRemoteNotification;
		///     }
		/// 
		/// 	private void OnDisable ()
		/// 	{
		/// 		// Unregistering event
		/// 	    NotificationService.DidLaunchWithRemoteNotificationEvent	-= OnLaunchedWithRemoteNotification;
		/// 	}
		/// 
		/// 	private void OnLaunchedWithRemoteNotification (CrossPlatformNotification _notification)
		/// 	{
		/// 		// Handle received notification
		/// 	}
		/// }
		/// </code>
		/// </example>
		public static event ReceivedNotificationResponse DidLaunchWithRemoteNotificationEvent;

		/// <summary>
		/// Event that will be called when your app has received a local notification.
		/// </summary>
		/// <remarks>
		/// \note This notifies about the notification received when your app is running.
		/// </remarks>
		/// <example>
		/// The following code example demonstrates how to use local notification received event.
		/// <code>
		/// using UnityEngine;
		/// using System.Collections;
		/// using VoxelBusters.NativePlugins;
		/// 
		/// public class ExampleClass : MonoBehaviour 
		/// {
		/// 	private void OnEnable ()
		/// 	{
		/// 		// Registering for event
		/// 	    NotificationService.DidReceiveLocalNotificationEvent	+= OnReceivingLocalNotification;
		///     }
		/// 
		/// 	private void OnDisable ()
		/// 	{
		/// 		// Unregistering event
		/// 	    NotificationService.DidReceiveLocalNotificationEvent	-= OnReceivingLocalNotification;
		/// 	}
		/// 
		/// 	private void OnReceivingLocalNotification (CrossPlatformNotification _notification)
		/// 	{
		/// 		// Handle received notification
		/// 	}
		/// }
		/// </code>
		/// </example>
		public static event ReceivedNotificationResponse DidReceiveLocalNotificationEvent;

		/// <summary>
		/// Event that will be called when your app has received a remote notification.
		/// </summary>
		/// <remarks>
		/// \note This notifies about the notification received when your app is running.
		/// </remarks>
		/// <example>
		/// The following code example demonstrates how to use remote notification received event.
		/// <code>
		/// using UnityEngine;
		/// using System.Collections;
		/// using VoxelBusters.NativePlugins;
		/// 
		/// public class ExampleClass : MonoBehaviour 
		/// {
		/// 	private void OnEnable ()
		/// 	{
		/// 		// Registering for event
		/// 	    NotificationService.DidReceiveRemoteNotificationEvent	+= OnReceivingRemoteNotification;
		///     }
		/// 
		/// 	private void OnDisable ()
		/// 	{
		/// 		// Unregistering event
		/// 	    NotificationService.DidReceiveRemoteNotificationEvent	-= OnReceivingRemoteNotification;
		/// 	}
		/// 
		/// 	private void OnReceivingRemoteNotification (CrossPlatformNotification _notification)
		/// 	{
		/// 		// Handle received notification
		/// 	}
		/// }
		/// </code>
		/// </example>
		public static event ReceivedNotificationResponse DidReceiveRemoteNotificationEvent;
		
		#endregion

		#region Local Notification Callback Methods

		private void DidReceiveLocalNotification (string _JSONString)
		{
			IDictionary		_JSONDict	= (IDictionary)JSONUtility.FromJSON(_JSONString);

			CrossPlatformNotification 	_receivedNotification;
			bool						_isLaunchNotification;
			m_platform.ParseReceivedNotificationEventData(_JSONDict, out _receivedNotification, out _isLaunchNotification);

			if (_receivedNotification != null)
			{
				StartCoroutine(DidReceiveLocalNotification(_receivedNotification, _isLaunchNotification));
			}
		}

		private IEnumerator DidReceiveLocalNotification (CrossPlatformNotification _receivedNotification, bool _isLaunchNotification)
		{
			while (!m_canSendEvents)
				yield return null;

			if (_isLaunchNotification)
			{
				DebugUtility.Logger.LogWarning(Constants.kDebugTag, "[NotificationService] App was launched from local notification.");
				
				if (DidLaunchWithLocalNotificationEvent != null)
					DidLaunchWithLocalNotificationEvent(_receivedNotification);
			}
			else
			{
				DebugUtility.Logger.LogWarning(Constants.kDebugTag, "[NotificationService] Received new local notification.");

				if (DidReceiveLocalNotificationEvent != null)
					DidReceiveLocalNotificationEvent(_receivedNotification);
			}
		}

		#endregion

		#region Remote Notification Callback Methods

		private void DidRegisterRemoteNotification (string _deviceToken)
		{
			StartCoroutine(DidFinishRegisterForRemoteNotification(_deviceToken, null));
		}

		private void DidFailToRegisterRemoteNotifications (string _errorDescription)
		{			
			StartCoroutine(DidFinishRegisterForRemoteNotification(null, _errorDescription));
		}

		private IEnumerator DidFinishRegisterForRemoteNotification (string _deviceToken, string _error)
		{
			// Wait until component is ready to fire events
			while (!m_canSendEvents)
				yield return null;

#if NP_DEBUG
			Debug.Log("[NotificationService] Remote notification registration finished.");
#endif
			
			if (DidFinishRegisterForRemoteNotificationEvent != null)
				DidFinishRegisterForRemoteNotificationEvent(_deviceToken, _error);
		}

		private void DidReceiveRemoteNotification (string _JSONString)
		{
			IDictionary		_JSONDict	= (IDictionary)JSONUtility.FromJSON(_JSONString);

			CrossPlatformNotification 	_receivedNotification;
			bool						_isLaunchNotification;
			m_platform.ParseReceivedNotificationEventData(_JSONDict, out _receivedNotification, out _isLaunchNotification);
			
			if (_receivedNotification != null)
			{
				StartCoroutine(DidReceiveRemoteNotification(_receivedNotification, _isLaunchNotification));
			}
		}

		private IEnumerator DidReceiveRemoteNotification (CrossPlatformNotification _receivedNotification, bool _isLaunchNotification)
		{
			while (!m_canSendEvents)
				yield return null;

			if (_isLaunchNotification)
			{
				DebugUtility.Logger.LogWarning(Constants.kDebugTag, "[NotificationService] App was launched from remote notification.");
				
				if (DidLaunchWithRemoteNotificationEvent != null)
					DidLaunchWithRemoteNotificationEvent(_receivedNotification);
			}
			else
			{
				DebugUtility.Logger.LogWarning(Constants.kDebugTag, "[NotificationService] Received remote notification.");
				
				if (DidReceiveRemoteNotificationEvent != null)
					DidReceiveRemoteNotificationEvent(_receivedNotification);
			}
		}

		#endregion

		#region Addon Callback Methods

#if USES_ONE_SIGNAL
		private void DidReceiveOneSignalNotification (OSNotification _notification)
		{
			CrossPlatformNotification	_receivedNotification	= new OneSignalNotificationPayload(_payload: _notification.payload);
			StartCoroutine(DidReceiveRemoteNotification(_receivedNotification, false));
		}
		private void DidReceiveOneSignalLaunchNotification (OSNotificationOpenedResult _notificationOpened)
		{
			CrossPlatformNotification	_receivedNotification	= new OneSignalNotificationPayload(_payload: _notificationOpened.notification.payload);
			StartCoroutine(DidReceiveRemoteNotification(_receivedNotification, true));
		}

        private void DidReceiveIDsAvaialble(string _playerID, string _deviceToken)
        {
            DidRegisterRemoteNotification(_deviceToken);
        }
#endif

		#endregion
	}
}
#endif
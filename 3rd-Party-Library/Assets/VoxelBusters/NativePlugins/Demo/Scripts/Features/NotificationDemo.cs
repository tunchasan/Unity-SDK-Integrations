using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using VoxelBusters.Utility;
using VoxelBusters.NativePlugins;

namespace VoxelBusters.NativePlugins.Demo
{
#if !USES_NOTIFICATION_SERVICE
	public class NotificationDemo : NPDisabledFeatureDemo 
#else
	public class NotificationDemo : NPDemoBase 
#endif
	{
		#region Properties

#pragma warning disable
		[SerializeField, EnumMaskField(typeof(NotificationType))]
		private 	NotificationType	m_notificationType;
		private 	ArrayList 			m_scheduledNotificationIDList	= new ArrayList();
#pragma warning restore

		#endregion

#if !USES_NOTIFICATION_SERVICE
	}
#else
		#region Unity Methods

		protected override void Start ()
		{
			base.Start ();

			// Set additional info texts
			AddExtraInfoTexts(
				"You can configure this feature in NPSettings->Notification Settings.",
				"For Android platform notification, complete customisation of payload keys is allowed. You can modify these keys in Notification Settings.",
				"Notification workflow is straight forward." +
					"\n1. Register for notification types that your application needs." +
					"\n2. Register for remote notifications, only in case your application needs remote notification support.",
				"You can also validate your payload string in Unity Editor itself. Just select Editor Notification Center from Menu (Window->Voxel Busters->Native Plugins) and try push notification.");
		}
		
		protected override void OnEnable ()
		{
			base.OnEnable ();

			// Register for callbacks
			NotificationService.DidFinishRegisterForRemoteNotificationEvent	+= DidFinishRegisterForRemoteNotificationEvent;
			NotificationService.DidLaunchWithLocalNotificationEvent			+= DidLaunchWithLocalNotificationEvent;
			NotificationService.DidLaunchWithRemoteNotificationEvent		+= DidLaunchWithRemoteNotificationEvent;
			NotificationService.DidReceiveLocalNotificationEvent 			+= DidReceiveLocalNotificationEvent;
			NotificationService.DidReceiveRemoteNotificationEvent			+= DidReceiveRemoteNotificationEvent;
		}
		
		protected override void OnDisable ()
		{
			base.OnDisable ();
			
			// Un-Register from callbacks
			NotificationService.DidFinishRegisterForRemoteNotificationEvent	-= DidFinishRegisterForRemoteNotificationEvent;
			NotificationService.DidLaunchWithLocalNotificationEvent 		-= DidLaunchWithLocalNotificationEvent;
			NotificationService.DidLaunchWithRemoteNotificationEvent 		-= DidLaunchWithRemoteNotificationEvent;
			NotificationService.DidReceiveLocalNotificationEvent 			-= DidReceiveLocalNotificationEvent;
			NotificationService.DidReceiveRemoteNotificationEvent			-= DidReceiveRemoteNotificationEvent;
		}
		
		#endregion
		
		#region GUI Methods
		
		protected override void DisplayFeatureFunctionalities ()
		{
			base.DisplayFeatureFunctionalities();

			DrawRegisterAPI();
			DrawScheduleNotificationAPI();
			DrawCancelNotificationAPI();
		}
		
		private void DrawRegisterAPI ()
		{
			GUILayout.Label("Register/Unregister", kSubTitleStyle);

			GUILayout.Box("[NOTE] Launch notification events: DidLaunchWithLocalNotificationEvent/DidLaunchWithRemoteNotificationEvent are fired only if application was launched using notification.");
			GUILayout.Box("[NOTE] DidReceiveLocalNotificationEvent is fired when application receives local notification.");
			GUILayout.Box("[NOTE] DidReceiveRemoteNotificationEvent is fired when application receives remote notification.");

			if (GUILayout.Button("Register Notification Types [None, Alert, Badge and Sound]"))
			{
				RegisterNotificationTypes(m_notificationType);
				DisplayRegisteredNotificationTypes(m_notificationType);
			}

			if (GUILayout.Button("Enabled Notification Types"))
			{
				NotificationType _registeredTypes	= EnabledNotificationTypes();

				DisplayRegisteredNotificationTypes(_registeredTypes);
			}
			
			if (GUILayout.Button("Register For Remote Notifications"))
			{
				RegisterForRemoteNotifications();
			}

			GUILayout.Box("[NOTE] When register for remote notification request completes, DidFinishRegisterForRemoteNotificationEvent is fired.");

			if (GUILayout.Button("Unregister For Remote Notifications"))
			{
				UnregisterForRemoteNotifications();
				AddNewResult("Unregistered for remote notifications");
			}
		}
		
		private void DrawScheduleNotificationAPI ()
		{
			GUILayout.Label("Schedule Notifications", kSubTitleStyle);
			
			if (GUILayout.Button("Schedule Local Notification (After 1min, Repeat: Disabled)"))
			{
				// Schedules a local notification after 1 min
				string _nID = ScheduleLocalNotification(CreateNotification(60, eNotificationRepeatInterval.NONE));
				
				// Add notification id to list
				m_scheduledNotificationIDList.Add(_nID);
				
				// Update info
				AddNewResult("Newly scheduled notification ID = " + _nID);
			}
			
			if (GUILayout.Button("Schedule Local Notification (After 1min, Repeat: Every Minute)"))
			{
				// Schedules a local notification after 1 min and it keeps rescheduling for every minute
				string _nID = ScheduleLocalNotification(CreateNotification(60, eNotificationRepeatInterval.MINUTE));
				
				// Add notification id to list
				m_scheduledNotificationIDList.Add(_nID);
				
				// Update info
				AddNewResult("Newly scheduled notification ID = " + _nID);
			}
			
			if (GUILayout.Button("Schedule Local Notification (After 1min, Repeat: Every Hour)"))
			{
				// Schedules a local notification after 1 min and it keeps rescheduling for every hour
				string _nID = ScheduleLocalNotification(CreateNotification(60, eNotificationRepeatInterval.HOUR));
				
				// Add notification id to list
				m_scheduledNotificationIDList.Add(_nID);
				
				// Update info
				AddNewResult("Newly scheduled notification ID = " + _nID);
			}
		}
		
		private void DrawCancelNotificationAPI ()
		{
			GUILayout.Label("Cancel Notifications", kSubTitleStyle);
			
			if (GUILayout.Button("Cancel Local Notification"))
			{
				if (m_scheduledNotificationIDList.Count > 0)
				{
					string _nID		= m_scheduledNotificationIDList[0] as string;
					
					AddNewResult("Cancelling notification with ID=" + _nID);
					
					CancelLocalNotification(_nID);
					
					// Remove notification id
					m_scheduledNotificationIDList.RemoveAt(0);
				}
				else
				{
					AddNewResult("No Scheduled Local Notifications");
				}
			}
			
			if (GUILayout.Button("Cancel All Local Notifications"))
			{
				// Clearing list
				m_scheduledNotificationIDList.Clear();
				
				// Cancelling all notifications
				CancelAllLocalNotifications();
				
				// Update info
				AddNewResult("Scheduled notifications are invalidated");
			}
			
			if (GUILayout.Button("Clear Notifications"))
			{
				ClearNotifications();
				
				// Update info
				AddNewResult("Cleared notifications from notification bar.");
			}
		}
		
		#endregion

		#region API Methods

		private void RegisterNotificationTypes (NotificationType _notificationTypes)
		{
			NPBinding.NotificationService.RegisterNotificationTypes(_notificationTypes);
		}

		private NotificationType EnabledNotificationTypes ()
		{
			return NPBinding.NotificationService.EnabledNotificationTypes();
		}

		private void RegisterForRemoteNotifications ()
		{
			NPBinding.NotificationService.RegisterForRemoteNotifications();
		}

		private void UnregisterForRemoteNotifications ()
		{
			NPBinding.NotificationService.UnregisterForRemoteNotifications();
		}

		private string ScheduleLocalNotification (CrossPlatformNotification _notification)
		{
			return NPBinding.NotificationService.ScheduleLocalNotification(_notification);
		}

		private void CancelLocalNotification (string _notificationID)
		{
			NPBinding.NotificationService.CancelLocalNotification(_notificationID);
		}
		
		private void CancelAllLocalNotifications ()
		{
			NPBinding.NotificationService.CancelAllLocalNotification();
		}
		
		private void ClearNotifications ()
		{
			NPBinding.NotificationService.ClearNotifications();
		}

		#endregion

		#region API Callback Methods
		
		private void DidLaunchWithLocalNotificationEvent (CrossPlatformNotification _notification)
		{
			AddNewResult("Application did launch with local notification.");
			AppendNotificationResult(_notification);
		}
		
		private void DidLaunchWithRemoteNotificationEvent (CrossPlatformNotification _notification)
		{
			AddNewResult("Application did launch with remote notification.");
			AppendNotificationResult(_notification);
		}

		private void DidReceiveLocalNotificationEvent (CrossPlatformNotification _notification)
		{
			AddNewResult("Application received local notification.");
			AppendNotificationResult(_notification);
		}
		
		private void DidReceiveRemoteNotificationEvent (CrossPlatformNotification _notification)
		{
			AddNewResult("Application received remote notification.");
			AppendNotificationResult(_notification);
		}
		
		private void DidFinishRegisterForRemoteNotificationEvent (string _deviceToken, string _error)
		{
			AddNewResult(string.Format("Request to register for remote notification finished. Error = {0}.", _error.GetPrintableString()));
			AppendResult("DeviceToken = " + _deviceToken);
		}

		#endregion
		
		#region Misc. Methods

		private void DisplayRegisteredNotificationTypes (NotificationType _notificationTypes)
		{
			if ((int)_notificationTypes == 0)
			{
				AddNewResult("Registered for none of the notification types.");
				return;
			}
			else
			{
				AddNewResult("Registered for following notification types:");
				
				if ((_notificationTypes & NotificationType.Badge) != 0)
					AppendResult("Badge");
				
				if ((_notificationTypes & NotificationType.Sound) != 0)
					AppendResult("Sound");
				
				if ((_notificationTypes & NotificationType.Alert) != 0)
					AppendResult("Alert");
			}
		}
		
		void AppendNotificationResult (CrossPlatformNotification _notification)
		{
			string _alert 					= _notification.AlertBody;

#pragma warning disable
			// Exists only for local notifications which will be useful if we need to cancel a local notification
			string _notificationIdentifier 	= _notification.GetNotificationID();
#pragma warning restore
			
			//Get UserInfo details
			IDictionary _userInfo 			= _notification.UserInfo;
			
			//Can get specific details of a notification based on platform
			/*
					//For Android
					_notification.AndroidProperties.ContentTitle
					_notification.AndroidProperties.TickerText

					//For iOS
					_notification.iOSProperties.AlertAction;
					_notification.iOSProperties.BadgeCount;
				*/
			
			// Append to result list
			AppendResult("Alert = " + _alert);

			// Append user info
			string _userInfoDetails = null;

			if (_userInfo != null)
			{
				// Initialize and iterate through the list
				_userInfoDetails	= string.Empty;

				foreach (string _key in _userInfo.Keys)
				{
					_userInfoDetails	+= _key + " : " + _userInfo[_key] + "\n";
				}
			}
			else
			{
				_userInfoDetails	= "NULL";
			}

			AppendResult("UserInfo = " + _userInfoDetails);	
		}
 
		private CrossPlatformNotification CreateNotification (long _fireAfterSec, eNotificationRepeatInterval _repeatInterval)
		{
			// User info
			IDictionary _userInfo			= new Dictionary<string, string>();
			_userInfo["data"]				= "custom data";
			
			CrossPlatformNotification.iOSSpecificProperties _iosProperties			= new CrossPlatformNotification.iOSSpecificProperties();
			_iosProperties.HasAction		= true;
			_iosProperties.AlertAction		= "alert action";
			
			CrossPlatformNotification.AndroidSpecificProperties _androidProperties	= new CrossPlatformNotification.AndroidSpecificProperties();
			_androidProperties.ContentTitle	= "content title";
			_androidProperties.TickerText	= "ticker ticks over here";
			_androidProperties.LargeIcon	= "NativePlugins.png"; //Keep the files in Assets/PluginResources/Android or Common folder.
			
			CrossPlatformNotification _notification	= new CrossPlatformNotification();
			_notification.AlertBody			= "alert body"; //On Android, this is considered as ContentText
			_notification.FireDate			= System.DateTime.Now.AddSeconds(_fireAfterSec);
			_notification.RepeatInterval	= _repeatInterval;
			_notification.SoundName			= "Notification.mp3"; //Keep the files in Assets/PluginResources/Android or iOS or Common folder.
			_notification.UserInfo			= _userInfo;
			_notification.iOSProperties		= _iosProperties;
			_notification.AndroidProperties	= _androidProperties;

			return _notification;
		}

		#endregion
	}
#endif
}
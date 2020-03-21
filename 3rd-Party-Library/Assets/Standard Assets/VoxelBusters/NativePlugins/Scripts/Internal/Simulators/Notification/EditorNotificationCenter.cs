#if USES_NOTIFICATION_SERVICE && UNITY_EDITOR
using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using VoxelBusters.Utility;
using VoxelBusters.UASUtils;

namespace VoxelBusters.NativePlugins.Internal
{
	public class EditorNotificationCenter : SharedScriptableObject<EditorNotificationCenter>
	{
		#region Constants
		
		// Preference keys
		private 	const 	string 				kDidStartWithLocalNotification					= "np-start-local-notification";
		private 	const 	string 				kDidStartWithRemoteNotification					= "np-start-remote-notification";
		private 	const 	string 				kScheduledLocalNotifications					= "np-scheduled-local-notifications";
		private 	const 	string 				kLocalNotifications								= "np-local-notifications";
		private 	const 	string 				kRemoteNotifications							= "np-remote-notifications";
		
		// Event callbacks
		private 	const 	string				kDidReceiveAppLaunchInfoEvent					= "DidReceiveAppLaunchInfo";
		private 	const 	string				kDidReceiveLocalNotificationEvent				= "DidReceiveLocalNotification";
		private 	const 	string				kDidRegisterRemoteNotificationEvent				= "DidRegisterRemoteNotification";
		private 	const 	string				kDidFailToRegisterRemoteNotificationEvent		= "DidFailToRegisterRemoteNotifications";
		private 	const 	string				kDidReceiveRemoteNotificationEvent				= "DidReceiveRemoteNotification";
		
		#endregion

		#region Fields

		[SerializeField, EnumMaskField(typeof(NotificationType))]
		private 			NotificationType	m_supportedNotificationTypes;
		[SerializeField]
		private 			bool				m_isRegisteredForRemoteNotifications;					

		#endregion

		#region Properties

		public NotificationType SupportedNotificationTypes
		{
			get
			{
				return (NotificationType)m_supportedNotificationTypes.GetValue();
			}

			private set
			{
				m_supportedNotificationTypes	= value;
			}
		}

		public List<CrossPlatformNotification> ScheduledLocalNotifications
		{
			get;
			private set;
		}

		public List<CrossPlatformNotification> LocalNotifications
		{
			get;
			private set;
		}

		public List<CrossPlatformNotification> RemoteNotifications
		{
			get;
			private set;
		}

		#endregion

		#region Constructors

		private EditorNotificationCenter()
		{
			ScheduledLocalNotifications			= new List<CrossPlatformNotification>();
			LocalNotifications					= new List<CrossPlatformNotification>();
			RemoteNotifications					= new List<CrossPlatformNotification>();

			// Invoke methods
			EditorUtils.InvokeRepeating(MonitorScheduledLocalNotifications, 1f, 1f);
		}

		#endregion

		#region Unity Callbacks

		/*
		 public void OnAfterDeserialize()
		{}

		public void OnBeforeSerialize()
		{			
			// Serialise
			Serialise();
		}
        */

		protected override void OnEnable()
		{
			base.OnEnable();
		
			// Deserialise
			Deserialise();
		}

        protected override void OnDisable()
        {
            base.OnDisable();

            // Serialise
            Serialise();
        }

        #endregion

        #region Initialise

        public void Initialise()
		{
			string 	_localNotificationPayload	= EditorPrefs.GetString(kDidStartWithLocalNotification, string.Empty);
			string 	_remoteNotificationPayload	= EditorPrefs.GetString(kDidStartWithRemoteNotification, string.Empty);
		
			// Get launch local notification
			if (!string.IsNullOrEmpty(_localNotificationPayload))
			{
				CrossPlatformNotification _notification	= new CrossPlatformNotification(JSONUtility.FromJSON(_localNotificationPayload) as IDictionary);
				SendLocalNotification(_notification, true);
			}
			else if (!string.IsNullOrEmpty(_remoteNotificationPayload))		
			{
				CrossPlatformNotification _notification	= new CrossPlatformNotification(JSONUtility.FromJSON(_remoteNotificationPayload) as IDictionary);
				SendRemoteNotification(_notification, true);
			}

			// Remove saved values
			EditorPrefs.DeleteKey(kDidStartWithLocalNotification);
			EditorPrefs.DeleteKey(kDidStartWithRemoteNotification);
		}

		#endregion

		#region Local Notification Methods

		public void RegisterNotificationTypes(NotificationType _notificationTypes)
		{
			m_supportedNotificationTypes	= _notificationTypes;
		}

		public NotificationType EnabledNotificationTypes()
		{
			return m_supportedNotificationTypes;
		}
		
		public void ScheduleLocalNotification(CrossPlatformNotification _notification)
		{
			if (0 == (int)m_supportedNotificationTypes)
			{
				return;
			}

			// Add this new notification
			ScheduledLocalNotifications.Add(_notification);
		}

		public void CancelLocalNotification(CrossPlatformNotification _notification)
		{
			if (ScheduledLocalNotifications.Contains(_notification))
				ScheduledLocalNotifications.Remove(_notification);
		}

		public void CancelAllLocalNotifications()
		{
			ScheduledLocalNotifications.Clear();
		}

		public void ClearNotifications()
		{
			ClearLocalNotifications();
			ClearRemoteNotifications();

			// Redraws inspector
			EditorUtility.SetDirty(this);
		}

		private void ClearLocalNotifications()
		{
			LocalNotifications.Clear();
		}

		private void MonitorScheduledLocalNotifications()
		{
			if (ScheduledLocalNotifications == null || ScheduledLocalNotifications.Count == 0)
				return;

			int _scheduledNotificationsCount	= ScheduledLocalNotifications.Count;
			System.DateTime _now				= System.DateTime.Now;

			for (int _iter = 0; _iter < _scheduledNotificationsCount; _iter++)
			{
				CrossPlatformNotification _scheduledNotification	= ScheduledLocalNotifications[_iter];
				int _secondsSinceNow								= (int)(_now - _scheduledNotification.FireDate).TotalSeconds;

				// Can fire event
				if (_secondsSinceNow > 0)
				{
					OnReceivingLocalNotification(_scheduledNotification);

					// Handle notification based on its repeat interval
					eNotificationRepeatInterval _repeatInterval	= _scheduledNotification.RepeatInterval;

					switch (_repeatInterval)
					{
					case eNotificationRepeatInterval.NONE:
						// Remove the notification from scheduled list, as its not repeatable
						ScheduledLocalNotifications.RemoveAt(_iter);
						_scheduledNotificationsCount--;
						_iter--;
						break;
						
					case eNotificationRepeatInterval.MINUTE:
						_scheduledNotification.FireDate	= _scheduledNotification.FireDate.AddMinutes(1);
						break;
						
					case eNotificationRepeatInterval.HOUR:
						_scheduledNotification.FireDate	= _scheduledNotification.FireDate.AddHours(1);
						break;
						
					case eNotificationRepeatInterval.DAY:
						_scheduledNotification.FireDate	= _scheduledNotification.FireDate.AddDays(1);
						break;
						
					case eNotificationRepeatInterval.WEEK:
						_scheduledNotification.FireDate	= _scheduledNotification.FireDate.AddDays(7);
						break;
						
					case eNotificationRepeatInterval.MONTH:
						_scheduledNotification.FireDate	= _scheduledNotification.FireDate.AddMonths(1);
						break;
						
					case eNotificationRepeatInterval.YEAR:
						_scheduledNotification.FireDate	= _scheduledNotification.FireDate.AddYears(1);
						break;

					default:
						DebugUtility.Logger.LogError(Constants.kDebugTag, "[RS] Unhandled notification interval=" + _repeatInterval);
						break;
					}
				}
			}
		}

		private void OnReceivingLocalNotification(CrossPlatformNotification _notification)
		{
			// In edit mode, we will cache all triggered notifications
			if (IsEditMode())
			{
				LocalNotifications.Add(_notification);

				// Play sound
				if ((SupportedNotificationTypes & NotificationType.Sound) != 0)
					EditorApplication.Beep();

				// Show alert message
				if ((SupportedNotificationTypes & NotificationType.Alert) != 0)
				{
					bool _viewNotification	= DisplayAlertDialog(_notification);

					if (_viewNotification)
						OnTappingLocalNotification(_notification);
				}
			}
			// In play mode,received notifications are sent to event listener
			else
			{
				SendLocalNotification(_notification, false);
			}
			
			// Redraws inspector
			EditorUtility.SetDirty(this);
		}
		
		public void OnTappingLocalNotification(CrossPlatformNotification _notification)
		{
			// First of all we need to remove that notification
			if (LocalNotifications.Contains(_notification))
				LocalNotifications.Remove(_notification);

			// In edit mode, save notification payload in editor preference
			if (IsEditMode())
			{
				// Serialise notification object and save it in editor preference
				string _notificationJSONStr	= _notification.JSONObject().ToJSON();

				EditorPrefs.SetString(kDidStartWithLocalNotification, _notificationJSONStr);

				// Start playing
				EditorApplication.isPlaying	= true;
			}
			// In play mode,received notifications are sent to event listener
			else
			{
				SendLocalNotification(_notification, false);
			}
		}

		private void SendLocalNotification(CrossPlatformNotification _notification, bool _isLaunchNotification)
		{
			SendNotification(true, _notification, _isLaunchNotification);
		}

		#endregion

		#region Remote Notification Methods

		public void RegisterForRemoteNotifications()
		{
			m_isRegisteredForRemoteNotifications	= true;

			// Notify registration failure on editor
			if (NPBinding.NotificationService != null)
			{      
				NPBinding.NotificationService.InvokeMethod(kDidFailToRegisterRemoteNotificationEvent, "Device token cannot be retrieved on editor!");
			}
		}

		public void UnregisterForRemoteNotifications()
		{
			m_isRegisteredForRemoteNotifications	= false;
		}

		private void ClearRemoteNotifications()
		{
			RemoteNotifications.Clear();
		}
	
		public void ReceivedRemoteNotication(string _notificationPayload)
		{
			if (!m_isRegisteredForRemoteNotifications)
				return;

			CrossPlatformNotification _notification	= CrossPlatformNotification.CreateNotificationFromPayload(_notificationPayload);

			if (_notification == null)
			{
				EditorUtility.DisplayDialog("Push Notification Service", "Failed to push notification. Please enter a valid JSON payload.", "Ok");
				return;
			}

			// In edit mode, all received notifications are cached
			if (IsEditMode())
			{
				RemoteNotifications.Add(_notification);

				// Play sound
				if ((SupportedNotificationTypes & NotificationType.Sound) != 0)
					EditorApplication.Beep();

				// Show alert message
				if ((SupportedNotificationTypes & NotificationType.Alert) != 0)
				{
					bool _viewNotification	= DisplayAlertDialog(_notification);
					
					if (_viewNotification)
						OnTappingRemoteNotification(_notification);
				}
			}
			// In play mode, received notification are sent to event listener
			else
			{
				SendRemoteNotification(_notification, false);
			}
			
			// Redraws inspector
			EditorUtility.SetDirty(this);
		}

		public void OnTappingRemoteNotification(CrossPlatformNotification _notification)
		{
			if (RemoteNotifications.Contains(_notification))
				RemoteNotifications.Remove(_notification);

			if (IsEditMode())
			{
				// Serialise notification object and save it in editor preference
				string _notificationJSONStr	= _notification.JSONObject().ToJSON();
				
				EditorPrefs.SetString(kDidStartWithRemoteNotification, _notificationJSONStr);

				// Start playing
				EditorApplication.isPlaying	= true;
			}
			else
			{
				SendRemoteNotification(_notification, false);
			}
		}

		private void SendRemoteNotification(CrossPlatformNotification _notification, bool _isLaunchNotification)
		{
			SendNotification(false, _notification, _isLaunchNotification);
		}

		#endregion

		#region Misc Methods

		private bool IsEditMode()
		{
			return !(EditorApplication.isPlaying || EditorApplication.isPaused);
		}

		private void SendNotification(bool _isLocalNotification, CrossPlatformNotification _notification, bool _isLaunchNotification)
		{
			// Resume application
			if (EditorApplication.isPaused)
				EditorApplication.isPaused	= false;

			// Send message
			if (NPBinding.NotificationService != null)
			{
				if (_isLocalNotification)
				{
					NPBinding.NotificationService.InvokeMethod(kDidReceiveLocalNotificationEvent, 
					                                           new object[] {
						_notification,
						_isLocalNotification
					}, 
					new System.Type[] {
						typeof(CrossPlatformNotification),
						typeof(bool)
					});
				}
				else
				{
					NPBinding.NotificationService.InvokeMethod(kDidReceiveRemoteNotificationEvent, 
					                                           new object[] {
						_notification,
						_isLocalNotification
					}, 
					new System.Type[] {
						typeof(CrossPlatformNotification),
						typeof(bool)
					});
				}
			}
		}

		private bool DisplayAlertDialog(CrossPlatformNotification _notification)
		{
			string 	_title			= string.Empty;
			string 	_message		= _notification.AlertBody;
			bool 	_canShowAlert	= !string.IsNullOrEmpty(_message);
			string 	_ok				= "View";

#if UNITY_ANDROID
			CrossPlatformNotification.AndroidSpecificProperties _androidProperties	= _notification.AndroidProperties;

			if (_androidProperties != null)
			{
				_title		= _androidProperties.ContentTitle;
			}
#elif UNITY_IOS
			CrossPlatformNotification.iOSSpecificProperties _iosProperties			= _notification.iOSProperties;

			if (_iosProperties != null && _iosProperties.HasAction)
			{
				if (!string.IsNullOrEmpty(_iosProperties.AlertAction))
					_ok		= _iosProperties.AlertAction;
			}
#endif

			if (_canShowAlert)
				return EditorUtility.DisplayDialog(_title, _message, _ok, "Cancel");

			return false;
		}

		private bool IsNetworkAvailable()
		{
			Ping _ping			= new Ping("8.8.8.8");
			float _startTime	= Time.time;
			float  _elapsedTime	= 0f;
			float  _timeOutPeriod = 2f;

			// Ping test
			while (!_ping.isDone && _elapsedTime < _timeOutPeriod)
			{
				_elapsedTime	= Time.time - _startTime;
			}
			
			// Ping request complted within timeout period, so we are connected to network
			if (_ping.isDone && (_ping.time != -1) && _elapsedTime < _timeOutPeriod)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		#endregion

		#region Serialisation

		private void Serialise()
		{
			SetNotificationListInEditorPrefs(kScheduledLocalNotifications, ScheduledLocalNotifications);
			SetNotificationListInEditorPrefs(kLocalNotifications, LocalNotifications);
			SetNotificationListInEditorPrefs(kRemoteNotifications, RemoteNotifications);
		}

		private void Deserialise()
		{
			ScheduledLocalNotifications	= GetNotificationListFromEditorPrefs(kScheduledLocalNotifications);
			LocalNotifications			= GetNotificationListFromEditorPrefs(kLocalNotifications);
			RemoteNotifications			= GetNotificationListFromEditorPrefs(kRemoteNotifications);
		}

		private void SetNotificationListInEditorPrefs(string _key, List<CrossPlatformNotification> _notificationList)
		{
			IList _payloadList	= new List<IDictionary>();
			string _jsonString	= "[]";

			if (_notificationList != null)
			{
 				foreach (CrossPlatformNotification _notification in _notificationList)
				{
					IDictionary _payloadDict	= _notification.JSONObject();

					// Add payload info
					_payloadList.Add(_payloadDict);
				}

				_jsonString	= _payloadList.ToJSON();
			}

			// Add to prefrence
			EditorPrefs.SetString(_key, _jsonString);
		}

		private List<CrossPlatformNotification> GetNotificationListFromEditorPrefs(string _key)
		{
			IList _notificationJSONList							= JSONUtility.FromJSON(EditorPrefs.GetString(_key, "[]")) as IList;
			List<CrossPlatformNotification> _notificationList	= new List<CrossPlatformNotification>(_notificationJSONList.Count);

			foreach (IDictionary _notificationDict in _notificationJSONList)
			{
				CrossPlatformNotification _notification	= new CrossPlatformNotification(_notificationDict);

				// Add notification
				_notificationList.Add(_notification);
			}

			return _notificationList;
		}
	
		#endregion
	}
}
#endif
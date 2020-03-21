#if USES_NOTIFICATION_SERVICE && UNITY_ANDROID
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using VoxelBusters.Utility;
using VoxelBusters.UASUtils;

namespace VoxelBusters.NativePlugins.Internal
{
	public sealed partial class NotificationServiceAndroid : INotificationServicePlatform
	{
		#region Constructors
		
		public NotificationServiceAndroid ()
		{
			Plugin = AndroidPluginUtility.GetSingletonInstance(NativeInfo.Class.NAME);
		}
		
		#endregion

		#region Initialise
		
		public void Initialise (NotificationServiceSettings _settings)
		{
#if USES_ONE_SIGNAL
			OneSignal.EnableVibrate(NPSettings.Notification.Android.AllowVibration);			
#endif
			// Pass sender id list and customkeys to Native platform
			Dictionary<string, string> customKeys = GetCustomKeysForNotfication(_settings);
            SendConfigInfoToNative(new string[]{}, customKeys, _settings.Android.NeedsBigStyle, _settings.Android.WhiteSmallIcon, _settings.Android.AllowVibration);
		}

		public void RegisterNotificationTypes (NotificationType _notificationTypes)
		{
			Plugin.Call(NativeInfo.Methods.SET_NOTIFICATION_TYPES, (int)_notificationTypes);

#if USES_ONE_SIGNAL
			OneSignal.EnableSound(((int)_notificationTypes & (int)NotificationType.Sound) == 1);
#endif
		}
		
		public NotificationType EnabledNotificationTypes ()
		{
			return (NotificationType)Plugin.Call<int>(NativeInfo.Methods.GET_ALLOWED_NOTIFICATION_TYPES);
		}
		
		#endregion

		#region Local Notification API'S

		public void ScheduleLocalNotification (CrossPlatformNotification _notification)
		{
			// Create meta info and pass to native
			IDictionary _payLoadInfo	= AndroidNotificationPayload.CreateNotificationPayload(_notification);
			Plugin.Call(NativeInfo.Methods.SCHEDULE_LOCAL_NOTIFICATION, _payLoadInfo.ToJSON());
		}
		
		public void CancelLocalNotification (string _notificationID)
		{
			Plugin.Call(NativeInfo.Methods.CANCEL_LOCAL_NOTIFICATION, _notificationID);
		}
		
		public void CancelAllLocalNotification ()
		{
			Plugin.Call(NativeInfo.Methods.CANCEL_ALL_LOCAL_NOTIFICATIONS);
		}
		
		#endregion
		
		#region Remote Notification API's

		public void RegisterForRemoteNotifications ()
		{
			Plugin.Call(NativeInfo.Methods.REGISTER_REMOTE_NOTIFICATIONS);
		}

		public void UnregisterForRemoteNotifications ()
		{
			Plugin.Call(NativeInfo.Methods.UNREGISTER_REMOTE_NOTIFICATIONS);
		}

		#endregion

		#region Common Local & Notification Notification API'S
		
		public void ClearNotifications ()
		{
			Plugin.Call(NativeInfo.Methods.CLEAR_ALL_NOTIFICATIONS);
		}
		
		#endregion

		#region Private Methods

		private Dictionary<string, string> GetCustomKeysForNotfication(NotificationServiceSettings _settings)
		{
			Dictionary<string, string> _data =  new Dictionary<string, string>();
			_data = AndroidNotificationPayload.GetNotificationKeyMap();			

			return _data;
			
		}

		private void SendConfigInfoToNative(string[] _senderIDs, Dictionary<string,string> _customKeysInfo, bool _needsBigStyle, Texture2D _whiteSmallNotificationIcon, bool _allowVibration)
		{
			List<string> list =  new List<string>(_senderIDs);	
			bool _usesExternalRemoteNotificationService = NPSettings.Application.SupportedAddonServices.UsesOneSignal;

			//Pass this to android
			Plugin.Call(NativeInfo.Methods.INITIALIZE,list.ToJSON(),_customKeysInfo.ToJSON(), _whiteSmallNotificationIcon == null ? false : true, _allowVibration, _usesExternalRemoteNotificationService);
		}
		
		#endregion
	}
}
#endif
#if USES_NOTIFICATION_SERVICE && UNITY_ANDROID
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using VoxelBusters.Utility;

namespace VoxelBusters.NativePlugins.Internal
{
	public sealed partial class NotificationServiceAndroid : INotificationServicePlatform
	{
		#region Constants
		
		private		const		string		 kIsLaunchNotificationKey		= "is-launch-notification";
		private		const		string		 kPayloadKey					= "notification-payload";
		
		#endregion

		#region Parse Methods

		public void ParseReceivedNotificationEventData (IDictionary _JSONDict, out CrossPlatformNotification _receivedNotification, out bool _isLaunchNotification)
		{
			_receivedNotification	= new AndroidNotificationPayload(_JSONDict);
			_isLaunchNotification	= _JSONDict.GetIfAvailable<bool>(kIsLaunchNotificationKey);
		}

		#endregion
	}
}
#endif
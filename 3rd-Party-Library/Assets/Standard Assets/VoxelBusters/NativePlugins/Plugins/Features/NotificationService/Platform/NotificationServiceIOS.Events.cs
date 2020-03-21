#if USES_NOTIFICATION_SERVICE && UNITY_IOS
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using VoxelBusters.Utility;

namespace VoxelBusters.NativePlugins.Internal
{
	public sealed partial class NotificationServiceIOS : INotificationServicePlatform
	{
		#region Constants

		private		const		string		 kIsLaunchNotificationKey		= "is-launch-notification";
		private		const		string		 kPayloadKey					= "payload";

		#endregion

		#region Parse Methods

		public void ParseReceivedNotificationEventData (IDictionary _JSONDict, out CrossPlatformNotification _receivedNotification, out bool _isLaunchNotification)
		{
			IDictionary		_payloadDict	= _JSONDict.GetIfAvailable<IDictionary>(kPayloadKey);
			_receivedNotification			= new iOSNotificationPayload(_payloadDict);
			_isLaunchNotification			= _JSONDict.GetIfAvailable<bool>(kIsLaunchNotificationKey);

			if (_receivedNotification.UserInfo != null)
			{
#if USES_ONE_SIGNAL
				// Our system should ignore raising events sent from One Signal
				const string 	_kOneSignalIdentifierKeyPath	= "custom/i";
				if (_receivedNotification.UserInfo.ContainsKeyPath(_kOneSignalIdentifierKeyPath))
				{
					_receivedNotification	= null;
					return;
				}
#endif
			}
		}

		#endregion
	}
}
#endif
using UnityEngine;
using System.Collections;

#if USES_NOTIFICATION_SERVICE && USES_ONE_SIGNAL
using System.Collections.Generic;
using VoxelBusters.Utility;

namespace VoxelBusters.NativePlugins.Internal
{
	public sealed class OneSignalNotificationPayload : CrossPlatformNotification 
	{
		#region Constant

		private 	const 	string 		kTitleKey				= "title";
		private 	const 	string 		kActionButtonsKey		= "actionButtons";
		private 	const 	string 		kSoundKey				= "sound";

		#endregion

		#region Constructor

		public OneSignalNotificationPayload (OSNotificationPayload _payload)
		{
			// iOS specific properties
			iOSSpecificProperties	_iOSProperties	= new iOSSpecificProperties();
			_iOSProperties.HasAction				= (_payload.actionButtons!= null) && (_payload.actionButtons.Count > 0);
			
			// Android specific properties
			AndroidSpecificProperties	_androidProperties	= new AndroidSpecificProperties();
			_androidProperties.ContentTitle					= _payload.title;

			// Get user info dictionary by removing used property keys
			IDictionary		_userInfoDict			= new Dictionary<string, object>(_payload.additionalData != null ? _payload.additionalData : new Dictionary<string, object>());
			_userInfoDict.Remove(kTitleKey);

			// Set properties
			this.AlertBody			= _payload.body;
			this.iOSProperties		= _iOSProperties;
			this.AndroidProperties	= _androidProperties;
			this.SoundName			= _payload.sound;
			this.UserInfo			= _userInfoDict;
		}

		#endregion
	}
}
#endif
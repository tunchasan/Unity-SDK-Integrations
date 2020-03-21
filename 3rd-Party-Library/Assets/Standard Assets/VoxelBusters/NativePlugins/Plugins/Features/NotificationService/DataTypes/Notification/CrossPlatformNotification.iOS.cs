using UnityEngine;
using System.Collections;

#if USES_NOTIFICATION_SERVICE 
using System.Collections.Generic;
using VoxelBusters.Utility;

namespace VoxelBusters.NativePlugins
{
	public partial class CrossPlatformNotification 
	{
		/// <summary>
		/// Represents an object that holds notification properties specific to iOS platform.
		/// </summary>
		public class iOSSpecificProperties
		{
			#region Constants
			
			private		const	string		kAlertActionKey		= "alert-action";
			private		const	string 		kHasActionKey		= "has-action";
			private		const	string 		kBadgeCountKey		= "badge-count";
			private		const	string 		kLaunchImageKey		= "launch-image";
			
			#endregion

			#region Properties

			/// <summary>
			/// The title of the action button or slider.
			/// </summary>
			public string AlertAction
			{
				get; 
				set;
			}

			/// <summary>
			/// A bool value that states whether the alert action is visible or not.
			/// </summary>
			public bool HasAction
			{
				get; 
				set;
			}

			/// <summary>
			/// The number to display as the application's icon badge.
			/// </summary>
			public int BadgeCount
			{
				get; 
				set;
			}

			/// <summary>
			/// The image used as the launch image when the user taps the action button.
			/// </summary>
			public string LaunchImage
			{
				get; 
				set;
			}
		
			#endregion

			#region Constructors

			/// <summary>
			/// Initializes a new instance of the <see cref="CrossPlatformNotification.iOSSpecificProperties"/> class.
			/// </summary>
			public iOSSpecificProperties ()
			{
				AlertAction	= null;
				HasAction	= true;
				BadgeCount	= 0;
				LaunchImage	= null;
			}

			internal iOSSpecificProperties (IDictionary _jsonDict)
			{
				AlertAction	= _jsonDict.GetIfAvailable<string>(kAlertActionKey);
				HasAction	= _jsonDict.GetIfAvailable<bool>(kHasActionKey);
				BadgeCount	= _jsonDict.GetIfAvailable<int>(kBadgeCountKey);
				LaunchImage	= _jsonDict.GetIfAvailable<string>(kLaunchImageKey);
			}

			#endregion

			#region Methods

			internal IDictionary JSONObject ()
			{
				Dictionary<string, object> _jsonDict	= new Dictionary<string, object>();
				_jsonDict[kAlertActionKey]				= AlertAction;
				_jsonDict[kHasActionKey]				= HasAction;
				_jsonDict[kBadgeCountKey]				= BadgeCount;
				_jsonDict[kLaunchImageKey]				= LaunchImage;

				return _jsonDict;
			}

			public override string ToString ()
			{
				return string.Format ("[iOSSpecificProperties: AlertAction={0}, HasAction={1}, BadgeCount={2}, LaunchImage={3}]", 
				                      AlertAction, HasAction, BadgeCount, LaunchImage);
			}

			#endregion
		}
	}
}
#endif
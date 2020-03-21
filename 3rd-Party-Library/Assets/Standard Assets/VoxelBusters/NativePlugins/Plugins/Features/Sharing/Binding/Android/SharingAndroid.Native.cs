#if UNITY_ANDROID
using UnityEngine;
using System.Collections;

namespace VoxelBusters.NativePlugins
{
	using Internal;

	public partial class SharingAndroid : Sharing
	{
		#region Platform Native Info
		
		private class Native
		{
			// Handler class name
			internal class Class
			{
				internal const string NAME						= "com.voxelbusters.nativeplugins.features.sharing.SharingHandler";
			}
			
			// For holding method names
			internal class Methods
			{
				internal const string SHARE		 				= "share";
				internal const string SEND_MAIL					= "sendMail";
				internal const string SEND_SMS					= "sendSms";
				internal const string IS_SERVICE_AVAILABLE		= "isServiceAvailable";
				internal const string SHARE_ON_WHATS_APP		= "shareOnWhatsApp";
				internal const string SET_ALLOWED_ORIENTATION	= "setAllowedOrientation";
			}
		}
		
		#endregion
		
		#region  Native Access Variables
		
		private AndroidJavaObject  	Plugin
		{
			get;
			set;
		}
		
		#endregion
	}
}
#endif
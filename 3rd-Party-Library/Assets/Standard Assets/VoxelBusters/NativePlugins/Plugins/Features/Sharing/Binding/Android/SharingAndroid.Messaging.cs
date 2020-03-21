#if UNITY_ANDROID
using UnityEngine;
using System.Collections;
using VoxelBusters.Utility;
using VoxelBusters.UASUtils;

namespace VoxelBusters.NativePlugins
{
	using Internal;

	public partial class SharingAndroid : Sharing 
	{
		#region Overriden API's 
		
		public override bool IsMessagingServiceAvailable ()
		{
			bool _isAvailable	= Plugin.Call<bool>(Native.Methods.IS_SERVICE_AVAILABLE, (int)eShareOptionsAndroid.MESSAGE);

			if (!_isAvailable)
			{
				DebugUtility.Logger.LogWarning(Constants.kDebugTag, "[Sharing:Messaging] IsMessagingServiceAvailable=" + _isAvailable);
			}
			
			return _isAvailable;
		}

		protected override void ShowMessageShareComposer (MessageShareComposer _composer)
		{
			base.ShowMessageShareComposer (_composer);
			
			if (!IsMessagingServiceAvailable())
				return;
			
			// Native method call
			string _recipientJSONList	= _composer.ToRecipients == null ? null : _composer.ToRecipients.ToJSON();

			// Native method is called
			Plugin.Call(Native.Methods.SEND_SMS, _composer.Body, _recipientJSONList);
		}
		
		#endregion

		#region Deprecated Methods

		[System.Obsolete(kSharingFeatureDeprecatedMethodInfo)]
		public override void SendTextMessage (string _body, string[] _recipients, SharingCompletion _onCompletion)
		{
			base.SendTextMessage (_body, _recipients, _onCompletion);

			if (IsMessagingServiceAvailable())
			{
				string _recipientJSONList	= _recipients == null ? null : _recipients.ToJSON();

				// Native method is called
				Plugin.Call(Native.Methods.SEND_SMS, _body, _recipientJSONList);
			}
		}

		#endregion
	}
}
#endif
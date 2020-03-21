#if USES_SHARING && UNITY_IOS
using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using VoxelBusters.Utility;
using VoxelBusters.UASUtils;

namespace VoxelBusters.NativePlugins
{
	using Internal;

	public partial class SharingIOS : Sharing 
	{
		#region Native Methods
		
		[DllImport("__Internal")]
		private static extern bool isMessagingAvailable ();
		
		[DllImport("__Internal")]
		private static extern void sendTextMessage (string _body, string _recipients);
		
		#endregion
		
		#region Overriden API's 
		
		public override bool IsMessagingServiceAvailable ()
		{
			bool _isAvailable	= isMessagingAvailable();
			DebugUtility.Logger.Log(Constants.kDebugTag, "[Sharing:Messaging] Is service available=" + _isAvailable);

			return _isAvailable;
		}

		protected override void ShowMessageShareComposer (MessageShareComposer _composer)
		{
			base.ShowMessageShareComposer (_composer);

			if (!IsMessagingServiceAvailable())
				return;

			// Native method call
			string _recipientJSONList	= _composer.ToRecipients == null ? null : _composer.ToRecipients.ToJSON();

			sendTextMessage(_composer.Body, _recipientJSONList);
		}
		
		#endregion
		
		#region Deprecated Methods

		[System.Obsolete(kSharingFeatureDeprecatedMethodInfo)]
		public override void SendTextMessage (string _body, string[] _recipients, SharingCompletion _onCompletion)
		{
			base.SendTextMessage (_body, _recipients, _onCompletion);
			
			if (IsMessagingServiceAvailable())
			{
				// Send message
				sendTextMessage(_body, _recipients.ToJSON());
			}
		}
		
		#endregion
	}
}
#endif
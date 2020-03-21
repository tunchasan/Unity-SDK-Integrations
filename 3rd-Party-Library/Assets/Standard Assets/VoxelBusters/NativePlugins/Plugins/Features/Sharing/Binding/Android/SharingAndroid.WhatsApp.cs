#if UNITY_ANDROID
using UnityEngine;
using System.Collections;
using VoxelBusters.UASUtils;

namespace VoxelBusters.NativePlugins
{
	using Internal;

	public partial class SharingAndroid : Sharing 
	{
		#region Overriden API's 

		public override bool IsWhatsAppServiceAvailable ()
		{
			bool _canShare	=  Plugin.Call<bool>(Native.Methods.IS_SERVICE_AVAILABLE, (int)eShareOptionsAndroid.WHATSAPP);

			if (!_canShare)
			{
				DebugUtility.Logger.Log(Constants.kDebugTag, "[Sharing:WhatsApp] CanShare=" + _canShare);
			}
			
			return _canShare;
		}

		protected override void ShowWhatsAppShareComposer (WhatsAppShareComposer _composer)
		{
			base.ShowWhatsAppShareComposer (_composer);
			
			if (!IsWhatsAppServiceAvailable())
				return;
			
			byte[]	_imageData			= 	_composer.ImageData;
			int		_imageDataLength	= 	(_imageData == null) ? 0 : _imageData.Length;
			Plugin.Call(Native.Methods.SHARE_ON_WHATS_APP, _composer.Text, _imageData, _imageDataLength);
		}
		
		#endregion
		
		#region Deprecated Methods
		
		[System.Obsolete(kSharingFeatureDeprecatedMethodInfo)]
		public override void ShareTextMessageOnWhatsApp (string _message, SharingCompletion _onCompletion)
		{
			base.ShareTextMessageOnWhatsApp(_message, _onCompletion);
			
			// Failed to share message
			if (string.IsNullOrEmpty(_message) || !IsWhatsAppServiceAvailable())
				return;
			
			Plugin.Call(Native.Methods.SHARE_ON_WHATS_APP, _message, null, 0);
		}
		
		[System.Obsolete(kSharingFeatureDeprecatedMethodInfo)]
		public override void ShareImageOnWhatsApp (byte[] _imageByteArray, SharingCompletion _onCompletion)
		{
			base.ShareImageOnWhatsApp(_imageByteArray, _onCompletion);
			
			// Failed to share image
			if (_imageByteArray == null || !IsWhatsAppServiceAvailable())
				return;
			
			Plugin.Call(Native.Methods.SHARE_ON_WHATS_APP, null, _imageByteArray, _imageByteArray.Length);
		}
		
		#endregion
	}
}
#endif
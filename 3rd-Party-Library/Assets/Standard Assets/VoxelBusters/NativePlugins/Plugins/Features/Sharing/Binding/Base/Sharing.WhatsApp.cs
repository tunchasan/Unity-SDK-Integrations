using UnityEngine;
using System.Collections;
using System.IO;
using VoxelBusters.Utility;
using VoxelBusters.UASUtils;

namespace VoxelBusters.NativePlugins
{
	using Internal;

	public partial class Sharing : MonoBehaviour 
	{
		#region Methods

		/// <summary>
		/// Determines whether the current device is able to share contents on WhatsApp.
		/// </summary>
		/// <returns><c>true</c> if the device can share on WhatsApp; otherwise, <c>false</c>.</returns>
		public virtual bool IsWhatsAppServiceAvailable ()
		{
			bool _isAvailable	= false;
			DebugUtility.Logger.Log(Constants.kDebugTag, "[Sharing] Is service available=" + _isAvailable);

			return _isAvailable;
		}

		protected virtual void ShowWhatsAppShareComposer (WhatsAppShareComposer _composer)
		{
			if (!IsWhatsAppServiceAvailable())
			{
				WhatsAppShareFinished(WhatsAppShareFailedResponse());
				return;
			}
		}

		#endregion

		#region Deprecated Methods

		[System.Obsolete(kSharingFeatureDeprecatedMethodInfo)]
		public virtual void ShareTextMessageOnWhatsApp (string _message, SharingCompletion _onCompletion)
		{
			// Pause unity player
			this.PauseUnity();
			
			// Cache callback
			OnSharingFinished	= _onCompletion;
			
			// Sharing on whatsapp isnt supported
			if (string.IsNullOrEmpty(_message) || !IsWhatsAppServiceAvailable())
			{
				DebugUtility.Logger.LogWarning(Constants.kDebugTag, "[Sharing] Failed to share text");

				WhatsAppShareFinished(WhatsAppShareFailedResponse());
				return;
			}
		}
		
		[System.Obsolete(kSharingFeatureDeprecatedMethodInfo)]
		public void ShareScreenshotOnWhatsApp (SharingCompletion _onCompletion)
		{
			// First capture frame
			StartCoroutine(TextureExtensions.TakeScreenshot((_texture)=>{
				// Convert texture into byte array
				byte[] _imageByteArray	= _texture.EncodeToPNG();
				
				// Share
				ShareImageOnWhatsApp(_imageByteArray, _onCompletion);
			}));
		}
		
		[System.Obsolete(kSharingFeatureDeprecatedMethodInfo)]
		public void ShareImageOnWhatsApp (string _imagePath, SharingCompletion _onCompletion)
		{
			DownloadAsset _request	= new DownloadAsset(new URL(_imagePath), true);
			_request.OnCompletion	= (WWW _www, string _error) => {
				
				if (string.IsNullOrEmpty(_error))
				{
					ShareImageOnWhatsApp(_www.bytes, _onCompletion);
				}
				else
				{
					DebugUtility.Logger.LogError(Constants.kDebugTag, "[Sharing] The operation could not be completed. Error=" + _error);
					ShareImageOnWhatsApp((byte[])null, _onCompletion);
					return;
				}
			};
			_request.StartRequest();
		}
		
		[System.Obsolete(kSharingFeatureDeprecatedMethodInfo)]
		public void ShareImageOnWhatsApp (Texture2D _texture, SharingCompletion _onCompletion)
		{
			if (_texture == null)
			{
				DebugUtility.Logger.LogError(Constants.kDebugTag, "[Sharing] Texture is null");
				ShareImageOnWhatsApp((byte[])null, _onCompletion);
				return;
			}
			
			// Convert texture into byte array
			byte[] _imageByteArray	= _texture.EncodeToPNG();
			
			// Share
			ShareImageOnWhatsApp(_imageByteArray, _onCompletion);
		}

		[System.Obsolete(kSharingFeatureDeprecatedMethodInfo)]
		public virtual void ShareImageOnWhatsApp (byte[] _imageByteArray, SharingCompletion _onCompletion)
		{
			// Pause unity player
			this.PauseUnity();
			
			// Cache callback
			OnSharingFinished	= _onCompletion;
			
			// Sharing on whatsapp isnt supported
			if (_imageByteArray == null || !IsWhatsAppServiceAvailable())
			{
				DebugUtility.Logger.LogError(Constants.kDebugTag, "[Sharing] Failed to share image");
				WhatsAppShareFinished(WhatsAppShareFailedResponse());
				return;
			}		
		}
		
		#endregion
	}
}
#if USES_MEDIA_LIBRARY && UNITY_ANDROID
using UnityEngine;
using System.Collections;
using VoxelBusters.Utility;
using VoxelBusters.UASUtils;

namespace VoxelBusters.NativePlugins
{
	using Internal;

	public partial class MediaLibraryAndroid : MediaLibrary
	{
		#region Constructors
		
		MediaLibraryAndroid()
		{
			Plugin = AndroidPluginUtility.GetSingletonInstance(Native.Class.NAME);
		}

		#endregion

		#region Unity Methods
		
		protected override void Awake()
		{
			base.Awake ();
			Plugin.Call(Native.Methods.INITIALIZE, NPSettings.MediaLibrary.Android.YoutubeAPIKey);		
		}
		
		#endregion

		#region overridden methods

		public override bool IsCameraSupported ()
		{
			bool _isSupported	= Plugin.Call<bool>(Native.Methods.IS_CAMERA_SUPPORTED);
			DebugUtility.Logger.Log(Constants.kDebugTag, "[MediaLibrary] IsCameraSupported=" + _isSupported);
			
			return _isSupported;
		}
		
		public override void SetAllowsImageEditing (bool _value)
		{
			DebugUtility.Logger.LogWarning(Constants.kDebugTag, Constants.kiOSFeature);
		}
		
		public override void PickImage (eImageSource _source, 	float _scaleFactor, 	
		                                PickImageCompletion _onCompletion)
		{
			base.PickImage(_source, _scaleFactor, _onCompletion);
			
			if (_scaleFactor > 0f)
				Plugin.Call(Native.Methods.PICK_IMAGE, (int)_source, _scaleFactor);
		}
		
		public override void SaveImageToGallery (byte[] _imageByteArray, SaveImageToGalleryCompletion _onCompletion)
		{			
			base.SaveImageToGallery(_imageByteArray, _onCompletion);
			
			if (_imageByteArray != null)
				Plugin.Call(Native.Methods.SAVE_IMAGE_TO_GALLERY, _imageByteArray, _imageByteArray.Length, NPSettings.MediaLibrary.Android.SaveGalleryImagesToAppSpecificFolder);
		}

		public override void PlayVideoFromGallery (PickVideoCompletion _onPickVideoCompletion,
		                                          PlayVideoCompletion _onPlayVideoCompletion)
		{
			base.PlayVideoFromGallery(_onPickVideoCompletion, _onPlayVideoCompletion);

			Plugin.Call(Native.Methods.PLAY_VIDEO_FROM_GALLERY);
		}

		public override void PlayVideoFromURL (URL _URL, 
		                                      PlayVideoCompletion _onCompletion)
		{
			base.PlayVideoFromURL(_URL, _onCompletion);
			
			if (!string.IsNullOrEmpty(_URL.URLString))
			{
				string _youtubeID = ExtractYoutubeVideoID(_URL.URLString);
				
				if(_youtubeID != null)
				{
					PlayYoutubeVideo(_youtubeID, _onCompletion);
				}
				else
				{
					Plugin.Call(Native.Methods.PLAY_VIDEO_FROM_URL, _URL.URLString);
				}
			}
		}

		public override void PlayYoutubeVideo (string _videoID, PlayVideoCompletion _onCompletion)
		{
			base.PlayYoutubeVideo(_videoID, _onCompletion);

			if(!string.IsNullOrEmpty(_videoID))
			{
				if (string.IsNullOrEmpty(NPSettings.MediaLibrary.Android.YoutubeAPIKey))
				{
					// Get Embed String
					string _embedHTMLString = GetYoutubeEmbedHTMLString(_videoID);

					// Play video
					PlayEmbeddedVideo(_embedHTMLString, _onCompletion);
				}
				else
				{
					Plugin.Call(Native.Methods.PLAY_VIDEO_FROM_YOUTUBE, _videoID);
				}
			}
		}	

		public override void PlayEmbeddedVideo (string _embedHTMLString, PlayVideoCompletion _onCompletion)
		{
			base.PlayEmbeddedVideo(_embedHTMLString, _onCompletion);
			
			if (!string.IsNullOrEmpty(_embedHTMLString))
				Plugin.Call(Native.Methods.PLAY_VIDEO_FROM_WEBVIEW, _embedHTMLString);
		}

		#endregion
	}
}
#endif	
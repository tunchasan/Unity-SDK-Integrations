using UnityEngine;

#if USES_MEDIA_LIBRARY && UNITY_ANDROID
namespace VoxelBusters.NativePlugins
{
	public partial class MediaLibraryAndroid : MediaLibrary
	{
		#region Platform Native Info
		
		private class Native
		{
			// Handler class name
			internal class Class
			{
				internal const string NAME						= "com.voxelbusters.nativeplugins.features.medialibrary.MediaLibraryHandler";
			}
			
			// For holding method names
			internal class Methods
			{
				// Init
				internal const string INITIALIZE		 			= "initialize";

				// Image Actions
				internal const string IS_CAMERA_SUPPORTED		 	= "isCameraSupported";
				internal const string PICK_IMAGE				 	= "showImagePicker";
				internal const string SAVE_IMAGE_TO_GALLERY	 	= "saveImageToAlbum";

				// Video Actions
				internal const string PLAY_VIDEO_FROM_GALLERY		= "playVideoFromGallery";
				internal const string PLAY_VIDEO_FROM_URL		 	= "playVideoFromURL";
				internal const string PLAY_VIDEO_FROM_YOUTUBE		= "playVideoFromYoutube";
				internal const string PLAY_VIDEO_FROM_WEBVIEW		= "playVideoFromWebView";
				
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
#if USES_MEDIA_LIBRARY
using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;
using VoxelBusters.Utility;
using VoxelBusters.UASUtils;

using DownloadTexture = VoxelBusters.Utility.DownloadTexture;

namespace VoxelBusters.NativePlugins
{
	using Internal;

	/// <summary>
	/// Provides cross-platform interface to access devices's media gallery and camera for picking images and playing videos.
	/// </summary>
	public partial class MediaLibrary : MonoBehaviour 
	{
		#region Properties

		private		float		m_scaleFactor;

		#endregion

		#region Unity Methods
		
		protected virtual void Awake()
		{}
		
		#endregion

		#region Pick Image

		/// <summary>
		/// Determines whether the device supports picking image using camera.
		/// </summary>
		/// <returns><c>true</c> if the device supports picking image using camera; otherwise, <c>false</c>.</returns>
		public virtual bool IsCameraSupported ()
		{
			bool _isSupported	= false;
			DebugUtility.Logger.Log(Constants.kDebugTag, "[MediaLibrary] IsCameraSupported=" + _isSupported);

			return _isSupported;
		}

		/// <summary>
		/// Sets the value indicating whether the user is allowed to edit a selected image.
		/// </summary>
		/// <param name="_value">The value to set.</param>
		/// <remarks>
		/// This property is set to <c>true</c> by default.
		/// </remarks>
		public virtual void SetAllowsImageEditing (bool _value)
		{}
		
		/// <summary>
		/// Opens an user interface to pick an image from specified image source.
		/// </summary>
		/// <param name="_source">The source to use to pick an image.</param>
		/// <param name="_scaleFactor">The factor used to rescale selected image. Having value as 1.0f returns the image without any modification.</param>
		/// <param name="_onCompletion">Callback that will be called after operation is completed.</param>
		public virtual void PickImage (eImageSource _source, float _scaleFactor, PickImageCompletion _onCompletion)
		{
			// Pause unity player
			this.PauseUnity();

			// Cache properties
			m_scaleFactor			= _scaleFactor;
			OnPickImageFinished		= _onCompletion;

			if (_scaleFactor <= 0f)
			{
				PickImageFinished(null, ePickImageFinishReason.FAILED);
				return;
			}
		}

		#endregion

		#region Album

		/// <summary>
		/// Captures a screenshot and saves it to device's media gallery.
		/// </summary>
		/// <param name="_onCompletion">Callback that will be called after operation is completed.</param>
		public void SaveScreenshotToGallery (SaveImageToGalleryCompletion _onCompletion)
		{
			// First capture screenshot
			StartCoroutine(TextureExtensions.TakeScreenshot((_texture)=>{

				// Now save it
				SaveImageToGallery(_texture, _onCompletion);
			}));
		}
		
		/// <summary>
		/// Saves specifed file to device's media gallery.
		/// </summary>
		/// <param name="_URL">The URL points to the file saved in local/remote path.</param>
		/// <param name="_onCompletion">Callback that will be called after operation is completed.</param>
		/// <remarks>
		/// \note Use absolute path if specified URL points to a local file. And needless to say, assign proper file path as the file structure will differ with platform.
		/// </remarks>
		public void SaveImageToGallery (URL _URL, SaveImageToGalleryCompletion _onCompletion)
		{
			// Download texture from given URL
			DownloadTexture _newDownload	= new DownloadTexture(_URL, true, false);
			_newDownload.OnCompletion		= (Texture2D _texture, string _error)=>{

				// Save downloaded texture
				if (!string.IsNullOrEmpty(_error))
				{
					DebugUtility.Logger.LogError(Constants.kDebugTag, "[MediaLibrary] Texture download failed, URL=" + _URL.URLString);
				}

				// Save image
				SaveImageToGallery(_texture, _onCompletion);
			};

			// Start download
			_newDownload.StartRequest();
		}

		/// <summary>
		/// Saves specifed texture to device's media gallery.
		/// </summary>
		/// <param name="_texture">Unity texture object to be saved.</param>
		/// <param name="_onCompletion">Callback that will be called after operation is completed.</param>
		public void SaveImageToGallery (Texture2D _texture, SaveImageToGalleryCompletion _onCompletion)
		{
			byte[] _imageByteArray	= null;

			// Convert texture to byte array
			if (_texture != null)
			{
				_imageByteArray	= _texture.EncodeToPNG();
			}

			// Use api to save
			SaveImageToGallery(_imageByteArray, _onCompletion);
		}
		
		/// <summary>
		/// Saves specifed image data to device's media gallery.
		/// </summary>
		/// <param name="_imageByteArray">Raw form of image to be saved.</param>
		/// <param name="_onCompletion">Callback that will be called after operation is completed.</param>
		public virtual void SaveImageToGallery (byte[] _imageByteArray, SaveImageToGalleryCompletion _onCompletion)
		{
			// Cache callback
			OnSaveImageToGalleryFinished	= _onCompletion;

			if (_imageByteArray == null)
			{
				DebugUtility.Logger.LogError(Constants.kDebugTag, "[MediaLibrary] Saving image to album failed, texture data is null");
				SaveImageToGalleryFinished(false);
				return;
			}
		}

		#endregion

		#region Video

		/// <summary>
		/// Plays a full-screen Youtube video.
		/// </summary>
		/// <param name="_videoID">A string used to identify Youtube video.</param>
		/// <param name="_onCompletion">Callback that will be called after operation is completed.</param>
		public virtual void PlayYoutubeVideo (string _videoID, PlayVideoCompletion _onCompletion)
		{	
			// Pause unity player
			this.PauseUnity();
		
			// Cache callback
			OnPlayVideoFinished	= _onCompletion;

			if (string.IsNullOrEmpty(_videoID))
			{
				DebugUtility.Logger.LogError(Constants.kDebugTag, "[MediaLibrary] Play youtube video failed, Video ID can't be null");
				PlayVideoFinished(ePlayVideoFinishReason.PLAYBACK_ERROR);
				return;
			}
		}

		/// <summary>
		/// Plays a full-screen video using native WebView.
		/// </summary>
		/// <param name="_embedHTMLString">Embedded HTML string that is loaded into WebView for playing video.</param>
		/// <param name="_onCompletion">Callback that will be called after operation is completed.</param>
		public virtual void PlayEmbeddedVideo (string _embedHTMLString, PlayVideoCompletion _onCompletion)
		{
			// Pause unity player
			this.PauseUnity();
			
			// Cache callback
			OnPlayVideoFinished	= _onCompletion;

			if (string.IsNullOrEmpty(_embedHTMLString))
			{
				DebugUtility.Logger.LogError(Constants.kDebugTag, "[MediaLibrary] Play video using webview failed, HTML string cant be null");
				PlayVideoFinished(ePlayVideoFinishReason.PLAYBACK_ERROR);
				return;
			}
		}

		/// <summary>
		/// Plays a full-screen video from specified URL. This URL can point to local/remote file. 
		/// </summary>
		/// <param name="_URL">URL of the video to play.</param>
		/// <param name="_onCompletion">Callback that will be called after operation is completed.</param>
		///	<remarks>
		/// \note Given URL should point directly to the video. Eg: http://www.voxelbusters.com/movie.mp4 
		/// </remarks>
		public virtual void PlayVideoFromURL (URL _URL, PlayVideoCompletion _onCompletion)
		{
			// Pause unity player
			this.PauseUnity();

			// Cache callback
			OnPlayVideoFinished	= _onCompletion;

			if (string.IsNullOrEmpty(_URL.URLString))
			{
				DebugUtility.Logger.LogError(Constants.kDebugTag, "[MediaLibrary] Play video from URL failed, URL can't be null");
				PlayVideoFinished(ePlayVideoFinishReason.PLAYBACK_ERROR);
				return;
			}
		}

		/// <summary>
		/// Plays a full-screen video selected from device's video gallery.
		/// </summary>
		/// <param name="_onPickVideoCompletion">Callback that will be called after video is picked from gallery.</param>
		/// <param name="_onPlayVideoCompletion">Callback that will be called after video playback ended.</param>
		public virtual void PlayVideoFromGallery (PickVideoCompletion _onPickVideoCompletion, PlayVideoCompletion _onPlayVideoCompletion)
		{
			// Pause unity player
			this.PauseUnity();

			// Cache callback
			OnPickVideoFinished	= _onPickVideoCompletion;
			OnPlayVideoFinished	= _onPlayVideoCompletion;
		}

		#endregion

		#region Helpers

		protected string ExtractYoutubeVideoID (string _url)
		{
			string _youtubeID = null;

			//Regex for youtube from - http://fiddle.re/w1nn6
			Match regexMatch = Regex.Match(_url, "^(?:https?\\:\\/\\/)?(?:www\\.)?(?:youtu\\.be\\/|youtube\\.com\\/(?:embed\\/|v\\/|watch\\?v\\=))([\\w-]{10,12})(?:[\\&\\?\\#].*?)*?(?:[\\&\\?\\#]t=([\\dhm]+s))?$", 
			                               RegexOptions.IgnoreCase);
			if (regexMatch.Success)
			{
				foreach(Group each in regexMatch.Groups)
				{
					DebugUtility.Logger.Log(Constants.kDebugTag, "Value "+each.Value);
				}

				if(regexMatch.Groups.Count > 1)
				{
					_youtubeID = regexMatch.Groups[1].Value;
				}
			}
			return _youtubeID;
		}

		protected string GetYoutubeEmbedHTMLString (string _videoID)
		{
			// Load Youtube player HTML script
			TextAsset _youtubePlayerHTML	= Resources.Load("YoutubePlayer", typeof(TextAsset)) as TextAsset;
			string _embedHTMLString			= null;
			
			if (_youtubePlayerHTML != null)
			{
				_embedHTMLString			= _youtubePlayerHTML.text.Replace("%@", _videoID);
			}

			return _embedHTMLString;
		}

		#endregion
	}
}
#endif
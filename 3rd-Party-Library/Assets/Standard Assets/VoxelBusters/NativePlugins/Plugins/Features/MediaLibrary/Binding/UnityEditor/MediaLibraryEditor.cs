#if USES_MEDIA_LIBRARY && UNITY_EDITOR
using UnityEngine;
using System.Collections;
using UnityEditor;
using VoxelBusters.Utility;
using VoxelBusters.UASUtils;

namespace VoxelBusters.NativePlugins
{
	using Internal;

	public partial class MediaLibraryEditor : MediaLibrary
	{
		#region Properties
	
		private EditorMediaGallery 		m_gallery;

		#endregion

		#region Unity Methods
		
		protected override void Awake()
		{
			base.Awake();

			// Load gallery
			LoadGallery();
		}
		
		#endregion

		#region Image Methods

		public override bool IsCameraSupported ()
		{
			DebugUtility.Logger.LogError(Constants.kDebugTag, Constants.kNotSupportedInEditor);
			return base.IsCameraSupported();
		}

		public override void SetAllowsImageEditing (bool _value)
		{
			DebugUtility.Logger.LogWarning(Constants.kDebugTag, Constants.kiOSFeature);
		}
		
		public override void PickImage (eImageSource _source, float _scaleFactor, PickImageCompletion _onCompletion)
		{
			base.PickImage(_source, _scaleFactor, _onCompletion);

			if (_scaleFactor > 0f)
			{
				m_gallery.PickImage(_source);
			}
		}
		
		public override void SaveImageToGallery (byte[] _imageByteArray, SaveImageToGalleryCompletion _onCompletion)
		{
			base.SaveImageToGallery(_imageByteArray, _onCompletion);

			if (_imageByteArray != null)
			{
				// Feature isnt supported
				DebugUtility.Logger.LogError(Constants.kDebugTag, Constants.kNotSupportedInEditor);
				
				// Associated error event is raised
				SaveImageToGalleryFinished(false);
			}
		}

		#endregion

		#region Video Methods
		
		public override void PlayYoutubeVideo (string _videoID, PlayVideoCompletion _onCompletion)
		{
			base.PlayYoutubeVideo(_videoID, _onCompletion);

			if (!string.IsNullOrEmpty(_videoID))
			{
				Application.OpenURL("http://www.youtube.com/watch?v=" + _videoID);
				PlayVideoFinished(ePlayVideoFinishReason.PLAYBACK_ENDED);
			}
		}	

		public override void PlayEmbeddedVideo (string _embedHTMLString, PlayVideoCompletion _onCompletion)
		{
			base.PlayEmbeddedVideo(_embedHTMLString, _onCompletion);
			
			if (!string.IsNullOrEmpty(_embedHTMLString))
			{
				string _tempPath = FileUtil.GetUniqueTempPathInProject() + ".html";

				DebugUtility.Logger.LogError(Constants.kDebugTag, _tempPath);
				System.IO.StreamWriter _stream = FileOperations.CreateText(_tempPath);
				_stream.Write(_embedHTMLString);
				_stream.Close();
				
				// Open URL
				Application.OpenURL("file://" + Application.dataPath + "/../" + _tempPath);

				// Associated error event is raised
				PlayVideoFinished(ePlayVideoFinishReason.PLAYBACK_ENDED);
			}
		}
		
		public override void PlayVideoFromURL (URL _URL, PlayVideoCompletion _onCompletion)
		{
			base.PlayVideoFromURL(_URL, _onCompletion);
			
			if (!string.IsNullOrEmpty(_URL.URLString))
			{
				Application.OpenURL(_URL.URLString);
				PlayVideoFinished(ePlayVideoFinishReason.PLAYBACK_ENDED);
			}
		}
		
		public override void PlayVideoFromGallery (PickVideoCompletion _onPickVideoCompletion, PlayVideoCompletion _onPlayVideoCompletion)
		{
			base.PlayVideoFromGallery(_onPickVideoCompletion, _onPlayVideoCompletion);

			m_gallery.PickVideoFromGallery();
			
			// Feature isnt supported
			DebugUtility.Logger.LogWarning(Constants.kDebugTag, "Video Playback from gallery not supported on Editor");

			// Associated event is raised.
			PlayVideoFinished(ePlayVideoFinishReason.PLAYBACK_ERROR);
		}
		
		#endregion

		#region Helpers

		private void LoadGallery ()
		{
			GameObject _gameObject 	= new GameObject("EditorGallery");
			_gameObject.hideFlags 	= HideFlags.HideInHierarchy;
			m_gallery = _gameObject.AddComponent<EditorMediaGallery>();
		}

		#endregion
	}
}
#endif	
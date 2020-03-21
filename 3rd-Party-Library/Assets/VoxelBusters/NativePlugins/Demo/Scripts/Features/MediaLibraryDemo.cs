using UnityEngine;
using System.Collections;
using VoxelBusters.Utility;
using VoxelBusters.Utility.UnityGUI.MENU;
using VoxelBusters.NativePlugins;

namespace VoxelBusters.NativePlugins.Demo
{
#if !USES_MEDIA_LIBRARY
	public class MediaLibraryDemo : NPDisabledFeatureDemo
#else
	public class MediaLibraryDemo : NPDemoBase
#endif
	{
		#region Properties

#pragma warning disable
		[SerializeField, Header("Video Section"), Tooltip ("This needs to be direct link to the video file. ex: http://www.google.com/video.mp4")]
		private 	string		m_videoURL;
		[SerializeField]
		private 	string		m_youtubeVideoID;
		[SerializeField]
		private		TextAsset	m_vimeoPlayerHTML;
		[SerializeField]
		private 	string		m_vimeoVideoID;

		private		bool		m_canEditImages			= true;

		private Texture m_texture;
#pragma warning restore

		#endregion

#if !USES_MEDIA_LIBRARY
	}
#else
		#region Unity Methods

		protected override void Start ()
		{
			base.Start ();

			// Set additional info texts
			AddExtraInfoTexts(
				"You can configure this feature in NPSettings->Media Library Settings.");
		}

		#endregion

		#region GUI Methods

		protected override void DisplayFeatureFunctionalities ()
		{
			base.DisplayFeatureFunctionalities ();

			DrawImageAPI ();
			DrawVideoAPI ();

			if(m_texture != null)
				GUILayout.Box (m_texture);
		}

		private void DrawImageAPI ()
		{
			GUILayout.Label("Image", kSubTitleStyle);

			if (GUILayout.Button("Is Camera Supported"))
			{
				bool _isSupported = IsCameraSupported();

				AddNewResult(_isSupported ? "Camera access is supported." : "Sorry, camera access is not supported.");
			}

			bool _canEditImagesNewValue = GUILayout.Toggle(m_canEditImages, "Allows Image Editing");

			if (_canEditImagesNewValue != m_canEditImages)
			{
				m_canEditImages	= _canEditImagesNewValue;

				SetAllowsImageEditing();
			}

			GUILayout.BeginHorizontal();
			{
				if (GUILayout.Button("Pick Image From ALBUM"))
				{
					PickImageFromAlbum();
				}

				if (GUILayout.Button("Pick Image From CAMERA"))
				{
					PickImageFromCamera();
				}
			}
			GUILayout.EndHorizontal();

			if (GUILayout.Button("Pick Image From BOTH - ALBUM & CAMERA"))
			{
				PickImageFromBoth();
			}

			if (GUILayout.Button("Save Screenshot To Album"))
			{
				SaveScreenshotToGallery();
			}
		}

		private void DrawVideoAPI ()
		{
			GUILayout.Label("Video", kSubTitleStyle);

			if (GUILayout.Button("Play Youtube Video"))
			{
				PlayYoutubeVideo();
			}

			if (GUILayout.Button("Play Video From URL"))
			{
				PlayVideoFromURL();
			}

			if (GUILayout.Button("Play Video From Gallery"))
			{
				PlayVideoFromGallery();
			}

			if (GUILayout.Button("Play Embedded Video"))
			{
				PlayEmbeddedVideo();
			}
		}

		#endregion

		#region Image API Methods

		private bool IsCameraSupported ()
		{
			return NPBinding.MediaLibrary.IsCameraSupported();
		}

		private void SetAllowsImageEditing ()
		{
			NPBinding.MediaLibrary.SetAllowsImageEditing(m_canEditImages);
		}

		private void PickImageFromAlbum ()
		{
			// Set popover to last touch position
			NPBinding.UI.SetPopoverPointAtLastTouchPosition();

			// Pick image
			NPBinding.MediaLibrary.PickImage(eImageSource.ALBUM, 1.0f, PickImageFinished);
		}

		private void PickImageFromCamera ()
		{
			// Set popover to last touch position
			NPBinding.UI.SetPopoverPointAtLastTouchPosition();

			// Pick image
			NPBinding.MediaLibrary.PickImage(eImageSource.CAMERA, 1.0f, PickImageFinished);
		}

		private void PickImageFromBoth ()
		{
			// Set popover to last touch position
			NPBinding.UI.SetPopoverPointAtLastTouchPosition();

			// Pick image
			NPBinding.MediaLibrary.PickImage(eImageSource.BOTH, 1.0f, PickImageFinished);
		}

		private void SaveScreenshotToGallery ()
		{
			NPBinding.MediaLibrary.SaveScreenshotToGallery(SaveImageToGalleryFinished);
		}

		#endregion

		#region Video API Methods

		private void PlayYoutubeVideo ()
		{
			NPBinding.MediaLibrary.PlayYoutubeVideo(m_youtubeVideoID, PlayVideoFinished);
		}

		private void PlayVideoFromURL ()
		{
			NPBinding.MediaLibrary.PlayVideoFromURL(URL.URLWithString(m_videoURL), PlayVideoFinished);
		}

		private void PlayVideoFromGallery ()
		{
			// Set popover to last touch position
			NPBinding.UI.SetPopoverPointAtLastTouchPosition();

			// Play video from gallery
			NPBinding.MediaLibrary.PlayVideoFromGallery(PickVideoFinished, PlayVideoFinished);
		}

		private void PlayEmbeddedVideo ()
		{
			string _embeddedVideoHTML	= m_vimeoPlayerHTML.text.Replace("$video_id", m_vimeoVideoID);
			NPBinding.MediaLibrary.PlayEmbeddedVideo(_embeddedVideoHTML, PlayVideoFinished);
		}

		#endregion

		#region API Callback Methods

		private void PickImageFinished (ePickImageFinishReason _reason, Texture2D _image)
		{
			AddNewResult("Request to pick image from gallery finished. Reason for finish is " + _reason + ".");
			AppendResult(string.Format("Selected image is {0}.", (_image == null ? "NULL" : _image.ToString())));
			m_texture = _image;
		}

		private void SaveImageToGalleryFinished (bool _saved)
		{
			AddNewResult(_saved ? "Image saved successfully to gallery." : "Sorry, something went wrong. Couldn't save image to gallery.");
		}

		private void PickVideoFinished (ePickVideoFinishReason _reason)
		{
			AddNewResult("Request to pick video from gallery finished. Reason for finish is " + _reason + ".");
		}

		private void PlayVideoFinished (ePlayVideoFinishReason _reason)
		{
			AddNewResult("Request to play video finished. Reason for finish is " + _reason + ".");
		}

		#endregion
	}
#endif
}

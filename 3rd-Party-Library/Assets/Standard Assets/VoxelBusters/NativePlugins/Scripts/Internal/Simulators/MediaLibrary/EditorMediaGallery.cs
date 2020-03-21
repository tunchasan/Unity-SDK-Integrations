#if USES_MEDIA_LIBRARY && UNITY_EDITOR
using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using VoxelBusters.Utility;

namespace VoxelBusters.NativePlugins.Internal
{
	public class EditorMediaGallery : MonoBehaviour
	{
		private enum eMediaLibraryType
		{
			NONE,
			ALBUM,
			CAMERA,
			ALBUM_AND_CAMERA,
			VIDEOS
		}

		#region Properties
		
		private GUISkin 			m_guiSkin;

		private eMediaLibraryType 	m_viewType;

		private Texture[] 			m_galleryImages 			= null;
		
		private GUIScrollView 		m_scrollView;

		private Rect 				m_windowRect;
		
		#endregion

		#region Constants

		// Event callbacks
		private const string		kPickImageFinishedEvent		= "PickImageFinished";
		private const string		kPickVideoFinishedEvent		= "PickVideoFinished";

		// Data keys
		private const string 		kImagePath					= "image-path";
		private const string		kFinishReason				= "finish-reason";

		#endregion

		#region Unity Methods
	
		void Start()
		{
			// Initialise
			m_scrollView 	= gameObject.AddComponent<GUIScrollView>();
			m_windowRect 	= new Rect(0f, 0f, Screen.width, Screen.height);

			// Load gallery images
			LoadGalleryImages();
		}

		#endregion

		#region API Implementation Methods

		public void PickImage(eImageSource _source)
		{
			if(_source == eImageSource.ALBUM)
			{
				m_viewType = eMediaLibraryType.ALBUM;
				m_scrollView.Reset();
			}
			else if(_source == eImageSource.CAMERA)
			{
				m_viewType = eMediaLibraryType.CAMERA;
			}
			else if(_source == eImageSource.BOTH)
			{
				m_viewType = eMediaLibraryType.ALBUM_AND_CAMERA;
			}
		}

		public void PickVideoFromGallery()
		{
			m_viewType = eMediaLibraryType.VIDEOS;
			m_scrollView.Reset();
		}

		#endregion

		#region Callbacks To Native Plugins
		
		private void PickImageFinished (string _path)
		{
			Dictionary<string, object> _packedData	= new Dictionary<string, object>();
			_packedData[kImagePath]					= _path;
			_packedData[kFinishReason]				= (int)ePickImageFinishReason.SELECTED;

			if (NPBinding.MediaLibrary != null)
				NPBinding.MediaLibrary.InvokeMethod(kPickImageFinishedEvent, _packedData.ToJSON());

			// Reset view
			ResetView();
		}
		
		private void PickImageCancelled()
		{
			Dictionary<string, int> _packedData		= new Dictionary<string, int>();
			_packedData[kFinishReason]				= (int)ePickImageFinishReason.CANCELLED;

			if (NPBinding.MediaLibrary != null)
				NPBinding.MediaLibrary.InvokeMethod(kPickImageFinishedEvent, _packedData.ToJSON());

			// Reset view
			ResetView();
		}
		
		private void PickVideoFinished()
		{
			if (NPBinding.MediaLibrary != null)
				NPBinding.MediaLibrary.InvokeMethod(kPickVideoFinishedEvent, ePickVideoFinishReason.SELECTED);

			// Reset view
			ResetView();
		}
		
		private void PickVideoCancelled()
		{
			if (NPBinding.MediaLibrary != null)
				NPBinding.MediaLibrary.InvokeMethod(kPickVideoFinishedEvent, ePickVideoFinishReason.CANCELLED);
			
			// Reset view
			ResetView();
		}
		
		#endregion

		#region UI

		private void OnGUI()
		{
			if (m_viewType == eMediaLibraryType.NONE)
			{
				return;
			}

			//Save previous skin to restore at end
			GUISkin _oldSkin 	= GUI.skin;
			GUI.skin 			= GetGUISkin();

			//Updating if any change in window size
			m_windowRect.width	= Screen.width;
			m_windowRect.height	= Screen.height;

			m_windowRect = GUI.ModalWindow(this.GetInstanceID(), m_windowRect, OnWindow, "");

			// Restore with previous skin
			GUI.skin = _oldSkin;
		}

		private void OnWindow(int _windowID)
		{
			if(m_viewType == eMediaLibraryType.ALBUM)
			{
				ShowGalleryWithImages();
			}
			else if(m_viewType == eMediaLibraryType.CAMERA)
			{
				ShowCameraSelection();
			}
			else if(m_viewType == eMediaLibraryType.ALBUM_AND_CAMERA)
			{
				ShowGalleryAndCameraSelection();
			}
			else if(m_viewType == eMediaLibraryType.VIDEOS)
			{
				ShowGalleryWithVideos();
			}
		}

		private void ShowGalleryWithImages()
		{
			GUILayout.BeginVertical(GetGUISkin().scrollView);

			GUILayout.Box("Pick From Gallery");

			GUILayout.FlexibleSpace();

			m_scrollView.BeginScrollView();

			foreach(Texture _each in m_galleryImages)
			{
				if(GUILayout.Button(_each))
				{
					PickImageFinished(GetAssetPath(_each));
				}
			}	

			m_scrollView.EndScrollView();

			GUILayout.FlexibleSpace();
	
			if(GUILayout.Button("Cancel"))
			{
				PickImageCancelled();
			}
	
			GUILayout.EndVertical();
		}

		private void ShowCameraSelection()
		{
			GUILayout.BeginVertical(GetGUISkin().scrollView);
			{
				GUILayout.Box("Pick From Camera");
	
				GUILayout.FlexibleSpace();
				
				GUILayout.Label("Camera not accessible on Editor. This will send a default image from gallery pictures to simulate");

				GUILayout.FlexibleSpace();

				if(GUILayout.Button("Select"))
				{
					if(m_galleryImages.Length > 0)
					{
						PickImageFinished(GetAssetPath(m_galleryImages[0]));
					}
					else
					{
						PickImageFinished(null);
					}
				}
				else if(GUILayout.Button("Cancel"))
				{
					PickImageCancelled();
				}
			}

			GUILayout.EndVertical();
		}

		private void ShowGalleryAndCameraSelection()
		{
			GUILayout.BeginVertical(GetGUISkin().scrollView);
			{
				GUILayout.FlexibleSpace();
				
				GUILayout.Box("Choose From");

				if(GUILayout.Button("Library"))
				{
					m_viewType = eMediaLibraryType.ALBUM;
				}
				else if(GUILayout.Button("Camera"))
				{
					m_viewType = eMediaLibraryType.CAMERA;
				}
			
				GUILayout.FlexibleSpace();
			}
			GUILayout.EndVertical();
		}

		private void ShowGalleryWithVideos()
		{
			GUILayout.BeginVertical(GetGUISkin().scrollView);
			{
				GUILayout.FlexibleSpace();
				
				GUILayout.Box("Pick Video");
	
				m_scrollView.BeginScrollView();
				{
					for(int _i = 0 ; _i < 10 ; _i++)
					{
						if(GUILayout.Button("Video " + _i))
						{
							PickVideoFinished();
						}
					}
				}
				m_scrollView.EndScrollView();

				GUILayout.FlexibleSpace();

				if(GUILayout.Button("Cancel"))
				{
					PickVideoCancelled();
				}
			}
			GUILayout.EndVertical();
		}

		#endregion

		#region Helpers
		
		private void LoadGalleryImages()
		{
			//Load available textures - this loading will be ONLY in editor.
			Texture[] _galleryImages = Resources.LoadAll<Texture>("");

			int _maxTexturesToLoad = 100;
			
			if(_galleryImages.Length > _maxTexturesToLoad)
			{
				m_galleryImages = new Texture[_maxTexturesToLoad];
				System.Array.Copy(_galleryImages, 0, m_galleryImages, 0, _maxTexturesToLoad);
			}
			else
			{
				m_galleryImages = _galleryImages;
			}
		}

		private string GetAssetPath(Object _asset)
		{
			return 	Application.dataPath + "/../" + AssetDatabase.GetAssetPath(_asset);
		}

		private void ResetView()
		{
			m_viewType = eMediaLibraryType.NONE;
		}

		#endregion

		#region Misc
		
		private GUISkin GetGUISkin()
		{
			if(m_guiSkin == null)
			{
				m_guiSkin = Resources.Load(Constants.kSampleUISkin) as GUISkin;
			}
			
			return m_guiSkin;
		}	
		
		#endregion
	}
}
#endif
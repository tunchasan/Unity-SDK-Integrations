using UnityEngine;
using System.Collections;
using VoxelBusters.Utility;

namespace VoxelBusters.NativePlugins
{
    using Internal;
    public partial class MediaLibrarySettings
    {
		[System.Serializable]
		public class AndroidSettings
		{
			#region Fields

			[SerializeField]
      [NotifyNPSettingsOnValueChange]
      [Tooltip("Youtube API key assigned to your application.")]
			private 	string 		m_youtubeAPIKey  = null;

			[SerializeField]
			[Tooltip("If you set this to false, the images will be saved to default gallery. Else to app specific album.")]
			private 	bool 		m_saveToGallerySavesToAppFolder = true;

			#endregion

			#region Properties

			internal string YoutubeAPIKey
			{
				get
				{
					return m_youtubeAPIKey;
				}
			}

			internal bool SaveGalleryImagesToAppSpecificFolder
			{
				get
				{
					return m_saveToGallerySavesToAppFolder;
				}
			}

			#endregion
		}
	}
}

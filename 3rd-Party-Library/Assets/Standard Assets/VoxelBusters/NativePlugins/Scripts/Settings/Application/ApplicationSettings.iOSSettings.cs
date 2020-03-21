using UnityEngine;
using System.Collections;

namespace VoxelBusters.NativePlugins
{
	public partial class ApplicationSettings 
	{
		/// <summary>
		/// Application Settings specific to iOS platform.
		/// </summary>
		[System.Serializable]
		public class iOSSettings
		{
			#region Properties

			[SerializeField]
			[Tooltip("The string that identifies your app in iOS App Store.")]
			private 	string		m_storeIdentifier;
			[SerializeField]
			private		string		m_addressBookUsagePermissionDescription	= "$(PRODUCT_NAME) uses contacts";
			[SerializeField]
			private		string		m_cameraUsagePermissionDescription		= "$(PRODUCT_NAME) uses camera";
			[SerializeField]
			private		string		m_photoAlbumUsagePermissionDescription	= "$(PRODUCT_NAME) uses photo library";
			[SerializeField]
			private		string		m_photoAlbumModifyUsagePermissionDescription	= "$(PRODUCT_NAME) saves images to photo library";

			#endregion

			#region Fields

			/// <summary>
			/// The string that identifies your app in App Store. (read-only)
			/// </summary>
			public string StoreIdentifier
			{
				get
				{
					return m_storeIdentifier;
				}
				private set
				{
					m_storeIdentifier	= value;
				}
			}

			public string AddressBookUsagePermissionDescription
			{
				get
				{
					return m_addressBookUsagePermissionDescription;
				}
			}

			public string CameraUsagePermissionDescription
			{
				get
				{
					return m_cameraUsagePermissionDescription;
				}
			}

			public string PhotoAlbumUsagePermissionDescription
			{
				get
				{
					return m_photoAlbumUsagePermissionDescription;
				}
			}

			public string PhotoAlbumModifyUsagePermissionDescription
			{
				get
				{
					return m_photoAlbumModifyUsagePermissionDescription;
				}
			}

			#endregion
		}
	}
}
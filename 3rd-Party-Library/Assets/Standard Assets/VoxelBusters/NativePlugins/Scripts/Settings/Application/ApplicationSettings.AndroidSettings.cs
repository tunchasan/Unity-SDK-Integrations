using UnityEngine;
using System.Collections;

namespace VoxelBusters.NativePlugins
{
    using Internal;
    public partial class ApplicationSettings
    {
		/// <summary>
		/// Application Settings specific to Android platform.
		/// </summary>
		[System.Serializable]
		public class AndroidSettings
		{
			#region Properties 

			[SerializeField] 
			[Tooltip("The string that identifies your app in Google Play Store.")]
			private 	string		m_storeIdentifier;

			#endregion

			#region Properties

			/// <summary>
			/// The string that identifies your app in Google Play Store. (read-only)
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

			#endregion
		}
	}
}
using UnityEngine;
using System.Collections;

namespace VoxelBusters.NativePlugins
{
	public partial class CloudServicesSettings
	{
		[System.Serializable]
		internal class AndroidSettings
		{
			#region Fields
			
			[SerializeField]
			[Tooltip ("Automatic internal Sync timer to sync with cloud. Tries to connect to cloud, load the data and upload if any changes to cloud.")]		
			private 	float 		m_syncInterval 		= 10.0f;
			
			#endregion
			
			#region Properties
			
			internal float SyncInterval
			{
				get 
				{ 
					return m_syncInterval;
				}
			}

			#endregion
		}
	}
}
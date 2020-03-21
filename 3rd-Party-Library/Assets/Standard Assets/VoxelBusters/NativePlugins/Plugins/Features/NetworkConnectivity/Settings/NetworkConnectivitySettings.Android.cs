using UnityEngine;
using System.Collections;

namespace VoxelBusters.NativePlugins
{
	public partial class NetworkConnectivitySettings
	{
		[System.Serializable]
		public class AndroidSettings
		{
			#region Fields
			
			[SerializeField]
			[Tooltip ("The connection port of the host. For DNS IP, it will be 53 or else 80.")]		
			private 	int 		m_port 		= 53;
			
			#endregion
			
			#region Properties
			
			public int Port
			{
				get 
				{ 
					return m_port; 
				}
			}

			#endregion
		}
	}
}
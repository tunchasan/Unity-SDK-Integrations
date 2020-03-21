using UnityEngine;
using System.Collections;
using VoxelBusters.Utility;

namespace VoxelBusters.NativePlugins
{
	[System.Serializable]
	public partial class MediaLibrarySettings
	{	
		#region Fields
		
		[SerializeField]
		private 	AndroidSettings		m_android	= new AndroidSettings();

		#endregion

		#region Properties

		internal AndroidSettings Android
		{
			get 
			{
				return m_android; 
			}
		}
		
		#endregion
	}
}
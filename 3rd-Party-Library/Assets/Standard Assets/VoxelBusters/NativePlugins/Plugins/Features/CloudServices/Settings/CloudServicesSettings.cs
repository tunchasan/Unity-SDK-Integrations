using UnityEngine;
using System.Collections;

namespace VoxelBusters.NativePlugins
{
	[System.Serializable]
	public partial class CloudServicesSettings
	{
		#region Fields

		[SerializeField]
		private 	AndroidSettings	m_android = null;

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

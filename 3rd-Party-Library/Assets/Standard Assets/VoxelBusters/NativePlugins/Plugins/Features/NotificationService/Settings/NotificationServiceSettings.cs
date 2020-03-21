using UnityEngine;
using System.Collections;

namespace VoxelBusters.NativePlugins
{
	using Internal;

	[System.Serializable]
	public partial class NotificationServiceSettings
	{
		#region Fields

		[SerializeField]
		private 	iOSSettings			m_iOS		= new iOSSettings();
		[SerializeField]
		private 	AndroidSettings		m_android	= new AndroidSettings();

		#endregion

		#region Properites

		internal iOSSettings iOS
		{
			get 
			{ 
				return m_iOS; 
			}
		}

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
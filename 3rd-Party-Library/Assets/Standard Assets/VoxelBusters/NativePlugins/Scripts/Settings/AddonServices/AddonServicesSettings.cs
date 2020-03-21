using UnityEngine;
using System.Collections;

namespace VoxelBusters.NativePlugins
{
	[System.Serializable]
	public class AddonServicesSettings 
	{
		#region Fields

	#if USES_SOOMLA_GROW
		[SerializeField]
		private		SoomlaGrowServiceSettings		m_soomlaGrowSettings	= new SoomlaGrowServiceSettings();
	#endif
	#if USES_ONE_SIGNAL
		[SerializeField]
		private		OneSignalServiceSettings		m_oneSignalSettings		= new OneSignalServiceSettings();
	#endif

		#endregion

		#region Properties

	#if USES_SOOMLA_GROW
		public SoomlaGrowServiceSettings SoomlaGrowService
		{
			get
			{
				return m_soomlaGrowSettings;
			}
		}
	#endif

	#if USES_ONE_SIGNAL
		public OneSignalServiceSettings OneSignalService
		{
			get
			{
				return m_oneSignalSettings;
			}
		}
	#endif

		#endregion
	}
}
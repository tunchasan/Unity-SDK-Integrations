using UnityEngine;
using System.Collections;
using VoxelBusters.NativePlugins.Internal;

namespace VoxelBusters.NativePlugins
{
	[System.Serializable]
	public class UtilitySettings
	{
		#region Fields

		[SerializeField]
		[Tooltip("Rate My App dialog settings.")]
		private 	RateMyAppSettings		m_rateMyApp;
		[SerializeField]
		private 	AndroidSettings			m_android	= null;

		#endregion

		#region Properties

		public RateMyAppSettings RateMyApp
		{
			get
			{
				return m_rateMyApp;
			}
			private set
			{
				m_rateMyApp	= value;
			}
		}

		public AndroidSettings Android
		{
			get
			{
				return m_android;
			}
		}

		#endregion

		[System.Serializable]
		public class AndroidSettings
		{
			#region Fields

			[SerializeField, NotifyNPSettingsOnValueChange]
			[Tooltip("Enable this for setting application badge on Android. Disable this if not used as it will skip adding extra libraries")]
			private 	bool		m_modifiesApplicationBadge = true;

			#endregion

			#region Properties

			internal bool ModifiesApplicationBadge
			{
				get
				{
					return m_modifiesApplicationBadge;
				}
			}

			#endregion
		}
	}
}

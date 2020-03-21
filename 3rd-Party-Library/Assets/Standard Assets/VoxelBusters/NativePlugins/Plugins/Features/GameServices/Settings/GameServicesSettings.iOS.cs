using UnityEngine;
using System.Collections;
using VoxelBusters.Utility;

namespace VoxelBusters.NativePlugins
{
	public partial class GameServicesSettings
	{
		[System.Serializable]
		public class iOSSettings 
		{
			#region Fields

			[SerializeField]
			[Tooltip ("If checked, a banner is displayed when an achievement is completed.")]
			private 	bool 	m_showDefaultAchievementCompletionBanner	=	true;

			#endregion

			#region Properties

			internal bool ShowDefaultAchievementCompletionBanner
			{
				get
				{
					return m_showDefaultAchievementCompletionBanner;
				}
			}

			#endregion
		}
	}
}
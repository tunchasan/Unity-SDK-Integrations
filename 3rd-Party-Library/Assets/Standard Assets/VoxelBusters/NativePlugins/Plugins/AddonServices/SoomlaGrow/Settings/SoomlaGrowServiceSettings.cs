using UnityEngine;
using System.Collections;

#if USES_SOOMLA_GROW
namespace VoxelBusters.NativePlugins
{
	[System.Serializable]
	public class SoomlaGrowServiceSettings 
	{
		#region Fields

		[SerializeField]
		[Tooltip("The value represents your Soomla Grow game key.")]
		private		string 		m_gameKey;
		[SerializeField]
		[Tooltip("The value represents your Soomla Grow environment key.")]
		private		string		m_environmentKey;
		
		#endregion

		#region Properties

		internal string GameKey
		{
			get
			{
				return m_gameKey;
			}
		}

		internal string EnvironmentKey
		{
			get
			{
				return m_environmentKey;
			}
		}

		#endregion
	}
}
#endif
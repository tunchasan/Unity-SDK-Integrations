using UnityEngine;
using System.Collections;

#if USES_ONE_SIGNAL
namespace VoxelBusters.NativePlugins
{
	[System.Serializable]
	public class OneSignalServiceSettings 
	{
		#region Fields

		[SerializeField]
		[Tooltip("The value represents your One Signal app id.")]
		private		string 		m_appID;
		[SerializeField]
		[Tooltip("The value represents the Google project number. It is required for Android GCM pushes.")]
		private		string 		m_googleProjectNumber;

		#endregion

		#region Properties

		internal string AppID
		{
			get
			{
				return m_appID;
			}
		}

		internal string GoogleProjectNumber
		{
			get
			{
				return m_googleProjectNumber;
			}
		}

		#endregion
	}
}
#endif
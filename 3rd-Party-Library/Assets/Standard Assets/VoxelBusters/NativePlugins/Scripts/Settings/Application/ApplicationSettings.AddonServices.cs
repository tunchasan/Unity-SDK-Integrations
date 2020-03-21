using UnityEngine;
using System.Collections;

namespace VoxelBusters.NativePlugins
{
	using Internal;
	
	public partial class ApplicationSettings 
	{
		[System.Serializable]
		public class AddonServices
		{
			#region Fields

#if !NATIVE_PLUGINS_LITE_VERSION
			[SerializeField]
			[NotifyNPSettingsOnValueChange]
			[Tooltip("If enabled, Soomla Grow service will be active within your application.")]
			private		bool	m_usesSoomlaGrow 	= false;
#endif	

#if !NATIVE_PLUGINS_LITE_VERSION
			[SerializeField]
			[NotifyNPSettingsOnValueChange]
			[Tooltip("If enabled, One Signal service will be active within your application.")]
			private		bool	m_usesOneSignal 	= false;
#endif	
			#endregion
			
			#region Properties
			
			public bool UsesSoomlaGrow
			{
				get
				{
#if !NATIVE_PLUGINS_LITE_VERSION
					return m_usesSoomlaGrow;
#else
					return false;
#endif
				}
			}
			
			public bool UsesOneSignal
			{
				get
				{
#if !NATIVE_PLUGINS_LITE_VERSION
					return m_usesOneSignal;
#else
					return false;
#endif
				}
			}
			
			#endregion
		}
	}
}
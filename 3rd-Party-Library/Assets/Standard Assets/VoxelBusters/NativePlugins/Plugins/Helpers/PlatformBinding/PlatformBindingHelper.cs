using UnityEngine;
using System.Collections;

namespace VoxelBusters.NativePlugins
{
	public partial class PlatformBindingHelper : MonoBehaviour 
	{
		
		void Awake()
		{
			#if UNITY_ANDROID && !UNITY_EDITOR
				InitializeAndroidSettings();
			#endif
		}
	}
}

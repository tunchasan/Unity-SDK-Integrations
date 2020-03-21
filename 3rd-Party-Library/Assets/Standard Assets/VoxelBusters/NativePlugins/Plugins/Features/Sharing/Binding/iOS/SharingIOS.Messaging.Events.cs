using UnityEngine;
using System.Collections;

#if USES_SHARING && UNITY_IOS
namespace VoxelBusters.NativePlugins
{
	public partial class SharingIOS : Sharing 
	{
		private enum eMessageComposeResult 
		{
			CANCELLED,
			SENT,
			FAILED
		}
	}
}
#endif
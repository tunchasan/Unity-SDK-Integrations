#if USES_SHARING && UNITY_EDITOR
using UnityEngine;
using System.Collections;
using VoxelBusters.UASUtils;

namespace VoxelBusters.NativePlugins
{
	using Internal;

	public partial class SharingEditor : Sharing 
	{
		#region Overriden API's 
		
		public override bool IsFBShareServiceAvailable ()
		{
			DebugUtility.Logger.LogError(Constants.kDebugTag, Constants.kNotSupportedInEditor);

			return base.IsTwitterShareServiceAvailable();
		}
		
		public override bool IsTwitterShareServiceAvailable ()
		{
			DebugUtility.Logger.LogError(Constants.kDebugTag, Constants.kNotSupportedInEditor);

			return base.IsTwitterShareServiceAvailable();
		}
		
		#endregion
	}
}
#endif
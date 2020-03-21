#if USES_SHARING && UNITY_EDITOR
using UnityEngine;
using System.Collections;
using VoxelBusters.UASUtils;

namespace VoxelBusters.NativePlugins
{
	using Internal;

	public partial class SharingEditor : Sharing 
	{
		#region Methods

		public override bool IsWhatsAppServiceAvailable ()
		{
			DebugUtility.Logger.LogError(Constants.kDebugTag, Constants.kNotSupportedInEditor);

			return base.IsWhatsAppServiceAvailable();
		}

		#endregion
	}
}
#endif
#if UNITY_IOS
using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using VoxelBusters.UASUtils;

namespace VoxelBusters.NativePlugins
{
	using Internal;

	public partial class UIIOS : UI 
	{
		#region Native Methods
		
		[DllImport("__Internal")]
		private static extern void setPopoverPoint (float x, float y);
		
		#endregion

		#region API Methods

		public override void ShowToast (string _message, eToastMessageLength _length)
		{
			DebugUtility.Logger.LogWarning(Constants.kDebugTag, Constants.kAndroidFeature);
		}

		public override void SetPopoverPoint (Vector2 _position)
		{
			setPopoverPoint(_position.x, _position.y);
		}

		#endregion
	}
}
#endif
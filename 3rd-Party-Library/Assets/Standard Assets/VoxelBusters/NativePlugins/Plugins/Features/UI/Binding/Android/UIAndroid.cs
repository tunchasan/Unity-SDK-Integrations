#if UNITY_ANDROID
using UnityEngine;
using System.Collections;
using VoxelBusters.UASUtils;

namespace VoxelBusters.NativePlugins
{
	using Internal;

	public partial class UIAndroid : UI 
	{	
		#region Constructors
		
		UIAndroid()
		{
			Plugin = AndroidPluginUtility.GetSingletonInstance(Native.Class.NAME);
		}
		
		#endregion

		#region API
		
		public override void ShowToast (string _message, eToastMessageLength _length)
		{
			Plugin.Call(Native.Methods.SHOW_TOAST, _message, _length == eToastMessageLength.SHORT ? "SHORT" : "LONG");
		}

		public override void SetPopoverPoint (Vector2 _position)
		{
			DebugUtility.Logger.LogWarning(Constants.kDebugTag, Constants.kiOSFeature);
		}
		
		#endregion
	}
}
#endif
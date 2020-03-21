#if USES_NETWORK_CONNECTIVITY && UNITY_ANDROID
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using VoxelBusters.Utility;

namespace VoxelBusters.NativePlugins
{
	using Internal;

	public partial class NetworkConnectivityAndroid : NetworkConnectivity 
	{	
		#region Constructors
		
		NetworkConnectivityAndroid()
		{
			Plugin = AndroidPluginUtility.GetSingletonInstance(Native.Class.NAME);
		}
		
		#endregion

		#region API

		public override void Initialise ()
		{
			base.Initialise ();

			NetworkConnectivitySettings _settings = NPSettings.NetworkConnectivity;
			Plugin.Call(Native.Methods.INITIALIZE,_settings.HostAddress, 
			            							_settings.Android.Port, 
													_settings.TimeGapBetweenPolling, 
													_settings.TimeOutPeriod,
													_settings.MaxRetryCount);
			
		}	

		#endregion
	}
}
#endif
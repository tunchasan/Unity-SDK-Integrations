using UnityEngine;

#if USES_NETWORK_CONNECTIVITY && UNITY_ANDROID
namespace VoxelBusters.NativePlugins
{
	using Internal;

	public partial class NetworkConnectivityAndroid : NetworkConnectivity 
	{
		#region Platform Native Info
		
		private class Native
		{
			// Handler class name
			internal class Class
			{
				internal const string NAME				= "com.voxelbusters.nativeplugins.features.reachability.NetworkReachabilityHandler";
			}
			
			// For holding method names
			internal class Methods
			{
				internal const string INITIALIZE		= "initialize";
			}
		}
		
		#endregion
		
		#region  Native Access Variables
		
		private AndroidJavaObject  	Plugin
		{
			get;
			set;
		}
		
		#endregion
	}
}
#endif
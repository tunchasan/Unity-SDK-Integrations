#if USES_NETWORK_CONNECTIVITY && UNITY_IOS
using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

namespace VoxelBusters.NativePlugins
{
	public class NetworkConnectivityIOS : NetworkConnectivity
	{
		#region Native Methods

		[DllImport("__Internal")]
		private static extern void cpnpNetworkConnectivitySetHostAddress (string _newIPAddress);

		#endregion

		#region API

		public override void Initialise ()
		{
			base.Initialise ();

			NetworkConnectivitySettings _settings = NPSettings.NetworkConnectivity;
			cpnpNetworkConnectivitySetHostAddress(_settings.HostAddress);
		}

		#endregion
	}
}
#endif
#if USES_NETWORK_CONNECTIVITY
using UnityEngine;
using System.Collections;
using VoxelBusters.UASUtils;

namespace VoxelBusters.NativePlugins
{
	using Internal;

	public partial class NetworkConnectivity : MonoBehaviour 
	{
		#region Delegates

		///	<summary>
		///	Delegate that will be called whenever connectivity state changes.
		///	</summary>
		///	<param name="_isConnected"><c>true</c> if connected to network; otherwise, <c>false</c>.</param>
		public delegate void NetworkConnectivityChanged (bool _isConnected);

		#endregion

		#region Events

		/// <summary>
		/// Event that will be called whenever connectivity state changes.
		/// </summary>
		/// <example>
		/// The following code example demonstrates how to use this event.
		/// <code>
		/// using UnityEngine;
		/// using System.Collections;
		/// using VoxelBusters.NativePlugins;
		/// 
		/// public class ExampleClass : MonoBehaviour 
		/// {
		/// 	private void OnEnable ()
		/// 	{
		/// 		// Registering for event
		/// 	    NetworkConnectivity.NetworkConnectivityChangedEvent	+= OnNetworkConnectivityChanged;
		///     }
		/// 
		/// 	private void OnDisable ()
		/// 	{
		/// 		// Unregistering event
		/// 	    NetworkConnectivity.NetworkConnectivityChangedEvent	-= OnNetworkConnectivityChanged;
		/// 	}
		/// 
		/// 	private void OnNetworkConnectivityChanged (bool _isConnected)
		/// 	{
		/// 		if (_isConnected)
		/// 		{
		/// 			// Handle when app goes online
		/// 		}
		/// 		else
		/// 		{
		/// 			// Handle when app goes offline
		/// 		}
		/// 	}
		/// }
		/// </code>
		/// </example>
		public static event NetworkConnectivityChanged	NetworkConnectivityChangedEvent;
		
		#endregion
		
		#region Native Callback Methods
		
		protected void ConnectivityChanged (string _newstate)
		{
			bool _isConnected	= bool.Parse(_newstate);
			ConnectivityChanged(_isConnected);
		}
		
		protected void ConnectivityChanged (bool _connected)
		{
			IsConnected = _connected;
			DebugUtility.Logger.Log(Constants.kDebugTag, "[NetworkConnectivity] Connectivity changed, IsConnected=" + IsConnected);
			
			// Trigger event in handler
			if (NetworkConnectivityChangedEvent != null)
				NetworkConnectivityChangedEvent(IsConnected);
		}
		
		#endregion
	}
}
#endif
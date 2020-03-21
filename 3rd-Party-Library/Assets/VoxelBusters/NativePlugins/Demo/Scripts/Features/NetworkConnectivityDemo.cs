using UnityEngine;
using System.Collections;
using VoxelBusters.Utility;
using VoxelBusters.NativePlugins;

namespace VoxelBusters.NativePlugins.Demo
{
#if !USES_NETWORK_CONNECTIVITY
	public class NetworkConnectivityDemo : NPDisabledFeatureDemo 
	{}
#else
	public class NetworkConnectivityDemo : NPDemoBase 
	{
		#region Unity Methods

		protected override void Start ()
		{
			base.Start ();

			// Set additional info texts
			AddExtraInfoTexts(
				"You can configure this feature in NPSettings->Network Connectivity Settings.");
		}
		
		protected override void OnEnable ()
		{
			base.OnEnable();
			
			// Register to event
			NetworkConnectivity.NetworkConnectivityChangedEvent	+= NetworkConnectivityChangedEvent;}
		
		protected override void OnDisable ()
		{
			base.OnDisable();
			
			// Deregister to event
			NetworkConnectivity.NetworkConnectivityChangedEvent	-= NetworkConnectivityChangedEvent;
		}
		
		#endregion
		
		#region GUI Methods
		
		protected override void DisplayFeatureFunctionalities ()
		{
			base.DisplayFeatureFunctionalities ();

			if (GUILayout.Button ("Initialise"))
			{
				Initialise ();
			}

			GUILayout.Box ("[NOTE] NetworkConnectivityChangedEvent is fired whenever there is a change in connectivity state.");

			if (GUILayout.Button ("Is Network Reachable?"))
			{
				bool _isConnected = IsConnected ();
				
				if (_isConnected)
				{
					AddNewResult ("Network is Reachable.");
				}
				else
				{
					AddNewResult ("Network is Unreachable.");
				}
			}	
		}
		
		#endregion

		#region API Methods
		
		private void Initialise ()
		{
			NPBinding.NetworkConnectivity.Initialise ();			
		}

		private bool IsConnected ()
		{
			return NPBinding.NetworkConnectivity.IsConnected;
		}
		
		#endregion

		#region API Callback Methods

		private void NetworkConnectivityChangedEvent (bool _isConnected)
		{
			AddNewResult ("Received connectivity changed event.");
			AppendResult (_isConnected ? "Yeah! Now we are online." : "Oh no! We lost connectivity.");
		}

		#endregion
	}
#endif
}
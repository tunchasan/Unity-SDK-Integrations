#if USES_NETWORK_CONNECTIVITY
using UnityEngine;
using System.Collections;
using VoxelBusters.UASUtils;

namespace VoxelBusters.NativePlugins
{
	/// <summary>
	/// Provides cross-platform interface to check network connectivity status.
	/// </summary>
	public partial class NetworkConnectivity : MonoBehaviour 
	{
		#region Properties

		/// <summary>
		/// A bool value that indicates connectivity status.
		/// </summary>
		/// <value><c>true</c> if connected to network; otherwise, <c>false</c>.</value>
		public bool IsConnected
		{
			get;
		 	protected set;
		}

		#endregion

		#region API Methods

		/// <summary>
		/// Initialises the component with the configuration values set in <b>Network Connectivity Settings</b>.
		/// </summary>
		///	<remarks> 
		/// \note You need to call this method, to start checking if IP Address specified in the <b>Network Connectivity Settings</b> is reachable or not. 
		/// </remarks>
		public virtual void Initialise ()
		{
			StartCoroutine(ManuallyTriggerInitialState());
		}

		#endregion

		#region Misc. Methods

		private IEnumerator ManuallyTriggerInitialState ()
		{
			yield return new WaitForSeconds(1f);

			if (IsConnected == false)
				ConnectivityChanged(false);
		}

		#endregion
	}
}
#endif
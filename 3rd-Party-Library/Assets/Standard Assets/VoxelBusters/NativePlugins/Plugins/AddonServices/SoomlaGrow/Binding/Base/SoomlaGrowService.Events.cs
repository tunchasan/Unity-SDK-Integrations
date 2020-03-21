using UnityEngine;
using System.Collections;

#if USES_SOOMLA_GROW
namespace VoxelBusters.NativePlugins
{
	public partial class SoomlaGrowService : MonoBehaviour 
	{
		#region Delegates

		public delegate void HighwayStateChanged (eHighwayConnectionState _newState);

		#endregion

		#region Events

		public static event HighwayStateChanged HighwayStateChangedEvent	= null;

		#endregion

		#region Native Callback Methods

		protected void OnGrowHighwayInitialised (string _dataStr)
		{
			if (HighwayStateChangedEvent != null)
				HighwayStateChangedEvent(eHighwayConnectionState.INITIALISED);
		}

		protected void OnGrowHighwayConnected (string _dataStr)
		{
			if (HighwayStateChangedEvent != null)
				HighwayStateChangedEvent(eHighwayConnectionState.CONNECTED);
		}

		protected void OnGrowHighwayDisconnected (string _dataStr)
		{
			if (HighwayStateChangedEvent != null)
				HighwayStateChangedEvent(eHighwayConnectionState.DISCONNECTED);
		}

		#endregion
	}
}
#endif
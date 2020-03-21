#if USES_NETWORK_CONNECTIVITY && UNITY_ANDROID
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using VoxelBusters.Utility;
using VoxelBusters.UASUtils;

namespace VoxelBusters.NativePlugins
{
	using Internal;

	public partial class NetworkConnectivityAndroid : NetworkConnectivity 
	{
		#region Internal Variables

		private bool m_currentConnectionStatus 	= false;
		private bool m_networkHardwareConnectivityStatus = false;
		private bool m_urlReachabilityStatus 	= false;

		#endregion

		#region Internal Callbacks
	
		//This will be triggered from native.
		private void NetworkHardwareStatusChange(string _statusStr)
		{
			bool _status	= bool.Parse(_statusStr);
			m_networkHardwareConnectivityStatus = _status;
			CheckForNetworkReachabilityStatus();
		}
		
		private void NetworkSocketStatusChange(string _statusStr)
		{
			bool _status	= bool.Parse(_statusStr);
			m_urlReachabilityStatus = _status;
			CheckForNetworkReachabilityStatus();
		}
		
		private void URLReachabilityChange(bool _status)
		{
			m_urlReachabilityStatus = _status;
			CheckForNetworkReachabilityStatus();
		}

		#endregion

		#region Notifiers
		
		private void CheckForNetworkReachabilityStatus()
		{
			bool _newConnectionStatus = m_networkHardwareConnectivityStatus && m_urlReachabilityStatus;
			
			if (m_currentConnectionStatus != _newConnectionStatus)
			{
				m_currentConnectionStatus = _newConnectionStatus;
				ConnectivityChanged(_newConnectionStatus);


				if(_newConnectionStatus == false)
				{
					DebugUtility.Logger.LogWarning(Constants.kDebugTag, "[NetworkConnectivity] networkHardwareConnected ? " + m_networkHardwareConnectivityStatus + " URL Reachable ? " + m_urlReachabilityStatus);
				}
			}
		}
		
		#endregion
	}
}
#endif
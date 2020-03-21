using UnityEngine;
using System.Collections;

namespace VoxelBusters.NativePlugins
{
	[System.Serializable]
	public partial class NetworkConnectivitySettings
	{
		#region Fields
		
		[SerializeField]
		[Tooltip("The host IP address in IPv4 format.")]
		private 	string 			m_hostAddressIPV4 		= "8.8.8.8";
		[SerializeField]
		[Tooltip("The host IP address in IPv6 format.")]
		private 	string 			m_hostAddressIPV6 		= "0:0:0:0:0:FFFF:0808:0808";
		[SerializeField]
		[Tooltip ("The number of seconds to wait before the request times out.")]		
		private 	int 			m_timeOutPeriod 		= 60;
		[SerializeField]
		[Tooltip ("The number of retry attempts, when a response is not received from the host.")]		
		private 	int 			m_maxRetryCount 		= 2;
		[SerializeField]
		[Tooltip ("The time interval between consecutive poll.")]		
		private 	float 			m_timeGapBetweenPolling = 2.0f;
		[SerializeField]
		private 	AndroidSettings	m_android				= new AndroidSettings();

		#endregion

		#region Properties

		public string HostAddress
		{
			get 
			{ 
				return (Application.platform == RuntimePlatform.IPhonePlayer)
						? m_hostAddressIPV6
						: m_hostAddressIPV4;
			}
		}

		public int TimeOutPeriod
		{
			get 
			{ 
				return m_timeOutPeriod; 
			}
		}

		public int MaxRetryCount
		{
			get 
			{ 
				return m_maxRetryCount; 
			}
		}

		public float TimeGapBetweenPolling
		{
			get 
			{ 
				return m_timeGapBetweenPolling;
			}
		}

		public AndroidSettings Android
		{
			get 
			{ 
				return m_android; 
			}
		}
		
		#endregion
	}
}
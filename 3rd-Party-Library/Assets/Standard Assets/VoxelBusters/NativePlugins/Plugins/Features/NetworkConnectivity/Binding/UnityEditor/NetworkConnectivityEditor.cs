#if USES_NETWORK_CONNECTIVITY && UNITY_EDITOR
using UnityEngine;
using System.Collections;

namespace VoxelBusters.NativePlugins
{
	using Internal;

	public partial class NetworkConnectivityEditor : NetworkConnectivity 
	{
		#region API
		
		public override void Initialise ()
		{
			base.Initialise ();

			NetworkConnectivitySettings _settings = NPSettings.NetworkConnectivity;

			// Starts scheduler to monitor connectivity
			StopCoroutine("MonitorNetworkConnectivity");
			StartCoroutine(MonitorNetworkConnectivity(_settings));
		}
		
		#endregion

		#region Connectivity Methods
		
		private IEnumerator MonitorNetworkConnectivity (NetworkConnectivitySettings _settings)
		{
			string _pingAddress		= _settings.HostAddress;
			int _maxRetryCount		= _settings.MaxRetryCount;
			float _dt				= _settings.TimeGapBetweenPolling;
			float _timeOutPeriod	= _settings.TimeOutPeriod;
			bool _connectedToNw		= IsConnected;
			
			while (true)
			{
				bool _nowConnected	= false;
				
				for (int _rIter = 0; _rIter < _maxRetryCount; _rIter++)
				{
					Ping _ping			= new Ping(_pingAddress);
					float  _elapsedTime	= 0f;
					
					// Ping test
					while (!_ping.isDone && _elapsedTime < _timeOutPeriod)
					{
						_elapsedTime	+= Time.deltaTime;
						
						// Wait until next frame
						yield return null;
					}
					
					// Ping request complted within timeout period, so we are connected to network
					if (_ping.isDone && (_ping.time != -1) && _elapsedTime < _timeOutPeriod)
					{
						_nowConnected	= true;
						break;
					}
				}
				
				// Notify Manager about state change
				if (!_connectedToNw)
				{
					if (_nowConnected)
					{
						_connectedToNw	= true;
						ConnectivityChanged(_connectedToNw);
					}
				}
				else
				{
					if (!_nowConnected)
					{
						_connectedToNw	= false;
						ConnectivityChanged(_connectedToNw);
					}
				}
				
				// Wait
				yield return new WaitForSeconds(_dt);
			}
		}

		#endregion
	}
}
#endif
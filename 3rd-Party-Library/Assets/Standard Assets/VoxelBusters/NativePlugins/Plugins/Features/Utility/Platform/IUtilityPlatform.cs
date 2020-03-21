using UnityEngine;
using System.Collections;

namespace VoxelBusters.NativePlugins.Internal
{
	public interface IUtilityPlatform 
	{
		#region Methods

		void OpenStoreLink (string _applicationID);
		void SetApplicationIconBadgeNumber (int _badgeNumber);

		RateMyApp CreateRateMyApp(RateMyAppSettings _settings);

		#endregion
	}
}
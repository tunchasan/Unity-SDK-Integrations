#if UNITY_IOS
using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using VoxelBusters.UASUtils;

namespace VoxelBusters.NativePlugins.Internal
{
	public class UtilityIOS : IUtilityPlatform 
	{
		#region Binding Methods
		
		[DllImport("__Internal")]
		private static extern void setApplicationIconBadgeNumber (int _badgeNumber);
		
		#endregion

		#region Public Methods

		public void OpenStoreLink (string _applicationID)
		{
			DebugUtility.Logger.Log(Constants.kDebugTag, "[Utility] Opening store link, ApplicationID=" + _applicationID);

			string	_version		= SystemInfo.operatingSystem;
			string	_appstoreURL	= (_version.CompareTo("7.0") >= 0)
				? string.Format("itms-apps://itunes.apple.com/app/id{0}", _applicationID)
				: string.Format("itms-apps://itunes.apple.com/WebObjects/MZStore.woa/wa/viewContentsUserReviews?type=Purple+Software&id={0}", _applicationID);
			
			Application.OpenURL(_appstoreURL);
		}
		
		public void SetApplicationIconBadgeNumber (int _badgeNumber)
		{
			setApplicationIconBadgeNumber(_badgeNumber);
		}

		public RateMyApp CreateRateMyApp(RateMyAppSettings _settings)
		{
#if USES_RATE_MY_APP
			RateMyAppIOSStoreController _controller = new RateMyAppIOSStoreController();
			return RateMyApp.Create(_viewController: _controller,
			                        _keysCollection: _controller,
			                        _eventResponder: _controller,
			                        _operationHandler: _controller,
			                        _settings: _settings);
#else
			return null;	
#endif
		}

		#endregion
	}
}
#endif
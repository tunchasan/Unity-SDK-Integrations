using UnityEngine;
using System.Collections;
using VoxelBusters.UASUtils;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace VoxelBusters.NativePlugins.Internal
{
	public class UtilityUnsupported : IUtilityPlatform 
	{
		#region Public Methods

		public void OpenStoreLink (string _applicationID)
		{
#if UNITY_EDITOR
			DebugUtility.Logger.Log(Constants.kDebugTag, "[Utility] Opening store, ApplicationID=" + _applicationID);

			if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android)
			{
				Application.OpenURL("https://play.google.com/store/apps/details?id=" + _applicationID);	
			}
#if UNITY_4_5 || UNITY_4_6 || UNITY_4_7 
			else if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.iPhone)
#else
			else if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.iOS)
#endif
			{
				Application.OpenURL("https://itunes.apple.com/app/id" + _applicationID);
			}
#else
			Debug.LogWarning(Constants.kNotSupported);
#endif
		}
		
		public void SetApplicationIconBadgeNumber (int _badgeNumber)
		{
			DebugUtility.Logger.LogWarning(Constants.kDebugTag, Constants.kiOSFeature);
		}

		public RateMyApp CreateRateMyApp(RateMyAppSettings _settings)
		{
			RateMyAppGenericController _controller = new RateMyAppGenericController();
			return RateMyApp.Create(_viewController: _controller,
			                        _keysCollection: _controller,
			                        _eventResponder: _controller,
			                        _operationHandler: _controller,
			                        _settings: _settings);
		}

		#endregion
	}
}
using UnityEngine;
using System.Collections;
using VoxelBusters.UASUtils;

using PlayerSettings = VoxelBusters.Utility.PlayerSettings;

namespace VoxelBusters.NativePlugins
{
	using Internal;

	/// <summary>
	/// Provides a cross-platform interface to access useful features such as RateMyApp, app's bundle information etc.
	/// </summary>
	public class Utility : MonoBehaviour 
	{
		#region Fields

		private		IUtilityPlatform	m_platform;

		#endregion

		#region Properties

		/// <summary>
		/// The shared instance of <see cref="RateMyApp"/> feature. (read-only)
		/// </summary>
		/// <remarks>
		/// \note Returns <c>null</c> value, if feature is marked disabled in Utility Settings.
		/// </remarks>
		public RateMyApp RateMyApp
		{
			get;
			private set;
		}

		#endregion

		#region Unity Methods

		private void Awake ()
		{
			// Set properties
			m_platform	= CreatePlatformSpecificObject();

			RateMyAppSettings _settings = NPSettings.Utility.RateMyApp;
			if (_settings.IsEnabled)
			{
				RateMyApp	= m_platform.CreateRateMyApp(_settings);
				RateMyApp.RecordAppLaunch();
			}
		}

		private IEnumerator Start ()
		{
			yield return new WaitForEndOfFrame();

			if (RateMyApp != null)
			{
				RateMyApp.AskForReview();
			}
		}

		private void OnApplicationPause (bool _isPaused)
		{
			if (_isPaused)
				return;
			
			if (RateMyApp != null)
			{
				RateMyApp.AskForReview();
			}
		}

		#endregion

		#region Methods

		/// <summary>
		/// Returns a new unique identifier.
		/// </summary>
		/// <returns>New unique identifier.</returns>
		public string GetUUID ()
		{
			return System.Guid.NewGuid().ToString();
		}

		/// <summary>
		/// Opens the Store page of the specified application.
		/// </summary>
		/// <description>
		///	For iOS platform, id is the value that identifies your app on App Store. 
		/// And on Android, it will be same as app's bundle identifier (com.example.test).
		/// </description>
		/// <param name="_applicationIDList">An array of string values, that holds app id's of each supported platform.</param>
		/// <example>
		/// The following code example shows how to open store link.
		/// <code>
		/// using UnityEngine;
		/// using System.Collections;
		/// using VoxelBusters.NativePlugins;
		/// 
		/// public class ExampleClass : MonoBehaviour 
		/// {
		/// 	public void OpenStorePage ()
		/// 	{
		/// 		NPBinding.Utility.OpenStoreLink(PlatformValue.Android("com.example.app"), 
		/// 										PlatformValue.IOS("ios-app-id"));
		///     }
		/// }
		/// </code>
		/// </example>
		public void OpenStoreLink (params PlatformValue[] _storeIdentifiers)
		{
			PlatformValue _storeIdentifier	= PlatformValueHelper.GetCurrentPlatformValue(_array: _storeIdentifiers);
			if (_storeIdentifier == null)
			{
				DebugUtility.Logger.Log(Constants.kDebugTag, "[Utility] The operation could not be completed because application identifier is invalid.");
				return;
			}
			
			OpenStoreLink(_applicationID: _storeIdentifier.Value);
		}
		
		/// <summary>
		/// Opens the Store page of the specified application.
		/// </summary>
		/// <description>
		///	For iOS platform, id is the value that identifies your app on App Store. 
		/// And on Android, it will be same as app's bundle identifier (com.example.test).
		/// </description>
		/// <param name="_applicationID">A string that identifies an application in the current platform's Store.</param>
		public void OpenStoreLink (string _applicationID)
		{
			m_platform.OpenStoreLink(_applicationID);
		}

		/// <summary>
		/// Sets the specified numeric value as the application's badge number.
		/// </summary>
		/// <param name="_badgeNumber">The numeric value to be set as badge number.</param>
		/// <remarks>
		/// \note Setting this property to 0 (zero) will hide the badge number.
		/// </remarks>
		public void SetApplicationIconBadgeNumber (int _badgeNumber)
		{
			m_platform.SetApplicationIconBadgeNumber(_badgeNumber);
		}

		/// <summary>
		/// Returns the string that specifies build version number of the bundle.
		/// </summary>
		/// <returns>The bundle version.</returns>
		public string GetBundleVersion ()
		{
			return PlayerSettings.GetBundleVersion();
		}

		/// <summary>
		/// Returns the string that identifies your application to the system.
		/// </summary>
		/// <returns>The bundle identifier.</returns>
		public string GetBundleIdentifier ()
		{
			return PlayerSettings.GetBundleIdentifier();
		}

		#endregion

		#region Private Methods

		private IUtilityPlatform CreatePlatformSpecificObject ()
		{
#if UNITY_EDITOR
			return new UtilityUnsupported();
#elif UNITY_IOS
			return new UtilityIOS();
#elif UNITY_ANDROID
			return new UtilityAndroid();
#else
			return new UtilityUnsupported();
#endif
		}

		#endregion
	}
}
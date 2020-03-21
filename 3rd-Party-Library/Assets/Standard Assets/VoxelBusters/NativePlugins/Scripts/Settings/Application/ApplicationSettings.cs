using UnityEngine;
using System.Collections;

namespace VoxelBusters.NativePlugins
{
	using Internal;

	/// <summary>
	/// Application Settings provides interface to configure properties related to this application.
	/// </summary>
	[System.Serializable]
	public partial class ApplicationSettings 
	{
		#region Fields

		[SerializeField]
		[Tooltip("Select the features that you would like to use.")]
		private		Features		m_supportedFeatures			= new Features();
		[SerializeField]
		[Tooltip("Select the Addon services that you would like to use.")]
		private		AddonServices 	m_supportedAddonServices	= new AddonServices();
		[SerializeField]
		private		iOSSettings		m_iOS						= new iOSSettings();
		[SerializeField]
		private 	AndroidSettings	m_android					= new AndroidSettings();

		#endregion

		#region Properties

		internal bool IsDebugMode
		{
			get
			{
				return Debug.isDebugBuild;
			}
		}

		internal iOSSettings IOS
		{
			get
			{
				return m_iOS;
			}
		}
		
		internal AndroidSettings Android
		{
			get
			{
				return m_android;
			}
		}

		internal Features SupportedFeatures
		{
			get
			{
				return m_supportedFeatures;
			}
		}

		internal AddonServices SupportedAddonServices
		{
			get
			{
				return m_supportedAddonServices;
			}
		}
		
		/// <summary>
		/// Gets the store identifier for current build platform.
		/// </summary>
		/// <value>The store identifier for current build platform.</value>
		public string StoreIdentifier
		{
			get
			{
#if UNITY_ANDROID
				return m_android.StoreIdentifier;
#else
				return m_iOS.StoreIdentifier;
#endif
			}
		}
		
		#endregion
	}
}
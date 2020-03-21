using UnityEngine;
using System.Collections;
using System;
using System.Reflection;
using System.Collections.Generic;
using VoxelBusters.DesignPatterns;
using VoxelBusters.NativePlugins;
using VoxelBusters.NativePlugins.Internal;
using VoxelBusters.Utility;

// Conflict handlers
#if USES_SOOMLA_GROW
using SoomlaGrowService		= VoxelBusters.NativePlugins.SoomlaGrowService;
#endif

[RequireComponent(typeof(PlatformBindingHelper))]
public class NPBinding : SingletonPattern <NPBinding>
{
	#region Static Fields

#if USES_ADDRESS_BOOK
	private 	static		AddressBook				addressBook;
#endif

#if USES_BILLING
	private 	static		Billing 				billing;
#endif

#if USES_CLOUD_SERVICES
	private 	static		CloudServices 			cloudServices;
#endif

#if USES_GAME_SERVICES
	private 	static		GameServices 			gameServices;
#endif

#if USES_MEDIA_LIBRARY
	private 	static		MediaLibrary 			mediaLibrary;
#endif

#if USES_NETWORK_CONNECTIVITY
	private		static		NetworkConnectivity		networkConnectivity;
#endif

#if USES_NOTIFICATION_SERVICE
	private 	static		NotificationService 	notificationService; 	
#endif

#if USES_SHARING
	private 	static		Sharing 				sharing; 		
#endif

#if USES_TWITTER
	private 	static		Twitter 				twitter;
#endif

	private		static		UI 						userInterface;
	private		static		Utility 				utility;

#if USES_WEBVIEW
	private 	static		WebViewNative 			webview;
#endif

#if USES_SOOMLA_GROW
	private		static		SoomlaGrowService		soomlaGrowService;	
#endif
	
	#endregion

	#region Static Properties

#if USES_ADDRESS_BOOK
	/// <summary>
	/// Returns platform specific interface to access Address Book feature.
	/// </summary>
	public static AddressBook AddressBook 	
	{ 
		get
		{
			NPBinding	_sharedInstance	= NPBinding.Instance;
			
			if (_sharedInstance == null)
				return null;

			if (addressBook == null)
				addressBook	= _sharedInstance.AddComponentBasedOnPlatformOnlyIfRequired<AddressBook>();
			
			return addressBook;
		}
	}
#endif

#if USES_BILLING
	/// <summary>
	/// Returns platform specific interface to access Billing (IAP) feature.
	/// </summary>
	public static Billing Billing 			
	{ 
		get
		{
			NPBinding	_sharedInstance	= NPBinding.Instance;
			
			if (_sharedInstance == null)
				return null;
			
			if (billing == null)
				billing	= _sharedInstance.AddComponentBasedOnPlatformOnlyIfRequired<Billing>();
			
			return billing;
		}
	}
#endif

#if USES_CLOUD_SERVICES
	/// <summary>
	/// Returns platform specific interface to access Billing (IAP) feature.
	/// </summary>
	public static CloudServices CloudServices 			
	{ 
		get
		{
			NPBinding	_sharedInstance	= NPBinding.Instance;
			
			if (_sharedInstance == null)
				return null;
			
			if (cloudServices == null)
				cloudServices	= _sharedInstance.AddComponentBasedOnPlatformOnlyIfRequired<CloudServices>();
			
			return cloudServices;
		}
	}
#endif

#if USES_GAME_SERVICES
	/// <summary>
	/// Returns platform specific interface to access Game Services feature.
	/// </summary>
	public static GameServices GameServices 			
	{ 
		get
		{
			NPBinding	_sharedInstance	= NPBinding.Instance;
			
			if (_sharedInstance == null)
				return null;
			
			if (gameServices == null)
				gameServices	= _sharedInstance.AddComponentBasedOnPlatformOnlyIfRequired<GameServices>();
			
			return gameServices;
		}
	}
#endif

#if USES_MEDIA_LIBRARY
	/// <summary>
	/// Returns platform specific interface to access Media Library feature.
	/// </summary>
	public static MediaLibrary MediaLibrary 	
	{ 
		get
		{
			NPBinding	_sharedInstance	= NPBinding.Instance;
			
			if (_sharedInstance == null)
				return null;
			
			if (mediaLibrary == null)
				mediaLibrary	= _sharedInstance.AddComponentBasedOnPlatformOnlyIfRequired<MediaLibrary>();
			
			return mediaLibrary;
		}
	}
#endif

#if USES_NETWORK_CONNECTIVITY
	/// <summary>
	/// Returns platform specific interface to access Network Connectivity feature.
	/// </summary>
	public static NetworkConnectivity NetworkConnectivity 	
	{ 
		get
		{
			NPBinding	_sharedInstance	= NPBinding.Instance;
			
			if (_sharedInstance == null)
				return null;

			if (networkConnectivity == null)
				networkConnectivity	= _sharedInstance.AddComponentBasedOnPlatformOnlyIfRequired<NetworkConnectivity>();
			
			return networkConnectivity;
		}
	}
#endif

#if USES_NOTIFICATION_SERVICE
	/// <summary>
	/// Returns platform specific interface to access Notification Service feature.
	/// </summary>
	public static NotificationService NotificationService 	
	{ 
		get
		{
			NPBinding	_sharedInstance	= NPBinding.Instance;
			
			if (_sharedInstance == null)
				return null;
			
			if (notificationService == null)
				notificationService	= _sharedInstance.CachedGameObject.AddComponentIfNotFound<NotificationService>();
			
			return notificationService;
		}
	}
#endif

#if USES_SHARING
	/// <summary>
	/// Returns platform specific interface to access Sharing feature.
	/// </summary>
	public static Sharing Sharing 		
	{ 
		get
		{
			NPBinding	_sharedInstance	= NPBinding.Instance;
			
			if (_sharedInstance == null)
				return null;
			
			if (sharing == null)
				sharing	= _sharedInstance.AddComponentBasedOnPlatformOnlyIfRequired<Sharing>();
				
			return sharing;
		}
	}
#endif

#if USES_TWITTER
	/// <summary>
	/// Returns platform specific interface to access Twitter feature.
	/// </summary>
	public static Twitter Twitter 		
	{ 
		get
		{
			NPBinding	_sharedInstance	= NPBinding.Instance;
			
			if (_sharedInstance == null)
				return null;
			
			if (twitter == null)
				twitter	= _sharedInstance.AddComponentBasedOnPlatformOnlyIfRequired<Twitter>();
			
			return twitter;
		}
	}
#endif
	
	/// <summary>
	/// Returns platform specific interface to access Native UI feature.
	/// </summary>
	public static UI UI 		
	{ 
		get
		{
			NPBinding	_sharedInstance	= NPBinding.Instance;
			
			if (_sharedInstance == null)
				return null;
			
			if (userInterface == null)
				userInterface	= _sharedInstance.AddComponentBasedOnPlatformOnlyIfRequired<UI>();
				
			return userInterface;
		}
	}
	
	/// <summary>
	/// Returns platform specific interface to access Utility functions.
	/// </summary>
	public static Utility Utility 		
	{ 
		get
		{
			NPBinding	_sharedInstance	= NPBinding.Instance;
			
			if (_sharedInstance == null)
				return null;
			
			if (utility == null)
				utility	= _sharedInstance.CachedGameObject.AddComponentIfNotFound<Utility>();
				
			return utility;
		}
	}

#if USES_WEBVIEW
	/// <summary>
	/// Returns platform specific interface to access Native WebView feature.
	/// </summary>
	public static WebViewNative WebView 			
	{ 
		get
		{	
			NPBinding	_sharedInstance	= NPBinding.Instance;
			
			if (_sharedInstance == null)
				return null;

			if (webview == null)
				webview	= _sharedInstance.CachedGameObject.AddComponentIfNotFound<WebViewNative>();
			
			return webview;
		}
	}
#endif

#if USES_SOOMLA_GROW
	public static SoomlaGrowService SoomlaGrowService 			
	{ 
		get
		{
			NPBinding	_sharedInstance	= NPBinding.Instance;

			if (_sharedInstance == null)
				return null;
			
			if (soomlaGrowService == null)
				soomlaGrowService	= _sharedInstance.AddComponentBasedOnPlatformOnlyIfRequired<SoomlaGrowService>();
			
			return soomlaGrowService;
		}
	}
#endif
	
	#endregion
	
	#region Overriden Methods
	
	protected override void Init ()
	{
		base.Init ();
		
		// Not interested in non singleton instance
		if (instance != this)
			return;

		// Create compoennts 
#if USES_ADDRESS_BOOK
		if (addressBook == null)
			addressBook		= AddComponentBasedOnPlatformOnlyIfRequired<AddressBook>();
#endif

#if USES_BILLING
		if (billing == null)
			billing			= AddComponentBasedOnPlatformOnlyIfRequired<Billing>();
#endif

#if USES_CLOUD_SERVICES
		if (cloudServices == null)
			cloudServices	= AddComponentBasedOnPlatformOnlyIfRequired<CloudServices>();
#endif
	
#if USES_GAME_SERVICES
		if (gameServices == null)
			gameServices	= AddComponentBasedOnPlatformOnlyIfRequired<GameServices>();
#endif

#if USES_MEDIA_LIBRARY
		if (mediaLibrary == null)
			mediaLibrary	= AddComponentBasedOnPlatformOnlyIfRequired<MediaLibrary>();
#endif

#if USES_NETWORK_CONNECTIVITY
		if (networkConnectivity == null)
			networkConnectivity		= AddComponentBasedOnPlatformOnlyIfRequired<NetworkConnectivity>();
#endif

#if USES_NOTIFICATION_SERVICE
		if (notificationService == null)
			notificationService		= CachedGameObject.AddComponentIfNotFound<NotificationService>();
#endif

#if USES_SHARING
		if (sharing == null)
			sharing			= AddComponentBasedOnPlatformOnlyIfRequired<Sharing>();
#endif

#if USES_TWITTER
		if (twitter == null)
			twitter			= AddComponentBasedOnPlatformOnlyIfRequired<Twitter>();
#endif

		if (userInterface == null)
			userInterface	= AddComponentBasedOnPlatformOnlyIfRequired<UI>();

		if (utility == null)
			utility			= CachedGameObject.AddComponentIfNotFound<Utility>();

#if USES_WEBVIEW
		if (webview == null)
			webview			= CachedGameObject.AddComponentIfNotFound<WebViewNative>();
#endif

#if USES_SOOMLA_GROW
		if (soomlaGrowService == null)
			soomlaGrowService	= AddComponentBasedOnPlatformOnlyIfRequired<SoomlaGrowService>();
#endif
	}
	
	#endregion

	#region Create Methods

	private T AddComponentBasedOnPlatformOnlyIfRequired <T> () where T : MonoBehaviour
	{
		T			_existingComponent	= GetComponent<T>();

		if (_existingComponent != null)
			return _existingComponent;

		// Until now this component hasn't been added so let's add it now
		System.Type _basicType		= typeof(T);
		string 		_baseTypeName	= _basicType.ToString();
		
		string 		_platformSpecificTypeName	= null;
		
#if UNITY_EDITOR
		_platformSpecificTypeName	= _baseTypeName + "Editor";	
#elif UNITY_IOS 
		_platformSpecificTypeName	= _baseTypeName + "IOS";	
#elif UNITY_ANDROID
		_platformSpecificTypeName	= _baseTypeName + "Android";
#endif	

		if (!string.IsNullOrEmpty(_platformSpecificTypeName))
		{
#if !NETFX_CORE
			System.Type _platformSpecificClassType	= _basicType.Assembly.GetType(_platformSpecificTypeName, false);
#else
			System.Type _platformSpecificClassType	= _basicType;
#endif

			return CachedGameObject.AddComponent(_platformSpecificClassType) as T;
		}

		return CachedGameObject.AddComponent<T>();
	}

	#endregion
}
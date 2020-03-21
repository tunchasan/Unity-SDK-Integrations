#if UNITY_EDITOR && !(UNITY_WINRT || UNITY_WEBPLAYER || UNITY_WEBGL)
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using VoxelBusters.Utility;
using VoxelBusters.UASUtils;
using VoxelBusters.ThirdParty.XUPorter;

using PlayerSettings	= UnityEditor.PlayerSettings;
using Features			= VoxelBusters.NativePlugins.ApplicationSettings.Features;
using AddonServices		= VoxelBusters.NativePlugins.ApplicationSettings.AddonServices;

namespace VoxelBusters.NativePlugins
{
	using Internal;

	public class PostProcessBuild
	{
		#region Constants

		// File folders
		private const string	kRelativePathNativePluginsFolder		= Constants.kPluginAssetsPath;
		private	const string	kRelativePathIOSNativeCodeFolder		= kRelativePathNativePluginsFolder + "/Plugins/NativeIOSCode";
		private const string	kRelativePathXcodeModDataCollectionFile	= kRelativePathNativePluginsFolder + "/XcodeModifier.txt";
		private const string 	kRelativePathInfoPlistFile				= "Info.plist";
		private const string 	kRelativePathInfoPlistBackupFile		= "Info.backup.plist";
		private	const string	kRelativePathNativePluginsSDKFolder		= "NativePlugins";

		// Mod keys
		private	const string	kModKeyAddressBook						= "NativePlugins-AddressBook";
		private	const string	kModKeyBilling							= "NativePlugins-Billing";
		private	const string	kModKeyCloudServices					= "NativePlugins-CloudServices";
		private	const string	kModKeyCommon							= "NativePlugins-Common";
		private	const string	kModKeyGameServices						= "NativePlugins-GameServices";
		private	const string	kModKeyMediaLibrary						= "NativePlugins-MediaLibrary";
		private	const string	kModKeyNetworkConnectivity				= "NativePlugins-NetworkConnectivity";
		private	const string	kModKeyNotification						= "NativePlugins-Notification";
		private	const string	kModKeySharing							= "NativePlugins-Sharing";
		private	const string	kModKeyTwitter							= "NativePlugins-Twitter";
		private	const string	kModKeyTwitterSDK						= "NativePlugins-TwitterSDK";
		private	const string	kModKeyWebView							= "NativePlugins-WebView";
		private	const string	kModKeySoomlaGrow						= "NativePlugins-SoomlaGrow";
		private	const string	kModKeyStoreReview 						= "NativePlugins-StoreReview";

		// PlayerPrefs keys
		private	const string	kTwitterConfigKey						= "twitter-config";

		// Fabric data
		private const string 	kFabricKitJsonStringFormat				= "{{\"Fabric\":{{\"APIKey\":\"{0}\",\"Kits\":[{{\"KitInfo\":{{\"consumerKey\":\"\",\"consumerSecret\":\"\"}},\"KitName\":\"Twitter\"}}]}}}}";

		// Pch file modification
		private const string 	kPrecompiledFileRelativeDirectoryPath	= "Classes/";
		private const string 	kPrecompiledHeaderExtensionPattern		= "*.pch";
		private const string	kPCHInsertHeaders						= "#ifdef __OBJC__\n\t#import \"Defines.h\"\n#endif\n";

		#endregion

		#region Static Fields

		private static Plist				infoPlist					= null;

		#endregion

		#region Methods

		[PostProcessBuild(0)]
		public static void OnPostProcessBuildActionStart(BuildTarget target, string buildPath)
		{
			string 	_targetStr	= target.ToString();
			if (_targetStr.Equals("iOS") || _targetStr.Equals("iPhone"))
			{
				ExecutePostProcessAction(buildPath);
				return;
			}
		}

		[PostProcessBuild(int.MaxValue-1000)]
		public static void OnPostProcessBuildActionFinish(BuildTarget target, string buildPath)
		{
			string 	_targetStr	= target.ToString();
			if (_targetStr.Equals("iOS") || _targetStr.Equals("iPhone"))
			{
				CleanupProject();
				return;
			}
		}

		private static void ExecutePostProcessAction(string buildPath)
		{
			// Load plist
			string 	_infoPlistFilePath	= GetInfoPlistFilePath(buildPath);
			infoPlist					= Plist.LoadPlistAtPath(_infoPlistFilePath);

			// Prepare project
			CleanupProject();
			CreateTempFolder();

			Features _supportedFeatures	= NPSettings.Application.SupportedFeatures;
			if (_supportedFeatures.UsesTwitter)
				DecompressTwitterSDKFiles();

			if (_supportedFeatures.UsesBilling)
				AddBuildInfoToBillingClass();

			// execute actions specific to features used within app
			GenerateXcodeModFiles();
			UpdateInfoPlist(buildPath);
			ModifyPchFile(buildPath);

			// release properties
			infoPlist 	= null;
		}

		private static void CleanupProject()
		{
			string[] _files = Directory.GetFiles(kRelativePathIOSNativeCodeFolder, "*.xcodemods", SearchOption.AllDirectories);
			foreach (string _path in _files)
			{
				File.SetAttributes(_path, FileAttributes.Normal);
				File.Delete(_path);

				string	_metaFilePath	= _path + ".meta";
				if (File.Exists(_metaFilePath))
				{
					File.SetAttributes(_metaFilePath, FileAttributes.Normal);
					File.Delete(_metaFilePath);
				}
			}
		}

		private static void CreateTempFolder()
		{
			// prepare the folder to contain generated files
			if (Directory.Exists(kRelativePathNativePluginsSDKFolder))
			{
				IOExtensions.AssignPermissionRecursively(kRelativePathNativePluginsSDKFolder, FileAttributes.Normal);
				Directory.Delete(kRelativePathNativePluginsSDKFolder, true);
			}
			Directory.CreateDirectory(kRelativePathNativePluginsSDKFolder);
		}

		private static void DecompressTwitterSDKFiles()
		{
			string	_projectPath					= AssetDatabaseUtils.GetProjectPath();
			string	_twitterNativeCodeFolderPath	= Path.Combine(_projectPath, kRelativePathIOSNativeCodeFolder + "/Twitter");

			if (!Directory.Exists(_twitterNativeCodeFolderPath))
				return;

			foreach (string _filePath in Directory.GetFiles(_twitterNativeCodeFolderPath, "*.gz", SearchOption.AllDirectories))
				Zip.DecompressToDirectory(_filePath, kRelativePathNativePluginsSDKFolder);
		}

		private static void AddBuildInfoToBillingClass()
		{
			#if USES_BILLING
			string		_rvFilePath	= Path.Combine(kRelativePathIOSNativeCodeFolder, "Billing/Source/ReceiptVerification/Manager/ReceiptVerificationManager.m");
			string[]	_contents	= File.ReadAllLines(_rvFilePath);
			int			_lineCount	= _contents.Length;

			// find the line which needs to be updated
			int	_targetLineIndex	= -1;
			for (int _iter = 0; _iter < _lineCount; _iter++)
			{
				string	_curLine	= _contents[_iter];
				if (!_curLine.StartsWith("const"))
					continue;

				if (_curLine.Contains("bundleIdentifier"))
				{
					_targetLineIndex = _iter;
					break;
				}
			}

			if (_targetLineIndex != -1)
			{
				// get the value corresponding to the player preference
				const string _kBundleVersionKey		= "CFBundleVersion";

				string	_bundleIdentifier	= "nil";
				string	_bundleVersion		= "nil";
				if (NPSettings.Billing.iOS.MakeCopyOfBuildInfo)
				{
#if UNITY_5_6_OR_NEWER
					_bundleIdentifier		= string.Format("@\"{0}\"", PlayerSettings.applicationIdentifier);
#else
					_bundleIdentifier		= string.Format("@\"{0}\"", PlayerSettings.bundleIdentifier);
#endif
					_bundleVersion 			= string.Format("@\"{0}\"", infoPlist[_kBundleVersionKey]);
				}

				// replace the existing details with new
				_contents[_targetLineIndex]		= string.Format("const NSString *bundleIdentifier\t= {0};", _bundleIdentifier);
				_contents[_targetLineIndex + 1]	= string.Format("const NSString *bundleVersion\t\t= {0};", _bundleVersion);

				// commit the new changes
				File.WriteAllLines(_rvFilePath, _contents);
			}
			#endif
		}

		private static void	GenerateXcodeModFiles()
		{
			string	_modDataStr	= File.ReadAllText(kRelativePathXcodeModDataCollectionFile);
			if (_modDataStr == null)
				throw new System.IO.FileNotFoundException("Couldn't find mod data file.");

			// create mod file related to supported features
			Dictionary<string, object>	_modDataDict			= (Dictionary<string, object>)JSONUtility.FromJSON(_modDataStr);
			Features 					_supportedFeatures		= NPSettings.Application.SupportedFeatures;
			AddonServices 				_supportedAddonServices	= NPSettings.Application.SupportedAddonServices;

			ExtractAndSerializeXcodeModInfo(_modDataDict,		kModKeyCommon, 			kRelativePathIOSNativeCodeFolder);

			if (_supportedFeatures.UsesAddressBook)
				ExtractAndSerializeXcodeModInfo(_modDataDict,	kModKeyAddressBook,		kRelativePathIOSNativeCodeFolder);

			if (_supportedFeatures.UsesBilling)
				ExtractAndSerializeXcodeModInfo(_modDataDict,	kModKeyBilling, 		kRelativePathIOSNativeCodeFolder);

			if (_supportedFeatures.UsesCloudServices)
				ExtractAndSerializeXcodeModInfo(_modDataDict,	kModKeyCloudServices, 	kRelativePathIOSNativeCodeFolder);

			if (_supportedFeatures.UsesGameServices)
				ExtractAndSerializeXcodeModInfo(_modDataDict,	kModKeyGameServices, 	kRelativePathIOSNativeCodeFolder);

			if (_supportedFeatures.UsesMediaLibrary)
				ExtractAndSerializeXcodeModInfo(_modDataDict,	kModKeyMediaLibrary, 	kRelativePathIOSNativeCodeFolder);

			if (_supportedFeatures.UsesNetworkConnectivity)
				ExtractAndSerializeXcodeModInfo(_modDataDict,	kModKeyNetworkConnectivity, kRelativePathIOSNativeCodeFolder);

			if (_supportedFeatures.UsesNotificationService)
				ExtractAndSerializeXcodeModInfo(_modDataDict,	kModKeyNotification, 	kRelativePathIOSNativeCodeFolder);

			if (_supportedFeatures.UsesSharing)
				ExtractAndSerializeXcodeModInfo(_modDataDict,	kModKeySharing, 		kRelativePathIOSNativeCodeFolder);

			if (_supportedFeatures.UsesTwitter)
			{
				ExtractAndSerializeXcodeModInfo(_modDataDict,	kModKeyTwitter, 		kRelativePathIOSNativeCodeFolder);
				ExtractAndSerializeXcodeModInfo(_modDataDict,	kModKeyTwitterSDK,		kRelativePathNativePluginsSDKFolder);
			}

			if (_supportedFeatures.UsesWebView)
				ExtractAndSerializeXcodeModInfo(_modDataDict,	kModKeyWebView, 		kRelativePathIOSNativeCodeFolder);

			// Create mod file related to supported addon features
			if (_supportedAddonServices.UsesSoomlaGrow)
				ExtractAndSerializeXcodeModInfo(_modDataDict, 	kModKeySoomlaGrow,		kRelativePathIOSNativeCodeFolder);

			if (NPSettings.Utility.RateMyApp.IsEnabled)
				ExtractAndSerializeXcodeModInfo(_modDataDict, 	kModKeyStoreReview,		kRelativePathIOSNativeCodeFolder);
		}

//		{
//			"Fabric": {
//				"APIKey": "{0}",
//				"Kits": [
//				    {
//					"KitInfo": {
//						"consumerKey": "",
//						"consumerSecret": ""
//					},
//					"KitName": "Twitter"
//				    }
//				    ]
//			}
//		}

		private static void UpdateInfoPlist(string buildPath)
		{
			Debug.Log ("[PostProcessBuild] : ModifyInfoPlist : " + buildPath);

			Dictionary<string, object> 	_newPermissionsDict		= new Dictionary<string, object>();

			// In this section, we add additional information required for proper functioning of used native features
			ApplicationSettings _applicationSettings 	= NPSettings.Application;
			Features 			_supportedFeatures		= _applicationSettings.SupportedFeatures;
			#if USES_TWITTER
			if (_supportedFeatures.UsesTwitter)
			{
				const string 	_kFabricKitRootKey 		= "Fabric";

				TwitterSettings _twitterSettings		= NPSettings.SocialNetworkSettings.TwitterSettings;
				string 			_fabricJsonStr			= string.Format(kFabricKitJsonStringFormat, _twitterSettings.ConsumerKey);

				IDictionary 	_fabricJsonDictionary	= (IDictionary)JSONUtility.FromJSON(_fabricJsonStr);
				_newPermissionsDict[_kFabricKitRootKey]	= _fabricJsonDictionary[_kFabricKitRootKey];
			}
			#endif

			// In this section, we add extra flags required as per Apple guidelines
			if (_supportedFeatures.UsesNotificationService)
			{
				if (_supportedFeatures.NotificationService.usesRemoteNotification)
				{
					const string	_kUIBackgroundModesKey		= "UIBackgroundModes";
					IList	_backgroundModesList				= AddUniqueValues(sourceList: (IList)infoPlist.GetKeyPathValue(_kUIBackgroundModesKey),
					                                                              values: "remote-notification");
					_newPermissionsDict[_kUIBackgroundModesKey]	= _backgroundModesList;
				}
			}

			if (_supportedFeatures.UsesGameServices)
			{
				const string	_kDeviceCapablitiesKey	= "UIRequiredDeviceCapabilities";
				IList			_deviceCapablitiesList	= AddUniqueValues(sourceList: (IList)infoPlist.GetKeyPathValue(_kDeviceCapablitiesKey),
				                                                          values: "gamekit");
				_newPermissionsDict[_kDeviceCapablitiesKey]	= _deviceCapablitiesList;
			}

			if (_supportedFeatures.UsesSharing)
			{
				const string	_kQuerySchemesKey		= "LSApplicationQueriesSchemes";
				IList			_queriesSchemesList		= AddUniqueValues(sourceList: (IList)infoPlist.GetKeyPathValue(_kQuerySchemesKey),
				                                                          values: new string[] { "whatsapp", "fb", "twitter" });
				_newPermissionsDict[_kQuerySchemesKey]	= _queriesSchemesList;
			}

			if (_supportedFeatures.UsesNetworkConnectivity || _supportedFeatures.UsesWebView)
			{
				const string	_kATSKey				= "NSAppTransportSecurity";
				const string	_karbitraryLoadsKey		= "NSAllowsArbitraryLoads";

				IDictionary		_transportSecurityDict	= (IDictionary)infoPlist.GetKeyPathValue(_kATSKey);
				if (_transportSecurityDict == null)
					_transportSecurityDict				= new Dictionary<string, object>();

				_transportSecurityDict[_karbitraryLoadsKey]	= true.ToString();
				_newPermissionsDict[_kATSKey]				= _transportSecurityDict;
			}

			// Add privacy info
			const string	_kPermissionContacts			= "NSContactsUsageDescription";
			const string	_kPermissionCamera				= "NSCameraUsageDescription";
			const string	_kPermissionPhotoLibrary		= "NSPhotoLibraryUsageDescription";
			const string	_kPermissionModifyPhotoLibrary	= "NSPhotoLibraryAddUsageDescription";

			if (_supportedFeatures.UsesAddressBook)
			{
				_newPermissionsDict[_kPermissionContacts]	= _applicationSettings.IOS.AddressBookUsagePermissionDescription;
			}

			if (_supportedFeatures.UsesMediaLibrary)
			{
				if (_supportedFeatures.MediaLibrary.usesCamera)
					_newPermissionsDict[_kPermissionCamera]			= _applicationSettings.IOS.CameraUsagePermissionDescription;

				if (_supportedFeatures.MediaLibrary.usesPhotoAlbum)
				{
					_newPermissionsDict [_kPermissionPhotoLibrary]			= _applicationSettings.IOS.PhotoAlbumUsagePermissionDescription;
					_newPermissionsDict [_kPermissionModifyPhotoLibrary]	= _applicationSettings.IOS.PhotoAlbumModifyUsagePermissionDescription;
				}
			}

			if (_supportedFeatures.UsesSharing)
			{
				_newPermissionsDict [_kPermissionModifyPhotoLibrary]	= _applicationSettings.IOS.PhotoAlbumModifyUsagePermissionDescription;
			}

			if (_newPermissionsDict.Count == 0)
				return;

			// Create a backup of old plist
			string	_infoPlistBackupSavePath	= GetInfoPlistBackupFilePath(buildPath);
			infoPlist.Save(_infoPlistBackupSavePath);

			// Save the plist with new permissions
			foreach (string _key in _newPermissionsDict.Keys)
				infoPlist.AddValue(_key, _newPermissionsDict[_key]);

			string	_infoPlistSavePath			= GetInfoPlistFilePath(buildPath);
			infoPlist.Save(_infoPlistSavePath);
		}

		private static void ModifyPchFile(string buildPath)
		{
			string 		_pchFileDirectory	= Path.Combine(buildPath, kPrecompiledFileRelativeDirectoryPath);
			string[] 	_pchFiles 			= Directory.GetFiles(_pchFileDirectory, kPrecompiledHeaderExtensionPattern);
			string 		_pchFilePath 		= null;

			// Check whether file exists
			if (_pchFiles.Length > 0)
				_pchFilePath =  _pchFiles[0];

			if (File.Exists(_pchFilePath))
			{
				string 	_fileContents 		= File.ReadAllText(_pchFilePath);
				if (!_fileContents.Contains("Defines.h"))
				{
					string 	_updatedContents	= _fileContents + "\n\n" + kPCHInsertHeaders;
					File.WriteAllText(_pchFilePath, _updatedContents);
				}
			}
		}

		#endregion

		#region Misc Methods

		private static void ExtractAndSerializeXcodeModInfo(Dictionary<string, object> modifierCollectionDict, string key, string relativePath)
		{
			object _modifierData;
			if (modifierCollectionDict.TryGetValue(key, out _modifierData))
			{
				string	_modifierFileName	= key + ".xcodemods";
				File.WriteAllText(Path.Combine(relativePath, _modifierFileName), JSONUtility.ToJSON(_modifierData));

				return;
			}

			DebugUtility.Logger.Log("Couldn't create modifier file for key: " + key);
		}

		private static IList AddUniqueValues<T>(IList sourceList, params T[] values)
		{
			if (sourceList == null)
				sourceList = new List<T>();

			foreach (T _value in values)
			{
				if (sourceList.Contains(_value))
					continue;

				sourceList.Add(_value);
			}

			return sourceList;
		}

		private static string GetInfoPlistFilePath (string _buildPath)
		{
			return Path.Combine(_buildPath, kRelativePathInfoPlistFile);
		}

		private static string GetInfoPlistBackupFilePath (string _buildPath)
		{
			return Path.Combine(_buildPath, kRelativePathInfoPlistBackupFile);
		}

		#endregion
	}
}
#endif

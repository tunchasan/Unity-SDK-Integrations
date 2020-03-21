using UnityEngine;
using System.Collections;

namespace VoxelBusters.NativePlugins.Internal
{
    public class Constants : MonoBehaviour
    {
        #region Errors

        public const string kDebugTag = "Native Plugins";
        public const string kNotSupportedInEditor = "The operation could not be completed because the requested feature is not simulated in Unity Editor. Use your mobile device for testing this functionality.";
        public const string kiOSFeature = "The operation could not be completed because the requested feature is supported only on iOS platform.";
        public const string kAndroidFeature = "The operation could not be completed because the requested feature is supported only on Android platform.";
        public const string kNotSupported = "The operation could not be completed because the requested feature is not supported.";

        #endregion

        #region Assets Path	

        public const string kRootAssetsPath = "Assets";
        public const string kVBCodebasePath = "Assets/Standard Assets/VoxelBusters";
        public const string kVBExternalCodebasePath = "Assets/VoxelBusters";
        public const string kAndroidPluginsPath = "Assets/Plugins/Android";

        public const string kPluginAssetsPath = kVBCodebasePath + "/NativePlugins";
        public const string kEditorAssetsPath = kPluginAssetsPath + "/EditorResources";
        public const string kLogoPath = kEditorAssetsPath + "/Logo/NativePlugins.png";

        // Default resources
        public const string kDefaultResourcesPath = "Default";
        public const string kDefaultContactImagePath = kDefaultResourcesPath + "/ContactImage";

        #endregion

        #region GUI Style

        public const string kSampleUISkin = "AssetStoreProductUISkin";//Available in AssetStoreProduct submodule
        public const string kSubTitleStyle = "sub-title";
        public const string kButtonLeftStyle = "ButtonLeft";
        public const string kButtonMidStyle = "ButtonMid";
        public const string kButtonRightStyle = "ButtonRight";

        #endregion

        #region Asset store

        public const string kAssetStorePath = "http://bit.ly/1Fnpb5j";
        public const string kPurchaseFullVersionButton = "Purchase Full Version";
        public const string kFeatureNotSupportedInLiteVersion = "Feature not supported in Lite version. Please purchase full version of Native Plugins.";

        #endregion

        #region Plugin Paths
        public const string kAndroidPluginsRootPath = kRootAssetsPath + "/Plugins/Android";
        public const string kAndroidPluginsCPNPPath = kAndroidPluginsRootPath + "/native_plugins_lib";
        public const string kAndroidPluginsCPNPJARPath = kAndroidPluginsCPNPPath + "/libs";

        #endregion

        #region Game Services 

        public const string kGameServicesUserAuthMissingError = "The requested operation could not be completed because local player has not been authenticated.";
        public const string kGameServicesIdentifierNullError = "The requested operation could not be completed because identifier is null.";
        public const string kGameServicesIdentifierInfoNotFoundError = "The requested operation could not be completed because identifier records are not found.";

        #endregion

        #region PlayMaker

        public const string kPlayMakerDateTimeFormat = "yyyy-MM-dd HH:mm";

        #endregion

        #region Plugin Android Jar Files / Library Names

        public const string kAddressBookJARName = "feature.addressbook";
        public const string kBillingJARName = "feature.billing";
        public const string kBillingAmazonJARName = "feature.billing.amazon";
        public const string kCloudServicesJARName = "feature.cloudservices";
        public const string kGameServicesJARName = "feature.gameservices";
        public const string kMediaLibraryJARName = "feature.medialibrary";
        public const string kNotificationJARName = "feature.notification";
        public const string kNetworkConnectivityJARName = "feature.reachability";
        public const string kSharingJARName = "feature.sharing";
        public const string kSoomlaIntegrationJARName = "feature.sdk.soomla.integration";
        public const string kSocialNetworkTwitterJARName = "feature.socialnetwork.twitter";
        public const string kWebviewJARName = "feature.webview";
        public const string kExternalNotificationLibJARName = "feature.externallibrary.shortcutbadger";

        public const string kTwitterLibraryName = "twitter_lib";
        public const string kYoutubeLibraryName = "youtube_lib";


        #endregion

        #region Plugin Editor Flag Keys

        public const string kPrefsKeyResolveAndroidDependencies = "np-resolve-android-dependencies";

        #endregion

        #region Asset Store Constants

        public const string kFullVersionProductURL = "https://www.assetstore.unity3d.com/en/#!/account/downloads/search=Cross Platform Native Plugins";
        public const string kLiteVersionProductURL = "https://www.assetstore.unity3d.com/en/#!/account/downloads/search=Cross Platform Native Plugins - Lite Version";
        public const string kProductURL =
#if NATIVE_PLUGINS_LITE_VERSION
			kLiteVersionProductURL
#else
            kFullVersionProductURL
#endif
            ;

        #endregion

        #region Update Check Constants

        public const string kNotifiedVersionKey = "native-plugins-notified-version";
        public const string kNativePluginsAssetStoreVersionKey = "native-plugins-asset-store-version";
        public const string kVBNewsKey = "native-plugins-news";

        #endregion
    }
}
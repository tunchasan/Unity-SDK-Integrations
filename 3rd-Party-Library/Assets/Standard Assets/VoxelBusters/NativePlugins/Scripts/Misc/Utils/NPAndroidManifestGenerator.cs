using UnityEngine;
using System.Collections;
using VoxelBusters.Utility;

#if UNITY_EDITOR
using System.Xml;

using PlayerSettings = VoxelBusters.Utility.PlayerSettings;

namespace VoxelBusters.NativePlugins
{
	public class NPAndroidManifestGenerator : AndroidManifestGenerator
	{
		#region Properties

		private			ApplicationSettings.Features		m_supportedFeatures;

		#endregion

		#region Constructors

		public NPAndroidManifestGenerator ()
		{
			m_supportedFeatures	= NPSettings.Application.SupportedFeatures;
		}

		#endregion

		#region Application Methods

		protected override void WriteApplicationProperties (XmlWriter _xmlWriter)
		{
			WriteActivityInfo(_xmlWriter);
			WriteProviderInfo(_xmlWriter);
			WriteReceiverInfo(_xmlWriter);
			WriteServiceInfo(_xmlWriter);
			WriteMetaInfo(_xmlWriter);
		}

		private void WriteActivityInfo (XmlWriter _xmlWriter)
		{
			#if !NATIVE_PLUGINS_LITE_VERSION
			// Billing
			if (m_supportedFeatures.UsesBilling)
			{
				WriteActivity(_xmlWriter:		_xmlWriter,
				              _name:			"com.voxelbusters.nativeplugins.features.billing.serviceprovider.google.GoogleBillingActivity",
						      _theme:			"@style/FloatingActivityTheme",
				              _comment:			"Billing : Activity used for purchase view");

			}

			// Media
			if (m_supportedFeatures.UsesMediaLibrary)
			{
				WriteActivity(_xmlWriter:		_xmlWriter,
				              _name:			"com.voxelbusters.nativeplugins.features.medialibrary.MediaLibraryActivity",
				              _theme:			"@style/FloatingActivityTheme",
				              _orientation:		"sensor",
				              _configChanges:	"keyboardHidden|orientation|screenSize",
				              _comment:			"MediaLibrary : Generic helper activity");

				WriteActivity(_xmlWriter:		_xmlWriter,
				              _name:			"com.voxelbusters.nativeplugins.features.medialibrary.GalleryVideoLauncherActivity",
				              _theme:			"@style/FloatingActivityTheme",
				              _orientation:		"sensor",
				              _comment:			"MediaLibrary : Used for Launching video from gallery");

				WriteActivity(_xmlWriter:		_xmlWriter,
				              _name:			"com.voxelbusters.nativeplugins.features.medialibrary.YoutubePlayerActivity",
				              _comment:			"MediaLibrary : Youtube player activity");
			}

			// Notifications
			if (m_supportedFeatures.UsesNotificationService)
			{
				WriteActivity(_xmlWriter:		_xmlWriter,
				              _name:			"com.voxelbusters.nativeplugins.features.notification.core.ApplicationLauncherFromNotification",
				              _theme:			"@style/FloatingActivityTheme",
				              _exported:		"true",
				              _comment:			"Application Launcher - Notifications : Used as a proxy to capture triggered notification.");
			}


			// Twitter
			if (m_supportedFeatures.UsesTwitter)
			{
				WriteActivity(_xmlWriter:		_xmlWriter,
				              _name:			"com.voxelbusters.nativeplugins.features.socialnetwork.twitter.TwitterHelperActivity",
				              _theme:			"@style/FloatingActivityTheme",
				              _comment:			"SocialNetworking - Twitter : Generic helper activity");
			}

			if (m_supportedFeatures.UsesGameServices)
			{
				WriteActivity(_xmlWriter:		_xmlWriter,
				              _name:			"com.voxelbusters.nativeplugins.features.gameservices.serviceprovider.google.GooglePlayGameUIActivity",
				              _theme:			"@style/FloatingActivityTheme",
				              _comment:			"Game Play Services helper activity");
			}

			#endif

			// Sharing
			if (m_supportedFeatures.UsesSharing)
			{
				WriteActivity(_xmlWriter:		_xmlWriter,
				              _name:			"com.voxelbusters.nativeplugins.features.sharing.SharingActivity",
				              _theme:			"@style/FloatingActivityTheme",
				              _comment:			"Sharing");
			}

			if (m_supportedFeatures.UsesWebView)
			{
				WriteActivity(_xmlWriter:		_xmlWriter,
				              _name:			"com.voxelbusters.nativeplugins.features.webview.FileChooserActivity",
				              _theme:			"@style/FloatingActivityTheme",
				              _comment:			"Webview : For File Choosing");
				WriteActivity(_xmlWriter:		_xmlWriter,
					_name:			"com.voxelbusters.nativeplugins.features.webview.WebviewActivity",
					_theme:			"@style/FloatingActivityTheme",
					_comment:			"Webview : For showing full screen webview's in a different new activity");
			}

			WriteActivity(_xmlWriter:		_xmlWriter,
				_name:			"com.voxelbusters.nativeplugins.features.medialibrary.CameraActivity",
				_theme:			"@style/FloatingActivityTheme",
				_comment:			"Media Library : For custom camera access");


			//Common required activities

			//UIActivity
			WriteActivity(_xmlWriter:		_xmlWriter,
			              _name:			"com.voxelbusters.nativeplugins.features.ui.UiActivity",
			              _theme:			"@style/FloatingActivityTheme",
			              _comment:			"UI  : Generic helper activity for launching Dialogs");

			WriteActivity(_xmlWriter:		_xmlWriter,
				_name:			"com.voxelbusters.nativeplugins.helpers.PermissionRequestActivity",
				_theme:			"@style/FloatingActivityTheme",
				_comment:			"Game Play Services helper activity");
		}

		private void WriteProviderInfo (XmlWriter _xmlWriter)
		{
			// Provider
			_xmlWriter.WriteComment("Custom File Provider. Sharing from internal folders  \"com.voxelbusters.nativeplugins.extensions.FileProviderExtended\"");
			_xmlWriter.WriteStartElement("provider");
			{
				WriteAttributeString(_xmlWriter, "android", "name", null, "com.voxelbusters.nativeplugins.extensions.FileProviderExtended");
				WriteAttributeString(_xmlWriter, "android", "authorities", null, string.Format("{0}.fileprovider", PlayerSettings.GetBundleIdentifier()));
				WriteAttributeString(_xmlWriter, "android", "exported", null, "false");
				WriteAttributeString(_xmlWriter, "android", "grantUriPermissions", null, "true");

				_xmlWriter.WriteStartElement("meta-data");
				{
					WriteAttributeString(_xmlWriter, "android", "name", null, "android.support.FILE_PROVIDER_PATHS");
					WriteAttributeString(_xmlWriter, "android", "resource", null, "@xml/nativeplugins_file_paths");
				}
				_xmlWriter.WriteEndElement();
			}
			_xmlWriter.WriteEndElement();
		}

		private void WriteReceiverInfo (XmlWriter _xmlWriter)
		{
			#if !NATIVE_PLUGINS_LITE_VERSION

			if (m_supportedFeatures.UsesBilling)
			{
				_xmlWriter.WriteComment("Billing : Amazon Billing Receiver");
				_xmlWriter.WriteStartElement("receiver");
				{
					WriteAttributeString(_xmlWriter, "android", "name", null, "com.amazon.device.iap.ResponseReceiver");

					_xmlWriter.WriteStartElement("intent-filter");
					{
						WriteAction(_xmlWriter:		_xmlWriter,
						            _name:			"com.amazon.inapp.purchasing.NOTIFY",
						            _permission:	"com.amazon.inapp.purchasing.Permission.NOTIFY"
									);
					}
					_xmlWriter.WriteEndElement();
				}
				_xmlWriter.WriteEndElement();
			}

			// Local notification receiver
			if (m_supportedFeatures.UsesNotificationService)
			{
				_xmlWriter.WriteComment("Notifications : Receiver for alarm to help Local Notifications");
				_xmlWriter.WriteStartElement("receiver");
				{
					WriteAttributeString(_xmlWriter, "android", "name", null, "com.voxelbusters.nativeplugins.features.notification.core.AlarmEventReceiver");
				}
				_xmlWriter.WriteEndElement();
			}
			#endif
		}

		private void WriteServiceInfo (XmlWriter _xmlWriter)
		{
			#if !NATIVE_PLUGINS_LITE_VERSION
			if (m_supportedFeatures.UsesNotificationService)
			{
                WriteService(_xmlWriter: _xmlWriter,
                    _name: "com.voxelbusters.nativeplugins.features.notification.serviceprovider.fcm.FCMMessagingService",
                    _permission: null,
                    _intentFilterAction:"com.google.firebase.MESSAGING_EVENT",
                    _comment: "Notifications : Firebase Cloud Messaging Service");
			}
			#endif
		}

        private void WriteMetaInfo(XmlWriter _xmlWriter)
        {
#if USES_GAME_SERVICES
            _xmlWriter.WriteStartElement("meta-data");
            {
                WriteAttributeString(_xmlWriter, "android", "name", null, "com.google.android.gms.games.APP_ID");
                WriteAttributeString(_xmlWriter, "android", "value", null, string.Format("\\u003{0}", NPSettings.GameServicesSettings.Android.PlayServicesApplicationID));// Space Added because its getting considered as integer when added from xml instead of string.
            }
            _xmlWriter.WriteEndElement();
#endif

#if USES_NOTIFICATION_SERVICE
            _xmlWriter.WriteStartElement("meta-data");
            {
                WriteAttributeString(_xmlWriter, "android", "name", null, "com.google.firebase.messaging.default_notification_icon");
                WriteAttributeString(_xmlWriter, "android", "resource", null, "@drawable/ic_stat_ic_notification");// Space Added because its getting considered as integer when added from xml instead of string.
            }
            _xmlWriter.WriteEndElement();

            _xmlWriter.WriteStartElement("meta-data");
            {
                WriteAttributeString(_xmlWriter, "android", "name", null, "com.google.firebase.messaging.default_notification_color");
                WriteAttributeString(_xmlWriter, "android", "resource", null, "@color/colorAccent");
            }
            _xmlWriter.WriteEndElement();
#endif
        }

		#endregion

		#region Permission Methods

		protected override void WritePermissions (XmlWriter _xmlWriter)
		{
			if (m_supportedFeatures.UsesAddressBook)
			{
				WriteUsesPermission(_xmlWriter:	_xmlWriter,
				                    _name: 		"android.permission.READ_CONTACTS",
				                    _comment: 	"Address Book");
			}

			if (m_supportedFeatures.UsesNetworkConnectivity)
			{

				WriteUsesPermission(_xmlWriter:	_xmlWriter,
				                    _name: 		"android.permission.ACCESS_NETWORK_STATE",
				                    _comment: 	"Network Connectivity");
			}


#if !NATIVE_PLUGINS_LITE_VERSION
			if (m_supportedFeatures.UsesBilling)
			{
				WriteUsesPermission(_xmlWriter:	_xmlWriter,
				                    _name: 		"com.android.vending.BILLING",
				                    _comment: 	"Billing");
			}

			if (m_supportedFeatures.UsesMediaLibrary)
			{
				if (m_supportedFeatures.MediaLibrary.usesCamera)
				{
					WriteUsesPermission(_xmlWriter:	_xmlWriter,
				    	                _name: 		"android.permission.CAMERA",
				        	            _features:	new Feature[] {
													new Feature("android.hardware.camera", false),
													new Feature("android.hardware.camera.autofocus", false)},
				                    _comment:	"Media Library");
				}

				if (m_supportedFeatures.MediaLibrary.usesPhotoAlbum)
				{
					WriteUsesPermission(_xmlWriter:	_xmlWriter,
				                    _name: 		"com.google.android.apps.photos.permission.GOOGLE_PHOTOS");


					WriteUsesPermission(_xmlWriter:	_xmlWriter,
				                    _name: 		"android.permission.MANAGE_DOCUMENTS");
				}
			}

			if (m_supportedFeatures.UsesNotificationService)
			{
				WriteUsesPermission(_xmlWriter:	_xmlWriter,
					_name: 		"android.permission.WAKE_LOCK");

#if USES_NOTIFICATION_SERVICE
				if(NPSettings.Notification.Android.AllowVibration)
				{
					WriteUsesPermission(_xmlWriter:	_xmlWriter,
					                    _name: 		"android.permission.VIBRATE",
					                    _comment: 	"Notifications : If vibration is required for notification");
				}
#endif
			}

			if(m_supportedFeatures.UsesGameServices)
			{
				WriteUsesPermission(_xmlWriter:	_xmlWriter,
					                _name: 		"com.google.android.providers.gsf.permission.READ_GSERVICES",
					                _comment: 	"GameServices : For getting content provider access.");

			}

			if(m_supportedFeatures.UsesWebView)
			{
				// Used for file uploads
				WriteUsesPermission(_xmlWriter:	_xmlWriter,
					_name: 		"android.permission.CAMERA",
					_features:	new Feature[] {
						new Feature("android.hardware.camera", false),
						new Feature("android.hardware.camera.autofocus", false)},
					_comment:	"Webview - Uses for file uploading from camera");

			}

#endif

			//Write common permissions here

			if(m_supportedFeatures.UsesNotificationService || NPSettings.Utility.Android.ModifiesApplicationBadge)
			{
				// For badge permissions
				WriteBadgePermissionsForAllPlatforms(_xmlWriter);
			}

			//Internet access - Add by default as many features need this.
			WriteUsesPermission(_xmlWriter:	_xmlWriter,
			                    _name: 		"android.permission.INTERNET",
			                    _comment:	"Required for internet access");

			#if !NATIVE_PLUGINS_LITE_VERSION
			//Storage Access
			if(m_supportedFeatures.UsesMediaLibrary)
			{
				WriteUsesPermission(_xmlWriter:	_xmlWriter,
				                    _name: 		"android.permission.WRITE_EXTERNAL_STORAGE",
				                    _comment:	"For Saving to external directory - Save to Gallery Feature in MediaLibrary");

				WriteUsesPermission(_xmlWriter:	_xmlWriter,
				                    _name: 		"android.permission.READ_EXTERNAL_STORAGE");
			}
			#endif

		}

		private void WriteBadgePermissionsForAllPlatforms (XmlWriter _xmlWriter)
		{
			WriteUsesPermission(_xmlWriter:	_xmlWriter,
					            _name: "com.sec.android.provider.badge.permission.READ",
			                    _comment: "Notifications : Badge Permission for Samsung Devices");

			WriteUsesPermission(_xmlWriter:	_xmlWriter,
			                    _name: "com.sec.android.provider.badge.permission.WRITE");

			WriteUsesPermission(_xmlWriter:	_xmlWriter,
			                    _name: "com.htc.launcher.permission.READ_SETTINGS",
			                    _comment: "Notifications : Badge Permission for HTC Devices");
			WriteUsesPermission(_xmlWriter:	_xmlWriter,
			                    _name: "com.htc.launcher.permission.UPDATE_SHORTCUT");


			WriteUsesPermission(_xmlWriter:	_xmlWriter,
			                    _name: "com.sonyericsson.home.permission.BROADCAST_BADGE",
			                    _comment: "Notifications : Badge Permission for Sony Devices");
			WriteUsesPermission(_xmlWriter:	_xmlWriter,
			                    _name: "com.sonymobile.home.permission.PROVIDER_INSERT_BADGE");


			WriteUsesPermission(_xmlWriter:	_xmlWriter,
			                    _name: "com.anddoes.launcher.permission.UPDATE_COUNT",
			                    _comment: "Notifications : Badge Permission for Apex Devices");


			WriteUsesPermission(_xmlWriter:	_xmlWriter,
			                    _name: "com.majeur.launcher.permission.UPDATE_BADGE",
			                    _comment: "Notifications : Badge Permission for Solid Devices");

			WriteUsesPermission(_xmlWriter:	_xmlWriter,
			                    _name: "com.huawei.android.launcher.permission.CHANGE_BADGE",
			                    _comment: "Notifications : Badge Permission for Huawei Devices");
			WriteUsesPermission(_xmlWriter:	_xmlWriter,
			                    _name: "com.huawei.android.launcher.permission.READ_SETTINGS");
			WriteUsesPermission(_xmlWriter:	_xmlWriter,
			                    _name: "com.huawei.android.launcher.permission.WRITE_SETTINGS");


			WriteUsesPermission(_xmlWriter:	_xmlWriter,
			                    _name: "android.permission.READ_APP_BADGE",
			                    _comment: "Notifications : Badge Permission for ZUK Devices");


			WriteUsesPermission(_xmlWriter:	_xmlWriter,
			                    _name: "com.oppo.launcher.permission.READ_SETTINGS",
			                    _comment: "Notifications : Badge Permission for Oppo Devices");
			WriteUsesPermission(_xmlWriter:	_xmlWriter,
			                    _name: "com.oppo.launcher.permission.WRITE_SETTINGS");


			WriteUsesPermission(_xmlWriter:	_xmlWriter,
			                    _name: "me.everything.badger.permission.BADGE_COUNT_READ",
			                    _comment: "Notifications : Badge Permission for EverythingMe Support");
			WriteUsesPermission(_xmlWriter:	_xmlWriter,
			                    _name: "me.everything.badger.permission.BADGE_COUNT_WRITE");

		}

		#endregion
	}
}
#endif

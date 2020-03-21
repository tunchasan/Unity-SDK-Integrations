#if UNITY_ANDROID
using System;
using UnityEditor;
using UnityEngine;
using VoxelBusters.NativePlugins.Internal;
using VoxelBusters.Utility;
using System.Collections.Generic;
using System.Xml;
using System.Collections;

namespace VoxelBusters.NativePlugins
{
	/// <summary>
	/// Play-Services Dependencies for Cross Platform Native Plugins.
	/// </summary>
	[InitializeOnLoad]
	public class NPAndroidLibraryDependencies
	{
		/// <summary>
		/// The name of your plugin.  This is used to create a settings file
		/// which contains the dependencies specific to your plugin.
		/// </summary>
		private static readonly string DependencyName = "CPNPDependencies.xml";

        private static readonly string PlayServicesVersionString	=	"16.0+";
        private static readonly string SupportLibsVersionString		=	"27.1+";
        private static readonly string FCMVersionString             =   "17.3.4+";
        private static readonly string GSONVersionString            =   "2.7";

		/// <summary>
		/// Initializes static members of the <see cref="NPAndroidLibraryDependencies"/> class.
		/// </summary>
		static NPAndroidLibraryDependencies()
		{
			EditorUtils.Invoke(()=>{
				CreateDependencyXML(Constants.kPluginAssetsPath + "/Editor/" + DependencyName);
			}, 0.1f);
		}

		private static void CreateDependencyXML(string _path)
		{
			// Settings
			XmlWriterSettings _settings 	= new XmlWriterSettings();
			_settings.Encoding 				= new System.Text.UTF8Encoding(true);
			_settings.ConformanceLevel 		= ConformanceLevel.Document;
			_settings.Indent 				= true;
			_settings.NewLineOnAttributes 	= true;
			_settings.IndentChars 			= "\t";

			// Generate and write dependecies
			using (XmlWriter _xmlWriter = XmlWriter.Create(_path, _settings))
			{
				_xmlWriter.WriteStartDocument();
				{
					_xmlWriter.WriteComment("DONT MODIFY HERE. AUTO GENERATED DEPENDENCIES FROM NPAndroidLibraryDependencies.cs.");

					_xmlWriter.WriteStartElement("dependencies");
					{
						_xmlWriter.WriteStartElement ("androidPackages");
						{
							if (NPSettings.Application.SupportedFeatures.UsesGameServices)
							{
								_xmlWriter.WriteComment("Dependency added for using Google Play Services");

								AndroidDependency _playServicesGamesDependency = new AndroidDependency("com.google.android.gms", "play-services-games", PlayServicesVersionString);
								_playServicesGamesDependency.AddPackageID("extra-google-m2repository");
								_playServicesGamesDependency.AddPackageID("extra-android-m2repository");

								WritePackageDependency(_xmlWriter, _playServicesGamesDependency);

								AndroidDependency _playServicesNearbyDependency = new AndroidDependency("com.google.android.gms", "play-services-nearby", PlayServicesVersionString);
								WritePackageDependency(_xmlWriter, _playServicesNearbyDependency);

                                /*AndroidDependency _playServicesAuthDependency = new AndroidDependency("com.google.android.gms", "play-services-auth", PlayServicesVersionString);
                                WritePackageDependency(_xmlWriter, _playServicesAuthDependency);*/
                            }

							if (NPSettings.Application.SupportedFeatures.UsesNotificationService && NPSettings.Application.SupportedFeatures.NotificationService.usesRemoteNotification)
							{
								_xmlWriter.WriteComment("Dependency added for using Notifications");
                                AndroidDependency _fcmDependency = new AndroidDependency("com.google.firebase", "firebase-messaging", FCMVersionString);
								WritePackageDependency(_xmlWriter, _fcmDependency);
							}

                            if(NPSettings.Application.SupportedFeatures.UsesTwitter)
                            {
                                _xmlWriter.WriteComment("Dependency added for using Twitter Kit");
                                AndroidDependency _gsonDependency = new AndroidDependency("com.google.code.gson", "gson", GSONVersionString);
                                WritePackageDependency(_xmlWriter, _gsonDependency);
                            }

							//https://developer.android.com/topic/libraries/support-library/packages.html
							// Marshmallow permissions requires app-compat. Also used by some old API's for compatibility.

							_xmlWriter.WriteComment("Dependency added for using Support Libraries");

							AndroidDependency _supportLibraryV4Dependency	= new AndroidDependency("com.android.support", "support-v4", SupportLibsVersionString);
							WritePackageDependency(_xmlWriter, _supportLibraryV4Dependency);

							AndroidDependency _supportLibraryExifDependency	= new AndroidDependency("com.android.support", "exifinterface", SupportLibsVersionString);
							WritePackageDependency(_xmlWriter, _supportLibraryExifDependency);

						}
						_xmlWriter.WriteEndElement();
					}
					_xmlWriter.WriteEndElement();
				}
				_xmlWriter.WriteEndDocument();
			}

			/*Google.VersionHandler.InvokeInstanceMethod(
				svcSupport, "DependOn",
				new object[] { 	"com.android.support",
					"appcompat-v7",
					SupportLibsVersionString },
				namedArgs: null
			);*/


		}


		private static void WritePackageDependency(XmlWriter _xmlWriter, AndroidDependency _dependency)
		{
			_xmlWriter.WriteStartElement ("androidPackage");
			{
				_xmlWriter.WriteAttributeString ("spec", String.Format("{0}:{1}:{2}", _dependency.Group, _dependency.Artifact, _dependency.Version));

				List<string> packageIDs = _dependency.PackageIDs;

				if (packageIDs != null)
				{
					_xmlWriter.WriteStartElement ("androidSdkPackageIds");
					{
						foreach(string _each in packageIDs)
						{
							_xmlWriter.WriteStartElement ("androidSdkPackageId");
							{
								_xmlWriter.WriteString (_each);
							}
							_xmlWriter.WriteEndElement ();
						}
					}
					_xmlWriter.WriteEndElement ();
				}

			}
			_xmlWriter.WriteEndElement ();
		}
	}

	public class AndroidDependency
	{
		private string 	m_group;
		private string 	m_artifact;
		private string	m_version;

		private	List<string>	m_packageIDs;


		public string Group
		{
			get
			{
				return m_group;
			}
		}

		public string Artifact
		{
			get
			{
				return m_artifact;
			}
		}

		public string Version
		{
			get
			{
				return m_version;
			}
		}

		public List<string> PackageIDs
		{
			get
			{
				return m_packageIDs;
			}
		}

		public AndroidDependency(string _group, string _artifact, string _version)
		{
			m_group = _group;
			m_artifact = _artifact;
			m_version = _version;
		}

		public void AddPackageID(string _packageID)
		{
			if (m_packageIDs == null)
				m_packageIDs = new List<string>();


			m_packageIDs.Add(_packageID);
		}
	}
}
#endif

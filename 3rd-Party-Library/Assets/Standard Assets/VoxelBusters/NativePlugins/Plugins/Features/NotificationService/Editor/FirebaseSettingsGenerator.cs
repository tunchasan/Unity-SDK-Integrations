using UnityEngine;
using System.Collections;
using VoxelBusters.Utility;
using System.Xml;

#if UNITY_EDITOR 
using UnityEditor;
using System.IO;

namespace VoxelBusters.NativePlugins.Internal
{
    [InitializeOnLoad]
    public class FirebaseSettingsGenerator : AssetPostprocessor
    {

        private const string kGoogleServicesJsonPath                = "Assets/google-services.json";
        private const string kGoogleServicesGeneratedXMLFilePath    = Constants.kAndroidPluginsCPNPPath + "/res/values/google-services.xml";


        //Monitor on Game-Services.json file
#if UNITY_ANDROID
        static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
#if NATIVE_PLUGIN_HIBERNATE
            return;
#endif
            foreach (string each in importedAssets)
            {
                CheckForFileUpdation(kGoogleServicesJsonPath, each);
            }

            foreach (string each in movedAssets)
            {
                CheckForFileUpdation(kGoogleServicesJsonPath, each);
            }

            foreach (string each in deletedAssets)
            {
                CheckForFileUpdation(kGoogleServicesGeneratedXMLFilePath, each);
            }

#if UNITY_ANDROID

            if (NPSettings.Application.SupportedFeatures.UsesNotificationService && NPSettings.Application.SupportedFeatures.NotificationService.usesRemoteNotification)
            {
                if(!IsGoogleServicesJsonFileAvailable())
                    Debug.LogWarning("[Cross Platform Native Plugins] Please add google-services.json in Assets folder for using Firebase Cloud Messaging(Remote Notifications). You can fetch the file from Firebase console under your project -> Project Settings -> General : https://console.firebase.google.com.");
            }

#endif
        }
#endif

        private static void CheckForFileUpdation(string targetFile, string path)
        {
            if (path.Equals(targetFile))
            {
                WriteValuesFile();
            }
        }

        private static bool IsGoogleServicesJsonFileAvailable()
        {
            return FileOperations.Exists(kGoogleServicesJsonPath);
        }

        private static void WriteValuesFile()
        {
            // Settings
            XmlWriterSettings _settings = new XmlWriterSettings();
            _settings.Encoding = new System.Text.UTF8Encoding(true);
            _settings.ConformanceLevel = ConformanceLevel.Document;
            _settings.Indent = true;


            string targetPath = kGoogleServicesGeneratedXMLFilePath;
            string templatePath = Constants.kPluginAssetsPath + "/Plugins/Features/NotificationService/Editor/google-services.template";

            Directory.CreateDirectory(Path.GetDirectoryName(targetPath));

            // Replace strings in Template
            string templateContent = FileOperations.ReadAllText(templatePath);

            if (IsGoogleServicesJsonFileAvailable())
            {
                string googleServicesJsonContent = FileOperations.ReadAllText(kGoogleServicesJsonPath);

                IDictionary googleServicesJsonContentMap = (IDictionary)JSONUtility.FromJSON(googleServicesJsonContent);

                IDictionary projectInfo = googleServicesJsonContentMap.GetIfAvailable<IDictionary>("project_info");
                string firebaseDatabaseURL = projectInfo["firebase_url"] as string;
                string projectNumber = projectInfo["project_number"] as string;
                string storageBucket = projectInfo["storage_bucket"] as string;
                string projectID = projectInfo["project_id"] as string;

                templateContent = templateContent.Replace("FIREBASE_DATABASE_URL", firebaseDatabaseURL);
                templateContent = templateContent.Replace("GCM_DEFAULT_SENDER_ID", projectNumber);
                templateContent = templateContent.Replace("GOOGLE_STORAGE_BUCKET", storageBucket);
                templateContent = templateContent.Replace("PROJECT_ID", projectID);

                IList clientList = googleServicesJsonContentMap.GetIfAvailable<IList>("client");

                if (clientList != null && clientList.Count > 0)
                {
                    foreach (IDictionary eachClient in clientList)
                    {
                        IDictionary clientInfo = eachClient.GetIfAvailable<IDictionary>("client_info");
                        IDictionary androidClientInfo = clientInfo.GetIfAvailable<IDictionary>("android_client_info");

                        string clientPackageName = androidClientInfo.GetIfAvailable<string>("package_name");

                        if (clientPackageName.Equals(VoxelBusters.Utility.PlayerSettings.GetBundleIdentifier()))
                        {
                            IDictionary oauthClient = (IDictionary)(eachClient.GetIfAvailable<IList>("oauth_client")[0]);
                            IDictionary apiKey = (IDictionary)(eachClient.GetIfAvailable<IList>("api_key")[0]);

                            templateContent = templateContent.Replace("GOOGLE_APP_ID", clientInfo["mobilesdk_app_id"] as string);
                            templateContent = templateContent.Replace("GOOGLE_API_KEY", apiKey["current_key"] as string);
                            templateContent = templateContent.Replace("GOOGLE_CRASH_REPORTING_API_KEY", apiKey["current_key"] as string);
                            templateContent = templateContent.Replace("DEFAULT_WEB_CLIENT_ID", oauthClient["client_id"] as string);
                            break;
                        }
                    }
                }
                FileOperations.WriteAllText(targetPath, templateContent);
            }
        }
    }
}

#endif

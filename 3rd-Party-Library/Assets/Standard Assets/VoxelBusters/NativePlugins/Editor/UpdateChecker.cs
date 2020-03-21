using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
#if UNITY_EDITOR && !UNITY_CLOUD_BUILD
using UnityEditor;
using UnityEngine;
using VoxelBusters.Utility;

namespace VoxelBusters.NativePlugins.Internal
{
    [InitializeOnLoad]
    public class UpdateChecker
    {
        #region Constants

        const string kNPUpdateChecker = "native-plugins-update-checker";
#if NATIVE_PLUGINS_LITE_VERSION
        const string kUrl = "https://raw.githubusercontent.com/voxelbusters/Cross-Platform-Native-Plugins-for-Unity/master/news.lite.json";
#else
        const string kUrl = "https://raw.githubusercontent.com/voxelbusters/Cross-Platform-Native-Plugins-for-Unity/master/news.ultra.json";
#endif

        #endregion


        static UpdateChecker()
        {
            EditorUtils.Invoke(() =>
            {
                CheckUpdates();
            }, 0.1f);
        }

        private static void CheckUpdates()
        {
            if (SessionState.GetBool(kNPUpdateChecker, false))
            {
               return;
            }

            DownloadAsset request = new DownloadAsset(new URL(kUrl), true);
            request.OnCompletion = (WWW _www, string _error) =>
            {
            if (string.IsNullOrEmpty(_error))
            {
                string jsonString = _www.text;
                if (!string.IsNullOrEmpty(jsonString))
                {
                    IDictionary _dataDict = (IDictionary)JSONUtility.FromJSON(jsonString);
#if NATIVE_PLUGINS_LITE_VERSION
                        IDictionary versionInfo = _dataDict.GetIfAvailable<IDictionary>("info");
#else
                        IDictionary versionInfo = _dataDict.GetIfAvailable<IDictionary>("info");
#endif

                        if (versionInfo == null)
                            return;

                        string  version         = versionInfo.GetIfAvailable<string>("version");
                        int     updateIndex     = versionInfo.GetIfAvailable<int>("update-index"); 

                        string description  = _dataDict.GetIfAvailable<string>("description");
                        IList news          = _dataDict.GetIfAvailable<IList>("news");

                        int latestVersionNumber           = GetCovertedVersionValue(version);
                        int currentVersionNumber          = GetCovertedVersionValue(NPSettings.kProductVersion);
                        int notifiedVersionNumber         = EditorPrefs.GetInt(Constants.kNotifiedVersionKey, currentVersionNumber);

                        if ((currentVersionNumber < latestVersionNumber) && ((latestVersionNumber + updateIndex) != notifiedVersionNumber))
                        {
                            EditorUtility.DisplayDialog("Cross Platform Native Plugins", description, "ok");
                            EditorPrefs.SetInt(Constants.kNotifiedVersionKey, latestVersionNumber + updateIndex);
                        }
                        EditorPrefs.SetString(Constants.kNativePluginsAssetStoreVersionKey, version);
                        EditorPrefs.SetString(Constants.kVBNewsKey, news.ToJSON());
                    }
                }

            };

            request.StartRequest();
        }

        private static int GetCovertedVersionValue(string versionString)
        {
            float factor = 100000f;
            versionString = Regex.Replace(versionString, "[A-Za-z ]", ".");

            string majorVersion = versionString.Substring(0, versionString.IndexOf('.'));
            string minorVersion = versionString.Substring(versionString.IndexOf('.')).Replace(".", "");

            versionString = majorVersion + "." + minorVersion;

            int versionInt = (int)Mathf.Round(float.Parse(versionString, System.Globalization.CultureInfo.InvariantCulture) * factor);
            return versionInt;
        }

    }
}
#endif
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text;

#if UNITY_2018_2_OR_NEWER

using UnityEditor.Android;
using System.IO;

namespace VoxelBusters.NativePlugins.Internal
{
    class GradlePostProcessor : IPostGenerateGradleAndroidProject
    {
        public int callbackOrder { get { return 0; } }
        public void OnPostGenerateGradleAndroidProject(string path)
        {
            MatchUnityGradleVersion(path);
            EnableJetifierIfRequired(path);
        }

        private void MatchUnityGradleVersion(string path)
        {
            string rootGradleVersion = null;
            string rootCompileSdkVersion = null;
            string rootBuildToolsVersion = null;
            string rootTargetSDKVersion = null;

            string[] targetProjectPaths =
            {
                "/native_plugins_lib",
                "/twitter_lib",
                "/youtube_lib",
                "/voxelbusters_utility_lib"
            };

            // First read the main build.gradle file
            string[] lines = File.ReadAllLines(path + "/build.gradle");

            foreach (string eachLine in lines)
            {
                // Detect gradle version
                if (HasText(eachLine, "classpath", "tools.build:gradle"))
                {
                    rootGradleVersion = eachLine;
                }
                // Detect compileSdkVersion version
                else if (HasText(eachLine, "compileSdkVersion"))
                {
                    rootCompileSdkVersion = eachLine;
                }
                // Detect buildToolsVersion version
                else if (HasText(eachLine, "buildToolsVersion"))
                {
                    rootBuildToolsVersion = eachLine;
                }
                // Detect targetSdkVersion version
                else if (HasText(eachLine, "targetSdkVersion"))
                {
                    rootTargetSDKVersion = eachLine;
                }
            }


            foreach (string eachProject in targetProjectPaths)
            {
                UpdateGradleFile(path + eachProject, rootGradleVersion, rootCompileSdkVersion, rootBuildToolsVersion, rootTargetSDKVersion);
            }
        }

        private void UpdateGradleFile(string projectPath, string rootGradleVersion, string rootCompileSdkVersion, string rootBuildToolsVersion, string rootTargetSDKVersion)
        {
            string filePath = projectPath + "/build.gradle";

            if(File.Exists(filePath))
            {
                string[] lines = File.ReadAllLines(filePath);
                string[] updatedLines = new string[lines.Length];

                for (int i=0; i< lines.Length; i++)
                {
                    string eachLine     = lines[i];
                    string updatedText  = eachLine;

                    // Detect gradle version
                    if (HasText(eachLine, "classpath", "tools.build:gradle"))
                    {
                        updatedText = rootGradleVersion;
                    }
                    // Detect compileSdkVersion version
                    else if (HasText(eachLine, "compileSdkVersion"))
                    {
                        updatedText = rootCompileSdkVersion;
                    }
                    // Detect buildToolsVersion version
                    else if (HasText(eachLine, "buildToolsVersion"))
                    {
                        updatedText = rootBuildToolsVersion;
                    }
                    // Detect targetSdkVersion version
                    else if (HasText(eachLine, "targetSdkVersion"))
                    {
                        updatedText = rootTargetSDKVersion;
                    }

                    updatedLines[i] = updatedText; 
                }

                File.WriteAllLines(filePath, updatedLines);
            }
        }

        private bool HasText(string inputString, string startSearchString, string additionalStringToSearch = null)
        {
            string trimmedText = inputString.Trim();

            if(trimmedText.StartsWith(startSearchString, System.StringComparison.InvariantCulture))
            {
                if(additionalStringToSearch != null)
                {
                    return trimmedText.Contains(additionalStringToSearch);
                }

                return true;
            }

            return false;
        }

        private void EnableJetifierIfRequired(string path)
        {
            string[] files = Directory.GetFiles(Application.dataPath + "/Plugins/Android" , "androidx.*.aar");

            if(files.Length > 0)
            {
                string gradlePropertiesPath = path + "/gradle.properties";

                string[] lines = File.ReadAllLines(gradlePropertiesPath);

                // Need jetifier patch process
                bool hasAndroidXProperty = lines.Any(text => text.Contains("android.useAndroidX"));
                bool hasJetifierProperty = lines.Any(text => text.Contains("android.enableJetifier"));

                StringBuilder builder = new StringBuilder();

                foreach(string each in lines)
                {
                    builder.AppendLine(each);
                }

                if (!hasAndroidXProperty)
                {
                    builder.AppendLine("android.useAndroidX=true");
                }

                if (!hasJetifierProperty)
                {
                    builder.AppendLine("android.enableJetifier=true");
                }

                File.WriteAllText(gradlePropertiesPath, builder.ToString());
            }
        }
    }
}

#endif
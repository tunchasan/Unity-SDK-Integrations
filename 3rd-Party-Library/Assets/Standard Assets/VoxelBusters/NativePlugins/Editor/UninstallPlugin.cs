using UnityEngine;
using System.Collections;

#if UNITY_EDITOR && !(UNITY_WEBPLAYER || UNITY_WEBGL || NETFX_CORE)
using UnityEditor;
using System.IO;
using VoxelBusters.Utility;

namespace VoxelBusters.NativePlugins.Internal
{
	public class UninstallPlugin
	{
		#region Constants
	
		private const	string	kUninstallAlertTitle	= "Uninstall - Cross Platform Native Plugin";
		private const	string	kUninstallAlertMessage	= "Backup before doing this step to preserve changes done in this plugin. This deletes files only related to CPNP plugin. Do you want to proceed?";

		private static string[] kPluginFolders	=	new string[]
		{
			Constants.kAndroidPluginsPath 		+	"/support_lib",
			Constants.kAndroidPluginsPath 		+	"/twitter_lib",
			Constants.kAndroidPluginsPath 		+	"/native_plugins_lib",
			Constants.kAndroidPluginsPath 		+	"/voxelbusters_utility_lib",
			Constants.kAndroidPluginsPath 		+	"/google-play-services_lib",
			Constants.kVBCodebasePath 			+	"/NativePlugins",
			Constants.kVBCodebasePath			+	"/Common",
			Constants.kVBCodebasePath			+	"/DebugPro",
			Constants.kVBExternalCodebasePath 	+ 	"/NativePlugins",
			Constants.kRootAssetsPath			+	"/PlayServicesResolver"
		};
		
		#endregion	
	
		#region Methods
	
		public static void Uninstall()
		{
			bool _startUninstall = EditorUtility.DisplayDialog(kUninstallAlertTitle, kUninstallAlertMessage, "Uninstall", "Cancel");

			if (_startUninstall)
			{
				foreach (string _eachFolder in kPluginFolders)
				{
					string _absolutePath = AssetDatabaseUtils.AssetPathToAbsolutePath(_eachFolder);

					if (Directory.Exists(_absolutePath))
					{
						Directory.Delete(_absolutePath, true);
						
						// Delete meta files.
						FileOperations.Delete(_absolutePath + ".meta");
					}
				}
				
				// For LITE version we need to remove defines.
				GlobalDefinesManager _definesManager	= new GlobalDefinesManager();

				foreach (int _eachCompiler in System.Enum.GetValues(typeof(GlobalDefinesManager.eCompiler)))
				{
					_definesManager.RemoveDefineSymbol((GlobalDefinesManager.eCompiler)_eachCompiler, NPSettings.kLiteVersionMacro);
				}

				_definesManager.SaveAllCompilers();
				
				AssetDatabase.Refresh();
				EditorUtility.DisplayDialog("Cross Platform Native Plugins",
				                            "Uninstall successful!", 
				                            "Ok");
			}
		}
		
		#endregion
	}
}
#endif
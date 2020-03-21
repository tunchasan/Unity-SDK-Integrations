using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
using System.IO;
using VoxelBusters.Utility;

namespace VoxelBusters.NativePlugins.Internal
{
	[InitializeOnLoad]
	public class NPAssetManager : AssetPostprocessor 
	{
		#region Constants

        private		const	string		kPluginResourcesFolderPath		= Constants.kRootAssetsPath + "/PluginResources/NativePlugins";
		private 	const 	string 		kiOSAssetsSavePath				= null;
        private		const 	string 		kAndroidAssetsSavePath			= Constants.kAndroidPluginsCPNPPath+"/res/raw";
	
		#endregion

		#region Static Methods

		private static void CopyAssetsToActiveBuildTargetFolder()
		{
			// Get save path based on active platform
			string	_activeBuildTarget			= EditorUserBuildSettings.activeBuildTarget.ToString();
			string	_sourceFolderPath			= null;
			string 	_saveFolderPath				= null;
			bool	_needsLowerCaseFileNames	= false;

			if (_activeBuildTarget.Equals("iOS") || _activeBuildTarget.Equals("iPhone"))
			{
				_sourceFolderPath			= Path.Combine(GetPluginResourcesPath(), "iOS");
				_saveFolderPath				= kiOSAssetsSavePath;
			}
			else if (_activeBuildTarget.Equals("Android"))
			{
				_sourceFolderPath			= Path.Combine(GetPluginResourcesPath(), "Android");
				_saveFolderPath				= kAndroidAssetsSavePath;
				_needsLowerCaseFileNames 	= true;
			}
			else
			{
				return;
			}

			// Copy assets to save folder
			if (!string.IsNullOrEmpty(_saveFolderPath))
			{
				CopyFiles(_sourceFolderPath, _saveFolderPath, _needsLowerCaseFileNames);

				//Copy common assets
				_sourceFolderPath		= Path.Combine(GetPluginResourcesPath(), "Common");
				CopyFiles(_sourceFolderPath, _saveFolderPath, _needsLowerCaseFileNames, false);

				AssetDatabase.Refresh();
			}
		}

		private static string GetPluginResourcesPath()
		{
			return kPluginResourcesFolderPath;
		}

		#endregion

		#region Build Callback Methods

#if UNITY_ANDROID
		static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths) 
		{
			string	_pluginResourcesFolderPath	= GetPluginResourcesPath();
			
			if(HasTargetAssetFolderChanged(_pluginResourcesFolderPath, importedAssets, movedAssets, deletedAssets))
			{
				CopyAssetsToActiveBuildTargetFolder ();
			}
		}
#endif

		#endregion

		#region Helpers

		private static bool HasTargetAssetFolderChanged(string _targetFolder, params string[][] _allChangedAssets)
		{
			foreach(string[] _eachSubList in _allChangedAssets )
			{
				foreach(string _eachPath in _eachSubList )
				{
					if(_eachPath.StartsWith(_targetFolder))
						return true;
				}
			}

			return false;
		}

		private static void CopyFiles(string _sourceFolderPath, string _destinationFolderPath, bool _lowercaseDestinationFileNames = false, bool _deleteExistingDestinationDirectory = true)
		{
			if (!Directory.Exists(_sourceFolderPath))
				return;

#if !(UNITY_WEBPLAYER || UNITY_WEBGL || NETFX_CORE)
			// Get the file list - which has meta option
			// Save with a lower case file name
			DirectoryInfo 	_sourceDirectoryInfo 			= new DirectoryInfo(_sourceFolderPath);
			DirectoryInfo 	_destinationDirectoryInfo 		= new DirectoryInfo(_destinationFolderPath);

			if (_deleteExistingDestinationDirectory && _destinationDirectoryInfo.Exists)
				_destinationDirectoryInfo.Delete(true);
			
			_destinationDirectoryInfo.Create();
			
			FileInfo[] 		_files 			= _sourceDirectoryInfo.GetFiles("*", SearchOption.AllDirectories);

			foreach (FileInfo _curFileInfo in _files)
			{
				if (_curFileInfo.Extension == ".meta")
					continue;
				
				IOExtensions.CopyFile(_curFileInfo, Path.Combine(_destinationDirectoryInfo.FullName, _curFileInfo.Name.ToLower()));
			}
#endif
		}

		#endregion
	}
}
#endif
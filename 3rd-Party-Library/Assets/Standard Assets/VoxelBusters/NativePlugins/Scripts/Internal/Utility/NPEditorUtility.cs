#if (UNITY_WEBPLAYER || UNITY_WEBGL || NETFX_CORE)
#define IO_UNSUPPORTED_PLATFORM
#endif

using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
using System.IO;
using System;
using System.Reflection;
using VoxelBusters.Utility;

namespace VoxelBusters.NativePlugins.Internal
{
	public static class NPEditorUtility 
	{
		#region Methods

		public static void DownloadSDK (string _downloadLink, string _errorTitle, string _errorMessage)
		{
#if IO_UNSUPPORTED_PLATFORM
			Debug.LogWarning("[NPEditorUtility] Not supported.");
#else
			DownloadAsset 	_newRequest		= new DownloadAsset(URL.URLWithString(_downloadLink), true); 
			_newRequest.OnCompletion		= (WWW _www, string _error)=> {
				
				if (_error != null)
				{
					bool	_openDownloadLink	= EditorUtility.DisplayDialog(_errorTitle, _errorMessage, "Ok", "Cancel");
					
					if (_openDownloadLink)
						UnityEngine.Application.OpenURL(_downloadLink);
				}
				else
				{
					const string 	_kTempFolderRelativePath	= "Assets/Temp";
					const string 	_kTempFileRelativePath		= _kTempFolderRelativePath + "/NewPackage.unitypackage";
					
					if (!Directory.Exists(_kTempFolderRelativePath))
						Directory.CreateDirectory(_kTempFolderRelativePath);
					
					// Save the file into temp location and import it
					File.WriteAllBytes(_kTempFileRelativePath, _www.bytes);
					AssetDatabase.ImportPackage(_kTempFileRelativePath, true);
					
					// Remove temp folder and refresh
					Directory.Delete(_kTempFolderRelativePath, true);
					File.Delete(_kTempFolderRelativePath + ".meta");
					AssetDatabase.Refresh();
				}
			};
			_newRequest.StartRequest();
#endif
		}

		#endregion
	}
}
#endif
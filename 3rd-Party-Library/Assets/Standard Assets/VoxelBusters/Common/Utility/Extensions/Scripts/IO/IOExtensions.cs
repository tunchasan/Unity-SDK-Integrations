#if (UNITY_WEBPLAYER || UNITY_WEBGL || NETFX_CORE)
#define IO_UNSUPPORTED_PLATFORM
#endif

using UnityEngine;
using System.Collections;
using System;
using System.IO;

namespace VoxelBusters.Utility
{
	public static class IOExtensions 
	{
		#region Static Methods

		public static void Destroy (string _path)
		{
#if IO_UNSUPPORTED_PLATFORM
			Debug.LogWarning("[IOExtensions] Not supported.");
#else
			FileAttributes _attributes = File.GetAttributes(_path);
			
			if ((_attributes & FileAttributes.Directory) == FileAttributes.Directory)
				Directory.Delete(_path, true);
			else
				File.Delete(_path);
#endif
		}

		public static string MakeRelativePath (this string _fromPath, string _toPath)
		{
			Uri 	_fromUri	= null;

			if (_fromPath != null) 
				_fromUri		= new Uri(_fromPath);

			return MakeRelativePath(_fromUri, _toPath);
		}

		public static string MakeRelativePath (this Uri _fromUri, string _toPath)
		{
#if NETFX_CORE
			Debug.LogWarning("[IOExtensions] Not supported.");
			return null;
#else
			// Check input parameters
			if (_fromUri == null)
				throw new ArgumentNullException("_fromUri");

			if (_toPath == null)
				throw new ArgumentNullException("_toPath");

			Uri 	_toUri 		= new Uri(_toPath);
			
			// Compare schemes, its necessary that both share same Uri
			if (_fromUri.Scheme != _toUri.Scheme) 
				return _toPath;
			
			Uri 			_relativeUri 		= _fromUri.MakeRelativeUri(_toUri);
			string 			_relativePath 		= Uri.UnescapeDataString(_relativeUri.ToString());
			const string 	_kFileSystemScheme	= "file";

			if (_toUri.Scheme.Equals(_kFileSystemScheme))
				_relativePath 	= _relativePath.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);

			return _relativePath;
#endif
		}

		public static bool AssignPermissionRecursively (string _directoryPath, FileAttributes _attribute)
		{
#if IO_UNSUPPORTED_PLATFORM
			Debug.LogWarning("[IOExtensions] Not supported.");
			return false;
#else
			DirectoryInfo	_directoryInfo	= new DirectoryInfo(_directoryPath);

			if (!_directoryInfo.Exists)
			{
				Debug.LogWarning("[IOExtensions] The operation could not be completed because directory doesn't exist.");
				return false;
			}

			return _directoryInfo.AssignPermissionRecursively(_attribute);
#endif
		}

#if !IO_UNSUPPORTED_PLATFORM
		public static bool AssignPermissionRecursively (this DirectoryInfo _directoryInfo, FileAttributes _attribute)
		{
			// Update directory attribute
			_directoryInfo.Attributes	|= _attribute;

			// Update file attribute
			foreach (FileInfo _curFileInfo in _directoryInfo.GetFiles())
				_curFileInfo.Attributes |= _attribute;

			// Update permissions of subfolders
			foreach (DirectoryInfo _subDirectoryInfo in _directoryInfo.GetDirectories())
				AssignPermissionRecursively(_subDirectoryInfo, _attribute);

			return true;
		}
#endif

		public static void CopyFilesRecursively (string _sourceDirectory, string _destinationDirectory, bool _excludeMetaFiles = true, bool _deleteDestinationFolderIfExists = true) 
		{
#if IO_UNSUPPORTED_PLATFORM
			Debug.LogWarning("[IOExtensions] Not supported.");
			return;
#else
			// Get the subdirectories for the specified directory.
			DirectoryInfo 	_sourceDirectoryInfo 		= new DirectoryInfo(_sourceDirectory);
			DirectoryInfo 	_destinationDirectoryInfo 	= new DirectoryInfo(_destinationDirectory);

			CopyFilesRecursively(_sourceDirectoryInfo, _destinationDirectoryInfo, _excludeMetaFiles, _deleteDestinationFolderIfExists);	
			return;
#endif
		}

#if !IO_UNSUPPORTED_PLATFORM
		public static void CopyFilesRecursively (DirectoryInfo _sourceDirectoryInfo, DirectoryInfo _destinationDirectoryInfo, bool _excludeMetaFiles = true, bool _deleteDestinationFolderIfExists = true)
		{
			// Check if source exists
			if (!_sourceDirectoryInfo.Exists)
				throw new DirectoryNotFoundException(string.Format("The operation could not be completed because directory does not exist. Path: {0}.", _sourceDirectoryInfo.FullName));

			// Optionally, delete existing destination folder
			if (_deleteDestinationFolderIfExists && _destinationDirectoryInfo.Exists)
				_destinationDirectoryInfo.Delete(true);

			_destinationDirectoryInfo.Create();

			// Get all the files and copy them to the new location.
			FileInfo[] 		_files				= _sourceDirectoryInfo.GetFiles();
			const string	_kMetaFileExtension	= ".meta";

			if (_excludeMetaFiles)
			{
				foreach (FileInfo _curFileInfo in _files)
				{
					if (_curFileInfo.Extension == _kMetaFileExtension)
						continue;

					CopyFile(_curFileInfo, Path.Combine(_destinationDirectoryInfo.FullName, _curFileInfo.Name));
				}
			}
			else
			{
				foreach (FileInfo _curFileInfo in _files)
					CopyFile(_curFileInfo, Path.Combine(_destinationDirectoryInfo.FullName, _curFileInfo.Name));
			}
			
			// If copying subdirectories, copy them and their contents to new location. 
			DirectoryInfo[]	 _subDirectories 	= _sourceDirectoryInfo.GetDirectories();

			foreach (DirectoryInfo _subDirectoryInfo in _subDirectories)
				CopyFilesRecursively(_subDirectoryInfo, new DirectoryInfo(Path.Combine(_destinationDirectoryInfo.FullName, _subDirectoryInfo.Name)), _excludeMetaFiles);
		}
#endif
		
		public static void CopyFile (string _sourceFilePath, string _destinationFilePath, bool _overwrite = true)
		{
#if IO_UNSUPPORTED_PLATFORM
			Debug.LogWarning("[IOExtensions] Not supported.");
			return;
#else
			CopyFile(new FileInfo(_sourceFilePath), _destinationFilePath, _overwrite);
#endif
		}

#if !IO_UNSUPPORTED_PLATFORM
		public static void CopyFile (FileInfo _sourceFileInfo, string _destinationFilePath, bool _overwrite = true)
		{
			if (!_sourceFileInfo.Exists)
			{
				Debug.LogWarning("[IOExtensions] The operation could not be completed because file doesn't exist.");
				return;
			}

			// Set attributes to normal, to avoid i/o exceptions
			FileAttributes	_prevAttributes	= _sourceFileInfo.Attributes;
			_sourceFileInfo.Attributes		= FileAttributes.Normal;
			
			// Copy file
			_sourceFileInfo.CopyTo(_destinationFilePath, _overwrite);
			
			// Revert back to original attribute
			_sourceFileInfo.Attributes		= _prevAttributes;
		}
#endif

		public static int ComparePath(string pathA, string pathB)
		{
			#if !IO_UNSUPPORTED_PLATFORM
			return string.Compare(Path.GetFullPath(pathA).TrimEnd('\\'),
			                      Path.GetFullPath(pathB).TrimEnd('\\'), 
			                      true);
			#else
			return -1;
			#endif
		}

		#endregion
	}
}
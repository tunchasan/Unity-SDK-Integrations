using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using VoxelBusters.Utility;


#if UNITY_EDITOR
using UnityEditor;
#endif

namespace VoxelBusters.UnityEngineUtils
{
	public static class ScriptableObjectUtils
	{
		#region Create Methods

#if UNITY_EDITOR
		public static T Create<T>(string path, bool autoImport = true) where T : ScriptableObject
		{
			if (EditorApplication.isPlayingOrWillChangePlaymode || EditorApplication.isPaused)
				return null;

			T	_instance		= ScriptableObject.CreateInstance<T>();

            AssetDatabase.CreateAsset(_instance, path);//AssetDatabase.GenerateUniqueAssetPath(path));
			_instance.SaveChanges(autoImport);

			return _instance;
		}

		public static T Create<T>(bool autoImport = true) where T : ScriptableObject
		{
			// get selected path
			string	_path = AssetDatabase.GetAssetPath(Selection.activeObject);
			if (_path == "")
			{
				_path = "Assets";
			}
			else if (Path.GetExtension(_path) != "")
			{
				_path = _path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
			}

			// append file name
			_path	+= typeof(T).Name + ".asset";

			// create asset file
			return Create<T>(_path, autoImport);
		}
#endif

		#endregion

		#region Save Methods

#if UNITY_EDITOR
		public static void SaveChanges<T>(this T instance, bool autoImport = true) where T : ScriptableObject
		{
			// Save operation is allowed only on Unity Editor
			// and that too while player is in edit mode
			if (EditorApplication.isPlaying || EditorApplication.isPaused)
				return;

			// Save the changes
			EditorUtility.SetDirty(instance);
			AssetDatabase.SaveAssets();

			if (autoImport)
			{
				AssetDatabase.Refresh();
			}
		}
#endif

		#endregion

		#region Load Methods

		public static T LoadAssetAtPath<T>(string path) where T : ScriptableObject
		{
#if UNITY_EDITOR
			return (T)AssetDatabase.LoadAssetAtPath(path, typeof(T));
#else
			string 	_resourcePath 		= "Assets/Resources/";
			string	_pathInResources	= IOExtensions.MakeRelativePath(_fromPath: Path.GetFullPath(_resourcePath), 
			                                                            _toPath: Path.GetFullPath(path));

			// remove file extension
			_pathInResources	= Path.ChangeExtension(_pathInResources, null);
			return Resources.Load<T>(_pathInResources);
#endif
		}

		#endregion
	}
}
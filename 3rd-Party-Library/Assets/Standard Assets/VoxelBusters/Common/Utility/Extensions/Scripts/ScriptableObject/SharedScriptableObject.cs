using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using VoxelBusters.UnityEngineUtils;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace VoxelBusters.Utility
{
	public abstract class SharedScriptableObject<T> : ScriptableObject, ISaveAssetCallback where T : ScriptableObject
	{
		#region Static Fields

		private		static		T		instance	= null;

		#endregion

		#region Static Properties

		public static T Instance
		{
			get 
			{ 
				if (instance == null)
				{
					#if UNITY_EDITOR
					MoveExistingAssetsTemp();
					#endif

					string	_path	= string.Format("Assets/Resources/SharedAssets/{0}.asset", typeof(T).Name);
					instance		= ScriptableObjectUtils.LoadAssetAtPath<T>(_path);

					#if UNITY_EDITOR
					if (instance == null)
					{
						instance	= ScriptableObjectUtils.Create<T>(_path);
					}
					#endif
				}

				return instance;
			}
			set
			{
				instance	= value;
			}
		}

		#endregion

		#region Public Methods

		#if UNITY_EDITOR
		public void Save()
		{
			OnBeforeSave();

			this.SaveChanges();
		}
		#endif

		#endregion

		#region Unity Callbacks

		protected virtual void Reset()
		{}

		protected virtual void OnEnable()
		{
			if (instance == null)
			{
				instance	= this as T;
			}
		}

		protected virtual void OnDisable()
		{}

		protected virtual void OnDestroy()
		{}

		#endregion

		#region ISaveAssetCallback Implementation

		public virtual void OnBeforeSave()
		{}

		#endregion

		#region Temporary Methods

		#if UNITY_EDITOR
		private static void MoveExistingAssetsTemp()
		{
			string	_oldPath	= "Assets/Resources/VoxelBusters";
			string	_newPath	= "Assets/Resources/SharedAssets";

			// check whether destination folder exists
			if (!AssetDatabase.IsValidFolder(_newPath))
			{
				AssetDatabaseUtils.CreateFolder(_newPath);
			}

			// parse through the assets and move it to destination folder
			if (AssetDatabase.IsValidFolder(_oldPath))
			{
				string[] _assetIDs	= AssetDatabase.FindAssets("t: ScriptableObject", new string[] { _oldPath });
				foreach (string _assetID in _assetIDs)
				{
					string	_sourcePath	= AssetDatabase.GUIDToAssetPath(_assetID);
					string	_fileName	= Path.GetFileName(_sourcePath);

					AssetDatabase.MoveAsset(_sourcePath, _newPath + "/" + _fileName);
				}
			}
		}
		#endif

		#endregion
	}
}
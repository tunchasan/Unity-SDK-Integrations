#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using VoxelBusters.Utility;

using PlayerSettings	= VoxelBusters.Utility.PlayerSettings;

namespace VoxelBusters.NativePlugins
{
	using Internal;

	[InitializeOnLoad]
	public class NPEditorManager : MonoBehaviour 
	{
		#region Constants

		private		const	string		kPrefsKeyBuildIdentifier	= "np-build-identifier";

		private		const	float 		kWaitingPeriod 	= 2f;

		#endregion

		#region Static Fields

		private		static		bool 		canCreateDependentAssets;
		private		static 		float 		startTime;

		#endregion

		#region Static Constructor

		#if !NATIVE_PLUGIN_HIBERNATE
		static NPEditorManager()
		{
			Reset();

			// regiser to editor update callback
			EditorApplication.update   += EditorUpdate;
		}
		#endif

		#endregion

		#region Static Methods

		private static void Reset()
		{
			EditorApplication.update   -= EditorUpdate;

			// set default properties
			canCreateDependentAssets	= true;
			startTime 					= (float)EditorApplication.timeSinceStartup;
		}

		private static void EditorUpdate()
		{
			if (GetTimeSinceStart() < kWaitingPeriod)
				return;

			if (canCreateDependentAssets)
			{
				CreateDependentAssets();
			}
			else
			{
				MonitorPlayerSettings();
			}
		}

		private static float GetTimeSinceStart()
		{
			return (float)(EditorApplication.timeSinceStartup - startTime);
		}

		private static void CreateDependentAssets()
		{
#pragma warning disable
			// create settings file
			NPSettings _instance	= NPSettings.Instance;

			// create simulator files
#if USES_GAME_SERVICES
			EditorGameCenter 			_gamecenter			= EditorGameCenter.Instance;
#endif
			
#if USES_NOTIFICATION_SERVICE
			EditorNotificationCenter	_notificationCenter	= EditorNotificationCenter.Instance;
#endif
#pragma warning restore
			
			// mark that dependent files are created
			canCreateDependentAssets	= false;
		}

		private static void MonitorPlayerSettings()
		{
			// check whether there's change in value
			string	_oldBuildIdentifier	= EditorPrefs.GetString(kPrefsKeyBuildIdentifier, null);
			string	_curBuildIdentifier	= PlayerSettings.GetBundleIdentifier();
			if (string.Equals(_oldBuildIdentifier, _curBuildIdentifier))
				return;

			// save copy of new value
			EditorPrefs.SetString(kPrefsKeyBuildIdentifier, _curBuildIdentifier);

			// rebuild associated files
			NPSettings _settings	= NPSettings.Instance;
			if (_settings != null)
				_settings.Rebuild();
		}

		#endregion
	}
}
#endif
using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace VoxelBusters.NativePlugins
{
	public class PlatformValueHelper 
	{
		#region Static Methods

		public static PlatformValue GetCurrentPlatformValue (PlatformValue[] _array)
		{
			if (_array == null)
				return null;

#if UNITY_EDITOR
			eRuntimePlatform _platform	= GetEditorPlatform();
#else
			eRuntimePlatform _platform	= GetRuntimePlatform();
#endif
			return System.Array.Find(_array, (_entry) => _entry.Platform == _platform);
		}

		#endregion

		#region Private Static Methods

#if UNITY_EDITOR
		private static eRuntimePlatform GetEditorPlatform ()
		{
			switch (EditorUserBuildSettings.activeBuildTarget)
			{
#if UNITY_4_5 || UNITY_4_6 || UNITY_4_7 
				case BuildTarget.iPhone:
#else
				case BuildTarget.iOS:
#endif
					return eRuntimePlatform.IOS;

				case BuildTarget.Android:
					return eRuntimePlatform.ANDROID;

				default:
					return eRuntimePlatform.UNKNOWN;
			}
		}
#endif

		private static eRuntimePlatform GetRuntimePlatform ()
		{
			switch (Application.platform)
			{
				case RuntimePlatform.IPhonePlayer:
						return eRuntimePlatform.IOS;

				case RuntimePlatform.Android:
						return eRuntimePlatform.ANDROID;

					default:
						return eRuntimePlatform.UNKNOWN;
			}
		}

		#endregion
	}
}
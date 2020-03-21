using UnityEngine;
using System.Collections;

namespace VoxelBusters.Utility
{
	public class PlayerSettings : MonoBehaviour
	{
		#region Static Methods

		public static string GetBundleVersion ()
		{
#if UNITY_EDITOR
			return UnityEditor.PlayerSettings.bundleVersion;
#else
			return NativeBinding.GetBundleVersion();
#endif
		}

		public static string GetBundleIdentifier ()
		{
#if UNITY_EDITOR
#if UNITY_5_6_OR_NEWER
			return UnityEditor.PlayerSettings.applicationIdentifier;
#else
			return UnityEditor.PlayerSettings.bundleIdentifier;
#endif
#else
			return NativeBinding.GetBundleIdentifier();
#endif
		}

		#endregion
	}
}
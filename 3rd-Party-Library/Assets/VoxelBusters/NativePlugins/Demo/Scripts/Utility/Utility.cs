using UnityEngine;
using System.Collections;

namespace VoxelBusters.NativePlugins.Demo
{
	public class Utility : MonoBehaviour
	{
		#region Unity Methods

		// Use this for initialization
		void Start ()
		{
#if UNITY_2017_1_OR_NEWER
			ScreenCapture.CaptureScreenshot("Screenshot.png");
#else
			Application.CaptureScreenshot("Screenshot.png");
#endif
		}

		#endregion

		#region Static Methods

		public static string GetScreenshotPath ()
		{
			return System.IO.Path.Combine(Application.persistentDataPath,  "Screenshot.png");
		}

		#endregion
	}
}

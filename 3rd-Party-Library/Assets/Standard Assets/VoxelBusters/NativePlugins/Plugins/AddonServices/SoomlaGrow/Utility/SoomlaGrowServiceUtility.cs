using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
namespace VoxelBusters.NativePlugins.Internal
{
	public class SoomlaGrowServiceUtility 
	{
		#region Constants

		private 	const 	string	kDownloadLink		= "http://bit.ly/1YhbKeM";
		private 	const 	string	kSignupLink			= "http://bit.ly/1NEeyuT";
		private 	const 	string	kHomePageLink		= "http://bit.ly/1Mnlkao";

		#endregion

		#region Methods

		public static void OpenSignupPage ()
		{
			Application.OpenURL(kSignupLink);
		}

		public static void OpenHomePage ()
		{
			Application.OpenURL(kHomePageLink);
		}
		
		public static void DownloadSDK ()
		{
			NPEditorUtility.DownloadSDK(kDownloadLink, 
			                            "Soomla Grow SDK: Export Failed", 
			                            "Failed to export latest version of Soomla Grow SDK. Would you like to download manually?"
			                            );
		}
		
		#endregion
	}
}
#endif
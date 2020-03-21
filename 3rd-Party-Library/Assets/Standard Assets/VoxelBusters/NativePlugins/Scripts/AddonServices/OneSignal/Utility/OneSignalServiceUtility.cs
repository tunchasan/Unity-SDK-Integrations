using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
namespace VoxelBusters.NativePlugins.Internal
{
	public class OneSignalServiceUtility 
	{
		#region Constants
		
		private 	const 	string	kDownloadLink		= "http://bit.ly/1HqKiqQ";
		private 	const 	string	kDocsLink			= "http://bit.ly/1XjrbWG";
		private 	const 	string	kHomePageLink		= "http://bit.ly/1NeFo2w";
		
		#endregion
		
		#region Methods
		
		public static void OpenHomePage ()
		{
			Application.OpenURL(kHomePageLink);
		}
		
		public static void OpenDocumentationPage ()
		{
			Application.OpenURL(kDocsLink);
		}
		
		public static void DownloadSDK ()
		{
			Application.OpenURL(kDownloadLink);
		}
		
		#endregion
	}
}
#endif
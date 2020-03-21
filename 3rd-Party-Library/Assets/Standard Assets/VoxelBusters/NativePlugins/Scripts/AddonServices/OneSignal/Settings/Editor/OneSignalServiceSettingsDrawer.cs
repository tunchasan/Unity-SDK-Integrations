using UnityEngine;
using System.Collections;

#if UNITY_EDITOR && USES_ONE_SIGNAL
using UnityEditor;

namespace VoxelBusters.NativePlugins
{
	using Internal;
	
	[CustomPropertyDrawer(typeof(OneSignalServiceSettings))]
	public class OneSignalServiceSettingsDrawer : AddonServiceSettingsDrawerBase 
	{
		#region Fields
		
		private		GUIContent[]	m_menuButtonNames	= new GUIContent[] {
			new GUIContent("Homepage", 	"Opens One Signal home page."),
			new GUIContent("Docs", 		"Opens One Signal SDK documentation."),
			new GUIContent("Download", 	"Opens link to download latest unitypackage file.")
		};
		
		#endregion
		
		#region Methods
		
		protected override GUIContent[] GetMenuButtonNames ()
		{
			return m_menuButtonNames;
		}
		
		protected override void OnMenuButtonPressed (int _buttonIndex)
		{
			switch (_buttonIndex)
			{
			case 0:
				OneSignalServiceUtility.OpenHomePage();
				break;
				
			case 1:
				OneSignalServiceUtility.OpenDocumentationPage();
				break;
				
			case 2:
				OneSignalServiceUtility.DownloadSDK();
				break;
				
			default:
				throw new System.Exception("Unhandled case.");
			}
		}
		
		#endregion
	}
}
#endif
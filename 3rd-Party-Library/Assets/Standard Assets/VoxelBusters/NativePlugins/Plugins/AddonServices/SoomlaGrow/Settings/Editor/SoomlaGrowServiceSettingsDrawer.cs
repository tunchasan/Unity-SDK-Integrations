using UnityEngine;
using System.Collections;

#if UNITY_EDITOR && USES_SOOMLA_GROW
using UnityEditor;

namespace VoxelBusters.NativePlugins
{
	using Internal;

	[CustomPropertyDrawer(typeof(SoomlaGrowServiceSettings))]
	public class SoomlaGrowServiceSettingsDrawer : AddonServiceSettingsDrawerBase 
	{
		#region Fields

		private		GUIContent[]	m_menuButtonNames	= new GUIContent[] {
			new GUIContent("Homepage", 	"Opens Soomla Grow home page."),
			new GUIContent("Signup", 	"Opens Soomla Grow signup page."),
			new GUIContent("Download", 	"Downloads and imports the latest unitypackage file.")
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
				SoomlaGrowServiceUtility.OpenHomePage();
				break;

			case 1:
				SoomlaGrowServiceUtility.OpenSignupPage();
				break;
				
			case 2:
				SoomlaGrowServiceUtility.DownloadSDK();
				break;

			default:
				throw new System.Exception("Unhandled case.");
			}
		}

		#endregion
	}
}
#endif
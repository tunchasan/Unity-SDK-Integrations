using UnityEngine;
using System.Collections;

#if UNITY_EDITOR && USES_ADDRESS_BOOK
using UnityEditor;
using VoxelBusters.Utility;

namespace VoxelBusters.NativePlugins.Internal
{
	[CustomEditor(typeof(EditorAddressBook))]
	public class EditorAddressBookInspector : CustomScriptableObjectEditor 
	{
		#region Methods

		protected override void OnGUIWindow ()
		{
			GUIStyle _subTitleStyle	= new GUIStyle("BoldLabel");
			_subTitleStyle.wordWrap	= true;

			GUILayout.BeginVertical(UnityEditorUtility.kOuterContainerStyle);
			{
				UnityEditorUtility.DrawLabel("Registration Status", _subTitleStyle, UnityEditorUtility.Alignment.Center);
				GUILayout.Space(2f);
				DrawProperty("m_authorizationStatus", GUIStyle.none);
				GUILayout.Space(2f);
			}
			GUILayout.EndVertical();
			GUILayout.Space(5f);

			GUILayout.BeginVertical(UnityEditorUtility.kOuterContainerStyle);
			{
				UnityEditorUtility.DrawLabel("Contacts", _subTitleStyle, UnityEditorUtility.Alignment.Center);
				GUILayout.Space(2f);
				DrawChildProperties("m_contactsList", GUIStyle.none);
				GUILayout.Space(2f);
			}
			GUILayout.EndVertical();
		}

		#endregion

	}
}
#endif
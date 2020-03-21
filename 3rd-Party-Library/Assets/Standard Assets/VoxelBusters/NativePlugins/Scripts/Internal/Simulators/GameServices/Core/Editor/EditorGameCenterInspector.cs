using UnityEngine;
using System.Collections;

#if UNITY_EDITOR && USES_GAME_SERVICES
using UnityEditor;
using VoxelBusters.Utility;

namespace VoxelBusters.NativePlugins.Internal
{
	[CustomEditor(typeof(EditorGameCenter))]
	public class EditorGameCenterInspector : CustomScriptableObjectEditor 
	{
		#region Unity Methods
		
		protected override void OnGUIWindow ()
		{
			GUIStyle _subTitleStyle	= new GUIStyle("BoldLabel");
			_subTitleStyle.wordWrap	= true;
			
			// Draw properties
			GUILayout.BeginVertical(UnityEditorUtility.kOuterContainerStyle);
			{
				UnityEditorUtility.DrawLabel("Registered Users", _subTitleStyle, UnityEditorUtility.Alignment.Center);
				GUILayout.Space(2f);
				DrawChildProperties("m_registeredUsers", GUIStyle.none);
				GUILayout.Space(2f);
			}
			GUILayout.EndVertical();
			GUILayout.Space(5f);
				
			GUILayout.BeginVertical(UnityEditorUtility.kOuterContainerStyle);
			{
				UnityEditorUtility.DrawLabel("Leaderboards", _subTitleStyle, UnityEditorUtility.Alignment.Center);
				GUILayout.Space(2f);
				DrawChildProperties("m_leaderboardList", GUIStyle.none);
				GUILayout.Space(2f);
			}
			GUILayout.EndVertical();
			GUILayout.Space(5f);

			GUILayout.BeginVertical(UnityEditorUtility.kOuterContainerStyle);
			{
				UnityEditorUtility.DrawLabel("Achievement Descriptions", _subTitleStyle, UnityEditorUtility.Alignment.Center);
				GUILayout.Space(2f);
				DrawChildProperties("m_achievementDescriptionList", GUIStyle.none);
				GUILayout.Space(2f);
			}
			GUILayout.EndVertical();
		}
		
		#endregion
	}
}
#endif
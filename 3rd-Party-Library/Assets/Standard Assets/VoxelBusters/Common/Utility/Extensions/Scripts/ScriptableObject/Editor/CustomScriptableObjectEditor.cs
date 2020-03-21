#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using UnityEditor;

namespace VoxelBusters.Utility
{
	public class CustomScriptableObjectEditor : Editor 
	{
		#region Fields

		private		float	m_leftMarginOffset	= 10f;
		private		float	m_rightMarginOffset	= 5f;

		#endregion

		#region Unity Callbacks
		
		protected virtual void OnEnable()
		{}
		
		protected virtual void OnDisable()
		{}

		public override bool UseDefaultMargins()
		{
			return false;
		}
		
		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			GUILayout.BeginVertical(EditorStyles.inspectorFullWidthMargins);
			{
				OnGUIWindow();
			}
			GUILayout.EndVertical();

			if (GUI.changed)
			{
				serializedObject.ApplyModifiedProperties();
			}
		}

		#endregion

		#region GUI Methods

		protected virtual void OnGUIWindow()
		{
			DrawProperties(UnityEditorUtility.kOuterContainerStyle);
		}
	
		#endregion

		#region Private Methods

		protected void DrawProperties(string _style)
		{
			DrawProperties(new GUIStyle(_style));
		}

		protected void DrawProperties(GUIStyle _style)
		{
			SerializedProperty 	_property	= serializedObject.GetIterator();
			_property.NextVisible(true);

			GUILayout.BeginHorizontal(_style);
			{
				GUILayout.Space(m_leftMarginOffset);
				GUILayout.BeginVertical();
				{
					GUILayout.Space(2f);
					while (_property.NextVisible(false))
						EditorGUILayout.PropertyField(_property, true);
					GUILayout.Space(4f);
				}
				GUILayout.EndVertical();	
				GUILayout.Space(m_rightMarginOffset);
			}
			GUILayout.EndHorizontal();
		}

		protected void DrawProperty (string _propertyName, string _style)
		{
			DrawProperty(_propertyName, new GUIStyle(_style));
		}

		protected void DrawProperty (string _propertyName, GUIStyle _style)
		{
			SerializedProperty 	_property	= serializedObject.FindProperty(_propertyName);

			GUILayout.BeginHorizontal(_style);
			{
				GUILayout.Space(m_leftMarginOffset);
				GUILayout.BeginVertical();
				{
					EditorGUILayout.PropertyField(_property, true);
				}
				GUILayout.EndVertical();	
				GUILayout.Space(m_rightMarginOffset);
			}
			GUILayout.EndHorizontal();
		}

		protected void DrawChildProperties (string _propertyName, GUIStyle _style)
		{
			SerializedProperty 	_property		= serializedObject.FindProperty(_propertyName);
			SerializedProperty 	_endProperty	= _property.GetEndProperty();

			// Move to child property
			_property.NextVisible(true);

			// Draw layout
			GUILayout.BeginHorizontal(_style);
			{
				GUILayout.Space(m_leftMarginOffset);
				GUILayout.BeginVertical();
				{
					do
					{
						if (SerializedProperty.EqualContents(_property, _endProperty))
							break;
						
						EditorGUILayout.PropertyField(_property, true);
					}while (_property.NextVisible(false));
				}
				GUILayout.EndVertical();	
				GUILayout.Space(m_rightMarginOffset);
			}
			GUILayout.EndHorizontal();
		}

		protected void SetLeftMarginOffset(float _offset)
		{
			m_leftMarginOffset	= _offset;
		}

		protected void SetRightMarginOffset(float _offset)
		{
			m_rightMarginOffset	= _offset;
		}

		#endregion
	}
}
#endif
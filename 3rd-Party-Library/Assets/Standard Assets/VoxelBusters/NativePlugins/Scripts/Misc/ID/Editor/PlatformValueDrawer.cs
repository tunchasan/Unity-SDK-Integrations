using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;

namespace VoxelBusters.NativePlugins
{
	[CustomPropertyDrawer(typeof(PlatformValue))]
	public class PlatformValueDrawer : PropertyDrawer 
	{
		#region Drawer Methods
		
		public override float GetPropertyHeight (SerializedProperty _property, GUIContent _label) 
		{
			return EditorGUIUtility.singleLineHeight;
		}
		
		public override void OnGUI (Rect _position, SerializedProperty _property, GUIContent _label) 
		{
			_label		= EditorGUI.BeginProperty(_position, _label, _property);

			// Draw property name label
			_position	= EditorGUI.PrefixLabel(_position, GUIUtility.GetControlID(FocusType.Passive), _label);

			// Draw property attributes
			Rect	_platformRect	= new Rect(_position.x, _position.y, 60, _position.height);
			Rect	_IDRect			= new Rect(_position.x + 65, _position.y, _position.width - 65, _position.height);
			int  	_indentLevel 	= EditorGUI.indentLevel;

			EditorGUI.indentLevel 	= 0;
			EditorGUI.PropertyField(_platformRect, _property.FindPropertyRelative("m_platform"), GUIContent.none);
			EditorGUI.PropertyField(_IDRect, _property.FindPropertyRelative("m_value"), GUIContent.none);
			EditorGUI.indentLevel 	= _indentLevel;
			
			EditorGUI.EndProperty();
		}
		
		#endregion
	}
}
#endif
#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using UnityEditor;
using VoxelBusters.Utility;

namespace VoxelBusters.NativePlugins
{
	[CustomPropertyDrawer(typeof(ApplicationSettings.Features.MultiComponentFeature), true)]
	public class MultiComponentFeatureDrawer : PropertyDrawer 
	{
		#region Drawer Methods

		public override float GetPropertyHeight (SerializedProperty _property, GUIContent _label) 
		{
			if (_property.isExpanded)
				return EditorGUI.GetPropertyHeight(_property) - 22f;

			return EditorGUIUtility.singleLineHeight;
		}

		public override void OnGUI (Rect _position, SerializedProperty _property, GUIContent _label) 
		{
			try
			{
				_label		= EditorGUI.BeginProperty(_position, _label, _property);
				_label.text	= "Uses " + _label.text;

				EditorGUI.BeginChangeCheck();

				// Show main property which determines whether property is used or not
				SerializedProperty	_valueProperty	= _property.FindPropertyRelative("value");
				Rect				_toggleRect		= new Rect(_position.x, _position.y, _position.width, EditorGUIUtility.singleLineHeight);
				_valueProperty.boolValue			= EditorGUI.Toggle(_toggleRect, _label, _valueProperty.boolValue);

				// Update property expand status
				_property.isExpanded	= _valueProperty.boolValue;

				// Show internal properties if feature is marked as used
				if (_property.isExpanded)
				{
					EditorGUI.indentLevel++;

					SerializedProperty 	_propertyCopy	= _property.Copy();
					SerializedProperty 	_endProperty	= _property.GetEndProperty();
					bool				_enterChild		= true;
					while (_propertyCopy.NextVisible(_enterChild))
					{
						_enterChild	= false;

						if (SerializedProperty.EqualContents(_propertyCopy, _valueProperty))
							continue;

						if (SerializedProperty.EqualContents(_propertyCopy, _endProperty))
							break;

						_toggleRect.y			+= EditorGUIUtility.singleLineHeight;
						EditorGUI.PropertyField(_toggleRect, _propertyCopy);
					}

					EditorGUI.indentLevel--;
				}
			}
			finally
			{
				if (EditorGUI.EndChangeCheck())
				{
					SerializedObject	_serializedObject	= _property.serializedObject;
					_serializedObject.ApplyModifiedProperties();
					_serializedObject.targetObject.InvokeMethod(NPSettings.kMethodPropertyChanged);
				}

				EditorGUI.EndProperty();
			}
		}

		#endregion
	}
}
#endif
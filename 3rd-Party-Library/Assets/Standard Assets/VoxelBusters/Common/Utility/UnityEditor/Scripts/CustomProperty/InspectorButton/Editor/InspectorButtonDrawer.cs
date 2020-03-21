#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using UnityEditor;

namespace VoxelBusters.Utility
{
	[CustomPropertyDrawer(typeof(InspectorButtonAttribute))]
	public class InspectorButtonDrawer : PropertyDrawer 
	{
		#region Constants
		
		private		const		float		kButtonWidth		= 228f;
		private		const		float		kButtonHeight		= 21f;
		private		const		float		kOffset				= 4f;
		
		#endregion

		#region Properties
		
		private InspectorButtonAttribute InspectorButton
		{ 
			get 
			{ 
				return (InspectorButtonAttribute)attribute; 
			} 
		}
		
		#endregion

		#region Drawer Methods
		
		public override float GetPropertyHeight (SerializedProperty _property, GUIContent _label) 
		{
			if (_property.isArray)
				return EditorGUI.GetPropertyHeight(_property);

			return EditorGUI.GetPropertyHeight(_property) + kButtonHeight + kOffset;
		}

		public override void OnGUI (Rect _position, SerializedProperty _property, GUIContent _label)
		{
			_label	= EditorGUI.BeginProperty(_position, _label, _property);

			if (_property.isArray)
			{
				EditorGUI.PropertyField(_position, _property, _label, true);
			}
			else
			{
				Rect 	_buttonGroupRect;
				Rect	_propertyRect;
				float 	_basePropertyHeight		= EditorGUI.GetPropertyHeight(_property);

				if (InspectorButton.Position == eInspectorButtonPosition.TOP)
				{
					_buttonGroupRect			= new Rect(_position.xMin, 		_position.yMin, 
					                          		 		_position.width, 	kButtonHeight);
					_propertyRect				= new Rect(_position.xMin, 		_buttonGroupRect.yMax + kOffset, 
					                        				_position.width, 	_basePropertyHeight);
				}
				else
				{
					_propertyRect				= new Rect(_position.xMin, 		_position.yMin, 
					                        				_position.width, 	_basePropertyHeight);
					_buttonGroupRect			= new Rect(_position.xMin, 		_propertyRect.yMax + kOffset, 
					                        				_position.width, 	kButtonHeight);
				}

				// Draw property
				EditorGUI.PropertyField(_propertyRect, _property, _label, true);

				// Draw button
				GUI.BeginGroup(_buttonGroupRect);
				{
					int		_totalButtons			= InspectorButton.Buttons.Length;
					float	_buttonInitialOffset	= _buttonGroupRect.width * 0.1f;
					float	_buttonGap				= _buttonGroupRect.width * 0.05f;
					float 	_buttonWidth			= (_buttonGroupRect.width - (_buttonGap * (_totalButtons - 1)) - (_buttonInitialOffset * 2)) / _totalButtons;
					Rect	_buttonRect				= new Rect(_buttonInitialOffset, 0f,
					                               _buttonWidth, _buttonGroupRect.height);

					foreach (InspectorButtonInfo _currentButton in InspectorButton.Buttons)
					{
						if (GUI.Button(_buttonRect, new GUIContent(_currentButton.Name, _currentButton.ToolTip)))
							_property.serializedObject.targetObject.InvokeMethod(_currentButton.InvokeMethod);

						_buttonRect.x				+= (_buttonRect.width + _buttonGap);
					}
				}
				GUI.EndGroup();
			}

			EditorGUI.EndProperty();
		}
		
		#endregion
	}
}
#endif
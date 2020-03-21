using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;

namespace VoxelBusters.NativePlugins
{
	using Internal;

	public abstract class AddonServiceSettingsDrawerBase : PropertyDrawer 
	{
		#region Constants

		private		const	float		kSpacingAfterEachElement	= 2f;
		private		const	float		kExtraSpacingAfterButton	= 5f;
		private		const	float		kButtonHeight				= 18f;

		#endregion

		#region Drawer Methods
		
		public override float GetPropertyHeight (SerializedProperty _property, GUIContent _label) 
		{
			GUIContent[] _menuButtons	= GetMenuButtonNames();
			float		_propertyHeight	= EditorGUI.GetPropertyHeight(_property);

			// Additional we need some space for menu
			if (_property.isExpanded && _menuButtons.Length > 0)
				_propertyHeight			+= kButtonHeight + kExtraSpacingAfterButton;

			return _propertyHeight;
		}
		
		public override void OnGUI (Rect _position, SerializedProperty _property, GUIContent _label) 
		{
			EditorGUI.BeginProperty(_position, _label, _property);

			// Caculate rect
			Rect		_foldoutRect		= new Rect(_position.x, _position.y, _position.width, EditorGUIUtility.singleLineHeight);

			// Draw properties
			_property.isExpanded			= EditorGUI.Foldout(_foldoutRect, _property.isExpanded, _label);

			if (_property.isExpanded)
			{
				// Draw menu buttons
				Rect				_updatedPosition	= new Rect(_position.x, _foldoutRect.yMax, _position.width, 0f);
				Rect				_childPropertyRect;

				DrawMenuButtons(_updatedPosition, out _childPropertyRect);

				// Draw child properties by indenting by one level
				EditorGUI.indentLevel++;

				SerializedProperty	_childProperty		= _property.Copy(); 
				SerializedProperty	_endProperty		= _property.GetEndProperty();

				// Point to the first child
				_childProperty.NextVisible(true);

				do
				{
					if (SerializedProperty.EqualContents(_childProperty, _endProperty))
						break;

					float 	_childPropertyHeight	= EditorGUI.GetPropertyHeight(_childProperty);

					// Update rect height
					_childPropertyRect.height		= _childPropertyHeight;
				
					EditorGUI.PropertyField(_childPropertyRect, _childProperty, true); 

					// Update rect origin, required to rect next property
					_childPropertyRect.y			= _childPropertyRect.yMax + kSpacingAfterEachElement;

				} while (_childProperty.NextVisible(false));
				
				// Reset indentation level
				EditorGUI.indentLevel--;
			}
			
			EditorGUI.EndProperty();          
		}

		private void DrawMenuButtons (Rect _position, out Rect _childPropertiesPosition)
		{
			GUIContent[] _menuButtons			= GetMenuButtonNames();
			int			_menuButtonCount		= _menuButtons.Length;

			if (_menuButtonCount == 0)
			{
				// Set child properties rect
				_childPropertiesPosition		= new Rect(_position.xMin,
				                                     _position.yMin + kSpacingAfterEachElement,
				                                     _position.width,
				                                     0f);
			}
			else
			{
				Rect	_buttonGroupRect		= new Rect(_position.xMin + _position.width * 0.4f,
				                                  	 _position.yMin + kSpacingAfterEachElement,
				                                 	 _position.width * 0.6f,
				                                  	 kButtonHeight);
				float	_buttonWidth			= _buttonGroupRect.width / _menuButtonCount;

				// Set child properties rect
				_childPropertiesPosition		= new Rect(_position.xMin,
				                                     _buttonGroupRect.yMax + kExtraSpacingAfterButton,
				                                     _position.width,
				                                     0f);
				                                 
				// Draw button group
				GUI.BeginGroup(_buttonGroupRect);
				{
					Rect	_buttonRect			= new Rect(0f, 0f, _buttonWidth, _buttonGroupRect.height);

					for (int _iter = 0; _iter < _menuButtonCount; _iter++)
					{
						GUIContent	_buttonName		= _menuButtons[_iter];
						string		_buttonStyle	= (_iter == 0) ? Constants.kButtonLeftStyle : (_iter == (_menuButtonCount - 1) ? Constants.kButtonRightStyle : Constants.kButtonMidStyle);

						if (GUI.Button(_buttonRect, _buttonName, _buttonStyle))
							OnMenuButtonPressed(_iter);

						// Update rect
						_buttonRect.x			= _buttonRect.xMax;
					}
				}
				GUI.EndGroup();	
			}
		}
		
		#endregion

		#region Abstract Methods

		protected abstract GUIContent[] GetMenuButtonNames ();
		protected abstract void OnMenuButtonPressed (int _buttonIndex);

		#endregion
	}
}
#endif
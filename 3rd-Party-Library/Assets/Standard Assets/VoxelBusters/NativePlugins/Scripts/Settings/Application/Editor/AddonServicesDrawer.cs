using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
using System.Collections.Generic;

namespace VoxelBusters.NativePlugins
{
	using Internal;

	[CustomPropertyDrawer(typeof(ApplicationSettings.AddonServices))]
	public class AddonServicesDrawer : PropertyDrawer 
	{
		#region Constants
		
		private		const	float		kSpacingAfterEachElement	= 2f;

		#endregion

		#region Fields

		private				Dictionary<string, System.Action>	m_downloadActionCollection		= new Dictionary<string, System.Action>()
		{
			{ "m_usesSoomlaGrow", 	SoomlaGrowServiceUtility.DownloadSDK },
			{ "m_usesOneSignal", 	OneSignalServiceUtility.DownloadSDK }
		};

		#endregion

		#region Drawer Methods
		
		public override float GetPropertyHeight (SerializedProperty _property, GUIContent _label) 
		{
			return EditorGUI.GetPropertyHeight(_property);
		}
		
		public override void OnGUI (Rect _position, SerializedProperty _property, GUIContent _label) 
		{
			_label	= EditorGUI.BeginProperty(_position, _label, _property);

			// Calculate rectangle
			Rect	_foldoutRect	= new Rect(_position.x, _position.y, _position.width, EditorGUIUtility.singleLineHeight);

			// Draw property label
			_property.isExpanded	= EditorGUI.Foldout(_foldoutRect, _property.isExpanded, _label);

			if (_property.isExpanded)
			{
				float	_buttonWidth		= Mathf.Min(80f, Screen.width * 0.25f);
				Rect	_childPropertyRect	= new Rect(_position.x, _foldoutRect.yMax, _position.width - _buttonWidth, EditorGUIUtility.singleLineHeight);
				Rect	_downloadButtonRect	= new Rect(_childPropertyRect.xMax, _foldoutRect.yMax, _buttonWidth, EditorGUIUtility.singleLineHeight);

				// Move identation to next level
				EditorGUI.indentLevel++;

				// Draw child properties
				foreach (string _propertyName in m_downloadActionCollection.Keys)
				{
					SerializedProperty	_childProperty	= _property.FindPropertyRelative(_propertyName);

					if (_childProperty == null)
						continue;
		
					EditorGUI.PropertyField(_childPropertyRect, _childProperty);
					
					if (GUI.Button(_downloadButtonRect, "Download"))
					{
						System.Action	_downloadAction	= m_downloadActionCollection[_propertyName];

						if (_downloadAction != null)
							_downloadAction();
					}
					
					_childPropertyRect.y	= _childPropertyRect.yMax + kSpacingAfterEachElement;
					_downloadButtonRect.y	= _downloadButtonRect.yMax + kSpacingAfterEachElement;
				}

				// Reset indendation to what it was
				EditorGUI.indentLevel--;
			}
			
			EditorGUI.EndProperty();
		}
		
		#endregion
	}
}
#endif
#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;

namespace VoxelBusters.Utility
{
	[CustomPropertyDrawer(typeof(EnumMaskFieldAttribute))]
	public class EnumMaskFieldDrawer : PropertyDrawer 
	{
		#region Properties
		
		private EnumMaskFieldAttribute EnumMaskFieldAttribute 
		{ 
			get 
			{ 
				return (EnumMaskFieldAttribute)attribute; 
			} 
		}
		
		#endregion

		#region Drawer Methods

		public override void OnGUI (Rect _position, SerializedProperty _property, GUIContent _label)
		{
			_label	= EditorGUI.BeginProperty(_position, _label, _property);

			if (EnumMaskFieldAttribute.IsEnum())
			{
				EditorGUI.BeginChangeCheck();
#if UNITY_2017_3_OR_NEWER
				System.Enum _enumValue	= EditorGUI.EnumFlagsField(_position, _label, EnumMaskFieldAttribute.GetEnumValue(_property));
#else
				System.Enum _enumValue	= EditorGUI.EnumMaskField(_position, _label, EnumMaskFieldAttribute.GetEnumValue(_property));
#endif
				if (EditorGUI.EndChangeCheck())
					_property.intValue	= System.Convert.ToInt32(_enumValue);
				
			}
			else
			{
				base.OnGUI(_position, _property, _label);
			}

			EditorGUI.EndProperty();
		}
		
		#endregion
	}
}
#endif
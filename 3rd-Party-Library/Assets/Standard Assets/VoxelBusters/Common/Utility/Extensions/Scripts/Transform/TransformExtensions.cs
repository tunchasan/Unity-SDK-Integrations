using UnityEngine;
using System.Collections;

namespace VoxelBusters.Utility
{
	public static class TransformExtensions
	{
		#region Add Child Methods

		public static void AddChild (this Transform _parentTransform, GameObject _childGameObject, Vector3 _localPosition, Quaternion _localRotation, Vector3 _localScale)
		{
			Transform _childTransform		= _childGameObject.transform;

			// Set as parent
			_childTransform.parent			= _parentTransform;

			// Set child properties
			_childTransform.localPosition	= _localPosition;
			_childTransform.localRotation	= _localRotation;
			_childTransform.localScale		= _localScale;
		}

		public static void AddChild (this RectTransform _parentTransform, GameObject _childGameObject, Vector3 _localPosition, Quaternion _localRotation, Vector3 _localScale)
		{
			Transform _childTransform		= _childGameObject.transform;

			// Set as parent
			_childTransform.SetParent(_parentTransform, false); 

			// Set child properties
			_childTransform.localPosition	= _localPosition;
			_childTransform.localRotation	= _localRotation;
			_childTransform.localScale		= _localScale;
		}

		#endregion

		#region Property Methods

		public static string GetPath (this Transform _transform)
		{
			if (_transform == null)
				return null;

			Transform _parentTransform	= _transform.parent;

			if (_parentTransform == null)
				return "/" + _transform.name;

			return _parentTransform.GetPath() + "/" + _transform.name;
		}

		#endregion
	}
}
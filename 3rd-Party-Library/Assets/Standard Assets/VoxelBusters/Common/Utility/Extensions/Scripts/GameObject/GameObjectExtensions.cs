using UnityEngine;
using System.Collections;

namespace VoxelBusters.Utility
{
	public static class GameObjectExtensions  
	{
		#region Add Child Methods

		public static GameObject CreateChild (this GameObject _parentGameObject, string _childName, Vector3 _localPosition, Quaternion _localRotation, Vector3 _localScale)
		{
			GameObject	_childGameObject	= new GameObject(_childName);
			_parentGameObject.AddChild(_childGameObject, _localPosition, _localRotation, _localScale);

			return _childGameObject;
		}

		public static void AddChild (this GameObject _parentGameObject, GameObject _childGameObject, Vector3 _localPosition, Quaternion _localRotation, Vector3 _localScale)
		{
			_parentGameObject.transform.AddChild(_childGameObject, _localPosition, _localRotation, _localScale);
		}

		#endregion

		#region Component Methods

		public static T AddInvisibleComponent<T> (this GameObject _gameObject) where T : MonoBehaviour
		{
			T _newComponent			= _gameObject.AddComponent<T>();
			_newComponent.hideFlags	= HideFlags.HideInInspector;

			return _newComponent;
		}

		#endregion

		#region Property Methods

		public static string GetPath (this GameObject _gameObject)
		{
			if (_gameObject == null)
				return null;

			return _gameObject.transform.GetPath();
		}

		#endregion
	}
}
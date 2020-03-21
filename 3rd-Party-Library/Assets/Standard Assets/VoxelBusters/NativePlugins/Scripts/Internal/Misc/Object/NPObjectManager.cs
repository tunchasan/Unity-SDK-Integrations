using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace VoxelBusters.NativePlugins.Internal
{
	public class NPObjectManager 
	{
		public enum eCollectionType
		{
			GAME_SERVICES
		}

		#region Static Fields

		private static Dictionary<string, NPObject> 	gameServicesObjectCollection	= new Dictionary<string, NPObject>();

		#endregion

		#region Static Methods

		public static void AddNewObjectToCollection (NPObject _newObject, eCollectionType _collectionType)
		{
			Dictionary<string, NPObject>	_collectionDict = null;

			switch (_collectionType)
			{
			case eCollectionType.GAME_SERVICES:
				_collectionDict 	= gameServicesObjectCollection;
				break;
			}
		
			_collectionDict.Add(_newObject.GetInstanceID(), _newObject);
		}

		public static T GetObjectWithInstanceID <T> (string _instanceID, eCollectionType _collectionType) where T : NPObject
		{
			Dictionary<string, NPObject>	_collectionDict = null;
			NPObject						_object;

			switch (_collectionType)
			{
			case eCollectionType.GAME_SERVICES:
				_collectionDict 	= gameServicesObjectCollection;
				break;
			}

			_collectionDict.TryGetValue(_instanceID, out _object);

			return (T)_object;
		}

		#endregion
	}
}
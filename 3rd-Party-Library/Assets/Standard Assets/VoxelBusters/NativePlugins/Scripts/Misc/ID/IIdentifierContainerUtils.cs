using UnityEngine;
using System.Collections;

namespace VoxelBusters.NativePlugins
{
	public static class IIdentifierContainerUtils
	{
		#region Static Methods
		
		public static IIdentifierContainer FindObjectWithGlobalID (this IIdentifierContainer[] _collection, string _globalID)
		{
			foreach (IIdentifierContainer _currentObject in _collection)
			{
				if (string.Equals(_currentObject.GlobalID, _globalID))
					return _currentObject;
			}
			
			// Couldn't find a matching identifier
			Debug.Log(string.Format("[IDContainer] Couldn't find id container with global ID= {0}.", _globalID));

			return null;
		}
		
		public static IIdentifierContainer FindObjectWithPlatformID (this IIdentifierContainer[] _collection, string _platformID)
		{
			foreach (IIdentifierContainer _currentObject in _collection)
			{
				string 	_currentObjectPlatformID	= _currentObject.GetCurrentPlatformID();
				if (string.Equals(_currentObjectPlatformID, _platformID))
					return _currentObject;
			}
			
			// Couldn't find a matching identifier
			Debug.Log(string.Format("[IDContainer] Couldn't find id container with platform ID= {0}.", _platformID));

			return null;
		}

		public static string GetCurrentPlatformID (this IIdentifierContainer _object)
		{
			PlatformValue _platform = PlatformValueHelper.GetCurrentPlatformValue(_array: _object.PlatformIDs);
			if (_platform == null)
				return null;

			return _platform.Value;
		}

		#endregion
	}
}
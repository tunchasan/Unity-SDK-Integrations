using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace VoxelBusters.Utility
{
    public static class IDictionaryExtensions
    {
        public static bool ContainsKeyPath(this IDictionary _sourceDictionary, string _keyPath)
        {
            if (string.IsNullOrEmpty(_keyPath))
                return false;

            try
            {
                string[] _pathComponents = _keyPath.Split('/');
                int _count = _pathComponents.Length;
                IDictionary _currentDict = _sourceDictionary;

                for (int _pIter = 0; _pIter < _count; _pIter++)
                {
                    string _key = _pathComponents[_pIter];

                    if (_currentDict == null || !_currentDict.Contains(_key))
                        return false;

                    // Update reference to object at current key path
                    _currentDict = _currentDict[_key] as IDictionary;
                }

                return true;
            }
            catch (System.Exception _exception)
            {
                Debug.LogWarning("[IDictionaryExtensions] " + _exception.Message);

                return false;
            }
        }

        public static T GetIfAvailable<T>(this IDictionary _sourceDictionary, string _key, T _defaultValue = default(T))
        {
            try
            {
                if (_key == null || !_sourceDictionary.Contains(_key))
                    return _defaultValue;

                object _value = _sourceDictionary[_key];
                Type _targetType = typeof(T);

                if (_value == null)
                    return _defaultValue;

                if (_targetType.IsInstanceOfType(_value))
                    return (T)_value;

#if !NETFX_CORE
                if (_targetType.IsEnum)
#else
    			if (_targetType.GetTypeInfo().IsEnum)
#endif
                {
                    return (T)Enum.ToObject(_targetType, _value);
                }
                else
                {
                    return (T)System.Convert.ChangeType(_value, _targetType);
                }
            }
            catch(Exception e)
            {
                Debug.LogError("Exception while converting for key : " + _key + " " + e);
                return _defaultValue;
		    }
        }

		public static T GetIfAvailable <T> (this IDictionary _sourceDictionary, string _key, string _path)
		{
			//Trim path at start
			if(_path != null)
			{
				//Trim start and end slash if exists.
				_path = _path.TrimStart('/').TrimEnd('/');
			}

			if(!string.IsNullOrEmpty(_key))
			{

				if(string.IsNullOrEmpty(_path))
				{
					return _sourceDictionary.GetIfAvailable<T>(_key);
				}
				else
				{
					string[] _pathComponents = _path.Split('/');
					IDictionary _currentDict = _sourceDictionary;

					//Here traverse to the path
					foreach(string _each in _pathComponents)
					{
						if(_currentDict.Contains(_each))
						{
							_currentDict = _currentDict[_each] as IDictionary;
						}
						else
						{
							Debug.LogError("Path not found " + _path);
							return default(T);
						}
					}
					
					return _currentDict.GetIfAvailable<T>(_key);
				}
			}
			else
			{
				return default(T);
			}
		}

		public static string GetKey <T> (this IDictionary _sourceDictionary, T _value)
		{
			string _key = null;

			if(_value != null)
			{
				ICollection _keys = _sourceDictionary.Keys;

				foreach (string _eachKey in _keys)	
				{
					object _eachValue = _sourceDictionary[_eachKey] as object;

					if (_eachValue != null && _eachValue.Equals(_value))
					{
						_key = _eachKey;
						break;
					}
				}
			}

			return _key;
		}
	}
}

using UnityEngine;

#if USES_CLOUD_SERVICES && UNITY_ANDROID
using System.Collections;
using System.Collections.Generic;
using VoxelBusters.Utility;

namespace VoxelBusters.NativePlugins
{
	public partial class CloudServicesAndroid : CloudServices
	{		

		#region Fields
			
		private bool 	m_isLocalMemoryDirty 			= false;
		private string 	m_currentCloudAccountName 	= null;
		private int	 	m_syncIndex 				=	0;
		
		#endregion

		#region In-Memory Data Store Methods

		private void SetValue(string _key, object _value)
		{
			m_dataStore[_key] = _value;
			m_isLocalMemoryDirty = true;
		}

		private T GetValue<T>(string _key)
		{
			return m_dataStore.GetIfAvailable<T>(_key);
		}

		private void RemoveKeyValuePair(string _key)
		{
			m_dataStore.Remove(_key);
			m_isLocalMemoryDirty = true;
		}

		private void RemoveAllKeyValuePairs()
		{
			m_dataStore.Clear ();
			m_isLocalMemoryDirty = true;
		}

		private void LoadLocalDataStore()
		{
			m_dataStore = GetLocalStoreDiskData();

			if (m_dataStore == null)
			{
				m_dataStore = new Dictionary<string, object>();
			}
		}

		private void RefreshLocalStoreMemoryData(ICollection _changedKeys, IDictionary _cloudData)
		{				
			foreach(string _eachKey in _changedKeys)
			{
				m_dataStore[_eachKey] = _cloudData[_eachKey];
			}
		}

		private bool IsLocalDirty()
		{
			return m_isLocalMemoryDirty;
		}

		private void SetLocalDirty(bool _status)
		{
			m_isLocalMemoryDirty = _status;
		}

		#endregion

		#region Saved Local Data Store Methods

		private IDictionary GetLocalStoreDiskData()
		{
			string _encodedLocalData = PlayerPrefs.GetString(kKeyForCloudServicesLocalStore, null);
			
			if (_encodedLocalData != null)
			{
				string _decodedLocalData = _encodedLocalData.FromBase64();
				return (IDictionary)JSONUtility.FromJSON(_decodedLocalData);
			}

			return null;
		}
		
	private void SetLocalStoreDiskData(IDictionary _newData)
		{
			string _newDataString = null;

			if (_newData != null)
			{
				_newDataString = _newData.ToJSON();
				_newDataString = _newDataString.ToBase64();
			}

			SetLocalStoreDiskData(_newDataString);
		}

		private void SetLocalStoreDiskData(string _dataStr)
		{
			PlayerPrefs.SetString(kKeyForCloudServicesLocalStore, _dataStr);
		}
		
		#endregion	

		#region Event Trigger Methods
		
		private void CheckChangedKeyValueStoreDataAndRefresh(IDictionary _newCloudData, string _cloudAccountName)
		{
			
			//Load from player prefs first.
			IDictionary _localData 							= GetLocalStoreDiskData();
			List<string> _changedKeys 						= null; 			
			eCloudDataStoreValueChangeReason _changeReason;
			
			if (_localData != null)
			{
				_changedKeys = GetChangedKeys(_localData, _newCloudData);
				
				// Trigger event here telling these keys got changed.

				if (!_cloudAccountName.Equals(m_currentCloudAccountName))
				{
					// Set the account name to latest
					m_currentCloudAccountName = _cloudAccountName;
					_changeReason = eCloudDataStoreValueChangeReason.STORE_ACCOUNT;	
				}
				else
				{
					_changeReason = eCloudDataStoreValueChangeReason.SERVER;
				}
			}
			else
			{
				_changeReason 	= eCloudDataStoreValueChangeReason.STORE_ACCOUNT;
				ICollection<string> _keysCollection 	= (ICollection<string>)_newCloudData.Keys;	

				_changedKeys	= new List<string>(_keysCollection);	
			}

			// Refresh the keys in in-memory store.
			RefreshLocalStoreMemoryData(_changedKeys, _newCloudData);	

			if (!m_isInitialised)
			{
				CloudKeyValueStoreDidInitialise(true);
				_changeReason 	= eCloudDataStoreValueChangeReason.INITIAL_SYNC;
			}
				
			// Save the synced local store data
			SetLocalStoreDiskData(_newCloudData);

			if (_changedKeys != null && _changedKeys.Count > 0)
			{	
				CloudKeyValueStoreDidChangeExternally(_changeReason, _changedKeys.ToArray());			
			}
		}
		
		private List<string> GetChangedKeys(IDictionary _localData, IDictionary _cloudData)
		{
			List<string> _changedKeys 	= new List<string>();
			
			// Merging and getting the total keys to reflect any removed keys.
			List<string> _totalKeys 	= MergeList(_localData.Keys, _cloudData.Keys);
			
			foreach(string _eachKey in _totalKeys)
			{
				object _eachCloudValue	= _cloudData.GetIfAvailable<object>(_eachKey);
				object _eachLocalValue 	= _localData.GetIfAvailable<object>(_eachKey);
				
				// If the values don't match, we can tell the keys got changed.
				if (!JSONUtility.ToJSON(_eachLocalValue).Equals(JSONUtility.ToJSON(_eachCloudValue)))
				{
					_changedKeys.Add(_eachKey);
				}
			}
			
			return _changedKeys;
		}

		#endregion

		#region Cloud Methods

		private void SyncWithCloud()
		{
			m_isSyncInProgress = true;
			m_syncIndex++;
			Plugin.Call(Native.Methods.LOAD_CLOUD_DATA);
		}

		#endregion

		#region Common Methods
	
		private List<string> MergeList(ICollection _keyList1, ICollection _keyList2)
		{
			List<string> _mergedKeys	 	= new List<string>();

			foreach(string _each in _keyList1)
			{
				if (!_mergedKeys.Contains(_each))
				{
					_mergedKeys.Add(_each);
				}
			}

			foreach(string _each in _keyList2)
			{
				if (!_mergedKeys.Contains(_each))
				{
					_mergedKeys.Add(_each);
				}
			}	

			return _mergedKeys;
		}
	
		#endregion
	}
}
#endif
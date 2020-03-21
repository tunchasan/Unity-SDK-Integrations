#if USES_CLOUD_SERVICES && UNITY_ANDROID
using UnityEngine;
using System.Collections;
using VoxelBusters.Utility;
using VoxelBusters.UASUtils;

namespace VoxelBusters.NativePlugins
{
	using Internal;

	public partial class CloudServicesAndroid : CloudServices 
	{
		#region  Constants
			
		private 	const 	string		kKeyForNewCloudData			= "new-cloud-data";
		private 	const 	string		kKeyForIsFetchedForSave		= "is-fetched-for-save";		
		private 	const 	string		kKeyForIsCommitSuccess		= "is-commit-success";	
		private 	const 	string		kKeyForCloudAccount			= "cloud-account-name";	

		private 	const 	string		kKeyForCloudServicesLocalStore 	= "np-cloud-services-local-store";
		
		#endregion
	

		#region Native Callback Methods

		private void DidReceiveNewCloudData (string _dataStr)
		{
			IDictionary		_dataDict			=	(IDictionary)JSONUtility.FromJSON(_dataStr);
			string			_encodedData		=	_dataDict.GetIfAvailable<string>(kKeyForNewCloudData);
			string 			_cloudAccountName	=	_dataDict.GetIfAvailable<string>(kKeyForCloudAccount);



			// Decode the data with Base64 and convert to Json IDict.
			if(!string.IsNullOrEmpty(_encodedData))
			{
				string _decodedData = _encodedData.FromBase64();

				DebugUtility.Logger.Log(Constants.kDebugTag, "[CloudServices] Received from Cloud : " + _decodedData);
				
				IDictionary _newCloudData = (IDictionary)JSONUtility.FromJSON(_decodedData);

				// Here compare the data with the one on local disk and trigger the ExternallyChagedEvent.
				CheckChangedKeyValueStoreDataAndRefresh(_newCloudData, _cloudAccountName);
			}
			else if (!m_isInitialised)
			{
				CloudKeyValueStoreDidInitialise(true);
			}

			//Set the flag to true
			m_isInitialised = true;


			if (IsLocalDirty())
			{
				SetLocalDirty(false); //We are setting to false and on failed commit to cloud we set it to true. If we don't set here, there could be chances for missing the flagstatus if set to false in callback on sucess of save.
				string _jsonString = m_dataStore.ToJSON();
				DebugUtility.Logger.Log(Constants.kDebugTag, "[CloudServices] Save To Cloud : " + _jsonString);
				Plugin.Call(Native.Methods.SAVE_CLOUD_DATA, _jsonString.ToBase64());
			}
			else
			{
				FinishedSync(true);
			}
		}

		private void DidReceiveErrorOnLoadCloudData(string _dataStr)
		{
			FinishedSync(false);			
		}

		private void DidFinishCommitingToCloud(string _dataStr)
		{
			IDictionary		_dataDict			=	(IDictionary)JSONUtility.FromJSON(_dataStr);
			string			_encodedData		=	_dataDict.GetIfAvailable<string>(kKeyForNewCloudData);
			bool 			_isSuccess			=	_dataDict.GetIfAvailable<bool>(kKeyForIsCommitSuccess);

			// If success, just make sure we write the new commited data to local disk.
			if (_isSuccess)
			{
				SetLocalStoreDiskData(_encodedData);
			}
			else // Don't do anything.
			{
				Debug.LogError("Failed commiting to cloud!!!");
			}

			FinishedSync(_isSuccess);
		}

		private void FinishedSync(bool _isSuccess)
		{
			if (!_isSuccess)
			{
				SetLocalDirty(true);
			}

			m_lastSyncTimeStamp = Time.realtimeSinceStartup;
			m_isSyncInProgress = false;
			m_syncIndex--;

			if (m_syncIndex <= 0)
			{
				if (m_isSyncManually)
				{
					CloudKeyValueStoreDidSynchronise(_isSuccess);
					m_isSyncManually = false;
				}

				m_syncIndex = 0;
			}
		}
		
		#endregion

	}
}
#endif
using UnityEngine;
using System.Collections;

#if USES_CLOUD_SERVICES && UNITY_IOS
using VoxelBusters.Utility;

namespace VoxelBusters.NativePlugins
{
	using Internal;

	public partial class CloudServicesIOS : CloudServices 
	{
		private enum NSUbiquitousKeyValueStoreChangeReason
		{
			NSUbiquitousKeyValueStoreServerChange,
			NSUbiquitousKeyValueStoreInitialSyncChange,
			NSUbiquitousKeyValueStoreQuotaViolationChange,
			NSUbiquitousKeyValueStoreAccountChange
		}

		#region  Constants
			
		private 	const 	string		kKeyForValueChangedKeys		= "keys";
		private 	const 	string		kKeyForChangeReason			= "reason";

		#endregion

		#region Native Callback Methods
		
		protected override void CloudKeyValueStoreDidChangeExternally (string _dataStr)
		{
			IDictionary		_dataDict				= (IDictionary)JSONUtility.FromJSON(_dataStr);
			IList			_changedKeysJSONList	= _dataDict.GetIfAvailable<IList>(kKeyForValueChangedKeys);
			eCloudDataStoreValueChangeReason _changeReason	= ConvertToUnityFormatChangeReason(_dataDict.GetIfAvailable<NSUbiquitousKeyValueStoreChangeReason>(kKeyForChangeReason));

			// Copy keys to string array
			string[] 		_changedKeysArray	= null;

			if (_changedKeysJSONList != null)
			{
				_changedKeysArray				= new string[_changedKeysJSONList.Count];

				_changedKeysJSONList.CopyTo(_changedKeysArray, 0);
			}

			// Invoke handler
			CloudKeyValueStoreDidChangeExternally(_changeReason, _changedKeysArray);
		}

		private eCloudDataStoreValueChangeReason ConvertToUnityFormatChangeReason (NSUbiquitousKeyValueStoreChangeReason _reason)
		{
			switch (_reason)
			{
			case NSUbiquitousKeyValueStoreChangeReason.NSUbiquitousKeyValueStoreServerChange:
				return eCloudDataStoreValueChangeReason.SERVER;

			case NSUbiquitousKeyValueStoreChangeReason.NSUbiquitousKeyValueStoreInitialSyncChange:
				return eCloudDataStoreValueChangeReason.INITIAL_SYNC;

			case NSUbiquitousKeyValueStoreChangeReason.NSUbiquitousKeyValueStoreQuotaViolationChange:
				return eCloudDataStoreValueChangeReason.QUOTA_VIOLATION;

			case NSUbiquitousKeyValueStoreChangeReason.NSUbiquitousKeyValueStoreAccountChange:
				return eCloudDataStoreValueChangeReason.STORE_ACCOUNT;
			}

			throw new System.Exception("[CloudServices] Unhandled change reason.");
		}

		#endregion
	}
}
#endif
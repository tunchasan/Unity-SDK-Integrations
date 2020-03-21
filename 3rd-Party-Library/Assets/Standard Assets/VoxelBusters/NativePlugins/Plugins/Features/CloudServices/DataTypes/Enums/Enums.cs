using UnityEngine;
using System.Collections;

#if USES_CLOUD_SERVICES
namespace VoxelBusters.NativePlugins
{
	/// <summary>
	/// Possible reasons when cloud data changed event occurs.
	/// </summary>
	public enum eCloudDataStoreValueChangeReason
	{
		/// <summary> This occurs when another instance of your app using same cloud service account, uploads a new value. </summary>
		SERVER,

		/// <summary> This occurs when an attempt to write to key-value storage was discarded because an initial download from cloud server has not yet happened.</summary>
		INITIAL_SYNC,

		/// <summary> This occurs when your app’s key-value store has exceeded its space quota on the cloud server.</summary>
		QUOTA_VIOLATION,

		/// <summary> This occurs when user has changed the cloud service account. The keys and values in the local key-value store have been replaced with those from the new account.</summary>
		STORE_ACCOUNT
	}
}
#endif
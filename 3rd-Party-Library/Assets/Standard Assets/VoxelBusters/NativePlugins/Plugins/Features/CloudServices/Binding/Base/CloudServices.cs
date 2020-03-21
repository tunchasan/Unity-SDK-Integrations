using UnityEngine;
using System.Collections;

#if USES_CLOUD_SERVICES
namespace VoxelBusters.NativePlugins
{
	/// <summary>
	/// Provides a cross-platform interface to sync information across various devices by storing it in the cloud.
	/// </summary>
	/// <description>
	/// <para>
	/// You can use to make preference, configuration, and app-state data available to every instance of your app on every device connected to a user’s cloud service account. 
	/// You can store primitive values as well as object types: <c>IList</c> and <c>IDictionary</c>.
	/// </para>
	/// <para>
	/// When you use this feature on iOS device, iCloud service will be used. Whereas on Android, it will use Google Cloud service.
	/// For setup instructions on iOS, read <a href="https://developer.apple.com/library/ios/documentation/IDEs/Conceptual/AppDistributionGuide/AddingCapabilities/AddingCapabilities.html#//apple_ref/doc/uid/TP40012582-CH26-SW2">Adding iCloud Support</a>, 
	/// Similarly for Android, see <a href=""></a>.
	/// </para>
	/// </description>
	/// <remarks>
	/// \note
	/// <para>
	/// On iOS, the total amount of space available to store key-value data, for a given user, is 1 MB. There is a per-key value size limit of 1 MB, and a maximum of 1024 keys.
	/// If you attempt to write data that exceeds these quotas, the write attempt fails and no change is made to your cloud.
	/// In this scenario, the system posts the <see cref="DataStoreValueDidChangeEvent"/> with a change reason of <see cref="eCloudDataStoreValueChangeReason.QUOTA_VIOLATION"/>.
	/// </para>
	/// </remarks>
	public partial class CloudServices : MonoBehaviour 
	{
		#region Fields
	
		protected bool m_isInitialised = false;

		#endregion


		#region Initialise

		/// <summary>
		/// Initialises the component.
		/// </summary>
		///	<remarks> 
		/// \note You need to call this method, before using any features. 
		/// </remarks>
		public virtual void Initialise ()
		{}

		#endregion
	
		#region Setting Values

		/// <summary>
		/// Sets a Boolean value for the specified key in the cloud data store.
		/// </summary>
		/// <param name="_key">The key under which to store the value. The length of this key must not exceed 64 bytes.</param>
		/// <param name="_value">The Boolean value to store.</param>
		public virtual void SetBool (string _key, bool _value)
		{}

		/// <summary>
		/// Sets a long value for the specified key in the cloud data store.
		/// </summary>
		/// <param name="_key">The key under which to store the value. The length of this key must not exceed 64 bytes.</param>
		/// <param name="_value">The long value to store.</param>
		public virtual void SetLong (string _key, long _value)
		{}

		/// <summary>
		/// Sets a double value for the specified key in the cloud data store.
		/// </summary>
		/// <param name="_key">The key under which to store the value. The length of this key must not exceed 64 bytes.</param>
		/// <param name="_value">The double value to store.</param>
		public virtual void SetDouble (string _key, double _value)
		{}

		/// <summary>
		/// Sets a string value for the specified key in the cloud data store.
		/// </summary>
		/// <param name="_key">The key under which to store the value. The length of this key must not exceed 64 bytes.</param>
		/// <param name="_value">The string value to store.</param>
		public virtual void SetString (string _key, string _value)
		{}

		/// <summary>
		/// Sets a list object for the specified key in the cloud data store.
		/// </summary>
		/// <param name="_key">The key under which to store the value. The length of this key must not exceed 64 bytes.</param>
		/// <param name="_value">The list object whose contents has to be stored. The objects in the list must be <c>primitive</c>, <c>IList</c>, <c>IDictionary</c>.</param>
		public virtual void SetList (string _key, IList _value)
		{}

		/// <summary>
		/// Sets a dictionary object for the specified key in the cloud data store.
		/// </summary>
		/// <param name="_key">The key under which to store the value. The length of this key must not exceed 64 bytes.</param>
		/// <param name="_value">A dictionary whose contents has to be stored. The objects in the dictionary must be <c>primitive</c>, <c>IList</c>, <c>IDictionary</c>.</param>
		public virtual void SetDictionary (string _key, IDictionary _value)
		{}

		#endregion

		#region Getting Values

		/// <summary>
		/// Returns the Boolean value associated with the specified key.
		/// </summary>
		/// <returns>The Boolean value associated with the specified key, that value is returned. or <c>false</c> if the key was not found.</returns>
		/// <param name="_key">A string used to identify the value stored in the cloud data store.</param>
		public virtual bool GetBool (string _key)
		{
			return false;
		}
		
		/// <summary>
		/// Returns the long value associated with the specified key.
		/// </summary>
		/// <returns>The long value associated with the specified key or <c>0</c> if the key was not found.</returns>
		/// <param name="_key">A string used to identify the value stored in the cloud data store.</param>
		public virtual long GetLong (string _key)
		{
			return 0L;
		}
		
		/// <summary>
		/// Returns the double value associated with the specified key.
		/// </summary>
		/// <returns>The double value associated with the specified key or <c>0</c> if the key was not found.</returns>
		/// <param name="_key">A string used to identify the value stored in the cloud data store.</param>
		public virtual double GetDouble (string _key)
		{
			return 0D;
		}
		
		/// <summary>
		/// Returns the string value associated with the specified key.
		/// </summary>
		/// <returns>The string associated with the specified key, or <c>null</c> if the key was not found or its value is not an <c>string</c> object.</returns>
		/// <param name="_key">A string used to identify the value stored in the cloud data store.</param>
		public virtual string GetString (string _key)
		{
			return null;
		}
		
		/// <summary>
		/// Returns the list object associated with the specified key.		
		/// </summary>
		/// <returns>The list object associated with the specified key, or <c>null</c> if the key was not found or its value is not an <c>IList</c> object.</returns>
		/// <param name="_key">A string used to identify the value stored in the cloud data store.</param>
		public virtual IList GetList (string _key)
		{
			return null;
		}
		
		/// <summary>
		/// Returns the dictionary object associated with the specified key.
		/// </summary>
		/// <returns>The dictionary object associated with the specified key, or <c>null</c> if the key was not found or its value is not an <c>IDictionary</c> object.</returns>
		/// <param name="_key">A string used to identify the value stored in the cloud data store.</param>
		public virtual IDictionary GetDictionary (string _key)
		{
			return null;
		}

		/// <summary>
		/// Returns true if initialised.
		/// </summary>
		/// <returns>True if initialisation is finished. Else false.</returns>
		public bool IsInitialised ()
		{
			return m_isInitialised;
		}

		#endregion

		#region Sync Values

		/// <summary>
		/// Explicitly synchronizes in-memory data with those stored on disk.
		/// </summary>
		/// <remarks>
		/// \note <see cref="KeyValueStoreDidSynchroniseEvent"/> is triggered, when your app has completed processing synchronisation request. 
		/// </remarks>
		public virtual void Synchronise ()
		{}

		#endregion

		#region Remove Values

		/// <summary>
		/// Removes the value associated with the specified key from the cloud data store.
		/// </summary>
		/// <param name="_key">The key corresponding to the value you want to remove.</param>
		public virtual void RemoveKey (string _key)
		{}


		/// <summary>
		/// Removes the values associated with the all keys from the cloud data store.
		/// </summary>
		public virtual void RemoveAllKeys ()
		{}

		#endregion
	}
}
#endif
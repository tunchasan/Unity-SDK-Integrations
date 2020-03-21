using UnityEngine;
using System.Collections;

#if USES_CLOUD_SERVICES && UNITY_IOS
using System.Runtime.InteropServices;
using VoxelBusters.Utility;

namespace VoxelBusters.NativePlugins
{
	public partial class CloudServicesIOS : CloudServices 
	{
#region Private Methods

		private 	bool	m_canSendEvents		= false;

#endregion

#region External Methods
		
		[DllImport("__Internal")]
		private static extern void cpnpCloudServicesInitialise ();

		[DllImport("__Internal")]
		private static extern void cpnpCloudServicesSetBool (string _key, bool _value);
		
		[DllImport("__Internal")]
		private static extern void cpnpCloudServicesSetLong (string _key, long _value);
		
		[DllImport("__Internal")]
		private static extern void cpnpCloudServicesSetDouble (string _key, double _value);
		
		[DllImport("__Internal")]
		private static extern void cpnpCloudServicesSetString (string _key, string _value);

		[DllImport("__Internal")]
		private static extern void cpnpCloudServicesSetList (string _key, string _value);

		[DllImport("__Internal")]
		private static extern void cpnpCloudServicesSetDictionary (string _key, string _value);

		[DllImport("__Internal")]
		private static extern bool cpnpCloudServicesGetBool (string _key);
		
		[DllImport("__Internal")]
		private static extern long cpnpCloudServicesGetLong (string _key);
		
		[DllImport("__Internal")]
		private static extern double cpnpCloudServicesGetDouble (string _key);
		
		[DllImport("__Internal")]
		private static extern string cpnpCloudServicesGetString (string _key);
		
		[DllImport("__Internal")]
		private static extern string cpnpCloudServicesGetList (string _key);
		
		[DllImport("__Internal")]
		private static extern string cpnpCloudServicesGetDictionary (string _key);
		
		[DllImport("__Internal")]
		private static extern bool cpnpCloudServicesSynchronise ();
		
		[DllImport("__Internal")]
		private static extern void cpnpCloudServicesRemoveKey (string _key);

		[DllImport("__Internal")]
		private static extern void cpnpCloudServicesRemoveAllKeys ();


#endregion

#region Initialise

		public override void Initialise ()
		{
			base.Initialise();

			// Native method call
			cpnpCloudServicesInitialise();
			Synchronise();

			// reset flag
			m_canSendEvents	= true;

            m_isInitialised = true;

			// send corresponding event
			CloudKeyValueStoreDidInitialise(_isSuccess: true);
		}

#endregion
	
#region Setting Values

		public override void SetBool (string _key, bool _value)
		{
			cpnpCloudServicesSetBool(_key, _value);
		}
		
		public override void SetLong (string _key, long _value)
		{
			cpnpCloudServicesSetLong(_key, _value);
		}

		public override void SetDouble (string _key, double _value)
		{
			cpnpCloudServicesSetDouble(_key, _value);
		}

		public override void SetString (string _key, string _value)
		{
			cpnpCloudServicesSetString(_key, _value);
		}
		
		public override void SetList (string _key, IList _value)
		{
			cpnpCloudServicesSetList(_key, _value == null ? null : _value.ToJSON());
		}
		
		public override void SetDictionary (string _key, IDictionary _value)
		{
			cpnpCloudServicesSetDictionary(_key, _value == null ? null : _value.ToJSON());
		}

#endregion

#region Getting Values

		public override bool GetBool (string _key)
		{
			return cpnpCloudServicesGetBool(_key);
		}
		
		public override long GetLong (string _key)
		{
			return cpnpCloudServicesGetLong(_key);
		}
		
		public override double GetDouble (string _key)
		{
			return cpnpCloudServicesGetDouble(_key);
		}
		
		public override string GetString (string _key)
		{
			return cpnpCloudServicesGetString(_key);
		}
		
		public override IList GetList (string _key)
		{
			string _JSONString	= cpnpCloudServicesGetList(_key);
			
			return (_JSONString == null) ? null : (IList)JSONUtility.FromJSON(_JSONString);
		}
		
		public override IDictionary GetDictionary (string _key)
		{
			string _JSONString	= cpnpCloudServicesGetDictionary(_key);

			return (_JSONString == null) ? null : (IDictionary)JSONUtility.FromJSON(_JSONString);
		}

#endregion

#region Sync Values

		public override void Synchronise ()
		{
			bool _success	= cpnpCloudServicesSynchronise();

			// send events
			if (m_canSendEvents)
			{
				CloudKeyValueStoreDidSynchronise(_success);
			}
		}
		
#endregion

#region Remove Values

		public override void RemoveKey (string _key)
		{
			cpnpCloudServicesRemoveKey(_key);
		}

		public override void RemoveAllKeys ()
		{
			cpnpCloudServicesRemoveAllKeys();
		}

#endregion
	}
}
#endif
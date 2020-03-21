using UnityEngine;
using System.Collections;

#if USES_CLOUD_SERVICES && UNITY_ANDROID
using System.Runtime.InteropServices;
using VoxelBusters.Utility;

namespace VoxelBusters.NativePlugins
{
	public partial class CloudServicesAndroid : CloudServices 
	{		

		#region Fields

		private	IDictionary		m_dataStore;
		private bool			m_isSyncInProgress;
		private bool			m_isSyncManually;
		private double			m_lastSyncTimeStamp;			
		private bool			m_isInternalLibraryInitialised;			
		
		#endregion

		#region Constructors
		
		CloudServicesAndroid()
		{
			Plugin = AndroidPluginUtility.GetSingletonInstance(Native.Class.NAME);
		}

		#endregion

		#region Life Cylce
		private void Awake()
		{
			// Load here m_dataStore from local disk
			LoadLocalDataStore();
		}
	
		#endregion


		#region Initialise

		public override void Initialise ()
		{
			base.Initialise();
			
			// Native method call
			Plugin.Call(Native.Methods.INITIALISE);

			m_isInternalLibraryInitialised = true;
		}

		private bool IsSignedIn()
		{
			if (Plugin != null)
			{
				return Plugin.Call<bool>(Native.Methods.IS_SIGNED_IN);
			}
		
			return false;
		}

		#endregion
	
		#region Setting Values

		public override void SetBool (string _key, bool _value)
		{
			SetValue(_key, _value);
		}
		
		public override void SetLong (string _key, long _value)
		{
			SetValue(_key, _value);
		}

		public override void SetDouble (string _key, double _value)
		{
			SetValue(_key, _value);
		}

		public override void SetString (string _key, string _value)
		{
			SetValue(_key, _value);
		}
		
		public override void SetList (string _key, IList _value)
		{
			SetValue(_key, _value == null ? null : _value.ToJSON());
		}
		
		public override void SetDictionary (string _key, IDictionary _value)
		{
			SetValue(_key, _value == null ? null : _value.ToJSON());
		}

		#endregion

		#region Getting Values

		public override bool GetBool (string _key)
		{
			return GetValue<bool>(_key);
		}
		
		public override long GetLong (string _key)
		{
			return GetValue<long>(_key);
		}
		
		public override double GetDouble (string _key)
		{
			return GetValue<double>(_key);
		}
		
		public override string GetString (string _key)
		{
			return GetValue<string>(_key);
		}
		
		public override IList GetList (string _key)
		{
			string _JSONString	= GetValue<string>(_key);
			
			return (_JSONString == null) ? null : (IList)JSONUtility.FromJSON(_JSONString);
		}
		
		public override IDictionary GetDictionary (string _key)
		{
			string _JSONString	= GetValue<string>(_key);
			
			return (_JSONString == null) ? null : (IDictionary)JSONUtility.FromJSON(_JSONString);
		}

		#endregion

		#region Sync Values

		public override void Synchronise ()
		{
			m_isSyncManually = true;
			SyncWithCloud();
		}
		
		#endregion

		#region Remove Values

		public override void RemoveKey (string _key)
		{
			RemoveKeyValuePair(_key);
		}

		public override void RemoveAllKeys ()
		{
			RemoveAllKeyValuePairs();
		}

		#endregion

		#region Unity Methods

		private void Update()
		{
			if (m_isInternalLibraryInitialised)
			{
				// Here schedule a load for every interval
				double _elapsedTime = (Time.realtimeSinceStartup - m_lastSyncTimeStamp);

				if (!m_isSyncInProgress && _elapsedTime > NPSettings.CloudServices.Android.SyncInterval)
				{
					SyncWithCloud();
				}
			}
		}

        private void OnApplicationPause(bool pause)
        {
            if(pause)
            {
                if(!m_isSyncInProgress)
                {
                    SyncWithCloud();
                }
            }
        }

        #endregion
    }
}
#endif
using UnityEngine;
using System.Collections;

#if USES_CLOUD_SERVICES && UNITY_EDITOR
using UnityEditor;
using System;
using VoxelBusters.Utility;

namespace VoxelBusters.NativePlugins
{
	public partial class CloudServicesEditor : CloudServices 
	{

		#region Initialise

		public override void Initialise ()
		{
			base.Initialise();
			CloudKeyValueStoreDidInitialise(true);
			m_isInitialised = true;
		}

		#endregion

		#region Setting Values

		public override void SetBool (string _key, bool _value)
		{
			EditorPrefs.SetInt(_key, Convert.ToInt32(_value));
		}
		
		public override void SetLong (string _key, long _value)
		{
			EditorPrefs.SetInt(_key, (int)_value);
		}

		public override void SetDouble (string _key, double _value)
		{
			EditorPrefs.SetFloat(_key, (float)_value);
		}

		public override void SetString (string _key, string _value)
		{
			EditorPrefs.SetString(_key, _value);
		}
		
		public override void SetList (string _key, IList _value)
		{
			EditorPrefs.SetString(_key, _value == null ? null : _value.ToJSON());
		}
		
		public override void SetDictionary (string _key, IDictionary _value)
		{
			EditorPrefs.SetString(_key, _value == null ? null : _value.ToJSON());
		}

		#endregion

		#region Getting Values

		public override bool GetBool (string _key)
		{
			return (EditorPrefs.GetInt(_key) == 1) ? true : false;
		}
		
		public override long GetLong (string _key)
		{
			return EditorPrefs.GetInt(_key);
		}
		
		public override double GetDouble (string _key)
		{
			return EditorPrefs.GetFloat(_key);
		}
		
		public override string GetString (string _key)
		{
			return EditorPrefs.GetString(_key, null);
		}
		
		public override IList GetList (string _key)
		{
			string _JSONString	= EditorPrefs.GetString(_key, null);
			
			return (_JSONString == null) ? null : (IList)JSONUtility.FromJSON(_JSONString);
		}
		
		public override IDictionary GetDictionary (string _key)
		{
			string _JSONString	= EditorPrefs.GetString(_key, null);
		
			return (_JSONString == null) ? null : (IDictionary)JSONUtility.FromJSON(_JSONString);
		}

		#endregion

		#region Sync Values

		public override void Synchronise ()
		{
			// Send event
			CloudKeyValueStoreDidSynchronise(true);
		}
		
		#endregion

		#region Remove Values

		public override void RemoveKey (string _key)
		{
			EditorPrefs.DeleteKey(_key);
		}

		#endregion
	}
}
#endif
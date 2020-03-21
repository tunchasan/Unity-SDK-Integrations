using UnityEngine;
using System.Collections;
using VoxelBusters.Utility;
using System.Collections.Generic;
using System.Linq;

namespace VoxelBusters.NativePlugins.Demo
{
	using Internal;

#if !USES_CLOUD_SERVICES
	public class CloudServicesDemo : NPDisabledFeatureDemo 
	{}
#else
	public class CloudServicesDemo : NPDemoBase 
	{
		#region Fields

		private		const	string 		kKeyForBoolValue		= "bool-value";
		private		const	string 		kKeyForLongValue		= "long-key";
		private		const	string 		kKeyForDoubleValue		= "double-key";
		private		const	string 		kKeyForStringValue		= "string-key";
		private		const	string 		kKeyForDictionaryValue	= "dictionary-key";

		#endregion

		#region Fields

		private		bool	m_isInitialised		= false;
		private		bool	m_boolValue			= false;
		private		long	m_longValue			= 0L;
		private		double	m_doubleValue		= 0D;
		private		string	m_stringValue		= "";

		private		Dictionary<string, string> m_dictionaryValue = new Dictionary<string, string>();

		#endregion

		#region Unity 

		protected override void Start ()
		{
			base.Start();
			
			// Set additional info texts
			AddExtraInfoTexts(
				"Apart from saving primitive types, you can even store collection types such as IList, IDictionary. The objects in the collection must be primitive, IList, IDictionary.",
				"On iOS, the total amount of space available to store key-value data, for a given user, is 1 MB."
				);
		}

		protected override void OnEnable ()
		{
			base.OnEnable();

			// Register to event
			CloudServices.KeyValueStoreDidInitialiseEvent		+= OnKeyValueStoreDidInitialise;
			CloudServices.KeyValueStoreDidSynchroniseEvent		+= OnKeyValueStoreDidSynchronise;
			CloudServices.KeyValueStoreDidChangeExternallyEvent	+= OnKeyValueStoreChanged;
		}
		
		protected override void OnDisable ()
		{
			base.OnDisable();
			
			// Deregister to event
			CloudServices.KeyValueStoreDidInitialiseEvent		-= OnKeyValueStoreDidInitialise;
			CloudServices.KeyValueStoreDidSynchroniseEvent		-= OnKeyValueStoreDidSynchronise;
			CloudServices.KeyValueStoreDidChangeExternallyEvent	-= OnKeyValueStoreChanged;
		}

		#endregion

		#region GUI

		protected override void DisplayFeatureFunctionalities ()
		{
			base.DisplayFeatureFunctionalities();

			if (!m_isInitialised)
			{
				if (GUILayout.Button("Initialise"))
				{
					m_isInitialised	= true;

					NPBinding.CloudServices.Initialise();
				}

				GUILayout.Box("[NOTE] You must call Initialise, before using any features.");
			}
			else
			{
				DrawGetSetValuesSection();
				DrawSynchroniseSection();
				DrawRemoveValuesSection();
			}
		}

		private void DrawGetSetValuesSection ()
		{
			GUILayout.Label("Store, Retrieve Values", kSubTitleStyle);
			
			GUILayout.BeginHorizontal();
			{
				if (GUILayout.Button("Set Bool"))
					SetBool();

				if (GUILayout.Button("Get Bool"))
					GetBool();
			}
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
			{
				if (GUILayout.Button("Set Long"))
					SetLong();

				if (GUILayout.Button("Get Long"))
					GetLong();
			}
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
			{
				if (GUILayout.Button("Set Double"))
					SetDouble();

				if (GUILayout.Button("Get Double"))
					GetDouble();
			}
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
			{
				if (GUILayout.Button("Set String"))
					SetString();

				if (GUILayout.Button("Get String"))
					GetString();
			}
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			{
				if (GUILayout.Button("Set Dictionary"))
					SetDictionary();

				if (GUILayout.Button("Get Dictionary"))
					GetDictionary();
			}
			GUILayout.EndHorizontal();
			
			GUILayout.Box("[NOTE] Similary you can use SetList, GetList, SetDictionary, GetDictonary methods to store and retrieve List and Dictionary objects respectively.");
		}

		private void DrawSynchroniseSection ()
		{
			GUILayout.Label("Synchronise Values", kSubTitleStyle);
			
			if (GUILayout.Button("Synchronise"))
				Synchronise();
		}

		private void DrawRemoveValuesSection ()
		{
			GUILayout.Label("Remove Values", kSubTitleStyle);
			
			GUILayout.BeginHorizontal();
			{
				GUILayout.BeginVertical();
				{
					if (GUILayout.Button("Remove Bool"))
						RemoveKey(kKeyForBoolValue);
					
					if (GUILayout.Button("Remove Long"))
						RemoveKey(kKeyForLongValue);
				}
				GUILayout.EndVertical();
				
				GUILayout.BeginVertical();
				{
					if (GUILayout.Button("Remove Double"))
						RemoveKey(kKeyForDoubleValue);
					
					if (GUILayout.Button("Remove String"))
						RemoveKey(kKeyForStringValue);
				}
				GUILayout.EndVertical();
			}
			GUILayout.EndHorizontal();

            GUILayout.BeginVertical();
            {
                if (GUILayout.Button("Remove All Keys"))
                    RemoveAllKeys();
            }
            GUILayout.EndVertical();
		}

		#endregion

		#region Feature

		private void Initialise ()
		{
			NPBinding.CloudServices.Initialise();
		}

		private void SetBool ()
		{
			m_boolValue	= !m_boolValue;

			NPBinding.CloudServices.SetBool(kKeyForBoolValue, m_boolValue);
		}

		private void GetBool ()
		{
			m_boolValue	= NPBinding.CloudServices.GetBool(kKeyForBoolValue);

			AddNewResult(string.Format("Boolean value stored in cloud: {0}", m_boolValue ? "true" : "false"));
		}
		
		private void SetLong ()
		{
			m_longValue	= Random.Range(0, 100000);
			
			NPBinding.CloudServices.SetLong(kKeyForLongValue, m_longValue);
		}
		
		private void GetLong ()
		{
			m_longValue		= NPBinding.CloudServices.GetLong(kKeyForLongValue);
			
			AddNewResult(string.Format("Long value stored in cloud: {0}", m_longValue));
		}
		
		private void SetDouble ()
		{
			m_doubleValue	= Random.Range(0f, 100000f);
			
			NPBinding.CloudServices.SetDouble(kKeyForDoubleValue, m_doubleValue);
		}
		
		private void GetDouble ()
		{
			m_doubleValue		= NPBinding.CloudServices.GetDouble(kKeyForDoubleValue);
			
			AddNewResult(string.Format("Double value stored in cloud: {0}", m_doubleValue));
		}
		
		private void SetString ()
		{
			m_stringValue	= System.IO.Path.GetRandomFileName();
			
			NPBinding.CloudServices.SetString(kKeyForStringValue, m_stringValue);
		}
		
		private void GetString ()
		{
			m_stringValue		= NPBinding.CloudServices.GetString(kKeyForStringValue);
			
			AddNewResult(string.Format("String value stored in cloud: {0}", m_stringValue));
		}

		private void SetDictionary ()
		{
			m_dictionaryValue.Add ("Key", "Value");
			NPBinding.CloudServices.SetDictionary(kKeyForDictionaryValue, m_dictionaryValue);
		}

		private void GetDictionary ()
		{
			Dictionary<string, object> dict		= NPBinding.CloudServices.GetDictionary(kKeyForDictionaryValue) as Dictionary<string, object>;
			m_dictionaryValue = dict.ToDictionary (kvp => kvp.Key, kvp => (string)kvp.Value);

			AddNewResult(string.Format("Dictionary value stored in cloud: {0}", m_dictionaryValue));
		}


		private void Synchronise ()
		{
			NPBinding.CloudServices.Synchronise();
		}

		private void RemoveKey (string _key)
		{
			NPBinding.CloudServices.RemoveKey(_key);

			AddNewResult(string.Format("Removed data associated with key: {0}.", _key));
		}

        private void RemoveAllKeys()
        {
            NPBinding.CloudServices.RemoveAllKeys();

            AddNewResult("Removed all data");
        }

		#endregion

		#region Callback

		private void OnKeyValueStoreDidInitialise (bool _success)
		{
			if (_success)
			{
				AddNewResult("Successfully Initialised keys and values.");
			}
			else
			{
				AddNewResult("Failed initialising keys and values.");
			}
		}

		private void OnKeyValueStoreDidSynchronise (bool _success)
		{
			if (_success)
			{
				AddNewResult("Successfully synchronised in-memory keys and values.");
			}
			else
			{
				AddNewResult("Failed to synchronise in-memory keys and values.");
			}
		}

		private void OnKeyValueStoreChanged (eCloudDataStoreValueChangeReason _reason, string[] _changedKeys)
		{
			AddNewResult("Cloud key-value store has been changed.");
			AppendResult(string.Format("Reason: {0}.", _reason));

			if (_changedKeys != null)
			{
				AppendResult(string.Format("Total keys changed: {0}.", _changedKeys.Length));
				AppendResult(string.Format("Pick a value from old and new and set the value to cloud for resolving conflict."));
					
				foreach (string _changedKey in _changedKeys)
				{
					if (_changedKey.Equals(kKeyForBoolValue))
					{
						AppendResult(string.Format("New value for key: {0} is {1}. Old value is {2}", _changedKey, NPBinding.CloudServices.GetBool(_changedKey), m_boolValue)); 
					}
					else if (_changedKey.Equals(kKeyForLongValue)) // Shows example of resolving a conflict
					{
						long _newCloudLongValue = NPBinding.CloudServices.GetLong(_changedKey);
						AppendResult(string.Format("New value for key: {0} is {1}. Old value is {2}", _changedKey, _newCloudLongValue, m_longValue)); 
						// Lets assume, we pick the bigger value and set it here.
						long _biggerValue = _newCloudLongValue > m_longValue ? _newCloudLongValue : m_longValue;
						NPBinding.CloudServices.SetLong(_changedKey, _biggerValue);
						AppendResult(string.Format("Picking bigger value {0} and setting to cloud.", _biggerValue));           
					}
					else if (_changedKey.Equals(kKeyForDoubleValue))
					{
						AppendResult(string.Format("New value for key: {0} is {1}. Old value is {2}", _changedKey, NPBinding.CloudServices.GetDouble(_changedKey), m_doubleValue)); 
					}
					else if (_changedKey.Equals(kKeyForStringValue))
					{
						AppendResult(string.Format("New value for key: {0} is {1}. Old value is {2}", _changedKey, NPBinding.CloudServices.GetString(_changedKey).GetPrintableString(), m_stringValue)); 
					}
				}
			}
		}

		#endregion
	}
#endif
}
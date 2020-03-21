#if USES_CLOUD_SERVICES
using UnityEngine;
using System.Collections;
using VoxelBusters.UASUtils;

namespace VoxelBusters.NativePlugins
{
	using Internal;

	public partial class CloudServices : MonoBehaviour 
	{
		#region Delegates

		/// <summary>
		/// Delegate that will be called upon completion of Initialise() method, done at start.
		/// </summary>
		/// <param name="_success">A bool value used to indicate operation status.</param>
		public delegate void InitialiseCompletion (bool _success);

		/// <summary>
		/// Delegate that will be called upon explicitly synchronising in-memory keys and values.
		/// </summary>
		/// <param name="_success">A bool value used to indicate operation status.</param>
		public delegate void SynchroniseCompletion (bool _success);

		/// <summary>
		/// Delegate that will be called when the value of one or more keys in the local key-value store changed due to incoming data pushed from cloud.
		/// </summary>
		/// <param name="_reason">The value indicates why the key-value store changed.</param>
		/// <param name="_changedKeys">An array of strings, each the name of a key whose value changed.</param>
		/// <remarks>
		/// \note This event is not sent when your app sets a value.
		/// </remarks>
		public delegate void KeyValueStoreChangedExternallyResponse  (eCloudDataStoreValueChangeReason _reason, string[] _changedKeys);

		#endregion

		#region Events

		/// <summary>
		/// Event that will be called when initial data from cloud gets downloaded. This can be considered as a result for Initialise call you do at start of this service.
		/// </summary>
		public static event InitialiseCompletion KeyValueStoreDidInitialiseEvent;

		/// <summary>
		/// Event that will be called upon explicitly synchronising in-memory keys and values.
		/// </summary>
		/// <example>
		/// The following code example demonstrates how to register for synchronise event.
		/// <code>
		/// using UnityEngine;
		/// using System.Collections;
		/// using VoxelBusters.NativePlugins;
		/// 
		/// public class ExampleClass : MonoBehaviour 
		/// {
		/// 	private void OnEnable ()
		/// 	{
		/// 		// Registering for event
		///			CloudServices.KeyValueStoreDidSynchroniseEvent	+= OnKeyValueStoreDidSynchronise;
		///     }
		/// 
		/// 	private void OnDisable ()
		/// 	{
		/// 		// Unregistering event
		///			CloudServices.KeyValueStoreDidSynchroniseEvent	-= OnKeyValueStoreDidSynchronise;
		/// 	}
		/// 
		/// 	private void OnKeyValueStoreDidSynchronise (bool _success)
		/// 	{
		/// 		if (_success)
		/// 		{
		/// 			Debug.Log("Successfully synchronised in-memory keys and values.");
		/// 		}
		/// 		else
		/// 		{
		/// 			Debug.Log("Failed to synchronise in-memory keys and values.");
		/// 		}
		/// 	}
		/// }
		/// </code>
		/// </example>
		/// <remarks>
		/// \note This notification is sent only if app makes <c>Synchronise</c> call.
		/// </remarks>
		public static event SynchroniseCompletion KeyValueStoreDidSynchroniseEvent;

		/// <summary>
		/// Event that will be called when the value of one or more keys in the local key-value store changed due to incoming data pushed from cloud.
		/// </summary>
		/// <example>
		/// The following code example demonstrates how to handle key-value store change event.
		/// <code>
		/// using UnityEngine;
		/// using System.Collections;
		/// using VoxelBusters.NativePlugins;
		/// 
		/// public class ExampleClass : MonoBehaviour 
		/// {
		/// 	private void OnEnable ()
		/// 	{
		/// 		// Registering for event
		///			CloudServices.KeyValueStoreDidChangeExternallyEvent	+= OnKeyValueStoreChanged;
		///     }
		/// 
		/// 	private void OnDisable ()
		/// 	{
		/// 		// Unregistering event
		///			CloudServices.KeyValueStoreDidChangeExternallyEvent	-= OnKeyValueStoreChanged;
		/// 	}
		/// 
		/// 	private void OnKeyValueStoreChanged (eCloudDataStoreValueChangeReason _reason, string[] _changedKeys)
		/// 	{
		/// 		Debug.Log(string.Format("Reason for key-value store change: {0}.", _reason));
		/// 
		/// 		if (_changedKeys != null)
		/// 		{
		/// 			Debug.Log("Changed keys are:");
		/// 			
		/// 			foreach (string _changedKey in _changedKeys)
		/// 			{
		/// 				Debug.Log(_changedKey);
		/// 			}
		/// 		}
		/// 	}
		/// }
		/// </code>
		/// </example>
		/// <remarks>
		/// \note This event is not sent when your app sets a value.
		/// </remarks>
		public static event KeyValueStoreChangedExternallyResponse KeyValueStoreDidChangeExternallyEvent;

		#endregion

		#region Native Callback Methods

		protected virtual void CloudKeyValueStoreDidInitialise (string _successStr)
		{
			bool	_success	= bool.Parse(_successStr);
			
			// Invoke handler
			CloudKeyValueStoreDidInitialise(_success);
		}

		protected virtual void CloudKeyValueStoreDidInitialise (bool _isSuccess)
		{
			DebugUtility.Logger.Log(Constants.kDebugTag, "[CloudServices] Received key store value Initialised event.");
			
			if (KeyValueStoreDidInitialiseEvent != null)
				KeyValueStoreDidInitialiseEvent(_isSuccess);
		}

		protected virtual void CloudKeyValueStoreDidSynchronise (string _successStr)
		{
			bool	_success	= bool.Parse(_successStr);
			
			// Invoke handler
			CloudKeyValueStoreDidSynchronise(_success);
		}

		protected void CloudKeyValueStoreDidSynchronise (bool _success)
		{
			DebugUtility.Logger.Log(Constants.kDebugTag, "[CloudServices] Received key store value synchronised event.");
			
			if (KeyValueStoreDidSynchroniseEvent != null)
				KeyValueStoreDidSynchroniseEvent(_success);
		}
		
		protected virtual void CloudKeyValueStoreDidChangeExternally (string _dataStr)
		{}

		protected void CloudKeyValueStoreDidChangeExternally (eCloudDataStoreValueChangeReason _reason, string[] _keys)
		{
			DebugUtility.Logger.Log(Constants.kDebugTag, "[CloudServices] Received key store value changed event.");
			
			if (KeyValueStoreDidChangeExternallyEvent != null)
				KeyValueStoreDidChangeExternallyEvent(_reason, _keys);
		}

		#endregion
	}
}
#endif
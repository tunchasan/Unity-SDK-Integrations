#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEditor;

namespace VoxelBusters.Utility
{
	[InitializeOnLoad]
	public class EditorUtils 
	{
		#region Static Fields

		private static Dictionary<Action, InvokeItemMetadata>	invokeCollection;
		private	static List<IEnumerator> 		routineList;
		private	static List<Action>				invalidActions;
		private static DateTime					timeWhenLastEditorUpdateWasCalled;

		#endregion

		#region Static Constructors

		static EditorUtils ()
		{
			// Set properties
			invokeCollection					= new Dictionary<Action, InvokeItemMetadata>();
			routineList							= new List<IEnumerator>();
			invalidActions						= new List<Action>();
			timeWhenLastEditorUpdateWasCalled	= DateTime.Now;

			// Register for updates
			EditorApplication.update	-= EditorUpdate;
			EditorApplication.update	+= EditorUpdate;
		}
	
		#endregion

		#region Static Methods

		/// <summary>
		/// Starts editor coroutine.
		/// </summary>
		public static void StartCoroutine (IEnumerator _enumerator)
		{
			routineList.Add(_enumerator);
		}

		/// <summary>
		/// Stops editor coroutine.
		/// </summary>
		public static void StopCoroutine (IEnumerator _enumerator)
		{	
			while (true)
			{
				if (!routineList.Remove(_enumerator))
					break;
			}
		}

		public static void Invoke (Action _method, float _time)
		{
			invokeCollection[_method]	= new InvokeItemMetadata() 
			{
				timeSinceLastInvoke		= 0f,
				invokeAfter				= _time,
				repeatRate				= 0f
			};
		}

		public static void InvokeRepeating (Action _method, float _time, float _repeatRate)
		{
			invokeCollection[_method]	= new InvokeItemMetadata() 
			{
				timeSinceLastInvoke		= 0f,
				invokeAfter				= _time,
				repeatRate				= _repeatRate
			};
		}

		private static void EditorUpdate ()
		{
			ManageActiveInvokes();
			ManageActiveCoroutines();
		}

		private static void ManageActiveInvokes ()
		{
			float	_dt		= (DateTime.Now - timeWhenLastEditorUpdateWasCalled).Milliseconds / 1000f;

			List<Action> _actionList = new List<Action>(invokeCollection.Keys);

			foreach (Action _action in _actionList)
			{
				InvokeItemMetadata	_metadata	= invokeCollection[_action];

				_metadata.timeSinceLastInvoke	+= _dt;

				if (_metadata.timeSinceLastInvoke > _metadata.invokeAfter)
				{
					_action();

					// Check whether invoke action can be invalidated
					if (Mathf.Approximately(0f, _metadata.repeatRate))
					{
						invalidActions.Add(_action);
						continue;
					}

					// Update item information
					_metadata.timeSinceLastInvoke	= 0f;
					_metadata.invokeAfter			= _metadata.repeatRate;
				}
			}

			// Reset properties
			foreach (Action _action in invalidActions)
				invokeCollection.Remove(_action);
			
			invalidActions.Clear();
			timeWhenLastEditorUpdateWasCalled	= DateTime.Now;
		}

		private static void ManageActiveCoroutines ()
		{
			int _count	= routineList.Count;
			for (int _iter = 0; _iter < _count; _iter++)
			{
				IEnumerator _routine	= routineList[_iter];
				if (!_routine.MoveNext())
				{
					routineList.Remove(_routine);
					_iter--;
					_count--;
				}
			}
		}
		
		#endregion

		#region Menu Methods

		[MenuItem("Extensions/Clear PlayerPrefs")]
		public static void ClearPlayerPrefs ()
		{
			PlayerPrefs.DeleteAll();
		}

		#endregion

		#region Nested Types

		private class InvokeItemMetadata 
		{
			#region Fields

			public 	float 	timeSinceLastInvoke;
			public 	float 	invokeAfter;
			public	float	repeatRate;

			#endregion
		}

		#endregion
	}
}
#endif
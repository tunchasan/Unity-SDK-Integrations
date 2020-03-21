using UnityEngine;
using System.Collections;

namespace VoxelBusters.NativePlugins.Internal
{
	public class NPObject
	{
		#region Fields

		private 	string		m_instaceID; 	

		#endregion

		#region Constructors

		private NPObject ()
		{}

		public NPObject (NPObjectManager.eCollectionType _collectionType)
		{
			// Intialize properties
			m_instaceID	= InstanceIDGenerator.Create();

			// Register object
			NPObjectManager.AddNewObjectToCollection(this, _collectionType);
		}

		#endregion

		#region Methods

		public string GetInstanceID ()
		{
			return m_instaceID;
		}

		#endregion
	}
}
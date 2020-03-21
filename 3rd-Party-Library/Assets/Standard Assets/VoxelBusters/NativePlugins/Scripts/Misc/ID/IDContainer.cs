using UnityEngine;
using System.Collections;

namespace VoxelBusters.NativePlugins
{
	[System.Serializable]
	public class IDContainer
	{
		#region Fields
		
		[SerializeField]
		private 	string				m_globalID;
		[SerializeField]
		private		PlatformValue[]		m_platformIDs;
		
		#endregion

		#region Properties

		public string GlobalID
		{
			get
			{
				return m_globalID;
			}
		}
		
		public PlatformValue[] PlatformIDs
		{
			get
			{
				return m_platformIDs;
			}
		}

		#endregion

		#region Constructors

		private IDContainer ()
		{}

		public IDContainer (string _globalID, params PlatformValue[] _platformIDs)
		{
			// Initialize properties
			m_globalID		= _globalID;
			m_platformIDs	= _platformIDs;
		}

		#endregion

		#region Methods

		public bool EqualsGlobalID (string _identifier)
		{
			return string.Equals(m_globalID, _identifier);
		}

		public bool EqualsCurrentPlatformID (string _identifier)
		{
			PlatformValue _object	= PlatformValueHelper.GetCurrentPlatformValue(_array: m_platformIDs);
			if (_object == null)
				return false;

			return string.Equals(_object.Value, _identifier);
		}

		#endregion
	}
}
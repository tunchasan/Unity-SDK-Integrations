using UnityEngine;
using System.Collections;

namespace VoxelBusters.NativePlugins
{
	[System.Serializable]
	public class LeaderboardMetadata : IIdentifierContainer
	{
		#region Fields

		[SerializeField]
		[Tooltip("String used to uniquely identify achievement across all the platforms.")]
		private		string			m_globalID;
		[SerializeField]
		[Tooltip("Collection of identifiers, where each identifier is used to identify achievement in a specific platform game server.")]
		private		PlatformValue[]	m_platformIDs;
		
		#endregion

		#region Properties

		public string GlobalID
		{
			get
			{
				return m_globalID;
			}
			set
			{
				m_globalID	= value;
			}
		}
		
		public PlatformValue[] PlatformIDs
		{
			get
			{
				return m_platformIDs;
			}
			set
			{
				m_platformIDs	= value;
			}
		}

		#endregion
		
		#region Constructors
		
		public LeaderboardMetadata ()
		{}
		
		#endregion
		
		#region Static Methods
		
		internal static LeaderboardMetadata Create (IDContainer _container)
		{
			LeaderboardMetadata _newObject	= new LeaderboardMetadata();
			_newObject.m_globalID			= _container.GlobalID;
			_newObject.m_platformIDs		= _container.PlatformIDs;

			return _newObject;
		}
		
		#endregion
	}
}
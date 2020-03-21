using UnityEngine;
using System.Collections;

namespace VoxelBusters.NativePlugins
{
	[System.Serializable]
	public class AchievementMetadata : IIdentifierContainer
	{
		#region Fields

		[SerializeField]
		[Tooltip("String used to uniquely identify achievement across all the platforms.")]
		private		string			m_globalID;
		[SerializeField]
		[Tooltip("Collection of identifiers, where each identifier is used to identify achievement in a specific platform game server.")]
		private		PlatformValue[]	m_platformIDs;
		[SerializeField]
		[Tooltip("The number of steps required to complete an achievement. Must be greater than 0.")]
		private		int				m_noOfSteps		= 1;

		#endregion
		
		#region Properties

		public int NoOfSteps
		{
			get
			{
				return m_noOfSteps;
			}
			
			set
			{
				m_noOfSteps	= value;
			}
		}
		
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

		public AchievementMetadata ()
		{
			// Default steps for instant achievement (non-progressive/non-incremental achievement)
			m_noOfSteps			= 1;
		}

		#endregion

		#region Static Methods

		internal static AchievementMetadata Create (IDContainer _container)
		{
			AchievementMetadata _newObject	= new AchievementMetadata();

			_newObject.m_globalID			= _container.GlobalID;
			_newObject.m_platformIDs		= _container.PlatformIDs;

			return _newObject;
		}

		#endregion
	}
}
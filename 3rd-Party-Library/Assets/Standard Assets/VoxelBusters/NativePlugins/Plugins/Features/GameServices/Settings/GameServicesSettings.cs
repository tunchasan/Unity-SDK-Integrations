using UnityEngine;
using System.Collections;
using VoxelBusters.Utility;

namespace VoxelBusters.NativePlugins
{
	using Internal;

	[System.Serializable]
	public partial class GameServicesSettings
	{
		#region Fields
		
		[SerializeField] 
		[Tooltip("Store additional information about all the leaderboards that are used.")]
		private		LeaderboardMetadata[]	m_leaderboardMetadataCollection;	
		[SerializeField]
		[Tooltip("Store additional information about all the achievements that are used.")]
		private		AchievementMetadata[]	m_achievementMetadataCollection;	
		[SerializeField]
		private 	iOSSettings				m_iOS		= new iOSSettings();
		[SerializeField]
		//[InspectorButton(eInspectorButtonPosition.BOTTOM, "Refresh Simulator;Updates simulator data with current config values.;RefreshEditorGameCenter", "Reset Achievements;Resets all achievement progress.;ResetEditorGameCenterAchievements")]
		private 	AndroidSettings			m_android	= new AndroidSettings();

		#endregion

		#region Properties
		
		internal LeaderboardMetadata[] LeaderboardMetadataCollection
		{
			get
			{
				return m_leaderboardMetadataCollection;
			}
			
			set
			{
				m_leaderboardMetadataCollection	= value;
			}
		}

		internal AchievementMetadata[] AchievementMetadataCollection
		{
			get
			{
				return m_achievementMetadataCollection;
			}

			set
			{
				m_achievementMetadataCollection	= value;
			}
		}

		internal iOSSettings IOS
		{
			get 
			{
				return m_iOS; 
			}
		}
		
		internal AndroidSettings Android
		{
			get 
			{
				return m_android; 
			}
		}

		#endregion
		
		#region Deprecated Fields
		
		[SerializeField]
		[HideInInspector]
		private		IDContainer[]		m_achievementIDCollection	= new IDContainer[0];	
		[SerializeField]
		[HideInInspector]
		private		IDContainer[]		m_leaderboardIDCollection	= new IDContainer[0];			
		
		#endregion

		#region Deprecated Properties

		internal IDContainer[] AchievementIDCollection
		{
			get
			{
				return m_achievementIDCollection;
			}
		}
		
		internal IDContainer[] LeaderboardIDCollection
		{
			get
			{
				return m_leaderboardIDCollection;
			}
		}

		#endregion
	}
}
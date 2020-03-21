using UnityEngine;
using System.Collections;

#if USES_GAME_SERVICES && UNITY_EDITOR
using System;
using System.Collections.Generic;
using VoxelBusters.Utility;

namespace VoxelBusters.NativePlugins.Internal
{
	[Serializable]
	public sealed class EGCScore
	{
		#region Fields
		
		[SerializeField]
		private				string					m_leaderboardID;
		[SerializeField]
		private				int						m_value;
		[SerializeField]
		private				string					m_userID;
		[NonSerialized]
		private				EGCUser					m_user;
		[SerializeField]
		private				string					m_date;
		[SerializeField]
		private				int						m_rank;
		
		#endregion
		
		#region Properties
		
		public string LeaderboardID
		{
			get
			{
				return m_leaderboardID;
			}
			
			private set
			{
				m_leaderboardID		= value;
			}
		}
		
		public EGCUser User
		{
			get
			{
				if (m_user == null) 
					m_user			= EditorGameCenter.Instance.GetUserWithID(m_userID);
				
				return m_user;
			}
		}
		
		public long Value
		{
			get
			{
				return (long)m_value;
			}
			
			set
			{
				m_value		= (int)value;
			}
		}
		
		public DateTime Date
		{
			get
			{
				if (string.IsNullOrEmpty(m_date))
					return new DateTime();
				
				return DateTime.Parse(m_date);
			}
			
			private set
			{
				m_date		= value.ToString();
			}
		}
		
		public int Rank
		{
			get
			{
				return m_rank;
			}
			
			set
			{
				m_rank		= value;	
			}
		}
		
		#endregion
		
		#region Constructors
		
		public EGCScore (string _leaderboardID, string _userID, long _scoreValue = 0L) 
		{
			// Initialize properties
			LeaderboardID		= _leaderboardID;
			m_userID			= _userID;
			Value				= _scoreValue;
			Date				= DateTime.Now;
		}
		
		#endregion
	}
}
#endif
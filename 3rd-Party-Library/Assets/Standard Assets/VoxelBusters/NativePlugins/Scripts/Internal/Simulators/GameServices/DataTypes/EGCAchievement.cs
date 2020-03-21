using UnityEngine;
using System.Collections;

#if USES_GAME_SERVICES && UNITY_EDITOR
using System;
using System.Collections.Generic;

namespace VoxelBusters.NativePlugins.Internal
{
	[Serializable]
	public sealed class EGCAchievement
	{
		#region Fields
		
		[SerializeField]
		private		string		m_identifier;		
		[SerializeField]
		private		float		m_percentageCompleted;
		[SerializeField]
		private		bool		m_completed;
		[SerializeField]
		private		string		m_lastReportedDate;
		
		#endregion
		
		#region Properties
		
		public string Identifier
		{
			get
			{
				return m_identifier;
			}
			
			private set
			{
				m_identifier	= value;
			}
		}

		public float PercentageCompleted
		{
			get
			{
				return m_percentageCompleted;
			}

			private set
			{
				m_percentageCompleted	= value;
			}
		}
		
		public bool Completed
		{
			get
			{
				return m_completed;
			}
			
			private set
			{
				m_completed		= value;
			}
		}
		
		public DateTime LastReportedDate
		{
			get
			{
				if (string.IsNullOrEmpty(m_lastReportedDate))
					return new DateTime();
				
				return DateTime.Parse(m_lastReportedDate);
			}
			
			private set
			{
				m_lastReportedDate	= value.ToString();
			}
		}
		
		#endregion
		
		#region Constructor
		
		public EGCAchievement (string _identifier, double _percentageCompleted)
		{
			// Initialize
			Identifier			= _identifier;

			// Set progress
			UpdateProgress(_percentageCompleted);
		}
		
		#endregion

		#region Methods

		public void UpdateProgress (double _percentageCompleted)
		{
			LastReportedDate	= DateTime.Now;
			PercentageCompleted	= (float)System.Math.Min(100d, _percentageCompleted);
			Completed			= (100d - _percentageCompleted) < System.Double.Epsilon;
		}

		#endregion
	}
}
#endif
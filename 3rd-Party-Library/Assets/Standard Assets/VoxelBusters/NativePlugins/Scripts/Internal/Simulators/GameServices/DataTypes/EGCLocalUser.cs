using UnityEngine;
using System.Collections;

#if USES_GAME_SERVICES && UNITY_EDITOR
using System;
using System.Collections.Generic;
using VoxelBusters.Utility;

namespace VoxelBusters.NativePlugins.Internal
{
	public sealed class EGCLocalUser
	{
		#region Fields
		
		private		EGCUser		m_userInfo;
		private		bool		m_isAuthenticated;
		
		#endregion
		
		#region Properties
		
		public EGCUser Info
		{
			get
			{
				return m_userInfo;
			}
			
			private set
			{
				m_userInfo	= value;
			}
		}
		
		public bool IsAuthenticated
		{
			get
			{
				return m_isAuthenticated;
			}
			
			set
			{
				m_isAuthenticated	= value;
			}
		}
		
		#endregion
		
		#region Constructor
		
		public EGCLocalUser (EGCUser _userInfo, bool _isAuthenticated)
		{
			// Initialize properties
			Info				= _userInfo;
			IsAuthenticated		= _isAuthenticated;
		}
		
		#endregion
	}
}
#endif
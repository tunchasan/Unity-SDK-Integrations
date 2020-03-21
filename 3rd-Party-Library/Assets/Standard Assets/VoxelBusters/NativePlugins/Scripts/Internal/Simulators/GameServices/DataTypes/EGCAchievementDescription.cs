using UnityEngine;
using System.Collections;

#if USES_GAME_SERVICES && UNITY_EDITOR
using System;

namespace VoxelBusters.NativePlugins.Internal
{
	[Serializable]
	public sealed class EGCAchievementDescription
	{
		#region Fields
		
		[SerializeField]
		private				string				m_identifier;
		[SerializeField]
		private				string				m_title;
		[SerializeField]
		private				string				m_achievedDescription;
		[SerializeField]
		private				string				m_unachievedDescription;
		[SerializeField]
		private				Texture2D			m_image;
		[SerializeField]
		private				int					m_maximumPoints;
		[SerializeField]
		private				bool				m_isHidden;
		
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
		
		public string Title
		{
			get
			{
				return m_title;
			}
			
			private set
			{
				m_title	= value;
			}
		}
		
		public string AchievedDescription
		{
			get
			{
				return m_achievedDescription;
			}
			
			private set
			{
				m_achievedDescription	= value;
			}
		}
		
		public string UnachievedDescription
		{
			
			get
			{
				return m_unachievedDescription;
			}
			
			private set
			{
				m_unachievedDescription	= value;
			}
		}
		
		public int MaximumPoints
		{
			get
			{
				return m_maximumPoints;
			}
			
			private set
			{
				m_maximumPoints	= value;
			}
		}
		
		public Texture2D Image
		{
			get
			{
				return m_image;
			}
			
			private set
			{
				m_image	= value;
			}
		}
		
		public bool IsHidden
		{
			get
			{
				return m_isHidden;
			}
			
			set
			{
				m_isHidden	= value;
			}
		}
		
		#endregion
		
		#region Constructors
		
		public EGCAchievementDescription (string _identifier, string _title, string _achievedDescription, string _unachievedDescription, int _maxPoints, Texture2D _image, bool _isHidden)
		{
			// Initialize properties
			Identifier				= _identifier;
			Title					= _title;
			AchievedDescription 	= _achievedDescription;
			UnachievedDescription 	= _unachievedDescription;
			MaximumPoints			= _maxPoints;
			Image					= _image;
			IsHidden				= _isHidden;
		}
		
		#endregion
	}
}
#endif
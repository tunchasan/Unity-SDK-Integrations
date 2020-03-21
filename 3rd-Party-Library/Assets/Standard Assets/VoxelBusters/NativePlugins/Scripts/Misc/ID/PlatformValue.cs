using UnityEngine;
using System.Collections;

namespace VoxelBusters.NativePlugins
{
	[System.Serializable]
	public class PlatformValue
	{
		#region Fields

		[SerializeField]
		private		eRuntimePlatform	m_platform;
		[SerializeField]
		private		string				m_value;

		#endregion

		#region Properties

		public eRuntimePlatform Platform
		{
			get
			{
				return m_platform;
			}
			private set
			{
				m_platform	= value;
			}
		}

		public string Value
		{
			get
			{
				return m_value;
			}
			private set
			{
				m_value		= value;
			}
		}

		#endregion

		#region Constructors

		private PlatformValue ()
		{}

		#endregion

		#region Static Methods

		public static PlatformValue IOS (string _identifier)
		{
			return new PlatformValue()
			{
				Platform	= eRuntimePlatform.IOS,
				Value		= _identifier,
			};
		}

		public static PlatformValue Android (string _identifier)
		{
			return new PlatformValue()
			{
				Platform	= eRuntimePlatform.ANDROID,
				Value		= _identifier,
			};		
		}

		public static PlatformValue Amazon (string _identifier)
		{
			return new PlatformValue()
			{
				Platform	= eRuntimePlatform.AMAZON,
				Value		= _identifier,
			};		
		}

		#endregion
	}
}
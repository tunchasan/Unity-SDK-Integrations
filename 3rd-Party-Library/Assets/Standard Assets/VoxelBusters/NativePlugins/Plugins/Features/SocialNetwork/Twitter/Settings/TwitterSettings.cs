using UnityEngine;
using System.Collections;
using VoxelBusters.Utility;

namespace VoxelBusters.NativePlugins
{
	[System.Serializable]
	public class TwitterSettings
	{
		#region Fields

		[SerializeField]
		[Tooltip("The Twitter application consumer key.")]
		private 	string 		m_consumerKey;
		[SerializeField]
		[Tooltip("The Twitter application consumer secret.")]
		private 	string 		m_consumerSecret;

		#endregion

		#region Properties

		/// <summary>
		/// The Twitter application consumer key (API Key). (read-only)
		/// </summary>
		internal string ConsumerKey
		{
			get 
			{ 
				return m_consumerKey; 
			}

			private set 
			{ 
				m_consumerKey	= value; 
			}
		}

		/// <summary>
		/// The Twitter application consumer secret (API Secret). (read-only)
		/// </summary>
		public string ConsumerSecret
		{
			get 
			{ 
				return m_consumerSecret; 
			}

			private set 
			{ 
				m_consumerSecret	= value; 
			}
		}

		#endregion
	}
}

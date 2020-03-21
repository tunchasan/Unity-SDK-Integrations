using UnityEngine;
using System.Collections;


namespace VoxelBusters.NativePlugins
{
	/// <summary>
	/// Represents an object that holds information external authentication.
	/// </summary>
	/// <remarks>
	/// \note Your game must authenticate the local user before using any features.
	/// </remarks>
	public partial class ExternalAuthenticationCredentials
	{
		#region Properties
		
		private 	iOS			m_iOS		= new iOS();
		public iOS iOSCredentials
		{
			get
			{
				return m_iOS;
			}
		}

		private 	Android		m_android	= new Android();
		public Android AndroidCredentials
		{
			get
			{
				return m_android;
			}
		}
		
		#endregion


		public ExternalAuthenticationCredentials (IDictionary _payloadDict)
		{
#if UNITY_IOS
			m_iOS.Load(_payloadDict);
#elif UNITY_ANDROID
			m_android.Load(_payloadDict);
#endif			
		}
		
	}
}
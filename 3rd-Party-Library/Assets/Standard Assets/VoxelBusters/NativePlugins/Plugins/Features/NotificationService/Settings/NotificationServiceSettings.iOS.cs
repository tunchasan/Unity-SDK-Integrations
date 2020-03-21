using UnityEngine;
using System.Collections;

namespace VoxelBusters.NativePlugins
{
	using Internal;

	public partial class NotificationServiceSettings
	{
		[System.Serializable]
		public class iOSSettings 
		{
			#region Fields
			
			[SerializeField]
			[Tooltip("The key used to capture user info dictionary from the payload.")]
			private 	string 		m_userInfoKey	= "user_info";
			
			#endregion
			
			#region Properties
			
			internal string UserInfoKey
			{
				get 
				{ 
					return m_userInfoKey; 
				}
			}
			
			#endregion
		}
	}
}
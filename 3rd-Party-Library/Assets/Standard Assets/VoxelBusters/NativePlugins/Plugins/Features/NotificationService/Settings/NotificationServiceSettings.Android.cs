using UnityEngine;
using System.Collections;

namespace VoxelBusters.NativePlugins
{
	using Internal;

	public partial class NotificationServiceSettings
	{
		[System.Serializable]
		public class AndroidSettings 
		{
			#region Fields
			
			private 	bool	 		m_needsBigStyle		= false; 
			[SerializeField, NotifyNPSettingsOnValueChange]
			[Tooltip ("If enabled, device vibrates on receiving a notification.")]
			private 	bool	 		m_allowVibration	= true; 
			
			[SerializeField, NotifyNPSettingsOnValueChange]
			[Tooltip ("The texture used as small icon in post Android L Devices.")]
            private 	Texture2D 		m_whiteSmallIcon    = null;
			[SerializeField, NotifyNPSettingsOnValueChange]
			[Tooltip ("The texture used as small icon in pre Android L Devices.")]
            private 	Texture2D 		m_colouredSmallIcon = null;
			
			[Header("Payload Keys:")]
			[SerializeField]
			[Tooltip("The key used to capture ticker text property from the payload.")]
			private 	string 			m_tickerTextKey 	= "ticker_text";
			[SerializeField]
			[Tooltip("The key used to capture content text property from the payload.")]
			private 	string 			m_contentTextKey 	= "content_text";
			[SerializeField]
			[Tooltip("The key used to capture content title property from the payload.")]
			private 	string 			m_contentTitleKey 	= "content_title";
			[SerializeField]
			[Tooltip("The key used to capture user info dictionary from the payload.")]
			private 	string 			m_userInfoKey 		= "user_info";
			[SerializeField]
			[Tooltip("The key used to capture tag property from the payload.")]
			private 	string 			m_tagKey 			= "tag";
			
			#endregion
			
			#region Properties
			
			internal bool NeedsBigStyle 
			{
				get
				{
					return m_needsBigStyle;
				}
			}
			
			internal bool AllowVibration
			{
				get
				{
					return m_allowVibration;
				}
			}
			
			internal Texture2D WhiteSmallIcon
			{
				get 
				{ 
					return m_whiteSmallIcon; 
				}
			}
			
			internal Texture2D ColouredSmallIcon
			{
				get 
				{ 
					return m_colouredSmallIcon; 
				}
			}
			
			internal string TickerTextKey
			{
				get 
				{ 
					return m_tickerTextKey; 
				}
			}
			
			internal string ContentTitleKey
			{
				get 
				{ 
					return m_contentTitleKey; 
				}
			}
			
			internal string ContentTextKey
			{
				get 
				{ 
					return m_contentTextKey; 
				}
			}
			
			internal string UserInfoKey
			{
				get 
				{ 
					return m_userInfoKey; 
				}
			}
			
			internal string TagKey
			{
				get 
				{ 
					return m_tagKey; 
				}
			}
			
			#endregion
		}
	}
}
using UnityEngine;
using System.Collections;

namespace VoxelBusters.NativePlugins
{
	using Internal;

	public partial class GameServicesSettings
	{
		[System.Serializable]
		public class AndroidSettings
		{
			#region Fields

			[SerializeField]
			[NotifyNPSettingsOnValueChange]
			[Tooltip ("Your application id in Google Play services.")]
			private 	string		m_playServicesApplicationID = null;

			[SerializeField]
			[NotifyNPSettingsOnValueChange]
			[Tooltip ("Your Server Client ID for getting external authentication credentials")]
			private 	string 		m_serverClientID = null;

			[Tooltip ("String formats used to derive completed achievement description. Note: Achievement title will be inserted in place of token \'#\'.")]
			private 	string[]	m_achievedDescriptionFormats = new string[] {
				"Awesome! Achievement # completed."
			};

			[SerializeField]
			[Tooltip ("Allow to show default error dialogs.")]
			private 	bool		m_showDefaultErrorDialogs = true;

			[SerializeField]
			[Tooltip ("Allow auto sign in if user logged in previously.")]
			private 	bool		m_allowAutoLogin = true;

			#endregion

			#region Properties

			internal string PlayServicesApplicationID
			{
				get
				{
					return m_playServicesApplicationID;
				}
			}

			internal string ServerClientID
			{
				get
				{
					return m_serverClientID;
				}
			}

			internal string[] AchievedDescriptionFormats
			{
				get
				{
					return m_achievedDescriptionFormats;
				}
			}

			internal bool ShowDefaultErrorDialogs
			{
				get
				{
					return m_showDefaultErrorDialogs;
				}
			}

			internal bool AllowAutoLogin
			{
				get
				{
					return m_allowAutoLogin;
				}
			}

			#endregion
		}
	}
}

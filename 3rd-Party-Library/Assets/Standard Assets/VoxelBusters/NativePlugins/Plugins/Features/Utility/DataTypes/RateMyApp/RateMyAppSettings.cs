using UnityEngine;
using System.Collections;
using VoxelBusters.Utility;
using VoxelBusters.NativePlugins.Internal;

namespace VoxelBusters.NativePlugins
{
	[System.Serializable]
	public class RateMyAppSettings 
	{
		#region Fields

		[SerializeField]
		[NotifyNPSettingsOnValueChange]
		[Tooltip("If enabled, Rate My App feature will be active within your application.")]
		private 	bool 		m_isEnabled						= false;
		[SerializeField]
		[Tooltip("The text that appears in the prompt's title bar.")]
		private 	string 		m_title							= "Rate My App";
		[SerializeField]
		[Tooltip("Descriptive message for the user.")]
		private 	string 		m_message						= "If you enjoy using Native Plugin would you mind taking a moment to rate it? " +
			"It wont take more than a minute. Thanks for your support";
		[SerializeField]
		[Tooltip("The number of hours since first launch, after which user is prompted to rate the app.")]
		private 	int			m_showFirstPromptAfterHours		= 2;
		[SerializeField]
		[Tooltip("The number of hours since last time we showed the prompt, after which user is prompted to rate the app.")]
		private 	int			m_successivePromptAfterHours	= 6;
		[SerializeField]
		[Tooltip("The number of times the user must launch the app, after which user is prompted to rate the app.")]
		private 	int			m_successivePromptAfterLaunches	= 5;
		[SerializeField]
		[Tooltip("The button label for the button, that will send user to app review page.")]
		private 	string		m_rateItButtonText				= "Rate It Now";
		[SerializeField]
		[Tooltip("The button label for the button, that will remind user to review later.")]
		private 	string		m_remindMeLaterButtonText		= "Remind Me Later";
		[SerializeField]
		[Tooltip("The button label for the button, that rejects reviewing the app. Keep this field empty, if you don't wish to show this option.")]
		private 	string		m_dontAskButtonText				= "No, Thanks";

		#endregion

		#region Properties

		public bool IsEnabled
		{
			get
			{
				return m_isEnabled;
			}
			set
			{
				m_isEnabled	= value;
			}
		}

		public string Title
		{
			get
			{
				return m_title;
			}
			set
			{
				m_title	= value;
			}
		}

		public string Message
		{
			get
			{
				return m_message;
			}
			set
			{
				m_message	= value;
			}
		}

		public int ShowFirstPromptAfterHours
		{
			get
			{
				return m_showFirstPromptAfterHours;
			}
			set
			{
				m_showFirstPromptAfterHours	= value;
			}
		}

		public int SuccessivePromptAfterHours
		{
			get
			{
				return m_successivePromptAfterHours;
			}
			set
			{
				m_successivePromptAfterHours	= value;
			}
		}

		public int SuccessivePromptAfterLaunches
		{
			get
			{
				return m_successivePromptAfterLaunches;
			}
			set
			{
				m_successivePromptAfterLaunches	= value;
			}
		}

		public string RemindMeLaterButtonText
		{
			get
			{
				return m_remindMeLaterButtonText;
			}
			set
			{
				m_remindMeLaterButtonText	= value;
			}
		}

		public string RateItButtonText
		{
			get
			{
				return m_rateItButtonText;
			}
			set
			{
				m_rateItButtonText	= value;
			}
		}

		public string DontAskButtonText
		{
			get
			{
				return m_dontAskButtonText;
			}
			set
			{
				m_dontAskButtonText	= value;
			}
		}

		#endregion
	}
}
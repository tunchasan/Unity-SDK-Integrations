using UnityEngine;
using System.Collections;
using VoxelBusters.Utility.UnityGUI.MENU;

namespace VoxelBusters.NativePlugins.Demo
{
	public class UIDemo : NPDemoBase 
	{
		#region Properties

		[SerializeField]
		private 	string 		m_title					= "Alert Title";
		[SerializeField]
		private 	string 		m_message				= "Alert message";

		[SerializeField]
		private 	string 		m_usernamePlaceHolder	= "username";
		[SerializeField]
		private 	string 		m_passwordPlaceHolder	= "password";

		[SerializeField]
		private 	string 		m_button				= "Ok";
		[SerializeField]
		private 	string[] 	m_buttons				= new string[] { 
			"Cancel", 
			"Ok" 
		};

		#endregion

		#region GUI Methods
		
		protected override void DisplayFeatureFunctionalities ()
		{
			base.DisplayFeatureFunctionalities ();

			GUILayout.Label ("Alert Dialogs", kSubTitleStyle);
			
			if (GUILayout.Button ("Show Alert Dialog With Single Button"))
			{
				ShowAlertDialogWithSingleButton ();
			}
			
			if (GUILayout.Button ("Show Alert Dialog With Multiple Buttons"))
			{
				ShowAlertDialogWithMultipleButtons ();
			}

			GUILayout.Label ("Prompt Dialogs", kSubTitleStyle);
			
			if (GUILayout.Button ("Show Plain Text Prompt Dialog"))
			{
				ShowPlainTextPromptDialog ();
			}
			
			if (GUILayout.Button ("Show Secured Text Prompt Dialog"))
			{
				ShowSecuredTextPromptDialog ();
			}
			
			if (GUILayout.Button ("Show Login Prompt Dialog"))
			{
				ShowLoginPromptDialog ();
			}

			GUILayout.Label ("Toast", kSubTitleStyle);

			if (GUILayout.Button ("Show Short Duration Toast"))
			{
				ShowShortDurationToast ();
			}

			if (GUILayout.Button ("Show Long Duration Toast"))
			{
				ShowLongDurationToast ();
			}
		}
		
		#endregion

		#region API Methods

		private void ShowAlertDialogWithSingleButton ()
		{
			NPBinding.UI.ShowAlertDialogWithSingleButton (m_title, m_message, m_button, (string _buttonPressed)=>{
				AddNewResult("Alert dialog closed");
				AppendResult("ButtonPressed=" + _buttonPressed);
			});
		}

		private void ShowAlertDialogWithMultipleButtons ()
		{
			NPBinding.UI.ShowAlertDialogWithMultipleButtons(m_title, m_message, m_buttons, MultipleButtonsAlertClosed); 
		}

		private void ShowPlainTextPromptDialog ()
		{
			NPBinding.UI.ShowSingleFieldPromptDialogWithPlainText (m_title, m_message, m_usernamePlaceHolder, m_buttons, SingleFieldPromptDialogClosed);
		}

		private void ShowSecuredTextPromptDialog ()
		{
			NPBinding.UI.ShowSingleFieldPromptDialogWithSecuredText (m_title, m_message, m_passwordPlaceHolder, m_buttons, SingleFieldPromptDialogClosed);
		}

		private void ShowLoginPromptDialog ()
		{
			NPBinding.UI.ShowLoginPromptDialog (m_title, m_message, m_usernamePlaceHolder, m_passwordPlaceHolder, m_buttons, LoginPromptDialogClosed);
		}

		private void ShowShortDurationToast ()
		{
			NPBinding.UI.ShowToast (m_message, eToastMessageLength.SHORT);
		}

		private void ShowLongDurationToast ()
		{
			NPBinding.UI.ShowToast (m_message, eToastMessageLength.LONG);
		}

		#endregion

		#region API Callback Methods

		private void MultipleButtonsAlertClosed (string _buttonPressed)
		{
			AddNewResult ("Alert dialog closed.");
			AppendResult ("Clicked button name is " + _buttonPressed + ".");
		}

		private void SingleFieldPromptDialogClosed (string _buttonPressed, string _input)
		{
			AddNewResult ("Single field prompt dialog closed.");
			AppendResult ("Clicked button name is " + _buttonPressed + ".");
			AppendResult ("Input text is " + _input + ".");
		}

		private void LoginPromptDialogClosed (string _buttonPressed, string _username, string _password)
		{
			AddNewResult ("Login prompt dialog closed.");
			AppendResult ("Clicked button name is " + _buttonPressed + ".");
			AppendResult ("Entered user name is " + _username + ".");
			AppendResult ("Entered password is " + _password + ".");
		}

		#endregion 
	}
}
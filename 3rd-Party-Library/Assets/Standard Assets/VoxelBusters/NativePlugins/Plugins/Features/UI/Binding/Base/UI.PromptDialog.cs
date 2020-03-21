using UnityEngine;
using System.Collections;
using VoxelBusters.Utility;

namespace VoxelBusters.NativePlugins
{
	public partial class UI : MonoBehaviour 
	{
		#region Prompt Dialog API's

		/// <summary>
		/// Shows a prompt dialog that allows the user to enter text.
		/// </summary>
		/// <param name="_title">The string that appears in the title bar.</param>
		/// <param name="_message">Descriptive text that provides more details than the title.</param>
		/// <param name="_placeholder">The string that is displayed when there is no other text in the textfield.</param>
		/// <param name="_buttonsList">An array of string values, used as title of action buttons.</param>
		/// <param name="_onCompletion">Callback that will be called after operation is completed.</param>
		/// <example>
		/// The following code example demonstrates how to prompt user to enter profile name.
		/// <code>
		/// using UnityEngine;
		/// using System.Collections;
		/// using VoxelBusters.NativePlugins;
		/// 
		/// public class ExampleClass : MonoBehaviour 
		/// {
		/// 	public void ShowPromptDialog ()
		/// 	{
		/// 		string[]	_buttons	= new string[] {
		/// 			"Ok",
		/// 			"Cancel"
		/// 		};
		/// 
		/// 		NPBinding.UI.ShowSingleFieldPromptDialogWithPlainText("Profile", "Please enter a profile name to store your game progress.", "username", _buttons, OnButtonPressed);
		///     }
		/// 
		/// 	private void OnButtonPressed (string _buttonPressed, string _inputText)
		/// 	{
		/// 		Debug.Log("Button pressed: " + _buttonPressed);
		/// 		Debug.Log("Input text: " + _inputText);
		/// 	}
		/// }
		/// </code>
		/// </example>
		public void ShowSingleFieldPromptDialogWithPlainText (string _title, string _message, string _placeholder, string[] _buttons, SingleFieldPromptCompletion _onCompletion)
		{
			ShowSingleFieldPromptDialog(_title, _message, _placeholder, false, _buttons, _onCompletion);
		}

		/// <summary>
		/// Shows a prompt dialog that allows the user to enter obscure text.
		/// </summary>
		/// <param name="_title">The string that appears in the title bar.</param>
		/// <param name="_message">Descriptive text that provides more details than the title.</param>
		/// <param name="_placeholder">The string that is displayed when there is no other text in the textfield.</param>
		/// <param name="_buttonsList">An array of string values, used as title of action buttons.</param>
		/// <param name="_onCompletion">Callback that will be called after operation is completed.</param>
		public void ShowSingleFieldPromptDialogWithSecuredText (string _title, string _message, string _placeholder, string[] _buttons, SingleFieldPromptCompletion _onCompletion)
		{
			ShowSingleFieldPromptDialog(_title, _message, _placeholder, true, _buttons, _onCompletion);
		}
		
		protected virtual void ShowSingleFieldPromptDialog (string _title, string _message, string _placeholder, bool _useSecureText, string[] _buttonsList, SingleFieldPromptCompletion _onCompletion)
		{
			// Cache callback
			OnSingleFieldPromptClosed	= _onCompletion;
		}

		/// <summary>
		/// Shows a prompt dialog that allows the user to enter login details.
		/// </summary>
		/// <param name="_title">The string that appears in the title bar.</param>
		/// <param name="_message">Descriptive text that provides more details than the title.</param>
		/// <param name="_usernamePlaceHolder">The string that is displayed when there is no other text in the username textfield.</param>
		/// <param name="_passwordPlaceHolder">The string that is displayed when there is no other text in the password textfield.</param>
		/// <param name="_buttonsList">An array of string values, used as title of action buttons.</param>
		/// <param name="_onCompletion">Callback that will be called after operation is completed.</param>
		/// <example>
		/// The following code example demonstrates how to prompt user to enter login details.
		/// <code>
		/// using UnityEngine;
		/// using System.Collections;
		/// using VoxelBusters.NativePlugins;
		/// 
		/// public class ExampleClass : MonoBehaviour 
		/// {
		/// 	public void ShowLoginDialog ()
		/// 	{
		/// 		string[]	_buttons	= new string[] {
		/// 			"Ok",
		/// 			"Cancel"
		/// 		};
		/// 
		/// 		NPBinding.UI.ShowSingleFieldPromptDialogWithPlainText("Example Game", "Please enter login details.", "username", "password", _buttons, OnButtonPressed);
		///     }
		/// 
		/// 	private void OnButtonPressed (string _buttonPressed, string _usernameText, string _passwordText)
		/// 	{
		/// 		Debug.Log("Button pressed: " + _buttonPressed);
		/// 		Debug.Log("Input username is: " + _usernameText);
		/// 		Debug.Log("Input password is: " + _passwordText);
		/// 	}
		/// }
		/// </code>
		/// </example>
		public virtual void ShowLoginPromptDialog (string _title, string _message, string _usernamePlaceHolder, string _passwordPlaceHolder, string[] _buttons, LoginPromptCompletion _onCompletion)
		{
			// Cache callback
			OnLoginPromptClosed			= _onCompletion;
		}
		
		#endregion
	}
}
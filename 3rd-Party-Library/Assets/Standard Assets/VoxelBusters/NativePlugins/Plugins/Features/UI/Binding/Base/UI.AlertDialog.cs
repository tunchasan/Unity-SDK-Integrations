using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace VoxelBusters.NativePlugins
{
	public partial class UI : MonoBehaviour 
	{
		#region Constants
			
		private const string kDefaultTextForButton = "Ok";

		#endregion

		#region Alert Dialog API's

		/// <summary>
		/// Shows an alert dialog with single button.
		/// </summary>
		/// <param name="_title">The string that appears in the title bar.</param>
		/// <param name="_message">Descriptive text that provides more details than the title.</param>
		/// <param name="_button">Title of action button.</param>
		/// <param name="_onCompletion">Callback that will be called after operation is completed.</param>
		/// <example>
		/// The following code example demonstrates how to show alert dialog with single button.
		/// <code>
		/// using UnityEngine;
		/// using System.Collections;
		/// using VoxelBusters.NativePlugins;
		/// 
		/// public class ExampleClass : MonoBehaviour 
		/// {
		/// 	public void ShowAlertDialog ()
		/// 	{
		/// 		NPBinding.UI.ShowAlertDialogWithSingleButton("Test", "This is a sample message.", "Ok", OnButtonPressed);
		///     }
		/// 
		/// 	private void OnButtonPressed (string _buttonPressed)
		/// 	{
		/// 		Debug.Log("Button pressed: " + _buttonPressed);
		/// 	}
		/// }
		/// </code>
		/// </example>
		public void ShowAlertDialogWithSingleButton (string _title, string _message, string _button, AlertDialogCompletion _onCompletion)
		{
			ShowAlertDialogWithMultipleButtons(_title, _message, new string[] {_button }, _onCompletion); 
		}

		/// <summary>
		/// Shows an alert dialog with multiple buttons.
		/// </summary>
		/// <param name="_title">The string that appears in the title bar.</param>
		/// <param name="_message">Descriptive text that provides more details than the title.</param>
		/// <param name="_buttonsList">An array of string values, used as title of action buttons.</param>
		/// <param name="_onCompletion">Callback that will be called after operation is completed.</param>
		/// <example>
		/// The following code example demonstrates how to show alert dialog with 2 action buttons.
		/// <code>
		/// using UnityEngine;
		/// using System.Collections;
		/// using VoxelBusters.NativePlugins;
		/// 
		/// public class ExampleClass : MonoBehaviour 
		/// {
		/// 	public void ShowAlertDialog ()
		/// 	{
		/// 		string[]	_buttons	= new string[] {
		/// 			"Ok",
		/// 			"Cancel"
		/// 		};
		/// 
		/// 		NPBinding.UI.ShowAlertDialogWithMultipleButtons("Test", "This is a sample message.", _buttons, OnButtonPressed);
		///     }
		/// 
		/// 	private void OnButtonPressed (string _buttonPressed)
		/// 	{
		/// 		Debug.Log("Button pressed: " + _buttonPressed);
		/// 	}
		/// }
		/// </code>
		/// </example>
		public void ShowAlertDialogWithMultipleButtons (string _title, string _message, string[] _buttons, AlertDialogCompletion _onCompletion)
		{
			// Cache callback
			string _callbackTag	= CacheAlertDialogCallback(_onCompletion);

			// Show alert
			ShowAlertDialogWithMultipleButtons(_title, _message, _buttons, _callbackTag);
		}

		protected virtual void ShowAlertDialogWithMultipleButtons (string _title, string _message, string[] _buttons, string _callbackTag)
		{
			if (_buttons == null || _buttons.Length == 0)
			{
				_buttons 	= new string[] {
					kDefaultTextForButton // Adding default text
				}; 
			}
			else if (string.IsNullOrEmpty(_buttons[0]))
			{
				_buttons[0] = kDefaultTextForButton;
			}
		}

		#endregion
	}
}
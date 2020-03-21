using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.NativePlugins.Internal
{
	public class RateMyAppGenericController : RateMyAppGenericStoreBase, IRateMyAppViewController, IRateMyAppEventResponder 
	{
		#region IRateMyAppViewController Implementation

		public void ShowDialog(string _title, string _message, 
		                       string[] _buttons, ShowDialogResultDelegate _onCompletion)
		{
			NPBinding.UI.ShowAlertDialogWithMultipleButtons(_title, 		
			                                                _message, 
			                                                _buttons, 	
			                                                (_buttonPressed) => 
			{
				if (_onCompletion != null)
					_onCompletion(_buttonPressed);
			});		
		}

		#endregion

		#region IRateMyAppEventResponder Implementation

		public void OnRemindMeLater()
		{}

		public void OnRate()
		{
			NPBinding.Utility.OpenStoreLink(_applicationID: NPSettings.Application.StoreIdentifier);
		}

		public void OnDontShow()
		{}

		#endregion
	}
}
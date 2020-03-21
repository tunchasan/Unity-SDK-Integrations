#if UNITY_IOS && USES_RATE_MY_APP
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

namespace VoxelBusters.NativePlugins.Internal
{
	public class RateMyAppIOSStoreController : RateMyAppGenericStoreBase, IRateMyAppViewController, IRateMyAppEventResponder 
	{
		#region Internal Methods

		[DllImport("__Internal")]
		private static extern bool cpnpUseDeepLinking();
		[DllImport("__Internal")]
		private static extern void cpnpRequestReview();

		#endregion

		#region IRateMyAppViewController Implementation

		public void ShowDialog(string _title, string _message, 
		                       string[] _buttons, ShowDialogResultDelegate _onCompletion)
		{
			if (cpnpUseDeepLinking())
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
			else
			{
				if (_onCompletion != null)
					_onCompletion(_buttons[0]);
			}
		}

		#endregion

		#region IRateMyAppEventResponder Implementation

		public void OnRemindMeLater()
		{}

		public void OnRate()
		{
			if (cpnpUseDeepLinking())
			{
				string	_appstoreURL	= string.Format("itms-apps://itunes.apple.com/app/id{0}?action=write-review", NPSettings.Application.StoreIdentifier);
				Application.OpenURL(_appstoreURL);
			}
			else
			{
				cpnpRequestReview();
			}
		}

		public void OnDontShow()
		{}

		#endregion
	}
}
#endif
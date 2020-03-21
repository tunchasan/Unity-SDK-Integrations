#if USES_BILLING && UNITY_IOS
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using VoxelBusters.Utility;
using VoxelBusters.UASUtils;

namespace VoxelBusters.NativePlugins
{
	using Internal;

	public partial class BillingIOS : Billing
	{
		#region Native Methods

		[DllImport("__Internal")]
		private static extern void cpnpBillingInit (bool _supportsReceiptValidation, string _validateUsingServerURL, string _sharedSecret);

		[DllImport("__Internal")]
		private static extern void cpnpBillingRequestForBillingProducts (string _consumableProductIDs, string _nonConsumableProductIDs);
		
		[DllImport("__Internal")]
		private static extern bool cpnpBillingCanMakePayments ();
		
		[DllImport("__Internal")]
		private static extern bool cpnpBillingIsProductPurchased (string _productID);
		
		[DllImport("__Internal")]
		private static extern void cpnpBillingBuyProduct (string _productID);
		
		[DllImport("__Internal")]
		private static extern void cpnpBillingRestoreCompletedTransactions ();

		[DllImport("__Internal")]
		private static extern void cpnpBillingFinishCompletedTransactions (string _transactionIDs, bool _isRestoreType);

		#endregion

		#region Overriden API's

		protected override void Initialise (BillingSettings _settings)
		{
			base.Initialise(_settings);

			// Get receipt validation URL
			BillingSettings.iOSSettings _iOSSettings			= _settings.iOS;
			string 						_validateUsingServerURL	= null;

			if (_iOSSettings.SupportsReceiptValidation)
			{
				// But user has forgot to set it, safe case we will use apple server
				if (string.IsNullOrEmpty(_iOSSettings.ValidateUsingServerURL))
				{
					_validateUsingServerURL	= null;
				}
				else
				{
					_validateUsingServerURL	= _iOSSettings.ValidateUsingServerURL;
				}
			}

			// Native store init is called
			cpnpBillingInit(_iOSSettings.SupportsReceiptValidation, _validateUsingServerURL, null);
		}

		public override bool IsAvailable ()
		{
			return true;
		}

		public override bool CanMakePayments ()
		{
			return cpnpBillingCanMakePayments();
		}

		protected override void RequestForBillingProducts (string[] _consumableProductIDs, string[] _nonConsumableProductIDs)
		{
			// Send request to native store
			cpnpBillingRequestForBillingProducts(_consumableProductIDs.ToJSON(), _nonConsumableProductIDs.ToJSON());
		}

#pragma warning disable
		public override bool IsProductPurchased (string _productID)
		{
			bool _isPurchased	= false;
			
			if (!string.IsNullOrEmpty(_productID))
				_isPurchased	= cpnpBillingIsProductPurchased(_productID);

			DebugUtility.Logger.Log(Constants.kDebugTag, string.Format("[Billing] Product= {0} IsPurchased= {1}.", _productID, _isPurchased));

			return _isPurchased;
		}

		public override void BuyProduct (string _productID)
		{
			base.BuyProduct(_productID);

			// Native call
			if (!string.IsNullOrEmpty(_productID)) 
				cpnpBillingBuyProduct(_productID);
		}
#pragma warning restore

		public override void RestorePurchases ()
		{
			base.RestorePurchases();

			// Native call
			cpnpBillingRestoreCompletedTransactions();
		}		

		#endregion
	}
}
#endif	
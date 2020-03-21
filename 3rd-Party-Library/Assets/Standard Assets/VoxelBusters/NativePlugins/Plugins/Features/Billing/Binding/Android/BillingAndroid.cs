#if USES_BILLING && UNITY_ANDROID
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using VoxelBusters.Utility;
using VoxelBusters.UASUtils;

namespace VoxelBusters.NativePlugins
{
	using Internal;

	public partial class BillingAndroid : Billing
	{

		#region Constructors

		BillingAndroid()
		{
			Plugin = AndroidPluginUtility.GetSingletonInstance(Native.Class.NAME);
		}

		#endregion

		#region Overriden API's

		protected override void Initialise (BillingSettings _settings)
		{
			base.Initialise(_settings);

			string _publicKey	= _settings.Android.PublicKey;

			if(string.IsNullOrEmpty(_publicKey))
			{
				DebugUtility.Logger.LogError(Constants.kDebugTag, "[Billing] Please specify public key in the configuration to proceed");
				_publicKey = "";
			}

			string[] _consumableProductIDs = GetConsumableProductIDs(_settings.Products);

			// Native store init is called
			Plugin.Call(Native.Methods.INITIALIZE, _publicKey, _consumableProductIDs.ToJSON()); //Update with consumable products initially.
		}

		public override bool IsAvailable ()
		{
			return (Plugin != null);
		}

		public override bool CanMakePayments ()
		{
			return (Plugin != null) && Plugin.Call<bool>(Native.Methods.IS_INITIALIZED);
		}

		protected override void RequestForBillingProducts (string[] _consumableProductIDs, string[] _nonConsumableProductIDs)
		{
			// Send request to native store
			Plugin.Call(Native.Methods.REQUEST_BILLING_PRODUCTS,_consumableProductIDs.ToJSON(), _nonConsumableProductIDs.ToJSON());
		}

		public override void BuyProduct (BillingProduct _product)
		{
			if (_product != null)
			{
				BuyProduct(_product.ProductIdentifier, _product.DeveloperPayload);
			}
			else
			{
				Debug.LogError("[Billing] Product can't be null");
			}
		}

		private void BuyProduct (string _productID , string _developerPayload)
		{
			#pragma warning disable
			base.BuyProduct(_productID);
			#pragma warning restore

			if (!string.IsNullOrEmpty(_productID))
			{
				Plugin.Call(Native.Methods.BUY_PRODUCT,_productID, _developerPayload);
			}
		}


#pragma warning disable

		public override bool IsProductPurchased (string _productID)
		{
			bool _isPurchased	= false;

			if (!string.IsNullOrEmpty(_productID))
				_isPurchased	= Plugin.Call<bool>(Native.Methods.IS_PRODUCT_PURCHASED,_productID);

			DebugUtility.Logger.Log(Constants.kDebugTag, string.Format("[Billing] Product= {0} IsPurchased= {1}.", _productID, _isPurchased));

			return _isPurchased;
		}

		public override void BuyProduct (string _productID)
		{
			if (!string.IsNullOrEmpty(_productID))
			{
				BuyProduct(_productID, null);
			}
		}

#pragma warning restore

		public override void RestorePurchases ()
		{
			base.RestorePurchases();

			// Native call
			Plugin.Call(Native.Methods.RESTORE_COMPLETED_TRANSACTIONS);
		}

		#endregion

		#region Helpers

		private string[] GetConsumableProductIDs(BillingProduct[] _billingProducts)
		{
			if (_billingProducts == null) 
			{
				return new string[0];
			}

			List<string> _consumableProductIDList		= new List<string>();

			foreach (BillingProduct _currentProduct in _billingProducts)
			{
				if (_currentProduct.IsConsumable)
					_consumableProductIDList.Add(_currentProduct.ProductIdentifier);
			}

			return _consumableProductIDList.ToArray();
		}

		#endregion
	}
}
#endif

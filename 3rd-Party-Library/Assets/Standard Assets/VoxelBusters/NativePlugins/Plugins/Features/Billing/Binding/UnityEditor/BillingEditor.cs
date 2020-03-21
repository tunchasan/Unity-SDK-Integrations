#if USES_BILLING && UNITY_EDITOR
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using VoxelBusters.UASUtils;

namespace VoxelBusters.NativePlugins
{	
	using Internal;

	public partial class BillingEditor : Billing 
	{
		#region API's

		protected override void Initialise (BillingSettings _settings)
		{
			base.Initialise(_settings);

			EditorStore.Initialise();
		}
		
		public override bool IsAvailable ()
		{
			return true;
		}
		
		public override bool CanMakePayments ()
		{
			return true;
		}

		public override void RequestForBillingProducts (BillingProduct[] _billingProducts)
		{
			// Cache requested products
			m_requestedProducts	= _billingProducts;

			EditorStore.RequestForBillingProducts(_billingProducts);
		}

#pragma warning disable
		public override bool IsProductPurchased (string _productID)
		{
			bool _isPurchased	= false;
			
			if (!string.IsNullOrEmpty(_productID))
				_isPurchased	= EditorStore.IsProductPurchased(_productID);

			DebugUtility.Logger.Log(Constants.kDebugTag, string.Format("[Billing] Product= {0} IsPurchased= {1}.", _productID, _isPurchased));

			return _isPurchased;
		}

		public override void BuyProduct (string _productID)
		{
			base.BuyProduct(_productID);

			if (!string.IsNullOrEmpty(_productID)) 
				EditorStore.BuyProduct(_productID);
		}
#pragma warning restore

		public override void RestorePurchases ()
		{
			base.RestorePurchases();

			EditorStore.RestorePurchases();
		}

		#endregion
	}
}
#endif
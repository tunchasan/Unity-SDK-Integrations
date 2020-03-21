#if USES_BILLING
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using VoxelBusters.UASUtils;

namespace VoxelBusters.NativePlugins
{
	using Internal;

	/// <summary>
	/// Provides cross-platform interface to request payment from a user for additional functionality or content that your application delivers.	
	/// </summary>
	///	<description>
	/// <para>
	/// This feature connects to the Store on your app’s behalf to securely process payments from users, prompting them to authorize payment. 
	/// The feature then notifies your app, which provides the purchased items to users. 
	/// For processing requests, feature contacts App Store, Google Play Store on iOS, Android platform respectively.
	/// You need to configure iOS billing product details at <a href="https://developer.apple.com/library/ios/IAPConfigGuide">iTunes Connect</a>. 
	/// Similarly for Android, you can set these details at <a href="http://developer.android.com/google/play/billing/billing_admin.html">Google Play Developer Console</a>.
	/// </para>
	/// <para>
	/// The interaction between the user, your app, and the Store during the purchase process take place in three stages.
	/// First, the your app displays purchasable products received from the Store. 
	/// Second, the user selects a product to buy and the app requests payment from the Store. 
	/// Third, the Store processes the payment and your app delivers the purchased product.
	/// </para>
	/// <para>
	/// Optionally, you can choose to verify receipts of completed transactions. The receipt is a record of purchase made from within the application and enabling receipt validation, adds one more level security to avoid unauthorised purchases.</para>
	/// <para>
	/// Users can also restore products that were previously purchased. As per iOS guidelines, if your application supports product types that are restorable, you must include an interface that allows users to restore these purchases.
	/// </para>
	///	</description>
	public partial class Billing : MonoBehaviour 
	{
		#region Fields
	
		protected 	BillingProduct[]	m_requestedProducts	= null;
		private 	BillingProduct[] 	m_storeProducts		= null;

		#endregion

		#region Unity Methods

		private void Awake ()
		{
			if (!NPSettings.Application.SupportedFeatures.UsesBilling)
				return;

			// Intialise component
			Initialise(NPSettings.Billing);
		}

		#endregion

		#region API's

		protected virtual void Initialise (BillingSettings _settings)
		{
#if USES_SOOMLA_GROW
			bool	_isSupported 	= IsAvailable();

			NPBinding.SoomlaGrowService.ReportOnBillingSupported(_isSupported);
#endif
		}

		/// <summary>
		/// Determines whether the billing feature is supported.
		/// </summary>
		/// <returns><c>true</c> if billing feature is supported; otherwise, <c>false</c>.</returns>
		public virtual bool IsAvailable ()
		{
			return false;
		}

		/// <summary>
		/// Determines whether the user is authorised to make payments.
		/// </summary>
		/// <returns><c>true</c> if the user is allowed to make product purchase payment; otherwise, <c>false</c>.</returns>
		public virtual bool CanMakePayments ()
		{
			return false;
		}

		/// <summary>
		/// Gets the billing product with localized information, which was previously fetched from the Store.
		/// </summary>
		/// <returns>The billing product fetched with localized information.</returns>
		/// <param name="_productID">A string used to identify a billing product.</param>
		public BillingProduct GetStoreProduct (string _productID)
		{
			if (m_storeProducts == null)
			{
				DebugUtility.Logger.LogWarning(Constants.kDebugTag, "[Billing] Store products information not found.");

				return null;
			}

			return System.Array.Find<BillingProduct>(m_storeProducts, (_currentProduct)=>{
				return _currentProduct.ProductIdentifier.Equals(_productID);
			});
		}

		/// <summary>
		/// Sends a request to retrieve localized information about the billing products from the Store.	
		/// </summary>
		///	<description> 
		/// Call to this method retrieves information of the products that are configured in <c>Billing Settings</c>.
		/// Your application uses this request to present localized prices and other information to the user without having to maintain that list itself. 
		/// </description>
		/// <remarks>
		/// \note When the request completes, <see cref="DidFinishProductsRequestEvent"/> is fired.
		/// </remarks>
		public void RequestForBillingProducts ()
		{
			BillingProduct[]	_billingProducts	= NPSettings.Billing.Products;

			RequestForBillingProducts(_billingProducts);
		}

		/// <summary>
		/// Sends a request to retrieve localized information about the billing products from the Store.	
		/// </summary>
		///	<description> 
		/// Your application uses this request to present localized prices and other information to the user without having to maintain that list itself. 
		/// </description>
		/// <param name="_billingProducts">List of billing products whose details has to be fetched.</param>
		/// <remarks>
		/// \note When the request completes, <see cref="DidFinishProductsRequestEvent"/> is fired.
		/// </remarks>
		public virtual void RequestForBillingProducts (BillingProduct[] _billingProducts)
		{
			if (_billingProducts == null || _billingProducts.Length == 0)
			{
				DidReceiveBillingProducts(null, "The operation could not be completed because products list is empty.");

				return;
			}

			// Cache requested products details
			m_requestedProducts	= _billingProducts;

			// Gather consumable and non consumable product ids
			List<string> _consumableProductIDList		= new List<string>();
			List<string> _nonConsumableProductIDList	= new List<string>();
			
			foreach (BillingProduct _currentProduct in _billingProducts)
			{
				if (_currentProduct.IsConsumable)
					_consumableProductIDList.Add(_currentProduct.ProductIdentifier);
				else
					_nonConsumableProductIDList.Add(_currentProduct.ProductIdentifier);
			}

			// Request for billing products
			RequestForBillingProducts(_consumableProductIDList.ToArray(), _nonConsumableProductIDList.ToArray());
		}

		protected virtual void RequestForBillingProducts (string[] _consumableProductIDs, string[] _nonConsumableProductIDs)
		{}

		/// <summary>
		/// Determines whether specified billing product is already purchased.
		/// </summary>
		/// <returns><c>true</c> if specified billing product is already purchased; otherwise, <c>false</c>.</returns>
		/// <param name="_product">The object identifies the billing product registered in the Store.</param>
		/// <remarks> 
		/// \note This works only for Non-Consumable (Managed) billing product. For Consumable products, this will always returns false.
		/// </remarks>
		public bool IsProductPurchased (BillingProduct _product)
		{
			string	_productID	= (_product == null) ? null : _product.ProductIdentifier;

#pragma warning disable
			return IsProductPurchased(_productID);
#pragma warning restore
		}

		/// <summary>
		/// Initiates purchase process for the specified billing product.
		/// </summary>
		/// <param name="_product">The object identifies the billing product registered in the Store.</param>
		/// <remarks>
		/// \note The payment request must have a product identifier registered with the Store.
		/// </remarks>
		public virtual void BuyProduct (BillingProduct _product)
		{
			string	_productID	= (_product == null) ? null : _product.ProductIdentifier;

#pragma warning disable
			BuyProduct(_productID);
#pragma warning restore
		}
		
		/// <summary>
		/// Sends a request to restore completed purchases.
		/// </summary>
		/// <description>
		/// Your application calls this method to restore transactions that were previously purchased so that you can process them again.
		/// </description>
		///	<remarks> 
		/// \note 
		/// Internally this feature requires consumable product information. So ensure that <see cref="RequestForBillingProducts"/> is called prior to this. 
		/// </remarks>
		public virtual void RestorePurchases ()
		{
#if USES_SOOMLA_GROW
			NPBinding.SoomlaGrowService.ReportOnBillingPurchasesRestoreStarted();
#endif
		}

		#endregion

		#region Deprecated Methods

		[System.Obsolete("This method is deprecated. Instead use IsAvailable.")]
		public bool IsSupported ()
		{
			return IsAvailable();
		}

		[System.Obsolete("This method is deprecated. Instead use IsProductPurchased (BillingProduct _product).")]
		public virtual bool IsProductPurchased (string _productID)
		{
			bool _isPurchased	= false;

			DebugUtility.Logger.Log(Constants.kDebugTag, string.Format("[Billing] Product= {0} IsPurchased= {1}.", _productID, _isPurchased));

			return _isPurchased;
		}
		
		[System.Obsolete("This method is deprecated. Instead use BuyProduct (BillingProduct _product).")]
		public virtual void BuyProduct (string _productID)
		{
			if (string.IsNullOrEmpty(_productID))
			{
				DidFinishProductPurchase(null, "The operation could not be completed because product identifier is invalid.");
			
				return;
			}

#if USES_SOOMLA_GROW
			NPBinding.SoomlaGrowService.ReportOnBillingPurchaseStarted(_productID);
#endif
		}
		
		[System.Obsolete("This method is deprecated. Instead use RestorePurchases.")]
		public void RestoreCompletedTransactions ()
		{
			RestorePurchases();
		}

		[System.Obsolete("This method is deprecated. Instead use BillingTransaction.OnCustomVerificationFinished (eBillingTransactionVerificationState _newState).")]
		public void CustomVerificationFinished (BillingTransaction _transaction)
		{}

		#endregion
	}
}
#endif
#if USES_BILLING
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using VoxelBusters.Utility;
using VoxelBusters.UASUtils;

namespace VoxelBusters.NativePlugins
{
	using Internal;

	public partial class Billing : MonoBehaviour 
	{
		#region Delegates

		///	<summary>
		///	Delegate that will be called when localized information of products are retreived from the Store.
		///	</summary>
		///	<param name="_products">An array of <see cref="BillingProduct"/> objects, that holds localized information of products.</param>
		/// <param name="_error">If the operation was successful, this value is nil; otherwise, this parameter holds the description of the problem that occurred.</param>
		public delegate void RequestForBillingProductsCompletion (BillingProduct[] _products, string _error);

		///	<summary>
		///	Delegate that will be called when Store completes processing product purchase request.
		///	</summary>
		///	<param name="_transaction">The <see cref="BillingTransaction"/> object holds payment details of the requested product.</param>
		public delegate void BuyProductCompletion (BillingTransaction _transaction);

		///	<summary>
		///	Delegate that will be called when payment details of the products (non-consumable only) previously purchased by the user are retreived from the Store. 
		///	</summary>
		///	<param name="_transactions">An array of <see cref="BillingTransaction"/> objects, that holds payment details of products to be restored.</param>
		/// <param name="_error">If the operation was successful, this value is nil; otherwise, this parameter holds the description of the problem that occurred.</param>
		public delegate void RestorePurchasesCompletion (BillingTransaction[] _transactions, string _error);

		#endregion

		#region Events
		
		/// <summary>
		/// Event that will be called when localized information of billing products are retreived from the Store.
		/// </summary>
		/// <example>
		/// The following example illustrates how to handle response to a request for information about a list of products.
		/// <code>
		/// using UnityEngine;
		/// using System.Collections;
		/// using VoxelBusters.NativePlugins;
		/// 
		/// public class ExampleClass : MonoBehaviour 
		/// {
		/// 	private void OnEnable ()
		/// 	{
		/// 		// Registering for event
		/// 	    Billing.DidFinishRequestForBillingProductsEvent	+= OnDidFinishRequestForBillingProducts;
		///     }
		/// 
		/// 	private void OnDisable ()
		/// 	{
		/// 		// Unregistering event
		/// 	    Billing.DidFinishRequestForBillingProductsEvent	-= OnDidFinishRequestForBillingProducts;
		/// 	}
		/// 
		/// 	private void OnDidFinishRequestForBillingProducts (BillingProduct[] _products, string _error)
		/// 	{
		/// 		if (_error == null)
		/// 		{
		/// 			foreach (BillingProduct _currentProduct in _products)
		/// 			{
		/// 				// Insert your code, eg: populating app's store screen
		/// 			}
		/// 		}
		/// 		else
		/// 		{
		/// 			// Something went wrong
		/// 			// Show an alert message informing user that store initialisation failed
		/// 		}
		/// 	}
		/// }
		/// </code>
		/// </example>
		public static event RequestForBillingProductsCompletion DidFinishRequestForBillingProductsEvent;

		/// <summary>
		/// Event that will be called when product payment response is received from the Store.
		/// </summary>
		/// <example>
		/// The following example illustrates how to handle product payment response.
		/// <code>
		/// using UnityEngine;
		/// using System.Collections;
		/// using VoxelBusters.NativePlugins;
		/// 
		/// public class ExampleClass : MonoBehaviour 
		/// {
		/// 	private void OnEnable ()
		/// 	{
		/// 		// Registering for event
		/// 	    Billing.DidFinishProductPurchaseEvent	+= OnDidFinishProductPurchase;
		///     }
		/// 
		/// 	private void OnDisable ()
		/// 	{
		/// 		// Unregistering event
		/// 	    Billing.DidFinishProductPurchaseEvent	-= OnDidFinishProductPurchase;
		/// 	}
		/// 
		/// 	private void OnDidFinishProductPurchase (BillingTransaction _transaction)
		/// 	{
		/// 		if (_transaction.VerificationState == eBillingTransactionVerificationState.SUCCESS)
		/// 		{
		/// 			if (_transaction.TransactionState == eBillingTransactionState.PURCHASED)
		/// 			{
		/// 				// Product successfully purchased; Insert code to credit the item associated with this product
		/// 			}
		///				else if (_transaction.TransactionState == eBillingTransactionState.FAILED)
		/// 			{
		/// 				// Product was not purchased; Insert code to handle failed purchase scenario
		/// 			}
		/// 		}
		/// 		else if (_transaction.VerificationState == eBillingTransactionVerificationState.FAILED)
		/// 		{
		/// 			// Something went wrong!
		/// 			// At this point, we recommend you not to credit the associated item
		/// 		}
		/// 		else
		/// 		{
		/// 			// Received transaction object is not validated yet!
		/// 			// This can happen only when developer opts out of inbuilt receipt validation
		/// 			// Now its your responsiblity to provide appropriate receipt validation status for this transaction
		/// 			// Skipping this step will cause unusual behaviour
		/// 			_transaction.OnCustomVerificationFinished(_newVerificationState);
		/// 		}	
		/// 	}
		/// }
		/// </code>
		/// </example>
		public static event BuyProductCompletion DidFinishProductPurchaseEvent;

		/// <summary>
		/// Event that will be called when restored transaction details are received from the Store.
		/// </summary>
		/// <example>
		/// The following example illustrates how to handle a response to restore purchases request.
		/// <code>
		/// using UnityEngine;
		/// using System.Collections;
		/// using VoxelBusters.NativePlugins;
		/// 
		/// public class ExampleClass : MonoBehaviour 
		/// {
		/// 	private void OnEnable ()
		/// 	{
		/// 		// Registering for event
		/// 	    Billing.DidFinishRestoringPurchasesEvent	+= OnDidFinishRestoringPurchases;
		///     }
		/// 
		/// 	private void OnDisable ()
		/// 	{
		/// 		// Unregistering event
		/// 	    Billing.DidFinishRestoringPurchasesEvent	-= OnDidFinishRestoringPurchases;
		/// 	}
		/// 
		/// 	private void OnDidFinishRestoringPurchases (BillingTransaction[] _transactions, string _error)
		/// 	{
		/// 		if (_error == null)
		/// 		{
		/// 			foreach (BillingTransaction _eachTransaction in _transactions)
		/// 			{
		/// 				if (_eachTransaction.VerificationState == eBillingTransactionVerificationState.SUCCESS)
		/// 				{
		/// 					// Insert code to restore product associated with this transaction
		/// 				}
		/// 				else if (_transaction.VerificationState == eBillingTransactionVerificationState.FAILED)
		/// 				{
		/// 					// Something went wrong!
		/// 					// At this point, we recommend you not to credit the associated item
		/// 				}
		/// 				else
		/// 				{
		/// 					// Received transaction object is not validated yet!
		/// 					// This can happen only when developer opts out of inbuilt receipt validation
		/// 					// Now its your responsiblity to provide appropriate receipt validation status for this transaction
		/// 					// Skipping this step will cause unusual behaviour
		/// 					_transaction.OnCustomVerificationFinished(_newVerificationState);
		/// 				}	
		/// 			}
		/// 		}
		/// 		else
		/// 		{
		/// 			// Something went wrong
		/// 			// Display an alert informing user that restore request failed
		/// 		}
		/// 	}
		/// }
		/// </code>
		/// </example>
		public static event RestorePurchasesCompletion DidFinishRestoringPurchasesEvent;
		
		#endregion

		#region Native Callback

		protected virtual void DidReceiveBillingProducts (string _dataStr)
		{}
		
		protected void DidReceiveBillingProducts (BillingProduct[] _storeProducts, string _error)
		{
			DebugUtility.Logger.Log(Constants.kDebugTag, "[Billing] Request for billing products finished successfully.");

			// Cache information
			m_storeProducts	= _storeProducts;

			// Update product type information
			if (_storeProducts != null)
			{
				foreach (BillingProductMutable _currentStoreProduct in _storeProducts)
				{
					int 	_productIndex	= System.Array.FindIndex(m_requestedProducts, (BillingProduct _requestedProduct) => _currentStoreProduct.ProductIdentifier.Equals(_requestedProduct.ProductIdentifier));
					
					if (_productIndex != -1)
						_currentStoreProduct.SetIsConsumable(m_requestedProducts[_productIndex].IsConsumable);
				}
			}

			// Send event
			if (DidFinishRequestForBillingProductsEvent != null)
				DidFinishRequestForBillingProductsEvent(_storeProducts, _error);

#pragma warning disable
			// Backward compatibility for event callback
			if (DidFinishProductsRequestEvent != null)
				DidFinishProductsRequestEvent(_storeProducts, _error);
#pragma warning restore
		}
		
		protected void DidFinishProductPurchase (string _dataStr)
		{
			BillingTransaction[] 	_transactions;
			string					_error;
			ExtractTransactionResponseData(_dataStr, out _transactions, out _error);

			ProcessPurchaseTransactions(_transactions);

			DidFinishProductPurchase(_transactions, _error);
		}

		protected void DidFinishProductPurchase (BillingTransaction[] _transactions, string _error)
		{
			DebugUtility.Logger.Log(Constants.kDebugTag, "[Billing] Received product purchase information.");

			// Send event
			if (DidFinishProductPurchaseEvent != null)
			{
				if (_error == null)
				{
					foreach (BillingTransaction _transaction in _transactions)
						DidFinishProductPurchaseEvent(_transaction);
				}
				else
				{
					DidFinishProductPurchaseEvent(new BillingTransaction(_error));
				}
			}
		
#pragma warning disable
			// Backward compatibility for event callback
			if (DidReceiveTransactionInfoEvent != null)
				DidReceiveTransactionInfoEvent(_transactions, _error);
#pragma warning restore
		}

		protected void DidFinishRestoringPurchases (string _dataStr)
		{
			BillingTransaction[] 	_transactions;
			string					_error;
			ExtractTransactionResponseData(_dataStr, out _transactions, out _error);

			ProcessRestoredTransactions(_transactions);

			DidFinishRestoringPurchases(_transactions, _error);
		}

		protected void DidFinishRestoringPurchases (BillingTransaction[] _transactions, string _error)
		{
			DebugUtility.Logger.Log(Constants.kDebugTag, "[Billing] Received restored purchases information.");

			// Send event
			if (DidFinishRestoringPurchasesEvent != null)
				DidFinishRestoringPurchasesEvent(_transactions, _error);
				
#pragma warning disable
			// Backward compatibility for event callback
			if (DidReceiveTransactionInfoEvent != null)
				DidReceiveTransactionInfoEvent(_transactions, _error);
#pragma warning restore
		}

		protected virtual void ExtractTransactionResponseData (string _dataStr, out BillingTransaction[] _transactions, out string _error)
		{
			_transactions	= null;
			_error			= null;
		}

		protected virtual void ProcessPurchaseTransactions (BillingTransaction[] _transactions)
		{}
		
		protected virtual void ProcessRestoredTransactions (BillingTransaction[] _transactions)
		{}

		#endregion

		#region Deprecated Events

		public delegate void TransactionResponse (BillingTransaction[] _transactions, string _error);

		[System.Obsolete("This event is deprecated. Instead use DidFinishRequestForBillingProductsEvent.")]
		public static event RequestForBillingProductsCompletion DidFinishProductsRequestEvent;

		[System.Obsolete("This event is deprecated. Instead use DidFinishProductPurchaseEvent and DidFinishRestoringPurchasesEvent.")]
		public static event TransactionResponse DidReceiveTransactionInfoEvent;

		#endregion
	}
}
#endif
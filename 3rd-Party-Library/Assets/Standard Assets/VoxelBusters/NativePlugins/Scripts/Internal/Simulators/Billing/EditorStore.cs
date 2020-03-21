#if USES_BILLING && UNITY_EDITOR
using UnityEngine;
using System.Collections;
using UnityEditor;
using System;
using System.Collections.Generic;
using VoxelBusters.Utility;
using VoxelBusters.UASUtils;

namespace VoxelBusters.NativePlugins.Internal
{
	public class EditorStore 
	{
		#region Constants
		
		// Event names
		private 	const 	string 		kProductsRequestFinishedEventName	= "DidReceiveBillingProducts";
		private 	const 	string 		kProductPurchaseFinishedEventName	= "DidFinishProductPurchase";
		private 	const 	string 		kRestoreFinishedEventName			= "DidFinishRestoringPurchases";

		#endregion

		#region Properties

		private 	static 	BillingProduct[]	registeredProducts			= new BillingProductEditor[0];

		#endregion

		#region Exposed Methods

		public static void Initialise ()
		{
			CheckIfInitialised();
		}

		public static void RequestForBillingProducts (BillingProduct[] _productsList)
		{
			CheckIfInitialised();

			if (_productsList == null || _productsList.Length == 0)
			{
				registeredProducts	= new BillingProductEditor[0];

				// Trigger handler
				OnFinishedProductsRequest(null, "The operation could not be completed because product list is empty.");

				return;
			}
			else
			{
				// Create new registered product list
				List<BillingProductEditor> _newlyRegisteredProductList	= new List<BillingProductEditor>();

				// Create list of registered products with price info
				foreach (BillingProduct _curProduct in _productsList)
				{
					if (_curProduct != null)
					{
						BillingProductEditor 	_newRegProduct	= new BillingProductEditor(_curProduct);

						_newRegProduct.SetLocalizePrice(string.Format("${0:0.00}", _curProduct.Price));
						_newRegProduct.SetCurrencyCode("USD");
						_newRegProduct.SetCurrencySymbol("$");

						// Add to store products
						_newlyRegisteredProductList.Add(_newRegProduct);
					}
				}

				// Cache new list
				registeredProducts	= _newlyRegisteredProductList.ToArray();

				// Trigger handler
				OnFinishedProductsRequest(registeredProducts, null);

				return;
			}
		}

		private static void OnFinishedProductsRequest (BillingProduct[] _registeredProducts, string _error)
		{
			// Callback is sent to binding event listener
			if (NPBinding.Billing != null)
				NPBinding.Billing.InvokeMethod(kProductsRequestFinishedEventName, new object[] {
					_registeredProducts,
					_error
				}, new Type[] {
					typeof(BillingProduct[]),
					typeof(string)
				});
		}
		
		public static bool IsProductPurchased (string _productID)
		{
			CheckIfInitialised();

			return EditorPrefs.GetInt(_productID, 0) == 1;
		}
		
		public static void BuyProduct (string _productID)
		{
			CheckIfInitialised();

			BillingProduct _buyProduct	= GetStoreProduct(_productID);

			if (_buyProduct == null)
			{
				OnTransactionFailed(_productID, "The operation could not be completed because given product id information not found.");
				return;
			}

			if (NPBinding.UI != null)
			{
				string 		_message	= string.Format("Do you want to buy {0} for {1}?", _buyProduct.Name, _buyProduct.LocalizedPrice);

				NPBinding.UI.ShowAlertDialogWithMultipleButtons("Confirm your purchase", 
				                                                _message, 
				                                                new string[] { 
					"Cancel", 
					"Buy" 
				},
				(string _buttonPressed) => {

					if (_buttonPressed.Equals("Buy"))
					{
						OnConfirmingPurchase(_buyProduct);
					}
					else
					{
						OnTransactionFailed(_productID, "The operation could not be completed because user cancelled purchase.");
					}
				});
			}
			else
			{
				DebugUtility.Logger.LogWarning(Constants.kDebugTag, "[EditorStore] Native UI component is null");
				return;
			}
		}
		
		public static void RestorePurchases ()
		{
			CheckIfInitialised();

			if (registeredProducts == null)
			{
				DebugUtility.Logger.LogError(Constants.kDebugTag, "[EditorStore] Restore purchases can be done only after getting products information from store.");
				return;
			}

			List<BillingTransaction> 	_restoredTransactions	= new List<BillingTransaction>();

			foreach (BillingProduct _curProduct in registeredProducts)
			{
				if (IsProductPurchased(_curProduct.ProductIdentifier))
				{
					BillingTransaction _transaction	= CreateTransactionObject(_curProduct.ProductIdentifier, eBillingTransactionState.RESTORED, null);

					// Add it to list of restored transactions
					_restoredTransactions.Add(_transaction);
				}
			}

			PostTransactionEvent(kRestoreFinishedEventName, _restoredTransactions.ToArray());
		}
		
		public static void CustomVerificationFinished (BillingTransaction _transaction)
		{
			if (_transaction.TransactionState == eBillingTransactionState.RESTORED)
			{
				PostTransactionEvent(kRestoreFinishedEventName, new BillingTransaction[1] { 
					_transaction 
				});
			}
			else
			{
				PostTransactionEvent(kProductPurchaseFinishedEventName, new BillingTransaction[1] { 
					_transaction 
				});
			}
		}

		#endregion

		#region Misc Methods

		private static void OnConfirmingPurchase (BillingProduct _product)
		{
			if (!_product.IsConsumable)
			{
				EditorPrefs.SetInt(_product.ProductIdentifier, 1);
			}

			BillingTransaction	_newTransaction	= CreateTransactionObject(_product.ProductIdentifier, eBillingTransactionState.PURCHASED, null);

			PostTransactionEvent(kProductPurchaseFinishedEventName, new BillingTransaction[1] { 
				_newTransaction 
			});
		}

		private static void OnTransactionFailed (string _productID, string _errorDescription)
		{
			BillingTransaction	_newTransaction	= CreateTransactionObject(_productID, eBillingTransactionState.FAILED, _errorDescription);

			PostTransactionEvent(kProductPurchaseFinishedEventName, new BillingTransaction[1] { 
				_newTransaction 
			});
		}

		private static BillingTransaction CreateTransactionObject (string _productID, eBillingTransactionState _transactionState, string _error)
		{
			BillingTransaction _transaction;

			if (_transactionState == eBillingTransactionState.FAILED)
			{
				_transaction = new BillingTransactionEditor(_productID, System.DateTime.UtcNow, null, null, _transactionState, eBillingTransactionVerificationState.SUCCESS, _error);
			}
			else
			{
				_transaction = new BillingTransactionEditor(_productID, System.DateTime.UtcNow, "transactionIdentifier", "receipt", _transactionState, eBillingTransactionVerificationState.SUCCESS, null);
			}

			return _transaction;
		}

		private static BillingProduct GetStoreProduct (string _productID)
		{
			foreach (BillingProduct _curProduct in registeredProducts)
			{
				if (_curProduct.ProductIdentifier.Equals(_productID))
				{
					return _curProduct;
				}
			}

			return null;
		}

		private static void CheckIfInitialised ()
		{
#if UNITY_ANDROID
			if (string.IsNullOrEmpty(NPSettings.Billing.Android.PublicKey))
			{
				DebugUtility.Logger.LogError(Constants.kDebugTag, "[EditorStore] Please add public key in NPSettings for Billing to work on Android.");
			}
#endif
		}

		private static void PostTransactionEvent (string _eventName, BillingTransaction[] _finishedTransaction)
		{
			if (NPBinding.Billing != null)
				NPBinding.Billing.InvokeMethod(_eventName, new object[] {
					_finishedTransaction,
					null
				}, new Type[] {
					typeof(BillingTransaction[]),
					typeof(string)
				});
		}

		public static void ClearPurchaseHistory ()
		{
			BillingProduct[]	_products	= NPSettings.Billing.Products;

			if (_products == null)
				return;

			foreach (BillingProduct _currentProduct in _products)
				if (!string.IsNullOrEmpty(_currentProduct.ProductIdentifier))
					EditorPrefs.DeleteKey(_currentProduct.ProductIdentifier);
		}

		#endregion
	}
}
#endif
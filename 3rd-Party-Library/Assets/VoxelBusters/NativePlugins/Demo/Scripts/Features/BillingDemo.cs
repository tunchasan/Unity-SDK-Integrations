using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using VoxelBusters.Utility;
using VoxelBusters.NativePlugins;

namespace VoxelBusters.NativePlugins.Demo
{
#if !USES_BILLING
	public class BillingDemo : NPDisabledFeatureDemo 
	{}
#else
	public class BillingDemo : NPDemoBase 
	{
		#region Properties

		private 	int					m_productIter;
		private 	BillingProduct[] 	m_products;
		private		bool				m_productRequestFinished;

		#endregion

		#region Unity Methods

		protected override void Start ()
		{
			base.Start();

			// Intialise
			m_products					= NPSettings.Billing.Products;
			m_productRequestFinished	= false;

			// Set info texts
			AddExtraInfoTexts(
				"You can configure this feature in NPSettings->Billing Settings.",
				"In Billing Settings you can also store all the Product information and access it at runtime using getter NPSettings.Billing.Products.",
				"Billing workflow is pretty much simple. " +
					"\n1. Request for product infomation." +
					"\n2. If your product list includes non consummable products, then restore old purchases." +
					"\n3. Initiate purchase using BuyProduct API. Also use IsPurchased API to check if product is already purchase or not.");
		}

		protected override void OnEnable ()
		{
			base.OnEnable();

#if UNITY_ANDROID
			if (string.IsNullOrEmpty(NPSettings.Billing.Android.PublicKey))
			{
				AddNewResult("[NOTE] Add public key in NPSettings for billing on Android. Else purchase process will be aborted.");
			}
#endif
			// Register for callbacks
			Billing.DidFinishRequestForBillingProductsEvent	+= OnDidFinishRequestForBillingProducts;
			Billing.DidFinishProductPurchaseEvent			+= OnDidFinishProductPurchase;
			Billing.DidFinishRestoringPurchasesEvent		+= OnDidFinishRestoringPurchases;
		}

		protected override void OnDisable ()
		{
			base.OnDisable();

			// Deregister for callbacks
			Billing.DidFinishRequestForBillingProductsEvent	-= OnDidFinishRequestForBillingProducts;
			Billing.DidFinishProductPurchaseEvent			-= OnDidFinishProductPurchase;
			Billing.DidFinishRestoringPurchasesEvent		-= OnDidFinishRestoringPurchases;
		}
		
		#endregion

		#region GUI Methods
		
		protected override void DisplayFeatureFunctionalities ()
		{
			base.DisplayFeatureFunctionalities();
			
			if (GUILayout.Button ("Is Available"))
			{
				if (IsAvailable())
					AddNewResult("Billing feature is supported.");
				else
					AddNewResult("Billing feature is not supported.");
			}

			if (!IsAvailable())
			{
				GUILayout.Box("Sorry, Billing feature is not supported on this device.");
				return;
			}

			if (m_products == null)
			{
				GUILayout.Box("We couldn't find any product information. Please configure product details in NPSettings-> Billing Settings.");
				return;
			}
			
			GUILayout.Label("Product Requests", kSubTitleStyle);
			
			if (GUILayout.Button("Request For Billing Products"))
			{
				AddNewResult("Sending request to load product information from store.");
				RequestBillingProducts(m_products);
			}
			
			GUILayout.Box("[NOTE] On finishing product request, DidFinishRequestForBillingProductsEvent is triggered.");
			
			if (m_productRequestFinished)
			{
				if (GUILayout.Button("Restore Purchases"))
				{
					AddNewResult("Sending request to restore old purchases.");
					RestorePurchases();
				}
				
				GUILayout.Box("[NOTE] On finishing restore request, DidFinishRestoringPurchasesEvent is triggered.");
				
				GUILayout.Label("Product Purchases", kSubTitleStyle);

				if (GUILayout.Button("Can Make Payments"))
				{
					if (CanMakePayments())
						AddNewResult("User is authorised to make purchases.");
					else
						AddNewResult("User doesn't have permissions to make purchases.");
				}

				GUILayout.Box("Current billing product = " + GetCurrentProduct().Name);
				
				GUILayout.BeginHorizontal();
				{
					if (GUILayout.Button("Previous Product"))
					{ 
						GotoPreviousProduct();
					}
					
					if (GUILayout.Button("Next Product"))
					{
						GotoNextProduct();
					}
				}
				GUILayout.EndHorizontal();
				
				if (GUILayout.Button("Buy Product"))
				{
					AddNewResult(string.Format("Requesting to buy product = {0}.", GetCurrentProduct().Name));
					BuyProduct(GetCurrentProduct());
				}
				
				GUILayout.Box("[NOTE] On finishing product purchase request, DidFinishProductPurchaseEvent is triggered.");
				
				if (GUILayout.Button("Is Product Purchased"))
				{
					BillingProduct	_product		= GetCurrentProduct();
					bool 			_isPurchased 	= IsProductPurchased(_product);
					
					AddNewResult(string.Format("{0} {1}", _product.Name, _isPurchased ? "is already purchased." : "is not yet purchased!"));
				}
				
				GUILayout.Box("[NOTE] Purchase history is tracked only for non-consumable products.");
				
				if (GUILayout.Button("Is Consumable Product"))
				{
					BillingProduct	_product		= GetCurrentProduct();
					bool 			_isConsumable 	= _product.IsConsumable;
					
					AddNewResult(string.Format("{0} {1}", _product.Name, _isConsumable ? "is consumable product." : "is non-consumable product."));
				}
			}
		}
		
		#endregion

		#region API Methods

		private bool IsAvailable ()
		{
			return NPBinding.Billing.IsAvailable();
		}
			
		private void RequestBillingProducts (BillingProduct[]  _products)
		{
			NPBinding.Billing.RequestForBillingProducts(_products);
		}

		private void RestorePurchases ()
		{
			NPBinding.Billing.RestorePurchases();
		}

		private bool CanMakePayments ()
		{
			return NPBinding.Billing.CanMakePayments();
		}

		private void BuyProduct (BillingProduct _product)
		{
			NPBinding.Billing.BuyProduct(_product);
		}

		private bool IsProductPurchased (BillingProduct _product)
		{
			return NPBinding.Billing.IsProductPurchased(_product);
		}

		#endregion

		#region API Callback Methods

		private void OnDidFinishRequestForBillingProducts (BillingProduct[] _products, string _error)
		{
			AddNewResult(string.Format("Billing products request finished. Error = {0}.", _error.GetPrintableString()));

			if (_products != null)
			{
				m_productRequestFinished	= true;
				AppendResult(string.Format("Totally {0} billing products information were received.", _products.Length));

				foreach (BillingProduct _currentProduct in _products)
					AppendResult(_currentProduct.ToString());
			}
		}

		private void OnDidFinishProductPurchase (BillingTransaction _transaction)
		{
			AddNewResult("Received product purchase response.");
			AppendResult("Product Identifier = " 		+ _transaction.ProductIdentifier);
			AppendResult("Transaction State = "			+ _transaction.TransactionState);
			AppendResult("Verification State = "		+ _transaction.VerificationState);
			AppendResult("Transaction Date[UTC] = "		+ _transaction.TransactionDateUTC);
			AppendResult("Transaction Date[Local] = "	+ _transaction.TransactionDateLocal);
			AppendResult("Transaction Identifier = "	+ _transaction.TransactionIdentifier);
			AppendResult("Transaction Receipt = "		+ _transaction.TransactionReceipt);
			AppendResult("Error = "						+ _transaction.Error.GetPrintableString());
		}

		private void OnDidFinishRestoringPurchases (BillingTransaction[] _transactions, string _error)
		{
			AddNewResult(string.Format("Received restore purchases response. Error = {0}.", _error.GetPrintableString()));
			
			if (_transactions != null)
			{				
				AppendResult(string.Format("Count of transaction information received = {0}.", _transactions.Length));
				
				foreach (BillingTransaction _currentTransaction in _transactions)
				{
					AppendResult("Product Identifier = " 		+ _currentTransaction.ProductIdentifier);
					AppendResult("Transaction State = "			+ _currentTransaction.TransactionState);
					AppendResult("Verification State = "		+ _currentTransaction.VerificationState);
					AppendResult("Transaction Date[UTC] = "		+ _currentTransaction.TransactionDateUTC);
					AppendResult("Transaction Date[Local] = "	+ _currentTransaction.TransactionDateLocal);
					AppendResult("Transaction Identifier = "	+ _currentTransaction.TransactionIdentifier);
					AppendResult("Transaction Receipt = "		+ _currentTransaction.TransactionReceipt);
					AppendResult("Error = "						+ _currentTransaction.Error.GetPrintableString());
				}
			}
		}
		
		#endregion
	
		#region Misc. Methods
		
		private BillingProduct GetCurrentProduct ()
		{
			return m_products[m_productIter];
		}
		
		private void GotoNextProduct ()
		{
			m_productIter++;
			
			if (m_productIter >= m_products.Length)
				m_productIter	= 0;
		}
		
		private void GotoPreviousProduct ()
		{
			m_productIter--;
			
			if (m_productIter < 0)
				m_productIter	= m_products.Length - 1;
		}
		
		private int GetProductsCount ()
		{
			return m_products.Length;
		}
		
		#endregion
	}
#endif
}
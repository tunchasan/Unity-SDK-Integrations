using UnityEngine;
using System.Collections;

#if USES_BILLING
using System.Collections.Generic;

namespace VoxelBusters.NativePlugins
{
	public abstract class AbstractBillingListener : MonoBehaviour 
	{
		#region Unity Methods

		protected virtual void OnEnable ()
		{
			// Register for events
			Billing.DidFinishRequestForBillingProductsEvent += OnDidFinishRequestForBillingProducts;
			Billing.DidFinishProductPurchaseEvent			+= OnDidFinishProductPurchase;
			Billing.DidFinishRestoringPurchasesEvent 		+= OnDidFinishRestoringPurchases;
		}

		protected virtual void OnDisable ()
		{
			// Unregister from events
			Billing.DidFinishRequestForBillingProductsEvent -= OnDidFinishRequestForBillingProducts;
			Billing.DidFinishProductPurchaseEvent			-= OnDidFinishProductPurchase;
			Billing.DidFinishRestoringPurchasesEvent 		-= OnDidFinishRestoringPurchases;
		}

		#endregion

		#region Abstract Methods

		public abstract void OnProductsRetrieved (BillingProduct[] _products);

		public abstract void OnProductsRetrieveFailed (string _error);

		public abstract void OnProductPurchased (BillingTransaction _transaction);

		public abstract void OnProductPurchaseFailed (BillingTransaction _transaction);

		public abstract void OnProductsRestored (BillingTransaction[] _transaction);
		
		public abstract void OnProductsRestoreFailed (string _error);

		#endregion

		#region Products Callback Methods

		private void OnDidFinishRequestForBillingProducts (BillingProduct[] _products, string _error)
		{
			if (_error == null)
				OnProductsRetrieved(_products);
			else
				OnProductsRetrieveFailed(_error);
		}

		#endregion

		#region Transaction Callback Methods

		private void OnDidFinishProductPurchase (BillingTransaction _transaction)
		{
			if (_transaction.VerificationState == eBillingTransactionVerificationState.FAILED)
			{
				OnProductPurchaseFailed(_transaction);
			}
			else
			{
				if (_transaction.TransactionState == eBillingTransactionState.PURCHASED)
					OnProductPurchased(_transaction);
				else
					OnProductPurchaseFailed(_transaction);
			}
		}

		private void OnDidFinishRestoringPurchases (BillingTransaction[] _transactions, string _error)
		{
			if (_error == null)
			{
				int		_count	= _transactions.Length;

				if (_count == 0)
				{
					OnProductsRestored(_transactions);

					return;
				}

				// Filter transaction based on receipt verification status
				List<BillingTransaction>	_restoreSuccessList	= new List<BillingTransaction>(_transactions.Length);

				foreach (BillingTransaction _currentTransaction in _transactions)
				{
					if (_currentTransaction.VerificationState == eBillingTransactionVerificationState.FAILED)
					{
						string	_restoreErrorDescription	= string.Format("The product with identifier: {0} could not be restored because transaction receipt verification failed.", _currentTransaction.ProductIdentifier);
						
						OnProductsRestoreFailed(_restoreErrorDescription);
					}
					else
					{
						_restoreSuccessList.Add(_currentTransaction);
					}
				}

				if (_restoreSuccessList.Count > 0)
					OnProductsRestored(_restoreSuccessList.ToArray());
			}
			else
			{
				OnProductsRestoreFailed(_error);
			}
		}

		#endregion
	}
}
#endif
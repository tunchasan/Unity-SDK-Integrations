using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if USES_BILLING && UNITY_IOS
using VoxelBusters.Utility;

namespace VoxelBusters.NativePlugins
{	
	using Internal;

	public partial class BillingIOS : Billing
	{
		#region Constants

		private		const 	string		kProductsKey		= "products";
		private		const 	string		kTransactionsKey	= "transactions";
		private 	const 	string		kErrorKey			= "error";

		#endregion

		#region Callback Methods

		protected override void DidReceiveBillingProducts (string _dataStr)
		{
			IDictionary		_dataDict	= (IDictionary)JSONUtility.FromJSON(_dataStr);
			string			_error		= _dataDict.GetIfAvailable<string>(kErrorKey);
			
			if (_error != null)
			{
				DidReceiveBillingProducts(null, _error);
				return;
			}
			else
			{
				IList				_regProductsJSONList	= _dataDict.GetIfAvailable<IList>(kProductsKey);
				BillingProduct[]	_regProductsList		= null;

				if (_regProductsJSONList != null)
				{
					_regProductsList	= new BillingProductIOS[_regProductsJSONList.Count];
					int		_iter		= 0;
					
					foreach (IDictionary _productInfoDict in _regProductsJSONList)
					{
						_regProductsList[_iter++]			= new BillingProductIOS(_productInfoDict);
					}
				}
				
				DidReceiveBillingProducts(_regProductsList, null);
				return;
			}
		}
		
		protected override void ExtractTransactionResponseData (string _dataStr, out BillingTransaction[] _transactions, out string _error)
		{
			// Set default values
			_transactions	= null;
			_error			= null;

			// Parse and fetch properties from JSON object
			IDictionary		_dataDict	= (IDictionary)JSONUtility.FromJSON(_dataStr);
			_error						= _dataDict.GetIfAvailable<string>(kErrorKey);

			if (_error == null)
			{
				IList		_transactionsJSONList	= _dataDict.GetIfAvailable<IList>(kTransactionsKey);

				if (_transactionsJSONList != null)
				{
					int		_count		= _transactionsJSONList.Count;
					_transactions		= new BillingTransactionIOS[_count];

					for (int _iter = 0; _iter < _count; _iter++)
						_transactions[_iter]	= new BillingTransactionIOS((IDictionary)_transactionsJSONList[_iter]);
				}
			}
		}

		protected override void ProcessPurchaseTransactions (BillingTransaction[] _transactions)
		{
			if (_transactions == null)
				return;
			
			IList _transactionIDs	= GetFinishedTransactionIdentifiers(_transactions);
			if (_transactionIDs.Count > 0)
				cpnpBillingFinishCompletedTransactions(_transactionIDs.ToJSON(), false);
		}

		protected override void ProcessRestoredTransactions (BillingTransaction[] _transactions)
		{
			if (_transactions == null)
				return;
			
			IList _transactionIDs	= GetFinishedTransactionIdentifiers(_transactions);
			if (_transactionIDs.Count > 0)
				cpnpBillingFinishCompletedTransactions(_transactionIDs.ToJSON(), true);
		}

		private IList GetFinishedTransactionIdentifiers (BillingTransaction[] _transactions)
		{
			int				_count			= _transactions.Length;
			
			List<string> 	_transactionIDs	= new List<string>(_count);
			for (int _iter = 0; _iter < _count; _iter++)
			{
				BillingTransaction _transaction	= _transactions[_iter];

				if (string.IsNullOrEmpty(_transaction.TransactionIdentifier))
					continue;

				if (_transaction.VerificationState == eBillingTransactionVerificationState.SUCCESS)
					_transactionIDs.Add(_transaction.TransactionIdentifier);
			}

			return _transactionIDs;
		}

		#endregion
	}
}
#endif
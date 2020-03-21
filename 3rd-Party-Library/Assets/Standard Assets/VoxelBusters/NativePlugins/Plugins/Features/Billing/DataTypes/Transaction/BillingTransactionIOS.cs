using UnityEngine;
using System.Collections;

#if USES_BILLING && UNITY_IOS
using System.Collections.Generic;
using System.Runtime.InteropServices;
using VoxelBusters.Utility;

namespace VoxelBusters.NativePlugins.Internal
{
	public sealed class BillingTransactionIOS : BillingTransaction
	{
		private enum SKPaymentTransactionState 
		{
			SKPaymentTransactionStatePurchasing,    // Transaction is being added to the server queue.
			SKPaymentTransactionStatePurchased,     // Transaction is in queue, user has been charged.  Client should complete the transaction.
			SKPaymentTransactionStateFailed,        // Transaction was cancelled or failed before being added to the server queue.
			SKPaymentTransactionStateRestored,      // Transaction was restored from user's purchase history.  Client should complete the transaction.
			SKPaymentTransactionStateDeferred		// The transaction is in the queue, but its final status is pending external action.
		}

//		Success:
//		{
//			"transaction-date": "2015-02-24 06:51:20 +0530",
//			"verification-state": 1,
//			"transaction-identifier": "transactionIdentifier",
//			"transaction-receipt": "receipt",
//			"transaction-state": 1,
//			"product-identifier": "54units",
//			"error": null
//		}

//		FAILED: 
//		{
//			"transaction-date": "2015-02-24 06:48:00 +0530",
//			"verification-state": 1,
//			"transaction-identifier": "transactionIdentifier",
//			"transaction-receipt": "receipt",
//			"transaction-state": 2,
//			"product-identifier": "54units",
//			"error": "Purchase was cancelled by user"
//		}

		#region Constants

		private const string kProductID				= "product-identifier";
		private const string kTransactionDate		= "transaction-date";
		private const string kTransactionID			= "transaction-identifier";
		private const string kTransactionReceipt	= "transaction-receipt";
		private const string kTransactionState		= "transaction-state";
		private const string kVerificationState		= "verification-state";
		private const string kError					= "error";

		#endregion

		#region Constructor

		public BillingTransactionIOS (IDictionary _transactionJsonDict)
		{
			ProductIdentifier		= _transactionJsonDict.GetIfAvailable<string>(kProductID);

			// Transaction date can be NULL/Empty
			string 	_tDateStr		= _transactionJsonDict.GetIfAvailable<string>(kTransactionDate);

			if (!string.IsNullOrEmpty(_tDateStr))
			{
				TransactionDateUTC		= _tDateStr.ToZuluFormatDateTimeUTC();
				TransactionDateLocal	= TransactionDateUTC.ToLocalTime();
			}

			// Transaction identifier
			TransactionIdentifier	= _transactionJsonDict.GetIfAvailable<string>(kTransactionID);

			// Transaction receipt can be NULL/Empty
			TransactionReceipt		= _transactionJsonDict.GetIfAvailable<string>(kTransactionReceipt);

			// Transactions state
			SKPaymentTransactionState _skTransactionState	= (SKPaymentTransactionState)int.Parse(_transactionJsonDict[kTransactionState].ToString());
			TransactionState		= ConvertToBillingTransactionState(_skTransactionState);

			// Verifications state
			VerificationState		= (eBillingTransactionVerificationState)int.Parse(_transactionJsonDict[kVerificationState].ToString());

			// Error can be NULL/Empty
			Error					= _transactionJsonDict.GetIfAvailable<string>(kError);		

			// Set response which is sent from Native side
			RawPurchaseData			= _transactionJsonDict.ToJSON();
		}

		#endregion

		#region Static Methods

		public static IDictionary CreateJSONObject (BillingTransaction _transaction)
		{
			SKPaymentTransactionState _skTransactionState	= ConvertToSKTransactionState(_transaction.TransactionState);

			IDictionary _transactionJsonDict			= new Dictionary<string, object>();
			_transactionJsonDict[kTransactionDate]		= _transaction.TransactionDateUTC.ToStringUsingZuluFormat();
			_transactionJsonDict[kVerificationState]	= (int)_transaction.VerificationState;
			_transactionJsonDict[kTransactionID]		= _transaction.TransactionIdentifier;
			_transactionJsonDict[kTransactionReceipt]	= _transaction.TransactionReceipt;
			_transactionJsonDict[kTransactionState]		= (int)_skTransactionState;
			_transactionJsonDict[kProductID]			= _transaction.ProductIdentifier;
			_transactionJsonDict[kError]				= _transaction.Error;

			return _transactionJsonDict;
		}

		private static eBillingTransactionState ConvertToBillingTransactionState (SKPaymentTransactionState _skTransactionState)
		{
			switch (_skTransactionState)
			{
			case SKPaymentTransactionState.SKPaymentTransactionStatePurchased:
				return eBillingTransactionState.PURCHASED;
				
			case SKPaymentTransactionState.SKPaymentTransactionStateFailed:
				return eBillingTransactionState.FAILED;

			case SKPaymentTransactionState.SKPaymentTransactionStateRestored:
				return eBillingTransactionState.RESTORED;
			}

			return eBillingTransactionState.FAILED;
		}

		private static SKPaymentTransactionState ConvertToSKTransactionState (eBillingTransactionState _billingTransactionState)
		{
			switch (_billingTransactionState)
			{
			case eBillingTransactionState.PURCHASED:
				return SKPaymentTransactionState.SKPaymentTransactionStatePurchased;
				
			case eBillingTransactionState.FAILED:
				return SKPaymentTransactionState.SKPaymentTransactionStateFailed;
				
			case eBillingTransactionState.RESTORED:
				return SKPaymentTransactionState.SKPaymentTransactionStateRestored;
			}
			
			return SKPaymentTransactionState.SKPaymentTransactionStateFailed;
		}

		#endregion

		#region External Methods

		[DllImport("__Internal")]
		private static extern void cpnpBillingCustomReceiptVerificationFinished (string _transactionID, int _transactionState, int _verificationState);

		#endregion

		#region Methods

		public override void OnCustomVerificationFinished (eBillingTransactionVerificationState _newState)
		{
			base.OnCustomVerificationFinished(_newState);

			// Native call
			cpnpBillingCustomReceiptVerificationFinished(TransactionIdentifier, (int)TransactionState, (int)VerificationState);
		}

		#endregion
	}
}
#endif
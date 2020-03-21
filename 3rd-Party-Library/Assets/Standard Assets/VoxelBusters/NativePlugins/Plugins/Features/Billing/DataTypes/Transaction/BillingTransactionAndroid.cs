using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using VoxelBusters.Utility;
using VoxelBusters.UASUtils;

namespace VoxelBusters.NativePlugins.Internal
{
	public sealed class BillingTransactionAndroid : BillingTransaction
	{
		#region Constants

		private const string	kProductIdentifier				= "product-identifier";
		private const string	kTransactionDate				= "transaction-date";
		private const string	kTransactionIdentifier			= "transaction-identifier";
		private const string	kTransactionReceipt				= "transaction-receipt";
		private const string	kTransactionState				= "transaction-state";
		private const string	kVerificationState				= "verification-state";
		private const string	kError							= "error";
		private const string	kRawPurchaseData				= "raw-purchase-data";
		

		// Validation values
		private const string 	kNoValidationDone				= "no-validation-done";
		private const string 	kValidationSuccess				= "success";
		private const string 	kValidationFailed				= "failed";

		// Purchase state values
		private const string 	kPurchaseFailed					= "failed";
		private const string 	kPurchaseSuccess				= "purchased";
		private const string 	kPurchaseRefunded				= "refunded";
		private const string 	kPurchaseRestored				= "restored";

		#endregion

		#region Constructor
		
		public BillingTransactionAndroid (IDictionary _transactionInfo)
		{
			// Get Product Identifier
			ProductIdentifier				= _transactionInfo.GetIfAvailable<string>(kProductIdentifier);
			
			// Transaction time
			long _purchaseTimeInMillis		= _transactionInfo.GetIfAvailable<long>(kTransactionDate);
			System.DateTime _purchaseDate 	= _purchaseTimeInMillis.ToDateTimeFromJavaTime();
			TransactionDateUTC				= _purchaseDate.ToUniversalTime();
			TransactionDateLocal			= _purchaseDate.ToLocalTime();
			
			// Transaction ID
			TransactionIdentifier			= _transactionInfo.GetIfAvailable<string>(kTransactionIdentifier);

			// Transaction Receipt
			TransactionReceipt				= _transactionInfo.GetIfAvailable<string>(kTransactionReceipt);
			
			// Transaction State
			string _transactionState		= _transactionInfo.GetIfAvailable<string>(kTransactionState);
			TransactionState				= GetTransactionState(_transactionState);
		
			// Verification State
			string _validationState 		= _transactionInfo.GetIfAvailable<string>(kVerificationState);
			VerificationState 				= GetValidationState(_validationState);

			// Error
			Error							= _transactionInfo.GetIfAvailable<string>(kError);					

			// Raw Purchase data in JSON format
			RawPurchaseData					= _transactionInfo.GetIfAvailable<string>(kRawPurchaseData);
		}

		#endregion

		#region Static Methods
		
		public static IDictionary CreateJSONObject (BillingTransaction _transaction)
		{
			IDictionary _transactionJsonDict						= new Dictionary<string, object>();
			
			_transactionJsonDict[kProductIdentifier] 				= _transaction.ProductIdentifier;
			_transactionJsonDict[kTransactionDate]					= _transaction.TransactionDateUTC.ToJavaTimeFromDateTime();
			_transactionJsonDict[kTransactionIdentifier]			= _transaction.TransactionIdentifier;
			_transactionJsonDict[kTransactionReceipt]				= _transaction.TransactionReceipt;
			_transactionJsonDict[kTransactionState]					= GetTransactionState(_transaction.TransactionState);
			_transactionJsonDict[kVerificationState]				= GetValidationState(_transaction.VerificationState);
			_transactionJsonDict[kError]							= _transaction.Error;
			_transactionJsonDict[kRawPurchaseData]					= _transaction.RawPurchaseData;

			return _transactionJsonDict;
		}

		private static eBillingTransactionVerificationState GetValidationState(string _validationState)
		{
			eBillingTransactionVerificationState _state;

			if(_validationState.Equals(kValidationFailed))
			{
				//This transaction validation failed
				_state	= eBillingTransactionVerificationState.FAILED;
				
			}
			else if(_validationState.Equals(kValidationSuccess))
			{
				//This transaction validation success
				_state	= eBillingTransactionVerificationState.SUCCESS;
			}
			else if(_validationState.Equals(kNoValidationDone))
			{
				_state	= eBillingTransactionVerificationState.NOT_CHECKED;
			}
			else
			{
				DebugUtility.Logger.LogError(Constants.kDebugTag, "[BillingTransaction] Invalid state " + _validationState);
				_state	= eBillingTransactionVerificationState.FAILED;
			}
			
			return _state;
		}

		private static string GetValidationState(eBillingTransactionVerificationState _state)
		{
			string _validationState;

			if(_state == eBillingTransactionVerificationState.FAILED)
			{
				//This transaction validation failed
				_validationState	= kValidationFailed;
				
			}
			else if(_state == eBillingTransactionVerificationState.SUCCESS)
			{
				_validationState	= kValidationSuccess;
				
			}
			else if(_state == eBillingTransactionVerificationState.NOT_CHECKED)
			{
				_validationState	= kNoValidationDone;
			}
			else
			{
				DebugUtility.Logger.LogError(Constants.kDebugTag, "[BillingTransaction] Invalid state " + _state);
				_validationState	= kValidationFailed;
			}
			
			return _validationState;
		}

		/*
	 	* The purchase state of the order.
	 	*/
		private static eBillingTransactionState GetTransactionState(string _transactionState)
		{
			eBillingTransactionState  _state = eBillingTransactionState.FAILED;

			if(_transactionState.Equals(kPurchaseFailed))
			{
				_state = eBillingTransactionState.FAILED;
			}
			else if (_transactionState.Equals(kPurchaseSuccess))
			{
				_state = eBillingTransactionState.PURCHASED;
			}
			else if (_transactionState.Equals(kPurchaseRefunded))
			{
				_state = eBillingTransactionState.REFUNDED;
			}
			else if (_transactionState.Equals(kPurchaseRestored))
			{
				_state = eBillingTransactionState.RESTORED;
			}
			return _state;
		}

		private static string GetTransactionState(eBillingTransactionState _state)
		{
			string  _transactionState = kPurchaseFailed;
			
			if (_state == eBillingTransactionState.FAILED)
			{
				_transactionState = kPurchaseFailed;
			}
			else if(_state == eBillingTransactionState.PURCHASED)
			{
				_transactionState = kPurchaseSuccess;
			}
			else if(_state == eBillingTransactionState.REFUNDED)
			{
				_transactionState = kPurchaseRefunded;
			}
			else if(_state == eBillingTransactionState.RESTORED)
			{
				_transactionState = kPurchaseRestored;
			}
	
			return _transactionState;
		}
		
		#endregion

		#region Methods
		
		public override void OnCustomVerificationFinished (eBillingTransactionVerificationState _newState)
		{
			base.OnCustomVerificationFinished(_newState);

			// Nothing to do here. Not supporting external validation for android. //TODO - This needs original payload to verify
			DebugUtility.Logger.Log(Constants.kDebugTag, "[Billing] On Android, all the transactions are validated implicitely, so this call has no effect.");
		}

		#endregion
	}
}
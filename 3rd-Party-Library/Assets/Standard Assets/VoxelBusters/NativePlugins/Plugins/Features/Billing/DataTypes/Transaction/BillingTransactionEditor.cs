using UnityEngine;
using System.Collections;
using VoxelBusters.Utility;

#if USES_BILLING && UNITY_EDITOR
namespace VoxelBusters.NativePlugins.Internal
{
	public sealed class BillingTransactionEditor : BillingTransaction 
	{
		#region Constructor
		
		public BillingTransactionEditor (string _productID, System.DateTime _timeUTC, string _transactionID, string _receipt, eBillingTransactionState _transactionState, eBillingTransactionVerificationState _verificationState, string _error)
		{
			ProductIdentifier		= _productID;
			TransactionDateUTC		= _timeUTC;
			TransactionDateLocal	= TransactionDateUTC.ToLocalTime();
			TransactionIdentifier	= _transactionID;
			TransactionReceipt		= _receipt;
			TransactionState		= _transactionState;
			VerificationState		= _verificationState;
			Error					= _error;
			
			// Create raw data
			RawPurchaseData			= ToJSONObject().ToJSON();
		}
		
		#endregion
		
		#region Methods
		
		private IDictionary ToJSONObject ()
		{
#if   UNITY_ANDROID
			return BillingTransactionAndroid.CreateJSONObject(this);
#elif UNITY_IOS
			return BillingTransactionIOS.CreateJSONObject(this);
#else
			return null;
#endif
		}

		public override void OnCustomVerificationFinished (eBillingTransactionVerificationState _newState)
		{
			base.OnCustomVerificationFinished(_newState);

			EditorStore.CustomVerificationFinished(this);
		}
		
		#endregion
	}
}
#endif

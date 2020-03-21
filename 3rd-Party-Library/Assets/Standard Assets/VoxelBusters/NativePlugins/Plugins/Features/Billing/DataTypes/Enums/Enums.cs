using UnityEngine;
using System.Collections;

namespace VoxelBusters.NativePlugins
{
	/// <summary>
	/// The state of a billing product payment.
	/// </summary>
	public enum eBillingTransactionState
	{
		/// <summary> The Store successfully processed payment. </summary>
		PURCHASED,

		/// <summary> The transaction failed.</summary>
		FAILED,

		/// <summary> This transaction restores content previously purchased by the user.</summary>
		RESTORED,

		/// <summary> This transaction was refunded back to the user. You can restrict/remove associated item.</summary>
		REFUNDED
	}

	/// <summary>
	/// The state of a payment receipt verification. 
	/// </summary>
	public enum eBillingTransactionVerificationState
	{
		/// <summary> Receipt verification has not yet been done.</summary>
		NOT_CHECKED,

		/// <summary> Receipt was successfully verified.</summary>
		SUCCESS,

		/// <summary> Receipt verification failed for some reason. Possible reasons can be network issue, mismatch of app build details etc.</summary>
		FAILED
	}
}
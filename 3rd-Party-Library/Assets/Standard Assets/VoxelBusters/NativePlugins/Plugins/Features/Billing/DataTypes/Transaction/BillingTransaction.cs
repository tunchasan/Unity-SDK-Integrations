using UnityEngine;
using System.Collections;

namespace VoxelBusters.NativePlugins
{
	using Internal;

	/// <summary>
	/// Represents an object that holds transaction information of the purchased product.
	/// </summary>
	public class BillingTransaction 
	{
		#region Properties

		/// <summary>
		/// The string used to identify a product that can be purchased from within your application. (read-only)
		/// </summary>
		public string ProductIdentifier 		
		{ 
			get; 
			protected set; 
		}

		/// <summary>
		/// The UTC date and time, when user initiated this transaction.
		/// </summary>
		public System.DateTime TransactionDateUTC 		
		{ 
			get; 
			protected set; 
		}

		/// <summary>
		/// The local date and time, when user initiated this transaction.
		/// </summary>
		public System.DateTime TransactionDateLocal 	
		{ 
			get; 
			protected set; 
		}
		
		/// <summary>
		/// The string that uniquely identifies a payment transaction. (read-only)
		/// </summary>
		public string TransactionIdentifier 	
		{ 
			get; 
			protected set; 
		}

		/// <summary>
		/// A signed receipt that records all information about a payment transaction. (read-only)
		/// </summary>
		///	<remarks>
		///	\note On iOS platform receipt data alone is enough to validate a transaction. However on Android platform, along with this receipt data (signature), original JSON may be required if you want to use external validation.
		///	</remarks>
		public string TransactionReceipt 		
		{ 
			get; 
			protected set; 
		}

		/// <summary>
		/// The current state of the transaction. (read-only)
		/// </summary>
		public eBillingTransactionState TransactionState 		
		{ 
			get; 
			protected set; 
		}

		/// <summary>
		/// The current state of receipt verification. (read-only)
		/// </summary>
		public eBillingTransactionVerificationState VerificationState		
		{ 
			get; 
			protected set; 
		}

		/// <summary>
		/// An object describing the error that occurred while processing the transaction. (read-only)
		/// </summary>
		public string Error					
		{ 
			get; 
			protected set; 
		}

		/// <summary>
		/// The purchase data in JSON format. (read-only)
		/// </summary>
		public string RawPurchaseData
		{
			get; 
			protected set; 
		}

		#endregion

		#region Constructor

		protected BillingTransaction ()
		{}

		internal BillingTransaction (string _error)
		{
			if (_error == null)
				throw new System.ArgumentNullException("Error is null.");

			// Set properties
			this.ProductIdentifier		= null;
			this.TransactionDateUTC		= System.DateTime.UtcNow;
			this.TransactionDateLocal	= System.DateTime.Now;
			this.TransactionIdentifier	= null;
			this.TransactionReceipt		= null;
			this.TransactionState		= eBillingTransactionState.FAILED;
			this.VerificationState		= eBillingTransactionVerificationState.FAILED;
			this.Error					= _error;
			this.RawPurchaseData		= null;
		}

		#endregion

		#region Methods

		/// <summary>
		/// Call this method to update state, after custom receipt verfication is completed.
		/// </summary>
		/// <param name="_newState">The new state for this transaction.</param>
		/// <description>
		/// Note that, calling this method will update product purchase inventory and followed by resends transaction event.
		/// </description>
		public virtual void OnCustomVerificationFinished (eBillingTransactionVerificationState _newState)
		{
			this.VerificationState	= _newState;
		}

		public override string ToString ()
		{
			return string.Format("[BillingTransaction: ProductIdentifier={0}, TransactionDateUTC={1}, TransactionIdentifier={2}, TransactionState={3}, VerificationState={4}, Error={5}]", 
			                     ProductIdentifier, TransactionDateUTC, TransactionIdentifier, TransactionState, VerificationState, Error);
		}
	
		#endregion
	}
}
using UnityEngine;

#if USES_BILLING && UNITY_ANDROID
namespace VoxelBusters.NativePlugins
{
	using Internal;
	
	public partial class BillingAndroid : Billing 
	{
		#region Platform Native Info
		
		private class Native
		{
			// Handler class name
			internal class Class
			{
				internal const string NAME								= "com.voxelbusters.nativeplugins.features.billing.BillingHandler";
			}
			
			// For holding method names
			internal class Methods
			{
				internal const string INITIALIZE		 				= "initialize";
				internal const string IS_INITIALIZED					= "isInitialized";
				internal const string REQUEST_BILLING_PRODUCTS 			= "requestBillingProducts"; 
				internal const string IS_PRODUCT_PURCHASED				= "isProductPurchased";
				internal const string BUY_PRODUCT						= "buyProduct";
				internal const string CUSTOM_VERIFICATION_FINISHED		= "customVerificationFinished";
				internal const string RESTORE_COMPLETED_TRANSACTIONS 	= "restoreCompletedTransactions";
			}
		}
		
		#endregion
		
		#region  Native Access Variables
		
		private AndroidJavaObject  	Plugin
		{
			get; 
			set;
		}

		#endregion
	}
}
#endif
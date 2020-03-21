#if USES_SOOMLA_GROW
using UnityEngine;
using System.Collections;
using VoxelBusters.UASUtils;

namespace VoxelBusters.NativePlugins
{
	using Internal;

	public partial class SoomlaGrowService : MonoBehaviour 
	{
		#region Constants

		private		const	string	kReferrerName	= "voxelbusters";

		#endregion

		#region Unity Methods

		private void Awake ()
		{
			SoomlaGrowServiceSettings	_settings	= NPSettings.AddonServicesSettings.SoomlaGrowService;

			// Initialise component
			Initialise(_settings.GameKey, _settings.EnvironmentKey, kReferrerName);
		}

		private void OnEnable ()
		{
#if USES_BILLING
			// Register for billing events
			Billing.DidFinishProductPurchaseEvent		+= OnDidFinishProductPurchase;
			Billing.DidFinishRestoringPurchasesEvent	+= OnDidFinishRestoringPurchases;
#endif
		}

		private void OnDisable ()
		{
#if USES_BILLING
			// Unregister from billing events
			Billing.DidFinishProductPurchaseEvent		-= OnDidFinishProductPurchase;
			Billing.DidFinishRestoringPurchasesEvent	-= OnDidFinishRestoringPurchases;
#endif
		}

		#endregion

		#region Methods

		protected virtual void Initialise (string _gameKey, string _environmentKey, string _referrerName)
		{
			DebugUtility.Logger.Log(Constants.kDebugTag, "[SoomlaGrow] Initialising SDK.");
		}

		#endregion

		#region Billing Methods

		internal void ReportOnBillingSupported (bool _isSupported)
		{
			if (_isSupported)
				ReportOnBillingSupported();
			else
				ReportOnBillingNotSupported();
		}

		protected virtual void ReportOnBillingSupported ()
		{
			DebugUtility.Logger.Log(Constants.kDebugTag, "[SoomlaGrow] Reporting: OnBillingSupported.");
		}

		protected virtual void ReportOnBillingNotSupported ()
		{
			DebugUtility.Logger.Log(Constants.kDebugTag, "[SoomlaGrow] Reporting: OnBillingNotSupported.");
		}
		
		internal virtual void ReportOnBillingPurchaseStarted (string _productID)
		{
			DebugUtility.Logger.Log(Constants.kDebugTag, "[SoomlaGrow] Reporting: OnBillingPurchaseStarted.");
		}

		internal virtual void ReportOnBillingPurchaseFinished (string _productID, long _priceInMicros, string _currencyCode)
		{			
			DebugUtility.Logger.Log(Constants.kDebugTag, "[SoomlaGrow] Reporting: OnBillingPurchaseFinished.");
		}

		internal virtual void ReportOnBillingPurchaseCancelled (string _productID)
		{
			DebugUtility.Logger.Log(Constants.kDebugTag, "[SoomlaGrow] Reporting: OnBillingPurchaseCancelled.");
		} 

		internal virtual void ReportOnBillingPurchaseFailed (string _productID)
		{		
			DebugUtility.Logger.Log(Constants.kDebugTag, "[SoomlaGrow] Reporting: OnBillingPurchaseFailed.");
		}

		internal virtual void ReportOnBillingPurchasesRestoreStarted ()
		{
			DebugUtility.Logger.Log(Constants.kDebugTag, "[SoomlaGrow] Reporting: OnBillingRestoreStarted.");
		}

		internal virtual void ReportOnBillingPurchasesRestoreFinished (bool _success)
		{			
			DebugUtility.Logger.Log(Constants.kDebugTag, "[SoomlaGrow] Reporting: OnBillingRestoreFinished.");
		}

		internal virtual void ReportOnBillingPurchaseVerificationFailed ()
		{			
			DebugUtility.Logger.Log(Constants.kDebugTag, "[SoomlaGrow] Reporting: OnBillingVerificationFailed.");
		}
	
		#endregion

		#region Social Feature Methods

		internal virtual void ReportOnSocialLoginStarted (eSocialProvider _provider)
		{		
			DebugUtility.Logger.Log(Constants.kDebugTag, "[SoomlaGrow] Reporting: OnSocialLoginStarted.");
		}

		internal virtual void ReportOnSocialLoginFinished (eSocialProvider _provider, string _userID)
		{		
			DebugUtility.Logger.Log(Constants.kDebugTag, "[SoomlaGrow] Reporting: OnSocialLoginFinished.");
		}

		internal virtual void ReportOnSocialLoginCancelled (eSocialProvider _provider)
		{
			DebugUtility.Logger.Log(Constants.kDebugTag, "[SoomlaGrow] Reporting: OnSocialLoginCancelled.");
		}

		internal virtual void ReportOnSocialLoginFailed (eSocialProvider _provider)
		{			
			DebugUtility.Logger.Log(Constants.kDebugTag, "[SoomlaGrow] Reporting: OnSocialLoginFailed.");
		}

		internal virtual void ReportOnSocialLogoutStarted (eSocialProvider _provider)
		{		
			DebugUtility.Logger.Log(Constants.kDebugTag, "[SoomlaGrow] Reporting: OnSocialLogoutStarted.");
		}

		internal virtual void ReportOnSocialLogoutFinished (eSocialProvider _provider)
		{			
			DebugUtility.Logger.Log(Constants.kDebugTag, "[SoomlaGrow] Reporting: OnSocialLogoutFinished.");
		}

		internal virtual void ReportOnSocialLogoutFailed (eSocialProvider _provider)
		{	
			DebugUtility.Logger.Log(Constants.kDebugTag, "[SoomlaGrow] Reporting: OnSocialLogoutFailed.");
		}

		internal virtual void ReportOnGetContactsStartedForProvider (eSocialProvider _provider)
		{	
			DebugUtility.Logger.Log(Constants.kDebugTag, "[SoomlaGrow] Reporting: OnGetContactsStartedForProvider.");
		}
		
		internal virtual void ReportOnGetContactsFinishedForProvider (eSocialProvider _provider)
		{
			DebugUtility.Logger.Log(Constants.kDebugTag, "[SoomlaGrow] Reporting: OnGetContactsFinishedForProvider.");
		}
		
		internal virtual void ReportOnGetContactsFailedForProvider (eSocialProvider _provider)
		{
			DebugUtility.Logger.Log(Constants.kDebugTag, "[SoomlaGrow] Reporting: OnGetContactsFailedForProvider.");
		}
		
		internal virtual void ReportOnSocialActionStarted (eSocialActionType _actionType, eSocialProvider _provider)
		{
			DebugUtility.Logger.Log(Constants.kDebugTag, "[SoomlaGrow] Reporting: OnSocialActionStarted.");
		}
		
		internal virtual void ReportOnSocialActionFinished (eSocialActionType _actionType, eSocialProvider _provider)
		{
			DebugUtility.Logger.Log(Constants.kDebugTag, "[SoomlaGrow] Reporting: OnSocialActionFinished.");
		}
		
		internal virtual void ReportOnSocialActionCancelled (eSocialActionType _actionType, eSocialProvider _provider)
		{
			DebugUtility.Logger.Log(Constants.kDebugTag, "[SoomlaGrow] Reporting: OnSocialActionCancelled.");
		}
		
		internal virtual void ReportOnSocialActionFailed (eSocialActionType _actionType, eSocialProvider _provider)
		{			
			DebugUtility.Logger.Log(Constants.kDebugTag, "[SoomlaGrow] Reporting: OnSocialActionFailed.");
		}

		#endregion

		#region Game Services Methods

		internal virtual void ReportOnLatestScore (string _scoreID, double _latestScore)
		{
			DebugUtility.Logger.Log(Constants.kDebugTag, "[SoomlaGrow] Reporting: OnLatestScore.");
		}

		#endregion

		#region Misc. Methods
	
		internal virtual void ReportOnUserRating ()
		{
			DebugUtility.Logger.Log(Constants.kDebugTag, "[SoomlaGrow] Reporting: OnUserRating.");
		}

		#endregion

		#region Callback Methods

#if USES_BILLING
		private void OnDidFinishProductPurchase (BillingTransaction _transaction)
		{
			string	_productID	= _transaction.ProductIdentifier;

			// Based on receipt verification, report event
			if (_transaction.VerificationState == eBillingTransactionVerificationState.SUCCESS)
			{
				if (_transaction.TransactionState == eBillingTransactionState.PURCHASED)
				{
					BillingProduct 	_productInfo	= NPBinding.Billing.GetStoreProduct(_productID);
					
					if (_productInfo == null)
					{
						DebugUtility.Logger.Log(Constants.kDebugTag, "[SoomlaGrow] The operation could not be completed because product information is not available.");
					}
					else
					{
						ReportOnBillingPurchaseFinished(_productID, (long)(_productInfo.Price * 1000000), _productInfo.CurrencyCode);
					}
				}
				else if (_transaction.TransactionState == eBillingTransactionState.FAILED)
				{
					if (_productID == null)
					{
						DebugUtility.Logger.Log(Constants.kDebugTag, "[SoomlaGrow] The operation could not be completed because product identifier information is not available.");
					}
					else
					{
						ReportOnBillingPurchaseFailed(_productID);
					}
				}
			}
			else if (_transaction.VerificationState == eBillingTransactionVerificationState.FAILED)
			{
				ReportOnBillingPurchaseVerificationFailed();

				return;
			}
		}

		private void OnDidFinishRestoringPurchases (BillingTransaction[] _transactions, string _error)
		{
			ReportOnBillingPurchasesRestoreFinished(_error == null);
		}
#endif

		#endregion
	}
}
#endif
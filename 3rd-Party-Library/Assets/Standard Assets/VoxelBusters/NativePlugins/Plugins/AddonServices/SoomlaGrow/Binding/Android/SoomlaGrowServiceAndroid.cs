#if USES_SOOMLA_GROW && UNITY_ANDROID
using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

namespace VoxelBusters.NativePlugins
{
	using Internal;

	public partial class SoomlaGrowServiceAndroid : SoomlaGrowService
	{
		#region Constructors
		
		SoomlaGrowServiceAndroid ()
		{
			Plugin = AndroidPluginUtility.GetSingletonInstance(Native.Class.NAME);
		}
		
		#endregion

		#region Methods

		protected override void Initialise (string _gameKey, string _environmentKey, string _referrerName)
		{
			base.Initialise(_gameKey, _environmentKey, _referrerName);

			// Native call
			Plugin.Call(Native.Methods.INITIALISE, _gameKey, _environmentKey, _referrerName);
		}

		#endregion

		#region Billing Methods

		protected override void ReportOnBillingSupported ()
		{
			base.ReportOnBillingSupported();

			// Native call
			Plugin.Call(Native.Methods.ON_BILLING_SUPPORTED);
		}

		protected override void ReportOnBillingNotSupported ()
		{
			base.ReportOnBillingNotSupported();

			// Native call
			Plugin.Call(Native.Methods.ON_BILLING_NOT_SUPPORTED);
		}

		internal override void ReportOnBillingPurchaseStarted (string _productID)
		{
			base.ReportOnBillingPurchaseStarted(_productID);

			// Native call
			Plugin.Call(Native.Methods.ON_MARKET_PURCHASE_STARTED, _productID);
		}

		internal override void ReportOnBillingPurchaseFinished (string _productID, long _priceInMicros, string _currencyCode)
		{
			base.ReportOnBillingPurchaseFinished(_productID, _priceInMicros, _currencyCode);

			// Native call
			Plugin.Call(Native.Methods.ON_MARKET_PURCHASE_FINISHED, _productID, _priceInMicros, _currencyCode);
		}

		internal override void ReportOnBillingPurchaseCancelled (string _productID)
		{
			base.ReportOnBillingPurchaseCancelled(_productID);

			// Native call
			Plugin.Call(Native.Methods.ON_MARKET_PURCHASE_CANCELLED, _productID);
		}

		internal override void ReportOnBillingPurchaseFailed (string _productID)
		{
			base.ReportOnBillingPurchaseFailed(_productID);

			// Native call
			Plugin.Call(Native.Methods.ON_MARKET_PURCHASE_FAILED, _productID);
		}

		internal override void ReportOnBillingPurchasesRestoreStarted ()
		{
			base.ReportOnBillingPurchasesRestoreStarted();

			// Native call
			Plugin.Call(Native.Methods.ON_RESTORE_TRANSACTIONS_STARTED);
		}

		internal override void ReportOnBillingPurchasesRestoreFinished (bool _success)
		{
			base.ReportOnBillingPurchasesRestoreFinished(_success);

			// Native call
			Plugin.Call(Native.Methods.ON_RESTORE_TRANSACTIONS_FINISHED, _success);
		}

		internal override void ReportOnBillingPurchaseVerificationFailed ()
		{
			base.ReportOnBillingPurchaseVerificationFailed();

			// Native call
			Plugin.Call(Native.Methods.ON_BILLING_PURCHASE_VERIFICATION_FAILED);
		}

		#endregion

		#region Social Methods

		internal override void ReportOnSocialLoginStarted (eSocialProvider _provider)
		{
			base.ReportOnSocialLoginStarted(_provider);

			// Native call
			Plugin.Call(Native.Methods.ON_SOCIAL_LOGIN_STARTED_FOR_PROVIDER, (int)_provider);
		}

		internal override void ReportOnSocialLoginFinished (eSocialProvider _provider, string _userID)
		{
			base.ReportOnSocialLoginFinished(_provider, _userID);

			// Native call
			Plugin.Call(Native.Methods.ON_SOCIAL_LOGIN_FINISHED_FOR_PROVIDER, (int)_provider, _userID);
		}

		internal override void ReportOnSocialLoginCancelled (eSocialProvider _provider)
		{
			base.ReportOnSocialLoginCancelled(_provider);

			// Native call
			Plugin.Call(Native.Methods.ON_SOCIAL_LOGIN_CANCELLED_FOR_PROVIDER, (int)_provider);
		}

		internal override void ReportOnSocialLoginFailed (eSocialProvider _provider)
		{
			base.ReportOnSocialLoginFailed(_provider);

			// Native call
			Plugin.Call(Native.Methods.ON_SOCIAL_LOGIN_FAILED_FOR_PROVIDER, (int)_provider);
		}

		internal override void ReportOnSocialLogoutStarted (eSocialProvider _provider)
		{
			base.ReportOnSocialLogoutStarted(_provider);

			// Native call
			Plugin.Call(Native.Methods.ON_SOCIAL_LOGOUT_STARTED_FOR_PROVIDER, (int)_provider);
		}

		internal override void ReportOnSocialLogoutFinished (eSocialProvider _provider)
		{
			base.ReportOnSocialLogoutFinished(_provider);

			// Native call
			Plugin.Call(Native.Methods.ON_SOCIAL_LOGOUT_FINISHED_FOR_PROVIDER, (int)_provider);
		}

		internal override void ReportOnSocialLogoutFailed (eSocialProvider _provider)
		{
			base.ReportOnSocialLogoutFailed(_provider);

			// Native call
			Plugin.Call(Native.Methods.ON_SOCIAL_LOGOUT_FAILED_FOR_PROVIDER, (int)_provider);
		}
		
		internal override void ReportOnGetContactsStartedForProvider (eSocialProvider _provider)
		{
			base.ReportOnGetContactsStartedForProvider(_provider);
			
			// Native call
			Plugin.Call(Native.Methods.ON_GET_CONTACTS_STARTED_FOR_PROVIDER, (int)_provider);
		}
		
		internal override void ReportOnGetContactsFinishedForProvider (eSocialProvider _provider)
		{
			base.ReportOnGetContactsFinishedForProvider(_provider);

			// Native call
			Plugin.Call(Native.Methods.ON_GET_CONTACTS_FINISHED_FOR_PROVIDER, (int)_provider);
		}
		
		internal override void ReportOnGetContactsFailedForProvider (eSocialProvider _provider)
		{
			base.ReportOnGetContactsFailedForProvider(_provider);
			
			// Native call
			Plugin.Call(Native.Methods.ON_GET_CONTACTS_FAILED_FOR_PROVIDER, (int)_provider);
		}

		internal override void ReportOnSocialActionStarted (eSocialActionType _actionType, eSocialProvider _provider)
		{
			base.ReportOnSocialActionStarted(_actionType, _provider);

			// Native call
			Plugin.Call(Native.Methods.ON_SOCIAL_ACTION_STARTED_FOR_PROVIDER, (int)_provider, (int)_actionType);
		}

		internal override void ReportOnSocialActionFinished (eSocialActionType _actionType, eSocialProvider _provider)
		{
			base.ReportOnSocialActionFinished(_actionType, _provider);

			// Native call
			Plugin.Call(Native.Methods.ON_SOCIAL_ACTION_FINISHED_FOR_PROVIDER, (int)_provider, (int)_actionType);
		}

		internal override void ReportOnSocialActionCancelled (eSocialActionType _actionType, eSocialProvider _provider)
		{
			base.ReportOnSocialActionCancelled(_actionType, _provider);

			// Native call
			Plugin.Call(Native.Methods.ON_SOCIAL_ACTION_CANCELLED_FOR_PROVIDER, (int)_provider, (int)_actionType);
		}

		internal override void ReportOnSocialActionFailed (eSocialActionType _actionType, eSocialProvider _provider)
		{
			base.ReportOnSocialActionFailed(_actionType, _provider);

			// Native call
			Plugin.Call(Native.Methods.ON_SOCIAL_ACTION_FAILED_FOR_PROVIDER, (int)_provider, (int)_actionType);
		}

		#endregion

		#region Game Services Methods

		internal override void ReportOnLatestScore (string _scoreID, double _latestScore)
		{
			base.ReportOnLatestScore(_scoreID, _latestScore);

			// Native call
			Plugin.Call(Native.Methods.ON_REPORT_LATEST_SCORE, _scoreID, _latestScore);
		}

		#endregion
		
		#region Misc Methods
		
		internal override void ReportOnUserRating ()
		{
			base.ReportOnUserRating();
			
			// Native call
			Plugin.Call(Native.Methods.ON_REPORT_USER_RATING);
		}
		
		#endregion
	}
}
#endif
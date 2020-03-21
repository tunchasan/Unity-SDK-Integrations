#if USES_SOOMLA_GROW && UNITY_IOS
using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

namespace VoxelBusters.NativePlugins
{
	using Internal;

	public partial class SoomlaGrowServiceIOS : SoomlaGrowService
	{
		#region External Methods
		
		[DllImport("__Internal")]
		private static extern void grow_initialise (string _gameKey, string _environmentKey, string _referrerName);

		// Billing
		[DllImport("__Internal")]
		private static extern void grow_onBillingNotSupported ();
		
		[DllImport("__Internal")]
		private static extern void grow_onBillingSupported ();
		
		[DllImport("__Internal")]
		private static extern void grow_onMarketPurchaseCancelledForProduct (string _productID);
		
		[DllImport("__Internal")]
		private static extern void grow_onMarketPurchaseFinishedForProduct (string _productID, string _priceMicros, string _currencyCode);
		
		[DllImport("__Internal")]
		private static extern void grow_onMarketPurchaseStartedForProduct (string _productID);
		
		[DllImport("__Internal")]
		private static extern void grow_onMarketPurchaseFailed ();
		
		[DllImport("__Internal")]
		private static extern void grow_onRestoreTransactionsFinishedWithSuccess (bool _success);
		
		[DllImport("__Internal")]
		private static extern void grow_onRestoreTransactionsStarted ();
		
		[DllImport("__Internal")]
		private static extern void grow_onVerificationTimeout ();
		
		[DllImport("__Internal")]
		private static extern void grow_onVerificationFailed ();
		
		[DllImport("__Internal")]
		private static extern void grow_onUnexpectedErrorOccurred ();

		// Social section
		[DllImport("__Internal")]
		private static extern void grow_onLoginCancelledForProvider (int _provider);
		
		[DllImport("__Internal")]
		private static extern void grow_onLoginFailedForProvider (int _provider);
		
		[DllImport("__Internal")]
		private static extern void grow_onLoginFinishedForProvider (int _provider, string _profileID);
		
		[DllImport("__Internal")]
		private static extern void grow_onLoginStartedForProvider (int _provider);
		
		[DllImport("__Internal")]
		private static extern void grow_onLogoutFailedForProvider (int _provider);
		
		[DllImport("__Internal")]
		private static extern void grow_onLogoutFinishedForProvider (int _provider);
		
		[DllImport("__Internal")]
		private static extern void grow_onLogoutStartedForProvider (int _provider);

		[DllImport("__Internal")]
		private static extern void grow_onGetContactsFailedForProvider (int _provider);

		[DllImport("__Internal")]
		private static extern void grow_onGetContactsFinishedForProvider (int _provider);

		[DllImport("__Internal")]
		private static extern void grow_onGetContactsStartedForProvider (int _provider);
		
		[DllImport("__Internal")]
		private static extern void grow_onSocialActionCancelledForProvider (int _actionType, int _provider);
		
		[DllImport("__Internal")]
		private static extern void grow_onSocialActionFailedForProvider (int _actionType, int _provider);
		
		[DllImport("__Internal")]
		private static extern void grow_onSocialActionFinishedForProvider (int _actionType, int _provider);
		
		[DllImport("__Internal")]
		private static extern void grow_onSocialActionStartedForProvider (int _actionType, int _provider);

		// Game center section
		[DllImport("__Internal")]
		private static extern void grow_onLevelEnded (string _levelID, bool _isCompleted, int _timesPlayed, int _timesStarted, long _fastestDuration, long _slowestDuration);

		[DllImport("__Internal")]
		private static extern void grow_onLevelStarted (string _levelID, int _timesPlayed, int _timesStarted, long _fastestDuration, long _slowestDuration);

		[DllImport("__Internal")]
		private static extern void grow_onLatestScore (string _scoreID, double _latestScore);

		[DllImport("__Internal")]
		private static extern void grow_onScoreRecord (string _scoreID, double _recordScore);

		[DllImport("__Internal")]
		private static extern void grow_onWorld (string _worldID, bool _isCompleted);

		// Ads section
		[DllImport("__Internal")]
		private static extern void grow_onAdShown ();

		[DllImport("__Internal")]
		private static extern void grow_onAdHidden ();

		[DllImport("__Internal")]
		private static extern void grow_onAdClicked ();

		[DllImport("__Internal")]
		private static extern void grow_onVideoAdStarted ();

		[DllImport("__Internal")]
		private static extern void grow_onVideoAdCompleted ();

		[DllImport("__Internal")]
		private static extern void grow_onVideoAdClicked ();

		[DllImport("__Internal")]
		private static extern void grow_onVideoAdClosed ();
		
		// Misc.
		[DllImport("__Internal")]
		private static extern void grow_onUserRating ();

		#endregion

		#region Initialise Methods

		protected override void Initialise (string _gameKey, string _environmentKey, string _referrerName)
		{
			base.Initialise(_gameKey, _environmentKey, _referrerName);

			// Native call
			grow_initialise(_gameKey, _environmentKey, _referrerName);
		}

		#endregion

		#region Billing Methods

		protected override void ReportOnBillingSupported ()
		{
			base.ReportOnBillingSupported();

			// Native call
			grow_onBillingSupported();
		}

		protected override void ReportOnBillingNotSupported ()
		{
			base.ReportOnBillingNotSupported();

			// Native call
			grow_onBillingNotSupported();
		}

		internal override void ReportOnBillingPurchaseStarted (string _productID)
		{
			base.ReportOnBillingPurchaseStarted(_productID);

			// Native call
			grow_onMarketPurchaseStartedForProduct(_productID);
		}

		internal override void ReportOnBillingPurchaseFinished (string _productID, long _priceInMicros, string _currencyCode)
		{
			base.ReportOnBillingPurchaseFinished(_productID, _priceInMicros, _currencyCode);

			// Native call
			grow_onMarketPurchaseFinishedForProduct(_productID, _priceInMicros.ToString(), _currencyCode);
		}

		internal override void ReportOnBillingPurchaseCancelled (string _productID)
		{
			base.ReportOnBillingPurchaseCancelled(_productID);

			// Native call
			grow_onMarketPurchaseCancelledForProduct(_productID);
		}

		internal override void ReportOnBillingPurchaseFailed (string _productID)
		{
			base.ReportOnBillingPurchaseFailed(_productID);

			// Native call
			grow_onMarketPurchaseFailed();
		}

		internal override void ReportOnBillingPurchasesRestoreStarted ()
		{
			base.ReportOnBillingPurchasesRestoreStarted();

			// Native call
			grow_onRestoreTransactionsStarted();
		}

		internal override void ReportOnBillingPurchasesRestoreFinished (bool _success)
		{
			base.ReportOnBillingPurchasesRestoreFinished(_success);

			// Native call
			grow_onRestoreTransactionsFinishedWithSuccess(_success);
		}

		internal override void ReportOnBillingPurchaseVerificationFailed ()
		{
			base.ReportOnBillingPurchaseVerificationFailed();

			// Native call
			grow_onVerificationFailed();
		}

		#endregion

		#region Social Methods

		internal override void ReportOnSocialLoginStarted (eSocialProvider _provider)
		{
			base.ReportOnSocialLoginStarted(_provider);

			// Native call
			grow_onLoginStartedForProvider((int)_provider);
		}

		internal override void ReportOnSocialLoginFinished (eSocialProvider _provider, string _userID)
		{
			base.ReportOnSocialLoginFinished(_provider, _userID);

			// Native call
			grow_onLoginFinishedForProvider((int)_provider, _userID);
		}

		internal override void ReportOnSocialLoginCancelled (eSocialProvider _provider)
		{
			base.ReportOnSocialLoginCancelled(_provider);

			// Native call
			grow_onLoginCancelledForProvider((int)_provider);
		}

		internal override void ReportOnSocialLoginFailed (eSocialProvider _provider)
		{
			base.ReportOnSocialLoginFailed(_provider);

			// Native call
			grow_onLoginFailedForProvider((int)_provider);
		}

		internal override void ReportOnSocialLogoutStarted (eSocialProvider _provider)
		{
			base.ReportOnSocialLogoutStarted(_provider);

			// Native call
			grow_onLogoutStartedForProvider((int)_provider);
		}

		internal override void ReportOnSocialLogoutFinished (eSocialProvider _provider)
		{
			base.ReportOnSocialLogoutFinished(_provider);

			// Native call
			grow_onLogoutFinishedForProvider((int)_provider);
		}

		internal override void ReportOnSocialLogoutFailed (eSocialProvider _provider)
		{
			base.ReportOnSocialLogoutFailed(_provider);

			// Native call
			grow_onLogoutFailedForProvider((int)_provider);
		}

		internal override void ReportOnGetContactsStartedForProvider (eSocialProvider _provider)
		{
			base.ReportOnGetContactsStartedForProvider(_provider);

			// Native call
			grow_onGetContactsStartedForProvider((int)_provider);
		}

		internal override void ReportOnGetContactsFinishedForProvider (eSocialProvider _provider)
		{
			base.ReportOnGetContactsFinishedForProvider(_provider);

			// Native call
			grow_onGetContactsFinishedForProvider((int)_provider);
		}

		internal override void ReportOnGetContactsFailedForProvider (eSocialProvider _provider)
		{
			base.ReportOnGetContactsFailedForProvider(_provider);

			// Native call
			grow_onGetContactsFailedForProvider((int)_provider);
		}

		internal override void ReportOnSocialActionStarted (eSocialActionType _actionType, eSocialProvider _provider)
		{
			base.ReportOnSocialActionStarted(_actionType, _provider);

			// Native call
			grow_onSocialActionStartedForProvider((int)_actionType, (int)_provider);
		}

		internal override void ReportOnSocialActionFinished (eSocialActionType _actionType, eSocialProvider _provider)
		{
			base.ReportOnSocialActionFinished(_actionType, _provider);

			// Native call
			grow_onSocialActionFinishedForProvider((int)_actionType, (int)_provider);
		}

		internal override void ReportOnSocialActionCancelled (eSocialActionType _actionType, eSocialProvider _provider)
		{
			base.ReportOnSocialActionCancelled(_actionType, _provider);

			// Native call
			grow_onSocialActionCancelledForProvider((int)_actionType, (int)_provider);
		}

		internal override void ReportOnSocialActionFailed (eSocialActionType _actionType, eSocialProvider _provider)
		{
			base.ReportOnSocialActionFailed(_actionType, _provider);

			// Native call
			grow_onSocialActionFailedForProvider((int)_actionType, (int)_provider);
		}

		#endregion

		#region Game Services Methods

		internal override void ReportOnLatestScore (string _scoreID, double _latestScore)
		{
			base.ReportOnLatestScore(_scoreID, _latestScore);

			// Native call
			grow_onLatestScore(_scoreID, _latestScore);
		}

		#endregion

		#region Misc Methods

		internal override void ReportOnUserRating ()
		{
			base.ReportOnUserRating();

			// Native call
			grow_onUserRating();
		}

		#endregion
	}
}
#endif
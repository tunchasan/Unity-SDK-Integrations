using UnityEngine;

#if USES_SOOMLA_GROW && UNITY_ANDROID
namespace VoxelBusters.NativePlugins
{
	using Internal;
	
	public sealed partial class SoomlaGrowServiceAndroid : SoomlaGrowService 
	{
		#region Platform Native Info

		internal class Native
		{
			// Handler class name
			internal class Class
			{
				internal const string NAME			= "com.voxelbusters.nativeplugins.features.external.sdk.soomla.soomlagrow.SoomlaGrow";
			}
			
			// For holding method names
			internal class Methods
			{
				// Initialization
				internal const string 	INITIALISE								=	"initialise";
				
				internal const string 	ON_BILLING_SUPPORTED					=	"onBillingSupported";
				internal const string 	ON_BILLING_NOT_SUPPORTED				=	"onBillingNotSupported";
				internal const string 	ON_MARKET_PURCHASE_STARTED				=	"onMarketPurchaseStarted";
				internal const string 	ON_MARKET_PURCHASE_FINISHED 			=	"onMarketPurchaseFinished";
				internal const string 	ON_MARKET_PURCHASE_CANCELLED 			=	"onMarketPurchaseCancelled";

				internal const string 	ON_MARKET_PURCHASE_FAILED 				=	"onMarketPurchaseFailed";
				internal const string 	ON_RESTORE_TRANSACTIONS_STARTED 		=	"onRestoreTransactionsStarted";
				
				internal const string 	ON_RESTORE_TRANSACTIONS_FINISHED 		=	"onRestoreTransactionsFinished";
				
				internal const string 	ON_BILLING_PURCHASE_VERIFICATION_FAILED =	"onBillingPurchaseVerificationFailed";
				
				internal const string 	ON_SOCIAL_LOGIN_STARTED_FOR_PROVIDER 	=	"onSocialLoginStartedForProvider";
				
				internal const string 	ON_SOCIAL_LOGIN_FINISHED_FOR_PROVIDER 	=	"onSocialLoginFinishedForProvider";


				internal const string 	ON_SOCIAL_LOGIN_CANCELLED_FOR_PROVIDER 	=	"onSocialLoginCancelledForProvider";
				internal const string 	ON_SOCIAL_LOGIN_FAILED_FOR_PROVIDER 	=	"onSocialLoginFailedForProvider";
				
				internal const string 	ON_SOCIAL_LOGOUT_STARTED_FOR_PROVIDER 	=	"onSocialLogoutStartedForProvider";
				
				internal const string 	ON_SOCIAL_LOGOUT_FINISHED_FOR_PROVIDER 	=	"onSocialLogoutFinishedForProvider";

				internal const string 	ON_SOCIAL_ACTION_STARTED_FOR_PROVIDER 	=	"onSocialActionStartedForProvider";
				
				internal const string 	ON_SOCIAL_ACTION_FINISHED_FOR_PROVIDER 	=	"onSocialActionFinishedForProvider";
				internal const string 	ON_SOCIAL_ACTION_CANCELLED_FOR_PROVIDER	=	"onSocialActionCancelledForProvider";
				internal const string 	ON_SOCIAL_ACTION_FAILED_FOR_PROVIDER 	=	"onSocialActionFailedForProvider";
				internal const string 	ON_LATEST_SCORE 						=	"onLatestScore";

				internal const string 	ON_SOCIAL_LOGOUT_FAILED_FOR_PROVIDER 	=	"onSocialLogoutFailedForProvider";
				internal const string 	ON_GET_CONTACTS_STARTED_FOR_PROVIDER 	=	"onGetContactsStartedForProvider";
				internal const string 	ON_GET_CONTACTS_FINISHED_FOR_PROVIDER 	=	"onGetContactsFinishedForProvider";
				internal const string 	ON_GET_CONTACTS_FAILED_FOR_PROVIDER 	=	"onGetContactsFailedForProvider";
				internal const string 	ON_REPORT_LEVEL_ENDED 					=	"onReportLevelEnded";
				internal const string 	ON_REPORT_LEVEL_STARTED 				=	"onReportLevelStarted";
				internal const string 	ON_REPORT_LATEST_SCORE 					=	"onReportLatestScore";
				internal const string 	ON_REPORT_SCORE_RECORD 					=	"onReportScoreRecord";
				internal const string 	ON_REPORT_ON_WORLD 						=	"onReportOnWorld";
				
				
				internal const string 	ON_AD_SHOWN								= 	"onAdShown";
				internal const string 	ON_AD_HIDDEN							= 	"onAdHidden";
				internal const string 	ON_AD_CLICKED							=	"onAdClicked";
				internal const string 	ON_VIDEO_AD_STARTED						=	"onVideoAdStarted";
				internal const string 	ON_VIDEO_AD_COMPLETED					=	"onVideoAdCompleted";
				internal const string 	ON_VIDEO_AD_CLICKED						=	"onVideoAdClicked";
				internal const string 	ON_VIDEO_AD_CLOSED						=	"onVideoAdClosed";
				internal const string 	ON_REPORT_USER_RATING					=	"onReportUserRating";
				
				
				
				
			}
		}

		#endregion


		#region  Native Access Variables
		
		internal static AndroidJavaObject  	Plugin
		{
			get;
			set;
		}
		
		#endregion
	}
}

#endif

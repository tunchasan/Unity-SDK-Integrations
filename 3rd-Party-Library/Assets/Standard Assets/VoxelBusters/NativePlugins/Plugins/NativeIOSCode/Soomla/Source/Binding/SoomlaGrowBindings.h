//
//  SoomlaGrowBindings.h
//  Cross Platform Native Plugins
//
//  Created by Ashwin kumar on 13/11/15.
//  Copyright (c) 2015 Voxel Busters Interactive LLP. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <GrowHighway.h>

// SDK methods
UIKIT_EXTERN void grow_initialise (const char *gameKey, const char *environmentKey, const char *referrerName);
UIKIT_EXTERN char* grow_getSoomlaUID ();
UIKIT_EXTERN int grow_getUIDType ();
UIKIT_EXTERN bool grow_isSocialConnect ();

// Billing methods
UIKIT_EXTERN void grow_onBillingNotSupported ();
UIKIT_EXTERN void grow_onBillingSupported ();
UIKIT_EXTERN void grow_onMarketPurchaseCancelledForProduct (const char *productID);
UIKIT_EXTERN void grow_onMarketPurchaseFinishedForProduct (const char *productID, const char *priceMicros, const char *currencyCode);
UIKIT_EXTERN void grow_onMarketPurchaseStartedForProduct (const char *productID);
UIKIT_EXTERN void grow_onMarketPurchaseFailed ();
UIKIT_EXTERN void grow_onRestoreTransactionsFinishedWithSuccess (bool success);
UIKIT_EXTERN void grow_onRestoreTransactionsStarted ();
UIKIT_EXTERN void grow_onVerificationTimeout ();
UIKIT_EXTERN void grow_onVerificationFailed ();
UIKIT_EXTERN void grow_onUnexpectedErrorOccurred ();

// Social methods
UIKIT_EXTERN void grow_onLoginCancelledForProvider (int provider);
UIKIT_EXTERN void grow_onLoginFailedForProvider (int provider);
UIKIT_EXTERN void grow_onLoginFinishedForProvider (int provider, const char *profileID);
UIKIT_EXTERN void grow_onLoginStartedForProvider (int provider);
UIKIT_EXTERN void grow_onLogoutFailedForProvider (int provider);
UIKIT_EXTERN void grow_onLogoutFinishedForProvider (int provider);
UIKIT_EXTERN void grow_onLogoutStartedForProvider (int provider);
UIKIT_EXTERN void grow_onGetContactsFailedForProvider (int provider);
UIKIT_EXTERN void grow_onGetContactsFinishedForProvider (int provider);
UIKIT_EXTERN void grow_onGetContactsStartedForProvider (int provider);
UIKIT_EXTERN void grow_onSocialActionCancelledForProvider (int actionType, int provider);
UIKIT_EXTERN void grow_onSocialActionFailedForProvider (int actionType, int provider);
UIKIT_EXTERN void grow_onSocialActionFinishedForProvider (int actionType, int provider);
UIKIT_EXTERN void grow_onSocialActionStartedForProvider (int actionType, int provider);

// Game center methods
UIKIT_EXTERN void grow_onLevelEnded (const char *levelID, bool isCompleted, int timesPlayed, int timesStarted, long fastestDuration, long slowestDuration);
UIKIT_EXTERN void grow_onLevelStarted (const char *levelID, int timesPlayed, int timesStarted, long fastestDuration, long slowestDuration);
UIKIT_EXTERN void grow_onLatestScore (const char *scoreID, double latestScore);
UIKIT_EXTERN void grow_onScoreRecord (const char *scoreID, double recordScore);
UIKIT_EXTERN void grow_onWorld (const char *worldId, bool isCompleted);

// Ads
UIKIT_EXTERN void grow_onAdShown ();
UIKIT_EXTERN void grow_onAdHidden ();
UIKIT_EXTERN void grow_onAdClicked ();
UIKIT_EXTERN void grow_onVideoAdStarted ();
UIKIT_EXTERN void grow_onVideoAdCompleted ();
UIKIT_EXTERN void grow_onVideoAdClicked ();
UIKIT_EXTERN void grow_onVideoAdClosed ();

// Misc.
UIKIT_EXTERN void grow_onUserRating ();
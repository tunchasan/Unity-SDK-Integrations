/*
 * Copyright (C) 2012-2015 Soomla Inc. - All Rights Reserved
 *
 *   Unauthorized copying of this file, via any medium is strictly prohibited
 *   Proprietary and confidential
 *
 *   Written by Refael Dakar <refael@soom.la>
 */

#import <Foundation/Foundation.h>
#import "SoomlaSocial.h"
#import "SoomlaEventsConsts.h"

#define EVENT_HIGHWAY_UID_CHANGED    @"sg_highway_uid_changed"

#define EVENT_HIGHWAY_INITIALIZED    @"sg_highway_initialized"
#define EVENT_HIGHWAY_CONNECTED      @"sg_highway_connected"
#define EVENT_HIGHWAY_DISCONNECTED   @"sg_highway_disconnected"

#define SERVICE_VERSION 3

typedef enum{
    UID_TYPE_DEVICE_ONLY = 0,
    UID_TYPE_SOCIAL = 1
}HighwayUidType;

// Error Codes
#define ERR_GENERAL                 0
#define ERR_VERIFICATION_TIMEOUT    1
#define ERR_VERIFICATION_FAIL       2
#define ERR_PURCHASE_FAIL           3

@interface GrowHighway : NSObject

+ (GrowHighway *)getInstance;

- (void)onLevelEnded:(NSString *)levelId
          andCompleted:(BOOL)isCompleted
       withTimesPlayed:(int)timesPlayed
       andTimesStarted:(int)timesStarted
    andFastestDuration:(long)fastestDuration
    andSlowestDuration:(long)slowestDuration;

- (void)onLevelStarted:(NSString *)levelId
         withTimesPlayed:(int)timesPlayed
         andTimesStarted:(int)timesStarted
      andFastestDuration:(long)fastestDuration
      andSlowestDuration:(long)slowestDuration;

- (void)onLatestScore:(NSString *)scoreId changedTo:(double)latestScore;

- (void)onScoreRecord:(NSString *)scoreId changedTo:(double)recordScore;

- (void)onWorld:(NSString *)worldId isCompleted:(BOOL)isCompleted;

- (void)onLoginCancelledForProvider:(SocialProvider)provider;

- (void)onLoginFailedForProvider:(SocialProvider)provider;

- (void)onLoginFinishedForProvider:(SocialProvider)provider withProfile:(NSString *)profileId;

- (void)onLoginStartedForProvider:(SocialProvider)provider;

- (void)onLogoutFailedForProvider:(SocialProvider)provider;

- (void)onLogoutFinishedForProvider:(SocialProvider)provider;

- (void)onLogoutStartedForProvider:(SocialProvider)provider;

- (void)onGetContactsFailedForProvider:(SocialProvider)provider;

- (void)onGetContactsFinishedForProvider:(SocialProvider)provider;

- (void)onGetContactsStartedForProvider:(SocialProvider)provider;

- (void)onSocialAction:(SocialActionType)socialActionType cancelledForProvider:(SocialProvider)provider;

- (void)onSocialAction:(SocialActionType)socialActionType failedForProvider:(SocialProvider)provider;

- (void)onSocialAction:(SocialActionType)socialActionType finishedForProvider:(SocialProvider)provider;

- (void)onSocialAction:(SocialActionType)socialActionType startedForProvider:(SocialProvider)provider;

- (void)onUserRating;

- (void)onBillingNotSupported;

- (void)onBillingSupported;

- (void)onMarketPurchaseCancelledForProduct:(NSString *)productId;

- (void)onMarketPurchaseFinishedForProduct:(NSString *)marketProductId
                            andPriceMicros:(long)priceMicros
                           andCurrencyCode:(NSString *)currencyCode;

- (void)onMarketPurchaseStartedForProduct:(NSString *)productId;

- (void)onMarketPurchaseFailed;

- (void)onRestoreTransactionsFinishedWithSuccess:(BOOL)success;

- (void)onRestoreTransactionsStarted;

- (void)onVerificationTimeout;

- (void)onVerificationFailed;

- (void)onUnexpectedErrorOccurred;

- (void)onAdShown;

- (void)onAdHidden;

- (void)onAdClicked;

- (void)onVideoAdStarted;

- (void)onVideoAdCompleted;

- (void)onVideoAdClicked;

- (void)onVideoAdClosed;

- (void)initializeWithGameKey:(NSString*)gameKey andEnvKey:(NSString*)envKey andReferrer:(NSString*)referrer;

//- (NSArray *)getHighwayComponents;

//- (void)sendStartDuration;
//- (void)sendStopDuration;

- (void)sendEvent:(NSString*)eventName withExtraData:(NSDictionary *)eventExtra;

- (NSString *)getSoomlaUID;
- (HighwayUidType)getUIDType;
- (BOOL)isSocialConnect;

@end

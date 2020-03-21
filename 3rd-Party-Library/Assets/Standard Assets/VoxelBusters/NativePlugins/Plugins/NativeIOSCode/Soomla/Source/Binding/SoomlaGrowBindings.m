//
//  SoomlaGrowBindings.m
//  Cross Platform Native Plugins
//
//  Created by Ashwin kumar on 13/11/15.
//  Copyright (c) 2015 Voxel Busters Interactive LLP. All rights reserved.
//

#import "SoomlaGrowBindings.h"
#import "SoomlaGrowHandler.h"

#pragma mark - SDK Methods

void grow_initialise (const char *gameKey, const char *environmentKey, const char *referrerName)
{
	[[GrowHighway getInstance] initializeWithGameKey:ConvertToNSString(gameKey)
										   andEnvKey:ConvertToNSString(environmentKey)
										 andReferrer:ConvertToNSString(referrerName)];
	
	[SoomlaGrowHandler initialise];
}

char* grow_getSoomlaUID ()
{
	const char *uid	= [[[GrowHighway getInstance] getSoomlaUID] UTF8String];
	
	// Return c value
	return CStringCopy(uid);
}

int grow_getUIDType ()
{
	return [[GrowHighway getInstance] getUIDType];
}

bool grow_isSocialConnect ()
{
	return [[GrowHighway getInstance] isSocialConnect];
}

#pragma mark - Billing Methods

void grow_onBillingNotSupported ()
{
	[[GrowHighway getInstance] onBillingNotSupported];
}

void grow_onBillingSupported ()
{
	[[GrowHighway getInstance] onBillingSupported];
}

void grow_onMarketPurchaseCancelledForProduct (const char *productID)
{
	[[GrowHighway getInstance] onMarketPurchaseCancelledForProduct:ConvertToNSString(productID)];
}

void grow_onMarketPurchaseFinishedForProduct (const char *productID, const char *priceMicros, const char *currencyCode)
{
	[[GrowHighway getInstance] onMarketPurchaseFinishedForProduct:ConvertToNSString(productID)
												   andPriceMicros:[ConvertToNSString(productID) longLongValue]
												  andCurrencyCode:ConvertToNSString(currencyCode)];
}

void grow_onMarketPurchaseStartedForProduct (const char *productID)
{
	[[GrowHighway getInstance] onMarketPurchaseStartedForProduct:ConvertToNSString(productID)];
}

void grow_onMarketPurchaseFailed ()
{
	[[GrowHighway getInstance] onMarketPurchaseFailed];
}

void grow_onRestoreTransactionsFinishedWithSuccess (bool success)
{
	[[GrowHighway getInstance] onRestoreTransactionsFinishedWithSuccess:success];
}

void grow_onRestoreTransactionsStarted ()
{
	[[GrowHighway getInstance] onRestoreTransactionsStarted];
}

void grow_onVerificationTimeout ()
{
	[[GrowHighway getInstance] onVerificationTimeout];
}

void grow_onVerificationFailed ()
{
	[[GrowHighway getInstance] onVerificationFailed];
}

void grow_onUnexpectedErrorOccurred ()
{
	[[GrowHighway getInstance] onUnexpectedErrorOccurred];
}

#pragma mark - Social Methods

void grow_onLoginCancelledForProvider (int provider)
{
	[[GrowHighway getInstance] onLoginCancelledForProvider:(SocialProvider)provider];
}

void grow_onLoginFailedForProvider (int provider)
{
	[[GrowHighway getInstance] onLoginFailedForProvider:(SocialProvider)provider];
}

void grow_onLoginFinishedForProvider (int provider, const char *profileID)
{
	[[GrowHighway getInstance] onLoginFinishedForProvider:(SocialProvider)provider withProfile:ConvertToNSString(profileID)];
}

void grow_onLoginStartedForProvider (int provider)
{
	[[GrowHighway getInstance] onLoginStartedForProvider:(SocialProvider)provider];
}

void grow_onLogoutFailedForProvider (int provider)
{
	[[GrowHighway getInstance] onLogoutFailedForProvider:(SocialProvider)provider];
}

void grow_onLogoutFinishedForProvider (int provider)
{
	[[GrowHighway getInstance] onLogoutFinishedForProvider:(SocialProvider)provider];
}

void grow_onLogoutStartedForProvider (int provider)
{
	[[GrowHighway getInstance] onLogoutStartedForProvider:(SocialProvider)provider];
}

void grow_onGetContactsFailedForProvider (int provider)
{
	[[GrowHighway getInstance] onGetContactsFailedForProvider:(SocialProvider)provider];
}

void grow_onGetContactsFinishedForProvider (int provider)
{
	[[GrowHighway getInstance] onGetContactsFinishedForProvider:(SocialProvider)provider];
}

void grow_onGetContactsStartedForProvider (int provider)
{
	[[GrowHighway getInstance] onGetContactsStartedForProvider:(SocialProvider)provider];
}

void grow_onSocialActionCancelledForProvider (int actionType, int provider)
{
	[[GrowHighway getInstance] onSocialAction:(SocialActionType)actionType
						 cancelledForProvider:(SocialProvider)provider];
}

void grow_onSocialActionFailedForProvider (int actionType, int provider)
{
	[[GrowHighway getInstance] onSocialAction:(SocialActionType)actionType
							failedForProvider:(SocialProvider)provider];
}

void grow_onSocialActionFinishedForProvider (int actionType, int provider)
{
	[[GrowHighway getInstance] onSocialAction:(SocialActionType)actionType
						  finishedForProvider:(SocialProvider)provider];
}

void grow_onSocialActionStartedForProvider (int actionType, int provider)
{
	[[GrowHighway getInstance] onSocialAction:(SocialActionType)actionType
						   startedForProvider:(SocialProvider)provider];
}

#pragma mark - GC Methods

void grow_onLevelEnded (const char *levelID, bool isCompleted, int timesPlayed, int timesStarted, long fastestDuration, long slowestDuration)
{
	[[GrowHighway getInstance] onLevelEnded:ConvertToNSString(levelID)
							   andCompleted:isCompleted
							withTimesPlayed:timesPlayed
							andTimesStarted:timesStarted
						 andFastestDuration:fastestDuration
						 andSlowestDuration:slowestDuration];
}

void grow_onLevelStarted (const char *levelID, int timesPlayed, int timesStarted, long fastestDuration, long slowestDuration)
{
	[[GrowHighway getInstance] onLevelStarted:ConvertToNSString(levelID)
							  withTimesPlayed:timesPlayed
							  andTimesStarted:timesStarted
						   andFastestDuration:fastestDuration
						   andSlowestDuration:slowestDuration];
}

void grow_onLatestScore (const char *scoreID, double latestScore)
{
	[[GrowHighway getInstance] onLatestScore:ConvertToNSString(scoreID)
								   changedTo:latestScore];
}

void grow_onScoreRecord (const char *scoreID, double recordScore)
{
	[[GrowHighway getInstance] onScoreRecord:ConvertToNSString(scoreID)
								   changedTo:recordScore];
}

void grow_onWorld (const char *worldId, bool isCompleted)
{
	[[GrowHighway getInstance] onWorld:ConvertToNSString(worldId)
						   isCompleted:isCompleted];
}

#pragma mark - GC Methods

void grow_onAdShown ()
{
	[[GrowHighway getInstance] onAdShown];
}

void grow_onAdHidden ()
{
	[[GrowHighway getInstance] onAdHidden];
}

void grow_onAdClicked ()
{
	[[GrowHighway getInstance] onAdClicked];
}

void grow_onVideoAdStarted ()
{
	[[GrowHighway getInstance] onVideoAdStarted];
}

void grow_onVideoAdCompleted ()
{
	[[GrowHighway getInstance] onVideoAdCompleted];
}

void grow_onVideoAdClicked ()
{
	[[GrowHighway getInstance] onVideoAdClicked];
}

void grow_onVideoAdClosed ()
{
	[[GrowHighway getInstance] onVideoAdClosed];
}

#pragma mark - Misc.

void grow_onUserRating ()
{
	[[GrowHighway getInstance] onUserRating];
}
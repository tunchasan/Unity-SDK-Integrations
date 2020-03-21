/*
 * Copyright (C) 2012-2015 Soomla Inc. - All Rights Reserved
 *
 *   Unauthorized copying of this file, via any medium is strictly prohibited
 *   Proprietary and confidential
 *
 *   Written by Refael Dakar <refael@soom.la>
 */

/******
 Includes all events from all modules, so they are defined in Highway
 but without the need to import headers from the actual modules, since they
 should be decoupled
 */

/** Store Events **/

// Module Events
#define INNER_EVENT_BILLING_NOT_SUPPORTED         @"sg_BillingNotSupported"
#define INNER_EVENT_BILLING_SUPPORTED             @"sg_BillingSupported"
#define INNER_EVENT_MARKET_PURCHASE_CANCELLED     @"sg_MarketPurchaseCancelled"
#define INNER_EVENT_MARKET_PURCHASED              @"sg_MarketPurchased"
#define INNER_EVENT_MARKET_PURCHASE_VERIF         @"sg_MarketPurchaseVerification"
#define INNER_EVENT_MARKET_PURCHASE_STARTED       @"sg_MarketPurchaseProcessStarted"
#define INNER_EVENT_RESTORE_TRANSACTIONS_FINISHED @"sg_RestoreTransactionsFinished"
#define INNER_EVENT_RESTORE_TRANSACTIONS_STARTED  @"sg_RestoreTransactionsStarted"
#define INNER_EVENT_UNEXPECTED_ERROR_IN_STORE     @"sg_UnexpectedErrorInStore"
#define INNER_EVENT_AD_SHOWN                      @"sg_AdShownHighway"
#define INNER_EVENT_AD_HIDDEN                     @"sg_AdHiddenHighway"
#define INNER_EVENT_AD_CLICKED                    @"sg_AdClickedHighway"
#define INNER_EVENT_VIDEO_AD_STARTED              @"sg_VideoAdStartedHighway"
#define INNER_EVENT_VIDEO_AD_COMPLETED            @"sg_VideoAdCompletedHighway"
#define INNER_EVENT_VIDEO_AD_CLICKED              @"sg_VideoAdClickedHighway"
#define INNER_EVENT_VIDEO_AD_CLOSED               @"sg_VideoAdClosedHighway"


// UserInfo Elements
#define INNER_DICT_ELEMENT_BALANCE             @"balance"
#define INNER_DICT_ELEMENT_CURRENCY            @"VirtualCurrency"
#define INNER_DICT_ELEMENT_AMOUNT_ADDED        @"amountAdded"
#define INNER_DICT_ELEMENT_GOOD                @"VirtualGood"
#define INNER_DICT_ELEMENT_EquippableVG        @"EquippableVG"
#define INNER_DICT_ELEMENT_UpgradeVG           @"UpgradeVG"
#define INNER_DICT_ELEMENT_PURCHASABLE         @"PurchasableVirtualItem"
#define INNER_DICT_ELEMENT_PURCHASABLE_ID      @"PurchasableVirtualItemId"
#define INNER_DICT_ELEMENT_PURCHASABLE_PROD_ID @"PurchasableVirtualItemProductId"
#define INNER_DICT_ELEMENT_ITEM_PRICE_MICROS   @"itemPriceMicros"
#define INNER_DICT_ELEMENT_ITEM_CURRENCY_CODE  @"itemCurrencyCode"
#define INNER_DICT_ELEMENT_ITEM_DEFAULT_PRICE  @"itemDefaultPrice"
#define INNER_DICT_ELEMENT_DEVELOPERPAYLOAD    @"DeveloperPayload"
#define INNER_DICT_ELEMENT_RECEIPT             @"receipt"
#define INNER_DICT_ELEMENT_TOKEN               @"token"
#define INNER_DICT_ELEMENT_SUCCESS             @"success"
#define INNER_DICT_ELEMENT_VERIFIED            @"verified"
#define INNER_DICT_ELEMENT_TRANSACTION         @"transaction"
#define INNER_DICT_ELEMENT_ERROR_MESSAGE       @"error_msg"
#define INNER_DICT_ELEMENT_ERROR_CODE          @"error_code"
#define INNER_DICT_ELEMENT_PRODUCTID           @"productId"
#define INNER_DICT_ELEMENT_PRICE               @"price"
#define INNER_DICT_ELEMENT_TITLE               @"title"
#define INNER_DICT_ELEMENT_DESCRIPTION         @"description"
#define INNER_DICT_ELEMENT_LOCALE              @"locale"
#define INNER_DICT_ELEMENT_MARKET_ITEMS        @"marketItems"

/** Profile Events **/

// Module Events
#define INNER_EVENT_UP_USER_RATING                    @"sg_up_user_rating"

#define INNER_EVENT_UP_LOGIN_STARTED                  @"sg_up_login_started"
#define INNER_EVENT_UP_LOGIN_FINISHED                 @"sg_up_login_finished"
#define INNER_EVENT_UP_LOGIN_FAILED                   @"sg_up_login_failed"
#define INNER_EVENT_UP_LOGIN_CANCELLED                @"sg_up_login_cancelled"

#define INNER_EVENT_UP_LOGOUT_STARTED                 @"sg_up_logout_started"
#define INNER_EVENT_UP_LOGOUT_FINISHED                @"sg_up_logout_finished"
#define INNER_EVENT_UP_LOGOUT_FAILED                  @"sg_up_logout_failed"

#define INNER_EVENT_UP_SOCIAL_ACTION_STARTED          @"sg_up_social_action_started"
#define INNER_EVENT_UP_SOCIAL_ACTION_FINISHED         @"sg_up_social_action_finished"
#define INNER_EVENT_UP_SOCIAL_ACTION_CANCELLED        @"sg_up_social_action_cancelled"
#define INNER_EVENT_UP_SOCIAL_ACTION_FAILED           @"sg_up_social_action_failed"

#define INNER_EVENT_UP_GET_CONTACTS_STARTED           @"sg_up_get_contacts_started"
#define INNER_EVENT_UP_GET_CONTACTS_FINISHED          @"sg_up_get_contacts_finished"
#define INNER_EVENT_UP_GET_CONTACTS_FAILED            @"sg_up_get_contacts_failed"


// UserProfile Elements
#define INNER_DICT_ELEMENT_USER_PROFILE               @"userProfile"
#define INNER_DICT_ELEMENT_PROFILE_ID                 @"profileId"
#define INNER_DICT_ELEMENT_PROVIDER                   @"provider"
#define INNER_DICT_ELEMENT_SOCIAL_ACTION_TYPE         @"socialActiontype"
#define INNER_DICT_ELEMENT_MESSAGE                    @"message"
#define INNER_DICT_ELEMENT_CONTACTS                   @"contacts"
#define INNER_DICT_ELEMENT_FEEDS                      @"feeds"
#define INNER_DICT_ELEMENT_REWARD                     @"reward"
#define INNER_DICT_ELEMENT_IS_BADGE                   @"isBadge"

/** LevelUp Events **/

// Module Events
#define INNER_EVENT_SCORE_RECORD_CHANGED                @"sg_lu_score_record_changed"
#define INNER_EVENT_SCORE_LATEST_CHANGED                @"sg_lu_score_latest_changed"
#define INNER_EVENT_SCORE_RECORD_REACHED                @"sg_lu_score_record_reached"
#define INNER_EVENT_WORLD_COMPLETED                     @"sg_lu_world_completed"
#define INNER_EVENT_LEVEL_STARTED                       @"sg_lu_level_started"
#define INNER_EVENT_LEVEL_ENDED                         @"sg_lu_level_ended"

// UserInfo Elements
#define INNER_DICT_ELEMENT_SCORE                   @"score"
#define INNER_DICT_ELEMENT_SCORE_RECORD            @"scoreRecord"
#define INNER_DICT_ELEMENT_SCORE_LATEST            @"scoreLatest"
#define INNER_DICT_ELEMENT_GATE                    @"gate"
#define INNER_DICT_ELEMENT_REWARD                  @"reward"
#define INNER_DICT_ELEMENT_WORLD                   @"world"
#define INNER_DICT_ELEMENT_WORLD_COMPLETED         @"worldCompleted"
#define INNER_DICT_ELEMENT_MISSION                 @"mission"
#define INNER_DICT_ELEMENT_LEVEL                   @"level"
#define INNER_DICT_ELEMENT_LEVEL_IS_COMPLETED      @"isCompleted"
#define INNER_DICT_ELEMENT_LEVEL_TIMES_PLAYED      @"timesPlayed"
#define INNER_DICT_ELEMENT_LEVEL_TIMES_STARTED     @"timesStarted"
#define INNER_DICT_ELEMENT_LEVEL_FASTEST_DURATION  @"fastestDuration"
#define INNER_DICT_ELEMENT_LEVEL_SLOWEST_DURATION  @"slowestDuration"
#define INNER_DICT_ELEMENT_IS_CHALLENGE            @"isChallenge"
#define INNER_DICT_ELEMENT_IS_BADGE                @"isBadge"
#define INNER_DICT_ELEMENT_METADATA                @"metadata"
#define INNER_DICT_ELEMENT_INNER_WORLD             @"innerWorld"

/** Highway Events **/

// Module Events
#define INNER_EVENT_INITIALIZE_HIGHWAY                   @"sg_initialize_highway"

// UserInfo Elements
#define INNER_DICT_ELEMENT_GAME_KEY                @"gameKey"
#define INNER_DICT_ELEMENT_ENV_KEY                 @"envKey"
#define INNER_DICT_ELEMENT_REFERRER                @"referrer"

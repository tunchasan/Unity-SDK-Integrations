/*
 * Copyright (C) 2012-2015 Soomla Inc. - All Rights Reserved
 *
 *   Unauthorized copying of this file, via any medium is strictly prohibited
 *   Proprietary and confidential
 *
 *   Written by Refael Dakar <refael@soom.la>
 */

/**
 an Enumaration listing all the social networks which are supported (or will be)
 */
typedef enum {
    FACEBOOK = 0,
    FOURSQUARE = 1,
    GOOGLE = 2,
    LINKEDIN = 3,
    MYSPACE = 4,
    TWITTER = 5,
    YAHOO = 6,
    SALESFORCE = 7,
    YAMMER = 8,
    RUNKEEPER = 9,
    INSTAGRAM = 10,
    FLICKR = 11
} SocialProvider;

/**
 an Enumaration listing all the social actions which are supported
 */
typedef enum {
    UPDATE_STATUS = 0,
    UPDATE_STORY = 1,
    UPLOAD_IMAGE = 2,
    GET_CONTACTS = 3,
    GET_FEED = 4
} SocialActionType;

/**
 Utility class to help convert `SocialProvider` and `SocialActionType` enum to `NSString` and back
 */
@interface SoomlaSocial : NSObject


+ (NSArray*)availableProviders;

/**
 Converts `Provider` enum to a string representation
 
 @param provider The provider value to convert
 @return a String representation if the supplied provider
 @exception NSException when the supplied provider is unspported
 */
+ (NSString *)providerEnumToString:(SocialProvider)provider;
+ (NSString *)providerNumberToString:(NSNumber*)providerNumber;

/**
 Converts the supplied `NSString` to `Provider` if possible
 
 @param providerTypeString The string to convert to `Provider`
 @return The `Provider` value corresponding to the supplied string
 @exception NSException when the supplied string does not have a corresponding
 `Provider` value
 */
+ (SocialProvider)providerStringToEnum:(NSString *)provider;

/**
 Maps an action enum to its corresponding string
 
 @param actionType The action enum to map
 @return A string representing the action enum
 */
+ (NSString *)actionEnumToString:(SocialActionType)actionType;
+ (NSString *)actionNumberToString:(NSNumber*)actionNumber;

/**
 Maps an action string to its corresponding enum value
 
 @param actionTypeString The action string to map
 @return An enum representing the action string
 */
+ (SocialActionType)actionStringToEnum:(NSString *)actionTypeString;

@end

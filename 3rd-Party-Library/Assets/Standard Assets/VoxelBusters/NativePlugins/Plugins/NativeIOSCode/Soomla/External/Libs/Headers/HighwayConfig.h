/*
 * Copyright (C) 2012-2015 Soomla Inc. - All Rights Reserved
 *
 *   Unauthorized copying of this file, via any medium is strictly prohibited
 *   Proprietary and confidential
 *
 *   Written by Refael Dakar <refael@soom.la>
 */

@interface HighwayConfig : NSObject

extern const NSString *const DEFAULT_URL;
extern const NSString *const DEFAULT_URL_REL_FASTLANE;
extern const NSString *const DEFAULT_URL_REL_CONNECT;
extern const NSString *const DEFAULT_URL_REL_META;
extern const NSString *const DEFAULT_URL_REL_ALIVE_TOUCH;
extern const NSString *const DEFAULT_MASTER_KEY;
extern const NSString *const DEFAULT_GAME_KEY;
extern const NSString *const DEFAULT_ENV_KEY;
extern const NSString *const DEFAULT_COUNTRY;

@property(nonatomic, strong) NSString *highwayUrl;
@property(nonatomic, strong) NSString *servicesUrl;

@property(nonatomic, strong) NSString *countryCode;

@property(nonatomic, strong) NSString *relUrlFastlane;
@property(nonatomic, strong) NSString *relUrlConnect;
@property(nonatomic, strong) NSString *relUrlStartDuration;
@property(nonatomic, strong) NSString *relUrlStopDuration;


@property(nonatomic, strong) NSString const *const gameKey;
@property(nonatomic, strong) NSString const *const envKey;

@property(nonatomic, readonly) NSMutableArray *components;

+ (HighwayConfig*)getInstance;

- (BOOL)isInitialized;

- (void)initializeWithGameKey:(NSString *)gameKey andEnvKey:(id)envKey;

@end




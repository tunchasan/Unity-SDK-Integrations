/*
 * Copyright (C) 2012-2015 Soomla Inc. - All Rights Reserved
 *
 *   Unauthorized copying of this file, via any medium is strictly prohibited
 *   Proprietary and confidential
 *
 *   Written by Refael Dakar <refael@soom.la>
 */

#import <Foundation/Foundation.h>

#define LogDebug(x,y) [SoomlaUtilsLite LogDebug:x withMessage:y];
#define LogError(x,y) [SoomlaUtilsLite LogError:x withMessage:y];
#define IsStringEmpty(x) [SoomlaUtilsLite isEmpty:x]

/**
 This class handles printing of error and debug messages.
 */
@interface SoomlaUtilsLite : NSObject

/**
 * Creates Log Debug message according to the given tag and message.
 *
 * @param tag The name of the class whose instance called this function.
 * @param msg The debug message to output to log.
 */
+ (void)LogDebug:(NSString*)tag withMessage:(NSString*)msg;

/**
 * Creates Log Error message according to the given tag and message.
 *
 * @param tag The name of the class whose instance called this function.
 * @param msg The error message to output to log.
 */
+ (void)LogError:(NSString*)tag withMessage:(NSString*)msg;

/**
 * Retrieves the device Id.
 *
 * @return The id of the device being used.
 */
+ (NSString*)deviceId;


+ (NSMutableDictionary*)jsonStringToDict:(NSString*)str;

+ (NSMutableArray*)jsonStringToArray:(NSString*)str;

+ (NSString*)dictToJsonString:(NSDictionary*)str;

+ (NSString*)arrayToJsonString:(NSArray*)arr;

+ (NSString *) applicationDirectory;

+ (BOOL)addSkipBackupAttributeToItemAtURL:(NSURL *)URL;

+ (void)setLoggingEnabled:(BOOL)logEnabled;

+ (NSString *)getClassName:(id)target;

+ (BOOL)isEmpty:(NSString *)target;

@end


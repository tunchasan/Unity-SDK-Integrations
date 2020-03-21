//
//  Utility.h
//  Cross Platform Native Plugins
//
//  Created by Ashwin kumar on 06/12/14.
//  Copyright (c) 2015 Voxel Busters Interactive LLP. All rights reserved.
//

#import <Foundation/Foundation.h>

// String
UIKIT_EXTERN NSString*			kNSStringDefault;
UIKIT_EXTERN NSString*			kBoolTrue;
UIKIT_EXTERN NSString*			kBoolFalse;
UIKIT_EXTERN const char*		kCStringEmpty;
UIKIT_EXTERN char* 				CStringCopy (const char* str);
UIKIT_EXTERN NSString* 			ConvertToNSString (const char * jsonCharArray);
UIKIT_EXTERN bool 				IsNullOrEmpty (NSString* string);

// Json
UIKIT_EXTERN id 				FromJson (const char * jsonCharArray);
UIKIT_EXTERN NSArray* 			ConvertToNSArray (const char * jsonCharArray);
UIKIT_EXTERN const char* 		ToJsonCString (id object);

// Rect operations
UIKIT_EXTERN CGRect 			GetScreenBounds ();
UIKIT_EXTERN CGRect 			GetApplicationFrame ();
UIKIT_EXTERN CGRect 			GetApplicationBounds ();
UIKIT_EXTERN CGRect 			ConvertToNormalisedRect (CGRect inputFrame);
UIKIT_EXTERN CGRect 			ConvertToApplicationSpace (CGRect normalisedRect);

// Device
UIKIT_EXTERN bool				IsIpadInterface();

typedef void (^ImageCompletionHandler)(const void *, NSInteger);

@interface Utility : NSObject

// Related to UIImage
+ (UIImage *)CreateImageFromByteArray:(void*)byteArray ofLength:(int)length;
+ (void)GetImageBytes:(NSString *)imageName completion:(ImageCompletionHandler)completion;

// Related to date time
+ (NSString *)ConvertNSDateToNSString:(NSDate *)date;
+ (NSDate *)ConvertNSStringToNSDate:(NSString *)dateString;

// NSData
+ (NSData *)CreateNSDataFromByteArray:(void*)byteArray ofLength:(int)length;

// Misc
+ (NSString *)GetUUID;

@end

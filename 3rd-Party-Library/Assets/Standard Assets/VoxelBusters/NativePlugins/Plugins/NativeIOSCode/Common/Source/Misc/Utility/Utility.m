//
//  Utility.m
//  Cross Platform Native Plugins
//
//  Created by Ashwin kumar on 06/12/14.
//  Copyright (c) 2015 Voxel Busters Interactive LLP. All rights reserved.
//

#import "Utility.h"

#pragma mark - String

const NSString *kNSStringDefault	= @"";
const NSString *kBoolTrue			= @"true";
const NSString *kBoolFalse			= @"false";
const char*		kCStringEmpty		= "";

NSString* ConvertToNSString (const char * jsonCharArray)
{
	if (jsonCharArray == NULL)
		return NULL;
	else
		return [NSString stringWithUTF8String:jsonCharArray];
}

bool IsNullOrEmpty (NSString* string)
{
	if (string == NULL)
		return TRUE;
	else
		return [string isEqualToString:@""];
}

char* CStringCopy (const char* str)
{
	if (str == NULL)
		return NULL;
	
	char* strCopy = (char*)malloc(strlen(str) + 1);
	strcpy(strCopy, str);
	
	return strCopy;
}

#pragma mark - Json

id FromJson (const char * jsonCharArray)
{
	NSString* jsonString   = ConvertToNSString(jsonCharArray);
	NSData* jsonData       = [jsonString dataUsingEncoding:NSUTF8StringEncoding];
	
	if (jsonData != NULL)
	{
		return [NSJSONSerialization JSONObjectWithData:jsonData
											   options:0
												 error:nil];
	}
	
	return NULL;
}

NSArray* ConvertToNSArray (const char * jsonCharArray)
{
	if (jsonCharArray == NULL)
		return [NSArray array];
	else
		return (NSArray *)FromJson(jsonCharArray);
}

const char* ToJsonCString (id object)
{
	
	NSError *error = NULL;
	
	if (object != NULL)
	{
		NSData *jsonData        = [NSJSONSerialization dataWithJSONObject:object
																  options:0
																	error:&error];
		NSString* jsonString    = [[[NSString alloc] initWithData:jsonData
														 encoding:NSUTF8StringEncoding] autorelease];
		
		return [jsonString UTF8String];
	}
	
	return "{}";
}

#pragma mark - Rect operations

CGFloat	GetStatusBarHeight ()
{
	if (![[UIApplication sharedApplication] isStatusBarHidden])
	{
		CGRect	statusBarFrame	= [[UIApplication sharedApplication] statusBarFrame];
		
		// Pre iOS 8, swap width and height
		if (SYSTEM_VERSION_LESS_THAN(@"8.0"))
		{
			return (UIInterfaceOrientationIsPortrait([UIApplication sharedApplication].statusBarOrientation)) ? statusBarFrame.size.height : statusBarFrame.size.width;
		}
		
		return statusBarFrame.size.height;
	}
	
	return 0;
}

CGRect GetScreenBounds ()
{
	CGRect bounds		= [[UIScreen mainScreen] bounds];

	// Pre iOS 8, swap width and height
	if (SYSTEM_VERSION_LESS_THAN(@"8.0"))
	{
		if (UIInterfaceOrientationIsLandscape([UIApplication sharedApplication].statusBarOrientation))
		{
			bounds.size	= CGSizeMake(CGRectGetHeight(bounds), CGRectGetWidth(bounds));
		}
	}
	
	return bounds;
}

CGRect GetApplicationFrame ()
{
	CGRect frame				= GetScreenBounds();
	
	// From iOS 7, application uses transparent status bar
	if (SYSTEM_VERSION_LESS_THAN(@"7.0"))
	{
		CGFloat	statusBarHeight	= GetStatusBarHeight();
		frame.origin.y			+= statusBarHeight;
		frame.size.height		-= statusBarHeight;
	}
	
	return frame;
}

CGRect GetApplicationBounds ()
{
	CGRect bounds	= GetApplicationFrame();
	
	// Reset origin
	bounds.origin.y	= 0;
	
	return bounds;
}

CGRect ConvertToNormalisedRect (CGRect inputFrame)
{
	CGRect applicationFrame		= GetApplicationFrame();

	// Convert frame to normalised rect
	CGRect normalisedRect;
	normalisedRect.origin.x		= (CGRectGetMinX(inputFrame) - CGRectGetMinX(applicationFrame)) / CGRectGetWidth(applicationFrame);
	normalisedRect.origin.y		= (CGRectGetMinY(inputFrame) - CGRectGetMinY(applicationFrame)) / CGRectGetHeight(applicationFrame);
	normalisedRect.size.width	= CGRectGetWidth(inputFrame) / CGRectGetWidth(applicationFrame);
	normalisedRect.size.height	= CGRectGetHeight(inputFrame) / CGRectGetHeight(applicationFrame);
	
	return normalisedRect;
}

CGRect ConvertToApplicationSpace (CGRect normalisedRect)
{
	CGRect applicationFrame		= GetApplicationFrame();
	
	// Calculate frame
	CGRect newFrame;
	newFrame.origin.x			= (CGRectGetMinX(normalisedRect) * CGRectGetWidth(applicationFrame)) + CGRectGetMinX(applicationFrame);
	newFrame.origin.y			= (CGRectGetMinY(normalisedRect) * CGRectGetHeight(applicationFrame)) + CGRectGetMinY(applicationFrame);
	newFrame.size.width			= CGRectGetWidth(normalisedRect) * CGRectGetWidth(applicationFrame);
	newFrame.size.height		= CGRectGetHeight(normalisedRect) * CGRectGetHeight(applicationFrame);
	
	return newFrame;
}

#pragma mark - Device

bool IsIpadInterface ()
{
	return (UI_USER_INTERFACE_IDIOM() == UIUserInterfaceIdiomPad);
}

@implementation Utility

#pragma mark - UIImage

+ (UIImage *)CreateImageFromByteArray:(void*)byteArray ofLength:(int)length
{
    if (length <= 0)
        return NULL;
    else
        return [UIImage imageWithData:[NSData dataWithBytes:(void*)byteArray length:length]];
}

+ (void)GetImageBytes:(NSString *)imageName completion:(ImageCompletionHandler)completion
{
	UIImage *image		= [UIImage imageNamed:imageName];
	NSData *imageData	= UIImageJPEGRepresentation(image, 1);
	
	// Assign value
	if (completion)
		completion([imageData bytes], [imageData length]);
}

#pragma mark - NSDate

+ (NSString *)ConvertNSDateToNSString:(NSDate*)date
{
	if (date == NULL)
		return NULL;
	
	NSDateFormatter *dateFormatter = [[NSDateFormatter alloc] init];
	[dateFormatter setLocale:[[[NSLocale alloc] initWithLocaleIdentifier:@"en_GB"] autorelease]];
	[dateFormatter setTimeZone:[NSTimeZone timeZoneWithAbbreviation:@"UTC"]];
	[dateFormatter setDateFormat:@"yyyy-MM-dd HH:mm:ss Z"];

    return [dateFormatter stringFromDate:date];
}

+ (NSDate *)ConvertNSStringToNSDate:(NSString *)dateString
{
	if (dateString == NULL)
		return NULL;
	
	NSDateFormatter *dateFormatter = [[NSDateFormatter alloc] init];
	[dateFormatter setTimeZone:[NSTimeZone timeZoneWithAbbreviation:@"UTC"]];
	[dateFormatter setDateFormat:@"yyyy-MM-dd HH:mm:ss Z"];
	
	return [dateFormatter dateFromString:dateString];
}

#pragma mark - NSData

+ (NSData *)CreateNSDataFromByteArray:(void*)byteArray ofLength:(int)length
{
    if (length <= 0)
        return NULL;
    else
        return [NSData dataWithBytes:(void*)byteArray length:length];
}

#pragma mark misc

+ (NSString *)GetUUID
{
    CFUUIDRef uuidRef           = CFUUIDCreate(NULL);
    CFStringRef uuidStringRef   = CFUUIDCreateString(NULL, uuidRef);
    CFRelease(uuidRef);
    
	NSLog(@"[Utility] UUID: %@", uuidStringRef);
    return [(NSString *)uuidStringRef autorelease];
}

@end
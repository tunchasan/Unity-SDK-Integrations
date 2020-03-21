//
//  NSData+UIImage.m
//  Cross Platform Native Plugins
//
//  Created by Ashwin kumar on 19/01/15.
//  Copyright (c) 2015 Voxel Busters Interactive LLP. All rights reserved.
//

#import "NSData+UIImage.h"

@implementation NSData (UIImage)

#pragma mark - Save Methods

- (NSString *)saveImage
{
	return [self saveImage:[Utility GetUUID]];
}

- (NSString *)saveImage:(NSString *)imageName
{
	NSString *imageNameWithExt	= NULL;
	
	if ([self isPNG])
		imageNameWithExt 		= [imageName stringByAppendingString:@".png"];
	else if ([self isJPEG])
		imageNameWithExt 		= [imageName stringByAppendingString:@".jpg"];
	else
		return NULL;
	
    // Now, we have to find the documents directory so we can save it
    NSArray *paths              = NSSearchPathForDirectoriesInDomains(NSDocumentDirectory, NSUserDomainMask, YES);
    NSString *documentsDir      = [paths objectAtIndex:0];
    
    // Now we get the full path to the file
    NSString *finalPath         = [documentsDir stringByAppendingPathComponent:imageNameWithExt];
    
    // Write it to disk
    [self writeToFile:finalPath atomically:NO];
	
	return finalPath;
}

#pragma mark - Image Format Methods

- (BOOL)isPNG
{
	uint8_t firstByte;
	
	// Read first byte
	[self getBytes:&firstByte length:1];
	
	if (firstByte == 0x89)
		return YES;
	
	return NO;
}

- (BOOL)isJPEG
{
	uint8_t firstByte;
	
	// Read first byte
	[self getBytes:&firstByte length:1];
	
	if (firstByte == 0xFF)
		return YES;
	
	return NO;
}

@end

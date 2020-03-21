//
//  UIImage+Save.m
//  Cross Platform Native Plugins
//
//  Created by Ashwin kumar on 20/01/15.
//  Copyright (c) 2015 Voxel Busters Interactive LLP. All rights reserved.
//

#import "UIImage+Save.h"
#import "NSData+UIImage.h"
#import "UIImage+Formats.h"
#import "UIImage+Resize.h"

@implementation UIImage (Save)

- (NSString *)saveImageToDocumentsDirectory
{
	return [self saveImageToDocumentsDirectory:[Utility GetUUID]];
}

- (NSString *)saveImageToDocumentsDirectory:(NSString *)imageName
{
	if ([self imageFormat] == UIImageFomatJPEG)
		return [UIImageJPEGRepresentation(self, 1) saveImage:imageName];
	else
		return [UIImagePNGRepresentation(self) saveImage:imageName];
}

@end

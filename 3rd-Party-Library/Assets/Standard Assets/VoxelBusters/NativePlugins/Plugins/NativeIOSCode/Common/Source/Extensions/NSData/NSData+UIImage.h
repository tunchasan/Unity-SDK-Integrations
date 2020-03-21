//
//  NSData+UIImage.h
//  Cross Platform Native Plugins
//
//  Created by Ashwin kumar on 19/01/15.
//  Copyright (c) 2015 Voxel Busters Interactive LLP. All rights reserved.
//

#import <Foundation/Foundation.h>

@interface NSData (UIImage)

// Save methods
- (NSString *)saveImage;
- (NSString *)saveImage:(NSString *)imageName;

// Image format checks
- (BOOL)isPNG;
- (BOOL)isJPEG;

@end

//
//  UIImage+Save.h
//  Cross Platform Native Plugins
//
//  Created by Ashwin kumar on 20/01/15.
//  Copyright (c) 2015 Voxel Busters Interactive LLP. All rights reserved.
//

#import <Foundation/Foundation.h>

@interface UIImage (Save)

// Save methods
- (NSString *)saveImageToDocumentsDirectory;
- (NSString *)saveImageToDocumentsDirectory:(NSString *)imageName;

@end
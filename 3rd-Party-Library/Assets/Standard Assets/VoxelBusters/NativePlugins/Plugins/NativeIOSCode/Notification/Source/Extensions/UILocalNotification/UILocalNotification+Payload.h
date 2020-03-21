//
//  UILocalNotification+Payload.h
//  Cross Platform Native Plugins
//
//  Created by Ashwin kumar on 27/01/15.
//  Copyright (c) 2015 Voxel Busters Interactive LLP. All rights reserved.
//

#import <Foundation/Foundation.h>

@interface UILocalNotification (Payload)

// Static methods
+ (UILocalNotification *)CreateLocalNotification:(NSDictionary *)payload;

// Payload methods
- (id)payload;

@end

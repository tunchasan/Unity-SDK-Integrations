//
//  GKPlayer+Extensions.h
//  Cross Platform Native Plugins
//
//  Created by Ashwin kumar on 04/09/15.
//  Copyright (c) 2015 Voxel Busters Interactive LLP. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <GameKit/GameKit.h>

UIKIT_EXTERN NSString *kGKPlayerPlayerID;
UIKIT_EXTERN NSString *kGKPlayerImagePathKey;

@interface GKPlayer (Serialization)

// JSON method
- (id)toJsonObject;

@end

@interface GKPlayer (Utility)

// Methods
- (NSString *)photoName;

@end

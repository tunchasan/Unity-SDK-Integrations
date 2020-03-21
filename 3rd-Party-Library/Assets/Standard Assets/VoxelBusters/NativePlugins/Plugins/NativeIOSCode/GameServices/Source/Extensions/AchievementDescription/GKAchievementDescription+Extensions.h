//
//  GKAchievementDescription+Extensions.h
//  Cross Platform Native Plugins
//
//  Created by Ashwin kumar on 04/09/15.
//  Copyright (c) 2015 Voxel Busters Interactive LLP. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <GameKit/GameKit.h>

UIKIT_EXTERN NSString *kGKAchievementDescriptionIdentifier;
UIKIT_EXTERN NSString *kGKAchievementDescriptionImagePathKey;

@interface GKAchievementDescription (Serialization)

// JSON methods
- (id)toJsonObject;

@end

@interface GKAchievementDescription (Utility)

// Methods
- (NSString *)imageName;

@end
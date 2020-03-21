//
//  GKAchievement+Extensions.h
//  Cross Platform Native Plugins
//
//  Created by Ashwin kumar on 05/09/15.
//  Copyright (c) 2015 Voxel Busters Interactive LLP. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <GameKit/GameKit.h>

UIKIT_EXTERN NSString *kGKAchievementInfo;
UIKIT_EXTERN NSString *kGKAchievementIdentifier;
UIKIT_EXTERN NSString *kGKAchievementPercentComplete;
UIKIT_EXTERN NSString *kGKAchievementCompleted;
UIKIT_EXTERN NSString *kGKAchievementLastReportedDate;

@interface GKAchievement (Serialization)

// JSON method
- (id)toJsonObject;

@end

//
//  GKLeaderboard+Extensions.h
//  Cross Platform Native Plugins
//
//  Created by Ashwin kumar on 05/09/15.
//  Copyright (c) 2015 Voxel Busters Interactive LLP. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <GameKit/GameKit.h>

UIKIT_EXTERN NSString *kGKLeaderboardInfo;
UIKIT_EXTERN NSString *kGKLeaderboardPlayerScope;
UIKIT_EXTERN NSString *kGKLeaderboardRangeFrom;
UIKIT_EXTERN NSString *kGKLeaderboardRangeLength;
UIKIT_EXTERN NSString *kGKLeaderboardTimeScope;
UIKIT_EXTERN NSString *kGKLeaderboardIdentifier;
UIKIT_EXTERN NSString *kGKLeaderboardImagePath;

@interface GKLeaderboard (Serialization)

// JSON methods
- (id)toJsonObject;

@end

@interface GKLeaderboard (Utility)

// Methods
- (NSString *)getLeaderboardIdentifier;
- (NSString *)imageName;

@end
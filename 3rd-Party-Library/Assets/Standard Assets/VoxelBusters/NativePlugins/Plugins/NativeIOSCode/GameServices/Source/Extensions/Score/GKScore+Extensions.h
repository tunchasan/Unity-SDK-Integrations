//
//  GKScore+Extensions.h
//  Cross Platform Native Plugins
//
//  Created by Ashwin kumar on 05/09/15.
//  Copyright (c) 2015 Voxel Busters Interactive LLP. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <GameKit/GameKit.h>

UIKIT_EXTERN NSString *kGKScoreInfoKey;
UIKIT_EXTERN NSString *kGKScoreLeaderboardIdentifier;

@interface GKScore (Serialization)

// JSON method
- (id)toJsonObject;

@end

@interface GKScore (Utility)

// Methods
- (NSString *)getLeaderboardIdentifier;
- (GKPlayer *)getPlayerInfo;

@end

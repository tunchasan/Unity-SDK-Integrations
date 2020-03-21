//
//  GKScore+Extensions.m
//  Cross Platform Native Plugins
//
//  Created by Ashwin kumar on 05/09/15.
//  Copyright (c) 2015 Voxel Busters Interactive LLP. All rights reserved.
//

#import "GKScore+Extensions.h"
#import "GameServicesHandler.h"
#import	"GKPlayer+Extensions.h"

const NSString *kGKScoreInfoKey						= @"score-info";
const NSString *kGKScoreDate						= @"date";
const NSString *kGKScoreFormattedValue				= @"formatted-value";
const NSString *kGKScoreLeaderboardIdentifier		= @"leaderboard-id";
const NSString *kGKScorePlayer						= @"player";
const NSString *kGKScoreRank						= @"rank";
const NSString *kGKScoreValue						= @"value";

@implementation GKScore (Serialization)

- (id)toJsonObject
{
	NSMutableDictionary *jsonObj 	= [NSMutableDictionary dictionary];
	NSString *leaderboardID			= [self getLeaderboardIdentifier];
	GKPlayer *playerInfo			= [self getPlayerInfo];
	
	// Set properties
	if (self.date)
		[jsonObj setObject:[Utility ConvertNSDateToNSString:self.date] forKey:kGKScoreDate];
	
	if (self.formattedValue)
		[jsonObj setObject:self.formattedValue forKey:kGKScoreFormattedValue];

	if (leaderboardID)
		[jsonObj setObject:leaderboardID forKey:kGKScoreLeaderboardIdentifier];
	
	if (playerInfo)
		[jsonObj setObject:[playerInfo toJsonObject] forKey:kGKScorePlayer];

	[jsonObj setObject:[NSNumber numberWithInteger:self.rank] forKey:kGKScoreRank];
	[jsonObj setObject:[NSNumber numberWithInteger:self.value] forKey:kGKScoreValue];

	return jsonObj;
}

@end


@implementation GKScore (Utility)

- (NSString *)getLeaderboardIdentifier
{
#ifdef __IPHONE_7_0
	if (SYSTEM_VERSION_GREATER_THAN_OR_EQUAL_TO(@"7.0"))
		return self.leaderboardIdentifier;
#endif
	
	return self.category;
}

- (GKPlayer *)getPlayerInfo
{
#ifdef __IPHONE_8_0
	if (SYSTEM_VERSION_GREATER_THAN_OR_EQUAL_TO(@"8.0"))
		return self.player;
#endif
	
	return [[GameServicesHandler Instance] getPlayer:self.playerID];
}

@end

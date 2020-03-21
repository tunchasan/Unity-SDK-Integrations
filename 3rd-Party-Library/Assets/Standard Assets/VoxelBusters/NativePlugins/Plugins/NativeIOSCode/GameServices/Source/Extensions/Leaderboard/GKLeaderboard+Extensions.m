//
//  GKLeaderboard+Extensions.m
//  Cross Platform Native Plugins
//
//  Created by Ashwin kumar on 05/09/15.
//  Copyright (c) 2015 Voxel Busters Interactive LLP. All rights reserved.
//

#import "GKLeaderboard+Extensions.h"
#import "GKScore+Extensions.h"

const NSString *kGKLeaderboardInfo			= @"leaderboard-info";
const NSString *kGKLeaderboardPlayerScope	= @"player-scope";
const NSString *kGKLeaderboardRangeFrom		= @"range-from";
const NSString *kGKLeaderboardRangeLength	= @"range-length";
const NSString *kGKLeaderboardTimeScope		= @"time-scope";
const NSString *kGKLeaderboardIdentifier	= @"id";
const NSString *kGKLeaderboardTitle			= @"title";
const NSString *kGKLeaderboardScores		= @"scores";
const NSString *kGKLeaderboardLocalScore	= @"local-score";
const NSString *kGKLeaderboardImagePath		= @"image-path";

@implementation GKLeaderboard (Serialization)

- (id)toJsonObject
{
	NSMutableDictionary *jsonObj = [NSMutableDictionary dictionary];
	
	// Set properties
	[jsonObj setObject:[NSNumber numberWithInteger:self.playerScope] 		forKey:kGKLeaderboardPlayerScope];
	[jsonObj setObject:[NSNumber numberWithInteger:self.range.location] 	forKey:kGKLeaderboardRangeFrom];
	[jsonObj setObject:[NSNumber numberWithInteger:self.range.length] 		forKey:kGKLeaderboardRangeLength];
	[jsonObj setObject:[NSNumber numberWithInteger:self.timeScope] 			forKey:kGKLeaderboardTimeScope];
	[jsonObj setObject:[self getLeaderboardIdentifier] 						forKey:kGKLeaderboardIdentifier];
	
	if (self.title)
		[jsonObj setObject:self.title forKey:kGKLeaderboardTitle];

	if (self.scores != NULL)
	{
		NSMutableArray *scoreJsonList	= [NSMutableArray array];
		
		for (GKScore *curScore in self.scores)
			[scoreJsonList addObject:[curScore toJsonObject]];
		
		[jsonObj setObject:scoreJsonList forKey:kGKLeaderboardScores];
	}
	
	if (self.localPlayerScore != NULL)
		[jsonObj setObject:[self.localPlayerScore toJsonObject] forKey:kGKLeaderboardLocalScore];
	
	return jsonObj;
}

@end

@implementation GKLeaderboard (Utility)

- (NSString *)getLeaderboardIdentifier
{
#ifdef __IPHONE_7_0
	if (SYSTEM_VERSION_GREATER_THAN_OR_EQUAL_TO(@"7.0"))
		return self.identifier;
#endif
	
	return self.category;
}

- (NSString *)imageName
{
	return [NSString stringWithFormat:@"GC_Leaderboard_%@", self.identifier];
}

@end

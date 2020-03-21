//
//  GKAchievementDescription+Extensions.m
//  Cross Platform Native Plugins
//
//  Created by Ashwin kumar on 04/09/15.
//  Copyright (c) 2015 Voxel Busters Interactive LLP. All rights reserved.
//

#import "GKAchievementDescription+Extensions.h"

const NSString *kGKAchievementDescriptionIdentifier					= @"id";
const NSString *kGKAchievementDescriptionTitle						= @"title";
const NSString *kGKAchievementDescriptionUnachievedDescription		= @"unachieved-desc";
const NSString *kGKAchievementDescriptionAchievedDescription		= @"achieved-desc";
const NSString *kGKAchievementDescriptionMaximumPoints				= @"max-points";
const NSString *kGKAchievementDescriptionHidden						= @"hidden";
const NSString *kGKAchievementDescriptionReplayable					= @"replayable";
const NSString *kGKAchievementDescriptionImagePathKey				= @"image-path";

@implementation GKAchievementDescription (Serialization)

- (id)toJsonObject
{
	NSMutableDictionary *jsonObj = [NSMutableDictionary dictionary];
	
	// Set properties
	[jsonObj setObject:self.identifier 									forKey:kGKAchievementDescriptionIdentifier];

	if (self.title)
		[jsonObj setObject:self.title 									forKey:kGKAchievementDescriptionTitle];
	
	if (self.unachievedDescription)
		[jsonObj setObject:self.unachievedDescription 					forKey:kGKAchievementDescriptionUnachievedDescription];
	
	if (self.achievedDescription)
		[jsonObj setObject:self.achievedDescription 					forKey:kGKAchievementDescriptionAchievedDescription];
	
	[jsonObj setObject:[NSNumber numberWithInteger:self.maximumPoints] 	forKey:kGKAchievementDescriptionMaximumPoints];
	[jsonObj setObject:[NSNumber numberWithBool:self.hidden] 			forKey:kGKAchievementDescriptionHidden];
	[jsonObj setObject:[NSNumber numberWithBool:self.replayable]	 	forKey:kGKAchievementDescriptionReplayable];
	
	return jsonObj;
}

@end

@implementation GKAchievementDescription (Utility)

- (NSString *)imageName
{
	return [NSString stringWithFormat:@"GC_Achievement_%@", self.identifier];
}

@end

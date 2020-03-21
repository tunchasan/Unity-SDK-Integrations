//
//  GKAchievement+Extensions.m
//  Cross Platform Native Plugins
//
//  Created by Ashwin kumar on 05/09/15.
//  Copyright (c) 2015 Voxel Busters Interactive LLP. All rights reserved.
//

#import "GKAchievement+Extensions.h"

const NSString *kGKAchievementInfo						= @"achievement-info";
const NSString *kGKAchievementIdentifier				= @"id";
const NSString *kGKAchievementPercentComplete			= @"percent-complete";
const NSString *kGKAchievementCompleted					= @"completed";
const NSString *kGKAchievementLastReportedDate			= @"last-reported-date";

@implementation GKAchievement (Serialization)

- (id)toJsonObject
{
	NSMutableDictionary *jsonObj = [NSMutableDictionary dictionary];
	
	// Set properties
	[jsonObj setObject:self.identifier									forKey:kGKAchievementIdentifier];
	[jsonObj setObject:[NSNumber numberWithDouble:self.percentComplete] forKey:kGKAchievementPercentComplete];
	[jsonObj setObject:[NSNumber numberWithBool:self.completed] 		forKey:kGKAchievementCompleted];
	
	if (self.lastReportedDate != NULL)
		[jsonObj setObject:[Utility ConvertNSDateToNSString:self.lastReportedDate] 	forKey:kGKAchievementLastReportedDate];
	
	return jsonObj;
}

@end

//
//  GKPlayer+Extensions.m
//  Cross Platform Native Plugins
//
//  Created by Ashwin kumar on 04/09/15.
//  Copyright (c) 2015 Voxel Busters Interactive LLP. All rights reserved.
//

#import "GKPlayer+Extensions.h"

const NSString *kGKPlayerPlayerID		= @"player-id";
const NSString *kGKPlayerAlias			= @"alias";
const NSString *kGKPlayerDisplayName	= @"display-name";
const NSString *kGKPlayerImagePathKey	= @"image-path";

@implementation GKPlayer (Serialization)

- (id)toJsonObject
{
	NSMutableDictionary *jsonObj = [NSMutableDictionary dictionary];
	
	// Set properties
	if (self.playerID)
		[jsonObj setObject:self.playerID forKey:kGKPlayerPlayerID];

	if (self.alias)
		[jsonObj setObject:self.alias forKey:kGKPlayerAlias];
	
	if (self.displayName)
		[jsonObj setObject:self.displayName forKey:kGKPlayerDisplayName];
	
	return jsonObj;
}

@end

@implementation GKPlayer (Utility)

- (NSString *)photoName
{
	return [NSString stringWithFormat:@"GC_Player_%@", self.playerID];
}

@end
//
//  GKLocalPlayer+Extensions.m
//  Cross Platform Native Plugins
//
//  Created by Ashwin kumar on 05/09/15.
//  Copyright (c) 2015 Voxel Busters Interactive LLP. All rights reserved.
//

#import "GKLocalPlayer+Extensions.h"
#import "GKPlayer+Extensions.h"

const NSString *kGKLocalPlayerInfo				= @"player-info";
const NSString *kGKLocalPlayerFriendPlayers		= @"friend-players";
const NSString *kGKLocalPlayerAuthenticated		= @"authenticated";
const NSString *kGKLocalPlayerUnderage			= @"underage";

@implementation GKLocalPlayer (Serialization)

- (id)toJsonObject
{
	NSMutableDictionary *jsonObj = [super toJsonObject];
	
	// Set properties
	[jsonObj setObject:[NSNumber numberWithBool:self.authenticated] forKey:kGKLocalPlayerAuthenticated];
	[jsonObj setObject:[NSNumber numberWithBool:self.underage] 		forKey:kGKLocalPlayerUnderage];
	
	return jsonObj;
}

@end

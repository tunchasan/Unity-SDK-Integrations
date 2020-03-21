//
//  SoomlaGrowHandler.m
//  Cross Platform Native Plugins
//
//  Created by Ashwin kumar on 13/11/15.
//  Copyright (c) 2015 Voxel Busters Interactive LLP. All rights reserved.
//

#import "SoomlaGrowHandler.h"

@implementation SoomlaGrowHandler

#define kHighwayInitialisedEvent 	"OnGrowHighwayInitialised"
#define kHighwayConnectedEvent 		"OnGrowHighwayConnected"
#define kHighwayDisconnectedEvent	"OnGrowHighwayDisconnected"

+ (void)initialise
{
	[[NSNotificationCenter defaultCenter] addObserver:self
											 selector:@selector(onGrowHighwayInitialised:)
												 name:EVENT_HIGHWAY_INITIALIZED
											   object:nil];
	
	[[NSNotificationCenter defaultCenter] addObserver:self
											 selector:@selector(onGrowHighwayConnected:)
												 name:EVENT_HIGHWAY_CONNECTED
											   object:nil];

	[[NSNotificationCenter defaultCenter] addObserver:self
											 selector:@selector(onGrowHighwayDisconnected:)
												 name:EVENT_HIGHWAY_DISCONNECTED
											   object:nil];

}

+ (void)onGrowHighwayInitialised:(NSNotification *)notification
{
	NotifyEventListener(kHighwayInitialisedEvent, "");
}

+ (void)onGrowHighwayConnected:(NSNotification *)notification
{
	NotifyEventListener(kHighwayConnectedEvent, "");
}

+ (void)onGrowHighwayDisconnected:(NSNotification *)notification
{
	NotifyEventListener(kHighwayDisconnectedEvent, "");
}

@end

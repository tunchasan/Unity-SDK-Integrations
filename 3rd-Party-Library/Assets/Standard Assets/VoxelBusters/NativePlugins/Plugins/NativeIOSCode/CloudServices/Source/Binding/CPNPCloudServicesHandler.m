//
//  CPNPCloudServicesHandler.m
//  Cross Platform Native Plugins
//
//  Created by Ashwin kumar on 18/01/16.
//  Copyright (c) 2015 Voxel Busters Interactive LLP. All rights reserved.
//

#import "CPNPCloudServicesHandler.h"

@implementation CPNPCloudServicesHandler

#define kKeyForValueChangedKeys					@"keys"
#define kKeyForChangeReason						@"reason"
#define kKeyValuePairChangedExternallyEvent	 	"CloudKeyValueStoreDidChangeExternally"

- (id)init
{
	self	= [super init];
	
	if (self)
	{
		// Register for events
		NSUbiquitousKeyValueStore *store = [NSUbiquitousKeyValueStore defaultStore];
		
		[[NSNotificationCenter defaultCenter] addObserver:self
												 selector:@selector(keyValueStoreDidChange:)
													 name:NSUbiquitousKeyValueStoreDidChangeExternallyNotification
												   object:store];
	}
	
	return self;
}

- (void)dealloc
{
	// Unregister if already registered
	[[NSNotificationCenter  defaultCenter]  removeObserver:self];
	
	[super dealloc];
}

#pragma mark - Event Callbacks

- (void)keyValueStoreDidChange:(NSNotification *)notification
{
	NSDictionary *userInfo	= [notification userInfo];
	NSNumber *changeReason	= [userInfo objectForKey:NSUbiquitousKeyValueStoreChangeReasonKey];
	
	// Check if value is valid
	if (!changeReason)
		return;
	
	NSArray *changedKeys	= [userInfo objectForKey:NSUbiquitousKeyValueStoreChangedKeysKey];
	
	// Notify Unity
	NSMutableDictionary *dataDict	= [NSMutableDictionary dictionary];
	
	[dataDict setObject:changeReason forKey:kKeyForChangeReason];
	
	if (changedKeys)
		[dataDict setObject:changedKeys forKey:kKeyForValueChangedKeys];
	
	NotifyEventListener(kKeyValuePairChangedExternallyEvent, ToJsonCString(dataDict));
}

@end
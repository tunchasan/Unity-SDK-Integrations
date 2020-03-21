//
//  UILocalNotification+Payload.m
//  Cross Platform Native Plugins
//
//  Created by Ashwin kumar on 27/01/15.
//  Copyright (c) 2015 Voxel Busters Interactive LLP. All rights reserved.
//

#import "UILocalNotification+Payload.h"
#import "NotificationHandler.h"

@implementation UILocalNotification (Payload)

#define kAPS				@"aps"
#define kAlert				@"alert"
#define kBody				@"body"
#define kAction				@"action-loc-key"
#define kHasAction			@"has-action"
#define kLaunchImage		@"launch-image"
#define kFireDate			@"fire-date"
#define kRepeatInterval		@"repeat-interval"
#define kBadge				@"badge"
#define kSound				@"sound"
#define kDefaultSoundName	@"default.mp3"


+ (UILocalNotification *)CreateLocalNotification:(NSDictionary *)payload
{
	NSDictionary *aps						= [payload objectForKey:kAPS];
	UILocalNotification *newNotification	= [[[UILocalNotification alloc] init] autorelease];
	
	if (aps != NULL)
	{
		NSDictionary *alert			= [aps objectForKey:kAlert];
		
		if (alert != NULL)
		{
			NSString *alertBody		= [alert objectForKey:kBody];
			NSString *aleryAction	= [alert objectForKey:kAction];
			NSString *launchImage	= [alert objectForKey:kLaunchImage];
			BOOL hasAction			= [[alert objectForKey:kHasAction] boolValue];

			if (alertBody != NULL)
			{
				[newNotification setAlertBody:alertBody];
			}
			
			if (aleryAction != NULL)
			{
				[newNotification setAlertAction:aleryAction];
			}
			
			if (launchImage != NULL)
			{
				[newNotification setAlertLaunchImage:aleryAction];
			}
			
			[newNotification setHasAction:hasAction];
		}
		
		NSInteger badgeCount		= [[aps objectForKey:kBadge] integerValue];
		NSString *soundName			= [aps objectForKey:kSound];
		
		[newNotification setApplicationIconBadgeNumber:badgeCount];

		if (soundName != NULL)
		{
			if ([soundName isEqualToString:kDefaultSoundName])
			{
				[newNotification setSoundName:UILocalNotificationDefaultSoundName];
			}
			else
			{
				[newNotification setSoundName:soundName];
			}
		}
	}
	
	NSDictionary *userInfo		= [payload objectForKey:[NotificationHandler GetUserInfoKey]];
	NSString *fireDate			= [payload objectForKey:kFireDate];
	NSInteger interval			= [[payload objectForKey:kRepeatInterval] integerValue];

	[newNotification setUserInfo:userInfo];
	[newNotification setFireDate:[Utility ConvertNSStringToNSDate:fireDate]];
	[newNotification setRepeatInterval:(NSCalendarUnit)interval];
	
	return newNotification;
}


- (id)payload
{
	NSMutableDictionary *payload	= [NSMutableDictionary dictionary];
	NSMutableDictionary *aps		= [NSMutableDictionary dictionary];
	
	// Payload: Add aps info
	[payload setObject:aps forKey:kAPS];
	
	// APS: Add alert info
	NSMutableDictionary *alert = [NSMutableDictionary dictionary];

	[aps setObject:alert forKey:kAlert];
	
	if ([self alertBody] != NULL)
	{
		[alert setObject:[self alertBody] forKey:kBody];
	}
	
	if ([self alertAction] != NULL)
	{
		[alert setObject:[self alertAction] forKey:kAction];
	}
	
	if ([self alertLaunchImage] != NULL)
	{
		[alert setObject:[self alertLaunchImage] forKey:kLaunchImage];
	}
	
	[alert setObject:[NSNumber numberWithBool:[self hasAction]] forKey:kHasAction];

	// APS: Add badge and sound info
	[aps setObject:[NSNumber numberWithInteger:[self applicationIconBadgeNumber]] forKey:kBadge];
	
	if ([self soundName] != NULL)
	{
		if ([[self soundName] isEqualToString:UILocalNotificationDefaultSoundName])
		{
			[aps setObject:kDefaultSoundName forKey:kSound];
		}
		else
		{
			[aps setObject:[self soundName] forKey:kSound];
		}
	}
	
	// Payload: Add user info, fire data, repeat interval
	if ([self userInfo] != NULL)
	{
		[payload setObject:[self userInfo] forKey:[NotificationHandler GetUserInfoKey]];
	}
	
	if ([self fireDate] != NULL)
	{
		[payload setObject:[Utility ConvertNSDateToNSString:[self fireDate]]forKey:kFireDate];
	}
	
	[payload setObject:[NSNumber numberWithInt:[self repeatInterval]] forKey:kRepeatInterval];
	
	return payload;
}

@end

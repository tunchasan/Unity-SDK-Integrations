//
//  AppController+Notification.m
//  Cross Platform Native Plugins
//
//  Created by Ashwin kumar on 14/02/15.
//  Copyright (c) 2015 Voxel Busters Interactive LLP. All rights reserved.
//

#import "AppController+Notification.h"

// Local notifications
NSString *kDidLaunchWithLocalNotification			= @"kDidLaunchWithLocalNotification";
NSString *kDidReceiveLocalNotification				= @"kDidReceiveLocalNotification";

// Register notifications
NSString *kDidRegisterForRemoteNotification			= @"kDidRegisterForRemoteNotification";
NSString *kDidFailToRegisterForRemoteNotification	= @"kDidFailToRegisterForRemoteNotification";

// Remote notification
NSString *kDidLaunchWithRemoteNotification			= @"kDidLaunchWithRemoteNotification";
NSString *kDidReceiveRemoteNotification				= @"kDidReceiveRemoteNotification";

#ifdef _USE_APP_CONTROLLER_H_
@implementation AppController (Notification)
#else
@implementation UnityAppController (Notification)
#endif

+ (void)load
{
	[[NSNotificationCenter defaultCenter] addObserver:self
											 selector:@selector(didFinishLaunching:)
												 name:UIApplicationDidFinishLaunchingNotification
											   object:Nil];
}

+ (void)didFinishLaunching:(NSNotification*)notification
{
	NSDictionary *launchOptions	= (NSDictionary *)[notification userInfo];
	
	// Get remote notification
	NSDictionary *remoteNotification = [launchOptions objectForKey:UIApplicationLaunchOptionsRemoteNotificationKey];
	
	if (remoteNotification != NULL)
	{
		[[NSNotificationCenter defaultCenter] postNotificationName:kDidLaunchWithRemoteNotification
															object:nil
														  userInfo:(id)remoteNotification];
	}

	// Get launch local notification
	UILocalNotification *localNotification 	= [launchOptions objectForKey:UIApplicationLaunchOptionsLocalNotificationKey];
	
	if (localNotification != NULL)
	{
		[[NSNotificationCenter defaultCenter] postNotificationName:kDidLaunchWithLocalNotification
															object:nil
														  userInfo:(id)localNotification];
	}
}

- (void)application:(UIApplication*)application didReceiveLocalNotification:(UILocalNotification *)notification
{
	if (notification == NULL)
		return;
	
	// Sends local notification
	[self sendLocalNotification:notification];
}

- (void)application:(UIApplication*)application didRegisterForRemoteNotificationsWithDeviceToken:(NSData *)deviceToken
{
	if (deviceToken == NULL)
		return;
	
	[self sendDeviceToken:deviceToken];
}

- (void)application:(UIApplication*)application didFailToRegisterForRemoteNotificationsWithError:(NSError *)error
{
	if (error == NULL)
		return;
	
	// Send error
	[self sendRemoteNotificationError:error];
}

- (void)application:(UIApplication*)application didReceiveRemoteNotification:(NSDictionary *)userInfo
{
	if (userInfo == NULL)
		return;
	
	// Sends remote notification
	[self sendRemoteNotification:userInfo];
}

#if !UNITY_TVOS
- (void)application:(UIApplication *)application didReceiveRemoteNotification:(NSDictionary *)userInfo fetchCompletionHandler:(void (^)(UIBackgroundFetchResult result))handler
{
	if (userInfo != NULL)
	{
		[self sendRemoteNotification:userInfo];
	}
	
	if (handler)
	{
		handler(UIBackgroundFetchResultNoData);
	}
}
#endif

- (void)sendLocalNotification:(UILocalNotification *)notification
{
	[[NSNotificationCenter defaultCenter] postNotificationName:kDidReceiveLocalNotification
														object:nil
													  userInfo:(id)notification];
	
#ifdef _TRAMPOLINE_UNITY_UNITYINTERFACE_H_
	UnitySendLocalNotification(notification);
#endif
}

- (void)sendDeviceToken:(NSData *)deviceToken
{
	// Get string representation of device token
	NSString *deviceTokenStr 	= [deviceToken description];
	deviceTokenStr 				= [deviceTokenStr stringByTrimmingCharactersInSet:[NSCharacterSet characterSetWithCharactersInString:@"<>"]];
	deviceTokenStr 				= [deviceTokenStr stringByReplacingOccurrencesOfString:@" " withString:@""];
	
	[[NSNotificationCenter defaultCenter] postNotificationName:kDidRegisterForRemoteNotification
														object:nil
													  userInfo:(id)deviceTokenStr];
	
#ifdef _TRAMPOLINE_UNITY_UNITYINTERFACE_H_
	UnitySendDeviceToken(deviceToken);
#endif
}

- (void)sendRemoteNotificationError:(NSError *) error
{
	[[NSNotificationCenter defaultCenter] postNotificationName:kDidFailToRegisterForRemoteNotification
														object:nil
													  userInfo:(id)error];
	
#ifdef _TRAMPOLINE_UNITY_UNITYINTERFACE_H_
	UnitySendRemoteNotificationError(error);
#endif
}

- (void)sendRemoteNotification:(NSDictionary *)userInfo
{
	[[NSNotificationCenter defaultCenter] postNotificationName:kDidReceiveRemoteNotification
														object:nil
													  userInfo:(id)userInfo];
	
#ifdef _TRAMPOLINE_UNITY_UNITYINTERFACE_H_
	UnitySendRemoteNotification(userInfo);
#endif
}

@end

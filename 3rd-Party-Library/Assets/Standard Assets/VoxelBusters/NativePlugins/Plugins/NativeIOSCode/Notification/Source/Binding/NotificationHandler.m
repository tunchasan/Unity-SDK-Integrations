//
//  NotificationHandler.m
//  Cross Platform Native Plugins
//
//  Created by Ashwin kumar on 08/01/15.
//  Copyright (c) 2015 Voxel Busters Interactive LLP. All rights reserved.
//

#import "NotificationHandler.h"
#import "UILocalNotification+Payload.h"
#import "AppController+Notification.h"

static NSString		*userInfoKey					= NULL;
static BOOL			canCaptureLocalNotifications	= YES;
static BOOL			canCaptureRemoteNotifications	= YES;

@interface NotificationHandler ()

@property(nonatomic)	BOOL	canSendNotifications;

@end

// Properties
@implementation NotificationHandler

#define kDidReceiveLocalNotificationEvent						"DidReceiveLocalNotification"
#define kDidReceiveRemoteNotificationEvent						"DidReceiveRemoteNotification"
#define kDidRegisterRemoteNotificationEvent						"DidRegisterRemoteNotification"
#define kDidFailRemoteNotificationRegistrationWithErrorEvent	"DidFailToRegisterRemoteNotifications"

#define kIsLaunchNotificationKey								@"is-launch-notification"
#define kPayloadKey												@"payload"

@synthesize launchLocalNotification;
@synthesize launchRemoteNotification;
@synthesize supportedNotificationTypes;

+ (void)load
{
	id instance = [self Instance];
	
	// Add observer for notification callbacks
	[[NSNotificationCenter defaultCenter] addObserver:instance
											 selector:@selector(didRegisterForRemoteNotificationsWithDeviceToken:)
												 name:kDidRegisterForRemoteNotification
											   object:Nil];
	
	[[NSNotificationCenter defaultCenter] addObserver:instance
											 selector:@selector(didFailToRegisterForRemoteNotificationsWithError:)
												 name:kDidFailToRegisterForRemoteNotification
											   object:Nil];
	
	[[NSNotificationCenter defaultCenter] addObserver:instance
											 selector:@selector(didLaunchWithLocalNotification:)
												 name:kDidLaunchWithLocalNotification
											   object:Nil];
	
	[[NSNotificationCenter defaultCenter] addObserver:instance
											 selector:@selector(didReceiveLocalNotification:)
												 name:kDidReceiveLocalNotification
											   object:Nil];
	
	[[NSNotificationCenter defaultCenter] addObserver:instance
											 selector:@selector(didLaunchWithRemoteNotification:)
												 name:kDidLaunchWithRemoteNotification
											   object:Nil];
	
	[[NSNotificationCenter defaultCenter] addObserver:instance
											 selector:@selector(didReceiveRemoteNotification:)
												 name:kDidReceiveRemoteNotification
											   object:Nil];
}

+ (NSString *)GetUserInfoKey
{
	return userInfoKey;
}

+ (void)SetUserInfoKey:(NSString *)value
{
	NSString *oldValue	= userInfoKey;
	
	// Set new value
	userInfoKey		= [value retain];
	
	// Release old value
	[oldValue release], oldValue = NULL;
}

+ (BOOL)GetCanCaptureLocalNotifications
{
	return canCaptureLocalNotifications;
}

+ (void)SetCanCaptureLocalNotifications:(BOOL)value
{
	canCaptureLocalNotifications	= value;
}

+ (BOOL)GetCanCaptureRemoteNotifications
{
	return canCaptureRemoteNotifications;
}

+ (void)SetCanCaptureRemoteNotifications:(BOOL)value
{
	canCaptureRemoteNotifications	= value;
}

#pragma mark - Lifecycle Methods

- (id)init
{
	self	= [super init];
	
	if (self)
	{
		self.launchLocalNotification	= NULL;
		self.launchRemoteNotification	= NULL;
		self.canSendNotifications		= false;
	}

	return  self;
}

- (void)dealloc
{
	// Remove observer
	[[NSNotificationCenter defaultCenter] removeObserver:self];
	
	// Release
	self.launchLocalNotification		= NULL;
	self.launchRemoteNotification		= NULL;
	[userInfoKey release], userInfoKey 	= NULL;
	
	// Release
	[super dealloc];
}

#pragma mark - Unity Callback

- (void)didFinishUnityComponentInitialisation
{
	// Can send notifications
	[self setCanSendNotifications:YES];
	
	// Now we are ready to push launch notifications to Unity
	if (self.launchLocalNotification != NULL)
	{
		[self sendLocalNotificationToUnity:self.launchLocalNotification isLaunchNotification:YES];
		
		// Release
		self.launchLocalNotification	= nil;
	}
	
	if (self.launchRemoteNotification != NULL)
	{
		[self sendRemoteNotificationToUnity:self.launchRemoteNotification isLaunchNotification:YES];
		
		// Release
		self.launchRemoteNotification	= nil;
	}
}

#pragma mark - Notification Type Methods

- (int)enabledNotificationTypes
{
#ifdef __IPHONE_8_0
	if (SYSTEM_VERSION_GREATER_THAN_OR_EQUAL_TO(@"8.0"))
	{
		return [[[UIApplication sharedApplication] currentUserNotificationSettings] types];
	}
	else
#endif
	{
		return [[UIApplication sharedApplication] enabledRemoteNotificationTypes];
	}
}

- (void)registerNotificationTypes:(int)notificationTypes
{
	self.supportedNotificationTypes						= notificationTypes;
	
	// ios 8+ feature
#ifdef __IPHONE_8_0
	if (SYSTEM_VERSION_GREATER_THAN_OR_EQUAL_TO(@"8.0"))
	{
		UIUserNotificationType userNotificationTypes	= (UIUserNotificationType)supportedNotificationTypes;
		UIUserNotificationSettings* mySettings	 		= [UIUserNotificationSettings settingsForTypes:userNotificationTypes categories:nil];
		
		// Settings are set
		[[UIApplication sharedApplication] registerUserNotificationSettings:mySettings];
	}
#endif
}

#pragma mark - Local Notification Methods

- (void)scheduleLocalNotification:(NSDictionary *)payload
{
	UILocalNotification *newNotification	= [UILocalNotification CreateLocalNotification:payload];
	
	[[UIApplication sharedApplication] scheduleLocalNotification:newNotification];
}

- (void)cancelLocalNotification:(NSString *)notificationID
{
	NSArray *scheduledNotifications			= [[UIApplication sharedApplication] scheduledLocalNotifications];
	
	for (UILocalNotification *currentNotification in scheduledNotifications)
	{
		NSDictionary *userInfo				= currentNotification.userInfo;
		NSString *scheduledNotificationID	= [userInfo objectForKey:@"np-notification-identifier"];
		
		if (scheduledNotificationID != nil && [scheduledNotificationID isEqualToString:notificationID])
		{
			[[UIApplication sharedApplication] cancelLocalNotification:currentNotification];
			break;
		}
	}
}

- (void)cancelAllLocalNotifications
{
	[[UIApplication sharedApplication] cancelAllLocalNotifications];
}

- (void)clearNotifications
{
	NSArray *scheduledNotifications	= [[UIApplication sharedApplication] scheduledLocalNotifications];
	
	// Clear notifications
	[[UIApplication sharedApplication] setApplicationIconBadgeNumber:0];
	[[UIApplication sharedApplication] cancelAllLocalNotifications];
	
	// Reschedule old notifications
	for (UILocalNotification *curNotification in scheduledNotifications)
	{
		UILocalNotification *newCopy	= [curNotification copy];
		
		[[UIApplication sharedApplication] scheduleLocalNotification:newCopy];
		
		// Release
		[newCopy release], newCopy = NULL;
	}
	
	NSLog(@"count %d", [[[UIApplication sharedApplication] scheduledLocalNotifications] count]);
}

#pragma mark - Remote Notification Methods

- (void)registerForRemoteNotifications
{
	// ios 8+ feature
#ifdef __IPHONE_8_0
	if (SYSTEM_VERSION_GREATER_THAN_OR_EQUAL_TO(@"8.0"))
	{
		[[UIApplication sharedApplication] registerForRemoteNotifications];
	}
	else
#endif
	{
		[[UIApplication sharedApplication] registerForRemoteNotificationTypes:(UIRemoteNotificationType)self.supportedNotificationTypes];
	}
}

- (void)unregisterForRemoteNotifications
{
	// Unregister
	[[UIApplication sharedApplication] unregisterForRemoteNotifications];
}

#pragma mark - Application Callback Methods

- (void)didRegisterForRemoteNotificationsWithDeviceToken:(NSNotification *)notification
{
	NSString *deviceToken	= (NSString *)[notification userInfo];
	
	// Notify unity
	NotifyEventListener(kDidRegisterRemoteNotificationEvent, [deviceToken UTF8String]);
}

- (void)didFailToRegisterForRemoteNotificationsWithError:(NSNotification *)notification
{
	NSError *error			= (NSError *)[notification userInfo];
	
	// Notify unity
	NotifyEventListener(kDidFailRemoteNotificationRegistrationWithErrorEvent, [[error description] UTF8String]);
}

- (void)didLaunchWithLocalNotification:(NSNotification *)notification
{
	UILocalNotification *localNotification	= (UILocalNotification *)[notification userInfo];
	
	// Cache value
	self.launchLocalNotification			= localNotification;
}

- (void)didReceiveLocalNotification:(NSNotification *)notification
{
	UILocalNotification *localNotification		= (UILocalNotification *)[notification userInfo];
	BOOL				isLaunchNotification	= [[UIApplication sharedApplication] applicationState] != UIApplicationStateActive;

	// If launch notification is same as current then ignore
	if (localNotification == self.launchLocalNotification)
		return;

	// Invoke handler to send message to Unity
	[self sendLocalNotificationToUnity:localNotification isLaunchNotification:isLaunchNotification];
}

- (void)didLaunchWithRemoteNotification:(NSNotification *)notification
{
	NSDictionary 	*payload		= (NSDictionary *)[notification userInfo];
	
	// Cache value
	self.launchRemoteNotification	= payload;
}

- (void)didReceiveRemoteNotification:(NSNotification *)notification
{
	NSDictionary 		*payload				= (NSDictionary *)[notification userInfo];
	BOOL				isLaunchNotification	= [[UIApplication sharedApplication] applicationState] != UIApplicationStateActive;
	
	// Invoke handler to send message to Unity
	[self sendRemoteNotificationToUnity:payload isLaunchNotification:isLaunchNotification];
}

#pragma mark - Send Methods

- (void)sendLocalNotificationToUnity:(UILocalNotification *)notification isLaunchNotification:(BOOL)isLaunchNotification
{
	if (notification == NULL)
		return;
	
	if (!canCaptureLocalNotifications)
		return;
	
	if (!self.canSendNotifications)
		return;
	
	// Notify Unity
	NSMutableDictionary *dataDict		= [NSMutableDictionary dictionary];
	
	[dataDict setObject:[notification payload] forKey:kPayloadKey];
	[dataDict setObject:[NSNumber numberWithBool:isLaunchNotification] forKey:kIsLaunchNotificationKey];
	
	NotifyEventListener(kDidReceiveLocalNotificationEvent, ToJsonCString(dataDict));
}

- (void)sendRemoteNotificationToUnity:(NSDictionary *)payload isLaunchNotification:(BOOL)isLaunchNotification
{
	if (payload == NULL)
		return;
	
	if (!canCaptureRemoteNotifications)
		return;
	
	if (!self.canSendNotifications)
		return;
	
	// Notify Unity
	NSMutableDictionary *dataDict		= [NSMutableDictionary dictionary];
	
	[dataDict setObject:payload forKey:kPayloadKey];
	[dataDict setObject:[NSNumber numberWithBool:isLaunchNotification] forKey:kIsLaunchNotificationKey];

	NotifyEventListener(kDidReceiveRemoteNotificationEvent, ToJsonCString(dataDict));
}

@end
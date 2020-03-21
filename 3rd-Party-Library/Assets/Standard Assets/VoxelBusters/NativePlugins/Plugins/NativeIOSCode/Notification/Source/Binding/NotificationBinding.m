//
//  NotificationBinding.m
//  Cross Platform Native Plugins
//
//  Created by Ashwin kumar on 11/01/15.
//  Copyright (c) 2015 Voxel Busters Interactive LLP. All rights reserved.
//

#import "NotificationBinding.h"
#import "NotificationHandler.h"

#pragma mark - Init Methods

void initNotificationService (bool captureLocalNotifications, bool captureRemoteNotifications, const char* userInfoKey)
{
	// Update static properties
	[NotificationHandler SetUserInfoKey:ConvertToNSString(userInfoKey)];
	[NotificationHandler SetCanCaptureLocalNotifications:captureLocalNotifications];
	[NotificationHandler SetCanCaptureRemoteNotifications:captureRemoteNotifications];
	
	// Notify
	[[NotificationHandler Instance] didFinishUnityComponentInitialisation];
}

#pragma mark - Notification Type Methods

int enabledNotificationTypes ()
{
	return [[NotificationHandler Instance] enabledNotificationTypes];
}

void registerNotificationTypes (int notificationTypes)
{
	[[NotificationHandler Instance] registerNotificationTypes:notificationTypes];
}

#pragma mark - Local Notification Methods

void scheduleLocalNotification (const char* payloadJSONString)
{
	[[NotificationHandler Instance] scheduleLocalNotification:FromJson(payloadJSONString)];
}

void cancelLocalNotification (const char* notificationID)
{
	[[NotificationHandler Instance] cancelLocalNotification:ConvertToNSString(notificationID)];
}

void cancelAllLocalNotifications ()
{
	[[NotificationHandler Instance] cancelAllLocalNotifications];
}

void clearNotifications ()
{
	[[NotificationHandler Instance] clearNotifications];
}

#pragma mark - Remote Notification Methods

void registerForRemoteNotifications ()
{
	[[NotificationHandler Instance] registerForRemoteNotifications];
}

void unregisterForRemoteNotifications ()
{
	[[NotificationHandler Instance] unregisterForRemoteNotifications];
}
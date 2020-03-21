//
//  NotificationBinding.h
//  Cross Platform Native Plugins
//
//  Created by Ashwin kumar on 11/01/15.
//  Copyright (c) 2015 Voxel Busters Interactive LLP. All rights reserved.
//

#import <Foundation/Foundation.h>

// Init methods
UIKIT_EXTERN void initNotificationService (bool captureLocalNotifications, bool captureRemoteNotifications, const char* keyForUserInfo);

// Notification type methods
UIKIT_EXTERN void registerNotificationTypes (int notificationTypes);
UIKIT_EXTERN int enabledNotificationTypes ();

// Local notification methods
UIKIT_EXTERN void scheduleLocalNotification (const char* payloadJSONString);
UIKIT_EXTERN void cancelLocalNotification (const char* notificationID);
UIKIT_EXTERN void cancelAllLocalNotifications ();
UIKIT_EXTERN void clearNotifications ();

// Remote notification methods
UIKIT_EXTERN void registerForRemoteNotifications ();
UIKIT_EXTERN void unregisterForRemoteNotifications ();
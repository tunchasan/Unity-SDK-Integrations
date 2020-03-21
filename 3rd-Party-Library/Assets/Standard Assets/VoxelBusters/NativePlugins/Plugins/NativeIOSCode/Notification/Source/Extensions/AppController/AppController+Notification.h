//
//  AppController+Notification.h
//  Cross Platform Native Plugins
//
//  Created by Ashwin kumar on 14/02/15.
//  Copyright (c) 2015 Voxel Busters Interactive LLP. All rights reserved.
//

#import <Foundation/Foundation.h>

// Local notifications
UIKIT_EXTERN NSString *kDidLaunchWithLocalNotification;
UIKIT_EXTERN NSString *kDidReceiveLocalNotification;

// Register notifications
UIKIT_EXTERN NSString *kDidRegisterForRemoteNotification;
UIKIT_EXTERN NSString *kDidFailToRegisterForRemoteNotification;

// Remote notification
UIKIT_EXTERN NSString *kDidLaunchWithRemoteNotification;
UIKIT_EXTERN NSString *kDidReceiveRemoteNotification;

// This is unity build
#ifdef UNITY_VERSION 
#if UNITY_VERSION < 420
	#define _USE_APP_CONTROLLER_H_
#endif
// This is iOS workspace
#else
#define _USE_APP_CONTROLLER_H_
#endif

#ifdef _USE_APP_CONTROLLER_H_
#import "AppController.h"
@interface AppController (Notification)
#else
#import "UnityAppController.h"
@interface UnityAppController (Notification)
#endif

@end

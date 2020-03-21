//
//  TwitterBinding.h
//  NativePluginIOSWorkspace
//
//  Created by Ashwin kumar on 11/01/15.
//  Copyright (c) 2015 Voxel Busters Interactive LLP. All rights reserved.
//

#import <Foundation/Foundation.h>

// Init
UIKIT_EXTERN void cpnpTwitterInitTwitterKit (const char* consumerKey, const char* consumerSecret);

// Account
UIKIT_EXTERN void cpnpTwitterLogin (bool requiresEmailAccess);
UIKIT_EXTERN void cpnpTwitterLogoutUserID (const char* userID);
UIKIT_EXTERN bool cpnpTwitterIsUserLoggedIn (const char* userID);

UIKIT_EXTERN char* cpnpTwitterGetSessionDictionaryWithUserID (const char* userID);

// Tweet
UIKIT_EXTERN void cpnpTwitterShowTweetComposer (const char* message, 	const char* URLString,
												UInt8* imgByteArray, 	int imgByteArrayLength);

// Requests
UIKIT_EXTERN void cpnpTwitterRequestAccountDetailsWithUserID (const char* userID);
UIKIT_EXTERN void cpnpTwitterRequestEmailWithUserID (const char* userID);
UIKIT_EXTERN void cpnpTwitterSendURLRequest (const char* userID,		const char* methodType,
											 const char* URLString,		const char* parameters);
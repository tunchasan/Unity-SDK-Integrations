//
//  TwitterBinding.m
//  NativePluginIOSWorkspace
//
//  Created by Ashwin kumar on 11/01/15.
//  Copyright (c) 2015 Voxel Busters Interactive LLP. All rights reserved.
//

#import "TwitterBinding.h"
#import "TwitterHandler.h"

#pragma mark - init

void cpnpTwitterInitTwitterKit (const char* consumerKey, const char* consumerSecret)
{
    [TwitterHandler InitTwitterKitWithConsumerKey:ConvertToNSString(consumerKey)
								   consumerSecret:ConvertToNSString(consumerSecret)];
}

#pragma mark - Session Methods

void cpnpTwitterLogin (bool requiresEmailAccess)
{
	[[TwitterHandler Instance] login:requiresEmailAccess];
}

void cpnpTwitterLogoutUserID (const char* userID)
{
    [[TwitterHandler Instance] logoutUserID:ConvertToNSString(userID)];
}

bool cpnpTwitterIsUserLoggedIn (const char* userID)
{
    return [[TwitterHandler Instance] isUserLoggedIn:ConvertToNSString(userID)];
}

char* cpnpTwitterGetSessionDictionaryWithUserID (const char* userID)
{
	NSDictionary *sessionDict	= [[TwitterHandler Instance] getSessionDictionaryWithUserID:ConvertToNSString(userID)];

	if (sessionDict)
		return CStringCopy(ToJsonCString(sessionDict));
		
	return NULL;
}

#pragma mark - Tweet methods

void cpnpTwitterShowTweetComposer (const char* message, const char* URLString, UInt8* imgByteArray, int imgByteArrayLength)
{
    // Show tweet compose
    [[TwitterHandler Instance] showTweetComposerWithMessage:ConvertToNSString(message)
                                                        URL:ConvertToNSString(URLString)
                                                      image:[Utility CreateImageFromByteArray:(void*)imgByteArray
                                                                                     ofLength:imgByteArrayLength]];
}

#pragma mark - Request Methods

void cpnpTwitterRequestAccountDetailsWithUserID(const char* userID)
{
    [[TwitterHandler Instance] requestAccountDetailsWithUserID:ConvertToNSString(userID)];
}

void cpnpTwitterRequestEmailWithUserID (const char* userID)
{
    [[TwitterHandler Instance] requestEmailWithUserID:ConvertToNSString(userID)];
}

void cpnpTwitterSendURLRequest (const char* userID,		const char* methodType,
								const char* URLString,	const char* parameters)
{
    [[TwitterHandler Instance] sendURLRequestUsingClientWithUserID:ConvertToNSString(userID)
															method:ConvertToNSString(methodType)
															   URL:ConvertToNSString(URLString)
														parameters:FromJson(parameters)];
}

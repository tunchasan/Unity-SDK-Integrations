//
//  TWTRSession+Additions.m
//  NativePluginIOSWorkspace
//
//  Created by Ashwin kumar on 11/01/15.
//  Copyright (c) 2015 Voxel Busters Interactive LLP. All rights reserved.
//

#import "TWTRSession+Additions.h"

@implementation TWTRSession (Additions)

#define kUserID				@"user-ID"
#define kAuthToken			@"auth-token"
#define kUserName			@"user-name"
#define kAuthTokenSecret	@"auth-token-secret"

- (id)toJsonObject
{
    NSMutableDictionary *sessionJsonObject	= [NSMutableDictionary dictionary];
	sessionJsonObject[kAuthToken]			= [self authToken];
	sessionJsonObject[kAuthTokenSecret]		= [self authTokenSecret];
	sessionJsonObject[kUserName]			= [self userName];
	sessionJsonObject[kUserID]				= [self userID];
	
	return sessionJsonObject;
}
@end

//
//  TWTRUser+Additions.m
//  NativePluginIOSWorkspace
//
//  Created by Ashwin kumar on 11/01/15.
//  Copyright (c) 2015 Voxel Busters Interactive LLP. All rights reserved.
//

#import "TWTRUser+Additions.h"

@implementation TWTRUser (Additions)

#define kIsVerified				@"is-verified"
#define kUserID					@"user-ID"
#define kName					@"name"
#define kProfileImageURL		@"profile-image-URL"
#define kIsProtected			@"is-protected"

- (id)toJsonObject
{
	NSMutableDictionary *userDetailsJsonDict	= [NSMutableDictionary dictionary];
	userDetailsJsonDict[kUserID]         		= [self userID];
	userDetailsJsonDict[kName]             		= [self name];
	userDetailsJsonDict[kIsVerified]       		= [NSNumber numberWithBool:[self isVerified]];
	userDetailsJsonDict[kIsProtected]        	= [NSNumber numberWithBool:[self isProtected]];
	userDetailsJsonDict[kProfileImageURL]		= [self profileImageURL];
	
	return userDetailsJsonDict;
}

@end

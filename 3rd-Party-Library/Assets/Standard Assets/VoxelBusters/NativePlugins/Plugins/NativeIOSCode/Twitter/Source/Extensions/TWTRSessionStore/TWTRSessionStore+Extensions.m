//
//  TWTRSessionStore+Extensions.m
//  Unity-iPhone
//
//  Created by Ashwin kumar on 01/06/16.
//
//

#import "TWTRSessionStore+Extensions.h"

@implementation TWTRSessionStore (Extensions)

#define kAuthToken			@"auth-token"
#define kAuthTokenSecret	@"auth-token-secret"
#define kUserID				@"user-ID"

- (NSDictionary *)getSessionDictionaryWithUserID:(NSString *)userID
{
	id<TWTRAuthSession> session			= [self sessionForUserID:userID];
	NSMutableDictionary *sessionDict	= NULL;
	
	if (session)
	{
		sessionDict						= [NSMutableDictionary dictionary];
		sessionDict[kAuthToken]			= [session authToken];
		sessionDict[kAuthTokenSecret]	= [session authTokenSecret];
		sessionDict[kUserID]			= [session userID];
	}
	
	return sessionDict;
}

@end

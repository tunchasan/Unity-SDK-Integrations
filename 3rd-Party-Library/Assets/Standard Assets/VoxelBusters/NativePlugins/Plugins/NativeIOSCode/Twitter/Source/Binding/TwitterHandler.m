//
//  TwitterHandler.m
//  Unity-iPhone
//
//  Created by Ashwin kumar on 27/12/14.
//  Copyright (c) 2015 Voxel Busters Interactive LLP. All rights reserved.
//

#import "TwitterHandler.h"
#import "TWTRSession+Additions.h"
#import "TWTRUser+Additions.h"
#import "TWTRSessionStore+Extensions.h"

@implementation TwitterHandler

#define kTWTRLoginSuccess				"TwitterLoginSuccess"
#define kTWTRLoginFailed				"TwitterLoginFailed"
#define kDismissedTweetComposer   		"TweetComposerDismissed"
#define kRequestAccountDetailsSuccess	"RequestAccountDetailsSuccess"
#define kRequestAccountDetailsFailed  	"RequestAccountDetailsFailed"
#define kEmailAccesSuccess    			"RequestEmailAccessSuccess"
#define kEmailAccessFailed      		"RequestEmailAccessFailed"
#define kTwitterURLRequestSuccess     	"TwitterURLRequestSuccess"
#define kTwitterURLRequestFailed      	"TwitterURLRequestFailed"

#define TwitterKit [Twitter sharedInstance]

#pragma mark - Init

+ (void)InitTwitterKitWithConsumerKey:(NSString *)consumerKey consumerSecret:(NSString *)consumerSecret
{
    NSLog(@"[TwitterHandler] initialising with consumer key %@ secret key %@", consumerKey, consumerSecret);
	
	if (consumerKey == NULL || consumerSecret == NULL)
	{
		NSLog(@"[TwitterHandler] failed initialising twitterkit");
		return;
	}
	
    [TwitterKit startWithConsumerKey:consumerKey consumerSecret:consumerSecret];
    [Fabric with:@[TwitterKit]];
}

#pragma mark - Login

- (void)login:(BOOL)requiresEmailAccess
{
    NSLog(@"[TwitterHandler] login request received.");
    
	TWTRLoginMethod methods	= requiresEmailAccess ? TWTRLoginMethodWebBased : TWTRLoginMethodAll;
	[TwitterKit logInWithMethods:methods completion:^(TWTRSession *session, NSError *error) {

        // Check for errors
        if (error == NULL)
		{
			NotifyEventListener(kTWTRLoginSuccess, ToJsonCString([session toJsonObject]));
		}
		else
		{
			[self notifyError:kTWTRLoginFailed withError:error];
		}
     }];
}

- (void)logoutUserID:(NSString *)userID
{
	NSLog(@"[TwitterHandler] logged out.");
	
	[[TwitterKit sessionStore] logOutUserID:userID];
}

- (BOOL)isUserLoggedIn:(NSString *)userID
{
	return [[TwitterKit sessionStore] sessionForUserID:userID] ? true : false;
}

- (NSDictionary *)getSessionDictionaryWithUserID:(NSString *)userID
{
	return [[TwitterKit sessionStore] getSessionDictionaryWithUserID:userID];
}

#pragma mark - Tweet

- (void)showTweetComposerWithMessage:(NSString *)message URL:(NSString *)URLString image:(UIImage *)image
{
    TWTRComposer *composer = [[[TWTRComposer alloc] init] autorelease];
    
	if (message)
		[composer setText:message];
    
    if (URLString)
        [composer setURL:[NSURL URLWithString:URLString]];
    
    if (image)
        [composer setImage:image];
    
    // Display the composer
	[composer showFromViewController:UnityGetGLViewController() completion:^(TWTRComposerResult result) {
        NSString *resultStr = [NSString stringWithFormat:@"%d", result];
        NSLog(@"[TwitterHandler] tweet composer dismissed, ressult %@", resultStr);
        
        // Notify
        NotifyEventListener(kDismissedTweetComposer, [resultStr UTF8String]);
    }];
}

#pragma mark - Advanced

- (void)requestAccountDetailsWithUserID:(NSString *)userID
{
    NSLog(@"[TwitterHandler] requesting user details");
	
	// Check if user is logged in
	if (![self isUserLoggedIn:userID])
	{
		// Notify
		[self notifyError:kRequestAccountDetailsFailed withError:[self createNoAuthError]];
		return;
	}
	
	// Send a request
	TWTRAPIClient 	*client	 	= [[[TWTRAPIClient alloc] initWithUserID:userID] autorelease];
    [client loadUserWithID:userID completion:^(TWTRUser *user, NSError *error) {
		// Check for errors
        if (error == NULL)
        {
            NotifyEventListener(kRequestAccountDetailsSuccess, ToJsonCString([user toJsonObject]));
        }
        else
        {
			[self notifyError:kRequestAccountDetailsFailed withError:error];
        }
    }];
}

- (void)requestEmailWithUserID:(NSString *)userID
{
	// Check if user is logged in
	if (![self isUserLoggedIn:userID])
	{
		// Notify
		[self notifyError:kEmailAccessFailed withError:[self createNoAuthError]];
		return;
	}
	
	// Create a request
	TWTRAPIClient *client = [[[TWTRAPIClient alloc] initWithUserID:userID] autorelease];
	NSURLRequest *request = [client URLRequestWithMethod:@"GET"
													 URL:@"https://api.twitter.com/1.1/account/verify_credentials.json"
											  parameters:@{@"include_email": @"true", @"skip_status": @"true"}
												   error:nil];
	
	[client sendTwitterRequest:request completion:^(NSURLResponse *response, NSData *data, NSError *connectionError) {
		
		// Parse response
		if (connectionError == NULL)
		{
			NSError *jsonError	= NULL;
			NSDictionary *json 	= [NSJSONSerialization JSONObjectWithData:data
																  options:0
																	error:&jsonError];
			
			NSString *email		= [json objectForKey:@"email"];
			NotifyEventListener(kEmailAccesSuccess, email ? [email UTF8String] : kCStringEmpty);
		}
		else
		{
			[self notifyError:kEmailAccessFailed withError:connectionError];
		}

	}];
}

- (void)sendURLRequestUsingClientWithUserID:(NSString *)userID method:(NSString *)method URL:(NSString *)URLString parameters:(NSDictionary *)parameters
{
	// Check if user is logged in
	if (![self isUserLoggedIn:userID])
	{
		// Notify
		[self notifyError:kTwitterURLRequestFailed withError:[self createNoAuthError]];
		return;
	}
	
	TWTRAPIClient 	*client	 	= [[[TWTRAPIClient alloc] initWithUserID:userID] autorelease];
    NSError         *clientError;
	
    NSURLRequest    *request	= [client URLRequestWithMethod:method
														URL:URLString
												 parameters:parameters
													  error:&clientError];
    if (request)
    {
        [client sendTwitterRequest:request
						completion:^(NSURLResponse *response, NSData *data, NSError *connectionError) {
            // Parse response
            if (data)
            {
                NSError *jsonError	= NULL;
                NSDictionary *json 	= [NSJSONSerialization JSONObjectWithData:data
																	  options:0
																		error:&jsonError];
                
                NotifyEventListener(kTwitterURLRequestSuccess, ToJsonCString(json));
            }
            else
            {
                [self notifyError:kTwitterURLRequestFailed withError:connectionError];
            }
        }];
    }
    else
    {
		[self notifyError:kTwitterURLRequestFailed withError:clientError];
    }
}

#pragma mark - Misc

- (void)notifyError:(const char *)methodName withError:(NSError *)error
{
	NSString *errorDescription	= error ? [error description] : kNSStringDefault;
	
	// Notify
	NotifyEventListener(methodName, [errorDescription UTF8String]);
}

- (NSError*)createNoAuthError
{
	NSError* noAuthError	= [[[NSError alloc] initWithDomain:@"TWTRErrorDomain"
													   code:TWTRErrorCodeNoAuthentication
												   userInfo:@{ NSLocalizedDescriptionKey : @"Auth token is missing. Please login before calling this API"}] autorelease];
	
	return noAuthError;
}

@end

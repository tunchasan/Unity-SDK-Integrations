//
//  SocialShare.m
//  Cross Platform Native Plugins
//
//  Created by Ashwin kumar on 09/09/15.
//  Copyright (c) 2015 Voxel Busters Interactive LLP. All rights reserved.
//

#import "SocialShare.h"

@implementation SocialShare

#define kSocialShareFinished     "SocialShareFinished"

static NSString *const kFacebookURLScheme   = @"fb://";
static NSString *const kTwitterURLScheme    = @"twitter://";

#pragma mark - Methods

- (NSString *)getServiceName:(SocialShareServiceType)serviceType
{
	if (serviceType == SocialShareServiceTypeFacebook)
	{
		return SLServiceTypeFacebook;
	}
	else if (serviceType == SocialShareServiceTypeTwitter)
	{
		return SLServiceTypeTwitter;
	}

	return NULL;
}

- (BOOL)isServiceTypeAvailable:(SocialShareServiceType)serviceType;
{
    if (SYSTEM_VERSION_GREATER_THAN_OR_EQUAL_TO(@"11.0"))
    {
        return [self checkWhetherServiceIsAvailable:serviceType];
    }
    else
    {
        NSString *serviceName    = [self getServiceName:serviceType];
        if (serviceName != NULL)
            return [SLComposeViewController isAvailableForServiceType:serviceName];

        return false;
    }
}

- (void)share:(SocialShareServiceType)serviceType
  withMessage:(NSString *)message
	  withURL:(NSString *)URLString
	 andImage:(UIImage *)image
{
	NSString *serviceName	= [self getServiceName:serviceType];

	// Check if service is not available
	if (![self isServiceTypeAvailable:serviceType])
	{
		// Invoke handler
		[self onFinishingSocialShare:serviceType
						  withResult:SLComposeViewControllerResultCancelled];
		return;
	}

	// Share
	SLComposeViewController *shareVC = [SLComposeViewController composeViewControllerForServiceType:serviceName];

	if (message)
		[shareVC setInitialText:message];

	if (URLString)
		[shareVC addURL:[NSURL URLWithString:URLString]];

	if (image)
		[shareVC addImage:image];

	[shareVC setCompletionHandler:^(SLComposeViewControllerResult result){

		// Invoke handler
		[self onFinishingSocialShare:serviceType
						  withResult:result];
	}];

	// Present view
	[UnityGetGLViewController() presentViewController:shareVC
											 animated:YES
										   completion:nil];
}

- (void)onFinishingSocialShare:(SocialShareServiceType)serviceType withResult:(SLComposeViewControllerResult)result
{
	// Notify unity
	NSMutableDictionary *dataDict	= [NSMutableDictionary dictionary];
	[dataDict setObject:[NSNumber numberWithInt:result] forKey:@"result"];
	[dataDict setObject:[NSNumber numberWithInt:serviceType] forKey:@"service-type"];

	NotifyEventListener(kSocialShareFinished, ToJsonCString(dataDict));
}

#pragma mark - Temporary methods

- (BOOL)checkWhetherServiceIsAvailable:(SocialShareServiceType)serviceType
{
    switch (serviceType)
    {
        case SocialShareServiceTypeTwitter:
            return [[UIApplication sharedApplication] canOpenURL:[NSURL URLWithString:kTwitterURLScheme]];
            break;

        case SocialShareServiceTypeFacebook:
            return [[UIApplication sharedApplication] canOpenURL:[NSURL URLWithString:kFacebookURLScheme]];
            break;

        default:
            break;
    }
    return false;
}


@end

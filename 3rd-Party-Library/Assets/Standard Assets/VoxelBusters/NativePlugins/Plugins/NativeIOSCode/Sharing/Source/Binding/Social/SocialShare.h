//
//  SocialShare.h
//  Cross Platform Native Plugins
//
//  Created by Ashwin kumar on 09/09/15.
//  Copyright (c) 2015 Voxel Busters Interactive LLP. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <Social/Social.h>

enum SocialShareServiceType
{
	SocialShareServiceTypeFacebook	= 0,
	SocialShareServiceTypeTwitter
};
typedef enum SocialShareServiceType SocialShareServiceType;

@interface SocialShare : NSObject

// Methods
- (BOOL)isServiceTypeAvailable:(SocialShareServiceType)serviceType;
- (void)share:(SocialShareServiceType)serviceType
  withMessage:(NSString *)message
	  withURL:(NSString *)URLString
	 andImage:(UIImage *)image;

@end

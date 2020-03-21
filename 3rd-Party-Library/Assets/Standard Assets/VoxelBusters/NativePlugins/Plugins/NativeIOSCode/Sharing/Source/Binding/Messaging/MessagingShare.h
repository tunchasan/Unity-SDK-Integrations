//
//  MessagingShare.h
//  Cross Platform Native Plugins
//
//  Created by Ashwin kumar on 13/01/15.
//  Copyright (c) 2015 Voxel Busters Interactive LLP. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <MessageUI/MessageUI.h>

@interface MessagingShare : NSObject <MFMessageComposeViewControllerDelegate>

// Related to sharing
- (BOOL)isMessagingAvailable;
- (void)sendMessageWithBody:(NSString *)body
			   toRecipients:(NSArray *)recipients;

@end

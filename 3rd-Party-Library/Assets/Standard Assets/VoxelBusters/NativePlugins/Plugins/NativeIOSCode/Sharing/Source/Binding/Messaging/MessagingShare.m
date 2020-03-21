//
//  MessagingShare.m
//  Cross Platform Native Plugins
//
//  Created by Ashwin kumar on 13/01/15.
//  Copyright (c) 2015 Voxel Busters Interactive LLP. All rights reserved.
//

#import "MessagingShare.h"

@implementation MessagingShare

#define kMessageShareFinished 	"MessagingShareFinished"

#pragma mark - Sharing

- (BOOL)isMessagingAvailable
{
    bool canSend	= [MFMessageComposeViewController canSendText];
	NSLog(@"[MessagingShare] can send message: %d", canSend);
	
    return canSend;
}

- (void)sendMessageWithBody:(NSString *)body
			   toRecipients:(NSArray *)recipients
{
	if (![self isMessagingAvailable])
	{
		[self onFinishingMessageShare:MessageComposeResultFailed];
		return;
	}
	
    // Create controller
	MFMessageComposeViewController *controller 	= [[[MFMessageComposeViewController alloc] init] autorelease];
	
	// Set properties
	controller.body                     		= body;
	controller.recipients              		 	= recipients;
	controller.messageComposeDelegate   		= self;
	
	// Present
	[UnityGetGLViewController() presentViewController:controller
											 animated:YES
										   completion:nil];
}

- (void)onFinishingMessageShare:(MessageComposeResult)result
{
	// Notify Unity
	NSString *resultStr	= [NSString stringWithFormat:@"%d", result];
	
	NotifyEventListener(kMessageShareFinished, [resultStr UTF8String]);
}

#pragma mark - Delegate

- (void)messageComposeViewController:(MFMessageComposeViewController *)controller
				 didFinishWithResult:(MessageComposeResult)result
{
	NSLog(@"[MessagingShare] did finish with result: %d", result);
	
	// Invoke handler
	[self onFinishingMessageShare:result];

	// Dismiss
	[UnityGetGLViewController() dismissViewControllerAnimated:YES
												   completion:nil];
}

@end